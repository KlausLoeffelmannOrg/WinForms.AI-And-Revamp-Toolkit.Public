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
            _tsmTools = new ToolStripMenuItem();
            _tsmOptions = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            _tsmAbout = new ToolStripMenuItem();
            _statusStrip = new StatusStrip();
            _tslInfo = new ToolStripStatusLabel();
            _toolStrip = new ToolStrip();
            newToolStripButton = new ToolStripButton();
            toolStripSeparator = new ToolStripSeparator();
            copyToolStripButton = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            toolStripLabel1 = new ToolStripLabel();
            _tscPersonalities = new ToolStripComboBox();
            toolStripSeparator2 = new ToolStripSeparator();
            _splitContainer = new SplitContainer();
            treeView1 = new TreeView();
            _chatAreaSplitter = new SplitContainer();
            _conversationView = new ConversationView();
            _decoratorPanel = new CommunityToolkit.WinForms.FluentUI.FluentDecoratorPanel();
            _promptControl = new SemanticKernelDemo.Controls.AsyncPromptControl();
            _skCommunicator = new CommunityToolkit.WinForms.AI.SemanticKernelComponent();
            _skMetaDataProcessor = new CommunityToolkit.WinForms.AI.SemanticKernelComponent();
            _menuStrip.SuspendLayout();
            _statusStrip.SuspendLayout();
            _toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_splitContainer).BeginInit();
            _splitContainer.Panel1.SuspendLayout();
            _splitContainer.Panel2.SuspendLayout();
            _splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_chatAreaSplitter).BeginInit();
            _chatAreaSplitter.Panel1.SuspendLayout();
            _chatAreaSplitter.Panel2.SuspendLayout();
            _chatAreaSplitter.SuspendLayout();
            _decoratorPanel.SuspendLayout();
            SuspendLayout();
            // 
            // _menuStrip
            // 
            _menuStrip.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            _menuStrip.ImageScalingSize = new Size(24, 24);
            _menuStrip.Items.AddRange(new ToolStripItem[] { _tsmFile, _tsmTools, helpToolStripMenuItem });
            _menuStrip.Location = new Point(0, 0);
            _menuStrip.Name = "_menuStrip";
            _menuStrip.Padding = new Padding(7, 2, 0, 2);
            _menuStrip.Size = new Size(1108, 28);
            _menuStrip.TabIndex = 0;
            _menuStrip.Text = "menuStrip1";
            // 
            // _tsmFile
            // 
            _tsmFile.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItem1, toolStripSeparator3, _tsmQuit });
            _tsmFile.Name = "_tsmFile";
            _tsmFile.Size = new Size(44, 24);
            _tsmFile.Text = "&File";
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(172, 24);
            toolStripMenuItem1.Text = "Start new chat";
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(169, 6);
            // 
            // _tsmQuit
            // 
            _tsmQuit.Name = "_tsmQuit";
            _tsmQuit.Size = new Size(172, 24);
            _tsmQuit.Text = "Quit";
            // 
            // _tsmTools
            // 
            _tsmTools.DropDownItems.AddRange(new ToolStripItem[] { _tsmOptions });
            _tsmTools.Name = "_tsmTools";
            _tsmTools.Size = new Size(56, 24);
            _tsmTools.Text = "Tools";
            // 
            // _tsmOptions
            // 
            _tsmOptions.Name = "_tsmOptions";
            _tsmOptions.Size = new Size(139, 24);
            _tsmOptions.Text = "Options...";
            _tsmOptions.Click += TsmOptions_Click;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { _tsmAbout });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(53, 24);
            helpToolStripMenuItem.Text = "&Help";
            // 
            // _tsmAbout
            // 
            _tsmAbout.Name = "_tsmAbout";
            _tsmAbout.Size = new Size(128, 24);
            _tsmAbout.Text = "About...";
            _tsmAbout.Click += About_Click;
            // 
            // _statusStrip
            // 
            _statusStrip.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            _statusStrip.ImageScalingSize = new Size(24, 24);
            _statusStrip.Items.AddRange(new ToolStripItem[] { _tslInfo });
            _statusStrip.Location = new Point(0, 622);
            _statusStrip.Name = "_statusStrip";
            _statusStrip.Padding = new Padding(1, 0, 17, 0);
            _statusStrip.Size = new Size(1108, 25);
            _statusStrip.TabIndex = 1;
            _statusStrip.Text = "statusStrip1";
            // 
            // _tslInfo
            // 
            _tslInfo.Name = "_tslInfo";
            _tslInfo.Size = new Size(1090, 20);
            _tslInfo.Spring = true;
            _tslInfo.Text = "#info";
            _tslInfo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // _toolStrip
            // 
            _toolStrip.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            _toolStrip.ImageScalingSize = new Size(32, 32);
            _toolStrip.Items.AddRange(new ToolStripItem[] { newToolStripButton, toolStripSeparator, copyToolStripButton, toolStripSeparator1, toolStripLabel1, _tscPersonalities, toolStripSeparator2 });
            _toolStrip.Location = new Point(0, 28);
            _toolStrip.Name = "_toolStrip";
            _toolStrip.Padding = new Padding(5);
            _toolStrip.Size = new Size(1108, 55);
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
            newToolStripButton.Size = new Size(36, 36);
            newToolStripButton.Text = "&New";
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
            copyToolStripButton.Size = new Size(36, 45);
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
            toolStripLabel1.Size = new Size(83, 45);
            toolStripLabel1.Text = "Personality:";
            // 
            // _tscPersonalities
            // 
            _tscPersonalities.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            _tscPersonalities.Margin = new Padding(4, 0, 5, 0);
            _tscPersonalities.Name = "_tscPersonalities";
            _tscPersonalities.Size = new Size(250, 45);
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(6, 45);
            // 
            // _splitContainer
            // 
            _splitContainer.Dock = DockStyle.Fill;
            _splitContainer.Location = new Point(0, 83);
            _splitContainer.Name = "_splitContainer";
            // 
            // _splitContainer.Panel1
            // 
            _splitContainer.Panel1.Controls.Add(treeView1);
            _splitContainer.Panel1.Padding = new Padding(10);
            // 
            // _splitContainer.Panel2
            // 
            _splitContainer.Panel2.Controls.Add(_chatAreaSplitter);
            _splitContainer.Panel2.Padding = new Padding(10);
            _splitContainer.Size = new Size(1108, 539);
            _splitContainer.SplitterDistance = 320;
            _splitContainer.TabIndex = 3;
            // 
            // treeView1
            // 
            treeView1.Dock = DockStyle.Fill;
            treeView1.Location = new Point(10, 10);
            treeView1.Name = "treeView1";
            treeView1.Size = new Size(300, 519);
            treeView1.TabIndex = 0;
            // 
            // _chatAreaSplitter
            // 
            _chatAreaSplitter.Dock = DockStyle.Fill;
            _chatAreaSplitter.Location = new Point(10, 10);
            _chatAreaSplitter.Name = "_chatAreaSplitter";
            _chatAreaSplitter.Orientation = Orientation.Horizontal;
            // 
            // _chatAreaSplitter.Panel1
            // 
            _chatAreaSplitter.Panel1.Controls.Add(_conversationView);
            _chatAreaSplitter.Panel1.Padding = new Padding(5);
            // 
            // _chatAreaSplitter.Panel2
            // 
            _chatAreaSplitter.Panel2.Controls.Add(_decoratorPanel);
            _chatAreaSplitter.Panel2.Padding = new Padding(5);
            _chatAreaSplitter.Size = new Size(764, 519);
            _chatAreaSplitter.SplitterDistance = 389;
            _chatAreaSplitter.TabIndex = 0;
            // 
            // _conversationView
            // 
            _conversationView.Dock = DockStyle.Fill;
            _conversationView.HostPage = "wwwroot/index.html";
            _conversationView.Location = new Point(5, 5);
            _conversationView.Name = "_conversationView";
            _conversationView.Size = new Size(754, 379);
            _conversationView.TabIndex = 0;
            _conversationView.Text = "conversationView1";
            // 
            // _decoratorPanel
            // 
            _decoratorPanel.BorderThickness = 1;
            _decoratorPanel.Controls.Add(_promptControl);
            _decoratorPanel.Dock = DockStyle.Fill;
            _decoratorPanel.Location = new Point(5, 5);
            _decoratorPanel.Name = "_decoratorPanel";
            _decoratorPanel.Padding = new Padding(5);
            _decoratorPanel.Size = new Size(754, 116);
            _decoratorPanel.TabIndex = 0;
            // 
            // _promptControl
            // 
            _promptControl.BorderStyle = BorderStyle.None;
            _promptControl.Dock = DockStyle.Fill;
            _promptControl.Location = new Point(6, 5);
            _promptControl.Multiline = true;
            _promptControl.Name = "_promptControl";
            _promptControl.Size = new Size(742, 106);
            _promptControl.TabIndex = 0;
            _promptControl.AsyncSendPrompt += PromptControl_AsyncSendPrompt;
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
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1108, 647);
            Controls.Add(_splitContainer);
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
            _splitContainer.Panel1.ResumeLayout(false);
            _splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_splitContainer).EndInit();
            _splitContainer.ResumeLayout(false);
            _chatAreaSplitter.Panel1.ResumeLayout(false);
            _chatAreaSplitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_chatAreaSplitter).EndInit();
            _chatAreaSplitter.ResumeLayout(false);
            _decoratorPanel.ResumeLayout(false);
            _decoratorPanel.PerformLayout();
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
        private SplitContainer _splitContainer;
        private TreeView treeView1;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem _tsmAbout;
        private SplitContainer _chatAreaSplitter;
        private ConversationView _conversationView;
        private CommunityToolkit.WinForms.FluentUI.FluentDecoratorPanel _decoratorPanel;
        private CommunityToolkit.WinForms.AI.SemanticKernelComponent _skCommunicator;
        private ToolStripComboBox _tscPersonalities;
        private ToolStripLabel toolStripLabel1;
        private ToolStripSeparator toolStripSeparator2;
        private SemanticKernelDemo.Controls.AsyncPromptControl _promptControl;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem _tsmTools;
        private ToolStripMenuItem _tsmOptions;
        private CommunityToolkit.WinForms.AI.SemanticKernelComponent _skMetaDataProcessor;
    }
}
