using BP.Domain.Entities;

namespace BP.Domain.Interfaces;

public interface ILibraryScanner
{
    IAsyncEnumerable<MediaItem> ScanAsync(IEnumerable<string> roots, CancellationToken cancellationToken = default);
}
