using Chatty.DataEntities;
using CommunityToolkit.WinForms.Controls;

namespace Chatty.Views;

public partial class SourceViewer: UserControl
{
    public SourceViewer()
    {
        InitializeComponent();
    }

    public SourceCodeViewer SourceCodeViewer => _sourceCodeViewer;

    internal async Task SetListingFileAsync(ListingFile listingFile)
    {
        await SourceCodeViewer.SetSourceCodeAsync(
            sourceCode: listingFile.Content,
            filename: listingFile.FileName);
    }
}
