using BP.Application.Services;
using BP.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BP.App;

public static class Program
{
    public static IServiceProvider BuildServices()
    {
        var services = new ServiceCollection();
        services.AddSingleton<PlaybackOrchestrator>();
        services.AddSingleton<SmartDetectionService>();

        // TODO: Register concrete infrastructure implementations once BP.Infrastructure is wired.
        services.AddSingleton<IMediaEngine, NotImplementedMediaEngine>();
        services.AddSingleton<IPlaybackStateRepository, NotImplementedPlaybackStateRepository>();

        return services.BuildServiceProvider();
    }
}

internal sealed class NotImplementedMediaEngine : IMediaEngine
{
    private static Task NotReady() => throw new NotImplementedException("Media engine bridge is pending BP.Media.Native/BP.Media.Interop implementation.");

    public Task LoadAsync(BP.Domain.Entities.MediaItem mediaItem, CancellationToken cancellationToken = default) => NotReady();
    public Task PlayAsync(CancellationToken cancellationToken = default) => NotReady();
    public Task PauseAsync(CancellationToken cancellationToken = default) => NotReady();
    public Task SeekAsync(TimeSpan position, bool frameAccurate, CancellationToken cancellationToken = default) => NotReady();
    public Task SetPlaybackSpeedAsync(double speed, CancellationToken cancellationToken = default) => NotReady();
    public Task SetSubtitleTrackAsync(string? trackId, CancellationToken cancellationToken = default) => NotReady();
    public Task SetAudioTrackAsync(string? trackId, CancellationToken cancellationToken = default) => NotReady();
}

internal sealed class NotImplementedPlaybackStateRepository : IPlaybackStateRepository
{
    public Task<BP.Domain.Entities.PlaybackState?> GetByMediaIdAsync(string mediaId, CancellationToken cancellationToken = default)
        => Task.FromResult<BP.Domain.Entities.PlaybackState?>(null);

    public Task UpsertAsync(BP.Domain.Entities.PlaybackState state, CancellationToken cancellationToken = default)
        => Task.CompletedTask;
}
