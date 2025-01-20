namespace Chatty.Views;

partial class ChatView
{
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Clean up any resources being used.
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChatView));
        _splitChatArea = new SplitContainer();
        _conversationView = new CommunityToolkit.WinForms.Controls.Blazor.ConversationView();
        _chatToolStrip = new ToolStrip();
        _decoratorPanel = new CommunityToolkit.WinForms.FluentUI.FluentDecoratorPanel();
        _promptControl = new SemanticKernelDemo.Controls.AsyncPromptControl();
        toolStripButton1 = new ToolStripButton();
        ((System.ComponentModel.ISupportInitialize)_splitChatArea).BeginInit();
        _splitChatArea.Panel1.SuspendLayout();
        _splitChatArea.Panel2.SuspendLayout();
        _splitChatArea.SuspendLayout();
        _chatToolStrip.SuspendLayout();
        _decoratorPanel.SuspendLayout();
        SuspendLayout();
        // 
        // _splitChatArea
        // 
        _splitChatArea.Dock = DockStyle.Fill;
        _splitChatArea.Location = new Point(0, 0);
        _splitChatArea.Name = "_splitChatArea";
        _splitChatArea.Orientation = Orientation.Horizontal;
        // 
        // _splitChatArea.Panel1
        // 
        _splitChatArea.Panel1.Controls.Add(_conversationView);
        _splitChatArea.Panel1.Padding = new Padding(5);
        // 
        // _splitChatArea.Panel2
        // 
        _splitChatArea.Panel2.Controls.Add(_decoratorPanel);
        _splitChatArea.Panel2.Controls.Add(_chatToolStrip);
        _splitChatArea.Panel2.Padding = new Padding(5);
        _splitChatArea.Size = new Size(1019, 726);
        _splitChatArea.SplitterDistance = 537;
        _splitChatArea.TabIndex = 1;
        // 
        // _conversationView
        // 
        _conversationView.ConversationTitle = "New";
        _conversationView.Dock = DockStyle.Fill;
        _conversationView.HostPage = "wwwroot/index.html";
        _conversationView.Location = new Point(5, 5);
        _conversationView.Name = "_conversationView";
        _conversationView.Size = new Size(1009, 527);
        _conversationView.TabIndex = 0;
        _conversationView.Text = "conversationView1";
        // 
        // toolStrip1
        // 
        _chatToolStrip.ImageScalingSize = new Size(32, 32);
        _chatToolStrip.Items.AddRange(new ToolStripItem[] { toolStripButton1 });
        _chatToolStrip.Location = new Point(5, 5);
        _chatToolStrip.Name = "toolStrip1";
        _chatToolStrip.Size = new Size(1009, 41);
        _chatToolStrip.TabIndex = 1;
        _chatToolStrip.Text = "_promptToolStrip";
        // 
        // _decoratorPanel
        // 
        _decoratorPanel.BorderThickness = 1;
        _decoratorPanel.Controls.Add(_promptControl);
        _decoratorPanel.Dock = DockStyle.Fill;
        _decoratorPanel.Location = new Point(5, 46);
        _decoratorPanel.Name = "_decoratorPanel";
        _decoratorPanel.Padding = new Padding(10);
        _decoratorPanel.Size = new Size(1009, 134);
        _decoratorPanel.TabIndex = 0;
        // 
        // _promptControl
        // 
        _promptControl.BorderStyle = BorderStyle.None;
        _promptControl.Dock = DockStyle.Fill;
        _promptControl.Location = new Point(11, 10);
        _promptControl.Multiline = true;
        _promptControl.Name = "_promptControl";
        _promptControl.Size = new Size(987, 114);
        _promptControl.TabIndex = 0;
        // 
        // toolStripButton1
        // 
        toolStripButton1.DisplayStyle = ToolStripItemDisplayStyle.Image;
        toolStripButton1.Image = (Image)resources.GetObject("toolStripButton1.Image");
        toolStripButton1.ImageTransparentColor = Color.Magenta;
        toolStripButton1.Name = "toolStripButton1";
        toolStripButton1.Size = new Size(36, 36);
        toolStripButton1.Text = "toolStripButton1";
        // 
        // ChatView
        // 
        AutoScaleDimensions = new SizeF(10F, 25F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(_splitChatArea);
        Name = "ChatView";
        Size = new Size(1019, 726);
        _splitChatArea.Panel1.ResumeLayout(false);
        _splitChatArea.Panel2.ResumeLayout(false);
        _splitChatArea.Panel2.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)_splitChatArea).EndInit();
        _splitChatArea.ResumeLayout(false);
        _chatToolStrip.ResumeLayout(false);
        _chatToolStrip.PerformLayout();
        _decoratorPanel.ResumeLayout(false);
        _decoratorPanel.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    private SplitContainer _splitChatArea;
    private CommunityToolkit.WinForms.Controls.Blazor.ConversationView _conversationView;
    private CommunityToolkit.WinForms.FluentUI.FluentDecoratorPanel _decoratorPanel;
    private SemanticKernelDemo.Controls.AsyncPromptControl _promptControl;
    private ToolStrip _chatToolStrip;
    private ToolStripButton toolStripButton1;
}
