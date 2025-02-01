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
        _decoratorPanel = new CommunityToolkit.WinForms.FluentUI.FluentDecoratorPanel();
        _promptControl = new CommunityToolkit.WinForms.FluentUI.Controls.AutoCompleteEditor();
        _chatToolStrip = new ToolStrip();
        _newPrompt = new ToolStripButton();
        _tsbRefreshMetaData = new ToolStripButton();
        _tsbSaveConversation = new ToolStripButton();
        toolStripSeparator = new ToolStripSeparator();
        _tsbCut = new ToolStripButton();
        _tsbCopy = new ToolStripButton();
        _tsbPaste = new ToolStripButton();
        toolStripSeparator1 = new ToolStripSeparator();
        ((System.ComponentModel.ISupportInitialize)_splitChatArea).BeginInit();
        _splitChatArea.Panel1.SuspendLayout();
        _splitChatArea.Panel2.SuspendLayout();
        _splitChatArea.SuspendLayout();
        _decoratorPanel.SuspendLayout();
        _chatToolStrip.SuspendLayout();
        SuspendLayout();
        // 
        // _splitChatArea
        // 
        _splitChatArea.Dock = DockStyle.Fill;
        _splitChatArea.Location = new Point(0, 0);
        _splitChatArea.Margin = new Padding(4);
        _splitChatArea.Name = "_splitChatArea";
        _splitChatArea.Orientation = Orientation.Horizontal;
        // 
        // _splitChatArea.Panel1
        // 
        _splitChatArea.Panel1.Controls.Add(_conversationView);
        _splitChatArea.Panel1.Padding = new Padding(6);
        // 
        // _splitChatArea.Panel2
        // 
        _splitChatArea.Panel2.Controls.Add(_decoratorPanel);
        _splitChatArea.Panel2.Controls.Add(_chatToolStrip);
        _splitChatArea.Panel2.Padding = new Padding(6);
        _splitChatArea.Size = new Size(1325, 929);
        _splitChatArea.SplitterDistance = 685;
        _splitChatArea.SplitterWidth = 5;
        _splitChatArea.TabIndex = 1;
        // 
        // _conversationView
        // 
        _conversationView.Dock = DockStyle.Fill;
        _conversationView.HostPage = "wwwroot/index.html";
        _conversationView.Location = new Point(6, 6);
        _conversationView.Margin = new Padding(4);
        _conversationView.Name = "_conversationView";
        _conversationView.Size = new Size(1313, 673);
        _conversationView.TabIndex = 0;
        _conversationView.Text = "conversationView1";
        // 
        // _decoratorPanel
        // 
        _decoratorPanel.BorderThickness = 1;
        _decoratorPanel.Controls.Add(_promptControl);
        _decoratorPanel.Dock = DockStyle.Fill;
        _decoratorPanel.Location = new Point(6, 48);
        _decoratorPanel.Margin = new Padding(4);
        _decoratorPanel.Name = "_decoratorPanel";
        _decoratorPanel.Padding = new Padding(13);
        _decoratorPanel.Size = new Size(1313, 185);
        _decoratorPanel.TabIndex = 0;
        _decoratorPanel.VerticalContentAlignment = CommunityToolkit.WinForms.FluentUI.FluentDecoratorPanel.VerticalContentAlignments.Fill;
        // 
        // _promptControl
        // 
        _promptControl.BorderStyle = BorderStyle.None;
        _promptControl.Location = new Point(14, 14);
        _promptControl.MinTimeSuggestionRequestSensitivity = 1F;
        _promptControl.Name = "_promptControl";
        _promptControl.Size = new Size(1285, 157);
        _promptControl.TabIndex = 0;
        _promptControl.Text = "";
        // 
        // _chatToolStrip
        // 
        _chatToolStrip.ImageScalingSize = new Size(32, 32);
        _chatToolStrip.Items.AddRange(new ToolStripItem[] { _newPrompt, _tsbRefreshMetaData, _tsbSaveConversation, toolStripSeparator, _tsbCut, _tsbCopy, _tsbPaste, toolStripSeparator1 });
        _chatToolStrip.Location = new Point(6, 6);
        _chatToolStrip.Name = "_chatToolStrip";
        _chatToolStrip.Size = new Size(1313, 42);
        _chatToolStrip.TabIndex = 1;
        _chatToolStrip.Text = "_promptToolStrip";
        // 
        // _newPrompt
        // 
        _newPrompt.DisplayStyle = ToolStripItemDisplayStyle.Image;
        _newPrompt.Image = (Image)resources.GetObject("_newPrompt.Image");
        _newPrompt.ImageTransparentColor = Color.Magenta;
        _newPrompt.Name = "_newPrompt";
        _newPrompt.Size = new Size(46, 36);
        _newPrompt.Text = "&New";
        // 
        // _tsbRefreshMetaData
        // 
        _tsbRefreshMetaData.DisplayStyle = ToolStripItemDisplayStyle.Image;
        _tsbRefreshMetaData.Image = (Image)resources.GetObject("_tsbRefreshMetaData.Image");
        _tsbRefreshMetaData.ImageTransparentColor = Color.Magenta;
        _tsbRefreshMetaData.Name = "_tsbRefreshMetaData";
        _tsbRefreshMetaData.Size = new Size(46, 36);
        _tsbRefreshMetaData.Text = "Refresh MetaData";
        _tsbRefreshMetaData.Click += TsbRefreshMetaData_Click;
        // 
        // _tsbSaveConversation
        // 
        _tsbSaveConversation.DisplayStyle = ToolStripItemDisplayStyle.Image;
        _tsbSaveConversation.Image = (Image)resources.GetObject("_tsbSaveConversation.Image");
        _tsbSaveConversation.ImageTransparentColor = Color.Magenta;
        _tsbSaveConversation.Name = "_tsbSaveConversation";
        _tsbSaveConversation.Size = new Size(46, 36);
        _tsbSaveConversation.Text = "&Save";
        // 
        // toolStripSeparator
        // 
        toolStripSeparator.Name = "toolStripSeparator";
        toolStripSeparator.Size = new Size(6, 42);
        // 
        // _tsbCut
        // 
        _tsbCut.DisplayStyle = ToolStripItemDisplayStyle.Image;
        _tsbCut.Image = (Image)resources.GetObject("_tsbCut.Image");
        _tsbCut.ImageTransparentColor = Color.Magenta;
        _tsbCut.Name = "_tsbCut";
        _tsbCut.Size = new Size(46, 36);
        _tsbCut.Text = "C&ut";
        // 
        // _tsbCopy
        // 
        _tsbCopy.DisplayStyle = ToolStripItemDisplayStyle.Image;
        _tsbCopy.Image = (Image)resources.GetObject("_tsbCopy.Image");
        _tsbCopy.ImageTransparentColor = Color.Magenta;
        _tsbCopy.Name = "_tsbCopy";
        _tsbCopy.Size = new Size(46, 36);
        _tsbCopy.Text = "&Copy";
        // 
        // _tsbPaste
        // 
        _tsbPaste.DisplayStyle = ToolStripItemDisplayStyle.Image;
        _tsbPaste.Image = (Image)resources.GetObject("_tsbPaste.Image");
        _tsbPaste.ImageTransparentColor = Color.Magenta;
        _tsbPaste.Name = "_tsbPaste";
        _tsbPaste.Size = new Size(46, 36);
        _tsbPaste.Text = "&Paste";
        // 
        // toolStripSeparator1
        // 
        toolStripSeparator1.Name = "toolStripSeparator1";
        toolStripSeparator1.Size = new Size(6, 42);
        // 
        // ChatView
        // 
        AutoScaleDimensions = new SizeF(13F, 32F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(_splitChatArea);
        Margin = new Padding(4);
        Name = "ChatView";
        Size = new Size(1325, 929);
        _splitChatArea.Panel1.ResumeLayout(false);
        _splitChatArea.Panel2.ResumeLayout(false);
        _splitChatArea.Panel2.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)_splitChatArea).EndInit();
        _splitChatArea.ResumeLayout(false);
        _decoratorPanel.ResumeLayout(false);
        _chatToolStrip.ResumeLayout(false);
        _chatToolStrip.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    private SplitContainer _splitChatArea;
    private CommunityToolkit.WinForms.Controls.Blazor.ConversationView _conversationView;
    private CommunityToolkit.WinForms.FluentUI.FluentDecoratorPanel _decoratorPanel;
    private ToolStrip _chatToolStrip;
    private ToolStripButton _newPrompt;
    private ToolStripButton _tsbRefreshMetaData;
    private ToolStripButton _tsbSaveConversation;
    private ToolStripSeparator toolStripSeparator;
    private ToolStripButton _tsbCut;
    private ToolStripButton _tsbCopy;
    private ToolStripButton _tsbPaste;
    private ToolStripSeparator toolStripSeparator1;
    private CommunityToolkit.WinForms.FluentUI.Controls.AutoCompleteEditor _promptControl;
}
