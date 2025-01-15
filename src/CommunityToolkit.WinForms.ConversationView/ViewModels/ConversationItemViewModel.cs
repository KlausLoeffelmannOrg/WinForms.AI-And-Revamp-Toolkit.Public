using CommunityToolkit.Mvvm.ComponentModel;
using System.Text.Json;

namespace CommunityToolkit.WinForms.Controls.Blazor;

public partial class ConversationItemViewModel : ObservableObject
{
    public ConversationItemViewModel()
    {
        _dateCreated = DateTime.Now.ToString("f");
    }

    [ObservableProperty]
    private string? _markdownContent;

    [ObservableProperty]
    private string? _htmlContent;

    [ObservableProperty]
    private bool _isResponse;

    [ObservableProperty]
    private string? _backColor;

    [ObservableProperty]
    private string? _foreColor;

    [ObservableProperty]
    private string _dateCreated;

    public override string? ToString() 
        => MarkdownContent ?? HtmlContent ?? base.ToString();

    public void WriteJSon(Utf8JsonWriter writer)
    {
        writer.WriteString(nameof(MarkdownContent), MarkdownContent);
        writer.WriteBoolean(nameof(IsResponse), IsResponse);
        writer.WriteString(nameof(BackColor), BackColor);
        writer.WriteString(nameof(ForeColor), ForeColor);
        writer.WriteString(nameof(DateCreated), DateCreated);
    }

    public static ConversationItemViewModel FromJsonElement(JsonElement jsonElement)
    {
        var conversationItemViewModel = new ConversationItemViewModel();

        conversationItemViewModel.MarkdownContent = jsonElement.GetProperty(nameof(MarkdownContent)).GetString();
        conversationItemViewModel.IsResponse = jsonElement.GetProperty(nameof(IsResponse)).GetBoolean();
        conversationItemViewModel.BackColor = jsonElement.GetProperty(nameof(BackColor)).GetString();
        conversationItemViewModel.ForeColor = jsonElement.GetProperty(nameof(ForeColor)).GetString();
        conversationItemViewModel.DateCreated = jsonElement.GetProperty(nameof(DateCreated)).GetString()!;

        return conversationItemViewModel;
    }
}
