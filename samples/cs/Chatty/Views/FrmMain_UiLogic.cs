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

        // Create nodes for time periods:
        var todayNode = _trvConversationHistory.Nodes.Add("Today");
        var yesterdayNode = _trvConversationHistory.Nodes.Add("Yesterday");
        var lastWeekNode = _trvConversationHistory.Nodes.Add("Last week");
        var lastTwoWeeksNode = _trvConversationHistory.Nodes.Add("Last 2 weeks");
        var lastMonthNode = _trvConversationHistory.Nodes.Add("Last month");

        // Set nodes to bold and expand them:
        todayNode.NodeFont = new Font(_trvConversationHistory.Font, FontStyle.Bold);
        yesterdayNode.NodeFont = new Font(_trvConversationHistory.Font, FontStyle.Bold);
        lastWeekNode.NodeFont = new Font(_trvConversationHistory.Font, FontStyle.Bold);
        lastTwoWeeksNode.NodeFont = new Font(_trvConversationHistory.Font, FontStyle.Bold);
        lastMonthNode.NodeFont = new Font(_trvConversationHistory.Font, FontStyle.Bold);

        // And add all files from the base path:
        foreach (string file in Directory.EnumerateFiles(_options!.BasePath, "*.cjson"))
        {
            var conversation = Conversation.GetHeaderFromFile(file);
            conversation.Filename = file;

            DateTimeOffset date = conversation.DateLastEdited;

            TreeNode parentNode;

            parentNode = date.Date switch
            {
                var d when d == DateTime.Today => todayNode,
                var d when d == DateTime.Today.AddDays(-1) => yesterdayNode,
                var d when d >= DateTime.Today.AddDays(-7) => lastWeekNode,
                var d when d >= DateTime.Today.AddDays(-14) => lastTwoWeeksNode,
                var d when d >= DateTime.Today.AddMonths(-1) => lastMonthNode,
                _ => _trvConversationHistory.Nodes.Cast<TreeNode>()
                    .FirstOrDefault(n => n.Text == date.ToString("MMMM yyyy"))
                    ?? _trvConversationHistory.Nodes.Add(date.ToString("MMMM yyyy"))
            };

            TreeNode node = parentNode.Nodes.Add(conversation.Title);
            node.Tag = conversation;

            if (id != Guid.Empty && conversation.Id == id)
            {
                nodeToReturn = node;
            }

            // Add metadata nodes
            node.Nodes.Add($"Date Created/Last edited: {conversation.DateCreated:f}/{conversation.DateLastEdited:f}");
            node.Nodes.Add($"Keywords: {conversation.Keywords}");
            node.Nodes.Add($"Token count: {conversation.TokenCount:#,###}");

            if (!parentNode.IsExpanded)
            {
                parentNode.Expand();
            }
        }

        if (id != Guid.Empty && nodeToReturn is null)
        {
            // Let's fail here. Someone tempered with the data files.
            throw new NullReferenceException($"Node not found - the respective data file seems to be corrupted.");
        }

        return nodeToReturn;
    }

    private void ConversationHistory_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
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
