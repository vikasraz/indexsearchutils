﻿using System;
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
        private static DateTime start;
        private static Dictionary<QueryInfo, List<Document>> searchResultDict;
        private static Dictionary<QueryInfo, Query> searchQueryDict;
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
            NetworkStream ns = client.GetStream();
            if (ns.CanRead && ns.CanWrite)
            {
                SearchInfo searchInfo = GetSearchInfo(ns);
                WriteToLog(searchInfo.ToString());
                if (searchResultDict == null)
                    searchResultDict = new Dictionary<QueryInfo, List<Document>>();
                if (searchQueryDict == null)
                    searchQueryDict = new Dictionary<QueryInfo, Query>();
                if (searchResultDict.ContainsKey(searchInfo.Query))
                {
                    List<Document> docList=searchResultDict[searchInfo.Query];
                    SearchResult result = new SearchResult();
                    result.PageNum = searchInfo.PageNum;
                    result.TotalPages = TotalPages(docList.Count, searchInfo.PageSize);
                    result.Docs.AddRange(GetPage(docList,searchInfo.PageSize,searchInfo.PageNum));
                    result.Query = searchQueryDict[searchInfo.Query];
                    SendResult(ns, result);
                    ns.Close();
                }
                else
                {
                    try
                    {
                        Query query;
                        WriteToLog("Try to execute search");
                        List<Document> docList = searcher.ExecuteFastSearch(searchInfo.Query, out query);
                        WriteToLog("After ExecuteSearch");
                        searchResultDict.Add(searchInfo.Query, docList);
                        searchQueryDict.Add(searchInfo.Query, query);

                        SearchResult result = new SearchResult();
                        result.PageNum = 1;
                        result.TotalPages = TotalPages(docList.Count, searchInfo.PageSize);
                        result.Docs.AddRange(GetPage(docList, searchInfo.PageSize, 1));
                        result.Query = searchQueryDict[searchInfo.Query];
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
            WriteToLog("GC Begin Collect");
            //TimeSpan span = DateTime.Now - start;
            //long hours = (long)span.TotalHours;
            //if ( hours>=1 && DateTime.Now.Minute == start.Minute && DateTime.Now.Second == start.Second )
            GC.Collect();
            WriteToLog("After GC Collect");
            //EventLog.WriteEntry("Client connected completed");
            // Signal the calling thread to continue.
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
            EventLog.WriteEntry("Searchd Start....");
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
            string configfile = System.AppDomain.CurrentDomain.BaseDirectory + @"\config.conf";
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
            start = DateTime.Now;
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
            if (totalNum % pageSize == 0)
                return totalNum / pageSize;
            else
                return totalNum / pageSize + 1;
        }
        public static List<Document> GetPage(List<Document> docList, int pageSize, int pageNum)
        {
            List<Document> resultList = new List<Document>();
            for (int i = (pageNum - 1) * pageSize; i < pageNum * pageSize && i < docList.Count; i++)
                resultList.Add(docList[i]);
            return resultList;
        }

    }
}
