using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ISUtils.Common;
using ISUtils.Utils;
using ISUtils.Async;
using ISUtils.Database;

namespace IndexEditor
{
    public partial class frmEditor : Form
    {
        private DictionarySet dictSet;
        private IndexerSet indexerSet;
        private SearchSet searchSet;
        private List<IndexSet> indexList;
        private List<Source> sourceList;
        private IndexSet indexSet=new IndexSet();
        private Source source=new Source();
        private PanelShowType panelShow=PanelShowType.Source;
        private Status status = Status.Cancel;
        private bool init = false;
        private bool makeChange = false;

        public const string DictionaryFilter ="文本文件 (*.txt;*.dat;*.ini)|*.txt;*.dat;*.ini|" + 
                                            "所有文件(*.*)|*.*";
        public const string OutputFilter = "文本文件 (*.txt;*.log;*.conf)|*.txt;*.log;*.conf|" +
                                            "所有文件(*.*)|*.*";
        public string AppPath="";
        public string CommandLineOptions=" ";
        public frmEditor()
        {
            InitializeComponent();
            AppPath = System.AppDomain.CurrentDomain.BaseDirectory;
            CommandLineOptions = " " + AppPath + @"\config.conf";
        }
        private bool InitData(string filename)
        {
            try
            {
                Parser parser = new Parser(filename);
                searchSet = parser.GetSearchd();
                sourceList = parser.GetSourceList();
                indexList = parser.GetIndexList();
                dictSet = parser.GetDictionarySet();
                indexerSet = parser.GetIndexer();
                return true;
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine(string.Format("Exception for open file {0},{1}", filename, ex.ToString()));
#endif
                ShowError(string.Format("Exception for open file {0},{1}", filename, ex.ToString()));
                return false;
            }
        }
        private void Form_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                this.notifyIcon.Visible = true;
            }
        }
        private void notifyIcon_Click(object sender, MouseEventArgs e)
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
            this.notifyIcon.Visible = false;
        }
        private void ShowInformation(string msg)
        {
            string caption = this.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBoxIcon icon = MessageBoxIcon.Information;
            MessageBoxDefaultButton defbtn = MessageBoxDefaultButton.Button1;
            DialogResult result = MessageBox.Show(this, msg, caption, buttons,icon, defbtn);
        }
        private void ShowExclamation(string msg)
        {
            string caption = this.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBoxIcon icon = MessageBoxIcon.Exclamation;
            MessageBoxDefaultButton defbtn = MessageBoxDefaultButton.Button1;
            DialogResult result = MessageBox.Show(this, msg, caption, buttons, icon, defbtn);
        }
        private void ShowError(string msg)
        {
            string caption = this.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBoxIcon icon = MessageBoxIcon.Error;
            MessageBoxDefaultButton defbtn = MessageBoxDefaultButton.Button1;
            DialogResult result = MessageBox.Show(this, msg, caption, buttons, icon, defbtn);
        }
        private void ShowWarning(string msg)
        {
            string caption = this.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBoxIcon icon = MessageBoxIcon.Warning;
            MessageBoxDefaultButton defbtn = MessageBoxDefaultButton.Button1;
            DialogResult result = MessageBox.Show(this, msg, caption, buttons, icon, defbtn);
        }
        private bool ShowQuestion(string msg)
        {
            string caption = this.Text;
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            MessageBoxIcon icon = MessageBoxIcon.Question;
            MessageBoxDefaultButton defbtn = MessageBoxDefaultButton.Button1;
            DialogResult result = MessageBox.Show(this, msg, caption, buttons, icon, defbtn);
            return result == DialogResult.Yes;
        }

        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView.FocusedItem != null)
            {
                if (listView.SelectedItems != null && listView.SelectedItems.Count > 0)
                {
                    //message = listView.SelectedItems[0].Text+listView.SelectedItems[0].Index.ToString();
                    //string message ="no item select";
                    //string caption = "no server name specified";
                    //MessageBoxButtons buttons = MessageBoxButtons.OK;
                    //DialogResult result;
                    //// displays the messagebox.
                    //result = MessageBox.Show(this, message, caption, buttons,
                    //MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);//,MessageBoxOptions.);
                    //if (result == DialogResult.Yes)
                    //{
                    //    //do your action here.
                    //}
                    panelShow = (PanelShowType)listView.SelectedItems[0].Index;
                    ShowPanel((PanelShowType)listView.SelectedItems[0].Index);
                }
            }
        }
        public enum PanelShowType
        {
            Source,
            IndexSet,
            IndexerSet,
            Searchd,
            Dictionary
        }
        public enum Status
        {
            Insert,
            Edit,
            Delete,
            Confirm,
            Cancel
        }
        private void ShowPanel(PanelShowType showType)
        {
            System.Windows.Forms.Panel[] panels = new Panel[] { panelSource,panelIndexSet,panelIndexerSet,panelSearchd,panelDictionary};
            foreach(Panel panel in panels)
            {
                panel.Visible = false;
            }
            panels[(int)showType].Visible = true;
        }
        private void EnablePanelSourceControls(bool enabled)
        {
            Control[] controls = new Control[] { textBoxSourceName,
                                                comboBoxSourceType,
                                                textBoxHostName,
                                                textBoxDataBase,
                                                textBoxUserName,
                                                textBoxPassword,
                                                textBoxQuery,
                                                textBoxFields};
            foreach (Control control in controls)
            {
                control.Enabled = enabled;
            }
        }
        private void ClearPanelSourceControls()
        {
            Control[] controls = new Control[] { textBoxSourceName,
                                                comboBoxSourceType, 
                                                textBoxHostName,
                                                textBoxDataBase,
                                                textBoxUserName,
                                                textBoxPassword, 
                                                textBoxQuery, 
                                                textBoxFields };
            foreach (Control control in controls)
            {
                control.Text="";
            }
        }
        private void InitPanelSourceControls(string sourcename)
        {
            foreach (Source s in sourceList)
            {
                if (s.SourceName.ToUpper().CompareTo(sourcename.ToUpper()) == 0)
                {
                    source = s;
                }
            }
            UpdatePanelSourceData(true);
        }
        private void UpdatePanelSourceData(bool update)
        {
            if (update)
            {
                if (source == null) return;
                textBoxSourceName.Text=source.SourceName;
                comboBoxSourceType.Text = ISUtils.Common.DbType.GetDbTypeStr(source.DBType);
                textBoxDataBase.Text=source.DataBase;
                textBoxFields.Text=source.GetFields();
                textBoxHostName.Text=source.HostName;
                textBoxPassword.Text=source.Password;
                textBoxQuery.Text=source.Query;
                textBoxUserName.Text=source.UserName;
            }
            else
            {
                if (source == null) source = new Source();
                source.SourceName = textBoxSourceName.Text;
                source.DBType = ISUtils.Common.DbType.GetDbType(comboBoxSourceType.Text);
                source.DataBase = textBoxDataBase.Text;
                source.Fields = textBoxFields.Text.Split(new char[]{'\t',',',' '});
                source.HostName = textBoxHostName.Text;
                source.Password = textBoxPassword.Text;
                source.Query = textBoxQuery.Text;
                source.UserName = textBoxUserName.Text;
            }
        }
        private void EnablePanelIndexControls(bool enabled)
        {
            Control[] controls = new Control[] { textBoxIndexName, 
                                                comboBoxIndexType, 
                                                comboBoxSouceSel,
                                                textBoxIndexPath,
                                                btnSetIndexPath };
            foreach (Control control in controls)
            {
                control.Enabled = enabled;
            }
        }
        private void ClearPanelIndexControls()
        {
            Control[] controls = new Control[] { textBoxIndexName, 
                                                comboBoxIndexType,
                                                comboBoxSouceSel,
                                                textBoxIndexPath };
            foreach (Control control in controls)
            {
                control.Text = "";
            }
        }
        private void InitPanelIndexControls(string indexname)
        {
            foreach (IndexSet i in indexList)
            {
                if (i.IndexName.ToUpper().CompareTo(indexname.ToUpper()) == 0)
                {
                    indexSet = i;
                }
            }
            UpdatePanelIndexData(true);
        }
        private void UpdatePanelIndexData(bool update)
        {
            if (update)
            {
                if (indexSet == null) return;
                textBoxIndexName.Text = indexSet.IndexName;
                comboBoxIndexType.Text = IndexType.GetIndexTypeStr(indexSet.Type);
                comboBoxSouceSel.Text = indexSet.SourceName;
                textBoxIndexPath.Text = indexSet.Path;
            }
            else
            {
                if (indexSet == null) indexSet = new IndexSet();
                indexSet.IndexName = textBoxIndexName.Text;
                indexSet.Type = IndexType.GetIndexType(comboBoxIndexType.Text);
                indexSet.SourceName = comboBoxSouceSel.Text;
                indexSet.Path = textBoxIndexPath.Text;
            }
        }
        private void EnablePanelIndexerControls(bool enabled)
        {
            Control[] controls = new Control[] { dateTimePickerMainReCreate, 
                                                numericUpDownMainCreateTimeSpan, 
                                                numericUpDownIncrTimeSpan, 
                                                numericUpDownRamBufferSize,
                                                numericUpDownMaxFieldLength,
                                                numericUpDownMaxBufferedDocs,
                                                numericUpDownMergeFactor};
            foreach (Control control in controls)
            {
                control.Enabled = enabled;
            }
        }
        //private void ClearPanelIndexerControls()
        //{
        //    Control[] controls = new Control[] { dateTimePickerMainReCreate, 
        //                                        numericUpDownMainCreateTimeSpan, 
        //                                        numericUpDownIncrTimeSpan, 
        //                                        numericUpDownRamBufferSize,
        //                                        numericUpDownMaxFieldLength,
        //                                        numericUpDownMaxBufferedDocs,
        //                                        numericUpDownMergeFactor};
        //    foreach (Control control in controls)
        //    {
        //        control.Text = "";
        //    }
        //}
        private void InitPanelIndexerControls()
        {
            UpdatePanelIndexerData(true);
        }
        private void UpdatePanelIndexerData(bool update)
        {
            if (update)
            {
                if (indexerSet == null) return;
                dateTimePickerMainReCreate.Text = indexerSet.MainIndexReCreateTime.ToShortTimeString();
                numericUpDownMainCreateTimeSpan.Value =indexerSet.MainIndexReCreateTimeSpan; 
                numericUpDownIncrTimeSpan.Value =indexerSet.IncrIndexReCreateTimeSpan; 
                numericUpDownRamBufferSize.Value =(int)indexerSet.RamBufferSize;
                numericUpDownMaxFieldLength.Value=indexerSet.MaxFieldLength;
                numericUpDownMaxBufferedDocs.Value =indexerSet.MaxBufferedDocs;
                numericUpDownMergeFactor.Value =indexerSet.MergeFactor;
            }
            else
            {
                if (indexerSet == null) indexerSet = new IndexerSet();
                indexerSet.IncrIndexReCreateTimeSpan = (int)numericUpDownIncrTimeSpan.Value;
                indexerSet.MainIndexReCreateTime=DateTime.Parse(dateTimePickerMainReCreate.Text);
                indexerSet.MainIndexReCreateTimeSpan = (int)numericUpDownMainCreateTimeSpan.Value;
                indexerSet.MaxBufferedDocs = (int)numericUpDownMaxBufferedDocs.Value;
                indexerSet.MaxFieldLength = (int)numericUpDownMaxFieldLength.Value;
                indexerSet.MergeFactor = (int)numericUpDownMergeFactor.Value;
                indexerSet.RamBufferSize=(double)numericUpDownRamBufferSize.Value;
            }
        }
        private void EnablePanelSearchControls(bool enabled)
        {
            Control[] controls = new Control[] { maskedTextBoxIpAddress, 
                                                numericUpDownPort, 
                                                numericUpDownTimeOut, 
                                                numericUpDownMaxMatches,
                                                numericUpDownMaxTransport,
                                                textBoxSearchdPath,
                                                textBoxQueryPath,
                                                btnSetSearchdPath,
                                                btnSetQueryPath};
            foreach (Control control in controls)
            {
                control.Enabled = enabled;
            }
        }
        private void InitPanelSearchControls()
        {
            UpdatePanelSearchData(true);
        }
        private void UpdatePanelSearchData(bool update)
        {
            if (update)
            {
                if (searchSet == null) return;
                maskedTextBoxIpAddress.Text =searchSet.Address; 
                numericUpDownPort.Value =searchSet.Port; 
                numericUpDownTimeOut.Value =searchSet.TimeOut; 
                numericUpDownMaxMatches.Value =searchSet.MaxMatches;
                numericUpDownMaxTransport.Value = searchSet.MaxTrans;
                textBoxSearchdPath.Text=searchSet.LogPath;
                textBoxQueryPath.Text = searchSet.QueryLogPath;
            }
            else
            {
                if (searchSet == null) searchSet = new SearchSet();
                searchSet.Address =maskedTextBoxIpAddress.Text;
                searchSet.LogPath=textBoxSearchdPath.Text;
                searchSet.MaxMatches=(int)numericUpDownMaxMatches.Value;
                searchSet.MaxTrans = (int)numericUpDownMaxTransport.Value;
                searchSet.Port = (int)numericUpDownPort.Value;
                searchSet.QueryLogPath=textBoxQueryPath.Text;
                searchSet.TimeOut = (int)numericUpDownTimeOut.Value;
            }
        }
        private void EnablePanelDictControls(bool enabled)
        {
            Control[] controls = new Control[] { textBoxBasePath, 
                                                textBoxFilterPath, 
                                                textBoxNamePath, 
                                                textBoxNumberPath,
                                                listBoxCustomPaths,
                                                btnSetBasePath,
                                                btnSetFilterPath,
                                                btnSetNamePath,
                                                btnSetNumberPath,
                                                btnSetCustomPaths};
            foreach (Control control in controls)
            {
                control.Enabled = enabled;
            }
        }
        private void InitPanelDictControls()
        { 
            UpdatePanelDictData(true);
        }
        private void UpdatePanelDictData(bool update)
        {
            if (update)
            {
                if (dictSet == null) return;
                textBoxBasePath.Text =dictSet.BasePath;
                textBoxFilterPath.Text =dictSet.FilterPath; 
                textBoxNamePath.Text =dictSet.NamePath;
                textBoxNumberPath.Text =dictSet.NumberPath;
                listBoxCustomPaths.Items.AddRange(dictSet.CustomPaths);
            }
            else
            {
                if (dictSet == null) return;
                dictSet.BasePath=textBoxBasePath.Text;
                dictSet.FilterPath=textBoxFilterPath.Text;
                dictSet.NamePath=textBoxNamePath.Text;
                dictSet.NumberPath=textBoxNumberPath.Text;
                object [] objs=new object[listBoxCustomPaths.Items.Count];
                string[] paths = new string[listBoxCustomPaths.Items.Count];
                listBoxCustomPaths.Items.CopyTo(objs,0);
                objs.CopyTo(paths, 0);
                dictSet.CustomPaths=paths;
            }
        }
        private string GetFileSavePath(string filter)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = filter;
            saveDialog.FilterIndex = 1;
            saveDialog.Title = this.Text;
            string filename="";
            if (saveDialog.ShowDialog() == DialogResult.OK)
                filename = saveDialog.FileName;
            return filename;
        }
        private string GetFileOpenPath(string filter)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = filter;
            openDialog.FilterIndex = 1;
            openDialog.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            openDialog.Title = this.Text;
            openDialog.CheckFileExists = true;
            string filename = "";
            if (openDialog.ShowDialog() == DialogResult.OK)
                filename = openDialog.FileName;
            return filename;
        }
        private string[] GetFileOpenPaths(string filter)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = filter;
            openDialog.FilterIndex = 1;
            openDialog.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            openDialog.Title = this.Text;
            openDialog.CheckFileExists = true;
            openDialog.Multiselect = true;
            string[] filenames =null;
            if (openDialog.ShowDialog() == DialogResult.OK)
                filenames = openDialog.FileNames;
            return filenames;
        }
        private string GetFolderPath()
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            folderDialog.SelectedPath = System.AppDomain.CurrentDomain.BaseDirectory;
            folderDialog.ShowNewFolderButton = true;
            string folder = "";
            if (folderDialog.ShowDialog() == DialogResult.OK)
                folder = folderDialog.SelectedPath;
            return folder;
        }
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
                return sc.Status.ToString();
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
                        SetProgress(0, 60,i );
                        sc.Refresh();
                        System.Threading.Thread.Sleep(1000);
                        if (sc.Status == System.ServiceProcess.ServiceControllerStatus.Running)
                        {
                            SetProgress(0, 60, 60);
                            return true;
                        }
                    }
                    SetProgress(0, 60, 60);
                }
                return false;
            }
            catch (Exception e)
            {
                ShowError(e.StackTrace.ToString());
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
                        SetProgress(0, 60, i);
                        sc.Refresh();
                        System.Threading.Thread.Sleep(1000);
                        if (sc.Status == System.ServiceProcess.ServiceControllerStatus.Stopped)
                        {
                            SetProgress(0, 60, 60);
                            return true;
                        }
                    }
                    SetProgress(0, 60, 60);
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                ShowError(e.StackTrace.ToString());
                return false;
            }
        }
        private bool InstallSystemServic(string service,string servicePath,string commandLineOptions)
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
                tranInstaller.Context=installContext;
                tranInstaller.Install(new Hashtable());
                return true;
            }
            catch(Exception e)
            {
                ShowError(e.StackTrace.ToString());
                return false;
            }

        }
        private bool UninstallSystemServic(string service,string servicePath,string commandLineOptions)
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
                tranInstaller.Context=installContext;
                tranInstaller.Uninstall(null);
                return true;
            }
            catch(Exception e)
            {
                ShowError(e.StackTrace.ToString());
                return false;
            }

        }
        void menuItem_Hide_Click(object sender, System.EventArgs e)
        {
            throw new System.Exception("The method or operation is not implemented.");
        }
        void menuItem_Show_Click(object sender, System.EventArgs e)
        {
            throw new System.Exception("The method or operation is not implemented.");
        }
        void menuItem_About_Click(object sender, System.EventArgs e)
        {
            ShowInformation("IndexEditor 1.0.0");
        }
        void menuItem_Exit_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
        private void btnSearchdConfirm_Click(object sender, EventArgs e)
        {
            if (!init) return;
            EnableControls(btnEditSearchd, btnSearchdConfirm);
            UpdatePanelSearchData(false);
            EnablePanelSearchControls(false);
        }

        private void btnEditSearchd_Click(object sender, EventArgs e)
        {
            if (!init) return;
            EnableControls(btnEditSearchd, btnSearchdConfirm);
            UpdatePanelSearchData(true);
            EnablePanelSearchControls(true);
        }

        private void btnSearchdService_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (SystemServiceExists("Searchd"))
            {
                ShowInformation("Service Searchd has been installed!");
            }
            else
            {
                if (InstallSystemServic("Searchd", AppPath + @"\Searchd.exe", CommandLineOptions))
                {
                    ShowInformation("Service Searchd install success!");
                }
                else
                {
                    ShowInformation("Service Searchd install failed!");
                }
            }
            this.Cursor = Cursors.Default;
        }

        private void btnSearchdStartService_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (StartSystemService("Searchd"))
            {
                ShowInformation("Searchd Service Start Successs!");
            }
            else
            {
                ShowInformation("Searchd Service Start Failed!");
            }
            this.Cursor = Cursors.Default;
        }

        private void btnSearchdStopService_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (StopSystemService("Searchd"))
            {
                ShowInformation("Searchd Service Stop Successs!");
            }
            else
            {
                ShowInformation("Searchd Service Stop Failed!");
            }
            this.Cursor = Cursors.Default;
        }

        private void btnSetSearchdPath_Click(object sender, EventArgs e)
        {
            string path=GetFileSavePath(OutputFilter);
            if (!string.IsNullOrEmpty(path))
               textBoxSearchdPath.Text = path;
        }

        private void btnSetQueryPath_Click(object sender, EventArgs e)
        {
            string path = GetFileSavePath(OutputFilter);
            if (!string.IsNullOrEmpty(path))
                textBoxQueryPath.Text = path;
        }

        private void btnTestDBLink_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxSourceName.Text))
                return;
            this.Cursor = Cursors.WaitCursor;
            UpdatePanelSourceData(false);
            if(source ==null)return;
            try
            {
                bool success = ISUtils.Database.DbCommon.TestDbLink(source.DBType, source.HostName,source.DataBase, source.UserName, source.Password);
                if (success)
                    ShowInformation("数据库连接测试成功！");
                else
                    ShowExclamation("数据库连接测试失败！");

            }
            catch (Exception ex)
            {
                ShowError(ex.StackTrace.ToString());
            }
            this.Cursor = Cursors.Default;
        }

        private void btnTestQuery_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxSourceName.Text))
                return;
            this.Cursor = Cursors.WaitCursor;
            UpdatePanelSourceData(false);
            if (source == null) return;
            try
            {
                bool success = ISUtils.Database.DbCommon.TestQuery(source.DBType, source.HostName, source.DataBase, source.UserName, source.Password,source.Query);
                if (success)
                    ShowInformation("数据库查询测试成功！");
                else
                    ShowExclamation("数据库查询测试失败！");

            }
            catch (Exception ex)
            {
                ShowError(ex.StackTrace.ToString());
            }
            this.Cursor = Cursors.Default;
        }

        private void btnTestFields_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxSourceName.Text))
                return;
            this.Cursor = Cursors.WaitCursor;
            UpdatePanelSourceData(false);
            if (source == null) return;
            try
            {
                bool success = ISUtils.Database.DbCommon.TestFields(source.DBType, source.HostName, source.DataBase, source.UserName, source.Password,source.Query,source.GetFields());
                if (success)
                    ShowInformation("数据库字段测试成功！");
                else
                    ShowExclamation("数据库字段测试失败！");

            }
            catch (Exception ex)
            {
                ShowError(ex.StackTrace.ToString());
            }
            this.Cursor = Cursors.Default;
        }
        private void EnableControls(bool enabled, params Control[] controls)
        {
            foreach (Control control in controls)
            {
                control.Enabled = enabled;
            }
        }
        private void EnableControls(params Control[] controls)
        {
            foreach (Control control in controls)
            {
                control.Enabled = !control.Enabled;
            }
        }
        private void EnablePanelSourceButtons(Status status)
        {
            Control[] buttons = new Control[] { btnAddSource,btnEditSource,btnDelSource,btnSourceConfim,btnSourceCancel};
            switch (status)
            {
                case Status.Cancel:
                case Status.Confirm:
                case Status.Insert:
                case Status.Edit:
                    EnableControls(buttons);
                    break;
                case Status.Delete:
                default :
                    break;
            }
        }
        private void btnAddSource_Click(object sender, EventArgs e)
        {
            if (sourceList == null) return;
            source = new Source();
            UpdatePanelSourceData(true);
            EnablePanelSourceControls(true);
            status = Status.Insert;
            EnablePanelSourceButtons(status);
            comboBoxSource.Enabled = false;
        }

        private void btnEditSource_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxSourceName.Text))
                return;
            EnablePanelSourceControls(true);
            textBoxSourceName.Enabled = false;
            status = Status.Edit;
            EnablePanelSourceButtons(status);
            comboBoxSource.Enabled = false;
        }

        private void btnDelSource_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxSourceName.Text))
                return;
            if (sourceList == null)
                return;
            if (source == null)
                return;
            if (sourceList.Count <= 0)
                return;
            status = Status.Delete;
            sourceList.Remove(source);
            foreach (IndexSet set in indexList)
            {
                if (set.SourceName == source.SourceName)
                {
                    indexList.Remove(set);
                }
            }
            if (indexList.Count > 0)
            {
                if (indexSet.SourceName == source.SourceName)
                    indexSet = indexList[0];
            }
            else
            {
                indexSet = new IndexSet();
            }
            InitComboBoxIndex(indexList);
            if (sourceList.Count > 0)
                source = sourceList[0];
            else
                source = new Source();
            InitComboBoxSource(sourceList);
            InitComboBoxSourceSel(sourceList);
            UpdatePanelSourceData(true);
            EnablePanelSourceButtons(status);
        }

        private void btnSourceConfim_Click(object sender, EventArgs e)
        {
            switch (status)
            {
                case Status.Insert:
                    if (sourceList != null)
                    {
                        UpdatePanelSourceData(false);
                        foreach (Source s in sourceList)
                        {
                            if (s.SourceName.ToLower().CompareTo(source.SourceName.ToLower()) == 0)
                            {
                                ShowWarning("已存在同名的数据源！");
                                textBoxSourceName.Text="";
                                return ;
                            }
                        }
                        if (ISUtils.Database.DbCommon.TestFields(source.DBType, source.HostName, source.DataBase, source.UserName, source.Password, source.Query, source.GetFields()))
                        {
                            sourceList.Add(source);
                            comboBoxSource.Items.Add(source.SourceName);
                            makeChange = true;
                        }
                        else
                        {
                            ShowWarning("数据库系列测试失败！");
                            return;
                        }
                    }
                    break;
                case Status.Edit:
                    if (sourceList != null)
                    {
                        UpdatePanelSourceData(false);
                        foreach (Source s in sourceList)
                        {
                            if (s.SourceName.ToLower().CompareTo(source.SourceName.ToLower()) == 0)
                            {
                                sourceList.Remove(s);
                                sourceList.Add(source);
                                makeChange = true;
                                break;
                            }
                        }
                    }
                    break;
                default :
                    break;
            }
            InitComboBoxSource(sourceList);
            InitComboBoxSourceSel(sourceList);
            EnablePanelSourceControls(false);
            status = Status.Confirm;
            comboBoxSource.Enabled = true;
        }

        private void btnSourceCancel_Click(object sender, EventArgs e)
        {
            if (sourceList != null && sourceList.Count>0)
            {
                source = sourceList[0];
                UpdatePanelSourceData(true);
            }
            EnablePanelSourceControls(false);
            status = Status.Cancel;
            comboBoxSource.Enabled = true;
        }

        private void btnSetBasePath_Click(object sender, EventArgs e)
        {
            string path = GetFileOpenPath(DictionaryFilter);
            if (!string.IsNullOrEmpty(path))
            textBoxBasePath.Text =path;
        }

        private void btnSetNamePath_Click(object sender, EventArgs e)
        {
            string path = GetFileOpenPath(DictionaryFilter);
            if (!string.IsNullOrEmpty(path))
                textBoxNamePath.Text = path;
        }

        private void btnSetNumberPath_Click(object sender, EventArgs e)
        {
            string path = GetFileOpenPath(DictionaryFilter);
            if (!string.IsNullOrEmpty(path))
                textBoxNumberPath.Text = path;
        }

        private void btnSetCustomPaths_Click(object sender, EventArgs e)
        {
            Dictionary<string, int> dict=new Dictionary<string,int>();
            foreach (Object o in listBoxCustomPaths.Items)
            {
                dict.Add(o.ToString(), o.ToString().Length);
            }
            string[] paths = GetFileOpenPaths(DictionaryFilter);
            if (paths != null)
            {
                foreach (string path in paths)
                {
                    if (!dict.ContainsKey(path))
                    {
                        listBoxCustomPaths.Items.Add(path);
                    }
                }
            }
        }

        private void btnSetFilterPath_Click(object sender, EventArgs e)
        {
            string path = GetFileOpenPath(DictionaryFilter);
            if (!string.IsNullOrEmpty(path))
                textBoxFilterPath.Text = path;
        }

        private void btnChangeDictSet_Click(object sender, EventArgs e)
        {
            if (!init) return;
            EnablePanelDictControls(true);
            EnableControls(btnChangeDictSet, btnDictConfirm);
        }

        private void btnDictConfirm_Click(object sender, EventArgs e)
        {
            if (!init) return;
            UpdatePanelDictData(false);
            EnablePanelDictControls(false);
            makeChange = true;
            EnableControls(btnChangeDictSet, btnDictConfirm);
        }

        private void btnDictService_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (SystemServiceExists("Indexer"))
            {
                ShowInformation("Service Indexer has been installed!");
            }
            else
            {
                if (InstallSystemServic("Indexer", AppPath + @"\Indexer.exe", CommandLineOptions))
                {
                    ShowInformation("Service Indexer install success!");
                }
                else
                {
                    ShowInformation("Service Indexer install failed!");
                }
            }
            if (SystemServiceExists("Searchd"))
            {
                ShowInformation("Service Searchd has been installed!");
            }
            else
            {
                if (InstallSystemServic("Searchd", AppPath + @"\Searchd.exe", CommandLineOptions))
                {
                    ShowInformation("Service Searchd install success!");
                }
                else
                {
                    ShowInformation("Service Searchd install failed!");
                }
            }
            this.Cursor = Cursors.Default;
        }

        private void btnDictStartService_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (StartSystemService("Indexer"))
            {
                ShowInformation("Indexer Service Start Successs!");
            }
            else
            {
                ShowInformation("Indexer Service Start Failed!");
            }
            if (StartSystemService("Indexer"))
            {
                ShowInformation("Indexer Service Start Successs!");
            }
            else
            {
                ShowInformation("Indexer Service Start Failed!");
            }
            this.Cursor = Cursors.Default;
        }

        private void btnDictStopService_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (StopSystemService("Indexer"))
            {
                ShowInformation("Indexer Service Stop Successs!");
            }
            else
            {
                ShowInformation("Indexer Service Stop Failed!");
            }
            if (StopSystemService("Searchd"))
            {
                ShowInformation("Searchd Service Stop Successs!");
            }
            else
            {
                ShowInformation("Searchd Service Stop Failed!");
            }
            this.Cursor = Cursors.Default;
        }

        private void btnAddIndex_Click(object sender, EventArgs e)
        {
            if (indexList == null) return;
            indexSet = new IndexSet();
            UpdatePanelIndexData(true);
            EnablePanelIndexControls(true);
            status = Status.Insert;
            EnableControls(btnAddIndex, btnEditIndex, btnDelIndex, btnIndexConfirm, btnIndexCancel);
            comboBoxIndex.Enabled = false;
        }

        private void btnEditIndex_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxIndexName.Text))
                return;
            EnablePanelIndexControls(true);
            EnableControls(btnAddIndex, btnEditIndex, btnDelIndex, btnIndexConfirm, btnIndexCancel);
            textBoxIndexName.Enabled = false;
            status = Status.Edit;
            comboBoxIndex.Enabled = false;
        }

        private void btnDelIndex_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxIndexName.Text))
                return;
            if (indexList == null)
                return;
            if (indexSet == null)
                return;
            if (indexList.Count <= 0)
                return;
            status = Status.Delete;
            indexList.Remove(indexSet);
            InitComboBoxIndex(indexList);
            if (indexList.Count > 0)
                indexSet = indexList[0];
            else
                indexSet = new IndexSet();
            InitComboBoxIndex(indexList);
            UpdatePanelSourceData(true);
        }

        private void btnIndexConfirm_Click(object sender, EventArgs e)
        {
            switch (status)
            {
                case Status.Insert:
                    if (indexList != null)
                    {
                        UpdatePanelIndexData(false);
                        foreach (IndexSet set in indexList)
                        {
                            if (set.IndexName.ToLower().CompareTo(indexSet.IndexName.ToLower()) == 0)
                            {
                                ShowWarning("已存在同名的索引！");
                                textBoxIndexName.Text = "";
                                return;
                            }
                        }
                        indexList.Add(indexSet);
                        comboBoxIndex.Items.Add(indexSet.IndexName);
                        makeChange = true;
                    }
                    break;
                case Status.Edit:
                    if (sourceList != null)
                    {
                        UpdatePanelIndexData(false);
                        foreach (IndexSet set in indexList)
                        {
                            if (set.IndexName.ToLower().CompareTo(indexSet.IndexName.ToLower()) == 0)
                            {
                                indexList.Remove(set);
                                indexList.Add(indexSet);
                                makeChange = true;
                                break;
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
            InitComboBoxIndex(indexList);
            EnablePanelIndexControls(false);
            status = Status.Confirm;
            comboBoxIndex.Enabled = true;
            EnableControls(btnAddIndex, btnEditIndex, btnDelIndex, btnIndexConfirm, btnIndexCancel);
        }

        private void btnIndexCancel_Click(object sender, EventArgs e)
        {
            if (indexList != null && indexList.Count > 0)
            {
                indexSet = indexList[0];
                UpdatePanelIndexData(true);
            }
            EnablePanelIndexControls(false);
            status = Status.Cancel;
            comboBoxIndex.Enabled = true;
            EnableControls(btnAddIndex, btnEditIndex, btnDelIndex, btnIndexConfirm, btnIndexCancel);
        }

        private void btnSetIndexPath_Click(object sender, EventArgs e)
        {
            string path=GetFolderPath();
            if (!string.IsNullOrEmpty(path))
                textBoxIndexPath.Text = path;
        }

        private void btnMainIndexReCreate_Click(object sender, EventArgs e)
        {
            if (!init) return;
            toolStripStatusLabelStatus.Text = "开始构建主索引！";
            this.Cursor = Cursors.WaitCursor;
            ISUtils.Utils.IndexUtil.SetIndexSettings(AppPath + @"\config.conf");
            toolStripProgressBar.Visible = true;
            toolStripProgressBar.Minimum = 0;
            toolStripProgressBar.Maximum = ISUtils.SupportClass.PERCENTAGEDIVE;
            Application.DoEvents();
            toolStripStatusLabelStatus.Text = "正在构建主索引！";
            ISUtils.Utils.IndexUtil.IndexWithEventEx(IndexTypeEnum.Ordinary,OnIndexCompleted,OnProgressChanged);
            toolStripProgressBar.Visible = false;
            this.Cursor = Cursors.Default;
            toolStripStatusLabelStatus.Text = "构建主索引完毕！";
            ShowInformation("主索引构建完成");
            toolStripStatusLabelStatus.Text = "";
        }
        private void OnIndexCompleted(object sender, IndexCompletedEventArgs e)
        {
            Application.DoEvents();
        }
        private void OnProgressChanged(object sender, IndexProgressChangedEventArgs e)
        {
            toolStripProgressBar.Value = (e.Current*ISUtils.SupportClass.PERCENTAGEDIVE) / e.Total;
            Application.DoEvents();
        }
        private void btnReCreateIncrIndex_Click(object sender, EventArgs e)
        {
            if (!init) return;
            toolStripStatusLabelStatus.Text = "开始构建增量索引！";
            this.Cursor = Cursors.WaitCursor;
            ISUtils.Utils.IndexUtil.SetIndexSettings(AppPath + @"\config.conf");
            toolStripProgressBar.Visible = true;
            toolStripProgressBar.Minimum = 0;
            toolStripProgressBar.Maximum = ISUtils.SupportClass.PERCENTAGEDIVE;
            Application.DoEvents();
            toolStripStatusLabelStatus.Text = "正在构建增量索引！";
            ISUtils.Utils.IndexUtil.IndexWithEventEx(IndexTypeEnum.Increment, OnIndexCompleted, OnProgressChanged);
            toolStripProgressBar.Visible = false;
            this.Cursor = Cursors.Default;
            toolStripStatusLabelStatus.Text = "构建增量索引完毕！";
            ShowInformation("增量索引构建完成");
            toolStripStatusLabelStatus.Text = "";
        }

        private void btnEditIndexer_Click(object sender, EventArgs e)
        {
            if (!init) return;
            EnableControls(btnEditIndexer, btnIndexerConfirm);
            UpdatePanelIndexerData(true);
            EnablePanelIndexerControls(true);
        }

        private void btnIndexerConfirm_Click(object sender, EventArgs e)
        {
            if (!init) return;
            EnableControls(btnEditIndexer, btnIndexerConfirm);
            UpdatePanelIndexerData(false);
            EnablePanelIndexerControls(false);
        }

        private void btnIndexerService_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (SystemServiceExists("Indexer"))
            {
                ShowInformation("Service Indexer has been installed!");
            }
            else
            {
                if (InstallSystemServic("Indexer", AppPath + @"\Indexer.exe", CommandLineOptions))
                {
                    ShowInformation("Service Indexer install success!");
                }
                else
                {
                    ShowInformation("Service Indexer install failed!");
                }
            }
            this.Cursor = Cursors.Default;
        }

        private void btnIndexerStartService_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (StartSystemService("Indexer"))
            {
                ShowInformation("Indexer Service Start Successs!");
            }
            else
            {
                ShowInformation("Indexer Service Start Failed!");
            }
            this.Cursor = Cursors.Default;
        }

        private void btnIndexerStopService_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (StopSystemService("Indexer"))
            {
                ShowInformation("Indexer Service Stop Successs!");
            }
            else
            {
                ShowInformation("Indexer Service Stop Failed!");
            }
            this.Cursor = Cursors.Default;
        }

        private void comboBoxIndex_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitPanelIndexControls(comboBoxIndex.SelectedItem.ToString());
        }

        private void comboBoxSouce_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitPanelSourceControls(comboBoxSource.SelectedItem.ToString());
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabelIndexer.Text = "Indexer Service "+ GetSystemServiceStatus("Indexer");
            toolStripStatusLabelSearchd.Text = "Searchd Service "+ GetSystemServiceStatus("Searchd");
        }
        private void InitComboBoxSource(List<Source> sourceList)
        {
            comboBoxSource.Items.Clear();
            if (sourceList == null) return;
            foreach (Source s in sourceList)
            {
                comboBoxSource.Items.Add(s.SourceName);
            }
        }
        private void InitComboBoxIndex(List<IndexSet> indexList)
        {
            comboBoxIndex.Items.Clear();
            if (indexList == null) return;
            foreach (IndexSet i in indexList)
            {
                comboBoxIndex.Items.Add(i.IndexName);
            }
        }
        private void InitComboBoxSourceSel(List<Source> sourceList)
        {
            comboBoxSouceSel.Items.Clear();
            if (sourceList == null) return;
            foreach (Source s in sourceList)
            {
                comboBoxSouceSel.Items.Add(s.SourceName);
            }
        }
        private void InitButtonsEnabled()
        {
            EnableControls(false, btnSourceConfim, btnSourceCancel);
            EnableControls(false, btnIndexConfirm, btnIndexCancel);
            EnableControls(false, btnIndexerConfirm);
            EnableControls(false, btnSearchdConfirm);
            EnableControls(false, btnDictConfirm);
        }
        private void InitGUIControls()
        {
            InitComboBoxSource(sourceList);
            InitComboBoxIndex(indexList);
            InitComboBoxSourceSel(sourceList);
            EnablePanelSourceControls(false);
            EnablePanelIndexControls(false);
            EnablePanelIndexerControls(false);
            EnablePanelSearchControls(false);
            EnablePanelDictControls(false);
            InitPanelIndexerControls();
            InitPanelSearchControls();
            InitPanelDictControls();
            InitButtonsEnabled();
        }
        private void toolStripMenuItemOpen_Click(object sender, EventArgs e)
        {
            if (!init)
            {
                if (!ISUtils.SupportClass.File.IsFileExists(AppPath +@"\config.conf"))
                {
                    ShowInformation(AppPath + @"\config.conf does not exists!");
                }
                if (InitData(AppPath + @"\config.conf"))
                {
                    InitGUIControls();
                    ShowInformation("成功加载配置文件！");
                    init = true;
                }
                else
                {
                    ShowInformation("加载配置文件失败！");
                }
            }
        }

        private void toolStripMenuItemExit_Click(object sender, EventArgs e)
        {
            if (init)
            {
                if (!makeChange)
                    this.Close();
                else
                    ShowWarning("尚未保存更改！");
            }
            else
            {
                this.Close();
            }
        }
        void frmEditor_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            if (init)
            {
                if (!makeChange)
                {
                    this.Dispose(true);
                }
                else
                {
                    ShowWarning("尚未保存更改！");
                    e.Cancel = true;
                }
            }
            else
            {
                this.Dispose(true);
            }
        }
        private void toolStripMenuItemSave_Click(object sender, EventArgs e)
        {
            if (init && makeChange)
            {
                ISUtils.SupportClass.File.WriteConfigFile(AppPath + @"\config.conf", sourceList, indexList, dictSet, indexerSet, searchSet);
                makeChange = false;
                ShowInformation("保存成功！");
            }
        }
        private void SetProgress(params int[] values)
        {
            if (toolStripProgressBar.Visible == false)
                toolStripProgressBar.Visible =true;
            if (values.Length == 1)
            {
                toolStripProgressBar.Value = values[0];
            }
            if (values.Length == 2)
            {
                toolStripProgressBar.Minimum = values[0];
                toolStripProgressBar.Maximum = values[1];
                toolStripProgressBar.Value = values[0];
            }
            if (values.Length == 3)
            {
                toolStripProgressBar.Minimum = values[0];
                toolStripProgressBar.Maximum = values[1];
                toolStripProgressBar.Value = values[2];
            }
            if (toolStripProgressBar.Value == toolStripProgressBar.Maximum)
                toolStripProgressBar.Visible = false;
        }

        private void toolStripMenuItemSaveAs_Click(object sender, EventArgs e)
        {
            if (init )
            {
                string path=GetFileSavePath(OutputFilter);
                if (!string.IsNullOrEmpty(path))
                {
                    ISUtils.SupportClass.File.WriteConfigFile(path, sourceList, indexList, dictSet, indexerSet, searchSet);
                    ShowInformation("保存成功！");
                }
            }
        }

        private void toolStripButtonOpen_Click(object sender, EventArgs e)
        {
            toolStripMenuItemOpen_Click(sender, e);
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            toolStripMenuItemSave_Click(sender, e);
        }

        private void toolStripButtonSaveAs_Click(object sender, EventArgs e)
        {
            toolStripMenuItemSaveAs_Click(sender, e);
        }

        private void toolStripButtonExit_Click(object sender, EventArgs e)
        {
            toolStripMenuItemExit_Click(sender, e);
        }
    }
}