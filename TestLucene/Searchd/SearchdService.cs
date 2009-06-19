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
using ISUtils.Searcher;
using ISUtils.Common;

namespace Searchd
{
    partial class SearchdService : ServiceBase
    {
        private SearchMaker searcher;
        private TcpListener listener;
        private DateTime start;

        public SearchdService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // TODO: 在此处添加代码以启动服务。
            EventLog.WriteEntry("Searchd Start....");
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
                EventLog.WriteEntry(System.AppDomain.CurrentDomain.BaseDirectory);
            }
            try
            {
                searcher = new SearchMaker(configfile);
            }
            catch (Exception ex)
            {
                WriteToLog(string.Format("Exception for open config file {0},{1}", configfile, ex.ToString()));
                EventLog.WriteEntry(string.Format("Exception for open config file {0},{1}", configfile, ex.ToString()));
                throw;
            }
            //Console.WriteLine("START...");
            //Console.ReadKey();
            this.timer.Enabled = true;
            //timeStart = DateTime.Now;
            WriteToLog("Searchd Start...");
            int port=3322;
            if (searcher != null)
                port=searcher.GetNetworkPort();
            WriteToLog(string.Format("port={0}",port));
            EventLog.WriteEntry(string.Format("port={0}", port));
            //IPAddress ipaddr=new IPAddress(
            IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
            listener = new TcpListener(ipEntry.AddressList[0],port);
            listener.Start();
            start = DateTime.Now;
        }

        protected override void OnStop()
        {
            // TODO: 在此处添加代码以执行停止服务所需的关闭操作。
            listener.Stop();
            this.timer.Enabled = false;
            GC.Collect();
        }
        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            TcpClient client = listener.AcceptTcpClient();
            NetworkStream ns = client.GetStream();
            if (ns.CanRead && ns.CanWrite)
            {
                try
                {
                    WriteToLog("Try to execute search");
                    Message msg = searcher.ExecuteSearch(ref ns, System.AppDomain.CurrentDomain.BaseDirectory + @"\log\search_log.txt");
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
                    EventLog.WriteEntry("Failed to Search. Reason: " + ex.Message);
                    //ns.Close();
                }
            }
            WriteToLog("GC Begin Collect");
            //TimeSpan span = DateTime.Now - start;
            //long hours = (long)span.TotalHours;
            //if ( hours>=1 && DateTime.Now.Minute == start.Minute && DateTime.Now.Second == start.Second )
            GC.Collect();
            WriteToLog("After GC Collect");
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
                throw;
            }
        }
    }
}
