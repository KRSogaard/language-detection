namespace LanguageDetection.Tests_new;

public class RealLifeTests
{
    private void RunTest(string language, string test)
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        int guesses = 100;
        int correctGuesses = 0;
        string detectedLanguage;

        for (int i = 0; i < 100; i++)
        {
            detectedLanguage = detector.Detect(test);
            if (language == detectedLanguage)
            {
                correctGuesses++;
            }
        }
        
        Assert.Equal(guesses, correctGuesses);
    }

    [Fact]
    public void TestDanishMaintenanceRequest()
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        string text =
            "Jeg står over for et lidt usædvanligt problem her i min lejlighed. Min sofa synes at have udviklet sin egen vilje - den er begyndt at varme sig selv op! \ud83d\ude32\ud83d\udd25 Hver gang jeg sætter mig for at se tv, bliver jeg gradvist ristet som en skumfidus over et bål. Kan I sende nogen til at tjekke, om der måske er blevet installeret en usynlig pejs under den, eller om sofaen bare har besluttet sig for at starte en karriere som radiator?.";
        RunTest("dan", text);
    }
}