using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Threading;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using ISUtils.Searcher;
using ISUtils.Common;
using Lucene.Net.Search;
using Lucene.Net.Documents;

namespace Searchd
{
    partial class SearchdService : ServiceBase
    {
        private static SearchMaker searcher;
        private static TcpListener listener;
        private static Hashtable searchResults;
        private static Hashtable searchQueries;
        private static Hashtable searchStatistics;
        private Thread MainThread;
        // Thread signal.
        public static ManualResetEvent tcpClientConnected =new ManualResetEvent(false);
        // Accept one client connection asynchronously.
        public static void DoBeginAcceptTcpClient(TcpListener listener)
        {
            // Set the event to nonsignaled state.
            tcpClientConnected.Reset();

            // Start to listen for connections from a client.
            //Console.WriteLine("Waiting for a connection...");
            //EventLog.WriteEntry("Waiting for a connection...");
            WriteToLog("Waiting for a connection...");

            // Accept the connection. 
            // BeginAcceptSocket() creates the accepted socket.
            listener.BeginAcceptTcpClient(new AsyncCallback(DoAcceptTcpClientCallback),listener);

            // Wait until a connection is made and processed before 
            // continuing.
            tcpClientConnected.WaitOne();
        }
        // Process the client connection.
        public static void DoAcceptTcpClientCallback(IAsyncResult ar)
        {
            // Get the listener that handles the client request.
            TcpListener listener = (TcpListener)ar.AsyncState;

            // End the operation and display the received data on 
            // the console.
            TcpClient client = listener.EndAcceptTcpClient(ar);

            // Process the connection here. (Add the client to a
            // server table, read data, etc.)
            //Console.WriteLine("Client connected completed");
            DateTime now = DateTime.Now;
            NetworkStream ns = client.GetStream();
            if (ns.CanRead && ns.CanWrite)
            {
                SearchInfo searchInfo = GetSearchInfo(ns);
                WriteToLog(searchInfo.ToString());
                if (searchResults == null)
                    searchResults = new Hashtable(new QueryInfoComparer());
                if (searchQueries == null)
                    searchQueries = new Hashtable(new QueryInfoComparer());
                if (searchStatistics == null)
                    searchStatistics = new Hashtable(new QueryInfoComparer());
                if (searchResults.ContainsKey(searchInfo.Query))
                {
                    WriteToLog("该搜索已经存在！");
                    List<SearchRecord> recordList = (List<SearchRecord>)searchResults[searchInfo.Query];
                    Dictionary<string, List<int>> statistics = (Dictionary<string, List<int>>)searchStatistics[searchInfo.Query];
                    WriteToLog("Total Hits:" + recordList.Count.ToString()+"\tPageSize="+searchInfo.PageSize.ToString());
                    SearchResult result = new SearchResult();
                    result.Statistics = Convert(statistics);
                    result.PageNum = searchInfo.PageNum;
                    result.TotalPages = TotalPages(TotalCount(statistics,searchInfo.Filter), searchInfo.PageSize);
                    result.Records.AddRange(GetPage(recordList, statistics,searchInfo.Filter, searchInfo.PageSize, searchInfo.PageNum));
                    result.Query = (Query)searchQueries[searchInfo.Query];
                    WriteToLog(result.ToString());
                    SendResult(ns, result);
                    ns.Close();
                }
                else
                {
                    WriteToLog("该搜索首次进行！");
                    try
                    {
                        Query query;
                        Dictionary<string,List<int>> statistics;
                        List<SearchRecord> recordList = searcher.ExecuteFastSearch(searchInfo.Query, out query,out statistics,searchInfo.HighLight);
                        searchResults.Add(searchInfo.Query, recordList);
                        searchQueries.Add(searchInfo.Query, query);
                        searchStatistics.Add(searchInfo.Query, statistics);
                        WriteToLog("Total Hits:" + recordList.Count.ToString() + "\tPageSize=" + searchInfo.PageSize.ToString());
                        SearchResult result = new SearchResult();
                        result.Statistics = Convert(statistics);
                        result.PageNum = 1;
                        WriteToLog("Filter:\t" + searchInfo.Filter);
                        result.TotalPages = TotalPages(TotalCount(statistics, searchInfo.Filter), searchInfo.PageSize);
                        result.Records.AddRange(GetPage(recordList,statistics,searchInfo.Filter, searchInfo.PageSize, 1));
                        result.Query = query;
                        WriteToLog(result.ToString());
                        SendResult(ns, result);
                        ns.Close();
                    }
                    catch (Exception ex)
                    {
                        WriteToLog(ex.StackTrace.ToString());
                        WriteToLog("Failed to Search. Reason: " + ex.Message);
                        //EventLog.WriteEntry("Failed to Search. Reason: " + ex.Message);
                        //ns.Close();
                    }
                }
            }
            TimeSpan span = DateTime.Now - now;
            WriteToLog("花费时间："+span.TotalMilliseconds.ToString());
            tcpClientConnected.Set();
            DoBeginAcceptTcpClient(listener);
        }
        public SearchdService()
        {
            InitializeComponent();
            MainThread = new Thread(new ThreadStart(ThreadFunc));
            MainThread.Priority = ThreadPriority.Normal;
        }
        protected override void OnStart(string[] args)
        {
            // TODO: 在此处添加代码以启动服务。
            EventLog.WriteEntry("搜索服务开始....");
            MainThread.Start();
        }
        protected override void OnStop()
        {
            // TODO: 在此处添加代码以执行停止服务所需的关闭操作。
            WriteToLog("In OnStop...");
            MainThread.Abort();
            listener.Stop();
            WriteToLog("listener.Stop....");
            GC.Collect();
            WriteToLog("GC.Collect()...");
        }
        public static void ThreadFunc()
        {
            string[] imagePathArgs = Environment.GetCommandLineArgs();
            string configfile = System.AppDomain.CurrentDomain.BaseDirectory + @"\config.xml";
            if (imagePathArgs.Length >= 2)
            {
                configfile = imagePathArgs[1];
            }
            try
            {
                WriteToLog("ConfigFile:\t" + configfile);
            }
            catch (Exception e)
            {
                //EventLog.WriteEntry(System.AppDomain.CurrentDomain.BaseDirectory + "\n" + e.StackTrace.ToString());
            }
            try
            {
                searcher = new SearchMaker(configfile);
            }
            catch (Exception ex)
            {
                WriteToLog(string.Format("Exception for open config file {0},{1}", configfile, ex.ToString()));
                //EventLog.WriteEntry(string.Format("Exception for open config file {0},{1}", configfile, ex.ToString()));
                throw;
            }
            //Console.WriteLine("START...");
            //Console.ReadKey();
            //this.timer.Enabled = true;
            //timeStart = DateTime.Now;
            WriteToLog("Searchd Start...");
            int port = 3322;
            if (searcher != null)
                port = searcher.GetNetworkPort();
            WriteToLog(string.Format("port={0}", port));
            //EventLog.WriteEntry(string.Format("port={0}", port));
            //IPAddress ipaddr=new IPAddress(
            IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
            listener = new TcpListener(ipEntry.AddressList[0], port);
            listener.Start();
            DoBeginAcceptTcpClient(listener);
        }
        public static void WriteToLog(string detail)
        {
            try
            {
                FileStream fs = new FileStream(System.AppDomain.CurrentDomain.BaseDirectory + @"\log\search_log.txt", FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                string str = "[" + ISUtils.SupportClass.Time.GetDateTime()+ "]\t" + detail;
                sw.WriteLine(str);
                sw.Flush();
                sw.Close();
                fs.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static SearchInfo GetSearchInfo(NetworkStream ns)
        {
            SearchInfo info = null;
            BinaryFormatter formater = new BinaryFormatter();
            try
            {
                info = (SearchInfo)formater.Deserialize(ns);
            }
            catch (SerializationException se)
            {
                WriteToLog(se.StackTrace.ToString());
            }
            return info;
        }
        public static void SendResult(NetworkStream ns, SearchResult result)
        {
            BinaryFormatter formater = new BinaryFormatter();
            try
            {
                formater.Serialize(ns, result);
            }
            catch (SerializationException e)
            {
                WriteToLog(e.StackTrace.ToString());
            }
        }
        public static int TotalPages(int totalNum, int pageSize)
        {
            if (pageSize <= 0)
                return 1;
            if (totalNum % pageSize == 0)
                return totalNum / pageSize;
            else
                return totalNum / pageSize + 1;
        }
        public static Dictionary<string, int> Convert(Dictionary<string, List<int>> statistics)
        { 
            Dictionary<string,int> result=new Dictionary<string,int>();
            foreach (string key in statistics.Keys)
            {
                result.Add(key, statistics[key].Count);
            }
            return result;
        }
        public static int TotalCount(Dictionary<string,List<int>> statistics,string filter)
        {
            int count = 0;
            if (string.IsNullOrEmpty(filter))
            {
                foreach (string key in statistics.Keys)
                {
                    count+=statistics[key].Count;
                }
            }
            else
            {
                string[] keys = filter.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (string key in keys)
                {
                    if (statistics.ContainsKey(key))
                        count += statistics[key].Count;
                }
            }
            return count;
        }
        public static List<int> GetPositions(Dictionary<string, List<int>> statistics, string filter)
        {
            List<int> posList = new List<int>();
            if (string.IsNullOrEmpty(filter))
            {
                foreach (string key in statistics.Keys)
                {
                    posList.AddRange(statistics[key]);
                }
            }
            else
            {
                string[] keys = filter.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (string key in keys)
                {
                    if (statistics.ContainsKey(key))
                        posList.AddRange(statistics[key]);
                }
            }
            return posList;
        }
        public static List<SearchRecord> GetPage(List<SearchRecord> recordList, List<int> posList, int pageSize, int pageNum)
        {
            if (pageSize > 0)
            {
                List<SearchRecord> resultList = new List<SearchRecord>();
                if (pageNum <= 0)
                    pageNum = 1;
                for (int i = (pageNum - 1) * pageSize; i < pageNum * pageSize && i < posList.Count; i++)
                    resultList.Add(recordList[posList[i]]);
                return resultList;
            }
            else
            {
                return recordList;
            }
        }
        public static List<SearchRecord> GetPage(List<SearchRecord> recordList, Dictionary<string, List<int>> statistics, string filter, int pageSize, int pageNum)
        {
            List<int> posList = GetPositions(statistics, filter);
            posList.Sort(delegate(int x, int y) { return x - y; });
            return GetPage(recordList, posList, pageSize, pageNum);
        }
        //public static List<SearchRecord> GetPage(List<SearchRecord> recordList,int pageSize, int pageNum)
        //{
        //    if (pageSize > 0)
        //    {
        //        List<SearchRecord> resultList = new List<SearchRecord>();
        //        if (pageNum <= 0)
        //            pageNum = 1;
        //        for (int i = (pageNum - 1) * pageSize; i < pageNum * pageSize && i < recordList.Count; i++)
        //            resultList.Add(recordList[i]);
        //        return resultList;
        //    }
        //    else
        //    {
        //        return recordList;
        //    }
        //}
    }
}
