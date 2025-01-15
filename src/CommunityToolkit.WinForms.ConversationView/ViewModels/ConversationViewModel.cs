using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace CommunityToolkit.WinForms.Controls.Blazor;

public partial class ConversationViewModel()
    : ObservableObject
{
    public ConversationViewModel(string title) : this()
    {
        _title = title;
        _backColor = string.Empty;
        _foreColor = string.Empty;
        _dateCreated = DateTimeOffset.Now;
        _dateLastEdited = DateTimeOffset.Now;
        _backColor = string.Empty;
        _foreColor = string.Empty;
    }

    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private string _summary = string.Empty;

    [ObservableProperty]
    private string _keywords = string.Empty;

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
    private ObservableCollection<ConversationItemViewModel> _conversationItems = [];

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
        writer.WriteString(nameof(Title), Title);
        writer.WriteString(nameof(Filename), Filename);
        writer.WriteString(nameof(DateCreated), DateCreated);
        writer.WriteString(nameof(DateLastEdited), DateLastEdited);
        writer.WriteString(nameof(Summary), Summary);
        writer.WriteString(nameof(Keywords), Keywords);

        writer.WriteStartArray(nameof(ConversationItems));

        foreach (ConversationItemViewModel item in ConversationItems)
        {
            writer.WriteStartObject();
            item.WriteJSon(writer);
            writer.WriteEndObject();
        }

        writer.WriteEndArray();
        writer.WriteEndObject();
    }

    public static ConversationViewModel FromJSon(Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream, nameof(stream));

        using JsonDocument document = JsonDocument.Parse(stream);
        JsonElement root = document.RootElement;

        ConversationViewModel conversation = ConversationViewModel.GetHeaderFromStream(stream);

        if (root.TryGetProperty(nameof(ConversationItems), out JsonElement conversationItemsElement))
        {
            foreach (JsonElement itemElement in conversationItemsElement.EnumerateArray())
            {
                var item = ConversationItemViewModel.FromJsonElement(itemElement);
                conversation.ConversationItems.Add(item);
            }
        }

        return conversation;
    }

    public static ConversationViewModel GetHeaderFromFile(string fileName)
    {
        using Stream stream = File.OpenRead(fileName);
        return GetHeaderFromStream(stream);
    }

    public static ConversationViewModel GetHeaderFromStream(Stream stream)
    {
        ConversationViewModel conversation = new();

        using JsonDocument document = JsonDocument.Parse(stream);
        JsonElement root = document.RootElement;

        conversation.Title = root.GetProperty(nameof(Title)).GetString() ?? string.Empty;
        conversation.Filename = root.GetProperty(nameof(Filename)).GetString() ?? string.Empty;
        conversation.DateCreated = root.GetProperty(nameof(DateCreated)).GetDateTimeOffset();
        conversation.DateLastEdited = root.GetProperty(nameof(DateLastEdited)).GetDateTimeOffset();
        conversation.Summary = root.GetProperty(nameof(Summary)).GetString() ?? string.Empty;
        conversation.Keywords = root.GetProperty(nameof(Keywords)).GetString() ?? string.Empty;

        return conversation;
    }
}
