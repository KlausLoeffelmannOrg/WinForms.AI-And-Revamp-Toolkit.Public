using CommunityToolkit.WinForms.Controls;

namespace Chatty.Views;

public partial class SourceViewer: UserControl
{
    public SourceViewer()
    {
        InitializeComponent();
    }

    public SourceCodeViewer SourceCodeViewer => _sourceCodeViewer;
}
