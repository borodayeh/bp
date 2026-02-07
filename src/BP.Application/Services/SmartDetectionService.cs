namespace BP.Application.Services;

public enum ContentType
{
    Unknown,
    Movie,
    Series,
    Music,
    Tutorial
}

public sealed class SmartDetectionService
{
    public ContentType DetectType(string fileName, bool hasVideo)
    {
        var normalized = fileName.ToLowerInvariant();

        if (!hasVideo)
        {
            return ContentType.Music;
        }

        if (normalized.Contains("s01e") || normalized.Contains("episode"))
        {
            return ContentType.Series;
        }

        if (normalized.Contains("tutorial") || normalized.Contains("lesson") || normalized.Contains("course"))
        {
            return ContentType.Tutorial;
        }

        return ContentType.Movie;
    }
}
