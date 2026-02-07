namespace BP.Domain.Entities;

public sealed record MediaItem(
    string Id,
    string FilePath,
    string FilePathHash,
    string Title,
    TimeSpan Duration,
    string Container,
    string VideoCodec,
    string AudioCodec,
    bool HasVideo,
    bool HasAudio,
    DateTimeOffset AddedAt,
    DateTimeOffset? LastPlayedAt
);
