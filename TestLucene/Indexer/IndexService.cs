using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.IO;
using ISUtils.Indexer;
using ISUtils.Common;

namespace Indexer
{
    partial class IndexService : ServiceBase
    {
        private IndexMaker maker=null;
        private DateTime timeStart;
        private static bool busy = false;
        public IndexService()
        {
            InitializeComponent();
            // TODO: 在 InitComponent 调用后添加任何初始化
        }

        protected override void OnStart(string[] args)
        {
            // TODO: 在此处添加代码以启动服务。
            EventLog.WriteEntry("Indexer Start....");
            EventLog.WriteEntry(System.AppDomain.CurrentDomain.BaseDirectory);
            string[] imagePathArgs = Environment.GetCommandLineArgs();
            string configfile = System.AppDomain.CurrentDomain.BaseDirectory + @"\config.conf";
            if (imagePathArgs.Length >= 2)
            {
                configfile = imagePathArgs[1];
            }
            WriteToLog("ConfigFile:\t" + configfile);
            try
            {
                maker = new IndexMaker(configfile);
            }
            catch (Exception ex)
            {
                WriteToLog(string.Format("Exception for open config file {0},{1}", configfile, ex.ToString()));
                EventLog.WriteEntry(string.Format("Exception for open config file {0},{1}", configfile, ex.ToString()));
            }
            this.timer.Enabled = true;
            timeStart = DateTime.Now;
            WriteToLog("Indexer Start...");
        }
        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (maker == null)
            {
                string[] imagePathArgs = Environment.GetCommandLineArgs();
                string configfile = System.AppDomain.CurrentDomain.BaseDirectory + @"\config.conf";
                if (imagePathArgs.Length >= 2)
                {
                    configfile = imagePathArgs[1];
                }
                try
                {
                    maker = new IndexMaker(configfile);
                }
                catch (Exception ex)
                {
                    WriteToLog(string.Format("Exception for open config file {0},{1}",configfile, ex.ToString()));
                    EventLog.WriteEntry(string.Format("Exception for open config file {0},{1}", configfile, ex.ToString()));
                }
            }
            DateTime now = DateTime.Now;
            TimeSpan span = DateTime.Now - timeStart;
            //WriteToLog("开始写主索引！");
            if (!busy)
            {
                busy = true;
                if (maker.CanIndex(span, IndexTypeEnum.Ordinary))
                {
                    try
                    {
                        Message msg = maker.ExecuteIndexer(span, IndexTypeEnum.Ordinary);
                        if (msg.Success)
                            WriteToLog(msg.ToString());
                        else
                            if (msg.ExceptionOccur)
                                WriteToLog(msg.ToString());
                    }
                    catch (Exception exp)
                    {
                        WriteToLog("Exception for execute ordinary index.Reason:" + exp.Message);
                        EventLog.WriteEntry("Exception for execute ordinary index.Reason:" + exp.Message);
                    }
                }
                if (maker.CanIndex(span, IndexTypeEnum.Increment))
                {
                    try
                    {
                        Message msg = maker.ExecuteIndexer(span, IndexTypeEnum.Increment);
                        if (msg.Success)
                            WriteToLog(msg.ToString());
                        else
                            if (msg.ExceptionOccur)
                                WriteToLog(msg.ToString());
                    }
                    catch (Exception exp)
                    {
                        WriteToLog("Exception for execute increment index.Reason:" + exp.Message);
                        EventLog.WriteEntry("Exception for execute increment index.Reason:" + exp.Message);
                    }
                }
                busy = false;
            }
        }
        public static long GetSeconds(TimeSpan span)
        {
            return (long)span.TotalSeconds;
        }
        public static string FormatTimeSpan(TimeSpan span)
        {
            return span.Days.ToString("00") + "." +
                   span.Hours.ToString("00") + ":" +
                   span.Minutes.ToString("00") + ":" +
                   span.Seconds.ToString("00") + "." +
                   span.Milliseconds.ToString("000");
        }
        public static void WriteToLog(string detail)
        {
            try
            {
                FileStream fs = new FileStream(System.AppDomain.CurrentDomain.BaseDirectory + @"\log\log.txt", FileMode.Append);
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
        protected override void OnStop()
        {
            // TODO: 在此处添加代码以执行停止服务所需的关闭操作。
            EventLog.WriteEntry("Indexer Stop....");
            this.timer.Enabled = false;
            WriteToLog("Indexer Stop...");
        }
        protected override void OnContinue()
        {
            this.timer.Enabled = true;
            WriteToLog("Indexer continue...");
            base.OnContinue();
        }
        protected override void OnPause()
        {
            this.timer.Enabled = false;
            WriteToLog("Indexer pause...");
            base.OnPause();
        }
        protected override void OnShutdown()
        {
            WriteToLog("Indexer Stop...");
            base.OnShutdown();
        }
    }
}
