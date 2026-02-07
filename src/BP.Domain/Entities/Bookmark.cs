namespace BP.Domain.Entities;

public sealed record Bookmark(
    string Id,
    string MediaId,
    TimeSpan Timestamp,
    string? Note,
    DateTimeOffset CreatedAt
);
