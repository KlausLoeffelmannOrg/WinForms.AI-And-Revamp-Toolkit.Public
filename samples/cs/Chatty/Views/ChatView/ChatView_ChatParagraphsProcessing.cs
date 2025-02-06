using CommunityToolkit.WinForms.AI;

namespace Chatty.Views;

public partial class ChatView : UserControl
{
    private string? _lastListingTitle;
    private string? _lastListingFilename;
    private readonly Lock _conversationLock = new();

    private Task SKCommunikator_ReceivedInlineMetaDataAsync(object? sender, AsyncReceivedInlineMetaDataEventArgs e)
    {
        if (e.MetaData.Key == "ListingTitle")
        {
            _lastListingTitle = e.MetaData.Value;
        }
        else if (e.MetaData.Key == "ListingFilename")
        {
            _lastListingFilename = e.MetaData.Value;
        }

        if (_conversationProcessor is null)
        {
            return Task.CompletedTask;
        }

        _conversationProcessor.CurrentListingDescription = _lastListingTitle ?? string.Empty;
        _conversationProcessor.CurrentListingFilename = _lastListingFilename ?? string.Empty;

        return Task.CompletedTask;
    }

    private async Task SKCommunicator_ReceivedNextParagraph(object? sender, AsyncReceivedNextParagraphEventArgs e)
    {
        lock (_conversationLock)
        {
            if (_conversationProcessor is null)
            {
                _conversationProcessor = new(
                    conversation: ConversationView.Conversation,
                    _options.BasePath ?? throw new InvalidOperationException("BasePath is not set."));
            }
        }

        await _conversationProcessor.HandleNewParagraphAsync(
            paragraph: e.Paragraph,
            textPosition: e.TextPosition,
            isLastParagraph: e.IsLastParagraph);
    }
}
