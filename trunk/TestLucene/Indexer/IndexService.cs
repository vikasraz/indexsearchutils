using System;
using System.Collections;
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
            WriteToLog("InitializeComponent成功");
        }

        protected override void OnStart(string[] args)
        {
            // TODO: 在此处添加代码以启动服务。
            EventLog.WriteEntry("索引服务开始运行....");
            WriteToLog("尝试运行索引服务...");
            string[] imagePathArgs = Environment.GetCommandLineArgs();
            string configfile = System.AppDomain.CurrentDomain.BaseDirectory + @"\config.xml";
            if (imagePathArgs.Length >= 2)
            {
                configfile = imagePathArgs[1];
            }
            WriteToLog("配置文件:\t" + configfile);
            try
            {
                maker = new IndexMaker(configfile);
            }
            catch (Exception ex)
            {
                WriteToLog(string.Format("Exception for open config file {0},{1}", configfile, ex.ToString()));
            }
            this.timer.Enabled = true;
            timeStart = DateTime.Now;
            WriteToLog("索引服务开始...");
        }
        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (maker == null)
            {
                string[] imagePathArgs = Environment.GetCommandLineArgs();
                string configfile = System.AppDomain.CurrentDomain.BaseDirectory + @"\config.xml";
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
                    WriteToLog(string.Format("打开文件 {0}时发生异常：{1}",configfile, ex.ToString()));
                }
            }
            DateTime now = DateTime.Now;
            TimeSpan span = DateTime.Now - timeStart;
            if (!busy)
            {
                busy = true;
                if (maker.CanIndex(span, IndexTypeEnum.Ordinary) || maker.CanIndex(span, IndexTypeEnum.Increment))
                {
                    try
                    {
                        WriteToLog("停止搜索服务....");
                        StopSystemService("Searchd");
                        WriteToLog("搜索服务停止成功！");
                    }
                    catch (Exception ssse)
                    {
                        WriteToLog("搜索服务无法停止.原因是:" + ssse.Message);
                    }
                }
                if (maker.CanIndex(span, IndexTypeEnum.Ordinary))
                {
                    WriteToLog("尝试开始写主索引......");
                    try
                    {
                        DataBaseLibrary.SearchUpdateManage dblSum = new DataBaseLibrary.SearchUpdateManage();
                        Message msg = maker.ExecuteBoostIndexer(dblSum, span, IndexTypeEnum.Ordinary);
                        if (msg.Success)
                            WriteToLog(msg.ToString());
                        else
                            if (msg.ExceptionOccur)
                                WriteToLog(msg.ToString());
                    }
                    catch (Exception exp)
                    {
                        WriteToLog("写主索引出错.原因是:" + exp.Message);
                        EventLog.WriteEntry("写主索引出错.原因是:" + exp.Message);
                    }
                    try
                    {
                        maker.IndexFile(true);
                    }
                    catch (Exception fe)
                    {
                        WriteToLog("文件索引出错。原因是:" + fe.StackTrace.ToString());
                    }
                    WriteToLog("主索引完成！");
                }
                if (maker.CanIndex(span, IndexTypeEnum.Increment))
                {
                    try
                    {
                        WriteToLog("开始增量索引......");
                        DataBaseLibrary.SearchUpdateManage dblSum = new DataBaseLibrary.SearchUpdateManage();
                        Message msg = maker.ExecuteBoostIndexer(dblSum, span, IndexTypeEnum.Increment);
                        if (msg.Success)
                            WriteToLog(msg.ToString());
                        else
                            if (msg.ExceptionOccur)
                                WriteToLog(msg.ToString());
                        WriteToLog("完成增量索引！");
                    }
                    catch (Exception exp)
                    {
                        WriteToLog("写增量索引时出错.原因是:" + exp.Message);
                    }
                }
                if (maker.CanIndex(span, IndexTypeEnum.Ordinary) || maker.CanIndex(span, IndexTypeEnum.Increment))
                {
                    try
                    {
                        WriteToLog("开启搜索服务......");
                        StartSystemService("Searchd");
                        WriteToLog("搜索服务开始!");
                    }
                    catch (Exception ste)
                    {
                        WriteToLog("搜索服务无法开始.原因是:" + ste.Message);
                    }
                }
                busy = false;
            }
            else
            {
                WriteToLog("正在建立索引，请等待.......");
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
                FileInfo info = new FileInfo(System.AppDomain.CurrentDomain.BaseDirectory + @"\log\log.txt");
                if (info.Length > 1024*100)
                    info.Delete();
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
            EventLog.WriteEntry("索引服务停止....");
            this.timer.Enabled = false;
            WriteToLog("索引服务停止...");
        }
        protected override void OnContinue()
        {
            this.timer.Enabled = true;
            WriteToLog("索引服务继续运行...");
            base.OnContinue();
        }
        protected override void OnPause()
        {
            this.timer.Enabled = false;
            WriteToLog("索引服务暂停...");
            base.OnPause();
        }
        protected override void OnShutdown()
        {
            WriteToLog("索引服务停止...");
            base.OnShutdown();
        }
        #region Service Function
        private bool SystemServiceExists(string service)
        {
            System.ServiceProcess.ServiceController[] services;
            services = System.ServiceProcess.ServiceController.GetServices();
            foreach (System.ServiceProcess.ServiceController sc in services)
            {
                if (sc.ServiceName.ToUpper().CompareTo(service.ToUpper()) == 0)
                {
                    return true;
                }
            }
            return false;
        }
        private string GetSystemServiceStatus(string service)
        {
            try
            {
                //foreach (System.ServiceProcess.ServiceController sc in System.ServiceProcess.ServiceController.GetServices())
                //{
                //    if (sc.ServiceName.ToUpper().CompareTo(service.ToUpper()) == 0)
                //    {
                //        return sc.Status.ToString();
                //    }
                //}
                System.ServiceProcess.ServiceController sc = new System.ServiceProcess.ServiceController();
                //sc.MachineName = "localhost";
                sc.ServiceName = service;
                sc.Refresh();
                switch (sc.Status)
                {
                    case System.ServiceProcess.ServiceControllerStatus.StartPending:
                        return "服务正在启动！";
                    case System.ServiceProcess.ServiceControllerStatus.ContinuePending:
                        return "服务即将继续！";
                    case System.ServiceProcess.ServiceControllerStatus.Paused:
                        return "服务已暂停！";
                    case System.ServiceProcess.ServiceControllerStatus.PausePending:
                        return "服务即将暂停！";
                    case System.ServiceProcess.ServiceControllerStatus.Running:
                        return "服务正在运行！";
                    case System.ServiceProcess.ServiceControllerStatus.Stopped:
                        return "服务未运行！";
                    case System.ServiceProcess.ServiceControllerStatus.StopPending:
                        return "服务正在停止！";
                    default:
                        return "服务状态未知！";
                }
            }
            catch (Exception e)
            {
                //ShowError(e.StackTrace.ToString());
                return "Error:" + e.Message;
            }
        }
        private bool StartSystemService(string service)
        {
            if (!SystemServiceExists(service))
                return false;
            try
            {
                System.ServiceProcess.ServiceController sc = new System.ServiceProcess.ServiceController();
                //sc.MachineName = "localhost";
                sc.ServiceName = service;
                if (sc.Status != System.ServiceProcess.ServiceControllerStatus.Running &&
                    sc.Status != System.ServiceProcess.ServiceControllerStatus.StartPending)
                {
                    sc.Start();
                    for (int i = 0; i < 60; i++)
                    {
                        sc.Refresh();
                        System.Threading.Thread.Sleep(1000);
                        if (sc.Status == System.ServiceProcess.ServiceControllerStatus.Running)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception )
            {
                return false;
            }
        }
        private bool StopSystemService(string service)
        {
            if (!SystemServiceExists(service))
                return false;
            try
            {
                System.ServiceProcess.ServiceController sc = new System.ServiceProcess.ServiceController();
                //sc.MachineName = "localhost";
                sc.ServiceName = service;
                if (sc.Status == System.ServiceProcess.ServiceControllerStatus.Running)
                {
                    sc.Stop();
                    for (int i = 0; i < 60; i++)
                    {
                        sc.Refresh();
                        System.Threading.Thread.Sleep(1000);
                        if (sc.Status == System.ServiceProcess.ServiceControllerStatus.Stopped)
                        {
                            return true;
                        }
                    }
                    return false;
                }
                return true;
            }
            catch (Exception )
            {
                return false;
            }
        }
        private bool InstallSystemServic(string service, string servicePath, string commandLineOptions)
        {
            if (SystemServiceExists(service))
                return true;
            if (!System.IO.File.Exists(servicePath))
                return false;
            try
            {
                System.Configuration.Install.TransactedInstaller tranInstaller = new System.Configuration.Install.TransactedInstaller();
                System.Configuration.Install.AssemblyInstaller assemInstaller = new System.Configuration.Install.AssemblyInstaller(servicePath, new string[] { commandLineOptions });
                tranInstaller.Installers.Add(assemInstaller);
                System.Configuration.Install.InstallContext installContext = new System.Configuration.Install.InstallContext("install.log", new string[] { commandLineOptions });
                tranInstaller.Context = installContext;
                tranInstaller.Install(new Hashtable());
                return true;
            }
            catch (Exception )
            {
                return false;
            }

        }
        private bool UninstallSystemServic(string service, string servicePath, string commandLineOptions)
        {
            if (!System.IO.File.Exists(servicePath))
                return false;
            if (SystemServiceExists(service))
                return false;
            try
            {
                System.Configuration.Install.TransactedInstaller tranInstaller = new System.Configuration.Install.TransactedInstaller();
                System.Configuration.Install.AssemblyInstaller assemInstaller = new System.Configuration.Install.AssemblyInstaller(servicePath, new string[] { commandLineOptions });
                tranInstaller.Installers.Add(assemInstaller);
                System.Configuration.Install.InstallContext installContext = new System.Configuration.Install.InstallContext("install.log", new string[] { commandLineOptions });
                tranInstaller.Context = installContext;
                tranInstaller.Uninstall(null);
                return true;
            }
            catch (Exception )
            {
                return false;
            }

        }
        #endregion
    }
}
