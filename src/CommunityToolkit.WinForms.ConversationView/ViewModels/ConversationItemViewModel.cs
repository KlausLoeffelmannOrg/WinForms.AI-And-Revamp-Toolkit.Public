using CommunityToolkit.Mvvm.ComponentModel;

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
}
