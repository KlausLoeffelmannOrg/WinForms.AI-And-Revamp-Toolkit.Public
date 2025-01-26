using CommunityToolkit.WinForms.ConversationView.Components;
using CommunityToolkit.WinForms.DebuggingAids;
using Markdig;
using Markdig.Helpers;
using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace CommunityToolkit.WinForms.Controls.Blazor;

/// <summary>
///  Represents a custom conversation view control that extends 
///  the BlazorWebView class.
/// </summary>
public partial class ConversationView : BlazorWebView
{
    public event EventHandler<ConversationTitleChangedEventArgs>? ConversationTitleChanged;
    public event EventHandler<ConversationItemAddedEventArgs>? ConversationItemAdded;
    public event EventHandler<ReceivedNextParagraphEventArgs>? ReceivedNextParagraph;
    public event EventHandler<ReceivedMetaDataEventArgs>? ReceivedMetaData;

    private Conversation _conversation;

    private readonly IServiceProvider? _serviceProvider;
    private readonly HexAsciiDumper _hexDumper = new();

    /// <summary>
    ///  Initializes a new instance of the ConversationView class.
    /// </summary>
    public ConversationView()
    {
        _serviceProvider = this.GetServiceProvider();

        if (_serviceProvider is not null)
        {
            Services = _serviceProvider;
        }

        _conversation = new Conversation()
        {
            Id = Guid.NewGuid(),
            BackColor = SystemColors.ControlLightLight.ToWebColor(),
            ForeColor = SystemColors.ControlText.ToWebColor(),
            Title = $"New conversation {DateTime.Now: ddd mm, HH:mm}"
        };

        _conversation.PropertyChanged += ViewModel_PropertyChanged;
        _conversation.ConversationItems.CollectionChanged += ConversationItems_CollectionChanged;

        WebView.CoreWebView2InitializationCompleted += WebView_CoreWebView2InitializationCompleted;
        WebView.NavigationCompleted += WebView_NavigationCompleted;
        WebView.NavigationStarting += WebView_NavigationStarting;
    }

    /// <summary>
    ///  Gets or sets the unique identifier for the conversation.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Conversation Conversation
    {
        get => _conversation;
        set
        {
            if (object.Equals(_conversation, value))
            {
                return;
            }

            // Fast-fail if the value is null
            ArgumentNullException.ThrowIfNull(value);

            // We do not replacing the conversation, but updating it.
            _conversation.ConversationItems.Clear();

            // Let's clone the classes content first:
            _conversation.Id = value.Id;
            _conversation.Title = value.Title;
            _conversation.DateCreated = value.DateCreated;
            _conversation.DateLastEdited = value.DateLastEdited;
            _conversation.Summary = value.Summary;
            _conversation.Keywords = value.Keywords;
            _conversation.TokenCount = value.TokenCount;
            _conversation.Model = value.Model;
            _conversation.Personality = value.Personality;

            // Now we add the conversation items:
            foreach (var item in value.ConversationItems)
            {
                _conversation.ConversationItems.Add(item);
            }
        }
    }

    /// <summary>
    ///  Adds a conversation item to the conversation view.
    /// </summary>
    /// <param name="text">The text content of the conversation item.</param>
    /// <param name="isResponse">Indicates whether the item is a response.</param>
    /// <returns>The added conversation item.</returns>
    public ConversationItem AddConversationItem(string text, bool isResponse)
    {
        var item = new ConversationItem
        {
            BackColor = SystemColors.ControlLight.ToWebColor(),
            ForeColor = ForeColor.ToWebColor(),
            HtmlContent = $"<p>{text}</p>",
            MarkdownContent = text,
            IsResponse = isResponse
        };

        _conversation?.ConversationItems.Add(item);

        return item;
    }

    /// <summary>
    ///  Clears the conversation history.
    /// </summary>
    public void ClearHistory()
        => _conversation?.ConversationItems.Clear();

