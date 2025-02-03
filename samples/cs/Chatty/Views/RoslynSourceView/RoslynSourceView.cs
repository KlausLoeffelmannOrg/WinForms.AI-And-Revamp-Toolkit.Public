using Chatty.DataEntities;
using CommunityToolkit.WinForms.Controls;
using System.ComponentModel;

namespace Chatty.Views;

internal partial class RoslynSourceView : UserControl
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RoslynSourceView"/> class.
    /// </summary>
    public RoslynSourceView()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Gets the listing file.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ListingFile? ListingFile { get; private set; }

    /// <summary>
    /// Gets the source code viewer.
    /// </summary>
    public SourceCodeViewer SourceCodeViewer => _sourceCodeViewer;

    /// <summary>
    /// Saves the listing file asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no listing file is set.</exception>
    public async Task SaveFileAsync(string basePath)
    {
        if (string.IsNullOrEmpty(ListingFile?.FileName))
        {
            throw new InvalidOperationException("No listing file set.");
        }

        string filename = Path.Combine(basePath, ListingFile.FileName)
            ?? throw new InvalidOperationException("No filename set.");

        // Let's save the file:
        await File.WriteAllTextAsync(filename, ListingFile.Content);
    }

    /// <summary>
    /// Sets the listing file asynchronously.
    /// </summary>
    /// <param name="listingFile">The listing file to set.</param>
    /// <returns>A task that represents the asynchronous set operation.</returns>
    public async Task SetListingFileAsync(ListingFile listingFile)
    {
        // Let's save the file:
        ListingFile = listingFile;

        _lblListingTitel.Text = listingFile.ListingTitle;

        await SourceCodeViewer.SetSourceCodeAsync(
            sourceCode: listingFile.Content,
            filename: listingFile.FileName);
    }
}
