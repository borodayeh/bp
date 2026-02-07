namespace BP.Domain.Entities;

public sealed record PlaybackState(
    string MediaId,
    TimeSpan Position,
    TimeSpan LastStableKeyframe,
    double Speed,
    string? SubtitleTrack,
    string? AudioTrack,
    DateTimeOffset UpdatedAt
);
