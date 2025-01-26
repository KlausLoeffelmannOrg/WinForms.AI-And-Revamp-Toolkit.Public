using System.Text.Json;

namespace Chatty.ViewModels;

/// <summary>
/// Represents the view model for managing personalities.
/// </summary>
internal class PersonalityViewModel
{
    private const string PersonalitiesFileName = "personalities.json";

    /// <summary>
    /// Gets or sets the list of personality items.
    /// </summary>
    public List<PersonalityItemViewModel> Personalities { get; set; } = [];

    /// <summary>
    /// Gets or sets the selected personality item.
    /// </summary>
    public PersonalityItemViewModel? SelectedPersonality { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the personality is new.
    /// </summary>
    public bool IsNew { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether the personality has unsaved changes.
    /// </summary>
    public bool IsDirty { get; set; } = false;

    /// <summary>
    /// Gets or sets the name being edited.
    /// </summary>
    public string? EditName { get; set; } = null;

    /// <summary>
    /// Gets or sets the description being edited.
    /// </summary>
    public string? EditDescription { get; set; } = null;

    /// <summary>
    /// Writes the current instance to a JSON stream.
    /// </summary>
    /// <param name="stream">The stream to write to.</param>
    public void WriteJSon(Stream stream)
    {
        JsonWriterOptions options = new()
        {
            Indented = true,
        };

        using Utf8JsonWriter writer = new(stream, options);

        writer.WriteStartObject();
        writer.WriteStartArray(nameof(Personalities));

        foreach (PersonalityItemViewModel item in Personalities)
        {
            writer.WriteStartObject();
            item.WriteJSon(writer);
            writer.WriteEndObject();
        }

        writer.WriteEndArray();
        writer.WriteEndObject();
    }

    /// <summary>
    /// Gets the default personality templates.
    /// </summary>
    /// <returns>A new instance of <see cref="PersonalityViewModel"/> with default templates.</returns>
    public static PersonalityViewModel GetTemplates() =>
        new()
        {
            Personalities = [.. s_personalities
                    .Select(
                        kv => new PersonalityItemViewModel
                        {
                            Name = kv.Key,
                            SystemPrompt = kv.Value
                        })]
        };

    /// <summary>
    /// Creates a new instance of <see cref="PersonalityViewModel"/> from a JSON stream.
    /// </summary>
    /// <param name="stream">The stream to read from.</param>
    /// <returns>A new instance of <see cref="PersonalityViewModel"/>.</returns>
    public static PersonalityViewModel FromJSon(Stream stream)
    {
        PersonalityViewModel personality = new();

        using JsonDocument document = JsonDocument.Parse(stream);
        JsonElement root = document.RootElement;

        if (root.TryGetProperty(nameof(Personalities), out JsonElement personalitiesItemElement))
        {
            foreach (JsonElement itemElement in personalitiesItemElement.EnumerateArray())
            {
                var item = PersonalityItemViewModel.FromJson(itemElement);
                personality.Personalities.Add(item);
            }
        }

        return personality;
    }

    /// <summary>
    /// Gets the personalities from the specified base path or returns the default templates if the file does not exist.
    /// </summary>
    /// <param name="basePath">The base path to look for the personalities file.</param>
    /// <returns>A new instance of <see cref="PersonalityViewModel"/>.</returns>
    internal static PersonalityViewModel GetPersonalitiesOrDefault(string basePath)
    {
        PersonalityViewModel personalities;

        string fileName = Path.Combine(basePath, PersonalitiesFileName);
        if (File.Exists(fileName))
        {
            using FileStream readerStream = File.OpenRead(fileName);
            personalities = PersonalityViewModel.FromJSon(readerStream);

            return personalities;
        }

        personalities = PersonalityViewModel.GetTemplates();

        using FileStream writerStream = File.OpenWrite(fileName);
        personalities.WriteJSon(writerStream);
        return personalities;
    }

    private static readonly Dictionary<string, string> s_personalities = new()
        {
            {
                "Default",
                "You are a friendly, versatile Assistant, trying to address all requests to the best of your abilities, "
              + "and you are engaged, even if folks just want to chat. But you politely decline requests "
              + "that are inappropriate or impolite."
            },
            {
                "Funny/Ironic",
                "You are a witty and ironic Assistant, always ready with a playful quip or punchline. Provide helpful answers, "
              + "but pepper them with dry humor or lighthearted irony. Keep responses fun, yet respectful."
            },
            {
                "C#/VB Coding Assistant","""
                You are a highly trained Developer and Architect for VB and C#.
                You handle these topics with expertise but you politely decline or redirect any non-Developer topics.
                When you craft code, please make sure to always apply modern coding standards such as
                * incorperating Null Reference Types and assuming `#nullable` is enabled
                * applying collection initializers (always assume > C# 13)
                * using short-forms of `using` or `namespace` over the curly-braces version.

                Important:
                * Please put every class in a separate listing-section in the resulting Markdown responses, unless asked not to.
                * Start every MD-Listing not only with ```[ListingType] but also include a describing filename you derive from the content
                  and also in another pair of double-curley braces, the abbrevation of the listing type (like 'CS:', 'VB:', 'CMD:' or 'PY:' 
                  and a very brief content description with maximal 50 characters.
                  - Sample 1: ```csharp{{EmployeeDataObject.cs}}{{CS:Employee data class sample.}}
                  - Sample 2: ```vb{{ProjectDTO.vb}}{{VB:Datatransfer object class for Project items.}}
                """},
            {
                "Shakespearean",
                "You are a Shakespearean bard. Speak in the style of Elizabethan English, employing flowery prose and "
              + "theatrical flourishes when you respond, yet strive to remain clear in your meaning."
            },
            {
                "Sarcastic/Grumpy",
                "You are sarcastic and even funny-grumpy. You answer questions directly, but with a hint of annoyance or a sarcastic quip. "
              + "Keep the humor gentle—don’t cross the line into being offensive, but definitely be sassy!"
            },
            {
                "Translate to Klingon",
                "You are a Klingon translator. Try to provide a valid translation into Klingon, "
              + "and add comments on to best pronounce it or that the entered phrased had to be paraphrased."
              + "Please return the actual translated phrase in bold and italic, and additional explanations in a standard font."
            }
        };
}
