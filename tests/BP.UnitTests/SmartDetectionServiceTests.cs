using BP.Application.Services;

namespace BP.UnitTests;

public class SmartDetectionServiceTests
{
    [Fact]
    public void DetectType_ReturnsMusic_WhenNoVideo()
    {
        var sut = new SmartDetectionService();
        var type = sut.DetectType("track.flac", hasVideo: false);
        Assert.Equal(ContentType.Music, type);
    }
}
