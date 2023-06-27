namespace LanguageDetection.Tests_new;

/// <summary>
///     Translations made by ChatGPT
/// </summary>
public class LanguageDetectorLanguageTests
{
    private void RunTest(string language, string test)
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        int guesses = 100;
        int correctGuesses = 0;
        string detectedLanguage;
        
        for (int i = 0; i < 100; i++) {
            detectedLanguage = detector.Detect(test);
            if (language == detectedLanguage)
            {
                correctGuesses++;
            }
        };
        Assert.Equal(guesses, correctGuesses);
    }

    [Fact]
    public void TestAfrikaans() // Good morning, how are you?
    {
        RunTest("afr", "Goeie môre, hoe gaan dit met jou?");
    }

    [Fact]
    public void TestArabic() // Hello, what's your name?
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        RunTest("ara", "مرحبا، ما اسمك؟");
    }

    [Fact]
    public void TestBengali() // I am very happy today.
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        RunTest("ben", "আমি আজ খুব খুশি।");
    }

    [Fact]
    public void
        TestBulgarian() // Today the sun is shining brightly, I am enjoying the beautiful nature and the fresh air.
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        RunTest("bul", "Днес слънцето грее ярко, аз се наслаждавам на прекрасната природа и свежия въздух.");
    }

    [Fact]
    public void TestCzech() // Where is the nearest station?
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        RunTest("ces", "Kde je nejbližší stanice?");
    }

    [Fact]
    public void TestDanish() // I am learning a new language, that is good to use in Copenhagen
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        RunTest("dan", "Jeg lærer et nyt sprog, der er godt at bruge i København.");
    }

    [Fact]
    public void TestGerman() // My favorite food is pizza.
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        RunTest("deu", "Mein Lieblingsessen ist Pizza.");
    }

    [Fact]
    public void TestGreek() // I love reading books.
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        RunTest("ell", "Μου αρέσει να διαβάζω βιβλία.");
    }

    [Fact]
    public void TestEnglish() // This is an English phrase.
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        RunTest("eng", "This is an English phrase.");
    }

    [Fact]
    public void TestEstonian() // It is a beautiful day.
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        RunTest("est", "See on ilus päev.");
    }

    [Fact]
    public void TestPersian() // The sky is blue.
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        RunTest("fas", "آسمان آبی است.");
    }

    [Fact]
    public void TestFinnish() // My cat is small.
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        RunTest("fin", "Kissaani on pieni.");
    }

    [Fact]
    public void TestFrench() // The black cat walks silently in the garden at night.
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        RunTest("fra", "Le chat noir se promène silencieusement dans le jardin la nuit.");
    }

    [Fact]
    public void TestGujarati() // I have two brothers.
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        RunTest("guj", "મારી પાસે બે ભાઈ છે.");
    }

    [Fact]
    public void TestHebrew() // I live in Jerusalem.
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        RunTest("heb", "אני גר בירושלים.");
    }

    [Fact]
    public void TestHindi() // My name is John.
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        RunTest("hin", "मेरा नाम जॉन है।");
    }

    [Fact]
    public void TestCroatian() // This is a beautiful city.
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        RunTest("hrv", "Ovo je prekrasan grad.");
    }

    [Fact]
    public void TestHungarian() // I am eating an apple.
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        RunTest("hun", "Almát eszek.");
    }

    [Fact]
    public void TestIndonesian() // I love to travel.
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        RunTest("ind", "Saya suka bepergian.");
    }

    [Fact]
    public void TestItalian() // The sea is very beautiful.
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        RunTest("ita", "Il mare è molto bello.");
    }

    [Fact]
    public void TestJapanese() // Nice to meet you.
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        RunTest("jpn", "はじめまして。");
    }

    [Fact]
    public void TestKannada() // I study every day.
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        RunTest("kan", "ನಾನು ಪ್ರತಿ ದಿನ ಅಧ್ಯಯನ ಮಾಡುತ್ತೇನೆ.");
    }

    [Fact]
    public void TestKorean() // I like coffee.
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        RunTest("kor", "나는 커피를 좋아합니다.");
    }

    [Fact]
    public void TestLatvian() // Spring is coming.
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        RunTest("lav", "Pavasaris nāk.");
    }

    [Fact]
    public void TestLithuanian() // It's a sunny day.
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        RunTest("lit", "Tai saulėtas diena.");
    }

    [Fact]
    public void TestMalayalam() // Rain is beautiful.
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        RunTest("mal", "മഴ സുന്ദരമാണ്.");
    }

    [Fact]
    public void TestMarathi() // I am learning Marathi.
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        RunTest("mar", "माझे मराठी शिकत आहे.");
    }

    [Fact]
    public void TestMacedonian() // This is a good book.
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        RunTest("mkd", "Ова е добра книга.");
    }

    [Fact]
    public void TestNepali() // I am a student.
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        RunTest("nep", "म विद्यार्थी हुँ।");
    }

    [Fact]
    public void TestDutch() // I have a dog.
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        RunTest("nld", "Ik heb een hond.");
    }

    [Fact]
    public void TestNorwegian() // I like to play football.
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        RunTest("nor", "Jeg liker å spille fotball.");
    }

    [Fact]
    public void TestPunjabi() // My mother is a teacher.
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        RunTest("pan", "ਮੇਰੀ ਮਾਂ ਇੱਕ ਅਧਿਆਪਕ ਹੈ।");
    }

    [Fact]
    public void TestPolish() // I love to cook.
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        RunTest("pol", "Uwielbiam gotować.");
    }

    [Fact]
    public void TestPortuguese() // The beach is beautiful.
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        RunTest("por", "A praia é linda.");
    }

    [Fact]
    public void TestRomanian() // It's a warm day.
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        RunTest("ron", "Este o zi caldă.");
    }

    [Fact]
    public void TestRussian() // My friend lives in Moscow.
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        RunTest("rus", "Мой друг живет в Москве.");
    }

    [Fact]
    public void
        TestSlovak() // The weather today is beautiful, the sun is shining, and the sky is clear, an ideal day for a walk in nature.
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        RunTest("slk", "Počasie dnes je krásne, slnko svieti a obloha je jasná, ideálny deň na prechádzku v prírode.");
    }

    [Fact]
    public void TestSlovenian() // I am from Slovenia.
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        RunTest("slv", "Sem iz Slovenije.");
    }

    [Fact]
    public void TestSomali() // I have three children.
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        RunTest("som", "Waxaan leeyahay saddex carruur.");
    }

    [Fact]
    public void TestSpanish() // I love reading books.
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        RunTest("spa", "Me encanta leer libros.");
    }

    [Fact]
    public void TestAlbanian() // This is a new car.
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        RunTest("sqi", "Ky është një makinë e re.");
    }

    [Fact]
    public void TestSwedish() // The food is delicious.
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        RunTest("swe", "Maten är läcker.");
    }

    [Fact]
    public void TestTamil() // I like to write.
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        RunTest("tam", "எனக்கு எழுதுவது பிடிக்கும்.");
    }

    [Fact]
    public void TestTelugu() // I am going home.
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        RunTest("tel", "నేను ఇంటికి వెళ్తున్నాను.");
    }

    [Fact]
    public void TestTagalog() // I am from the Philippines.
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        RunTest("tgl", "Galing ako sa Pilipinas.");
    }

    [Fact]
    public void TestThai() // I am a teacher.
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        RunTest("tha", "ฉันเป็นครู");
    }

    [Fact]
    public void TestTurkish() // I love Istanbul.
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        RunTest("tur", "İstanbul'u seviyorum.");
    }

    [Fact]
    public void TestUkrainian() // The snow is white.
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        RunTest("ukr", "Сніг білий.");
    }

    [Fact]
    public void TestUrdu() // I am studying.
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        RunTest("urd", "میں پڑھ رہا ہوں۔");
    }

    [Fact]
    public void TestVietnamese() // Today is Sunday.
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        RunTest("vie", "Hôm nay là Chủ Nhật.");
    }

    [Fact]
    public void TestChinese() // I like tea.
    {
        LanguageDetector detector = new LanguageDetector();
        detector.AddAllLanguages();
        RunTest("zho", "我喜欢喝茶。");
    }
}