using System.Text.Json;

namespace Chatty.ViewModels;

/// <summary>
/// Represents a view model for a personality item.
/// </summary>
internal class PersonalityItemViewModel
{
    /// <summary>
    /// Gets or sets the unique identifier for the personality item.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets the name of the personality item.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the system prompt associated with the personality item.
    /// </summary>
    public string SystemPrompt { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date and time when the personality item was created.
    /// </summary>
    public DateTime DateCreated { get; set; } = DateTime.Now;

    /// <summary>
    /// Gets or sets the date and time when the personality item was last edited.
    /// </summary>
    public DateTime DateLastEdited { get; set; } = DateTime.Now;

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString() => $"{Name}";

    /// <summary>
    /// Creates a new instance of <see cref="PersonalityItemViewModel"/> from a JSON element.
    /// </summary>
    /// <param name="jsonElement">The JSON element to parse.</param>
    /// <returns>A new instance of <see cref="PersonalityItemViewModel"/>.</returns>
    public static PersonalityItemViewModel FromJson(JsonElement jsonElement)
    {
        PersonalityItemViewModel viewModel = new()
        {
            Id = jsonElement.GetProperty(nameof(Id)).GetGuid(),
            Name = jsonElement.GetProperty(nameof(Name)).GetString() ?? string.Empty,
            SystemPrompt = jsonElement.GetProperty(nameof(SystemPrompt)).GetString() ?? string.Empty,
            DateCreated = jsonElement.GetProperty(nameof(DateCreated)).GetDateTime(),
            DateLastEdited = jsonElement.GetProperty(nameof(DateLastEdited)).GetDateTime()
        };

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
        writer.WriteString(nameof(DateCreated), DateCreated);
        writer.WriteString(nameof(DateLastEdited), DateLastEdited);
    }
}
