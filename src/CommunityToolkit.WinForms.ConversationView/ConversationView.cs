using CommunityToolkit.WinForms.ConversationView.Components;
using Markdig;
using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using System.ComponentModel;
using System.Diagnostics;
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

    private readonly IServiceProvider? _serviceProvider;
    private ConversationViewModel _viewModel;

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

        _viewModel = new ConversationViewModel()
        {
            BackColor = SystemColors.ControlLightLight.ToWebColor(),
            ForeColor = SystemColors.ControlText.ToWebColor(),
            Title = "New"
        };

        _viewModel.PropertyChanged += ViewModel_PropertyChanged;
        _viewModel.ConversationItems.CollectionChanged += ConversationItems_CollectionChanged;

        WebView.CoreWebView2InitializationCompleted += WebView_CoreWebView2InitializationCompleted;
        WebView.NavigationCompleted += WebView_NavigationCompleted;
        WebView.NavigationStarting += WebView_NavigationStarting;
    }

    private void ConversationItems_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (ConversationItemAdded is not null
            && e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add
            && e.NewItems?.Count == 1
            && e.NewItems[0] is ConversationItemViewModel conversationItem)
        {
            ConversationItemAdded.Invoke(this, new ConversationItemAddedEventArgs(conversationItem));
        }
    }

    private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ConversationViewModel.Title))
        {
            OnConversationTitleChanged(new ConversationTitleChangedEventArgs(_viewModel.Title));
        }
    }

    protected virtual void OnConversationTitleChanged(ConversationTitleChangedEventArgs conversationTitleChangedEventArgs) 
        => ConversationTitleChanged?.Invoke(this, conversationTitleChangedEventArgs);

    [DefaultValue("")]
    public string? ConversationTitle
    {
        get => _viewModel.Title;
        set
        {
            if (_viewModel is not null)
            {
                _viewModel.Title = value!;
            }
        }
    }

    public IEnumerable<ConversationItemViewModel> ConversationItems 
        => _viewModel.ConversationItems;

    /// <summary>
    ///  Adds a conversation item to the conversation view.
    /// </summary>
    /// <param name="text">The text of the conversation item.</param>
    /// <param name="isResponse">A flag indicating whether the conversation item is a response.</param>
    public ConversationItemViewModel AddConversationItem(string text, bool isResponse)
    {
        var item = new ConversationItemViewModel
        {
            BackColor = SystemColors.ControlLight.ToWebColor(),
            ForeColor = ForeColor.ToWebColor(),
            HtmlContent = $"<p>{text}<p>",
            MarkdownContent = text,
            IsResponse = isResponse
        };

        _viewModel?.ConversationItems.Add(item);

        return item;
    }

    /// <summary>
    ///  Updates the current response asynchronously.
    /// </summary>
    /// <param name="asyncEnumerable">The async enumerable that provides the responses.</param>
    /// <returns>An async enumerable of the responses.</returns>
    public async IAsyncEnumerable<string> UpdateCurrentResponseAsync(IAsyncEnumerable<string> asyncEnumerable)
    {
        if (_viewModel is null)
        {
            yield break;
        }

        StringBuilder currentParagraph = new();
        StringBuilder builtUpHtmlParagraphs = new();
        StringBuilder builtUpMarkdown = new();

        string currentMarkdown = string.Empty;

        // Iterate through the responses asynchronously and add them to the conversation view model
        await foreach (var response in asyncEnumerable)
        {
            currentParagraph.Append(response);
            currentMarkdown = currentParagraph.ToString();

            // Convert Markdown to HTML using Markdig
            string currentHTML = Markdown.ToHtml(currentMarkdown);

            // Test, if the response ends with any sort of LineFeed/CR:
            if (response.EndsWith('\n') || response.EndsWith('\r'))
            {
                Debug.Print($"Next Paragraph: {currentMarkdown}");

                builtUpHtmlParagraphs.Append(currentHTML);
                builtUpMarkdown.Append(currentMarkdown);

                currentParagraph.Clear();
                currentMarkdown = string.Empty;
                currentHTML = string.Empty;
            }

            _viewModel.ResponseInProgress = builtUpHtmlParagraphs + currentHTML;

            yield return response;
        }

        _viewModel.ConversationItems.Add(new ConversationItemViewModel
        {
            BackColor = SystemColors.ControlLight.ToWebColor(),
            ForeColor = ForeColor.ToWebColor(),
            MarkdownContent = $"{builtUpMarkdown}",
            HtmlContent = $"<p>{_viewModel.ResponseInProgress}<p>",
            IsResponse = true
        });

        _viewModel.ResponseInProgress = string.Empty;
    }

    /// <summary>
    ///  Clears the conversation history.
    /// </summary>
    public void ClearHistory()
        => _viewModel?.ConversationItems.Clear();

    /// <summary>
    ///  Paints the background of the control.
    /// </summary>
    /// <param name="e">A PaintEventArgs that contains the event data.</param>
    protected override void OnPaintBackground(PaintEventArgs e)
    {
        base.OnPaintBackground(e);
        e.Graphics.Clear(SystemColors.ControlLightLight);
    }

    /// <summary>
    ///  Raises the HandleCreated event.
    /// </summary>
    /// <param name="e">An EventArgs that contains the event data.</param>
    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);

        // Create new dictionary of parameters for the component
        Dictionary<string, object?> parameters = new()
            {
                { nameof(ConversationRenderer.ViewModel), _viewModel },
                { nameof(ConversationRenderer.BackColor), SystemColors.ControlDark.ToWebColor() }
            };

        var component = new RootComponent(
            selector: "#app",
            componentType: typeof(ConversationRenderer),
            parameters: parameters);

        RootComponents.Add(component);

        // Let's pass the system theme to the Blazor component:
        string systemMode = Application.IsDarkModeEnabled ? "dark" : "light";
        HostPage = $"wwwroot/index.html";
    }

    public string ToJson()
    {
        try
        {
            using MemoryStream stream = new();
            _viewModel.WriteJSon(stream);

            return Encoding.UTF8.GetString(stream.ToArray());
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void FromJson(string json)
    {
        _viewModel.ConversationItems.Clear();

        try
        {
            using MemoryStream stream = new(Encoding.UTF8.GetBytes(json));
            var tempModel = ConversationViewModel.FromJSon(stream);

            // Let's clone the model and the items,
            // so the Blazor view will get the changes and update.
            _viewModel.Title = tempModel.Title;
            _viewModel.BackColor = tempModel.BackColor;
            _viewModel.ForeColor = tempModel.ForeColor;

            foreach (var item in tempModel.ConversationItems)
            {
                string currentHTML = Markdown.ToHtml(item.MarkdownContent!);
                item.HtmlContent = $"<p>{currentHTML}<p>";
                _viewModel.ConversationItems.Add(item);
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
}
