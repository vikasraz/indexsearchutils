namespace IndexEditor
{
    partial class frmEditor
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEditor));
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("数据源", 0);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("索引设置", 1);
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem("索引器", 2);
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem("搜索设置", 3);
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem("词库设置", 4);
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.NicontextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItem_Hide = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_Show = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_About = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItemFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSave = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonOpen = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSaveAs = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonExit = new System.Windows.Forms.ToolStripButton();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelSearchd = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelIndexer = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.listView = new System.Windows.Forms.ListView();
            this.panelSource = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSourceCancel = new System.Windows.Forms.Button();
            this.imageListButtons = new System.Windows.Forms.ImageList(this.components);
            this.btnSourceConfim = new System.Windows.Forms.Button();
            this.btnDelSource = new System.Windows.Forms.Button();
            this.btnEditSource = new System.Windows.Forms.Button();
            this.btnAddSource = new System.Windows.Forms.Button();
            this.btnTestFields = new System.Windows.Forms.Button();
            this.btnTestQuery = new System.Windows.Forms.Button();
            this.btnTestDBLink = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxQuery = new System.Windows.Forms.TextBox();
            this.comboBoxSourceType = new System.Windows.Forms.ComboBox();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.textBoxUserName = new System.Windows.Forms.TextBox();
            this.textBoxDataBase = new System.Windows.Forms.TextBox();
            this.textBoxFields = new System.Windows.Forms.TextBox();
            this.textBoxHostName = new System.Windows.Forms.TextBox();
            this.textBoxSourceName = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxSource = new System.Windows.Forms.ComboBox();
            this.panelDictionary = new System.Windows.Forms.Panel();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.btnChangeDictSet = new System.Windows.Forms.Button();
            this.btnDictStopService = new System.Windows.Forms.Button();
            this.btnDictStartService = new System.Windows.Forms.Button();
            this.btnDictService = new System.Windows.Forms.Button();
            this.btnDictConfirm = new System.Windows.Forms.Button();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.btnSetCustomPaths = new System.Windows.Forms.Button();
            this.btnSetFilterPath = new System.Windows.Forms.Button();
            this.btnSetNumberPath = new System.Windows.Forms.Button();
            this.btnSetNamePath = new System.Windows.Forms.Button();
            this.btnSetBasePath = new System.Windows.Forms.Button();
            this.textBoxFilterPath = new System.Windows.Forms.TextBox();
            this.textBoxNumberPath = new System.Windows.Forms.TextBox();
            this.textBoxNamePath = new System.Windows.Forms.TextBox();
            this.textBoxBasePath = new System.Windows.Forms.TextBox();
            this.listBoxCustomPaths = new System.Windows.Forms.ListBox();
            this.label37 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.label40 = new System.Windows.Forms.Label();
            this.label43 = new System.Windows.Forms.Label();
            this.label44 = new System.Windows.Forms.Label();
            this.panelIndexSet = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnDelIndex = new System.Windows.Forms.Button();
            this.btnEditIndex = new System.Windows.Forms.Button();
            this.btnAddIndex = new System.Windows.Forms.Button();
            this.btnIndexCancel = new System.Windows.Forms.Button();
            this.btnIndexConfirm = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnSetIndexPath = new System.Windows.Forms.Button();
            this.comboBoxSouceSel = new System.Windows.Forms.ComboBox();
            this.comboBoxIndexType = new System.Windows.Forms.ComboBox();
            this.textBoxIndexPath = new System.Windows.Forms.TextBox();
            this.textBoxIndexName = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.comboBoxIndex = new System.Windows.Forms.ComboBox();
            this.panelIndexerSet = new System.Windows.Forms.Panel();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.btnEditIndexer = new System.Windows.Forms.Button();
            this.btnReCreateIncrIndex = new System.Windows.Forms.Button();
            this.btnMainIndexReCreate = new System.Windows.Forms.Button();
            this.btnIndexerStopService = new System.Windows.Forms.Button();
            this.btnIndexerStartService = new System.Windows.Forms.Button();
            this.btnIndexerService = new System.Windows.Forms.Button();
            this.btnIndexerConfirm = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.numericUpDownMergeFactor = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownMaxBufferedDocs = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownMaxFieldLength = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownRamBufferSize = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownIncrTimeSpan = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownMainCreateTimeSpan = new System.Windows.Forms.NumericUpDown();
            this.dateTimePickerMainReCreate = new System.Windows.Forms.DateTimePicker();
            this.label22 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.panelSearchd = new System.Windows.Forms.Panel();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.btnSearchdStopService = new System.Windows.Forms.Button();
            this.btnSearchdStartService = new System.Windows.Forms.Button();
            this.btnEditSearchd = new System.Windows.Forms.Button();
            this.btnSearchdService = new System.Windows.Forms.Button();
            this.btnSearchdConfirm = new System.Windows.Forms.Button();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.btnSetQueryPath = new System.Windows.Forms.Button();
            this.textBoxQueryPath = new System.Windows.Forms.TextBox();
            this.btnSetSearchdPath = new System.Windows.Forms.Button();
            this.textBoxSearchdPath = new System.Windows.Forms.TextBox();
            this.maskedTextBoxIpAddress = new System.Windows.Forms.MaskedTextBox();
            this.numericUpDownMaxTransport = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownMaxMatches = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownPort = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownTimeOut = new System.Windows.Forms.NumericUpDown();
            this.label25 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.NicontextMenu.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.panelSource.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panelDictionary.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.panelIndexSet.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.panelIndexerSet.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMergeFactor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxBufferedDocs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxFieldLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRamBufferSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownIncrTimeSpan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMainCreateTimeSpan)).BeginInit();
            this.panelSearchd.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxTransport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxMatches)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimeOut)).BeginInit();
            this.SuspendLayout();
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "source.jpg");
            this.imageList.Images.SetKeyName(1, "index.jpg");
            this.imageList.Images.SetKeyName(2, "indexer.jpg");
            this.imageList.Images.SetKeyName(3, "search.jpg");
            this.imageList.Images.SetKeyName(4, "dictionaryset.jpg");
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.NicontextMenu;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "notifyIcon1";
            this.notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_Click);
            // 
            // NicontextMenu
            // 
            this.NicontextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItem_Hide,
            this.menuItem_Show,
            this.menuItem_About,
            this.menuItem_Exit});
            this.NicontextMenu.Name = "NicontextMenu";
            this.NicontextMenu.Size = new System.Drawing.Size(95, 92);
            // 
            // menuItem_Hide
            // 
            this.menuItem_Hide.Name = "menuItem_Hide";
            this.menuItem_Hide.Size = new System.Drawing.Size(94, 22);
            this.menuItem_Hide.Text = "隐藏";
            this.menuItem_Hide.Click += new System.EventHandler(this.menuItem_Hide_Click);
            // 
            // menuItem_Show
            // 
            this.menuItem_Show.Name = "menuItem_Show";
            this.menuItem_Show.Size = new System.Drawing.Size(94, 22);
            this.menuItem_Show.Text = "显示";
            this.menuItem_Show.Click += new System.EventHandler(this.menuItem_Show_Click);
            // 
            // menuItem_About
            // 
            this.menuItem_About.Name = "menuItem_About";
            this.menuItem_About.Size = new System.Drawing.Size(94, 22);
            this.menuItem_About.Text = "关于";
            this.menuItem_About.Click += new System.EventHandler(this.menuItem_About_Click);
            // 
            // menuItem_Exit
            // 
            this.menuItem_Exit.Name = "menuItem_Exit";
            this.menuItem_Exit.Size = new System.Drawing.Size(94, 22);
            this.menuItem_Exit.Text = "退出";
            this.menuItem_Exit.Click += new System.EventHandler(this.menuItem_Exit_Click);
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemFile});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(832, 24);
            this.menuStrip.TabIndex = 8;
            this.menuStrip.Text = "菜单栏";
            // 
            // toolStripMenuItemFile
            // 
            this.toolStripMenuItemFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemOpen,
            this.toolStripMenuItemSave,
            this.toolStripMenuItem1,
            this.toolStripMenuItemSaveAs,
            this.toolStripMenuItem2,
            this.toolStripMenuItemExit});
            this.toolStripMenuItemFile.Name = "toolStripMenuItemFile";
            this.toolStripMenuItemFile.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F)));
            this.toolStripMenuItemFile.Size = new System.Drawing.Size(59, 20);
            this.toolStripMenuItemFile.Text = "文件(&F)";
            // 
            // toolStripMenuItemOpen
            // 
            this.toolStripMenuItemOpen.Name = "toolStripMenuItemOpen";
            this.toolStripMenuItemOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.toolStripMenuItemOpen.Size = new System.Drawing.Size(165, 22);
            this.toolStripMenuItemOpen.Text = "加载(&O)";
            this.toolStripMenuItemOpen.Click += new System.EventHandler(this.toolStripMenuItemOpen_Click);
            // 
            // toolStripMenuItemSave
            // 
            this.toolStripMenuItemSave.Name = "toolStripMenuItemSave";
            this.toolStripMenuItemSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.toolStripMenuItemSave.Size = new System.Drawing.Size(165, 22);
            this.toolStripMenuItemSave.Text = "保存(&S)";
            this.toolStripMenuItemSave.Click += new System.EventHandler(this.toolStripMenuItemSave_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(162, 6);
            // 
            // toolStripMenuItemSaveAs
            // 
            this.toolStripMenuItemSaveAs.Name = "toolStripMenuItemSaveAs";
            this.toolStripMenuItemSaveAs.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.toolStripMenuItemSaveAs.Size = new System.Drawing.Size(165, 22);
            this.toolStripMenuItemSaveAs.Text = "另存为(&A)";
            this.toolStripMenuItemSaveAs.Click += new System.EventHandler(this.toolStripMenuItemSaveAs_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(162, 6);
            // 
            // toolStripMenuItemExit
            // 
            this.toolStripMenuItemExit.Name = "toolStripMenuItemExit";
            this.toolStripMenuItemExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.toolStripMenuItemExit.Size = new System.Drawing.Size(165, 22);
            this.toolStripMenuItemExit.Text = "退出(&X)";
            this.toolStripMenuItemExit.Click += new System.EventHandler(this.toolStripMenuItemExit_Click);
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonOpen,
            this.toolStripButtonSave,
            this.toolStripButtonSaveAs,
            this.toolStripButtonExit});
            this.toolStrip.Location = new System.Drawing.Point(0, 24);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(832, 25);
            this.toolStrip.TabIndex = 9;
            this.toolStrip.Text = "工具栏";
            // 
            // toolStripButtonOpen
            // 
            this.toolStripButtonOpen.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonOpen.Image")));
            this.toolStripButtonOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonOpen.Name = "toolStripButtonOpen";
            this.toolStripButtonOpen.Size = new System.Drawing.Size(49, 22);
            this.toolStripButtonOpen.Text = "加载";
            this.toolStripButtonOpen.Click += new System.EventHandler(this.toolStripButtonOpen_Click);
            // 
            // toolStripButtonSave
            // 
            this.toolStripButtonSave.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSave.Image")));
            this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSave.Name = "toolStripButtonSave";
            this.toolStripButtonSave.Size = new System.Drawing.Size(49, 22);
            this.toolStripButtonSave.Text = "保存";
            this.toolStripButtonSave.Click += new System.EventHandler(this.toolStripButtonSave_Click);
            // 
            // toolStripButtonSaveAs
            // 
            this.toolStripButtonSaveAs.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSaveAs.Image")));
            this.toolStripButtonSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSaveAs.Name = "toolStripButtonSaveAs";
            this.toolStripButtonSaveAs.Size = new System.Drawing.Size(61, 22);
            this.toolStripButtonSaveAs.Text = "另存为";
            this.toolStripButtonSaveAs.Click += new System.EventHandler(this.toolStripButtonSaveAs_Click);
            // 
            // toolStripButtonExit
            // 
            this.toolStripButtonExit.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonExit.Image")));
            this.toolStripButtonExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonExit.Name = "toolStripButtonExit";
            this.toolStripButtonExit.Size = new System.Drawing.Size(49, 22);
            this.toolStripButtonExit.Text = "退出";
            this.toolStripButtonExit.Click += new System.EventHandler(this.toolStripButtonExit_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.AllowItemReorder = true;
            this.statusStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelStatus,
            this.toolStripStatusLabelSearchd,
            this.toolStripStatusLabelIndexer,
            this.toolStripProgressBar});
            this.statusStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.statusStrip.Location = new System.Drawing.Point(0, 547);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(832, 22);
            this.statusStrip.TabIndex = 12;
            this.statusStrip.Text = "状态栏";
            // 
            // toolStripStatusLabelStatus
            // 
            this.toolStripStatusLabelStatus.AutoSize = false;
            this.toolStripStatusLabelStatus.Name = "toolStripStatusLabelStatus";
            this.toolStripStatusLabelStatus.Size = new System.Drawing.Size(150, 17);
            // 
            // toolStripStatusLabelSearchd
            // 
            this.toolStripStatusLabelSearchd.AutoSize = false;
            this.toolStripStatusLabelSearchd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusLabelSearchd.Name = "toolStripStatusLabelSearchd";
            this.toolStripStatusLabelSearchd.Size = new System.Drawing.Size(200, 17);
            // 
            // toolStripStatusLabelIndexer
            // 
            this.toolStripStatusLabelIndexer.AutoSize = false;
            this.toolStripStatusLabelIndexer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusLabelIndexer.Name = "toolStripStatusLabelIndexer";
            this.toolStripStatusLabelIndexer.Size = new System.Drawing.Size(200, 17);
            // 
            // toolStripProgressBar
            // 
            this.toolStripProgressBar.AutoSize = false;
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            this.toolStripProgressBar.Size = new System.Drawing.Size(100, 16);
            this.toolStripProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.toolStripProgressBar.Visible = false;
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer.Location = new System.Drawing.Point(0, 49);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.listView);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.panelIndexerSet);
            this.splitContainer.Panel2.Controls.Add(this.panelSearchd);
            this.splitContainer.Panel2.Controls.Add(this.panelSource);
            this.splitContainer.Panel2.Controls.Add(this.panelDictionary);
            this.splitContainer.Panel2.Controls.Add(this.panelIndexSet);
            this.splitContainer.Size = new System.Drawing.Size(832, 498);
            this.splitContainer.SplitterDistance = 167;
            this.splitContainer.TabIndex = 13;
            // 
            // listView
            // 
            this.listView.Cursor = System.Windows.Forms.Cursors.Default;
            this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView.GridLines = true;
            this.listView.ImeMode = System.Windows.Forms.ImeMode.Off;
            listViewItem1.StateImageIndex = 0;
            listViewItem2.StateImageIndex = 0;
            listViewItem3.StateImageIndex = 0;
            this.listView.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4,
            listViewItem5});
            this.listView.LargeImageList = this.imageList;
            this.listView.Location = new System.Drawing.Point(0, 0);
            this.listView.Margin = new System.Windows.Forms.Padding(10);
            this.listView.MultiSelect = false;
            this.listView.Name = "listView";
            this.listView.ShowItemToolTips = true;
            this.listView.Size = new System.Drawing.Size(167, 498);
            this.listView.TabIndex = 0;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.SelectedIndexChanged += new System.EventHandler(this.listView_SelectedIndexChanged);
            // 
            // panelSource
            // 
            this.panelSource.Controls.Add(this.groupBox2);
            this.panelSource.Controls.Add(this.groupBox1);
            this.panelSource.Controls.Add(this.label1);
            this.panelSource.Controls.Add(this.comboBoxSource);
            this.panelSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSource.Location = new System.Drawing.Point(0, 0);
            this.panelSource.Name = "panelSource";
            this.panelSource.Size = new System.Drawing.Size(661, 498);
            this.panelSource.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnSourceCancel);
            this.groupBox2.Controls.Add(this.btnSourceConfim);
            this.groupBox2.Controls.Add(this.btnDelSource);
            this.groupBox2.Controls.Add(this.btnEditSource);
            this.groupBox2.Controls.Add(this.btnAddSource);
            this.groupBox2.Controls.Add(this.btnTestFields);
            this.groupBox2.Controls.Add(this.btnTestQuery);
            this.groupBox2.Controls.Add(this.btnTestDBLink);
            this.groupBox2.Location = new System.Drawing.Point(473, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(178, 482);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            // 
            // btnSourceCancel
            // 
            this.btnSourceCancel.Enabled = false;
            this.btnSourceCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSourceCancel.ImageIndex = 7;
            this.btnSourceCancel.ImageList = this.imageListButtons;
            this.btnSourceCancel.Location = new System.Drawing.Point(0, 422);
            this.btnSourceCancel.Name = "btnSourceCancel";
            this.btnSourceCancel.Size = new System.Drawing.Size(178, 59);
            this.btnSourceCancel.TabIndex = 3;
            this.btnSourceCancel.Text = "取消操作";
            this.btnSourceCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSourceCancel.UseVisualStyleBackColor = true;
            this.btnSourceCancel.Click += new System.EventHandler(this.btnSourceCancel_Click);
            // 
            // imageListButtons
            // 
            this.imageListButtons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListButtons.ImageStream")));
            this.imageListButtons.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListButtons.Images.SetKeyName(0, "testdblink.jpg");
            this.imageListButtons.Images.SetKeyName(1, "testquery.jpg");
            this.imageListButtons.Images.SetKeyName(2, "testsourcefields.jpg");
            this.imageListButtons.Images.SetKeyName(3, "newsource.jpg");
            this.imageListButtons.Images.SetKeyName(4, "editsource.jpg");
            this.imageListButtons.Images.SetKeyName(5, "sourcedelete.jpg");
            this.imageListButtons.Images.SetKeyName(6, "sourceconfirm.jpg");
            this.imageListButtons.Images.SetKeyName(7, "sourcecancel.jpg");
            this.imageListButtons.Images.SetKeyName(8, "indexcreate.jpg");
            this.imageListButtons.Images.SetKeyName(9, "indexservice.jpg");
            this.imageListButtons.Images.SetKeyName(10, "startservice.png");
            this.imageListButtons.Images.SetKeyName(11, "stopservice.png");
            // 
            // btnSourceConfim
            // 
            this.btnSourceConfim.Enabled = false;
            this.btnSourceConfim.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSourceConfim.ImageIndex = 6;
            this.btnSourceConfim.ImageList = this.imageListButtons;
            this.btnSourceConfim.Location = new System.Drawing.Point(0, 363);
            this.btnSourceConfim.Name = "btnSourceConfim";
            this.btnSourceConfim.Size = new System.Drawing.Size(178, 59);
            this.btnSourceConfim.TabIndex = 3;
            this.btnSourceConfim.Text = "确定操作";
            this.btnSourceConfim.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSourceConfim.UseVisualStyleBackColor = true;
            this.btnSourceConfim.Click += new System.EventHandler(this.btnSourceConfim_Click);
            // 
            // btnDelSource
            // 
            this.btnDelSource.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelSource.ImageIndex = 5;
            this.btnDelSource.ImageList = this.imageListButtons;
            this.btnDelSource.Location = new System.Drawing.Point(0, 304);
            this.btnDelSource.Name = "btnDelSource";
            this.btnDelSource.Size = new System.Drawing.Size(178, 59);
            this.btnDelSource.TabIndex = 3;
            this.btnDelSource.Text = "删除数据源";
            this.btnDelSource.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDelSource.UseVisualStyleBackColor = true;
            this.btnDelSource.Click += new System.EventHandler(this.btnDelSource_Click);
            // 
            // btnEditSource
            // 
            this.btnEditSource.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEditSource.ImageIndex = 4;
            this.btnEditSource.ImageList = this.imageListButtons;
            this.btnEditSource.Location = new System.Drawing.Point(0, 245);
            this.btnEditSource.Name = "btnEditSource";
            this.btnEditSource.Size = new System.Drawing.Size(178, 59);
            this.btnEditSource.TabIndex = 3;
            this.btnEditSource.Text = "修改数据源";
            this.btnEditSource.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEditSource.UseVisualStyleBackColor = true;
            this.btnEditSource.Click += new System.EventHandler(this.btnEditSource_Click);
            // 
            // btnAddSource
            // 
            this.btnAddSource.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddSource.ImageIndex = 3;
            this.btnAddSource.ImageList = this.imageListButtons;
            this.btnAddSource.Location = new System.Drawing.Point(0, 186);
            this.btnAddSource.Name = "btnAddSource";
            this.btnAddSource.Size = new System.Drawing.Size(178, 59);
            this.btnAddSource.TabIndex = 3;
            this.btnAddSource.Text = "添加数据源";
            this.btnAddSource.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAddSource.UseVisualStyleBackColor = true;
            this.btnAddSource.Click += new System.EventHandler(this.btnAddSource_Click);
            // 
            // btnTestFields
            // 
            this.btnTestFields.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTestFields.ImageIndex = 2;
            this.btnTestFields.ImageList = this.imageListButtons;
            this.btnTestFields.Location = new System.Drawing.Point(0, 127);
            this.btnTestFields.Name = "btnTestFields";
            this.btnTestFields.Size = new System.Drawing.Size(178, 59);
            this.btnTestFields.TabIndex = 3;
            this.btnTestFields.Text = "测试查询字段";
            this.btnTestFields.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnTestFields.UseVisualStyleBackColor = true;
            this.btnTestFields.Click += new System.EventHandler(this.btnTestFields_Click);
            // 
            // btnTestQuery
            // 
            this.btnTestQuery.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTestQuery.ImageIndex = 1;
            this.btnTestQuery.ImageList = this.imageListButtons;
            this.btnTestQuery.Location = new System.Drawing.Point(0, 68);
            this.btnTestQuery.Name = "btnTestQuery";
            this.btnTestQuery.Size = new System.Drawing.Size(178, 59);
            this.btnTestQuery.TabIndex = 3;
            this.btnTestQuery.Text = "测试查询语句";
            this.btnTestQuery.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnTestQuery.UseVisualStyleBackColor = true;
            this.btnTestQuery.Click += new System.EventHandler(this.btnTestQuery_Click);
            // 
            // btnTestDBLink
            // 
            this.btnTestDBLink.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTestDBLink.ImageIndex = 0;
            this.btnTestDBLink.ImageList = this.imageListButtons;
            this.btnTestDBLink.Location = new System.Drawing.Point(0, 9);
            this.btnTestDBLink.Name = "btnTestDBLink";
            this.btnTestDBLink.Size = new System.Drawing.Size(178, 59);
            this.btnTestDBLink.TabIndex = 2;
            this.btnTestDBLink.Text = "测试数据库连接";
            this.btnTestDBLink.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnTestDBLink.UseVisualStyleBackColor = true;
            this.btnTestDBLink.Click += new System.EventHandler(this.btnTestDBLink_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxQuery);
            this.groupBox1.Controls.Add(this.comboBoxSourceType);
            this.groupBox1.Controls.Add(this.textBoxPassword);
            this.groupBox1.Controls.Add(this.textBoxUserName);
            this.groupBox1.Controls.Add(this.textBoxDataBase);
            this.groupBox1.Controls.Add(this.textBoxFields);
            this.groupBox1.Controls.Add(this.textBoxHostName);
            this.groupBox1.Controls.Add(this.textBoxSourceName);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(19, 51);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(437, 437);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "数据源信息：";
            // 
            // textBoxQuery
            // 
            this.textBoxQuery.Location = new System.Drawing.Point(89, 189);
            this.textBoxQuery.Multiline = true;
            this.textBoxQuery.Name = "textBoxQuery";
            this.textBoxQuery.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxQuery.Size = new System.Drawing.Size(342, 211);
            this.textBoxQuery.TabIndex = 11;
            // 
            // comboBoxSourceType
            // 
            this.comboBoxSourceType.FormattingEnabled = true;
            this.comboBoxSourceType.Items.AddRange(new object[] {
            "SqlServer",
            "Oracle",
            "OleDB",
            "ODBC"});
            this.comboBoxSourceType.Location = new System.Drawing.Point(89, 54);
            this.comboBoxSourceType.Name = "comboBoxSourceType";
            this.comboBoxSourceType.Size = new System.Drawing.Size(257, 20);
            this.comboBoxSourceType.TabIndex = 9;
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(89, 161);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.PasswordChar = '*';
            this.textBoxPassword.Size = new System.Drawing.Size(257, 21);
            this.textBoxPassword.TabIndex = 8;
            // 
            // textBoxUserName
            // 
            this.textBoxUserName.Location = new System.Drawing.Point(89, 134);
            this.textBoxUserName.Name = "textBoxUserName";
            this.textBoxUserName.Size = new System.Drawing.Size(257, 21);
            this.textBoxUserName.TabIndex = 8;
            // 
            // textBoxDataBase
            // 
            this.textBoxDataBase.Location = new System.Drawing.Point(89, 107);
            this.textBoxDataBase.Name = "textBoxDataBase";
            this.textBoxDataBase.Size = new System.Drawing.Size(257, 21);
            this.textBoxDataBase.TabIndex = 8;
            // 
            // textBoxFields
            // 
            this.textBoxFields.Location = new System.Drawing.Point(89, 406);
            this.textBoxFields.Name = "textBoxFields";
            this.textBoxFields.Size = new System.Drawing.Size(342, 21);
            this.textBoxFields.TabIndex = 8;
            // 
            // textBoxHostName
            // 
            this.textBoxHostName.Location = new System.Drawing.Point(89, 80);
            this.textBoxHostName.Name = "textBoxHostName";
            this.textBoxHostName.Size = new System.Drawing.Size(257, 21);
            this.textBoxHostName.TabIndex = 8;
            // 
            // textBoxSourceName
            // 
            this.textBoxSourceName.Location = new System.Drawing.Point(89, 26);
            this.textBoxSourceName.Name = "textBoxSourceName";
            this.textBoxSourceName.Size = new System.Drawing.Size(257, 21);
            this.textBoxSourceName.TabIndex = 8;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 409);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 12);
            this.label9.TabIndex = 7;
            this.label9.Text = "搜索字段：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 192);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 6;
            this.label8.Tag = " ";
            this.label8.Text = "查  询：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 167);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 5;
            this.label7.Text = "密  码：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 138);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 12);
            this.label6.TabIndex = 4;
            this.label6.Text = "用 户 名 ：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 111);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 12);
            this.label5.TabIndex = 3;
            this.label5.Text = "数据库名称：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.SystemColors.Control;
            this.label4.Location = new System.Drawing.Point(6, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = "主机名称：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "数据库类型：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "数据源名称：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "数据源：";
            // 
            // comboBoxSource
            // 
            this.comboBoxSource.FormattingEnabled = true;
            this.comboBoxSource.Location = new System.Drawing.Point(108, 25);
            this.comboBoxSource.Name = "comboBoxSource";
            this.comboBoxSource.Size = new System.Drawing.Size(257, 20);
            this.comboBoxSource.TabIndex = 2;
            this.comboBoxSource.SelectedIndexChanged += new System.EventHandler(this.comboBoxSouce_SelectedIndexChanged);
            // 
            // panelDictionary
            // 
            this.panelDictionary.Controls.Add(this.groupBox9);
            this.panelDictionary.Controls.Add(this.groupBox10);
            this.panelDictionary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDictionary.Location = new System.Drawing.Point(0, 0);
            this.panelDictionary.Name = "panelDictionary";
            this.panelDictionary.Size = new System.Drawing.Size(661, 498);
            this.panelDictionary.TabIndex = 11;
            this.panelDictionary.Visible = false;
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.btnChangeDictSet);
            this.groupBox9.Controls.Add(this.btnDictStopService);
            this.groupBox9.Controls.Add(this.btnDictStartService);
            this.groupBox9.Controls.Add(this.btnDictService);
            this.groupBox9.Controls.Add(this.btnDictConfirm);
            this.groupBox9.Location = new System.Drawing.Point(19, 16);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(448, 102);
            this.groupBox9.TabIndex = 12;
            this.groupBox9.TabStop = false;
            // 
            // btnChangeDictSet
            // 
            this.btnChangeDictSet.ImageIndex = 4;
            this.btnChangeDictSet.ImageList = this.imageListButtons;
            this.btnChangeDictSet.Location = new System.Drawing.Point(2, 9);
            this.btnChangeDictSet.Name = "btnChangeDictSet";
            this.btnChangeDictSet.Size = new System.Drawing.Size(89, 90);
            this.btnChangeDictSet.TabIndex = 3;
            this.btnChangeDictSet.Text = "修改设置";
            this.btnChangeDictSet.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnChangeDictSet.UseVisualStyleBackColor = true;
            this.btnChangeDictSet.Click += new System.EventHandler(this.btnChangeDictSet_Click);
            // 
            // btnDictStopService
            // 
            this.btnDictStopService.ImageIndex = 11;
            this.btnDictStopService.ImageList = this.imageListButtons;
            this.btnDictStopService.Location = new System.Drawing.Point(358, 9);
            this.btnDictStopService.Name = "btnDictStopService";
            this.btnDictStopService.Size = new System.Drawing.Size(89, 90);
            this.btnDictStopService.TabIndex = 3;
            this.btnDictStopService.Text = "停止服务";
            this.btnDictStopService.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnDictStopService.UseVisualStyleBackColor = true;
            this.btnDictStopService.Click += new System.EventHandler(this.btnDictStopService_Click);
            // 
            // btnDictStartService
            // 
            this.btnDictStartService.ImageIndex = 10;
            this.btnDictStartService.ImageList = this.imageListButtons;
            this.btnDictStartService.Location = new System.Drawing.Point(269, 9);
            this.btnDictStartService.Name = "btnDictStartService";
            this.btnDictStartService.Size = new System.Drawing.Size(89, 90);
            this.btnDictStartService.TabIndex = 3;
            this.btnDictStartService.Text = "开始服务";
            this.btnDictStartService.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnDictStartService.UseVisualStyleBackColor = true;
            this.btnDictStartService.Click += new System.EventHandler(this.btnDictStartService_Click);
            // 
            // btnDictService
            // 
            this.btnDictService.ImageIndex = 9;
            this.btnDictService.ImageList = this.imageListButtons;
            this.btnDictService.Location = new System.Drawing.Point(180, 9);
            this.btnDictService.Name = "btnDictService";
            this.btnDictService.Size = new System.Drawing.Size(89, 90);
            this.btnDictService.TabIndex = 3;
            this.btnDictService.Text = "设置系统服务";
            this.btnDictService.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnDictService.UseVisualStyleBackColor = true;
            this.btnDictService.Click += new System.EventHandler(this.btnDictService_Click);
            // 
            // btnDictConfirm
            // 
            this.btnDictConfirm.Enabled = false;
            this.btnDictConfirm.ImageIndex = 6;
            this.btnDictConfirm.ImageList = this.imageListButtons;
            this.btnDictConfirm.Location = new System.Drawing.Point(91, 9);
            this.btnDictConfirm.Name = "btnDictConfirm";
            this.btnDictConfirm.Size = new System.Drawing.Size(89, 90);
            this.btnDictConfirm.TabIndex = 3;
            this.btnDictConfirm.Text = "确定操作";
            this.btnDictConfirm.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnDictConfirm.UseVisualStyleBackColor = true;
            this.btnDictConfirm.Click += new System.EventHandler(this.btnDictConfirm_Click);
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.btnSetCustomPaths);
            this.groupBox10.Controls.Add(this.btnSetFilterPath);
            this.groupBox10.Controls.Add(this.btnSetNumberPath);
            this.groupBox10.Controls.Add(this.btnSetNamePath);
            this.groupBox10.Controls.Add(this.btnSetBasePath);
            this.groupBox10.Controls.Add(this.textBoxFilterPath);
            this.groupBox10.Controls.Add(this.textBoxNumberPath);
            this.groupBox10.Controls.Add(this.textBoxNamePath);
            this.groupBox10.Controls.Add(this.textBoxBasePath);
            this.groupBox10.Controls.Add(this.listBoxCustomPaths);
            this.groupBox10.Controls.Add(this.label37);
            this.groupBox10.Controls.Add(this.label38);
            this.groupBox10.Controls.Add(this.label40);
            this.groupBox10.Controls.Add(this.label43);
            this.groupBox10.Controls.Add(this.label44);
            this.groupBox10.Location = new System.Drawing.Point(21, 140);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(620, 297);
            this.groupBox10.TabIndex = 11;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "词库设置：";
            // 
            // btnSetCustomPaths
            // 
            this.btnSetCustomPaths.Location = new System.Drawing.Point(558, 111);
            this.btnSetCustomPaths.Name = "btnSetCustomPaths";
            this.btnSetCustomPaths.Size = new System.Drawing.Size(33, 23);
            this.btnSetCustomPaths.TabIndex = 4;
            this.btnSetCustomPaths.Text = "...";
            this.btnSetCustomPaths.UseVisualStyleBackColor = true;
            this.btnSetCustomPaths.Click += new System.EventHandler(this.btnSetCustomPaths_Click);
            // 
            // btnSetFilterPath
            // 
            this.btnSetFilterPath.Location = new System.Drawing.Point(558, 260);
            this.btnSetFilterPath.Name = "btnSetFilterPath";
            this.btnSetFilterPath.Size = new System.Drawing.Size(33, 23);
            this.btnSetFilterPath.TabIndex = 4;
            this.btnSetFilterPath.Text = "...";
            this.btnSetFilterPath.UseVisualStyleBackColor = true;
            this.btnSetFilterPath.Click += new System.EventHandler(this.btnSetFilterPath_Click);
            // 
            // btnSetNumberPath
            // 
            this.btnSetNumberPath.Location = new System.Drawing.Point(558, 80);
            this.btnSetNumberPath.Name = "btnSetNumberPath";
            this.btnSetNumberPath.Size = new System.Drawing.Size(33, 23);
            this.btnSetNumberPath.TabIndex = 4;
            this.btnSetNumberPath.Text = "...";
            this.btnSetNumberPath.UseVisualStyleBackColor = true;
            this.btnSetNumberPath.Click += new System.EventHandler(this.btnSetNumberPath_Click);
            // 
            // btnSetNamePath
            // 
            this.btnSetNamePath.Location = new System.Drawing.Point(558, 52);
            this.btnSetNamePath.Name = "btnSetNamePath";
            this.btnSetNamePath.Size = new System.Drawing.Size(33, 23);
            this.btnSetNamePath.TabIndex = 4;
            this.btnSetNamePath.Text = "...";
            this.btnSetNamePath.UseVisualStyleBackColor = true;
            this.btnSetNamePath.Click += new System.EventHandler(this.btnSetNamePath_Click);
            // 
            // btnSetBasePath
            // 
            this.btnSetBasePath.Location = new System.Drawing.Point(558, 24);
            this.btnSetBasePath.Name = "btnSetBasePath";
            this.btnSetBasePath.Size = new System.Drawing.Size(33, 23);
            this.btnSetBasePath.TabIndex = 4;
            this.btnSetBasePath.Text = "...";
            this.btnSetBasePath.UseVisualStyleBackColor = true;
            this.btnSetBasePath.Click += new System.EventHandler(this.btnSetBasePath_Click);
            // 
            // textBoxFilterPath
            // 
            this.textBoxFilterPath.Location = new System.Drawing.Point(143, 261);
            this.textBoxFilterPath.Name = "textBoxFilterPath";
            this.textBoxFilterPath.Size = new System.Drawing.Size(414, 21);
            this.textBoxFilterPath.TabIndex = 3;
            // 
            // textBoxNumberPath
            // 
            this.textBoxNumberPath.Location = new System.Drawing.Point(143, 81);
            this.textBoxNumberPath.Name = "textBoxNumberPath";
            this.textBoxNumberPath.Size = new System.Drawing.Size(414, 21);
            this.textBoxNumberPath.TabIndex = 3;
            // 
            // textBoxNamePath
            // 
            this.textBoxNamePath.Location = new System.Drawing.Point(143, 53);
            this.textBoxNamePath.Name = "textBoxNamePath";
            this.textBoxNamePath.Size = new System.Drawing.Size(414, 21);
            this.textBoxNamePath.TabIndex = 3;
            // 
            // textBoxBasePath
            // 
            this.textBoxBasePath.Location = new System.Drawing.Point(143, 25);
            this.textBoxBasePath.Name = "textBoxBasePath";
            this.textBoxBasePath.Size = new System.Drawing.Size(414, 21);
            this.textBoxBasePath.TabIndex = 3;
            // 
            // listBoxCustomPaths
            // 
            this.listBoxCustomPaths.FormattingEnabled = true;
            this.listBoxCustomPaths.ItemHeight = 12;
            this.listBoxCustomPaths.Location = new System.Drawing.Point(143, 113);
            this.listBoxCustomPaths.Name = "listBoxCustomPaths";
            this.listBoxCustomPaths.Size = new System.Drawing.Size(414, 136);
            this.listBoxCustomPaths.TabIndex = 2;
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(24, 265);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(89, 12);
            this.label37.TabIndex = 1;
            this.label37.Text = "过滤词库路径：";
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(22, 113);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(101, 12);
            this.label38.TabIndex = 1;
            this.label38.Text = "自定义词库路径：";
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(24, 85);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(89, 12);
            this.label40.TabIndex = 1;
            this.label40.Text = "数值词库路径：";
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Location = new System.Drawing.Point(22, 57);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(89, 12);
            this.label43.TabIndex = 1;
            this.label43.Text = "姓氏词库路径：";
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Location = new System.Drawing.Point(24, 29);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(77, 12);
            this.label44.TabIndex = 0;
            this.label44.Text = "主词库路径：";
            // 
            // panelIndexSet
            // 
            this.panelIndexSet.Controls.Add(this.groupBox3);
            this.panelIndexSet.Controls.Add(this.groupBox4);
            this.panelIndexSet.Controls.Add(this.label18);
            this.panelIndexSet.Controls.Add(this.comboBoxIndex);
            this.panelIndexSet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelIndexSet.Location = new System.Drawing.Point(0, 0);
            this.panelIndexSet.Name = "panelIndexSet";
            this.panelIndexSet.Size = new System.Drawing.Size(661, 498);
            this.panelIndexSet.TabIndex = 5;
            this.panelIndexSet.Visible = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnDelIndex);
            this.groupBox3.Controls.Add(this.btnEditIndex);
            this.groupBox3.Controls.Add(this.btnAddIndex);
            this.groupBox3.Controls.Add(this.btnIndexCancel);
            this.groupBox3.Controls.Add(this.btnIndexConfirm);
            this.groupBox3.Location = new System.Drawing.Point(13, 23);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(533, 112);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            // 
            // btnDelIndex
            // 
            this.btnDelIndex.ImageIndex = 5;
            this.btnDelIndex.ImageList = this.imageListButtons;
            this.btnDelIndex.Location = new System.Drawing.Point(215, 9);
            this.btnDelIndex.Name = "btnDelIndex";
            this.btnDelIndex.Size = new System.Drawing.Size(103, 99);
            this.btnDelIndex.TabIndex = 3;
            this.btnDelIndex.Text = "删除索引";
            this.btnDelIndex.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnDelIndex.UseVisualStyleBackColor = true;
            this.btnDelIndex.Click += new System.EventHandler(this.btnDelIndex_Click);
            // 
            // btnEditIndex
            // 
            this.btnEditIndex.ImageIndex = 4;
            this.btnEditIndex.ImageList = this.imageListButtons;
            this.btnEditIndex.Location = new System.Drawing.Point(108, 9);
            this.btnEditIndex.Name = "btnEditIndex";
            this.btnEditIndex.Size = new System.Drawing.Size(103, 99);
            this.btnEditIndex.TabIndex = 3;
            this.btnEditIndex.Text = "修改索引";
            this.btnEditIndex.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnEditIndex.UseVisualStyleBackColor = true;
            this.btnEditIndex.Click += new System.EventHandler(this.btnEditIndex_Click);
            // 
            // btnAddIndex
            // 
            this.btnAddIndex.ImageIndex = 3;
            this.btnAddIndex.ImageList = this.imageListButtons;
            this.btnAddIndex.Location = new System.Drawing.Point(1, 9);
            this.btnAddIndex.Name = "btnAddIndex";
            this.btnAddIndex.Size = new System.Drawing.Size(103, 99);
            this.btnAddIndex.TabIndex = 3;
            this.btnAddIndex.Text = "添加索引";
            this.btnAddIndex.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnAddIndex.UseVisualStyleBackColor = true;
            this.btnAddIndex.Click += new System.EventHandler(this.btnAddIndex_Click);
            // 
            // btnIndexCancel
            // 
            this.btnIndexCancel.Enabled = false;
            this.btnIndexCancel.ImageIndex = 7;
            this.btnIndexCancel.ImageList = this.imageListButtons;
            this.btnIndexCancel.Location = new System.Drawing.Point(429, 9);
            this.btnIndexCancel.Name = "btnIndexCancel";
            this.btnIndexCancel.Size = new System.Drawing.Size(103, 99);
            this.btnIndexCancel.TabIndex = 3;
            this.btnIndexCancel.Text = "取消操作";
            this.btnIndexCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnIndexCancel.UseVisualStyleBackColor = true;
            this.btnIndexCancel.Click += new System.EventHandler(this.btnIndexCancel_Click);
            // 
            // btnIndexConfirm
            // 
            this.btnIndexConfirm.Enabled = false;
            this.btnIndexConfirm.ImageIndex = 6;
            this.btnIndexConfirm.ImageList = this.imageListButtons;
            this.btnIndexConfirm.Location = new System.Drawing.Point(324, 9);
            this.btnIndexConfirm.Name = "btnIndexConfirm";
            this.btnIndexConfirm.Size = new System.Drawing.Size(103, 99);
            this.btnIndexConfirm.TabIndex = 3;
            this.btnIndexConfirm.Text = "确定操作";
            this.btnIndexConfirm.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnIndexConfirm.UseVisualStyleBackColor = true;
            this.btnIndexConfirm.Click += new System.EventHandler(this.btnIndexConfirm_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnSetIndexPath);
            this.groupBox4.Controls.Add(this.comboBoxSouceSel);
            this.groupBox4.Controls.Add(this.comboBoxIndexType);
            this.groupBox4.Controls.Add(this.textBoxIndexPath);
            this.groupBox4.Controls.Add(this.textBoxIndexName);
            this.groupBox4.Controls.Add(this.label13);
            this.groupBox4.Controls.Add(this.label15);
            this.groupBox4.Controls.Add(this.label16);
            this.groupBox4.Controls.Add(this.label17);
            this.groupBox4.Location = new System.Drawing.Point(13, 212);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(638, 228);
            this.groupBox4.TabIndex = 8;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "数据源信息：";
            // 
            // btnSetIndexPath
            // 
            this.btnSetIndexPath.Location = new System.Drawing.Point(582, 184);
            this.btnSetIndexPath.Name = "btnSetIndexPath";
            this.btnSetIndexPath.Size = new System.Drawing.Size(34, 23);
            this.btnSetIndexPath.TabIndex = 10;
            this.btnSetIndexPath.Text = "...";
            this.btnSetIndexPath.UseVisualStyleBackColor = true;
            this.btnSetIndexPath.Click += new System.EventHandler(this.btnSetIndexPath_Click);
            // 
            // comboBoxSouceSel
            // 
            this.comboBoxSouceSel.FormattingEnabled = true;
            this.comboBoxSouceSel.Location = new System.Drawing.Point(89, 133);
            this.comboBoxSouceSel.Name = "comboBoxSouceSel";
            this.comboBoxSouceSel.Size = new System.Drawing.Size(335, 20);
            this.comboBoxSouceSel.TabIndex = 9;
            // 
            // comboBoxIndexType
            // 
            this.comboBoxIndexType.FormattingEnabled = true;
            this.comboBoxIndexType.Items.AddRange(new object[] {
            "Increment",
            "Ordinary"});
            this.comboBoxIndexType.Location = new System.Drawing.Point(89, 80);
            this.comboBoxIndexType.Name = "comboBoxIndexType";
            this.comboBoxIndexType.Size = new System.Drawing.Size(335, 20);
            this.comboBoxIndexType.TabIndex = 9;
            // 
            // textBoxIndexPath
            // 
            this.textBoxIndexPath.Location = new System.Drawing.Point(89, 186);
            this.textBoxIndexPath.Name = "textBoxIndexPath";
            this.textBoxIndexPath.Size = new System.Drawing.Size(487, 21);
            this.textBoxIndexPath.TabIndex = 8;
            // 
            // textBoxIndexName
            // 
            this.textBoxIndexName.Location = new System.Drawing.Point(89, 26);
            this.textBoxIndexName.Name = "textBoxIndexName";
            this.textBoxIndexName.Size = new System.Drawing.Size(335, 21);
            this.textBoxIndexName.TabIndex = 8;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(6, 189);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(65, 12);
            this.label13.TabIndex = 4;
            this.label13.Text = "索引位置：";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.SystemColors.Control;
            this.label15.Location = new System.Drawing.Point(6, 136);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(77, 12);
            this.label15.TabIndex = 2;
            this.label15.Text = "索引数据源：";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(6, 83);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(65, 12);
            this.label16.TabIndex = 1;
            this.label16.Text = "索引类型：";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(6, 30);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(65, 12);
            this.label17.TabIndex = 0;
            this.label17.Text = "索引名称：";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(93, 171);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(65, 12);
            this.label18.TabIndex = 7;
            this.label18.Text = "索引列表：";
            // 
            // comboBoxIndex
            // 
            this.comboBoxIndex.FormattingEnabled = true;
            this.comboBoxIndex.Location = new System.Drawing.Point(164, 168);
            this.comboBoxIndex.Name = "comboBoxIndex";
            this.comboBoxIndex.Size = new System.Drawing.Size(257, 20);
            this.comboBoxIndex.TabIndex = 6;
            this.comboBoxIndex.SelectedIndexChanged += new System.EventHandler(this.comboBoxIndex_SelectedIndexChanged);
            // 
            // panelIndexerSet
            // 
            this.panelIndexerSet.Controls.Add(this.groupBox6);
            this.panelIndexerSet.Controls.Add(this.groupBox5);
            this.panelIndexerSet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelIndexerSet.Location = new System.Drawing.Point(0, 0);
            this.panelIndexerSet.Name = "panelIndexerSet";
            this.panelIndexerSet.Size = new System.Drawing.Size(661, 498);
            this.panelIndexerSet.TabIndex = 3;
            this.panelIndexerSet.Visible = false;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.btnEditIndexer);
            this.groupBox6.Controls.Add(this.btnReCreateIncrIndex);
            this.groupBox6.Controls.Add(this.btnMainIndexReCreate);
            this.groupBox6.Controls.Add(this.btnIndexerStopService);
            this.groupBox6.Controls.Add(this.btnIndexerStartService);
            this.groupBox6.Controls.Add(this.btnIndexerService);
            this.groupBox6.Controls.Add(this.btnIndexerConfirm);
            this.groupBox6.Location = new System.Drawing.Point(13, 19);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(622, 102);
            this.groupBox6.TabIndex = 10;
            this.groupBox6.TabStop = false;
            // 
            // btnEditIndexer
            // 
            this.btnEditIndexer.ImageIndex = 4;
            this.btnEditIndexer.ImageList = this.imageListButtons;
            this.btnEditIndexer.Location = new System.Drawing.Point(178, 9);
            this.btnEditIndexer.Name = "btnEditIndexer";
            this.btnEditIndexer.Size = new System.Drawing.Size(89, 90);
            this.btnEditIndexer.TabIndex = 3;
            this.btnEditIndexer.Text = "修改设置";
            this.btnEditIndexer.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnEditIndexer.UseVisualStyleBackColor = true;
            this.btnEditIndexer.Click += new System.EventHandler(this.btnEditIndexer_Click);
            // 
            // btnReCreateIncrIndex
            // 
            this.btnReCreateIncrIndex.ImageIndex = 3;
            this.btnReCreateIncrIndex.ImageList = this.imageListButtons;
            this.btnReCreateIncrIndex.Location = new System.Drawing.Point(89, 9);
            this.btnReCreateIncrIndex.Name = "btnReCreateIncrIndex";
            this.btnReCreateIncrIndex.Size = new System.Drawing.Size(89, 90);
            this.btnReCreateIncrIndex.TabIndex = 3;
            this.btnReCreateIncrIndex.Text = "重建增量索引";
            this.btnReCreateIncrIndex.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnReCreateIncrIndex.UseVisualStyleBackColor = true;
            this.btnReCreateIncrIndex.Click += new System.EventHandler(this.btnReCreateIncrIndex_Click);
            // 
            // btnMainIndexReCreate
            // 
            this.btnMainIndexReCreate.ImageIndex = 8;
            this.btnMainIndexReCreate.ImageList = this.imageListButtons;
            this.btnMainIndexReCreate.Location = new System.Drawing.Point(0, 9);
            this.btnMainIndexReCreate.Name = "btnMainIndexReCreate";
            this.btnMainIndexReCreate.Size = new System.Drawing.Size(89, 90);
            this.btnMainIndexReCreate.TabIndex = 2;
            this.btnMainIndexReCreate.Text = "重新主索引";
            this.btnMainIndexReCreate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnMainIndexReCreate.UseVisualStyleBackColor = true;
            this.btnMainIndexReCreate.Click += new System.EventHandler(this.btnMainIndexReCreate_Click);
            // 
            // btnIndexerStopService
            // 
            this.btnIndexerStopService.ImageIndex = 11;
            this.btnIndexerStopService.ImageList = this.imageListButtons;
            this.btnIndexerStopService.Location = new System.Drawing.Point(534, 9);
            this.btnIndexerStopService.Name = "btnIndexerStopService";
            this.btnIndexerStopService.Size = new System.Drawing.Size(89, 90);
            this.btnIndexerStopService.TabIndex = 3;
            this.btnIndexerStopService.Text = "停止服务";
            this.btnIndexerStopService.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnIndexerStopService.UseVisualStyleBackColor = true;
            this.btnIndexerStopService.Click += new System.EventHandler(this.btnIndexerStopService_Click);
            // 
            // btnIndexerStartService
            // 
            this.btnIndexerStartService.ImageIndex = 10;
            this.btnIndexerStartService.ImageList = this.imageListButtons;
            this.btnIndexerStartService.Location = new System.Drawing.Point(445, 9);
            this.btnIndexerStartService.Name = "btnIndexerStartService";
            this.btnIndexerStartService.Size = new System.Drawing.Size(89, 90);
            this.btnIndexerStartService.TabIndex = 3;
            this.btnIndexerStartService.Text = "开始服务";
            this.btnIndexerStartService.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnIndexerStartService.UseVisualStyleBackColor = true;
            this.btnIndexerStartService.Click += new System.EventHandler(this.btnIndexerStartService_Click);
            // 
            // btnIndexerService
            // 
            this.btnIndexerService.ImageIndex = 9;
            this.btnIndexerService.ImageList = this.imageListButtons;
            this.btnIndexerService.Location = new System.Drawing.Point(356, 9);
            this.btnIndexerService.Name = "btnIndexerService";
            this.btnIndexerService.Size = new System.Drawing.Size(89, 90);
            this.btnIndexerService.TabIndex = 3;
            this.btnIndexerService.Text = "设置系统服务";
            this.btnIndexerService.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnIndexerService.UseVisualStyleBackColor = true;
            this.btnIndexerService.Click += new System.EventHandler(this.btnIndexerService_Click);
            // 
            // btnIndexerConfirm
            // 
            this.btnIndexerConfirm.Enabled = false;
            this.btnIndexerConfirm.ImageIndex = 6;
            this.btnIndexerConfirm.ImageList = this.imageListButtons;
            this.btnIndexerConfirm.Location = new System.Drawing.Point(267, 9);
            this.btnIndexerConfirm.Name = "btnIndexerConfirm";
            this.btnIndexerConfirm.Size = new System.Drawing.Size(89, 90);
            this.btnIndexerConfirm.TabIndex = 3;
            this.btnIndexerConfirm.Text = "确定操作";
            this.btnIndexerConfirm.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnIndexerConfirm.UseVisualStyleBackColor = true;
            this.btnIndexerConfirm.Click += new System.EventHandler(this.btnIndexerConfirm_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.numericUpDownMergeFactor);
            this.groupBox5.Controls.Add(this.numericUpDownMaxBufferedDocs);
            this.groupBox5.Controls.Add(this.numericUpDownMaxFieldLength);
            this.groupBox5.Controls.Add(this.numericUpDownRamBufferSize);
            this.groupBox5.Controls.Add(this.numericUpDownIncrTimeSpan);
            this.groupBox5.Controls.Add(this.numericUpDownMainCreateTimeSpan);
            this.groupBox5.Controls.Add(this.dateTimePickerMainReCreate);
            this.groupBox5.Controls.Add(this.label22);
            this.groupBox5.Controls.Add(this.label21);
            this.groupBox5.Controls.Add(this.label20);
            this.groupBox5.Controls.Add(this.label19);
            this.groupBox5.Controls.Add(this.label24);
            this.groupBox5.Controls.Add(this.label14);
            this.groupBox5.Controls.Add(this.label23);
            this.groupBox5.Controls.Add(this.label12);
            this.groupBox5.Controls.Add(this.label11);
            this.groupBox5.Controls.Add(this.label10);
            this.groupBox5.Location = new System.Drawing.Point(15, 143);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(620, 297);
            this.groupBox5.TabIndex = 0;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "索引器设置：";
            // 
            // numericUpDownMergeFactor
            // 
            this.numericUpDownMergeFactor.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownMergeFactor.Location = new System.Drawing.Point(334, 259);
            this.numericUpDownMergeFactor.Maximum = new decimal(new int[] {
            20000,
            0,
            0,
            0});
            this.numericUpDownMergeFactor.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownMergeFactor.Name = "numericUpDownMergeFactor";
            this.numericUpDownMergeFactor.Size = new System.Drawing.Size(154, 21);
            this.numericUpDownMergeFactor.TabIndex = 4;
            this.numericUpDownMergeFactor.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // numericUpDownMaxBufferedDocs
            // 
            this.numericUpDownMaxBufferedDocs.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownMaxBufferedDocs.Location = new System.Drawing.Point(334, 220);
            this.numericUpDownMaxBufferedDocs.Maximum = new decimal(new int[] {
            20000,
            0,
            0,
            0});
            this.numericUpDownMaxBufferedDocs.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownMaxBufferedDocs.Name = "numericUpDownMaxBufferedDocs";
            this.numericUpDownMaxBufferedDocs.Size = new System.Drawing.Size(154, 21);
            this.numericUpDownMaxBufferedDocs.TabIndex = 4;
            this.numericUpDownMaxBufferedDocs.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // numericUpDownMaxFieldLength
            // 
            this.numericUpDownMaxFieldLength.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownMaxFieldLength.Location = new System.Drawing.Point(334, 181);
            this.numericUpDownMaxFieldLength.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numericUpDownMaxFieldLength.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownMaxFieldLength.Name = "numericUpDownMaxFieldLength";
            this.numericUpDownMaxFieldLength.Size = new System.Drawing.Size(154, 21);
            this.numericUpDownMaxFieldLength.TabIndex = 4;
            this.numericUpDownMaxFieldLength.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // numericUpDownRamBufferSize
            // 
            this.numericUpDownRamBufferSize.Location = new System.Drawing.Point(334, 142);
            this.numericUpDownRamBufferSize.Maximum = new decimal(new int[] {
            768,
            0,
            0,
            0});
            this.numericUpDownRamBufferSize.Minimum = new decimal(new int[] {
            64,
            0,
            0,
            0});
            this.numericUpDownRamBufferSize.Name = "numericUpDownRamBufferSize";
            this.numericUpDownRamBufferSize.Size = new System.Drawing.Size(154, 21);
            this.numericUpDownRamBufferSize.TabIndex = 4;
            this.numericUpDownRamBufferSize.Value = new decimal(new int[] {
            512,
            0,
            0,
            0});
            // 
            // numericUpDownIncrTimeSpan
            // 
            this.numericUpDownIncrTimeSpan.Location = new System.Drawing.Point(334, 103);
            this.numericUpDownIncrTimeSpan.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.numericUpDownIncrTimeSpan.Minimum = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.numericUpDownIncrTimeSpan.Name = "numericUpDownIncrTimeSpan";
            this.numericUpDownIncrTimeSpan.Size = new System.Drawing.Size(154, 21);
            this.numericUpDownIncrTimeSpan.TabIndex = 4;
            this.numericUpDownIncrTimeSpan.Value = new decimal(new int[] {
            120,
            0,
            0,
            0});
            // 
            // numericUpDownMainCreateTimeSpan
            // 
            this.numericUpDownMainCreateTimeSpan.Location = new System.Drawing.Point(334, 64);
            this.numericUpDownMainCreateTimeSpan.Maximum = new decimal(new int[] {
            7,
            0,
            0,
            0});
            this.numericUpDownMainCreateTimeSpan.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownMainCreateTimeSpan.Name = "numericUpDownMainCreateTimeSpan";
            this.numericUpDownMainCreateTimeSpan.Size = new System.Drawing.Size(154, 21);
            this.numericUpDownMainCreateTimeSpan.TabIndex = 4;
            this.numericUpDownMainCreateTimeSpan.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // dateTimePickerMainReCreate
            // 
            this.dateTimePickerMainReCreate.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePickerMainReCreate.Location = new System.Drawing.Point(334, 25);
            this.dateTimePickerMainReCreate.Name = "dateTimePickerMainReCreate";
            this.dateTimePickerMainReCreate.RightToLeftLayout = true;
            this.dateTimePickerMainReCreate.ShowUpDown = true;
            this.dateTimePickerMainReCreate.Size = new System.Drawing.Size(154, 21);
            this.dateTimePickerMainReCreate.TabIndex = 3;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(68, 263);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(89, 12);
            this.label22.TabIndex = 1;
            this.label22.Text = "文档合并因子：";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(68, 224);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(113, 12);
            this.label21.TabIndex = 1;
            this.label21.Text = "内存文档存储限额：";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(68, 185);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(113, 12);
            this.label20.TabIndex = 1;
            this.label20.Text = "索引字段长度限制：";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(68, 146);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(113, 12);
            this.label19.TabIndex = 1;
            this.label19.Text = "允许使用内存容量：";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(496, 146);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(17, 12);
            this.label24.TabIndex = 1;
            this.label24.Text = "MB";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(68, 107);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(113, 12);
            this.label14.TabIndex = 1;
            this.label14.Text = "增量索引重建间隔：";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(496, 107);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(17, 12);
            this.label23.TabIndex = 1;
            this.label23.Text = "秒";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(496, 68);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(17, 12);
            this.label12.TabIndex = 1;
            this.label12.Text = "天";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(68, 68);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(101, 12);
            this.label11.TabIndex = 1;
            this.label11.Text = "主索引重建间隔：";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(68, 29);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(101, 12);
            this.label10.TabIndex = 0;
            this.label10.Text = "主索引重建时间：";
            // 
            // panelSearchd
            // 
            this.panelSearchd.Controls.Add(this.groupBox7);
            this.panelSearchd.Controls.Add(this.groupBox8);
            this.panelSearchd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSearchd.Location = new System.Drawing.Point(0, 0);
            this.panelSearchd.Name = "panelSearchd";
            this.panelSearchd.Size = new System.Drawing.Size(661, 498);
            this.panelSearchd.TabIndex = 2;
            this.panelSearchd.Visible = false;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.btnSearchdStopService);
            this.groupBox7.Controls.Add(this.btnSearchdStartService);
            this.groupBox7.Controls.Add(this.btnEditSearchd);
            this.groupBox7.Controls.Add(this.btnSearchdService);
            this.groupBox7.Controls.Add(this.btnSearchdConfirm);
            this.groupBox7.Location = new System.Drawing.Point(27, 25);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(519, 108);
            this.groupBox7.TabIndex = 12;
            this.groupBox7.TabStop = false;
            // 
            // btnSearchdStopService
            // 
            this.btnSearchdStopService.ImageIndex = 11;
            this.btnSearchdStopService.ImageList = this.imageListButtons;
            this.btnSearchdStopService.Location = new System.Drawing.Point(412, 9);
            this.btnSearchdStopService.Name = "btnSearchdStopService";
            this.btnSearchdStopService.Size = new System.Drawing.Size(103, 99);
            this.btnSearchdStopService.TabIndex = 7;
            this.btnSearchdStopService.Text = "停止服务";
            this.btnSearchdStopService.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnSearchdStopService.UseVisualStyleBackColor = true;
            this.btnSearchdStopService.Click += new System.EventHandler(this.btnSearchdStopService_Click);
            // 
            // btnSearchdStartService
            // 
            this.btnSearchdStartService.ImageIndex = 10;
            this.btnSearchdStartService.ImageList = this.imageListButtons;
            this.btnSearchdStartService.Location = new System.Drawing.Point(309, 9);
            this.btnSearchdStartService.Name = "btnSearchdStartService";
            this.btnSearchdStartService.Size = new System.Drawing.Size(103, 99);
            this.btnSearchdStartService.TabIndex = 6;
            this.btnSearchdStartService.Text = "开始服务";
            this.btnSearchdStartService.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnSearchdStartService.UseVisualStyleBackColor = true;
            this.btnSearchdStartService.Click += new System.EventHandler(this.btnSearchdStartService_Click);
            // 
            // btnEditSearchd
            // 
            this.btnEditSearchd.ImageIndex = 4;
            this.btnEditSearchd.ImageList = this.imageListButtons;
            this.btnEditSearchd.Location = new System.Drawing.Point(0, 8);
            this.btnEditSearchd.Name = "btnEditSearchd";
            this.btnEditSearchd.Size = new System.Drawing.Size(103, 99);
            this.btnEditSearchd.TabIndex = 3;
            this.btnEditSearchd.Text = "修改设置";
            this.btnEditSearchd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnEditSearchd.UseVisualStyleBackColor = true;
            this.btnEditSearchd.Click += new System.EventHandler(this.btnEditSearchd_Click);
            // 
            // btnSearchdService
            // 
            this.btnSearchdService.ImageIndex = 9;
            this.btnSearchdService.ImageList = this.imageListButtons;
            this.btnSearchdService.Location = new System.Drawing.Point(206, 8);
            this.btnSearchdService.Name = "btnSearchdService";
            this.btnSearchdService.Size = new System.Drawing.Size(103, 99);
            this.btnSearchdService.TabIndex = 3;
            this.btnSearchdService.Text = "设置系统服务";
            this.btnSearchdService.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnSearchdService.UseVisualStyleBackColor = true;
            this.btnSearchdService.Click += new System.EventHandler(this.btnSearchdService_Click);
            // 
            // btnSearchdConfirm
            // 
            this.btnSearchdConfirm.Enabled = false;
            this.btnSearchdConfirm.ImageIndex = 6;
            this.btnSearchdConfirm.ImageList = this.imageListButtons;
            this.btnSearchdConfirm.Location = new System.Drawing.Point(103, 8);
            this.btnSearchdConfirm.Name = "btnSearchdConfirm";
            this.btnSearchdConfirm.Size = new System.Drawing.Size(103, 99);
            this.btnSearchdConfirm.TabIndex = 3;
            this.btnSearchdConfirm.Text = "确定操作";
            this.btnSearchdConfirm.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnSearchdConfirm.UseVisualStyleBackColor = true;
            this.btnSearchdConfirm.Click += new System.EventHandler(this.btnSearchdConfirm_Click);
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.btnSetQueryPath);
            this.groupBox8.Controls.Add(this.textBoxQueryPath);
            this.groupBox8.Controls.Add(this.btnSetSearchdPath);
            this.groupBox8.Controls.Add(this.textBoxSearchdPath);
            this.groupBox8.Controls.Add(this.maskedTextBoxIpAddress);
            this.groupBox8.Controls.Add(this.numericUpDownMaxTransport);
            this.groupBox8.Controls.Add(this.numericUpDownMaxMatches);
            this.groupBox8.Controls.Add(this.numericUpDownPort);
            this.groupBox8.Controls.Add(this.numericUpDownTimeOut);
            this.groupBox8.Controls.Add(this.label25);
            this.groupBox8.Controls.Add(this.label26);
            this.groupBox8.Controls.Add(this.label27);
            this.groupBox8.Controls.Add(this.label28);
            this.groupBox8.Controls.Add(this.label30);
            this.groupBox8.Controls.Add(this.label33);
            this.groupBox8.Controls.Add(this.label34);
            this.groupBox8.Location = new System.Drawing.Point(27, 151);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(519, 297);
            this.groupBox8.TabIndex = 11;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "搜索器设置";
            // 
            // btnSetQueryPath
            // 
            this.btnSetQueryPath.Location = new System.Drawing.Point(468, 258);
            this.btnSetQueryPath.Name = "btnSetQueryPath";
            this.btnSetQueryPath.Size = new System.Drawing.Size(35, 23);
            this.btnSetQueryPath.TabIndex = 7;
            this.btnSetQueryPath.Text = "...";
            this.btnSetQueryPath.UseVisualStyleBackColor = true;
            this.btnSetQueryPath.Click += new System.EventHandler(this.btnSetQueryPath_Click);
            // 
            // textBoxQueryPath
            // 
            this.textBoxQueryPath.Location = new System.Drawing.Point(102, 259);
            this.textBoxQueryPath.Name = "textBoxQueryPath";
            this.textBoxQueryPath.Size = new System.Drawing.Size(366, 21);
            this.textBoxQueryPath.TabIndex = 6;
            // 
            // btnSetSearchdPath
            // 
            this.btnSetSearchdPath.Location = new System.Drawing.Point(468, 219);
            this.btnSetSearchdPath.Name = "btnSetSearchdPath";
            this.btnSetSearchdPath.Size = new System.Drawing.Size(35, 23);
            this.btnSetSearchdPath.TabIndex = 7;
            this.btnSetSearchdPath.Text = "...";
            this.btnSetSearchdPath.UseVisualStyleBackColor = true;
            this.btnSetSearchdPath.Click += new System.EventHandler(this.btnSetSearchdPath_Click);
            // 
            // textBoxSearchdPath
            // 
            this.textBoxSearchdPath.Location = new System.Drawing.Point(102, 220);
            this.textBoxSearchdPath.Name = "textBoxSearchdPath";
            this.textBoxSearchdPath.Size = new System.Drawing.Size(366, 21);
            this.textBoxSearchdPath.TabIndex = 6;
            // 
            // maskedTextBoxIpAddress
            // 
            this.maskedTextBoxIpAddress.Location = new System.Drawing.Point(102, 25);
            this.maskedTextBoxIpAddress.Mask = "255.255.255.255";
            this.maskedTextBoxIpAddress.Name = "maskedTextBoxIpAddress";
            this.maskedTextBoxIpAddress.Size = new System.Drawing.Size(154, 21);
            this.maskedTextBoxIpAddress.TabIndex = 5;
            // 
            // numericUpDownMaxTransport
            // 
            this.numericUpDownMaxTransport.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownMaxTransport.Location = new System.Drawing.Point(102, 181);
            this.numericUpDownMaxTransport.Maximum = new decimal(new int[] {
            20000,
            0,
            0,
            0});
            this.numericUpDownMaxTransport.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownMaxTransport.Name = "numericUpDownMaxTransport";
            this.numericUpDownMaxTransport.Size = new System.Drawing.Size(154, 21);
            this.numericUpDownMaxTransport.TabIndex = 4;
            this.numericUpDownMaxTransport.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // numericUpDownMaxMatches
            // 
            this.numericUpDownMaxMatches.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownMaxMatches.Location = new System.Drawing.Point(102, 142);
            this.numericUpDownMaxMatches.Maximum = new decimal(new int[] {
            20000,
            0,
            0,
            0});
            this.numericUpDownMaxMatches.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownMaxMatches.Name = "numericUpDownMaxMatches";
            this.numericUpDownMaxMatches.Size = new System.Drawing.Size(154, 21);
            this.numericUpDownMaxMatches.TabIndex = 4;
            this.numericUpDownMaxMatches.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // numericUpDownPort
            // 
            this.numericUpDownPort.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownPort.Location = new System.Drawing.Point(102, 64);
            this.numericUpDownPort.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.numericUpDownPort.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownPort.Name = "numericUpDownPort";
            this.numericUpDownPort.Size = new System.Drawing.Size(154, 21);
            this.numericUpDownPort.TabIndex = 4;
            this.numericUpDownPort.Value = new decimal(new int[] {
            3312,
            0,
            0,
            0});
            // 
            // numericUpDownTimeOut
            // 
            this.numericUpDownTimeOut.Location = new System.Drawing.Point(102, 103);
            this.numericUpDownTimeOut.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.numericUpDownTimeOut.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownTimeOut.Name = "numericUpDownTimeOut";
            this.numericUpDownTimeOut.Size = new System.Drawing.Size(154, 21);
            this.numericUpDownTimeOut.TabIndex = 4;
            this.numericUpDownTimeOut.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(24, 185);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(65, 12);
            this.label25.TabIndex = 1;
            this.label25.Text = "传输限制：";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(24, 146);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(65, 12);
            this.label26.TabIndex = 1;
            this.label26.Text = "匹配限制：";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(24, 107);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(65, 12);
            this.label27.TabIndex = 1;
            this.label27.Text = "超时设置：";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(24, 263);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(65, 12);
            this.label28.TabIndex = 1;
            this.label28.Text = "查询输出：";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(24, 224);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(65, 12);
            this.label30.TabIndex = 1;
            this.label30.Text = "结果文件：";
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(24, 68);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(65, 12);
            this.label33.TabIndex = 1;
            this.label33.Text = "监听端口：";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(24, 29);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(53, 12);
            this.label34.TabIndex = 0;
            this.label34.Text = "IP地址：";
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 10000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // frmEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(832, 569);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmEditor";
            this.ShowInTaskbar = false;
            this.Text = "数据库索引编辑器";
            this.SizeChanged += new System.EventHandler(this.Form_SizeChanged);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmEditor_FormClosing);
            this.NicontextMenu.ResumeLayout(false);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            this.panelSource.ResumeLayout(false);
            this.panelSource.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panelDictionary.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.panelIndexSet.ResumeLayout(false);
            this.panelIndexSet.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.panelIndexerSet.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMergeFactor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxBufferedDocs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxFieldLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRamBufferSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownIncrTimeSpan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMainCreateTimeSpan)).EndInit();
            this.panelSearchd.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxTransport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxMatches)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimeOut)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        //ServiceController 
        //FolderBrowserDialog
        //SaveFileDialog
        //NotifyIcon
        private System.Windows.Forms.NotifyIcon notifyIcon;
        //ContextMenuStrip
        private System.Windows.Forms.ContextMenuStrip NicontextMenu;
        //ToolStripMenuItem
        private System.Windows.Forms.ToolStripMenuItem menuItem_Hide;
        private System.Windows.Forms.ToolStripMenuItem menuItem_Show;
        private System.Windows.Forms.ToolStripMenuItem menuItem_About;
        private System.Windows.Forms.ToolStripMenuItem menuItem_Exit;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFile;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemOpen;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSave;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSaveAs;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExit;
        //ToolStripSeparator
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        //ToolStripStatusLabel
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelStatus;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelSearchd;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelIndexer;
        //ToolStripProgressBar
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
        //ToolStripButton
        private System.Windows.Forms.ToolStripButton toolStripButtonOpen;
        private System.Windows.Forms.ToolStripButton toolStripButtonSave;
        private System.Windows.Forms.ToolStripButton toolStripButtonSaveAs;
        private System.Windows.Forms.ToolStripButton toolStripButtonExit;
        //ImageList
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ImageList imageListButtons;
        //MenuStrip
        private System.Windows.Forms.MenuStrip menuStrip;
        //ToolStrip
        private System.Windows.Forms.ToolStrip toolStrip;
        //StatusStrip
        private System.Windows.Forms.StatusStrip statusStrip;
        //SplitContainer
        private System.Windows.Forms.SplitContainer splitContainer;
        //ListView
        private System.Windows.Forms.ListView listView;
        //Panel
        private System.Windows.Forms.Panel panelIndexerSet;
        private System.Windows.Forms.Panel panelSearchd;
        private System.Windows.Forms.Panel panelSource;
        private System.Windows.Forms.Panel panelIndexSet;
        private System.Windows.Forms.Panel panelDictionary;
        //Timer
        private System.Windows.Forms.Timer timer;
        //GroupBox
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.GroupBox groupBox10;
        //Label
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.Label label44;
        //ComboBox
        private System.Windows.Forms.ComboBox comboBoxSource;
        private System.Windows.Forms.ComboBox comboBoxSourceType;
        private System.Windows.Forms.ComboBox comboBoxIndexType;
        private System.Windows.Forms.ComboBox comboBoxIndex;
        private System.Windows.Forms.ComboBox comboBoxSouceSel;
        //TextBox
        private System.Windows.Forms.TextBox textBoxUserName;
        private System.Windows.Forms.TextBox textBoxDataBase;
        private System.Windows.Forms.TextBox textBoxHostName;
        private System.Windows.Forms.TextBox textBoxSourceName;
        private System.Windows.Forms.TextBox textBoxQuery;
        private System.Windows.Forms.TextBox textBoxFields;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.TextBox textBoxIndexPath;
        private System.Windows.Forms.TextBox textBoxIndexName;
        private System.Windows.Forms.TextBox textBoxFilterPath;
        private System.Windows.Forms.TextBox textBoxNumberPath;
        private System.Windows.Forms.TextBox textBoxNamePath;
        private System.Windows.Forms.TextBox textBoxBasePath;
        private System.Windows.Forms.TextBox textBoxQueryPath;
        private System.Windows.Forms.TextBox textBoxSearchdPath;
        //Button
        private System.Windows.Forms.Button btnTestDBLink;
        private System.Windows.Forms.Button btnSourceCancel;
        private System.Windows.Forms.Button btnSourceConfim;
        private System.Windows.Forms.Button btnDelSource;
        private System.Windows.Forms.Button btnEditSource;
        private System.Windows.Forms.Button btnAddSource;
        private System.Windows.Forms.Button btnTestFields;
        private System.Windows.Forms.Button btnTestQuery;
        private System.Windows.Forms.Button btnIndexCancel;
        private System.Windows.Forms.Button btnIndexConfirm;
        private System.Windows.Forms.Button btnDelIndex;
        private System.Windows.Forms.Button btnEditIndex;
        private System.Windows.Forms.Button btnAddIndex;
        private System.Windows.Forms.Button btnSetIndexPath;
        private System.Windows.Forms.Button btnMainIndexReCreate;
        private System.Windows.Forms.Button btnChangeDictSet;
        private System.Windows.Forms.Button btnDictStopService;
        private System.Windows.Forms.Button btnDictConfirm;
        private System.Windows.Forms.Button btnDictStartService;
        private System.Windows.Forms.Button btnDictService;
        private System.Windows.Forms.Button btnSetCustomPaths;
        private System.Windows.Forms.Button btnSetFilterPath;
        private System.Windows.Forms.Button btnSetNumberPath;
        private System.Windows.Forms.Button btnSetNamePath;
        private System.Windows.Forms.Button btnSetBasePath;
        private System.Windows.Forms.Button btnEditIndexer;
        private System.Windows.Forms.Button btnReCreateIncrIndex;
        private System.Windows.Forms.Button btnIndexerConfirm;
        private System.Windows.Forms.Button btnIndexerService;
        private System.Windows.Forms.Button btnIndexerStopService;
        private System.Windows.Forms.Button btnIndexerStartService;
        private System.Windows.Forms.Button btnEditSearchd;
        private System.Windows.Forms.Button btnSearchdService;
        private System.Windows.Forms.Button btnSearchdConfirm;
        private System.Windows.Forms.Button btnSearchdStopService;
        private System.Windows.Forms.Button btnSearchdStartService;
        private System.Windows.Forms.Button btnSetQueryPath;
        private System.Windows.Forms.Button btnSetSearchdPath;
        //List
        private System.Windows.Forms.ListBox listBoxCustomPaths;
        //DateTimePicker
        private System.Windows.Forms.DateTimePicker dateTimePickerMainReCreate;
        //NumericUpDown
        private System.Windows.Forms.NumericUpDown numericUpDownIncrTimeSpan;
        private System.Windows.Forms.NumericUpDown numericUpDownMainCreateTimeSpan;
        private System.Windows.Forms.NumericUpDown numericUpDownRamBufferSize;
        private System.Windows.Forms.NumericUpDown numericUpDownMergeFactor;
        private System.Windows.Forms.NumericUpDown numericUpDownMaxBufferedDocs;
        private System.Windows.Forms.NumericUpDown numericUpDownMaxFieldLength;
        private System.Windows.Forms.NumericUpDown numericUpDownMaxTransport;
        private System.Windows.Forms.NumericUpDown numericUpDownMaxMatches;
        private System.Windows.Forms.NumericUpDown numericUpDownTimeOut;
        private System.Windows.Forms.NumericUpDown numericUpDownPort;
        //MaskedTextBox
        private System.Windows.Forms.MaskedTextBox maskedTextBoxIpAddress;
    }
}