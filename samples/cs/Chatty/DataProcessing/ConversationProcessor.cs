using Chatty.DataEntities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.WinForms.Controls.Blazor;
using System.Text;

namespace Chatty.DataProcessing;

internal class ConversationProcessor(Conversation conversation, string basePath)
{
    public event EventHandler<ListingFileAddedEventArgs>? ListingFileAdded;

    private readonly List<ListingFile> _textFiles = [];
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
            fileName = GenerateUniqueFilename(BasePath, title, ".cjson");
            Conversation.Filename = fileName;
        }

        // Let's open a file stream to write the conversation to:
        using FileStream fileStream = new(
            path: fileName,
            mode: FileMode.Create,
            access: FileAccess.Write);

        Conversation.WriteJSon(fileStream);

        static string GenerateUniqueFilename(string basePath, string title, string extension)
        {
            // Remove invalid characters from the title
            string sanitizedTitle = string.Concat(title.Split(Path.GetInvalidFileNameChars()));
            string fileName = sanitizedTitle;

            if (string.IsNullOrWhiteSpace(fileName))
            {
                int counter = 1;

                // Ensure the filename is unique
                while (File.Exists(Path.Combine(basePath, fileName + extension)))
                {
                    fileName = $"{sanitizedTitle}_{counter}";
                    counter++;
                }
            }

            return Path.Combine(basePath, fileName + extension);
        }
    }

    internal void AddParagraph(string paragraph)
    {
        string trimmedParagraph = paragraph.Trim();

        if (trimmedParagraph.StartsWith("```"))
        {
            // Handle listing start or end
            if (trimmedParagraph.Length > 3 && _currentListingBuilder == null)
            {
                // This is a start of a new listing with a specific type
                _currentListingType = trimmedParagraph[3..].Trim();
                _currentListingBuilder = new StringBuilder();
            }
            else if (_currentListingBuilder != null)
            {
                // This is the end of the current listing
                string listingContent = _currentListingBuilder.ToString();
                var listingFile = new ListingFile(basePath, listingContent);
                OnListingFileAdded(new(listingFile));

                _currentListingBuilder = null;
                _currentListingType = null;
            }
        }
        else
        {
            // Add paragraph to the current listing
            // Note: The paragraph is not trimmed, so we only Append
            // to not add additional whitespace.
            _currentListingBuilder?.Append(paragraph);
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
