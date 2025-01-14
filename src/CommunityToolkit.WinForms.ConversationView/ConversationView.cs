using CommunityToolkit.WinForms.ConversationView.Components;
using Markdig;
using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using Microsoft.Web.WebView2.Core;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace CommunityToolkit.WinForms.Controls.Blazor;

/// <summary>
///  Represents a custom conversation view control that extends 
///  the BlazorWebView class.
/// </summary>
public class ConversationView : BlazorWebView
{
    private readonly IServiceProvider? _serviceProvider;

    [AllowNull]
    private ConversationViewModel _viewModel;
    private readonly Conversation _conversation = new();

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

        WebView.CoreWebView2InitializationCompleted += WebView_CoreWebView2InitializationCompleted;
        WebView.NavigationCompleted += WebView_NavigationCompleted;
        WebView.NavigationStarting += WebView_NavigationStarting;
    }

    private void WebView_NavigationStarting(object? sender, CoreWebView2NavigationStartingEventArgs e)
    {
        Debug.Print($"Navigation starting - URI:{e.Uri}");
    }

    private void CoreWebView2_DOMContentLoaded(object? sender, CoreWebView2DOMContentLoadedEventArgs e)
        => Debug.Print($"DOM content loaded - ID:{e.NavigationId}");

    private void WebView_NavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
        => Debug.Print($"Navigation completed - success:{e.IsSuccess}");

    private void WebView_CoreWebView2InitializationCompleted(object? sender, CoreWebView2InitializationCompletedEventArgs e)
    {
        WebView.CoreWebView2.DOMContentLoaded += CoreWebView2_DOMContentLoaded;
        WebView.CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;
        WebView.CoreWebView2.WebResourceRequested += CoreWebView2_WebResourceRequested;
    }

    private void CoreWebView2_WebResourceRequested(object? sender, CoreWebView2WebResourceRequestedEventArgs e)
    {
        Debug.Print($"Web resource requested: {e.Request.Uri}");
    }

    private void CoreWebView2_WebMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
    {
        Debug.Print($"Web message received: {e.TryGetWebMessageAsString()}");
    }

    /// <summary>
    ///  Gets the conversation associated with the view.
    /// </summary>
    public Conversation Conversation => _conversation;

    /// <summary>
    ///  Adds a conversation item to the conversation view.
    /// </summary>
    /// <param name="text">The text of the conversation item.</param>
    /// <param name="isResponse">A flag indicating whether the conversation item is a response.</param>
    public void AddConversationItem(string text, bool isResponse)
    {
        var item = new ConversationItemViewModel
        {
            BackColor = SystemColors.ControlLight.ToWebColor(),
            ForeColor = ForeColor.ToWebColor(),
            HtmlContent = $"<p>{text}<p>",
            IsResponse = isResponse
        };

        _viewModel?.ConversationItems.Add(item);
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
        string builtUpHtmlParagraphs = string.Empty;

        // Iterate through the responses asynchronously and add them to the conversation view model
        await foreach (var response in asyncEnumerable)
        {
            currentParagraph.Append(response);

            // Convert Markdown to HTML using Markdig
            string currentHTML = Markdown.ToHtml(currentParagraph.ToString());

            // Test, if the response ends with any sort of LineFeed/CR:
            if (response.EndsWith('\n') || response.EndsWith('\r'))
            {
                Debug.Print($"Next Paragraph: {currentParagraph}");
                builtUpHtmlParagraphs += currentHTML;
                currentParagraph.Clear();
                currentHTML = string.Empty;
            }

            _viewModel.ResponseInProgress = builtUpHtmlParagraphs + currentHTML;

            yield return response;
        }

        _viewModel.ConversationItems.Add(new ConversationItemViewModel
        {
            BackColor = SystemColors.ControlLight.ToWebColor(),
            ForeColor = ForeColor.ToWebColor(),
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
    ///  Updates the title of the conversation.
    /// </summary>
    /// <param name="title">The new title of the conversation.</param>
    public void UpdateConversationTitle(string title)
    {
        _conversation.Title = title;
        _viewModel.Headline = title;
    }

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

        _viewModel = new ConversationViewModel(
            headline: $"Conversation on {DateTime.Now:f}",
            backColor: SystemColors.ControlLightLight.ToWebColor(),
            foreColor: SystemColors.ControlText.ToWebColor(),
            newItemsBackColor: SystemColors.ControlLightLight.ToWebColor());

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
}

/// <summary>
///  Represents a conversation.
/// </summary>
public class Conversation
{
    /// <summary>
    ///  Gets or sets the title of the conversation.
    /// </summary>
    public string? Title { get; set; }
}
