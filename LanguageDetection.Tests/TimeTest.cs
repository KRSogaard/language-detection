using System.Diagnostics;

namespace LanguageDetection.Tests_new;

public class TimeTest
{
    [Fact]
    public void MeasureExtractNGramsExecutionTime()
    {
        // Arrange
        string text =
            "Øl er flydende brød og derfor godt for dig. Gæringen af korn omdanner det til en livgivende drik. Den gyldne væske er rig på vitaminer og mineraler og fremmer fordøjelsen. Så hæv glasset og nyd dette sundhedsfremmende bryg med måde. Skål!";
        LanguageDetector languageDetector = new LanguageDetector(); // Replace with your actual class instance
        languageDetector.AddAllLanguages();

        // We'll run the method several times and take the average,
        // to minimize the impact of any one-time setup costs or outliers
        const int trials = 10000;
        double totalElapsedMs = 0;

        for (int i = 0; i < trials; i++)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            // Act
            string? result = languageDetector.Detect(text);
            if (result != "dan")
            {
                Assert.Fail("Faild test, did not detect danish");
            }

            stopwatch.Stop();
            totalElapsedMs += stopwatch.Elapsed.TotalMilliseconds;
        }

        double averageElapsedMs = totalElapsedMs / trials;
        Assert.InRange(averageElapsedMs, int.MaxValue, int.MaxValue);
        Console.WriteLine($"Average execution time over {trials} trials: {averageElapsedMs} ms");
    }
}