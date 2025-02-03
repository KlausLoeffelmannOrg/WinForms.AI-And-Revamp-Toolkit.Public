using Chatty.DataProcessing;
using CommunityToolkit.WinForms.AI;

namespace Chatty.Views;

public partial class ChatView : UserControl
{
    private string? _lastListingTitle;
    private string? _lastListingFilename;

    private Task ConversationView_ReceivedInlineMetaDataAsync(object? sender, AsyncReceivedInlineMetaDataEventArgs e)
    {
        // TODO: We have the refactor that to the new generalized way of handling inline-meta data.
        if (e.MetaData.Contains(':'))
        {
            _lastListingTitle = e.MetaData;
        }
        else
        {
            _lastListingFilename = e.MetaData;
        }

        if (_conversationProcessor is null)
        {
            return Task.CompletedTask;
        }

        _conversationProcessor.CurrentListingDescription = _lastListingTitle ?? string.Empty;
        _conversationProcessor.CurrentListingFilename = _lastListingFilename ?? string.Empty;

        return Task.CompletedTask;
    }

    private async Task SKConversationView_ReceivedNextParagraph(object? sender, AsyncReceivedNextParagraphEventArgs e)
    {
        if (_conversationProcessor is null)
        {
            _conversationProcessor = new(
                conversation: ConversationView.Conversation,
                _options.BasePath ?? throw new InvalidOperationException("BasePath is not set."));

            _conversationProcessor.AsyncListingFileAdded += ConversationProcessor_ListingFileAdded;
        }

        await _conversationProcessor.HandleNewParagraphAsync(
            paragraph: e.Paragraph,
            textPosition: e.TextPosition,
            isLastParagraph: e.IsLastParagraph);

        Task ConversationProcessor_ListingFileAdded(object? sender, AsyncListingFileAddedEventArgs e) 
            => OnAsyncListingFileAddedAsync(e);
    }
}
