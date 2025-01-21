using CommunityToolkit.WinForms.AI;
using CommunityToolkit.WinForms.Controls.Blazor;
using CommunityToolkit.WinForms.ConversationView;

namespace Chatty
{
    partial class FrmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            _menuStrip = new MenuStrip();
            _tsmFile = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripMenuItem();
            toolStripSeparator3 = new ToolStripSeparator();
            _tsmQuit = new ToolStripMenuItem();
            _tsiEdit = new ToolStripMenuItem();
            _tsiPersonalities = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripMenuItem();
            _tsmTools = new ToolStripMenuItem();
            _tsmOptions = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            _tsmAbout = new ToolStripMenuItem();
            _statusStrip = new StatusStrip();
            _tslInfo = new ToolStripStatusLabel();
            _tslClockInfo = new ToolStripStatusLabel();
            _toolStrip = new ToolStrip();
            newToolStripButton = new ToolStripButton();
            toolStripSeparator = new ToolStripSeparator();
            copyToolStripButton = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            toolStripLabel1 = new ToolStripLabel();
            _tscPersonalities = new ToolStripComboBox();
            toolStripSeparator2 = new ToolStripSeparator();
            toolStripSeparator4 = new ToolStripSeparator();
            _tslModels = new ToolStripLabel();
            _tscModels = new ToolStripComboBox();
            _splitMain = new SplitContainer();
            _trvConversationHistory = new TreeView();
            _tlpHeader = new TableLayoutPanel();
            _mainTabControl = new CommunityToolkit.WinForms.FluentUI.FluentTabControl();
            _lblConversationTitle = new Label();
            _lblDate = new Label();
            _skCommunicator = new SemanticKernelComponent();
            _skMetaDataProcessor = new SemanticKernelComponent();
            _menuStrip.SuspendLayout();
            _statusStrip.SuspendLayout();
            _toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_splitMain).BeginInit();
            _splitMain.Panel1.SuspendLayout();
            _splitMain.Panel2.SuspendLayout();
            _splitMain.SuspendLayout();
            _tlpHeader.SuspendLayout();
            SuspendLayout();
            // 
            // _menuStrip
            // 
            _menuStrip.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            _menuStrip.ImageScalingSize = new Size(24, 24);
            _menuStrip.Items.AddRange(new ToolStripItem[] { _tsmFile, _tsiEdit, toolStripMenuItem2, _tsmTools, helpToolStripMenuItem });
            _menuStrip.Location = new Point(0, 0);
            _menuStrip.Name = "_menuStrip";
            _menuStrip.Padding = new Padding(7, 2, 0, 2);
            _menuStrip.Size = new Size(1264, 44);
            _menuStrip.TabIndex = 0;
            _menuStrip.Text = "menuStrip1";
            // 
            // _tsmFile
            // 
            _tsmFile.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItem1, toolStripSeparator3, _tsmQuit });
            _tsmFile.Name = "_tsmFile";
            _tsmFile.Size = new Size(72, 40);
            _tsmFile.Text = "&File";
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(299, 44);
            toolStripMenuItem1.Text = "Start new chat";
            toolStripMenuItem1.Click += BtnStartNewChat_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(296, 6);
            // 
            // _tsmQuit
            // 
            _tsmQuit.Name = "_tsmQuit";
            _tsmQuit.Size = new Size(299, 44);
            _tsmQuit.Text = "Quit";
            // 
            // _tsiEdit
            // 
            _tsiEdit.DropDownItems.AddRange(new ToolStripItem[] { _tsiPersonalities });
            _tsiEdit.Name = "_tsiEdit";
            _tsiEdit.Size = new Size(76, 40);
            _tsiEdit.Text = "&Edit";
            // 
            // _tsiPersonalities
            // 
            _tsiPersonalities.Name = "_tsiPersonalities";
            _tsiPersonalities.Size = new Size(356, 44);
            _tsiPersonalities.Text = "Chat Personalities...";
            _tsiPersonalities.Click += Personalities_Click;
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Size(88, 40);
            toolStripMenuItem2.Text = "&View";
            // 
            // _tsmTools
            // 
            _tsmTools.DropDownItems.AddRange(new ToolStripItem[] { _tsmOptions });
            _tsmTools.Name = "_tsmTools";
            _tsmTools.Size = new Size(91, 40);
            _tsmTools.Text = "Tools";
            // 
            // _tsmOptions
            // 
            _tsmOptions.Name = "_tsmOptions";
            _tsmOptions.Size = new Size(244, 44);
            _tsmOptions.Text = "Options...";
            _tsmOptions.Click += TsmOptions_Click;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { _tsmAbout });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(86, 40);
            helpToolStripMenuItem.Text = "&Help";
            // 
            // _tsmAbout
            // 
            _tsmAbout.Name = "_tsmAbout";
            _tsmAbout.Size = new Size(224, 44);
            _tsmAbout.Text = "About...";
            _tsmAbout.Click += About_Click;
            // 
            // _statusStrip
            // 
            _statusStrip.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            _statusStrip.ImageScalingSize = new Size(24, 24);
            _statusStrip.Items.AddRange(new ToolStripItem[] { _tslInfo, _tslClockInfo });
            _statusStrip.Location = new Point(0, 751);
            _statusStrip.Name = "_statusStrip";
            _statusStrip.Padding = new Padding(1, 0, 17, 0);
            _statusStrip.Size = new Size(1264, 45);
            _statusStrip.TabIndex = 1;
            _statusStrip.Text = "statusStrip1";
            // 
            // _tslInfo
            // 
            _tslInfo.Name = "_tslInfo";
            _tslInfo.Size = new Size(1158, 36);
            _tslInfo.Spring = true;
            _tslInfo.Text = "#info";
            _tslInfo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // _tslClockInfo
            // 
            _tslClockInfo.Name = "_tslClockInfo";
            _tslClockInfo.Size = new Size(88, 36);
            _tslClockInfo.Text = "#clock";
            // 
            // _toolStrip
            // 
            _toolStrip.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            _toolStrip.ImageScalingSize = new Size(32, 32);
            _toolStrip.Items.AddRange(new ToolStripItem[] { newToolStripButton, toolStripSeparator, copyToolStripButton, toolStripSeparator1, toolStripLabel1, _tscPersonalities, toolStripSeparator2, toolStripSeparator4, _tslModels, _tscModels });
            _toolStrip.Location = new Point(0, 44);
            _toolStrip.Name = "_toolStrip";
            _toolStrip.Padding = new Padding(10);
            _toolStrip.Size = new Size(1264, 65);
            _toolStrip.TabIndex = 2;
            _toolStrip.Text = "toolStrip1";
            // 
            // newToolStripButton
            // 
            newToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            newToolStripButton.Image = (Image)resources.GetObject("newToolStripButton.Image");
            newToolStripButton.ImageTransparentColor = Color.Magenta;
            newToolStripButton.Margin = new Padding(0, 4, 0, 5);
            newToolStripButton.Name = "newToolStripButton";
            newToolStripButton.Size = new Size(40, 36);
            newToolStripButton.Text = "&New";
            newToolStripButton.Click += BtnStartNewChat_Click;
            // 
            // toolStripSeparator
            // 
            toolStripSeparator.Name = "toolStripSeparator";
            toolStripSeparator.Size = new Size(6, 45);
            // 
            // copyToolStripButton
            // 
            copyToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            copyToolStripButton.Image = (Image)resources.GetObject("copyToolStripButton.Image");
            copyToolStripButton.ImageTransparentColor = Color.Magenta;
            copyToolStripButton.Margin = new Padding(4, 0, 5, 0);
            copyToolStripButton.Name = "copyToolStripButton";
            copyToolStripButton.Size = new Size(40, 45);
            copyToolStripButton.Text = "&Copy";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 45);
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Margin = new Padding(4, 0, 5, 0);
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(146, 45);
            toolStripLabel1.Text = "Personality:";
            // 
            // _tscPersonalities
            // 
            _tscPersonalities.DropDownWidth = 300;
            _tscPersonalities.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            _tscPersonalities.Margin = new Padding(4, 0, 5, 0);
            _tscPersonalities.Name = "_tscPersonalities";
            _tscPersonalities.Size = new Size(300, 45);
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(6, 45);
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(6, 45);
            // 
            // _tslModels
            // 
            _tslModels.Name = "_tslModels";
            _tslModels.Size = new Size(112, 39);
            _tslModels.Text = "Models: ";
            // 
            // _tscModels
            // 
            _tscModels.Name = "_tscModels";
            _tscModels.Size = new Size(250, 45);
            // 
            // _splitMain
            // 
            _splitMain.Dock = DockStyle.Fill;
            _splitMain.Location = new Point(0, 109);
            _splitMain.Name = "_splitMain";
            // 
            // _splitMain.Panel1
            // 
            _splitMain.Panel1.Controls.Add(_trvConversationHistory);
            _splitMain.Panel1.Padding = new Padding(10);
            // 
            // _splitMain.Panel2
            // 
            _splitMain.Panel2.Controls.Add(_tlpHeader);
            _splitMain.Panel2.Padding = new Padding(10);
            _splitMain.Size = new Size(1264, 642);
            _splitMain.SplitterDistance = 363;
            _splitMain.TabIndex = 3;
            // 
            // _trvConversationHistory
            // 
            _trvConversationHistory.Dock = DockStyle.Fill;
            _trvConversationHistory.FullRowSelect = true;
            _trvConversationHistory.HideSelection = false;
            _trvConversationHistory.Location = new Point(10, 10);
            _trvConversationHistory.Name = "_trvConversationHistory";
            _trvConversationHistory.ShowNodeToolTips = true;
            _trvConversationHistory.Size = new Size(343, 622);
            _trvConversationHistory.TabIndex = 0;
            _trvConversationHistory.NodeMouseDoubleClick += ConversationHistory_NodeMouseDoubleClick;
            // 
            // _tlpHeader
            // 
            _tlpHeader.ColumnCount = 2;
            _tlpHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _tlpHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 457F));
            _tlpHeader.Controls.Add(_mainTabControl, 0, 1);
            _tlpHeader.Controls.Add(_lblConversationTitle, 0, 0);
            _tlpHeader.Controls.Add(_lblDate, 1, 0);
            _tlpHeader.Dock = DockStyle.Fill;
            _tlpHeader.Location = new Point(10, 10);
            _tlpHeader.Name = "_tlpHeader";
            _tlpHeader.RowCount = 2;
            _tlpHeader.RowStyles.Add(new RowStyle());
            _tlpHeader.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            _tlpHeader.Size = new Size(877, 622);
            _tlpHeader.TabIndex = 0;
            // 
            // _mainTabControl
            // 
            _mainTabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _tlpHeader.SetColumnSpan(_mainTabControl, 2);
            _mainTabControl.Location = new Point(3, 54);
            _mainTabControl.Name = "_mainTabControl";
            _mainTabControl.Size = new Size(871, 565);
            _mainTabControl.TabIndex = 0;
            // 
            // _lblConversationTitle
            // 
            _lblConversationTitle.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _lblConversationTitle.AutoSize = true;
            _lblConversationTitle.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            _lblConversationTitle.Location = new Point(3, 0);
            _lblConversationTitle.Name = "_lblConversationTitle";
            _lblConversationTitle.Size = new Size(414, 51);
            _lblConversationTitle.TabIndex = 1;
            _lblConversationTitle.Text = "Conversation Title";
            // 
            // _lblDate
            // 
            _lblDate.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            _lblDate.AutoSize = true;
            _lblDate.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            _lblDate.Location = new Point(730, 0);
            _lblDate.Name = "_lblDate";
            _lblDate.Size = new Size(144, 51);
            _lblDate.TabIndex = 2;
            _lblDate.Text = "#dateTime";
            _lblDate.TextAlign = ContentAlignment.MiddleRight;
            // 
            // _skCommunicator
            // 
            _skCommunicator.TopP = null;
            // 
            // _skMetaDataProcessor
            // 
            _skMetaDataProcessor.TopP = null;
            // 
            // FrmMain
            // 
            AutoScaleDimensions = new SizeF(14F, 36F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1264, 796);
            Controls.Add(_splitMain);
            Controls.Add(_toolStrip);
            Controls.Add(_statusStrip);
            Controls.Add(_menuStrip);
            Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            MainMenuStrip = _menuStrip;
            Margin = new Padding(4);
            Name = "FrmMain";
            Text = "C.H.A.T.T.Y.";
            _menuStrip.ResumeLayout(false);
            _menuStrip.PerformLayout();
            _statusStrip.ResumeLayout(false);
            _statusStrip.PerformLayout();
            _toolStrip.ResumeLayout(false);
            _toolStrip.PerformLayout();
            _splitMain.Panel1.ResumeLayout(false);
            _splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_splitMain).EndInit();
            _splitMain.ResumeLayout(false);
            _tlpHeader.ResumeLayout(false);
            _tlpHeader.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip _menuStrip;
        private ToolStripMenuItem _tsmFile;
        private ToolStripMenuItem _tsmQuit;
        private StatusStrip _statusStrip;
        private ToolStrip _toolStrip;
        private ToolStripButton newToolStripButton;
        private ToolStripSeparator toolStripSeparator;
        private ToolStripButton copyToolStripButton;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripStatusLabel _tslInfo;
        private SplitContainer _splitMain;
        private TreeView _trvConversationHistory;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem _tsmAbout;
        private SemanticKernelComponent _skCommunicator;
        private ToolStripComboBox _tscPersonalities;
        private ToolStripLabel toolStripLabel1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem _tsmTools;
        private ToolStripMenuItem _tsmOptions;
        private SemanticKernelComponent _skMetaDataProcessor;
        private ToolStripMenuItem _tsiEdit;
        private ToolStripMenuItem _tsiPersonalities;
        private ToolStripMenuItem toolStripMenuItem2;
        private TableLayoutPanel _tlpHeader;
        private CommunityToolkit.WinForms.FluentUI.FluentTabControl _mainTabControl;
        private Label _lblConversationTitle;
        private ToolStripStatusLabel _tslClockInfo;
        private Label _lblDate;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripLabel _tslModels;
        private ToolStripComboBox _tscModels;
    }
}
