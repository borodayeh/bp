namespace BP.Domain.Entities;

public sealed record Profile(
    string Id,
    string Name,
    double NormalizationTargetLufs,
    bool DialogueEnhancementEnabled,
    double BassGainDb,
    DateTimeOffset CreatedAt
);
