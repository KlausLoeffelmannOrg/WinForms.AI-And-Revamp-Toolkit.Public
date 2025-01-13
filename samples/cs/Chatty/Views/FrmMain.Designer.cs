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
            _tsmQuit = new ToolStripMenuItem();
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
            splitContainer1 = new SplitContainer();
            _conversationView = new ConversationView();
            _decoratorPanel = new CommunityToolkit.WinForms.FluentUI.FluentDecoratorPanel();
            _promptControl = new SemanticKernelDemo.Controls.AsyncPromptControl();
            _semanticKernelCommunicator = new CommunityToolkit.WinForms.AI.SemanticKernelComponent();
            _menuStrip.SuspendLayout();
            _statusStrip.SuspendLayout();
            _toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_splitContainer).BeginInit();
            _splitContainer.Panel1.SuspendLayout();
            _splitContainer.Panel2.SuspendLayout();
            _splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            _decoratorPanel.SuspendLayout();
            SuspendLayout();
            // 
            // _menuStrip
            // 
            _menuStrip.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            _menuStrip.ImageScalingSize = new Size(24, 24);
            _menuStrip.Items.AddRange(new ToolStripItem[] { _tsmFile, helpToolStripMenuItem });
            _menuStrip.Location = new Point(0, 0);
            _menuStrip.Name = "_menuStrip";
            _menuStrip.Padding = new Padding(7, 2, 0, 2);
            _menuStrip.Size = new Size(1230, 38);
            _menuStrip.TabIndex = 0;
            _menuStrip.Text = "menuStrip1";
            // 
            // _tsmFile
            // 
            _tsmFile.DropDownItems.AddRange(new ToolStripItem[] { _tsmQuit });
            _tsmFile.Name = "_tsmFile";
            _tsmFile.Size = new Size(62, 34);
            _tsmFile.Text = "&File";
            // 
            // _tsmQuit
            // 
            _tsmQuit.Name = "_tsmQuit";
            _tsmQuit.Size = new Size(157, 38);
            _tsmQuit.Text = "Quit";
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { _tsmAbout });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(75, 34);
            helpToolStripMenuItem.Text = "&Help";
            // 
            // _tsmAbout
            // 
            _tsmAbout.Name = "_tsmAbout";
            _tsmAbout.Size = new Size(190, 38);
            _tsmAbout.Text = "About...";
            _tsmAbout.Click += About_Click;
            // 
            // _statusStrip
            // 
            _statusStrip.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            _statusStrip.ImageScalingSize = new Size(24, 24);
            _statusStrip.Items.AddRange(new ToolStripItem[] { _tslInfo });
            _statusStrip.Location = new Point(0, 695);
            _statusStrip.Name = "_statusStrip";
            _statusStrip.Padding = new Padding(1, 0, 17, 0);
            _statusStrip.Size = new Size(1230, 37);
            _statusStrip.TabIndex = 1;
            _statusStrip.Text = "statusStrip1";
            // 
            // _tslInfo
            // 
            _tslInfo.Name = "_tslInfo";
            _tslInfo.Size = new Size(1212, 30);
            _tslInfo.Spring = true;
            _tslInfo.Text = "#info";
            _tslInfo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // _toolStrip
            // 
            _toolStrip.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            _toolStrip.ImageScalingSize = new Size(32, 32);
            _toolStrip.Items.AddRange(new ToolStripItem[] { newToolStripButton, toolStripSeparator, copyToolStripButton, toolStripSeparator1, toolStripLabel1, _tscPersonalities, toolStripSeparator2 });
            _toolStrip.Location = new Point(0, 38);
            _toolStrip.Name = "_toolStrip";
            _toolStrip.Padding = new Padding(5);
            _toolStrip.Size = new Size(1230, 55);
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
            toolStripLabel1.Size = new Size(122, 45);
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
            _splitContainer.Location = new Point(0, 93);
            _splitContainer.Name = "_splitContainer";
            // 
            // _splitContainer.Panel1
            // 
            _splitContainer.Panel1.Controls.Add(treeView1);
            _splitContainer.Panel1.Padding = new Padding(10);
            // 
            // _splitContainer.Panel2
            // 
            _splitContainer.Panel2.Controls.Add(splitContainer1);
            _splitContainer.Panel2.Padding = new Padding(10);
            _splitContainer.Size = new Size(1230, 602);
            _splitContainer.SplitterDistance = 356;
            _splitContainer.TabIndex = 3;
            // 
            // treeView1
            // 
            treeView1.Dock = DockStyle.Fill;
            treeView1.Location = new Point(10, 10);
            treeView1.Name = "treeView1";
            treeView1.Size = new Size(336, 582);
            treeView1.TabIndex = 0;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(10, 10);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(_conversationView);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(_decoratorPanel);
            splitContainer1.Size = new Size(850, 582);
            splitContainer1.SplitterDistance = 438;
            splitContainer1.TabIndex = 0;
            // 
            // _conversationView
            // 
            _conversationView.Dock = DockStyle.Fill;
            _conversationView.HostPage = "wwwroot/index.html";
            _conversationView.Location = new Point(0, 0);
            _conversationView.Name = "_conversationView";
            _conversationView.Size = new Size(850, 438);
            _conversationView.TabIndex = 0;
            _conversationView.Text = "conversationView1";
            // 
            // _decoratorPanel
            // 
            _decoratorPanel.BorderThickness = 1;
            _decoratorPanel.Controls.Add(_promptControl);
            _decoratorPanel.Dock = DockStyle.Fill;
            _decoratorPanel.Location = new Point(0, 0);
            _decoratorPanel.Name = "_decoratorPanel";
            _decoratorPanel.Padding = new Padding(5);
            _decoratorPanel.Size = new Size(850, 140);
            _decoratorPanel.TabIndex = 0;
            // 
            // _promptControl
            // 
            _promptControl.BorderStyle = BorderStyle.None;
            _promptControl.Dock = DockStyle.Fill;
            _promptControl.Location = new Point(6, 5);
            _promptControl.Multiline = true;
            _promptControl.Name = "_promptControl";
            _promptControl.Size = new Size(838, 130);
            _promptControl.TabIndex = 0;
            _promptControl.AsyncSendPrompt += PromptControl_AsyncSendPrompt;
            // 
            // _semanticKernelCommunicator
            // 
            _semanticKernelCommunicator.TopP = null;
            // 
            // FrmMain
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1230, 732);
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
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
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
        private SplitContainer splitContainer1;
        private ConversationView _conversationView;
        private CommunityToolkit.WinForms.FluentUI.FluentDecoratorPanel _decoratorPanel;
        private CommunityToolkit.WinForms.AI.SemanticKernelComponent _semanticKernelCommunicator;
        private ToolStripComboBox _tscPersonalities;
        private ToolStripLabel toolStripLabel1;
        private ToolStripSeparator toolStripSeparator2;
        private SemanticKernelDemo.Controls.AsyncPromptControl _promptControl;
    }
}
