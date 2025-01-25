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

    private readonly IServiceProvider? _serviceProvider;
    private readonly Conversation _conversation;
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
    public Conversation Conversation => _conversation;

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
    ///  Loads conversation items from a JSON string.
    /// </summary>
    /// <param name="json">The JSON string representing the conversation items.</param>
    public void FromJson(string json)
    {
        _conversation.ConversationItems.Clear();

        try
        {
            using MemoryStream stream = new(Encoding.UTF8.GetBytes(json));
            var tempModel = Conversation.FromJSon(stream);

            // Let's clone the model and the items,
            // so the Blazor view will get the changes and update.
            _conversation.Title = tempModel.Title;

            foreach (var item in tempModel.ConversationItems)
            {
                string currentHTML = Markdown.ToHtml(item.MarkdownContent!);
                item.HtmlContent = $"<p>{currentHTML}</p>";
                item.ForeColor = ForeColor.ToWebColor();
                item.BackColor = SystemColors.ControlLight.ToWebColor();

                _conversation.ConversationItems.Add(item);
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);

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

        RootComponents.Add(component);
        HostPage = $"wwwroot/index.html";
    }

    protected virtual void OnConversationTitleChanged(ConversationTitleChangedEventArgs conversationTitleChangedEventArgs)
    => ConversationTitleChanged?.Invoke(this, conversationTitleChangedEventArgs);

    protected virtual void OnReceivedNextParagraph(ReceivedNextParagraphEventArgs receivedNextParagraphEventArgs)
        => ReceivedNextParagraph?.Invoke(this, receivedNextParagraphEventArgs);

    protected override void OnPaintBackground(PaintEventArgs e)
    {
        base.OnPaintBackground(e);
        e.Graphics.Clear(SystemColors.ControlLightLight);
    }

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

        // Iterate responses as characters
        await foreach (char c in ConvertToCharStreamAsync(asyncEnumerable))
        {
            wordBuilder.Append(c);

            // If we've hit a word boundary, yield the word
            if (!IsWordSeparator(c) || wordBuilder.Length <= 0)
            {
                continue;
            }

            yield return ProcessWord(wordBuilder.ToString(), c);

            wordBuilder.Clear();
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
                OnReceivedNextParagraph(new ReceivedNextParagraphEventArgs(localMarkdown));

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
            ConversationItemAdded.Invoke(this, new ConversationItemAddedEventArgs(conversationItem));
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
}
