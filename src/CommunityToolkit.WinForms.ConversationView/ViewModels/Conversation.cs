using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.WinForms.ConversationView.Extensions;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace CommunityToolkit.WinForms.Controls.Blazor;

public partial class Conversation()
    : ObservableObject
{
    public Conversation(string title) : this()
    {
        _title = title;
        _id = Guid.NewGuid();
        _backColor = string.Empty;
        _foreColor = string.Empty;
        _dateCreated = DateTimeOffset.Now;
        _dateLastEdited = DateTimeOffset.Now;
        _backColor = string.Empty;
        _foreColor = string.Empty;
    }

    [ObservableProperty]
    private Guid _id;

    [ObservableProperty]
    private string? _headline;

    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private string _summary = string.Empty;

    [ObservableProperty]
    private string _keywords = string.Empty;

    [ObservableProperty]
    private int _tokenCount;

    [ObservableProperty]
    private string _filename = string.Empty;

    [ObservableProperty]
    private DateTimeOffset _dateCreated = DateTimeOffset.Now;

    [ObservableProperty]
    private DateTimeOffset _dateLastEdited = DateTimeOffset.Now;

    [ObservableProperty]
    private string _backColor = string.Empty;

    [ObservableProperty]
    private string _foreColor = string.Empty;

    [ObservableProperty]
    private string? _responseInProgress;

    [ObservableProperty]
    private string? _responseInProgressBackColor;

    [ObservableProperty]
    private ObservableCollection<ConversationItem> _conversationItems = [];

    public void WriteJSon(Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream, nameof(stream));

        JsonWriterOptions options = new()
        {
            Indented = true,
        };

        // Let's use System.Text.Json to serialize the object:
        using Utf8JsonWriter writer = new(stream, options);

        writer.WriteStartObject();
        writer.WriteString(nameof(Id), Id);
        writer.WriteString(nameof(Title), Title);
        writer.WriteString(nameof(Filename), Filename);
        writer.WriteString(nameof(DateCreated), DateCreated);
        writer.WriteString(nameof(DateLastEdited), DateLastEdited);
        writer.WriteString(nameof(Summary), Summary);
        writer.WriteString(nameof(Keywords), Keywords);
        writer.WriteNumber(nameof(TokenCount), TokenCount);

        writer.WriteStartArray(nameof(ConversationItems));

        foreach (ConversationItem item in ConversationItems)
        {
            writer.WriteStartObject();
            item.WriteJSon(writer);
            writer.WriteEndObject();
        }

        writer.WriteEndArray();
        writer.WriteEndObject();
    }

    public static Conversation FromJSon(Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream, nameof(stream));

        using JsonDocument document = JsonDocument.Parse(stream);
        JsonElement root = document.RootElement;

        Conversation conversation = Conversation.GetHeaderFromRoot(root);

        if (!root.TryGetProperty(nameof(ConversationItems), out JsonElement conversationItemsElement))
        {
            return conversation;
        }

        foreach (JsonElement itemElement in conversationItemsElement.EnumerateArray())
        {
            var item = ConversationItem.FromJsonElement(itemElement);
            conversation.ConversationItems.Add(item);
        }

        return conversation;
    }

    public static Conversation GetHeaderFromFile(string fileName)
    {
        using Stream stream = File.OpenRead(fileName);
        return GetHeaderFromStream(stream);
    }

    public static Conversation GetHeaderFromStream(Stream stream)
    {
        using JsonDocument document = JsonDocument.Parse(stream);
        JsonElement root = document.RootElement;

        var conversation = GetHeaderFromRoot(root);

        return conversation;
    }

    private static Conversation GetHeaderFromRoot(JsonElement root)
    {
        Conversation conversation = new();

        conversation.Title = root.GetPropertyOrDefault(nameof(Title), conversation.Title);
        conversation.Id = root.GetPropertyOrDefault(nameof(Id), conversation.Id);
        conversation.Filename = root.GetPropertyOrDefault(nameof(Filename), conversation.Filename);
        conversation.DateCreated = root.GetPropertyOrDefault(nameof(DateCreated), conversation.DateCreated);
        conversation.DateLastEdited = root.GetPropertyOrDefault(nameof(DateLastEdited), conversation.DateLastEdited);
        conversation.Summary = root.GetPropertyOrDefault(nameof(Summary), conversation.Summary);
        conversation.Keywords = root.GetPropertyOrDefault(nameof(Keywords), conversation.Keywords);
        conversation.TokenCount = root.GetPropertyOrDefault(nameof(TokenCount), conversation.TokenCount);

        return conversation;
    }
}
