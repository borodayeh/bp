using BP.Domain.Entities;
using BP.Domain.Interfaces;

namespace BP.Application.Services;

public sealed class PlaybackOrchestrator
{
    private readonly IMediaEngine _mediaEngine;
    private readonly IPlaybackStateRepository _playbackStateRepository;

    public PlaybackOrchestrator(IMediaEngine mediaEngine, IPlaybackStateRepository playbackStateRepository)
    {
        _mediaEngine = mediaEngine;
        _playbackStateRepository = playbackStateRepository;
    }

    public async Task StartAsync(MediaItem mediaItem, CancellationToken cancellationToken = default)
    {
        await _mediaEngine.LoadAsync(mediaItem, cancellationToken);

        var resumeState = await _playbackStateRepository.GetByMediaIdAsync(mediaItem.Id, cancellationToken);
        if (resumeState is not null)
        {
            await _mediaEngine.SeekAsync(resumeState.Position, frameAccurate: true, cancellationToken);
            await _mediaEngine.SetPlaybackSpeedAsync(resumeState.Speed, cancellationToken);
            await _mediaEngine.SetSubtitleTrackAsync(resumeState.SubtitleTrack, cancellationToken);
            await _mediaEngine.SetAudioTrackAsync(resumeState.AudioTrack, cancellationToken);
        }

        await _mediaEngine.PlayAsync(cancellationToken);
    }

    public Task PauseAsync(CancellationToken cancellationToken = default) => _mediaEngine.PauseAsync(cancellationToken);
}
