namespace BP.Domain.Entities;

public enum SessionEventType
{
    Start,
    Stop,
    Seek,
    Error
}

public sealed record SessionEvent(
    string Id,
    SessionEventType Type,
    string MediaId,
    DateTimeOffset Timestamp,
    string Payload
);
