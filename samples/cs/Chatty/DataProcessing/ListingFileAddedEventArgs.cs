using Chatty.DataEntities;

namespace Chatty.DataProcessing;

public class ListingFileAddedEventArgs(ListingFile listingFile) : EventArgs
{
    public ListingFile ListingFile { get; } = listingFile
        ?? throw new ArgumentNullException(nameof(listingFile));
}
