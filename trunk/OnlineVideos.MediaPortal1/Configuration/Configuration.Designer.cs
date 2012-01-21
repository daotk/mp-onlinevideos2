﻿/*
 * Created by SharpDevelop.
 * User: GZamor1
 * Date: 7/24/2007
 * Time: 9:34 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

namespace OnlineVideos.MediaPortal1
{
	partial class Configuration
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Configuration));
			this.txtDownloadDir = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.RssLinkList = new System.Windows.Forms.ListBox();
			this.bindingSourceRssLink = new System.Windows.Forms.BindingSource(this.components);
			this.bindingSourceSiteSettings = new System.Windows.Forms.BindingSource(this.components);
			this.txtRssUrl = new System.Windows.Forms.TextBox();
			this.label26 = new System.Windows.Forms.Label();
			this.txtRssName = new System.Windows.Forms.TextBox();
			this.label25 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.iconSite = new System.Windows.Forms.PictureBox();
			this.descriptionTextBox = new System.Windows.Forms.TextBox();
			this.label22 = new System.Windows.Forms.Label();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.propertyGridUserConfig = new System.Windows.Forms.PropertyGrid();
			this.cbLanguages = new System.Windows.Forms.ComboBox();
			this.label18 = new System.Windows.Forms.Label();
			this.btnAdvanced = new System.Windows.Forms.Button();
			this.label27 = new System.Windows.Forms.Label();
			this.label28 = new System.Windows.Forms.Label();
			this.txtSiteName = new System.Windows.Forms.TextBox();
			this.cbSiteUtil = new System.Windows.Forms.ComboBox();
			this.chkAgeConfirm = new System.Windows.Forms.CheckBox();
			this.label4 = new System.Windows.Forms.Label();
			this.toolStripContainer3 = new System.Windows.Forms.ToolStripContainer();
			this.siteList = new OnlineVideos.MediaPortal1.BindableCheckedListBox();
			this.toolStripSiteUpDown = new System.Windows.Forms.ToolStrip();
			this.btnSiteUp = new System.Windows.Forms.ToolStripButton();
			this.btnSiteDown = new System.Windows.Forms.ToolStripButton();
			this.toolStripSites = new System.Windows.Forms.ToolStrip();
			this.toolStripDropDownBtnImport = new System.Windows.Forms.ToolStripDropDownButton();
			this.btnImportXml = new System.Windows.Forms.ToolStripMenuItem();
			this.btnImportGlobal = new System.Windows.Forms.ToolStripMenuItem();
			this.btnAddSite = new System.Windows.Forms.ToolStripButton();
			this.btnDeleteSite = new System.Windows.Forms.ToolStripButton();
			this.btnPublishSite = new System.Windows.Forms.ToolStripButton();
			this.btnReportSite = new System.Windows.Forms.ToolStripButton();
			this.btnCreateSite = new System.Windows.Forms.ToolStripButton();
			this.txtFilters = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtThumbLoc = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			this.label5 = new System.Windows.Forms.Label();
			this.chkUseAgeConfirmation = new System.Windows.Forms.CheckBox();
			this.btnSave = new System.Windows.Forms.Button();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPageRssLinks = new System.Windows.Forms.TabPage();
			this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
			this.toolStripRss = new System.Windows.Forms.ToolStrip();
			this.btnAddRss = new System.Windows.Forms.ToolStripButton();
			this.btnDeleteRss = new System.Windows.Forms.ToolStripButton();
			this.txtRssThumb = new System.Windows.Forms.TextBox();
			this.label19 = new System.Windows.Forms.Label();
			this.tabChannels = new System.Windows.Forms.TabPage();
			this.toolStripContainer2 = new System.Windows.Forms.ToolStripContainer();
			this.tvGroups = new System.Windows.Forms.TreeView();
			this.toolStripChannels = new System.Windows.Forms.ToolStrip();
			this.btnAddGroup = new System.Windows.Forms.ToolStripButton();
			this.btnAddChannel = new System.Windows.Forms.ToolStripButton();
			this.btnDeleteChannel = new System.Windows.Forms.ToolStripButton();
			this.tbxChannelThumb = new System.Windows.Forms.TextBox();
			this.label20 = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.tbxStreamThumb = new System.Windows.Forms.TextBox();
			this.btnSaveChannel = new System.Windows.Forms.Button();
			this.label10 = new System.Windows.Forms.Label();
			this.tbxStreamUrl = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.tbxStreamName = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.tbxChannelName = new System.Windows.Forms.TextBox();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.tbxPin = new System.Windows.Forms.TextBox();
			this.tbxScreenName = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.btnBrowseForDlFolder = new System.Windows.Forms.Button();
			this.mainTabControl = new System.Windows.Forms.TabControl();
			this.tabGeneral = new System.Windows.Forms.TabPage();
			this.pictureBox6 = new System.Windows.Forms.PictureBox();
			this.groupBoxLatestVideos = new System.Windows.Forms.GroupBox();
			this.chkLatestVideosRandomize = new System.Windows.Forms.CheckBox();
			this.label53 = new System.Windows.Forms.Label();
			this.label52 = new System.Windows.Forms.Label();
			this.tbxLatestVideosGuiRefresh = new System.Windows.Forms.TextBox();
			this.tbxLatestVideosOnlineRefresh = new System.Windows.Forms.TextBox();
			this.tbxLatestVideosAmount = new System.Windows.Forms.TextBox();
			this.label51 = new System.Windows.Forms.Label();
			this.label50 = new System.Windows.Forms.Label();
			this.label49 = new System.Windows.Forms.Label();
			this.tbxUpdatePeriod = new System.Windows.Forms.TextBox();
			this.label45 = new System.Windows.Forms.Label();
			this.label44 = new System.Windows.Forms.Label();
			this.btnWiki = new System.Windows.Forms.Button();
			this.pictureBox5 = new System.Windows.Forms.PictureBox();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.label21 = new System.Windows.Forms.Label();
			this.pictureBox4 = new System.Windows.Forms.PictureBox();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.label34 = new System.Windows.Forms.Label();
			this.rbOff = new System.Windows.Forms.RadioButton();
			this.rbLastSearch = new System.Windows.Forms.RadioButton();
			this.rbExtendedSearchHistory = new System.Windows.Forms.RadioButton();
			this.label38 = new System.Windows.Forms.Label();
			this.nUPSearchHistoryItemCount = new System.Windows.Forms.NumericUpDown();
			this.pictureBox3 = new System.Windows.Forms.PictureBox();
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.label33 = new System.Windows.Forms.Label();
			this.chkUseQuickSelect = new System.Windows.Forms.CheckBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.label48 = new System.Windows.Forms.Label();
			this.label47 = new System.Windows.Forms.Label();
			this.tbxCategoriesTimeout = new System.Windows.Forms.TextBox();
			this.label32 = new System.Windows.Forms.Label();
			this.label31 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.tbxWebCacheTimeout = new System.Windows.Forms.TextBox();
			this.label15 = new System.Windows.Forms.Label();
			this.tbxUtilTimeout = new System.Windows.Forms.TextBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label30 = new System.Windows.Forms.Label();
			this.label29 = new System.Windows.Forms.Label();
			this.udPlayBuffer = new System.Windows.Forms.DomainUpDown();
			this.label16 = new System.Windows.Forms.Label();
			this.label24 = new System.Windows.Forms.Label();
			this.tbxWMPBuffer = new System.Windows.Forms.TextBox();
			this.label23 = new System.Windows.Forms.Label();
			this.chkDoAutoUpdate = new System.Windows.Forms.CheckBox();
			this.btnCancel = new System.Windows.Forms.Button();
			this.lblVersion = new System.Windows.Forms.Label();
			this.Thumbnails = new System.Windows.Forms.GroupBox();
			this.label36 = new System.Windows.Forms.Label();
			this.tbxThumbAge = new System.Windows.Forms.TextBox();
			this.label35 = new System.Windows.Forms.Label();
			this.bntBrowseFolderForThumbs = new System.Windows.Forms.Button();
			this.tabGroups = new System.Windows.Forms.TabPage();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.chkFavFirst = new System.Windows.Forms.CheckBox();
			this.chkAutoGroupByLang = new System.Windows.Forms.CheckBox();
			this.btnBrowseSitesGroupThumb = new System.Windows.Forms.Button();
			this.tbxSitesGroupDesc = new System.Windows.Forms.TextBox();
			this.bindingSourceSitesGroup = new System.Windows.Forms.BindingSource(this.components);
			this.label43 = new System.Windows.Forms.Label();
			this.tbxSitesGroupThumb = new System.Windows.Forms.TextBox();
			this.label42 = new System.Windows.Forms.Label();
			this.tbxSitesGroupName = new System.Windows.Forms.TextBox();
			this.label39 = new System.Windows.Forms.Label();
			this.toolStripContainer4 = new System.Windows.Forms.ToolStripContainer();
			this.listBoxSitesGroups = new System.Windows.Forms.ListBox();
			this.toolStrip2 = new System.Windows.Forms.ToolStrip();
			this.btnSitesGroupUp = new System.Windows.Forms.ToolStripButton();
			this.btnSitesGroupDown = new System.Windows.Forms.ToolStripButton();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.btnAddSitesGroup = new System.Windows.Forms.ToolStripButton();
			this.btnDeleteSitesGroup = new System.Windows.Forms.ToolStripButton();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.listViewSitesNotInGroup = new System.Windows.Forms.ListView();
			this.label40 = new System.Windows.Forms.Label();
			this.listViewSitesInGroup = new System.Windows.Forms.ListView();
			this.label41 = new System.Windows.Forms.Label();
			this.tabSites = new System.Windows.Forms.TabPage();
			this.tabHosters = new System.Windows.Forms.TabPage();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.propertyGridHoster = new System.Windows.Forms.PropertyGrid();
			this.listBoxHosters = new System.Windows.Forms.ListBox();
			this.tabPageCodecs = new System.Windows.Forms.TabPage();
			this.videopanel = new System.Windows.Forms.Panel();
			this.groupBoxSplitter = new System.Windows.Forms.GroupBox();
			this.btnTestAvi = new System.Windows.Forms.Button();
			this.btnTestWmv = new System.Windows.Forms.Button();
			this.btnTestMp4 = new System.Windows.Forms.Button();
			this.btnTestMov = new System.Windows.Forms.Button();
			this.tbxMOVSplitter = new System.Windows.Forms.TextBox();
			this.chkMOVSplitterInstalled = new System.Windows.Forms.CheckBox();
			this.label46 = new System.Windows.Forms.Label();
			this.btnTestFlv = new System.Windows.Forms.Button();
			this.tbxWMVSplitter = new System.Windows.Forms.TextBox();
			this.tbxAVISplitter = new System.Windows.Forms.TextBox();
			this.tbxMP4Splitter = new System.Windows.Forms.TextBox();
			this.tbxFLVSplitter = new System.Windows.Forms.TextBox();
			this.chkWMVSplitterInstalled = new System.Windows.Forms.CheckBox();
			this.chkAVISplitterInstalled = new System.Windows.Forms.CheckBox();
			this.chkMP4SplitterInstalled = new System.Windows.Forms.CheckBox();
			this.label14 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.chkFLVSplitterInstalled = new System.Windows.Forms.CheckBox();
			this.label11 = new System.Windows.Forms.Label();
			this.sitesBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.onlineVideosService1 = new OnlineVideos.OnlineVideosWebservice.OnlineVideosService();
			((System.ComponentModel.ISupportInitialize)(this.bindingSourceRssLink)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.bindingSourceSiteSettings)).BeginInit();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.iconSite)).BeginInit();
			this.tableLayoutPanel1.SuspendLayout();
			this.toolStripContainer3.ContentPanel.SuspendLayout();
			this.toolStripContainer3.LeftToolStripPanel.SuspendLayout();
			this.toolStripContainer3.TopToolStripPanel.SuspendLayout();
			this.toolStripContainer3.SuspendLayout();
			this.toolStripSiteUpDown.SuspendLayout();
			this.toolStripSites.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPageRssLinks.SuspendLayout();
			this.toolStripContainer1.ContentPanel.SuspendLayout();
			this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
			this.toolStripContainer1.SuspendLayout();
			this.toolStripRss.SuspendLayout();
			this.tabChannels.SuspendLayout();
			this.toolStripContainer2.ContentPanel.SuspendLayout();
			this.toolStripContainer2.TopToolStripPanel.SuspendLayout();
			this.toolStripContainer2.SuspendLayout();
			this.toolStripChannels.SuspendLayout();
			this.mainTabControl.SuspendLayout();
			this.tabGeneral.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
			this.groupBoxLatestVideos.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
			this.groupBox5.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
			this.groupBox4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nUPSearchHistoryItemCount)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.groupBox3.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.Thumbnails.SuspendLayout();
			this.tabGroups.SuspendLayout();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.bindingSourceSitesGroup)).BeginInit();
			this.toolStripContainer4.ContentPanel.SuspendLayout();
			this.toolStripContainer4.LeftToolStripPanel.SuspendLayout();
			this.toolStripContainer4.TopToolStripPanel.SuspendLayout();
			this.toolStripContainer4.SuspendLayout();
			this.toolStrip2.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			this.tabSites.SuspendLayout();
			this.tabHosters.SuspendLayout();
			this.tabPageCodecs.SuspendLayout();
			this.groupBoxSplitter.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.sitesBindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
			this.SuspendLayout();
			// 
			// txtDownloadDir
			// 
			this.txtDownloadDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtDownloadDir.Location = new System.Drawing.Point(225, 35);
			this.txtDownloadDir.Name = "txtDownloadDir";
			this.txtDownloadDir.Size = new System.Drawing.Size(429, 20);
			this.txtDownloadDir.TabIndex = 2;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(6, 38);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(87, 13);
			this.label3.TabIndex = 0;
			this.label3.Text = "Download Folder";
			// 
			// RssLinkList
			// 
			this.RssLinkList.DataSource = this.bindingSourceRssLink;
			this.RssLinkList.DisplayMember = "Name";
			this.RssLinkList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.RssLinkList.FormattingEnabled = true;
			this.RssLinkList.Location = new System.Drawing.Point(0, 0);
			this.RssLinkList.Name = "RssLinkList";
			this.RssLinkList.Size = new System.Drawing.Size(196, 252);
			this.RssLinkList.TabIndex = 6;
			this.RssLinkList.SelectedIndexChanged += new System.EventHandler(this.RssLinkListSelectedIndexChanged);
			// 
			// bindingSourceRssLink
			// 
			this.bindingSourceRssLink.DataSource = typeof(OnlineVideos.RssLink);
			// 
			// bindingSourceSiteSettings
			// 
			this.bindingSourceSiteSettings.DataSource = typeof(OnlineVideos.SiteSettings);
			// 
			// txtRssUrl
			// 
			this.txtRssUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtRssUrl.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourceRssLink, "Url", true));
			this.txtRssUrl.Enabled = false;
			this.txtRssUrl.Location = new System.Drawing.Point(286, 50);
			this.txtRssUrl.Name = "txtRssUrl";
			this.txtRssUrl.Size = new System.Drawing.Size(407, 20);
			this.txtRssUrl.TabIndex = 16;
			// 
			// label26
			// 
			this.label26.AutoSize = true;
			this.label26.Location = new System.Drawing.Point(203, 31);
			this.label26.Name = "label26";
			this.label26.Size = new System.Drawing.Size(60, 13);
			this.label26.TabIndex = 9;
			this.label26.Text = "RSS Name";
			// 
			// txtRssName
			// 
			this.txtRssName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtRssName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourceRssLink, "Name", true));
			this.txtRssName.Enabled = false;
			this.txtRssName.Location = new System.Drawing.Point(286, 28);
			this.txtRssName.Name = "txtRssName";
			this.txtRssName.Size = new System.Drawing.Size(407, 20);
			this.txtRssName.TabIndex = 15;
			// 
			// label25
			// 
			this.label25.AutoSize = true;
			this.label25.Location = new System.Drawing.Point(203, 53);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(45, 13);
			this.label25.TabIndex = 10;
			this.label25.Text = "RSS Url";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.iconSite);
			this.groupBox1.Controls.Add(this.descriptionTextBox);
			this.groupBox1.Controls.Add(this.label22);
			this.groupBox1.Controls.Add(this.tableLayoutPanel1);
			this.groupBox1.Controls.Add(this.toolStripContainer3);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox1.Location = new System.Drawing.Point(2, 2);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(700, 230);
			this.groupBox1.TabIndex = 17;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Sites";
			// 
			// iconSite
			// 
			this.iconSite.Location = new System.Drawing.Point(214, 147);
			this.iconSite.Name = "iconSite";
			this.iconSite.Size = new System.Drawing.Size(60, 60);
			this.iconSite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.iconSite.TabIndex = 29;
			this.iconSite.TabStop = false;
			// 
			// descriptionTextBox
			// 
			this.descriptionTextBox.AcceptsReturn = true;
			this.descriptionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.descriptionTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourceSiteSettings, "Description", true));
			this.descriptionTextBox.Location = new System.Drawing.Point(290, 178);
			this.descriptionTextBox.Multiline = true;
			this.descriptionTextBox.Name = "descriptionTextBox";
			this.descriptionTextBox.Size = new System.Drawing.Size(407, 46);
			this.descriptionTextBox.TabIndex = 28;
			// 
			// label22
			// 
			this.label22.AutoSize = true;
			this.label22.Location = new System.Drawing.Point(208, 211);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(60, 13);
			this.label22.TabIndex = 27;
			this.label22.Text = "Description";
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.ColumnCount = 4;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 83F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.propertyGridUserConfig, 3, 0);
			this.tableLayoutPanel1.Controls.Add(this.cbLanguages, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.label18, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.btnAdvanced, 1, 4);
			this.tableLayoutPanel1.Controls.Add(this.label27, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.label28, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.txtSiteName, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.cbSiteUtil, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.chkAgeConfirm, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(205, 42);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 5;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(493, 132);
			this.tableLayoutPanel1.TabIndex = 26;
			// 
			// propertyGridUserConfig
			// 
			this.propertyGridUserConfig.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyGridUserConfig.Location = new System.Drawing.Point(246, 3);
			this.propertyGridUserConfig.Name = "propertyGridUserConfig";
			this.propertyGridUserConfig.PropertySort = System.Windows.Forms.PropertySort.NoSort;
			this.tableLayoutPanel1.SetRowSpan(this.propertyGridUserConfig, 5);
			this.propertyGridUserConfig.Size = new System.Drawing.Size(244, 126);
			this.propertyGridUserConfig.TabIndex = 29;
			this.propertyGridUserConfig.ToolbarVisible = false;
			// 
			// cbLanguages
			// 
			this.cbLanguages.DataBindings.Add(new System.Windows.Forms.Binding("SelectedItem", this.bindingSourceSiteSettings, "Language", true));
			this.cbLanguages.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cbLanguages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbLanguages.FormattingEnabled = true;
			this.cbLanguages.Location = new System.Drawing.Point(85, 54);
			this.cbLanguages.Margin = new System.Windows.Forms.Padding(2);
			this.cbLanguages.Name = "cbLanguages";
			this.cbLanguages.Size = new System.Drawing.Size(146, 21);
			this.cbLanguages.TabIndex = 22;
			// 
			// label18
			// 
			this.label18.AutoSize = true;
			this.label18.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label18.Location = new System.Drawing.Point(3, 52);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(77, 26);
			this.label18.TabIndex = 23;
			this.label18.Text = "Language";
			this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnAdvanced
			// 
			this.btnAdvanced.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnAdvanced.Location = new System.Drawing.Point(86, 107);
			this.btnAdvanced.Name = "btnAdvanced";
			this.btnAdvanced.Size = new System.Drawing.Size(144, 22);
			this.btnAdvanced.TabIndex = 25;
			this.btnAdvanced.Text = "Advanced";
			this.btnAdvanced.UseVisualStyleBackColor = true;
			this.btnAdvanced.Click += new System.EventHandler(this.btnAdvanced_Click);
			// 
			// label27
			// 
			this.label27.AutoSize = true;
			this.label27.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label27.Location = new System.Drawing.Point(3, 0);
			this.label27.Name = "label27";
			this.label27.Size = new System.Drawing.Size(77, 26);
			this.label27.TabIndex = 9;
			this.label27.Text = "Site Name";
			this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label28
			// 
			this.label28.AutoSize = true;
			this.label28.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label28.Location = new System.Drawing.Point(3, 26);
			this.label28.Name = "label28";
			this.label28.Size = new System.Drawing.Size(77, 26);
			this.label28.TabIndex = 10;
			this.label28.Text = "Site Util";
			this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtSiteName
			// 
			this.txtSiteName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourceSiteSettings, "Name", true));
			this.txtSiteName.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtSiteName.Location = new System.Drawing.Point(86, 3);
			this.txtSiteName.Name = "txtSiteName";
			this.txtSiteName.Size = new System.Drawing.Size(144, 20);
			this.txtSiteName.TabIndex = 11;
			// 
			// cbSiteUtil
			// 
			this.cbSiteUtil.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cbSiteUtil.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbSiteUtil.FormattingEnabled = true;
			this.cbSiteUtil.Location = new System.Drawing.Point(85, 28);
			this.cbSiteUtil.Margin = new System.Windows.Forms.Padding(2);
			this.cbSiteUtil.Name = "cbSiteUtil";
			this.cbSiteUtil.Size = new System.Drawing.Size(146, 21);
			this.cbSiteUtil.TabIndex = 21;
			this.cbSiteUtil.SelectedValueChanged += new System.EventHandler(this.SelectedSiteUtilChanged);
			// 
			// chkAgeConfirm
			// 
			this.chkAgeConfirm.AutoSize = true;
			this.chkAgeConfirm.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.bindingSourceSiteSettings, "ConfirmAge", true));
			this.chkAgeConfirm.Dock = System.Windows.Forms.DockStyle.Fill;
			this.chkAgeConfirm.Location = new System.Drawing.Point(86, 81);
			this.chkAgeConfirm.Name = "chkAgeConfirm";
			this.chkAgeConfirm.Size = new System.Drawing.Size(144, 20);
			this.chkAgeConfirm.TabIndex = 16;
			this.chkAgeConfirm.UseVisualStyleBackColor = true;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label4.Location = new System.Drawing.Point(3, 78);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(77, 26);
			this.label4.TabIndex = 30;
			this.label4.Text = "Confirm Age";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// toolStripContainer3
			// 
			this.toolStripContainer3.BottomToolStripPanelVisible = false;
			// 
			// toolStripContainer3.ContentPanel
			// 
			this.toolStripContainer3.ContentPanel.Controls.Add(this.siteList);
			this.toolStripContainer3.ContentPanel.Margin = new System.Windows.Forms.Padding(2);
			this.toolStripContainer3.ContentPanel.Size = new System.Drawing.Size(172, 186);
			this.toolStripContainer3.Dock = System.Windows.Forms.DockStyle.Left;
			// 
			// toolStripContainer3.LeftToolStripPanel
			// 
			this.toolStripContainer3.LeftToolStripPanel.Controls.Add(this.toolStripSiteUpDown);
			this.toolStripContainer3.Location = new System.Drawing.Point(3, 16);
			this.toolStripContainer3.Margin = new System.Windows.Forms.Padding(2);
			this.toolStripContainer3.Name = "toolStripContainer3";
			this.toolStripContainer3.RightToolStripPanelVisible = false;
			this.toolStripContainer3.Size = new System.Drawing.Size(196, 211);
			this.toolStripContainer3.TabIndex = 24;
			this.toolStripContainer3.Text = "toolStripContainer3";
			// 
			// toolStripContainer3.TopToolStripPanel
			// 
			this.toolStripContainer3.TopToolStripPanel.Controls.Add(this.toolStripSites);
			// 
			// siteList
			// 
			this.siteList.CheckedMember = "IsEnabled";
			this.siteList.DataSource = this.bindingSourceSiteSettings;
			this.siteList.DisplayMember = "Name";
			this.siteList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.siteList.FormattingEnabled = true;
			this.siteList.Location = new System.Drawing.Point(0, 0);
			this.siteList.Name = "siteList";
			this.siteList.Size = new System.Drawing.Size(172, 186);
			this.siteList.TabIndex = 5;
			this.siteList.SelectedValueChanged += new System.EventHandler(this.SiteListSelectedValueChanged);
			// 
			// toolStripSiteUpDown
			// 
			this.toolStripSiteUpDown.Dock = System.Windows.Forms.DockStyle.None;
			this.toolStripSiteUpDown.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStripSiteUpDown.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSiteUp,
            this.btnSiteDown});
			this.toolStripSiteUpDown.Location = new System.Drawing.Point(0, 0);
			this.toolStripSiteUpDown.Name = "toolStripSiteUpDown";
			this.toolStripSiteUpDown.Size = new System.Drawing.Size(24, 211);
			this.toolStripSiteUpDown.Stretch = true;
			this.toolStripSiteUpDown.TabIndex = 0;
			// 
			// btnSiteUp
			// 
			this.btnSiteUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnSiteUp.Enabled = false;
			this.btnSiteUp.Image = global::OnlineVideos.MediaPortal1.Properties.Resources.Up;
			this.btnSiteUp.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnSiteUp.Name = "btnSiteUp";
			this.btnSiteUp.Size = new System.Drawing.Size(22, 20);
			this.btnSiteUp.Text = "Up";
			this.btnSiteUp.Click += new System.EventHandler(this.btnSiteUp_Click);
			// 
			// btnSiteDown
			// 
			this.btnSiteDown.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.btnSiteDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnSiteDown.Enabled = false;
			this.btnSiteDown.Image = global::OnlineVideos.MediaPortal1.Properties.Resources.Down;
			this.btnSiteDown.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnSiteDown.Name = "btnSiteDown";
			this.btnSiteDown.Size = new System.Drawing.Size(22, 20);
			this.btnSiteDown.Text = "Down";
			this.btnSiteDown.Click += new System.EventHandler(this.btnSiteDown_Click);
			// 
			// toolStripSites
			// 
			this.toolStripSites.Dock = System.Windows.Forms.DockStyle.None;
			this.toolStripSites.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStripSites.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownBtnImport,
            this.btnAddSite,
            this.btnDeleteSite,
            this.btnPublishSite,
            this.btnReportSite,
            this.btnCreateSite});
			this.toolStripSites.Location = new System.Drawing.Point(3, 0);
			this.toolStripSites.Name = "toolStripSites";
			this.toolStripSites.Size = new System.Drawing.Size(147, 25);
			this.toolStripSites.TabIndex = 0;
			// 
			// toolStripDropDownBtnImport
			// 
			this.toolStripDropDownBtnImport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripDropDownBtnImport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnImportXml,
            this.btnImportGlobal});
			this.toolStripDropDownBtnImport.Image = global::OnlineVideos.MediaPortal1.Properties.Resources.Import;
			this.toolStripDropDownBtnImport.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripDropDownBtnImport.Name = "toolStripDropDownBtnImport";
			this.toolStripDropDownBtnImport.Size = new System.Drawing.Size(29, 22);
			this.toolStripDropDownBtnImport.ToolTipText = "Import";
			// 
			// btnImportXml
			// 
			this.btnImportXml.Image = global::OnlineVideos.MediaPortal1.Properties.Resources.ImportXml;
			this.btnImportXml.Name = "btnImportXml";
			this.btnImportXml.Size = new System.Drawing.Size(108, 22);
			this.btnImportXml.Text = "XML";
			this.btnImportXml.ToolTipText = "Import from Xml";
			this.btnImportXml.Click += new System.EventHandler(this.btnImportSite_Click);
			// 
			// btnImportGlobal
			// 
			this.btnImportGlobal.Image = global::OnlineVideos.MediaPortal1.Properties.Resources.ImportGlobal;
			this.btnImportGlobal.Name = "btnImportGlobal";
			this.btnImportGlobal.Size = new System.Drawing.Size(108, 22);
			this.btnImportGlobal.Text = "Global";
			this.btnImportGlobal.ToolTipText = "Import from global List";
			this.btnImportGlobal.Click += new System.EventHandler(this.btnImportGlobal_Click);
			// 
			// btnAddSite
			// 
			this.btnAddSite.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnAddSite.Image = global::OnlineVideos.MediaPortal1.Properties.Resources.Add;
			this.btnAddSite.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnAddSite.Name = "btnAddSite";
			this.btnAddSite.Size = new System.Drawing.Size(23, 22);
			this.btnAddSite.Text = "Add";
			this.btnAddSite.Click += new System.EventHandler(this.btnAddSite_Click);
			// 
			// btnDeleteSite
			// 
			this.btnDeleteSite.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnDeleteSite.Enabled = false;
			this.btnDeleteSite.Image = global::OnlineVideos.MediaPortal1.Properties.Resources.delete;
			this.btnDeleteSite.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnDeleteSite.Name = "btnDeleteSite";
			this.btnDeleteSite.Size = new System.Drawing.Size(23, 22);
			this.btnDeleteSite.Text = "Delete";
			this.btnDeleteSite.Click += new System.EventHandler(this.btnDeleteSite_Click);
			// 
			// btnPublishSite
			// 
			this.btnPublishSite.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnPublishSite.Enabled = false;
			this.btnPublishSite.Image = global::OnlineVideos.MediaPortal1.Properties.Resources.PublishToWeb;
			this.btnPublishSite.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnPublishSite.Name = "btnPublishSite";
			this.btnPublishSite.Size = new System.Drawing.Size(23, 22);
			this.btnPublishSite.Text = "Publish to Web";
			this.btnPublishSite.Click += new System.EventHandler(this.btnPublishSite_Click);
			// 
			// btnReportSite
			// 
			this.btnReportSite.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnReportSite.Enabled = false;
			this.btnReportSite.Image = global::OnlineVideos.MediaPortal1.Properties.Resources.NewReport;
			this.btnReportSite.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnReportSite.Name = "btnReportSite";
			this.btnReportSite.Size = new System.Drawing.Size(23, 22);
			this.btnReportSite.Text = "Write report";
			this.btnReportSite.Click += new System.EventHandler(this.btnReportSite_Click);
			// 
			// btnCreateSite
			// 
			this.btnCreateSite.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnCreateSite.Image = global::OnlineVideos.MediaPortal1.Properties.Resources.CreateSite;
			this.btnCreateSite.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnCreateSite.Name = "btnCreateSite";
			this.btnCreateSite.Size = new System.Drawing.Size(23, 22);
			this.btnCreateSite.Text = "Create/Edit Generic Site";
			this.btnCreateSite.Click += new System.EventHandler(this.btnCreateSite_Click);
			// 
			// txtFilters
			// 
			this.txtFilters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtFilters.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtFilters.Location = new System.Drawing.Point(225, 50);
			this.txtFilters.Name = "txtFilters";
			this.txtFilters.Size = new System.Drawing.Size(460, 20);
			this.txtFilters.TabIndex = 9;
			this.toolTip1.SetToolTip(this.txtFilters, "Comma seperated list of words that will be used to filter out videos.");
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(6, 53);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(155, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "Filter out videos with these tags";
			// 
			// txtThumbLoc
			// 
			this.txtThumbLoc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtThumbLoc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtThumbLoc.Location = new System.Drawing.Point(285, 23);
			this.txtThumbLoc.Name = "txtThumbLoc";
			this.txtThumbLoc.Size = new System.Drawing.Size(369, 20);
			this.txtThumbLoc.TabIndex = 20;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(222, 26);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(48, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Location";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(6, 25);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(109, 13);
			this.label5.TabIndex = 0;
			this.label5.Text = "Use Age Confirmation";
			// 
			// chkUseAgeConfirmation
			// 
			this.chkUseAgeConfirmation.AutoSize = true;
			this.chkUseAgeConfirmation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkUseAgeConfirmation.Location = new System.Drawing.Point(225, 25);
			this.chkUseAgeConfirmation.Name = "chkUseAgeConfirmation";
			this.chkUseAgeConfirmation.Size = new System.Drawing.Size(15, 14);
			this.chkUseAgeConfirmation.TabIndex = 7;
			this.chkUseAgeConfirmation.UseVisualStyleBackColor = true;
			this.chkUseAgeConfirmation.CheckedChanged += new System.EventHandler(this.chkUseAgeConfirmation_CheckedChanged);
			// 
			// btnSave
			// 
			this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnSave.Image = global::OnlineVideos.MediaPortal1.Properties.Resources.Save;
			this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnSave.Location = new System.Drawing.Point(499, 512);
			this.btnSave.Name = "btnSave";
			this.btnSave.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.btnSave.Size = new System.Drawing.Size(90, 25);
			this.btnSave.TabIndex = 26;
			this.btnSave.Text = "Save";
			this.btnSave.UseVisualStyleBackColor = true;
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPageRssLinks);
			this.tabControl1.Controls.Add(this.tabChannels);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.ImageList = this.imageList1;
			this.tabControl1.Location = new System.Drawing.Point(2, 232);
			this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(700, 308);
			this.tabControl1.TabIndex = 27;
			// 
			// tabPageRssLinks
			// 
			this.tabPageRssLinks.Controls.Add(this.toolStripContainer1);
			this.tabPageRssLinks.Controls.Add(this.txtRssThumb);
			this.tabPageRssLinks.Controls.Add(this.label19);
			this.tabPageRssLinks.Controls.Add(this.txtRssUrl);
			this.tabPageRssLinks.Controls.Add(this.label26);
			this.tabPageRssLinks.Controls.Add(this.txtRssName);
			this.tabPageRssLinks.Controls.Add(this.label25);
			this.tabPageRssLinks.ImageIndex = 0;
			this.tabPageRssLinks.Location = new System.Drawing.Point(4, 23);
			this.tabPageRssLinks.Margin = new System.Windows.Forms.Padding(2);
			this.tabPageRssLinks.Name = "tabPageRssLinks";
			this.tabPageRssLinks.Padding = new System.Windows.Forms.Padding(2);
			this.tabPageRssLinks.Size = new System.Drawing.Size(692, 281);
			this.tabPageRssLinks.TabIndex = 0;
			this.tabPageRssLinks.Text = "RssLinks";
			this.tabPageRssLinks.UseVisualStyleBackColor = true;
			// 
			// toolStripContainer1
			// 
			this.toolStripContainer1.BottomToolStripPanelVisible = false;
			// 
			// toolStripContainer1.ContentPanel
			// 
			this.toolStripContainer1.ContentPanel.Controls.Add(this.RssLinkList);
			this.toolStripContainer1.ContentPanel.Margin = new System.Windows.Forms.Padding(2);
			this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(196, 252);
			this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Left;
			this.toolStripContainer1.LeftToolStripPanelVisible = false;
			this.toolStripContainer1.Location = new System.Drawing.Point(2, 2);
			this.toolStripContainer1.Margin = new System.Windows.Forms.Padding(2);
			this.toolStripContainer1.Name = "toolStripContainer1";
			this.toolStripContainer1.RightToolStripPanelVisible = false;
			this.toolStripContainer1.Size = new System.Drawing.Size(196, 277);
			this.toolStripContainer1.TabIndex = 34;
			this.toolStripContainer1.Text = "toolStripContainer1";
			// 
			// toolStripContainer1.TopToolStripPanel
			// 
			this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStripRss);
			// 
			// toolStripRss
			// 
			this.toolStripRss.Dock = System.Windows.Forms.DockStyle.None;
			this.toolStripRss.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStripRss.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAddRss,
            this.btnDeleteRss});
			this.toolStripRss.Location = new System.Drawing.Point(3, 0);
			this.toolStripRss.Name = "toolStripRss";
			this.toolStripRss.Size = new System.Drawing.Size(49, 25);
			this.toolStripRss.TabIndex = 0;
			// 
			// btnAddRss
			// 
			this.btnAddRss.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnAddRss.Enabled = false;
			this.btnAddRss.Image = global::OnlineVideos.MediaPortal1.Properties.Resources.Add;
			this.btnAddRss.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnAddRss.Name = "btnAddRss";
			this.btnAddRss.Size = new System.Drawing.Size(23, 22);
			this.btnAddRss.Text = "Add";
			this.btnAddRss.Click += new System.EventHandler(this.BtnAddRssClick);
			// 
			// btnDeleteRss
			// 
			this.btnDeleteRss.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnDeleteRss.Enabled = false;
			this.btnDeleteRss.Image = global::OnlineVideos.MediaPortal1.Properties.Resources.delete;
			this.btnDeleteRss.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnDeleteRss.Name = "btnDeleteRss";
			this.btnDeleteRss.Size = new System.Drawing.Size(23, 22);
			this.btnDeleteRss.Text = "Delete";
			this.btnDeleteRss.Click += new System.EventHandler(this.BtnDeleteRssClick);
			// 
			// txtRssThumb
			// 
			this.txtRssThumb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtRssThumb.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourceRssLink, "Thumb", true));
			this.txtRssThumb.Enabled = false;
			this.txtRssThumb.Location = new System.Drawing.Point(286, 75);
			this.txtRssThumb.Name = "txtRssThumb";
			this.txtRssThumb.Size = new System.Drawing.Size(407, 20);
			this.txtRssThumb.TabIndex = 19;
			// 
			// label19
			// 
			this.label19.AutoSize = true;
			this.label19.Location = new System.Drawing.Point(203, 78);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(56, 13);
			this.label19.TabIndex = 18;
			this.label19.Text = "Thumb Url";
			// 
			// tabChannels
			// 
			this.tabChannels.Controls.Add(this.toolStripContainer2);
			this.tabChannels.Controls.Add(this.tbxChannelThumb);
			this.tabChannels.Controls.Add(this.label20);
			this.tabChannels.Controls.Add(this.label17);
			this.tabChannels.Controls.Add(this.tbxStreamThumb);
			this.tabChannels.Controls.Add(this.btnSaveChannel);
			this.tabChannels.Controls.Add(this.label10);
			this.tabChannels.Controls.Add(this.tbxStreamUrl);
			this.tabChannels.Controls.Add(this.label9);
			this.tabChannels.Controls.Add(this.tbxStreamName);
			this.tabChannels.Controls.Add(this.label8);
			this.tabChannels.Controls.Add(this.tbxChannelName);
			this.tabChannels.ImageIndex = 1;
			this.tabChannels.Location = new System.Drawing.Point(4, 23);
			this.tabChannels.Margin = new System.Windows.Forms.Padding(2);
			this.tabChannels.Name = "tabChannels";
			this.tabChannels.Padding = new System.Windows.Forms.Padding(2);
			this.tabChannels.Size = new System.Drawing.Size(692, 281);
			this.tabChannels.TabIndex = 1;
			this.tabChannels.Text = "Channels";
			this.tabChannels.UseVisualStyleBackColor = true;
			// 
			// toolStripContainer2
			// 
			// 
			// toolStripContainer2.ContentPanel
			// 
			this.toolStripContainer2.ContentPanel.Controls.Add(this.tvGroups);
			this.toolStripContainer2.ContentPanel.Margin = new System.Windows.Forms.Padding(2);
			this.toolStripContainer2.ContentPanel.Size = new System.Drawing.Size(196, 252);
			this.toolStripContainer2.Dock = System.Windows.Forms.DockStyle.Left;
			this.toolStripContainer2.Location = new System.Drawing.Point(2, 2);
			this.toolStripContainer2.Margin = new System.Windows.Forms.Padding(2);
			this.toolStripContainer2.Name = "toolStripContainer2";
			this.toolStripContainer2.Size = new System.Drawing.Size(196, 277);
			this.toolStripContainer2.TabIndex = 30;
			this.toolStripContainer2.Text = "toolStripContainer2";
			// 
			// toolStripContainer2.TopToolStripPanel
			// 
			this.toolStripContainer2.TopToolStripPanel.Controls.Add(this.toolStripChannels);
			// 
			// tvGroups
			// 
			this.tvGroups.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvGroups.Location = new System.Drawing.Point(0, 0);
			this.tvGroups.Margin = new System.Windows.Forms.Padding(2);
			this.tvGroups.Name = "tvGroups";
			this.tvGroups.Size = new System.Drawing.Size(196, 252);
			this.tvGroups.TabIndex = 0;
			this.tvGroups.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvGroups_AfterSelect);
			// 
			// toolStripChannels
			// 
			this.toolStripChannels.Dock = System.Windows.Forms.DockStyle.None;
			this.toolStripChannels.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStripChannels.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAddGroup,
            this.btnAddChannel,
            this.btnDeleteChannel});
			this.toolStripChannels.Location = new System.Drawing.Point(3, 0);
			this.toolStripChannels.Name = "toolStripChannels";
			this.toolStripChannels.Size = new System.Drawing.Size(72, 25);
			this.toolStripChannels.TabIndex = 0;
			// 
			// btnAddGroup
			// 
			this.btnAddGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnAddGroup.Enabled = false;
			this.btnAddGroup.Image = global::OnlineVideos.MediaPortal1.Properties.Resources.NewFolderHS;
			this.btnAddGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnAddGroup.Name = "btnAddGroup";
			this.btnAddGroup.Size = new System.Drawing.Size(23, 22);
			this.btnAddGroup.Text = "Add Group";
			this.btnAddGroup.Click += new System.EventHandler(this.btnAddGroup_Click);
			// 
			// btnAddChannel
			// 
			this.btnAddChannel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnAddChannel.Enabled = false;
			this.btnAddChannel.Image = global::OnlineVideos.MediaPortal1.Properties.Resources.Add;
			this.btnAddChannel.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnAddChannel.Name = "btnAddChannel";
			this.btnAddChannel.Size = new System.Drawing.Size(23, 22);
			this.btnAddChannel.Text = "Add Stream";
			this.btnAddChannel.Click += new System.EventHandler(this.btnAddChannel_Click);
			// 
			// btnDeleteChannel
			// 
			this.btnDeleteChannel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnDeleteChannel.Enabled = false;
			this.btnDeleteChannel.Image = global::OnlineVideos.MediaPortal1.Properties.Resources.delete;
			this.btnDeleteChannel.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnDeleteChannel.Name = "btnDeleteChannel";
			this.btnDeleteChannel.Size = new System.Drawing.Size(23, 22);
			this.btnDeleteChannel.Text = "Delete";
			this.btnDeleteChannel.Click += new System.EventHandler(this.btnDeleteChannel_Click);
			// 
			// tbxChannelThumb
			// 
			this.tbxChannelThumb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbxChannelThumb.Enabled = false;
			this.tbxChannelThumb.Location = new System.Drawing.Point(286, 50);
			this.tbxChannelThumb.Name = "tbxChannelThumb";
			this.tbxChannelThumb.Size = new System.Drawing.Size(404, 20);
			this.tbxChannelThumb.TabIndex = 29;
			// 
			// label20
			// 
			this.label20.AutoSize = true;
			this.label20.Location = new System.Drawing.Point(203, 53);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(56, 13);
			this.label20.TabIndex = 28;
			this.label20.Text = "Thumb Url";
			// 
			// label17
			// 
			this.label17.AutoSize = true;
			this.label17.Location = new System.Drawing.Point(203, 141);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(56, 13);
			this.label17.TabIndex = 26;
			this.label17.Text = "Thumb Url";
			// 
			// tbxStreamThumb
			// 
			this.tbxStreamThumb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbxStreamThumb.Enabled = false;
			this.tbxStreamThumb.Location = new System.Drawing.Point(286, 138);
			this.tbxStreamThumb.Name = "tbxStreamThumb";
			this.tbxStreamThumb.Size = new System.Drawing.Size(404, 20);
			this.tbxStreamThumb.TabIndex = 27;
			// 
			// btnSaveChannel
			// 
			this.btnSaveChannel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSaveChannel.Enabled = false;
			this.btnSaveChannel.Image = global::OnlineVideos.MediaPortal1.Properties.Resources.Save;
			this.btnSaveChannel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnSaveChannel.Location = new System.Drawing.Point(594, 162);
			this.btnSaveChannel.Name = "btnSaveChannel";
			this.btnSaveChannel.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.btnSaveChannel.Size = new System.Drawing.Size(95, 23);
			this.btnSaveChannel.TabIndex = 22;
			this.btnSaveChannel.Text = "Save";
			this.btnSaveChannel.UseVisualStyleBackColor = true;
			this.btnSaveChannel.Click += new System.EventHandler(this.btnSaveChannel_Click);
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(203, 117);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(56, 13);
			this.label10.TabIndex = 20;
			this.label10.Text = "Stream Url";
			// 
			// tbxStreamUrl
			// 
			this.tbxStreamUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbxStreamUrl.Enabled = false;
			this.tbxStreamUrl.Location = new System.Drawing.Point(286, 114);
			this.tbxStreamUrl.Name = "tbxStreamUrl";
			this.tbxStreamUrl.Size = new System.Drawing.Size(404, 20);
			this.tbxStreamUrl.TabIndex = 21;
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(203, 92);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(71, 13);
			this.label9.TabIndex = 18;
			this.label9.Text = "Stream Name";
			// 
			// tbxStreamName
			// 
			this.tbxStreamName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbxStreamName.Enabled = false;
			this.tbxStreamName.Location = new System.Drawing.Point(286, 89);
			this.tbxStreamName.Name = "tbxStreamName";
			this.tbxStreamName.Size = new System.Drawing.Size(404, 20);
			this.tbxStreamName.TabIndex = 19;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(203, 28);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(77, 13);
			this.label8.TabIndex = 16;
			this.label8.Text = "Channel Name";
			// 
			// tbxChannelName
			// 
			this.tbxChannelName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbxChannelName.Enabled = false;
			this.tbxChannelName.Location = new System.Drawing.Point(286, 25);
			this.tbxChannelName.Name = "tbxChannelName";
			this.tbxChannelName.Size = new System.Drawing.Size(404, 20);
			this.tbxChannelName.TabIndex = 17;
			// 
			// imageList1
			// 
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList1.Images.SetKeyName(0, "Rss.png");
			this.imageList1.Images.SetKeyName(1, "Tv.png");
			// 
			// tbxPin
			// 
			this.tbxPin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbxPin.Enabled = false;
			this.tbxPin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbxPin.Location = new System.Drawing.Point(276, 22);
			this.tbxPin.Margin = new System.Windows.Forms.Padding(2);
			this.tbxPin.Name = "tbxPin";
			this.tbxPin.PasswordChar = '*';
			this.tbxPin.Size = new System.Drawing.Size(409, 20);
			this.tbxPin.TabIndex = 8;
			// 
			// tbxScreenName
			// 
			this.tbxScreenName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbxScreenName.Location = new System.Drawing.Point(225, 9);
			this.tbxScreenName.Name = "tbxScreenName";
			this.tbxScreenName.Size = new System.Drawing.Size(460, 20);
			this.tbxScreenName.TabIndex = 1;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(6, 11);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(129, 13);
			this.label7.TabIndex = 0;
			this.label7.Text = "BasicHome Screen Name";
			// 
			// btnBrowseForDlFolder
			// 
			this.btnBrowseForDlFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowseForDlFolder.AutoSize = true;
			this.btnBrowseForDlFolder.Location = new System.Drawing.Point(655, 34);
			this.btnBrowseForDlFolder.Margin = new System.Windows.Forms.Padding(2);
			this.btnBrowseForDlFolder.Name = "btnBrowseForDlFolder";
			this.btnBrowseForDlFolder.Size = new System.Drawing.Size(30, 23);
			this.btnBrowseForDlFolder.TabIndex = 3;
			this.btnBrowseForDlFolder.Text = "...";
			this.btnBrowseForDlFolder.UseVisualStyleBackColor = true;
			this.btnBrowseForDlFolder.Click += new System.EventHandler(this.btnBrowseForDlFolder_Click);
			// 
			// mainTabControl
			// 
			this.mainTabControl.Controls.Add(this.tabGeneral);
			this.mainTabControl.Controls.Add(this.tabGroups);
			this.mainTabControl.Controls.Add(this.tabSites);
			this.mainTabControl.Controls.Add(this.tabHosters);
			this.mainTabControl.Controls.Add(this.tabPageCodecs);
			this.mainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainTabControl.Location = new System.Drawing.Point(0, 0);
			this.mainTabControl.Margin = new System.Windows.Forms.Padding(2);
			this.mainTabControl.Name = "mainTabControl";
			this.mainTabControl.SelectedIndex = 0;
			this.mainTabControl.Size = new System.Drawing.Size(712, 568);
			this.mainTabControl.TabIndex = 0;
			// 
			// tabGeneral
			// 
			this.tabGeneral.Controls.Add(this.pictureBox6);
			this.tabGeneral.Controls.Add(this.groupBoxLatestVideos);
			this.tabGeneral.Controls.Add(this.tbxUpdatePeriod);
			this.tabGeneral.Controls.Add(this.label45);
			this.tabGeneral.Controls.Add(this.label44);
			this.tabGeneral.Controls.Add(this.btnWiki);
			this.tabGeneral.Controls.Add(this.pictureBox5);
			this.tabGeneral.Controls.Add(this.groupBox5);
			this.tabGeneral.Controls.Add(this.pictureBox4);
			this.tabGeneral.Controls.Add(this.groupBox4);
			this.tabGeneral.Controls.Add(this.pictureBox3);
			this.tabGeneral.Controls.Add(this.pictureBox2);
			this.tabGeneral.Controls.Add(this.pictureBox1);
			this.tabGeneral.Controls.Add(this.label33);
			this.tabGeneral.Controls.Add(this.chkUseQuickSelect);
			this.tabGeneral.Controls.Add(this.groupBox3);
			this.tabGeneral.Controls.Add(this.groupBox2);
			this.tabGeneral.Controls.Add(this.label23);
			this.tabGeneral.Controls.Add(this.chkDoAutoUpdate);
			this.tabGeneral.Controls.Add(this.btnCancel);
			this.tabGeneral.Controls.Add(this.lblVersion);
			this.tabGeneral.Controls.Add(this.btnBrowseForDlFolder);
			this.tabGeneral.Controls.Add(this.btnSave);
			this.tabGeneral.Controls.Add(this.tbxScreenName);
			this.tabGeneral.Controls.Add(this.label7);
			this.tabGeneral.Controls.Add(this.label3);
			this.tabGeneral.Controls.Add(this.txtDownloadDir);
			this.tabGeneral.Controls.Add(this.Thumbnails);
			this.tabGeneral.Location = new System.Drawing.Point(4, 22);
			this.tabGeneral.Margin = new System.Windows.Forms.Padding(2);
			this.tabGeneral.Name = "tabGeneral";
			this.tabGeneral.Padding = new System.Windows.Forms.Padding(2);
			this.tabGeneral.Size = new System.Drawing.Size(704, 542);
			this.tabGeneral.TabIndex = 0;
			this.tabGeneral.Text = "General";
			this.tabGeneral.UseVisualStyleBackColor = true;
			// 
			// pictureBox6
			// 
			this.pictureBox6.Image = global::OnlineVideos.MediaPortal1.Properties.Resources.Latest;
			this.pictureBox6.Location = new System.Drawing.Point(5, 442);
			this.pictureBox6.Name = "pictureBox6";
			this.pictureBox6.Size = new System.Drawing.Size(24, 24);
			this.pictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox6.TabIndex = 72;
			this.pictureBox6.TabStop = false;
			// 
			// groupBoxLatestVideos
			// 
			this.groupBoxLatestVideos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBoxLatestVideos.Controls.Add(this.chkLatestVideosRandomize);
			this.groupBoxLatestVideos.Controls.Add(this.label53);
			this.groupBoxLatestVideos.Controls.Add(this.label52);
			this.groupBoxLatestVideos.Controls.Add(this.tbxLatestVideosGuiRefresh);
			this.groupBoxLatestVideos.Controls.Add(this.tbxLatestVideosOnlineRefresh);
			this.groupBoxLatestVideos.Controls.Add(this.tbxLatestVideosAmount);
			this.groupBoxLatestVideos.Controls.Add(this.label51);
			this.groupBoxLatestVideos.Controls.Add(this.label50);
			this.groupBoxLatestVideos.Controls.Add(this.label49);
			this.groupBoxLatestVideos.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBoxLatestVideos.Location = new System.Drawing.Point(0, 447);
			this.groupBoxLatestVideos.Name = "groupBoxLatestVideos";
			this.groupBoxLatestVideos.Size = new System.Drawing.Size(699, 56);
			this.groupBoxLatestVideos.TabIndex = 71;
			this.groupBoxLatestVideos.TabStop = false;
			this.groupBoxLatestVideos.Text = "      Latest Videos";
			// 
			// chkLatestVideosRandomize
			// 
			this.chkLatestVideosRandomize.AutoSize = true;
			this.chkLatestVideosRandomize.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkLatestVideosRandomize.Location = new System.Drawing.Point(200, 24);
			this.chkLatestVideosRandomize.Name = "chkLatestVideosRandomize";
			this.chkLatestVideosRandomize.Size = new System.Drawing.Size(79, 17);
			this.chkLatestVideosRandomize.TabIndex = 49;
			this.chkLatestVideosRandomize.Text = "Randomize";
			this.toolTip1.SetToolTip(this.chkLatestVideosRandomize, "After retrieving all latest videos from all sites randomize them before displayin" +
        "g.");
			this.chkLatestVideosRandomize.UseVisualStyleBackColor = true;
			// 
			// label53
			// 
			this.label53.AutoSize = true;
			this.label53.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label53.Location = new System.Drawing.Point(638, 25);
			this.label53.Name = "label53";
			this.label53.Size = new System.Drawing.Size(24, 13);
			this.label53.TabIndex = 48;
			this.label53.Text = "sec";
			// 
			// label52
			// 
			this.label52.AutoSize = true;
			this.label52.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label52.Location = new System.Drawing.Point(459, 25);
			this.label52.Name = "label52";
			this.label52.Size = new System.Drawing.Size(23, 13);
			this.label52.TabIndex = 48;
			this.label52.Text = "min";
			// 
			// tbxLatestVideosGuiRefresh
			// 
			this.tbxLatestVideosGuiRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbxLatestVideosGuiRefresh.Location = new System.Drawing.Point(574, 22);
			this.tbxLatestVideosGuiRefresh.Name = "tbxLatestVideosGuiRefresh";
			this.tbxLatestVideosGuiRefresh.Size = new System.Drawing.Size(43, 20);
			this.tbxLatestVideosGuiRefresh.TabIndex = 24;
			this.tbxLatestVideosGuiRefresh.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.toolTip1.SetToolTip(this.tbxLatestVideosGuiRefresh, "Seconds after which latest video items rotata to show all items when more are ava" +
        "ilable than shown concurrently.");
			this.tbxLatestVideosGuiRefresh.Validating += new System.ComponentModel.CancelEventHandler(this.CheckValidNumber);
			// 
			// tbxLatestVideosOnlineRefresh
			// 
			this.tbxLatestVideosOnlineRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbxLatestVideosOnlineRefresh.Location = new System.Drawing.Point(387, 22);
			this.tbxLatestVideosOnlineRefresh.Name = "tbxLatestVideosOnlineRefresh";
			this.tbxLatestVideosOnlineRefresh.Size = new System.Drawing.Size(53, 20);
			this.tbxLatestVideosOnlineRefresh.TabIndex = 23;
			this.tbxLatestVideosOnlineRefresh.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.toolTip1.SetToolTip(this.tbxLatestVideosOnlineRefresh, "Minutes after which all configured sites will be asked for new latest videos to d" +
        "isplay.");
			this.tbxLatestVideosOnlineRefresh.Validating += new System.ComponentModel.CancelEventHandler(this.CheckValidNumber);
			// 
			// tbxLatestVideosAmount
			// 
			this.tbxLatestVideosAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbxLatestVideosAmount.Location = new System.Drawing.Point(108, 22);
			this.tbxLatestVideosAmount.Name = "tbxLatestVideosAmount";
			this.tbxLatestVideosAmount.Size = new System.Drawing.Size(53, 20);
			this.tbxLatestVideosAmount.TabIndex = 22;
			this.tbxLatestVideosAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.toolTip1.SetToolTip(this.tbxLatestVideosAmount, "Number of latest video items to set data concurrently. Default is 3. Set to 0 to " +
        "disable this feature.");
			this.tbxLatestVideosAmount.Validating += new System.ComponentModel.CancelEventHandler(this.CheckValidNumber);
			// 
			// label51
			// 
			this.label51.AutoSize = true;
			this.label51.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label51.Location = new System.Drawing.Point(502, 25);
			this.label51.Name = "label51";
			this.label51.Size = new System.Drawing.Size(66, 13);
			this.label51.TabIndex = 2;
			this.label51.Text = "Refresh GUI";
			// 
			// label50
			// 
			this.label50.AutoSize = true;
			this.label50.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label50.Location = new System.Drawing.Point(311, 25);
			this.label50.Name = "label50";
			this.label50.Size = new System.Drawing.Size(70, 13);
			this.label50.TabIndex = 1;
			this.label50.Text = "Refresh Data";
			// 
			// label49
			// 
			this.label49.AutoSize = true;
			this.label49.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label49.Location = new System.Drawing.Point(6, 25);
			this.label49.Name = "label49";
			this.label49.Size = new System.Drawing.Size(80, 13);
			this.label49.TabIndex = 0;
			this.label49.Text = "Display Amount";
			// 
			// tbxUpdatePeriod
			// 
			this.tbxUpdatePeriod.Location = new System.Drawing.Point(309, 58);
			this.tbxUpdatePeriod.Name = "tbxUpdatePeriod";
			this.tbxUpdatePeriod.Size = new System.Drawing.Size(40, 20);
			this.tbxUpdatePeriod.TabIndex = 5;
			this.toolTip1.SetToolTip(this.tbxUpdatePeriod, "Automatic update and thumbnail deletion on startup will only run after this many " +
        "hours passed since the last run.");
			this.tbxUpdatePeriod.Validating += new System.ComponentModel.CancelEventHandler(this.CheckValidNumber);
			// 
			// label45
			// 
			this.label45.AutoSize = true;
			this.label45.Location = new System.Drawing.Point(355, 61);
			this.label45.Name = "label45";
			this.label45.Size = new System.Drawing.Size(33, 13);
			this.label45.TabIndex = 69;
			this.label45.Text = "hours";
			// 
			// label44
			// 
			this.label44.AutoSize = true;
			this.label44.Location = new System.Drawing.Point(246, 61);
			this.label44.Name = "label44";
			this.label44.Size = new System.Drawing.Size(57, 13);
			this.label44.TabIndex = 68;
			this.label44.Text = "Only every";
			// 
			// btnWiki
			// 
			this.btnWiki.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnWiki.Image = global::OnlineVideos.MediaPortal1.Properties.Resources.help;
			this.btnWiki.Location = new System.Drawing.Point(4, 516);
			this.btnWiki.Name = "btnWiki";
			this.btnWiki.Size = new System.Drawing.Size(23, 23);
			this.btnWiki.TabIndex = 25;
			this.toolTip1.SetToolTip(this.btnWiki, "Open the OnlineVideos Wiki.");
			this.btnWiki.UseVisualStyleBackColor = true;
			this.btnWiki.Click += new System.EventHandler(this.btnWiki_Click);
			// 
			// pictureBox5
			// 
			this.pictureBox5.Image = global::OnlineVideos.MediaPortal1.Properties.Resources.key;
			this.pictureBox5.Location = new System.Drawing.Point(4, 100);
			this.pictureBox5.Name = "pictureBox5";
			this.pictureBox5.Size = new System.Drawing.Size(24, 24);
			this.pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox5.TabIndex = 66;
			this.pictureBox5.TabStop = false;
			// 
			// groupBox5
			// 
			this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox5.BackColor = System.Drawing.Color.Transparent;
			this.groupBox5.Controls.Add(this.label5);
			this.groupBox5.Controls.Add(this.chkUseAgeConfirmation);
			this.groupBox5.Controls.Add(this.label21);
			this.groupBox5.Controls.Add(this.tbxPin);
			this.groupBox5.Controls.Add(this.label2);
			this.groupBox5.Controls.Add(this.txtFilters);
			this.groupBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox5.Location = new System.Drawing.Point(0, 105);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(699, 80);
			this.groupBox5.TabIndex = 65;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "      Adult content";
			// 
			// label21
			// 
			this.label21.AutoSize = true;
			this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label21.Location = new System.Drawing.Point(246, 25);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(22, 13);
			this.label21.TabIndex = 0;
			this.label21.Text = "Pin";
			// 
			// pictureBox4
			// 
			this.pictureBox4.Image = global::OnlineVideos.MediaPortal1.Properties.Resources.search;
			this.pictureBox4.Location = new System.Drawing.Point(5, 187);
			this.pictureBox4.Name = "pictureBox4";
			this.pictureBox4.Size = new System.Drawing.Size(24, 24);
			this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox4.TabIndex = 64;
			this.pictureBox4.TabStop = false;
			// 
			// groupBox4
			// 
			this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox4.Controls.Add(this.label34);
			this.groupBox4.Controls.Add(this.rbOff);
			this.groupBox4.Controls.Add(this.rbLastSearch);
			this.groupBox4.Controls.Add(this.rbExtendedSearchHistory);
			this.groupBox4.Controls.Add(this.label38);
			this.groupBox4.Controls.Add(this.nUPSearchHistoryItemCount);
			this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox4.Location = new System.Drawing.Point(0, 192);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(699, 56);
			this.groupBox4.TabIndex = 63;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "      Search history";
			// 
			// label34
			// 
			this.label34.AutoSize = true;
			this.label34.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label34.Location = new System.Drawing.Point(6, 26);
			this.label34.Name = "label34";
			this.label34.Size = new System.Drawing.Size(34, 13);
			this.label34.TabIndex = 0;
			this.label34.Text = "Mode";
			// 
			// rbOff
			// 
			this.rbOff.AutoSize = true;
			this.rbOff.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rbOff.Location = new System.Drawing.Point(225, 24);
			this.rbOff.Name = "rbOff";
			this.rbOff.Size = new System.Drawing.Size(39, 17);
			this.rbOff.TabIndex = 10;
			this.rbOff.TabStop = true;
			this.rbOff.Text = "Off";
			this.rbOff.UseVisualStyleBackColor = true;
			this.rbOff.CheckedChanged += new System.EventHandler(this.searchType_CheckedChanged);
			// 
			// rbLastSearch
			// 
			this.rbLastSearch.AutoSize = true;
			this.rbLastSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rbLastSearch.Location = new System.Drawing.Point(270, 24);
			this.rbLastSearch.Name = "rbLastSearch";
			this.rbLastSearch.Size = new System.Drawing.Size(80, 17);
			this.rbLastSearch.TabIndex = 11;
			this.rbLastSearch.TabStop = true;
			this.rbLastSearch.Text = "Last search";
			this.rbLastSearch.UseVisualStyleBackColor = true;
			this.rbLastSearch.CheckedChanged += new System.EventHandler(this.searchType_CheckedChanged);
			// 
			// rbExtendedSearchHistory
			// 
			this.rbExtendedSearchHistory.AutoSize = true;
			this.rbExtendedSearchHistory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rbExtendedSearchHistory.Location = new System.Drawing.Point(356, 24);
			this.rbExtendedSearchHistory.Name = "rbExtendedSearchHistory";
			this.rbExtendedSearchHistory.Size = new System.Drawing.Size(107, 17);
			this.rbExtendedSearchHistory.TabIndex = 12;
			this.rbExtendedSearchHistory.TabStop = true;
			this.rbExtendedSearchHistory.Text = "Extended (dialog)";
			this.rbExtendedSearchHistory.UseVisualStyleBackColor = true;
			this.rbExtendedSearchHistory.CheckedChanged += new System.EventHandler(this.searchType_CheckedChanged);
			// 
			// label38
			// 
			this.label38.AutoSize = true;
			this.label38.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label38.Location = new System.Drawing.Point(638, 26);
			this.label38.Name = "label38";
			this.label38.Size = new System.Drawing.Size(32, 13);
			this.label38.TabIndex = 0;
			this.label38.Text = "Items";
			// 
			// nUPSearchHistoryItemCount
			// 
			this.nUPSearchHistoryItemCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.nUPSearchHistoryItemCount.Location = new System.Drawing.Point(541, 21);
			this.nUPSearchHistoryItemCount.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
			this.nUPSearchHistoryItemCount.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
			this.nUPSearchHistoryItemCount.Name = "nUPSearchHistoryItemCount";
			this.nUPSearchHistoryItemCount.Size = new System.Drawing.Size(76, 20);
			this.nUPSearchHistoryItemCount.TabIndex = 13;
			this.toolTip1.SetToolTip(this.nUPSearchHistoryItemCount, "Defines the number of search strings stored per Site.");
			this.nUPSearchHistoryItemCount.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
			// 
			// pictureBox3
			// 
			this.pictureBox3.Image = global::OnlineVideos.MediaPortal1.Properties.Resources.thumbnail;
			this.pictureBox3.Location = new System.Drawing.Point(5, 380);
			this.pictureBox3.Name = "pictureBox3";
			this.pictureBox3.Size = new System.Drawing.Size(24, 24);
			this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox3.TabIndex = 62;
			this.pictureBox3.TabStop = false;
			// 
			// pictureBox2
			// 
			this.pictureBox2.Image = global::OnlineVideos.MediaPortal1.Properties.Resources.cache;
			this.pictureBox2.Location = new System.Drawing.Point(5, 315);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(24, 24);
			this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox2.TabIndex = 50;
			this.pictureBox2.TabStop = false;
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::OnlineVideos.MediaPortal1.Properties.Resources.timeout;
			this.pictureBox1.Location = new System.Drawing.Point(5, 251);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(24, 24);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox1.TabIndex = 45;
			this.pictureBox1.TabStop = false;
			// 
			// label33
			// 
			this.label33.AutoSize = true;
			this.label33.Location = new System.Drawing.Point(6, 81);
			this.label33.Name = "label33";
			this.label33.Size = new System.Drawing.Size(90, 13);
			this.label33.TabIndex = 0;
			this.label33.Text = "Use Quick Select";
			// 
			// chkUseQuickSelect
			// 
			this.chkUseQuickSelect.AutoSize = true;
			this.chkUseQuickSelect.Location = new System.Drawing.Point(225, 81);
			this.chkUseQuickSelect.Name = "chkUseQuickSelect";
			this.chkUseQuickSelect.Size = new System.Drawing.Size(15, 14);
			this.chkUseQuickSelect.TabIndex = 6;
			this.toolTip1.SetToolTip(this.chkUseQuickSelect, "Allows you to quickly select entries that start with the letter or number you pre" +
        "ssed in the list.");
			this.chkUseQuickSelect.UseVisualStyleBackColor = true;
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.groupBox3.Controls.Add(this.label48);
			this.groupBox3.Controls.Add(this.label47);
			this.groupBox3.Controls.Add(this.tbxCategoriesTimeout);
			this.groupBox3.Controls.Add(this.label32);
			this.groupBox3.Controls.Add(this.label31);
			this.groupBox3.Controls.Add(this.label6);
			this.groupBox3.Controls.Add(this.tbxWebCacheTimeout);
			this.groupBox3.Controls.Add(this.label15);
			this.groupBox3.Controls.Add(this.tbxUtilTimeout);
			this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox3.Location = new System.Drawing.Point(0, 256);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(699, 56);
			this.groupBox3.TabIndex = 50;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "      Timeouts";
			// 
			// label48
			// 
			this.label48.AutoSize = true;
			this.label48.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label48.Location = new System.Drawing.Point(222, 26);
			this.label48.Name = "label48";
			this.label48.Size = new System.Drawing.Size(57, 13);
			this.label48.TabIndex = 47;
			this.label48.Text = "Categories";
			// 
			// label47
			// 
			this.label47.AutoSize = true;
			this.label47.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label47.Location = new System.Drawing.Point(358, 26);
			this.label47.Name = "label47";
			this.label47.Size = new System.Drawing.Size(23, 13);
			this.label47.TabIndex = 46;
			this.label47.Text = "min";
			// 
			// tbxCategoriesTimeout
			// 
			this.tbxCategoriesTimeout.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbxCategoriesTimeout.Location = new System.Drawing.Point(285, 23);
			this.tbxCategoriesTimeout.Name = "tbxCategoriesTimeout";
			this.tbxCategoriesTimeout.Size = new System.Drawing.Size(53, 20);
			this.tbxCategoriesTimeout.TabIndex = 15;
			this.tbxCategoriesTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.toolTip1.SetToolTip(this.tbxCategoriesTimeout, "Minutes after which a sites dynamic categories are retrieved from the website aga" +
        "in. Default is 300 (5h).");
			this.tbxCategoriesTimeout.Validating += new System.ComponentModel.CancelEventHandler(this.CheckValidNumber);
			// 
			// label32
			// 
			this.label32.AutoSize = true;
			this.label32.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label32.Location = new System.Drawing.Point(638, 26);
			this.label32.Name = "label32";
			this.label32.Size = new System.Drawing.Size(24, 13);
			this.label32.TabIndex = 44;
			this.label32.Text = "sec";
			// 
			// label31
			// 
			this.label31.AutoSize = true;
			this.label31.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label31.Location = new System.Drawing.Point(178, 26);
			this.label31.Name = "label31";
			this.label31.Size = new System.Drawing.Size(23, 13);
			this.label31.TabIndex = 43;
			this.label31.Text = "min";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(6, 26);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(91, 13);
			this.label6.TabIndex = 40;
			this.label6.Text = "Cached Webdata";
			// 
			// tbxWebCacheTimeout
			// 
			this.tbxWebCacheTimeout.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbxWebCacheTimeout.Location = new System.Drawing.Point(108, 23);
			this.tbxWebCacheTimeout.Name = "tbxWebCacheTimeout";
			this.tbxWebCacheTimeout.Size = new System.Drawing.Size(53, 20);
			this.tbxWebCacheTimeout.TabIndex = 14;
			this.tbxWebCacheTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.toolTip1.SetToolTip(this.tbxWebCacheTimeout, "WebRequests are cached internally. This number determines the minutes after which" +
        " the cached data becomes invalid. Set to 0 to disable.");
			this.tbxWebCacheTimeout.Validating += new System.ComponentModel.CancelEventHandler(this.CheckValidNumber);
			// 
			// label15
			// 
			this.label15.AutoSize = true;
			this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label15.Location = new System.Drawing.Point(465, 26);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(70, 13);
			this.label15.TabIndex = 42;
			this.label15.Text = "Webrequests";
			// 
			// tbxUtilTimeout
			// 
			this.tbxUtilTimeout.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbxUtilTimeout.Location = new System.Drawing.Point(541, 23);
			this.tbxUtilTimeout.Name = "tbxUtilTimeout";
			this.tbxUtilTimeout.Size = new System.Drawing.Size(76, 20);
			this.tbxUtilTimeout.TabIndex = 16;
			this.tbxUtilTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.toolTip1.SetToolTip(this.tbxUtilTimeout, "When the GUI request data from the web you can specifiy how many seconds to wait " +
        "before a timeout will occur.");
			this.tbxUtilTimeout.Validating += new System.ComponentModel.CancelEventHandler(this.CheckValidNumber);
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.label30);
			this.groupBox2.Controls.Add(this.label29);
			this.groupBox2.Controls.Add(this.udPlayBuffer);
			this.groupBox2.Controls.Add(this.label16);
			this.groupBox2.Controls.Add(this.label24);
			this.groupBox2.Controls.Add(this.tbxWMPBuffer);
			this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox2.Location = new System.Drawing.Point(0, 321);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(699, 56);
			this.groupBox2.TabIndex = 49;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "      Playback Buffer";
			// 
			// label30
			// 
			this.label30.AutoSize = true;
			this.label30.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label30.Location = new System.Drawing.Point(291, 26);
			this.label30.Name = "label30";
			this.label30.Size = new System.Drawing.Size(15, 13);
			this.label30.TabIndex = 49;
			this.label30.Text = "%";
			// 
			// label29
			// 
			this.label29.AutoSize = true;
			this.label29.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label29.Location = new System.Drawing.Point(638, 26);
			this.label29.Name = "label29";
			this.label29.Size = new System.Drawing.Size(32, 13);
			this.label29.TabIndex = 44;
			this.label29.Text = "msec";
			// 
			// udPlayBuffer
			// 
			this.udPlayBuffer.BackColor = System.Drawing.SystemColors.Window;
			this.udPlayBuffer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.udPlayBuffer.Items.Add("20");
			this.udPlayBuffer.Items.Add("19");
			this.udPlayBuffer.Items.Add("18");
			this.udPlayBuffer.Items.Add("17");
			this.udPlayBuffer.Items.Add("16");
			this.udPlayBuffer.Items.Add("15");
			this.udPlayBuffer.Items.Add("14");
			this.udPlayBuffer.Items.Add("13");
			this.udPlayBuffer.Items.Add("12");
			this.udPlayBuffer.Items.Add("11");
			this.udPlayBuffer.Items.Add("10");
			this.udPlayBuffer.Items.Add("9");
			this.udPlayBuffer.Items.Add("8");
			this.udPlayBuffer.Items.Add("7");
			this.udPlayBuffer.Items.Add("6");
			this.udPlayBuffer.Items.Add("5");
			this.udPlayBuffer.Items.Add("4");
			this.udPlayBuffer.Items.Add("3");
			this.udPlayBuffer.Items.Add("2");
			this.udPlayBuffer.Items.Add("1");
			this.udPlayBuffer.Location = new System.Drawing.Point(225, 24);
			this.udPlayBuffer.Name = "udPlayBuffer";
			this.udPlayBuffer.ReadOnly = true;
			this.udPlayBuffer.Size = new System.Drawing.Size(53, 20);
			this.udPlayBuffer.TabIndex = 17;
			this.udPlayBuffer.Text = "1";
			this.udPlayBuffer.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.toolTip1.SetToolTip(this.udPlayBuffer, "Percentage of the file to buffer from web before starting playback.");
			// 
			// label16
			// 
			this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label16.Location = new System.Drawing.Point(393, 17);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(127, 28);
			this.label16.TabIndex = 43;
			this.label16.Text = "Windows Media Player VLC Media Player";
			// 
			// label24
			// 
			this.label24.AutoSize = true;
			this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label24.Location = new System.Drawing.Point(6, 26);
			this.label24.Name = "label24";
			this.label24.Size = new System.Drawing.Size(101, 13);
			this.label24.TabIndex = 48;
			this.label24.Text = "Internal Player (http)";
			// 
			// tbxWMPBuffer
			// 
			this.tbxWMPBuffer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbxWMPBuffer.Location = new System.Drawing.Point(541, 23);
			this.tbxWMPBuffer.Name = "tbxWMPBuffer";
			this.tbxWMPBuffer.Size = new System.Drawing.Size(76, 20);
			this.tbxWMPBuffer.TabIndex = 18;
			this.tbxWMPBuffer.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.toolTip1.SetToolTip(this.tbxWMPBuffer, "Number of milliseconds to use as buffer for playback with Windows Media Player.");
			this.tbxWMPBuffer.Validating += new System.ComponentModel.CancelEventHandler(this.CheckValidNumber);
			// 
			// label23
			// 
			this.label23.AutoSize = true;
			this.label23.Location = new System.Drawing.Point(6, 61);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(125, 13);
			this.label23.TabIndex = 0;
			this.label23.Text = "Update Sites on first load";
			// 
			// chkDoAutoUpdate
			// 
			this.chkDoAutoUpdate.AutoSize = true;
			this.chkDoAutoUpdate.Checked = true;
			this.chkDoAutoUpdate.CheckState = System.Windows.Forms.CheckState.Indeterminate;
			this.chkDoAutoUpdate.Location = new System.Drawing.Point(225, 61);
			this.chkDoAutoUpdate.Name = "chkDoAutoUpdate";
			this.chkDoAutoUpdate.Size = new System.Drawing.Size(15, 14);
			this.chkDoAutoUpdate.TabIndex = 4;
			this.chkDoAutoUpdate.ThreeState = true;
			this.toolTip1.SetToolTip(this.chkDoAutoUpdate, "If checked plugin will perform an autoupdate the first time it is started each Me" +
        "diaPortal Session. If indeterminate, plugin will ask.");
			this.chkDoAutoUpdate.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.CausesValidation = false;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.No;
			this.btnCancel.Image = global::OnlineVideos.MediaPortal1.Properties.Resources.delete;
			this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnCancel.Location = new System.Drawing.Point(595, 512);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(90, 25);
			this.btnCancel.TabIndex = 27;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// lblVersion
			// 
			this.lblVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblVersion.Location = new System.Drawing.Point(32, 520);
			this.lblVersion.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblVersion.Name = "lblVersion";
			this.lblVersion.Size = new System.Drawing.Size(200, 14);
			this.lblVersion.TabIndex = 37;
			this.lblVersion.Text = "Version";
			this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// Thumbnails
			// 
			this.Thumbnails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Thumbnails.Controls.Add(this.label36);
			this.Thumbnails.Controls.Add(this.tbxThumbAge);
			this.Thumbnails.Controls.Add(this.label35);
			this.Thumbnails.Controls.Add(this.label1);
			this.Thumbnails.Controls.Add(this.bntBrowseFolderForThumbs);
			this.Thumbnails.Controls.Add(this.txtThumbLoc);
			this.Thumbnails.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Thumbnails.Location = new System.Drawing.Point(0, 384);
			this.Thumbnails.Name = "Thumbnails";
			this.Thumbnails.Size = new System.Drawing.Size(699, 56);
			this.Thumbnails.TabIndex = 56;
			this.Thumbnails.TabStop = false;
			this.Thumbnails.Text = "      Thumbnails";
			// 
			// label36
			// 
			this.label36.AutoSize = true;
			this.label36.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label36.Location = new System.Drawing.Point(178, 26);
			this.label36.Name = "label36";
			this.label36.Size = new System.Drawing.Size(29, 13);
			this.label36.TabIndex = 45;
			this.label36.Text = "days";
			// 
			// tbxThumbAge
			// 
			this.tbxThumbAge.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbxThumbAge.Location = new System.Drawing.Point(108, 23);
			this.tbxThumbAge.Name = "tbxThumbAge";
			this.tbxThumbAge.Size = new System.Drawing.Size(53, 20);
			this.tbxThumbAge.TabIndex = 19;
			this.tbxThumbAge.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.toolTip1.SetToolTip(this.tbxThumbAge, "Thumbnails older than this will be deleted on first OnlineVideos start each time " +
        "MediaPortal runs. Set to 0 to delete all and -1 to keep all.");
			this.tbxThumbAge.Validating += new System.ComponentModel.CancelEventHandler(this.CheckValidInteger);
			// 
			// label35
			// 
			this.label35.AutoSize = true;
			this.label35.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label35.Location = new System.Drawing.Point(6, 26);
			this.label35.Name = "label35";
			this.label35.Size = new System.Drawing.Size(73, 13);
			this.label35.TabIndex = 56;
			this.label35.Text = "Maximum Age";
			// 
			// bntBrowseFolderForThumbs
			// 
			this.bntBrowseFolderForThumbs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.bntBrowseFolderForThumbs.AutoSize = true;
			this.bntBrowseFolderForThumbs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.bntBrowseFolderForThumbs.Location = new System.Drawing.Point(655, 21);
			this.bntBrowseFolderForThumbs.Margin = new System.Windows.Forms.Padding(2);
			this.bntBrowseFolderForThumbs.Name = "bntBrowseFolderForThumbs";
			this.bntBrowseFolderForThumbs.Size = new System.Drawing.Size(30, 23);
			this.bntBrowseFolderForThumbs.TabIndex = 21;
			this.bntBrowseFolderForThumbs.Text = "...";
			this.bntBrowseFolderForThumbs.UseVisualStyleBackColor = true;
			this.bntBrowseFolderForThumbs.Click += new System.EventHandler(this.bntBrowseFolderForThumbs_Click);
			// 
			// tabGroups
			// 
			this.tabGroups.Controls.Add(this.splitContainer1);
			this.tabGroups.Location = new System.Drawing.Point(4, 22);
			this.tabGroups.Name = "tabGroups";
			this.tabGroups.Size = new System.Drawing.Size(704, 542);
			this.tabGroups.TabIndex = 3;
			this.tabGroups.Text = "Groups";
			this.tabGroups.UseVisualStyleBackColor = true;
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.chkFavFirst);
			this.splitContainer1.Panel1.Controls.Add(this.chkAutoGroupByLang);
			this.splitContainer1.Panel1.Controls.Add(this.btnBrowseSitesGroupThumb);
			this.splitContainer1.Panel1.Controls.Add(this.tbxSitesGroupDesc);
			this.splitContainer1.Panel1.Controls.Add(this.label43);
			this.splitContainer1.Panel1.Controls.Add(this.tbxSitesGroupThumb);
			this.splitContainer1.Panel1.Controls.Add(this.label42);
			this.splitContainer1.Panel1.Controls.Add(this.tbxSitesGroupName);
			this.splitContainer1.Panel1.Controls.Add(this.label39);
			this.splitContainer1.Panel1.Controls.Add(this.toolStripContainer4);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
			this.splitContainer1.Size = new System.Drawing.Size(704, 542);
			this.splitContainer1.SplitterDistance = 240;
			this.splitContainer1.TabIndex = 0;
			// 
			// chkFavFirst
			// 
			this.chkFavFirst.AutoSize = true;
			this.chkFavFirst.Location = new System.Drawing.Point(264, 194);
			this.chkFavFirst.Name = "chkFavFirst";
			this.chkFavFirst.Size = new System.Drawing.Size(277, 17);
			this.chkFavFirst.TabIndex = 18;
			this.chkFavFirst.Text = "Favorites and Downloads first instead of last in the list";
			this.chkFavFirst.UseVisualStyleBackColor = true;
			// 
			// chkAutoGroupByLang
			// 
			this.chkAutoGroupByLang.AutoSize = true;
			this.chkAutoGroupByLang.Location = new System.Drawing.Point(264, 162);
			this.chkAutoGroupByLang.Name = "chkAutoGroupByLang";
			this.chkAutoGroupByLang.Size = new System.Drawing.Size(353, 17);
			this.chkAutoGroupByLang.TabIndex = 17;
			this.chkAutoGroupByLang.Text = "Automatically group all sites by their language if no groups are defined";
			this.chkAutoGroupByLang.UseVisualStyleBackColor = true;
			// 
			// btnBrowseSitesGroupThumb
			// 
			this.btnBrowseSitesGroupThumb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowseSitesGroupThumb.AutoSize = true;
			this.btnBrowseSitesGroupThumb.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnBrowseSitesGroupThumb.Location = new System.Drawing.Point(666, 49);
			this.btnBrowseSitesGroupThumb.Margin = new System.Windows.Forms.Padding(2);
			this.btnBrowseSitesGroupThumb.Name = "btnBrowseSitesGroupThumb";
			this.btnBrowseSitesGroupThumb.Size = new System.Drawing.Size(30, 23);
			this.btnBrowseSitesGroupThumb.TabIndex = 16;
			this.btnBrowseSitesGroupThumb.Text = "...";
			this.btnBrowseSitesGroupThumb.UseVisualStyleBackColor = true;
			this.btnBrowseSitesGroupThumb.Click += new System.EventHandler(this.btnBrowseSitesGroupThumb_Click);
			// 
			// tbxSitesGroupDesc
			// 
			this.tbxSitesGroupDesc.AcceptsReturn = true;
			this.tbxSitesGroupDesc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbxSitesGroupDesc.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourceSitesGroup, "Description", true));
			this.tbxSitesGroupDesc.Location = new System.Drawing.Point(264, 77);
			this.tbxSitesGroupDesc.Multiline = true;
			this.tbxSitesGroupDesc.Name = "tbxSitesGroupDesc";
			this.tbxSitesGroupDesc.Size = new System.Drawing.Size(432, 60);
			this.tbxSitesGroupDesc.TabIndex = 6;
			// 
			// bindingSourceSitesGroup
			// 
			this.bindingSourceSitesGroup.DataSource = typeof(OnlineVideos.MediaPortal1.SitesGroup);
			// 
			// label43
			// 
			this.label43.AutoSize = true;
			this.label43.Location = new System.Drawing.Point(185, 80);
			this.label43.Name = "label43";
			this.label43.Size = new System.Drawing.Size(60, 13);
			this.label43.TabIndex = 5;
			this.label43.Text = "Description";
			// 
			// tbxSitesGroupThumb
			// 
			this.tbxSitesGroupThumb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbxSitesGroupThumb.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourceSitesGroup, "Thumbnail", true));
			this.tbxSitesGroupThumb.Location = new System.Drawing.Point(264, 51);
			this.tbxSitesGroupThumb.Name = "tbxSitesGroupThumb";
			this.tbxSitesGroupThumb.Size = new System.Drawing.Size(397, 20);
			this.tbxSitesGroupThumb.TabIndex = 4;
			// 
			// label42
			// 
			this.label42.AutoSize = true;
			this.label42.Location = new System.Drawing.Point(185, 54);
			this.label42.Name = "label42";
			this.label42.Size = new System.Drawing.Size(40, 13);
			this.label42.TabIndex = 3;
			this.label42.Text = "Thumb";
			// 
			// tbxSitesGroupName
			// 
			this.tbxSitesGroupName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbxSitesGroupName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourceSitesGroup, "Name", true));
			this.tbxSitesGroupName.Location = new System.Drawing.Point(264, 22);
			this.tbxSitesGroupName.Name = "tbxSitesGroupName";
			this.tbxSitesGroupName.Size = new System.Drawing.Size(432, 20);
			this.tbxSitesGroupName.TabIndex = 2;
			// 
			// label39
			// 
			this.label39.AutoSize = true;
			this.label39.Location = new System.Drawing.Point(185, 25);
			this.label39.Name = "label39";
			this.label39.Size = new System.Drawing.Size(35, 13);
			this.label39.TabIndex = 1;
			this.label39.Text = "Name";
			// 
			// toolStripContainer4
			// 
			// 
			// toolStripContainer4.ContentPanel
			// 
			this.toolStripContainer4.ContentPanel.Controls.Add(this.listBoxSitesGroups);
			this.toolStripContainer4.ContentPanel.Size = new System.Drawing.Size(155, 215);
			this.toolStripContainer4.Dock = System.Windows.Forms.DockStyle.Left;
			// 
			// toolStripContainer4.LeftToolStripPanel
			// 
			this.toolStripContainer4.LeftToolStripPanel.Controls.Add(this.toolStrip2);
			this.toolStripContainer4.Location = new System.Drawing.Point(0, 0);
			this.toolStripContainer4.Name = "toolStripContainer4";
			this.toolStripContainer4.Size = new System.Drawing.Size(179, 240);
			this.toolStripContainer4.TabIndex = 0;
			this.toolStripContainer4.Text = "toolStripContainer4";
			// 
			// toolStripContainer4.TopToolStripPanel
			// 
			this.toolStripContainer4.TopToolStripPanel.Controls.Add(this.toolStrip1);
			// 
			// listBoxSitesGroups
			// 
			this.listBoxSitesGroups.DataSource = this.bindingSourceSitesGroup;
			this.listBoxSitesGroups.DisplayMember = "Name";
			this.listBoxSitesGroups.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listBoxSitesGroups.FormattingEnabled = true;
			this.listBoxSitesGroups.Location = new System.Drawing.Point(0, 0);
			this.listBoxSitesGroups.Name = "listBoxSitesGroups";
			this.listBoxSitesGroups.Size = new System.Drawing.Size(155, 215);
			this.listBoxSitesGroups.TabIndex = 0;
			this.listBoxSitesGroups.SelectedValueChanged += new System.EventHandler(this.listBoxSitesGroups_SelectedValueChanged);
			// 
			// toolStrip2
			// 
			this.toolStrip2.Dock = System.Windows.Forms.DockStyle.None;
			this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSitesGroupUp,
            this.btnSitesGroupDown});
			this.toolStrip2.Location = new System.Drawing.Point(0, 0);
			this.toolStrip2.Name = "toolStrip2";
			this.toolStrip2.Size = new System.Drawing.Size(24, 240);
			this.toolStrip2.Stretch = true;
			this.toolStrip2.TabIndex = 0;
			// 
			// btnSitesGroupUp
			// 
			this.btnSitesGroupUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnSitesGroupUp.Enabled = false;
			this.btnSitesGroupUp.Image = global::OnlineVideos.MediaPortal1.Properties.Resources.Up;
			this.btnSitesGroupUp.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnSitesGroupUp.Name = "btnSitesGroupUp";
			this.btnSitesGroupUp.Size = new System.Drawing.Size(22, 20);
			this.btnSitesGroupUp.Text = "Move Group Up";
			this.btnSitesGroupUp.Click += new System.EventHandler(this.btnSitesGroupUp_Click);
			// 
			// btnSitesGroupDown
			// 
			this.btnSitesGroupDown.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.btnSitesGroupDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnSitesGroupDown.Enabled = false;
			this.btnSitesGroupDown.Image = global::OnlineVideos.MediaPortal1.Properties.Resources.Down;
			this.btnSitesGroupDown.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnSitesGroupDown.Name = "btnSitesGroupDown";
			this.btnSitesGroupDown.Size = new System.Drawing.Size(22, 20);
			this.btnSitesGroupDown.Text = "Move Group Down";
			this.btnSitesGroupDown.Click += new System.EventHandler(this.btnSitesGroupDown_Click);
			// 
			// toolStrip1
			// 
			this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
			this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAddSitesGroup,
            this.btnDeleteSitesGroup});
			this.toolStrip1.Location = new System.Drawing.Point(3, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(49, 25);
			this.toolStrip1.TabIndex = 0;
			// 
			// btnAddSitesGroup
			// 
			this.btnAddSitesGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnAddSitesGroup.Image = global::OnlineVideos.MediaPortal1.Properties.Resources.Add;
			this.btnAddSitesGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnAddSitesGroup.Name = "btnAddSitesGroup";
			this.btnAddSitesGroup.Size = new System.Drawing.Size(23, 22);
			this.btnAddSitesGroup.Text = "Add Group";
			this.btnAddSitesGroup.Click += new System.EventHandler(this.btnAddSitesGroup_Click);
			// 
			// btnDeleteSitesGroup
			// 
			this.btnDeleteSitesGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnDeleteSitesGroup.Enabled = false;
			this.btnDeleteSitesGroup.Image = global::OnlineVideos.MediaPortal1.Properties.Resources.delete;
			this.btnDeleteSitesGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnDeleteSitesGroup.Name = "btnDeleteSitesGroup";
			this.btnDeleteSitesGroup.Size = new System.Drawing.Size(23, 22);
			this.btnDeleteSitesGroup.Text = "Delete Group";
			this.btnDeleteSitesGroup.Click += new System.EventHandler(this.btnDeleteSitesGroup_Click);
			// 
			// splitContainer2
			// 
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.Location = new System.Drawing.Point(0, 0);
			this.splitContainer2.Name = "splitContainer2";
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.Controls.Add(this.listViewSitesNotInGroup);
			this.splitContainer2.Panel1.Controls.Add(this.label40);
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add(this.listViewSitesInGroup);
			this.splitContainer2.Panel2.Controls.Add(this.label41);
			this.splitContainer2.Size = new System.Drawing.Size(704, 298);
			this.splitContainer2.SplitterDistance = 352;
			this.splitContainer2.SplitterWidth = 1;
			this.splitContainer2.TabIndex = 0;
			// 
			// listViewSitesNotInGroup
			// 
			this.listViewSitesNotInGroup.BackColor = System.Drawing.Color.AliceBlue;
			this.listViewSitesNotInGroup.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listViewSitesNotInGroup.Location = new System.Drawing.Point(0, 23);
			this.listViewSitesNotInGroup.Name = "listViewSitesNotInGroup";
			this.listViewSitesNotInGroup.Size = new System.Drawing.Size(352, 275);
			this.listViewSitesNotInGroup.TabIndex = 0;
			this.listViewSitesNotInGroup.UseCompatibleStateImageBehavior = false;
			this.listViewSitesNotInGroup.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBoxSitesNotInGroup_MouseDoubleClick);
			// 
			// label40
			// 
			this.label40.Dock = System.Windows.Forms.DockStyle.Top;
			this.label40.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label40.Location = new System.Drawing.Point(0, 0);
			this.label40.Name = "label40";
			this.label40.Size = new System.Drawing.Size(352, 23);
			this.label40.TabIndex = 1;
			this.label40.Text = "Not in Group (doubleclick to add)";
			this.label40.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// listViewSitesInGroup
			// 
			this.listViewSitesInGroup.BackColor = System.Drawing.Color.AliceBlue;
			this.listViewSitesInGroup.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listViewSitesInGroup.Location = new System.Drawing.Point(0, 23);
			this.listViewSitesInGroup.Name = "listViewSitesInGroup";
			this.listViewSitesInGroup.Size = new System.Drawing.Size(351, 275);
			this.listViewSitesInGroup.TabIndex = 0;
			this.listViewSitesInGroup.UseCompatibleStateImageBehavior = false;
			this.listViewSitesInGroup.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBoxSitesInGroup_MouseDoubleClick);
			// 
			// label41
			// 
			this.label41.Dock = System.Windows.Forms.DockStyle.Top;
			this.label41.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label41.Location = new System.Drawing.Point(0, 0);
			this.label41.Name = "label41";
			this.label41.Size = new System.Drawing.Size(351, 23);
			this.label41.TabIndex = 1;
			this.label41.Text = "In Group (doubleclick to remove)";
			this.label41.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// tabSites
			// 
			this.tabSites.AutoScroll = true;
			this.tabSites.Controls.Add(this.tabControl1);
			this.tabSites.Controls.Add(this.groupBox1);
			this.tabSites.Location = new System.Drawing.Point(4, 22);
			this.tabSites.Margin = new System.Windows.Forms.Padding(2);
			this.tabSites.Name = "tabSites";
			this.tabSites.Padding = new System.Windows.Forms.Padding(2);
			this.tabSites.Size = new System.Drawing.Size(704, 542);
			this.tabSites.TabIndex = 1;
			this.tabSites.Text = "Sites";
			this.tabSites.UseVisualStyleBackColor = true;
			// 
			// tabHosters
			// 
			this.tabHosters.Controls.Add(this.splitter1);
			this.tabHosters.Controls.Add(this.propertyGridHoster);
			this.tabHosters.Controls.Add(this.listBoxHosters);
			this.tabHosters.Location = new System.Drawing.Point(4, 22);
			this.tabHosters.Name = "tabHosters";
			this.tabHosters.Size = new System.Drawing.Size(704, 542);
			this.tabHosters.TabIndex = 4;
			this.tabHosters.Text = "Hosters";
			this.tabHosters.UseVisualStyleBackColor = true;
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(120, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 542);
			this.splitter1.TabIndex = 2;
			this.splitter1.TabStop = false;
			// 
			// propertyGridHoster
			// 
			this.propertyGridHoster.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyGridHoster.Location = new System.Drawing.Point(120, 0);
			this.propertyGridHoster.Name = "propertyGridHoster";
			this.propertyGridHoster.Size = new System.Drawing.Size(584, 542);
			this.propertyGridHoster.TabIndex = 1;
			// 
			// listBoxHosters
			// 
			this.listBoxHosters.Dock = System.Windows.Forms.DockStyle.Left;
			this.listBoxHosters.FormattingEnabled = true;
			this.listBoxHosters.Location = new System.Drawing.Point(0, 0);
			this.listBoxHosters.Name = "listBoxHosters";
			this.listBoxHosters.Size = new System.Drawing.Size(120, 542);
			this.listBoxHosters.TabIndex = 0;
			this.listBoxHosters.SelectedValueChanged += new System.EventHandler(this.listBoxHosters_SelectedValueChanged);
			// 
			// tabPageCodecs
			// 
			this.tabPageCodecs.Controls.Add(this.videopanel);
			this.tabPageCodecs.Controls.Add(this.groupBoxSplitter);
			this.tabPageCodecs.Location = new System.Drawing.Point(4, 22);
			this.tabPageCodecs.Margin = new System.Windows.Forms.Padding(2);
			this.tabPageCodecs.Name = "tabPageCodecs";
			this.tabPageCodecs.Padding = new System.Windows.Forms.Padding(2);
			this.tabPageCodecs.Size = new System.Drawing.Size(704, 542);
			this.tabPageCodecs.TabIndex = 2;
			this.tabPageCodecs.Text = "Codecs";
			this.tabPageCodecs.UseVisualStyleBackColor = true;
			// 
			// videopanel
			// 
			this.videopanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.videopanel.Location = new System.Drawing.Point(6, 157);
			this.videopanel.Name = "videopanel";
			this.videopanel.Size = new System.Drawing.Size(691, 377);
			this.videopanel.TabIndex = 3;
			// 
			// groupBoxSplitter
			// 
			this.groupBoxSplitter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBoxSplitter.Controls.Add(this.btnTestAvi);
			this.groupBoxSplitter.Controls.Add(this.btnTestWmv);
			this.groupBoxSplitter.Controls.Add(this.btnTestMp4);
			this.groupBoxSplitter.Controls.Add(this.btnTestMov);
			this.groupBoxSplitter.Controls.Add(this.tbxMOVSplitter);
			this.groupBoxSplitter.Controls.Add(this.chkMOVSplitterInstalled);
			this.groupBoxSplitter.Controls.Add(this.label46);
			this.groupBoxSplitter.Controls.Add(this.btnTestFlv);
			this.groupBoxSplitter.Controls.Add(this.tbxWMVSplitter);
			this.groupBoxSplitter.Controls.Add(this.tbxAVISplitter);
			this.groupBoxSplitter.Controls.Add(this.tbxMP4Splitter);
			this.groupBoxSplitter.Controls.Add(this.tbxFLVSplitter);
			this.groupBoxSplitter.Controls.Add(this.chkWMVSplitterInstalled);
			this.groupBoxSplitter.Controls.Add(this.chkAVISplitterInstalled);
			this.groupBoxSplitter.Controls.Add(this.chkMP4SplitterInstalled);
			this.groupBoxSplitter.Controls.Add(this.label14);
			this.groupBoxSplitter.Controls.Add(this.label13);
			this.groupBoxSplitter.Controls.Add(this.label12);
			this.groupBoxSplitter.Controls.Add(this.chkFLVSplitterInstalled);
			this.groupBoxSplitter.Controls.Add(this.label11);
			this.groupBoxSplitter.Location = new System.Drawing.Point(6, 10);
			this.groupBoxSplitter.Margin = new System.Windows.Forms.Padding(2);
			this.groupBoxSplitter.Name = "groupBoxSplitter";
			this.groupBoxSplitter.Padding = new System.Windows.Forms.Padding(2);
			this.groupBoxSplitter.Size = new System.Drawing.Size(691, 142);
			this.groupBoxSplitter.TabIndex = 1;
			this.groupBoxSplitter.TabStop = false;
			this.groupBoxSplitter.Text = "Filetypes";
			// 
			// btnTestAvi
			// 
			this.btnTestAvi.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnTestAvi.Location = new System.Drawing.Point(641, 62);
			this.btnTestAvi.Name = "btnTestAvi";
			this.btnTestAvi.Size = new System.Drawing.Size(45, 23);
			this.btnTestAvi.TabIndex = 18;
			this.btnTestAvi.Text = "Test";
			this.btnTestAvi.UseVisualStyleBackColor = true;
			this.btnTestAvi.Click += new System.EventHandler(this.btnTestAvi_Click);
			// 
			// btnTestWmv
			// 
			this.btnTestWmv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnTestWmv.Location = new System.Drawing.Point(641, 87);
			this.btnTestWmv.Name = "btnTestWmv";
			this.btnTestWmv.Size = new System.Drawing.Size(45, 23);
			this.btnTestWmv.TabIndex = 17;
			this.btnTestWmv.Text = "Test";
			this.btnTestWmv.UseVisualStyleBackColor = true;
			this.btnTestWmv.Click += new System.EventHandler(this.btnTestWmv_Click);
			// 
			// btnTestMp4
			// 
			this.btnTestMp4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnTestMp4.Location = new System.Drawing.Point(641, 38);
			this.btnTestMp4.Name = "btnTestMp4";
			this.btnTestMp4.Size = new System.Drawing.Size(45, 23);
			this.btnTestMp4.TabIndex = 16;
			this.btnTestMp4.Text = "Test";
			this.btnTestMp4.UseVisualStyleBackColor = true;
			this.btnTestMp4.Click += new System.EventHandler(this.btnTestMp4_Click);
			// 
			// btnTestMov
			// 
			this.btnTestMov.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnTestMov.Location = new System.Drawing.Point(641, 111);
			this.btnTestMov.Name = "btnTestMov";
			this.btnTestMov.Size = new System.Drawing.Size(45, 23);
			this.btnTestMov.TabIndex = 14;
			this.btnTestMov.Text = "Test";
			this.btnTestMov.UseVisualStyleBackColor = true;
			this.btnTestMov.Click += new System.EventHandler(this.btnTestMov_Click);
			// 
			// tbxMOVSplitter
			// 
			this.tbxMOVSplitter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbxMOVSplitter.Location = new System.Drawing.Point(118, 112);
			this.tbxMOVSplitter.Margin = new System.Windows.Forms.Padding(2);
			this.tbxMOVSplitter.Name = "tbxMOVSplitter";
			this.tbxMOVSplitter.ReadOnly = true;
			this.tbxMOVSplitter.Size = new System.Drawing.Size(518, 20);
			this.tbxMOVSplitter.TabIndex = 15;
			// 
			// chkMOVSplitterInstalled
			// 
			this.chkMOVSplitterInstalled.AutoSize = true;
			this.chkMOVSplitterInstalled.Checked = true;
			this.chkMOVSplitterInstalled.CheckState = System.Windows.Forms.CheckState.Indeterminate;
			this.chkMOVSplitterInstalled.Enabled = false;
			this.chkMOVSplitterInstalled.Location = new System.Drawing.Point(100, 116);
			this.chkMOVSplitterInstalled.Margin = new System.Windows.Forms.Padding(2);
			this.chkMOVSplitterInstalled.Name = "chkMOVSplitterInstalled";
			this.chkMOVSplitterInstalled.Size = new System.Drawing.Size(15, 14);
			this.chkMOVSplitterInstalled.TabIndex = 13;
			this.chkMOVSplitterInstalled.ThreeState = true;
			this.chkMOVSplitterInstalled.UseVisualStyleBackColor = true;
			// 
			// label46
			// 
			this.label46.AutoSize = true;
			this.label46.Location = new System.Drawing.Point(4, 115);
			this.label46.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label46.Name = "label46";
			this.label46.Size = new System.Drawing.Size(27, 13);
			this.label46.TabIndex = 12;
			this.label46.Text = "mov";
			// 
			// btnTestFlv
			// 
			this.btnTestFlv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnTestFlv.Location = new System.Drawing.Point(641, 13);
			this.btnTestFlv.Name = "btnTestFlv";
			this.btnTestFlv.Size = new System.Drawing.Size(45, 23);
			this.btnTestFlv.TabIndex = 3;
			this.btnTestFlv.Text = "Test";
			this.btnTestFlv.UseVisualStyleBackColor = true;
			this.btnTestFlv.Click += new System.EventHandler(this.btnTestFlv_Click);
			// 
			// tbxWMVSplitter
			// 
			this.tbxWMVSplitter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbxWMVSplitter.Location = new System.Drawing.Point(118, 88);
			this.tbxWMVSplitter.Margin = new System.Windows.Forms.Padding(2);
			this.tbxWMVSplitter.Name = "tbxWMVSplitter";
			this.tbxWMVSplitter.ReadOnly = true;
			this.tbxWMVSplitter.Size = new System.Drawing.Size(518, 20);
			this.tbxWMVSplitter.TabIndex = 11;
			// 
			// tbxAVISplitter
			// 
			this.tbxAVISplitter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbxAVISplitter.Location = new System.Drawing.Point(118, 64);
			this.tbxAVISplitter.Margin = new System.Windows.Forms.Padding(2);
			this.tbxAVISplitter.Name = "tbxAVISplitter";
			this.tbxAVISplitter.ReadOnly = true;
			this.tbxAVISplitter.Size = new System.Drawing.Size(518, 20);
			this.tbxAVISplitter.TabIndex = 10;
			// 
			// tbxMP4Splitter
			// 
			this.tbxMP4Splitter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbxMP4Splitter.Location = new System.Drawing.Point(118, 40);
			this.tbxMP4Splitter.Margin = new System.Windows.Forms.Padding(2);
			this.tbxMP4Splitter.Name = "tbxMP4Splitter";
			this.tbxMP4Splitter.ReadOnly = true;
			this.tbxMP4Splitter.Size = new System.Drawing.Size(518, 20);
			this.tbxMP4Splitter.TabIndex = 9;
			// 
			// tbxFLVSplitter
			// 
			this.tbxFLVSplitter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbxFLVSplitter.Location = new System.Drawing.Point(118, 16);
			this.tbxFLVSplitter.Margin = new System.Windows.Forms.Padding(2);
			this.tbxFLVSplitter.Name = "tbxFLVSplitter";
			this.tbxFLVSplitter.ReadOnly = true;
			this.tbxFLVSplitter.Size = new System.Drawing.Size(518, 20);
			this.tbxFLVSplitter.TabIndex = 8;
			// 
			// chkWMVSplitterInstalled
			// 
			this.chkWMVSplitterInstalled.AutoSize = true;
			this.chkWMVSplitterInstalled.Checked = true;
			this.chkWMVSplitterInstalled.CheckState = System.Windows.Forms.CheckState.Indeterminate;
			this.chkWMVSplitterInstalled.Enabled = false;
			this.chkWMVSplitterInstalled.Location = new System.Drawing.Point(100, 92);
			this.chkWMVSplitterInstalled.Margin = new System.Windows.Forms.Padding(2);
			this.chkWMVSplitterInstalled.Name = "chkWMVSplitterInstalled";
			this.chkWMVSplitterInstalled.Size = new System.Drawing.Size(15, 14);
			this.chkWMVSplitterInstalled.TabIndex = 7;
			this.chkWMVSplitterInstalled.ThreeState = true;
			this.chkWMVSplitterInstalled.UseVisualStyleBackColor = true;
			// 
			// chkAVISplitterInstalled
			// 
			this.chkAVISplitterInstalled.AutoSize = true;
			this.chkAVISplitterInstalled.Checked = true;
			this.chkAVISplitterInstalled.CheckState = System.Windows.Forms.CheckState.Indeterminate;
			this.chkAVISplitterInstalled.Enabled = false;
			this.chkAVISplitterInstalled.Location = new System.Drawing.Point(100, 67);
			this.chkAVISplitterInstalled.Margin = new System.Windows.Forms.Padding(2);
			this.chkAVISplitterInstalled.Name = "chkAVISplitterInstalled";
			this.chkAVISplitterInstalled.Size = new System.Drawing.Size(15, 14);
			this.chkAVISplitterInstalled.TabIndex = 6;
			this.chkAVISplitterInstalled.ThreeState = true;
			this.chkAVISplitterInstalled.UseVisualStyleBackColor = true;
			// 
			// chkMP4SplitterInstalled
			// 
			this.chkMP4SplitterInstalled.AutoSize = true;
			this.chkMP4SplitterInstalled.Checked = true;
			this.chkMP4SplitterInstalled.CheckState = System.Windows.Forms.CheckState.Indeterminate;
			this.chkMP4SplitterInstalled.Enabled = false;
			this.chkMP4SplitterInstalled.Location = new System.Drawing.Point(100, 43);
			this.chkMP4SplitterInstalled.Margin = new System.Windows.Forms.Padding(2);
			this.chkMP4SplitterInstalled.Name = "chkMP4SplitterInstalled";
			this.chkMP4SplitterInstalled.Size = new System.Drawing.Size(15, 14);
			this.chkMP4SplitterInstalled.TabIndex = 5;
			this.chkMP4SplitterInstalled.ThreeState = true;
			this.chkMP4SplitterInstalled.UseVisualStyleBackColor = true;
			// 
			// label14
			// 
			this.label14.AutoSize = true;
			this.label14.Location = new System.Drawing.Point(4, 91);
			this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(29, 13);
			this.label14.TabIndex = 4;
			this.label14.Text = "wmv";
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Location = new System.Drawing.Point(4, 66);
			this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(21, 13);
			this.label13.TabIndex = 3;
			this.label13.Text = "avi";
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(4, 42);
			this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(27, 13);
			this.label12.TabIndex = 2;
			this.label12.Text = "mp4";
			// 
			// chkFLVSplitterInstalled
			// 
			this.chkFLVSplitterInstalled.AutoSize = true;
			this.chkFLVSplitterInstalled.Checked = true;
			this.chkFLVSplitterInstalled.CheckState = System.Windows.Forms.CheckState.Indeterminate;
			this.chkFLVSplitterInstalled.Enabled = false;
			this.chkFLVSplitterInstalled.Location = new System.Drawing.Point(100, 20);
			this.chkFLVSplitterInstalled.Margin = new System.Windows.Forms.Padding(2);
			this.chkFLVSplitterInstalled.Name = "chkFLVSplitterInstalled";
			this.chkFLVSplitterInstalled.Size = new System.Drawing.Size(15, 14);
			this.chkFLVSplitterInstalled.TabIndex = 1;
			this.chkFLVSplitterInstalled.ThreeState = true;
			this.chkFLVSplitterInstalled.UseVisualStyleBackColor = true;
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(4, 19);
			this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(18, 13);
			this.label11.TabIndex = 0;
			this.label11.Text = "flv";
			// 
			// sitesBindingSource
			// 
			this.sitesBindingSource.DataMember = "Sites";
			this.sitesBindingSource.DataSource = this.bindingSourceSitesGroup;
			// 
			// errorProvider1
			// 
			this.errorProvider1.ContainerControl = this;
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			// 
			// onlineVideosService1
			// 
			this.onlineVideosService1.Credentials = null;
			this.onlineVideosService1.Url = "http://87.106.7.69/OnlineVideosWebService/OnlineVideos.asmx";
			this.onlineVideosService1.UseDefaultCredentials = false;
			// 
			// Configuration
			// 
			this.AcceptButton = this.btnSave;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(712, 568);
			this.Controls.Add(this.mainTabControl);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Configuration";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "OnlineVideos Configuration";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConfigurationFormClosing);
			this.Load += new System.EventHandler(this.Configuration_Load);
			((System.ComponentModel.ISupportInitialize)(this.bindingSourceRssLink)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.bindingSourceSiteSettings)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.iconSite)).EndInit();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.toolStripContainer3.ContentPanel.ResumeLayout(false);
			this.toolStripContainer3.LeftToolStripPanel.ResumeLayout(false);
			this.toolStripContainer3.LeftToolStripPanel.PerformLayout();
			this.toolStripContainer3.TopToolStripPanel.ResumeLayout(false);
			this.toolStripContainer3.TopToolStripPanel.PerformLayout();
			this.toolStripContainer3.ResumeLayout(false);
			this.toolStripContainer3.PerformLayout();
			this.toolStripSiteUpDown.ResumeLayout(false);
			this.toolStripSiteUpDown.PerformLayout();
			this.toolStripSites.ResumeLayout(false);
			this.toolStripSites.PerformLayout();
			this.tabControl1.ResumeLayout(false);
			this.tabPageRssLinks.ResumeLayout(false);
			this.tabPageRssLinks.PerformLayout();
			this.toolStripContainer1.ContentPanel.ResumeLayout(false);
			this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
			this.toolStripContainer1.TopToolStripPanel.PerformLayout();
			this.toolStripContainer1.ResumeLayout(false);
			this.toolStripContainer1.PerformLayout();
			this.toolStripRss.ResumeLayout(false);
			this.toolStripRss.PerformLayout();
			this.tabChannels.ResumeLayout(false);
			this.tabChannels.PerformLayout();
			this.toolStripContainer2.ContentPanel.ResumeLayout(false);
			this.toolStripContainer2.TopToolStripPanel.ResumeLayout(false);
			this.toolStripContainer2.TopToolStripPanel.PerformLayout();
			this.toolStripContainer2.ResumeLayout(false);
			this.toolStripContainer2.PerformLayout();
			this.toolStripChannels.ResumeLayout(false);
			this.toolStripChannels.PerformLayout();
			this.mainTabControl.ResumeLayout(false);
			this.tabGeneral.ResumeLayout(false);
			this.tabGeneral.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
			this.groupBoxLatestVideos.ResumeLayout(false);
			this.groupBoxLatestVideos.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
			this.groupBox5.ResumeLayout(false);
			this.groupBox5.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nUPSearchHistoryItemCount)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.Thumbnails.ResumeLayout(false);
			this.Thumbnails.PerformLayout();
			this.tabGroups.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.bindingSourceSitesGroup)).EndInit();
			this.toolStripContainer4.ContentPanel.ResumeLayout(false);
			this.toolStripContainer4.LeftToolStripPanel.ResumeLayout(false);
			this.toolStripContainer4.LeftToolStripPanel.PerformLayout();
			this.toolStripContainer4.TopToolStripPanel.ResumeLayout(false);
			this.toolStripContainer4.TopToolStripPanel.PerformLayout();
			this.toolStripContainer4.ResumeLayout(false);
			this.toolStripContainer4.PerformLayout();
			this.toolStrip2.ResumeLayout(false);
			this.toolStrip2.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel2.ResumeLayout(false);
			this.splitContainer2.ResumeLayout(false);
			this.tabSites.ResumeLayout(false);
			this.tabHosters.ResumeLayout(false);
			this.tabPageCodecs.ResumeLayout(false);
			this.groupBoxSplitter.ResumeLayout(false);
			this.groupBoxSplitter.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.sitesBindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
			this.ResumeLayout(false);

        }
		private System.Windows.Forms.TextBox txtDownloadDir;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtRssUrl;
		private System.Windows.Forms.TextBox txtRssName;
		private System.Windows.Forms.ListBox RssLinkList;
        private BindableCheckedListBox siteList;
		private System.Windows.Forms.Label label27;
		private System.Windows.Forms.Label label28;
        private System.Windows.Forms.TextBox txtSiteName;
        private System.Windows.Forms.CheckBox chkAgeConfirm;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label26;
		private System.Windows.Forms.TextBox txtFilters;
		private System.Windows.Forms.TextBox txtThumbLoc;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		//private System.Windows.Forms.TabPage General_Tab;
		//private System.Windows.Forms.TabControl tabControl1;
		
		void CheckBox1CheckedChanged(object sender, System.EventArgs e)
		{
			
		}

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkUseAgeConfirmation;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageRssLinks;
        private System.Windows.Forms.TabPage tabChannels;
        private System.Windows.Forms.TreeView tvGroups;
        private System.Windows.Forms.ComboBox cbSiteUtil;
        private System.Windows.Forms.TextBox tbxPin;
        private System.Windows.Forms.TextBox tbxScreenName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbxChannelName;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tbxStreamUrl;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbxStreamName;
        private System.Windows.Forms.Button btnSaveChannel;
        private System.Windows.Forms.Button btnBrowseForDlFolder;
        private System.Windows.Forms.TabControl mainTabControl;
        private System.Windows.Forms.TabPage tabGeneral;
        private System.Windows.Forms.TabPage tabSites;
        private System.Windows.Forms.TabPage tabPageCodecs;
        private System.Windows.Forms.GroupBox groupBoxSplitter;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox chkFLVSplitterInstalled;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.CheckBox chkWMVSplitterInstalled;
        private System.Windows.Forms.CheckBox chkAVISplitterInstalled;
        private System.Windows.Forms.CheckBox chkMP4SplitterInstalled;
        private System.Windows.Forms.TextBox tbxFLVSplitter;
        private System.Windows.Forms.TextBox tbxWMVSplitter;
        private System.Windows.Forms.TextBox tbxAVISplitter;
        private System.Windows.Forms.TextBox tbxMP4Splitter;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox tbxStreamThumb;
        private System.Windows.Forms.ComboBox cbLanguages;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtRssThumb;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox tbxChannelThumb;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.BindingSource bindingSourceSiteSettings;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStrip toolStripRss;
        private System.Windows.Forms.ToolStripButton btnDeleteRss;
        private System.Windows.Forms.ToolStripButton btnAddRss;
        private System.Windows.Forms.ToolStripContainer toolStripContainer2;
        private System.Windows.Forms.ToolStrip toolStripChannels;
        private System.Windows.Forms.ToolStripButton btnAddChannel;
        private System.Windows.Forms.ToolStripButton btnAddGroup;
        private System.Windows.Forms.ToolStripButton btnDeleteChannel;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripContainer toolStripContainer3;
        private System.Windows.Forms.ToolStrip toolStripSites;
        private System.Windows.Forms.ToolStripButton btnAddSite;
        private System.Windows.Forms.ToolStripButton btnDeleteSite;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStrip toolStripSiteUpDown;
        private System.Windows.Forms.ToolStripButton btnSiteUp;
        private System.Windows.Forms.ToolStripButton btnSiteDown;
        private System.Windows.Forms.BindingSource bindingSourceRssLink;
        private System.Windows.Forms.Button btnAdvanced;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox descriptionTextBox;
        private System.Windows.Forms.PropertyGrid propertyGridUserConfig;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbxWebCacheTimeout;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.TextBox tbxUtilTimeout;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox tbxWMPBuffer;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ToolStripButton btnPublishSite;
        private System.Windows.Forms.ToolStripButton btnReportSite;
        private System.Windows.Forms.PictureBox iconSite;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.CheckBox chkDoAutoUpdate;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.DomainUpDown udPlayBuffer;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.CheckBox chkUseQuickSelect;
        private System.Windows.Forms.Button bntBrowseFolderForThumbs;
        private System.Windows.Forms.GroupBox Thumbnails;
        private System.Windows.Forms.TextBox tbxThumbAge;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownBtnImport;
        private System.Windows.Forms.ToolStripMenuItem btnImportXml;
        private System.Windows.Forms.ToolStripMenuItem btnImportGlobal;
		private System.Windows.Forms.ToolStripButton btnCreateSite;
        private System.Windows.Forms.RadioButton rbExtendedSearchHistory;
        private System.Windows.Forms.RadioButton rbLastSearch;
        private System.Windows.Forms.RadioButton rbOff;
        private System.Windows.Forms.NumericUpDown nUPSearchHistoryItemCount;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.TabPage tabGroups;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripContainer toolStripContainer4;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton btnSitesGroupUp;
        private System.Windows.Forms.ToolStripButton btnSitesGroupDown;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnAddSitesGroup;
        private System.Windows.Forms.ToolStripButton btnDeleteSitesGroup;
        private System.Windows.Forms.ListBox listBoxSitesGroups;
        private System.Windows.Forms.TextBox tbxSitesGroupName;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ListView listViewSitesNotInGroup;
        private System.Windows.Forms.ListView listViewSitesInGroup;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.BindingSource bindingSourceSitesGroup;
        private System.Windows.Forms.BindingSource sitesBindingSource;
        private System.Windows.Forms.TextBox tbxSitesGroupThumb;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.TextBox tbxSitesGroupDesc;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.Button btnBrowseSitesGroupThumb;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.CheckBox chkAutoGroupByLang;
        private System.Windows.Forms.Button btnWiki;
        private System.Windows.Forms.TextBox tbxUpdatePeriod;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.CheckBox chkFavFirst;
        private System.Windows.Forms.Button btnTestFlv;
        private System.Windows.Forms.Panel videopanel;
        private System.Windows.Forms.Button btnTestMov;
        private System.Windows.Forms.TextBox tbxMOVSplitter;
        private System.Windows.Forms.CheckBox chkMOVSplitterInstalled;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.Button btnTestAvi;
        private System.Windows.Forms.Button btnTestWmv;
        private System.Windows.Forms.Button btnTestMp4;
        private System.Windows.Forms.TabPage tabHosters;
        private System.Windows.Forms.ListBox listBoxHosters;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.PropertyGrid propertyGridHoster;
		private System.Windows.Forms.Label label48;
		private System.Windows.Forms.Label label47;
		private System.Windows.Forms.TextBox tbxCategoriesTimeout;
		private System.Windows.Forms.GroupBox groupBoxLatestVideos;
		private System.Windows.Forms.Label label51;
		private System.Windows.Forms.Label label50;
		private System.Windows.Forms.Label label49;
		private System.Windows.Forms.PictureBox pictureBox6;
		private System.Windows.Forms.Label label53;
		private System.Windows.Forms.Label label52;
		private System.Windows.Forms.TextBox tbxLatestVideosGuiRefresh;
		private System.Windows.Forms.TextBox tbxLatestVideosOnlineRefresh;
		private System.Windows.Forms.TextBox tbxLatestVideosAmount;
		private System.Windows.Forms.CheckBox chkLatestVideosRandomize;
		private OnlineVideosWebservice.OnlineVideosService onlineVideosService1;
	}
}