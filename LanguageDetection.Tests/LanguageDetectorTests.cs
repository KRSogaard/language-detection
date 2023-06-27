namespace LanguageDetection.Tests_new;

public class LanguageDetectorTests
{
    [Fact]
    public void TestLanguageHasBeenAdded()
    {
        LanguageDetector detector = new LanguageDetector();
        Assert.Throws<Exception>(() => detector.Detect("Dette sprog er dansk"));
        detector.AddLanguages("dan");
        detector.Detect("Dette sprog er dansk");
    }

    [Fact]
    public void TestOnlyOneLanguageIsReturned()
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddLanguages("eng", "dan");
        IEnumerable<DetectedLanguage> result = detector.DetectAll("Dette sprog er dansk");
        Assert.Single(result);
    }

    [Fact]
    public void TestNotAbleToAddBadLanguage()
    {
        LanguageDetector detector = new LanguageDetector();
        Assert.Throws<ArgumentException>(() => detector.AddLanguages("xxx"));
    }
}