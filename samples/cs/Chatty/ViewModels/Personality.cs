using CommunityToolkit.Mvvm.ComponentModel;
using System.Text.Json;

namespace Chatty.ViewModels;

/// <summary>
/// Represents a view model for a personality item.
/// </summary>
internal partial class Personality : ObservableObject
{
    public Personality()
    {
    }

    public Personality(Personality personality)
    {
        Id = personality.Id;
        Name = personality.Name;
        SystemPrompt = personality.SystemPrompt;
        DateCreated = personality.DateCreated;
        DateLastEdited = DateTime.Now;
        FileExtractionSettings = personality.FileExtractionSettings;
    }

    /// <summary>
    /// Gets or sets the unique identifier for the personality item.
    /// </summary>
    [ObservableProperty]
    private Guid _id = Guid.NewGuid();

    /// <summary>
    /// Gets or sets the name of the personality item.
    /// </summary>
    [ObservableProperty]
    private string _name = string.Empty;

    /// <summary>
    /// Gets or sets the system prompt associated with the personality item.
    /// </summary>
    [ObservableProperty]
    private string _systemPrompt = string.Empty;

    /// <summary>
    /// Gets or sets the date and time when the personality item was created.
    /// </summary>
    [ObservableProperty]
    private DateTime _dateCreated = DateTime.Now;

    /// <summary>
    /// Gets or sets the date and time when the personality item was last edited.
    /// </summary>
    [ObservableProperty]
    private DateTime _dateLastEdited = DateTime.Now;

    [ObservableProperty]
    private PersonalityFileExtractionSetting _fileExtractionSettings = PersonalityFileExtractionSetting.None;

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString() => $"{Name}";

    /// <summary>
    /// Creates a new instance of <see cref="Personality"/> from a JSON element.
    /// </summary>
    /// <param name="jsonElement">The JSON element to parse.</param>
    /// <returns>A new instance of <see cref="Personality"/>.</returns>
    public static Personality FromJson(JsonElement jsonElement)
    {
        Personality viewModel = new()
        {
            Id = jsonElement.GetProperty(nameof(Id)).GetGuid(),
            Name = jsonElement.GetProperty(nameof(Name)).GetString() ?? string.Empty,
            SystemPrompt = jsonElement.GetProperty(nameof(SystemPrompt)).GetString() ?? string.Empty,
            DateCreated = jsonElement.GetProperty(nameof(DateCreated)).GetDateTime(),
            DateLastEdited = jsonElement.GetProperty(nameof(DateLastEdited)).GetDateTime()
        };

        if (jsonElement.TryGetProperty(nameof(FileExtractionSettings), out JsonElement fileExtractionSetting))
        {
            viewModel.FileExtractionSettings = (PersonalityFileExtractionSetting) fileExtractionSetting.GetInt32();
        }

        return viewModel;
    }

    /// <summary>
    /// Writes the current instance to a JSON writer.
    /// </summary>
    /// <param name="writer">The JSON writer to write to.</param>
    public void WriteJSon(Utf8JsonWriter writer)
    {
        writer.WriteString(nameof(Id), Id);
        writer.WriteString(nameof(Name), Name);
        writer.WriteString(nameof(SystemPrompt), SystemPrompt);
        writer.WriteNumber(nameof(FileExtractionSettings), (int) FileExtractionSettings);
        writer.WriteString(nameof(DateCreated), DateCreated);
        writer.WriteString(nameof(DateLastEdited), DateLastEdited);
    }
}