    /// <summary>
    ///  Converts the conversation items to a JSON string.
    /// </summary>
    /// <returns>A JSON string representing the conversation items.</returns>
    public string ToJson()
    {
        try
        {
            using MemoryStream stream = new();
            _conversation.WriteJSon(stream);

            return Encoding.UTF8.GetString(stream.ToArray());
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async IAsyncEnumerable<string> UpdateCurrentResponseAsync(
        IAsyncEnumerable<string> asyncEnumerable)
    {
        if (_conversation is null)
        {
            yield break;
        }

        StringBuilder currentParagraph = new();
        StringBuilder builtUpHtmlParagraphs = new();
        StringBuilder builtUpMarkdown = new();
        StringBuilder wordBuilder = new();
        StringBuilder metaDataBuilder = new();

        int positionCounter = 0;
        char lastChar = '\0';
        bool buildingMetaData = false;
        bool metaDataPhaseInOrOut = false;

        // Iterate responses as characters
        await foreach (char c in ConvertToCharStreamAsync(asyncEnumerable))
        {
            if (!buildingMetaData)
            {
                if (c == '{' && lastChar != '{')
                {
                    // We've hit a metadata block and need to start parsing.
                    // We're not doing anything yet, but we're setting a flag.
                    metaDataPhaseInOrOut = true;
                    lastChar = c;
                    continue;
                }

                if (metaDataPhaseInOrOut && c != '{')
                {
                    // false alarm.
                    // We're not in a metadata block.
                    metaDataPhaseInOrOut = false;
                    positionCounter += 2;
                    wordBuilder.Append($"{{{c}");

                    lastChar = c;
                    continue;
                }

                if (c == '{' && lastChar == '{')
                {
                    // We've hit a metadata block and need to start parsing
                    // until we find the end.
                    metaDataPhaseInOrOut = false;
                    metaDataBuilder.Clear();
                    buildingMetaData = true;

                    lastChar = c;
                    continue;
                }
            }

            if (buildingMetaData)
            {
                if (c == '}' && lastChar != '}')
                {
                    // We've hit a metadata end-block and need maybe to stop parsing.
                    // We're not doing anything yet, but we're setting a flag.
                    metaDataPhaseInOrOut = true;

                    lastChar = c;
                    continue;
                }

                if (metaDataPhaseInOrOut && c != '}')
                {
                    // false alarm.
                    // We're not in a metadata block.
                    metaDataPhaseInOrOut = false;
                    positionCounter += 2;
                    wordBuilder.Append($"}}{c}");

                    lastChar = c;
                    continue;
                }

                if (c == '}' && lastChar == '}')
                {
                    // We've hit a metadata block and need to start parsing
                    // until we find the end.
                    metaDataPhaseInOrOut = false;

                    OnReceivedMetaData(new ReceivedMetaDataEventArgs(
                        metaDataBuilder.ToString(),
                        positionCounter));

                    metaDataBuilder.Clear();
                    buildingMetaData = false;

                    lastChar = c;
                    continue;
                }

                metaDataBuilder.Append(c);
                lastChar = c;
                continue;
            }

            positionCounter++;
            wordBuilder.Append(c);

            // If we've hit a word boundary, yield the word
            if (!IsWordSeparator(c) || wordBuilder.Length <= 0)
            {
                continue;
            }

            yield return ProcessWord(wordBuilder.ToString(), c);

            wordBuilder.Clear();
            lastChar = c;
        }

        // Flush any leftover word
        if (wordBuilder.Length > 0)
        {
            yield return ProcessWord(wordBuilder.ToString(), null);
            wordBuilder.Clear();
        }

        // Add the conversation item
        _conversation.ConversationItems.Add(
            new ConversationItem
            {
                BackColor = SystemColors.ControlLight.ToWebColor(),
                ForeColor = ForeColor.ToWebColor(),
                MarkdownContent = builtUpMarkdown.ToString(),
                HtmlContent = $"<p>{_conversation.ResponseInProgress}</p>",
                IsResponse = true
            });

        _conversation.ResponseInProgress = string.Empty;
        builtUpHtmlParagraphs.Clear();
        builtUpMarkdown.Clear();

        /// <summary>
        ///  Processes a single word, updating paragraph/HTML/markdown buffers.
        /// </summary>
        string ProcessWord(string word, char? lastChar)
        {
            currentParagraph.Append(word);

            // Convert the accumulated text to HTML
            string localMarkdown = currentParagraph.ToString();
            string localHtml = Markdown.ToHtml(localMarkdown);

            // If we ended with a newline, treat it as a paragraph boundary
            if (lastChar.HasValue && lastChar.Value.IsNewLineOrLineFeed())
            {
                Debug.Print($"Next Paragraph: {localMarkdown}");
                OnReceivedNextParagraph(
                    new ReceivedNextParagraphEventArgs(
                        paragraph: localMarkdown,
                        textPosition: positionCounter));

                builtUpHtmlParagraphs.Append(localHtml);
                builtUpMarkdown.Append(localMarkdown);

                currentParagraph.Clear();
                localMarkdown = string.Empty;
                localHtml = string.Empty;
            }

            _conversation.ResponseInProgress = builtUpHtmlParagraphs.ToString() + localHtml;

            return word;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool IsWordSeparator(char c)
            => c switch
            {
                ' ' => true,
                '\n' => true,
                '\r' => true,
                _ => false
            };

        static async IAsyncEnumerable<char> ConvertToCharStreamAsync(IAsyncEnumerable<string> asyncEnumerable)
        {
            await foreach (var response in asyncEnumerable)
            {
                foreach (char c in response)
                {
                    yield return c;
                }
            }
        }
    }

    private void ConversationItems_CollectionChanged(
        object? sender,
        NotifyCollectionChangedEventArgs e)
    {
        if (ConversationItemAdded is not null
            && e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add
            && e.NewItems?.Count == 1
            && e.NewItems[0] is ConversationItem conversationItem)
        {
            OnConversationItemAdded(new ConversationItemAddedEventArgs(conversationItem));
        }

        _conversation.DateLastEdited = DateTimeOffset.Now;
    }

    private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Conversation.Title))
        {
            OnConversationTitleChanged(new ConversationTitleChangedEventArgs(_conversation.Title));
        }

        if (e.PropertyName != nameof(Conversation.DateLastEdited))
        {
            _conversation.DateLastEdited = DateTimeOffset.Now;
        }
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        UpdateRootComponent();

    }

