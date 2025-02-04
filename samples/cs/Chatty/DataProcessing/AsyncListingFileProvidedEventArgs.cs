using Chatty.DataEntities;

namespace Chatty.DataProcessing;

/// <summary>
///  Provides data for the AsyncListingFileProvided event.
/// </summary>
public class AsyncListingFileProvidedEventArgs(ListingFile listingFile)
    : EventArgs
{
    /// <summary>
    ///  Gets the listing file associated with the event.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when the listing file is null.</exception>
    public ListingFile ListingFile { get; } = listingFile
        ?? throw new ArgumentNullException(nameof(listingFile));

    /// <summary>
    ///  Gets or sets a value indicating whether the 
    ///  event has been handled.
    /// </summary>
    public bool Handled { get; set; }
}
