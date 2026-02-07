using BP.Domain.Entities;

namespace BP.Domain.Interfaces;

public interface IPlaybackStateRepository
{
    Task<PlaybackState?> GetByMediaIdAsync(string mediaId, CancellationToken cancellationToken = default);
    Task UpsertAsync(PlaybackState state, CancellationToken cancellationToken = default);
}