    private void UpdateRootComponent()
    {
        if (IsAncestorSiteInDesignMode)
        {
            return;
        }

        // Create new dictionary of parameters for the component
        Dictionary<string, object?> parameters = new()
            {
                { nameof(ConversationRenderer.ViewModel), _conversation },
                { nameof(ConversationRenderer.BackColor), SystemColors.ControlDark.ToWebColor() }
            };

        RootComponent component = new(
            selector: "#app",
            componentType: typeof(ConversationRenderer),
            parameters: parameters);

        RootComponents.Clear();
        RootComponents.Add(component);
        HostPage = $"wwwroot/index.html";
    }

    /// <summary>
    /// Raises the <see cref="ConversationTitleChanged"/> event.
    /// </summary>
    /// <param name="conversationTitleChangedEventArgs">The event data.</param>
    protected virtual void OnConversationTitleChanged(ConversationTitleChangedEventArgs conversationTitleChangedEventArgs)
        => ConversationTitleChanged?.Invoke(this, conversationTitleChangedEventArgs);

    /// <summary>
    /// Raises the <see cref="ReceivedNextParagraph"/> event.
    /// </summary>
    /// <param name="receivedNextParagraphEventArgs">The event data.</param>
    protected virtual void OnReceivedNextParagraph(ReceivedNextParagraphEventArgs receivedNextParagraphEventArgs)
        => ReceivedNextParagraph?.Invoke(this, receivedNextParagraphEventArgs);

    /// <summary>
    /// Paints the background of the control.
    /// </summary>
    /// <param name="e">A <see cref="PaintEventArgs"/> that contains the event data.</param>
    protected override void OnPaintBackground(PaintEventArgs e)
    {
        base.OnPaintBackground(e);
        e.Graphics.Clear(SystemColors.ControlLightLight);
    }

    /// <summary>
    /// Raises the <see cref="ConversationItemAdded"/> event.
    /// </summary>
    /// <param name="conversationItemAddedEventArgs">The event data.</param>
    protected virtual void OnConversationItemAdded(ConversationItemAddedEventArgs conversationItemAddedEventArgs)
        => ConversationItemAdded?.Invoke(this, conversationItemAddedEventArgs);

    /// <summary>
    /// Raises the <see cref="ReceivedMetaData"/> event.
    /// </summary>
    /// <param name="receivedMetaDataEventArgs">The event data.</param>
    protected virtual void OnReceivedMetaData(ReceivedMetaDataEventArgs receivedMetaDataEventArgs)
        => ReceivedMetaData?.Invoke(this, receivedMetaDataEventArgs);
}
