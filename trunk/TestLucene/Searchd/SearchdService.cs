using System;
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
using ISUtils.Searcher;
using ISUtils.Common;

namespace Searchd
{
    partial class SearchdService : ServiceBase
    {
        private static SearchMaker searcher;
        private static TcpListener listener;
        private static DateTime start;
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
                try
                {
                    WriteToLog("Try to execute search");
                    Message msg = searcher.ExecuteSearch(ref ns, System.AppDomain.CurrentDomain.BaseDirectory + @"\log\search_log.txt", true);
                    WriteToLog("After ExecuteSearch");
                    if (msg.Success)
                        WriteToLog(msg.ToString());
                    WriteToLog("Write Success Message");
                    if (!msg.Success && msg.ExceptionOccur)
                        WriteToLog(msg.ToString());
                }
                catch (Exception ex)
                {
                    WriteToLog(ex.StackTrace.ToString());
                    WriteToLog("Failed to Search. Reason: " + ex.Message);
                    //EventLog.WriteEntry("Failed to Search. Reason: " + ex.Message);
                    //ns.Close();
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
                string str = "[" + DateTime.Now.ToString() + "]\t" + detail;
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
    }
}
