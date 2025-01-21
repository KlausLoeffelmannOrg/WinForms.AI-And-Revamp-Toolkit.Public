using CommunityToolkit.WinForms.Controls.Blazor;
using System.Text;

namespace Chatty.DataProcessing;

internal class ConversationProcessor(Conversation conversation, string basePath)
{
    public event EventHandler<ListingFileAddedEventArgs>? ListingFileAdded;

    private readonly List<string> _textFiles = [];
    private StringBuilder? _currentListingBuilder;
    private string? _currentListingType;

    private Conversation Conversation { get; } = conversation 
        ?? throw new ArgumentNullException(nameof(conversation));

    private string BasePath { get; } = basePath 
        ?? throw new ArgumentNullException(nameof(basePath));

    public void SaveConversation()
    {
        // Let's create a filename from the title and make sure,
        // it doesn't have any invalid characters:
        string title = Conversation.Title!;
        string? fileName = Conversation.Filename;

        if (string.IsNullOrWhiteSpace(fileName))
        {
            fileName = string.Join("_", title.Split(Path.GetInvalidFileNameChars()));
            Conversation.Filename = fileName;
        }

        // Let's open a file stream to write the conversation to:
        using FileStream fileStream = new(
            path: Path.Combine(BasePath, fileName + ".cjson"),
            mode: FileMode.Create,
            access: FileAccess.Write);

        Conversation.WriteJSon(fileStream);
    }

    internal void AddParagraph(string paragraph)
    {
        if (paragraph.StartsWith("```"))
        {
            // Handle listing start or end
            if (paragraph.Length > 3 && _currentListingBuilder == null)
            {
                // This is a start of a new listing with a specific type
                _currentListingType = paragraph[3..].Trim();
                _currentListingBuilder = new StringBuilder();
            }
            else if (_currentListingBuilder != null)
            {
                // This is the end of the current listing
                string listingContent = _currentListingBuilder.ToString();
                _textFiles.Add($"Type: {_currentListingType}\n{listingContent}");

                OnListingFileAdded(
                    new ListingFileAddedEventArgs(
                        GetListingTypeFromString(_currentListingType ?? ListingType.Other.ToString()),
                        listingContent, 
                        "Filename1"));

                _currentListingBuilder = null;
                _currentListingType = null;
            }
        }
        else
        {
            // Add paragraph to the current listing
            _currentListingBuilder?.AppendLine(paragraph);
        }

        static ListingType GetListingTypeFromString(string text)
            => Enum.TryParse(text, out ListingType listingType)
                ? listingType
                : ListingType.Other;
    }

    protected virtual void OnListingFileAdded(ListingFileAddedEventArgs e)
    {
        ListingFileAdded?.Invoke(this, e);
    }
}
