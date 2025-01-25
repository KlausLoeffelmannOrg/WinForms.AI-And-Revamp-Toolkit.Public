using Chatty.Views;
using CommunityToolkit.WinForms.Controls.Blazor;

namespace Chatty;

public partial class FrmMain
{
    private TreeNode UpdateTreeView(Guid id = default)
    {
        TreeNode nodeToReturn = null!;

        // Clear the TreeView:
        _trvConversationHistory.Nodes.Clear();


        // Get all files from the base path and sort them by date descending:
        List<Conversation> files = [.. Directory.EnumerateFiles(_options!.BasePath, "*.cjson")
            .Select(file => Conversation.GetHeaderFromFile(file))
            // We filter out the null entries.
            .OfType<Conversation>()
            .OrderByDescending(x => x.DateCreated)];

        // Add files to the TreeView:
        foreach (Conversation conversation in files)
        {
            DateTimeOffset date = conversation.DateCreated;

            TreeNode parentNode;

            parentNode = date.Date switch
            {
                var d when d == DateTime.Today => _knownNodes[KnownNode.Today],
                var d when d == DateTime.Today.AddDays(-1) => _knownNodes[KnownNode.Yesterday],
                var d when d >= DateTime.Today.AddDays(-7) => _knownNodes[KnownNode.LastWeek],
                var d when d >= DateTime.Today.AddDays(-14) => _knownNodes[KnownNode.LastTwoWeeks],
                var d when d >= DateTime.Today.AddMonths(-1) => _knownNodes[KnownNode.LastMonth],
                _ => _trvConversationHistory.Nodes.Cast<TreeNode>()
                    .FirstOrDefault(n => n.Text == date.ToString("MMMM yyyy"))
                    ?? _trvConversationHistory.Nodes.Add(date.ToString("MMMM yyyy"))
            };

            // Let's find out, if that node is already in the TreeView:
            if (!_trvConversationHistory.Nodes.Contains(parentNode))
            {
                _trvConversationHistory.Nodes.Add(parentNode);
            }

            TreeNode node = parentNode.Nodes.Add(conversation.Title);
            node.Tag = conversation;

            if (id != Guid.Empty && conversation.Id == id)
            {
                nodeToReturn = node;
            }

            if (!parentNode.IsExpanded)
            {
                parentNode.Expand();
            }
        }

        return nodeToReturn;
    }

    private void TrvConversationHistory_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
    {
        if (e.Node?.Tag is not Conversation conversation)
        {
            return;
        }

        // We're updating the status bar:
        UpdateStatusBar(conversation);
    }

    private void UpdateStatusBar(Conversation conversation)
    {
        _tslItemDateInfo.Text = $"{conversation.DateCreated:g}";
        _tsddKeywords.Text = null;
        _tsddKeywords.DropDownItems.Clear();

        ReportToStatusBarInfo("OK.");

        if (string.IsNullOrWhiteSpace(conversation.Keywords))
        {
            return;
        }

        foreach (string keyword in conversation.Keywords.Split(';'))
        {
            ToolStripMenuItem item = new(keyword);

            // We only show the first keyword in the status drop-down button.
            _tsddKeywords.Text ??= keyword;

            // TODO: We want to filter for keywords later.
            item.Click += (s, e) => _tsddKeywords.Text = keyword;
            _tsddKeywords.DropDownItems.Add(item);
        }
    }

    private void ConversationHistory_NodeMouseDoubleClick(
        object sender, 
        TreeNodeMouseClickEventArgs e)
    {
        // Let's get the ConservationView from the node's tag:
        if (e.Node!.Tag is not Conversation conversation)
        {
            // TODO: Error Message.
            return;
        }

        _currentNode = e.Node;

        // And read the content of the file:
        string fileFullName = conversation.Filename;

        string json = File.ReadAllText(fileFullName);

        // And load it into the conversation view:
        _chatView.ConversationView.FromJson(json);

        _skCommunicator.ChatHistory?.Clear();
        _lblConversationTitle.Text = conversation.Title;
        _lblDate.Text = conversation.DateLastEdited.ToString("F");

        foreach (ConversationItem item in _chatView.ConversationView.Conversation.ConversationItems)
        {
            _skCommunicator.AddChatItem(
                item.IsResponse,
                item.MarkdownContent!);
        }
    }

    private void ReportToStatusBarInfo(string message,
        string? toolTipText = null,
        string? additionalInfo = null,
        bool critical = false)
    {
        _tslInfo.Text = message;
        _tslInfo.ToolTipText = toolTipText is null ? message : toolTipText;
        _tslInfo.ForeColor = critical ? Color.Red : SystemColors.ControlText;
        _tsbInfo.Visible = !string.IsNullOrWhiteSpace(additionalInfo);
        _tsbInfo.DropDownItems.Clear();

        if (_tsbInfo.Visible)
        {
            _tsbInfo.DropDownItems.Add(additionalInfo);
        }
    }

    private void TsmOptions_Click(object sender, EventArgs e)
    {
        using FrmOptions frmOptions = new(_options!);

        if (frmOptions.ShowDialog() == DialogResult.OK)
        {
            _options = frmOptions.Options;
        }
    }

    private void About_Click(object sender, EventArgs e)
    {
        using FrmAbout about = new();
        about.ShowDialog();
    }
}
