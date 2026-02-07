using BP.Domain.Entities;

namespace BP.Domain.Interfaces;

public interface IMediaEngine
{
    Task LoadAsync(MediaItem mediaItem, CancellationToken cancellationToken = default);
    Task PlayAsync(CancellationToken cancellationToken = default);
    Task PauseAsync(CancellationToken cancellationToken = default);
    Task SeekAsync(TimeSpan position, bool frameAccurate, CancellationToken cancellationToken = default);
    Task SetPlaybackSpeedAsync(double speed, CancellationToken cancellationToken = default);
    Task SetSubtitleTrackAsync(string? trackId, CancellationToken cancellationToken = default);
    Task SetAudioTrackAsync(string? trackId, CancellationToken cancellationToken = default);
}
