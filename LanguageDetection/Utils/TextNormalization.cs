using System.Text;
using System.Text.RegularExpressions;

namespace LanguageDetection.Utils;

public static class TextNormalization
{
    private static readonly Regex
        UrlRegex = new Regex("https?://[-_.?&~;+=/#0-9A-Za-z]{1,2076}", RegexOptions.Compiled);

    private static readonly Regex EmailRegex =
        new Regex("[-_.0-9A-Za-z]{1,64}@[-_0-9A-Za-z]{1,255}[-_.0-9A-Za-z]{1,255}", RegexOptions.Compiled);

    public static string NormalizeText(string text, LanguageDetectorSettings settings)
    {
        if (text.Length > settings.MaxTextLength)
        {
            text = text.Substring(0, settings.MaxTextLength);
        }

        text = RemoveAddresses(text);
        text = NormalizeAlphabet(text);
        text = NormalizeWhitespace(text);

        return text;
    }

    private static string NormalizeAlphabet(string text)
    {
        int latinCount = 0;
        int nonLatinCount = 0;

        for (int i = 0; i < text.Length; ++i)
        {
            char c = text[i];

            if (c <= 'z' && c >= 'A')
            {
                ++latinCount;
            }
            else if (c >= '\u0300' && !(c >= 0x1e00 && c <= 0x1eff))
            {
                ++nonLatinCount;
            }
        }

        if (latinCount * 2 < nonLatinCount)
        {
            StringBuilder textWithoutLatin = new StringBuilder();
            for (int i = 0; i < text.Length; ++i)
            {
                char c = text[i];
                if (c > 'z' || c < 'A')
                {
                    textWithoutLatin.Append(c);
                }
            }

            text = textWithoutLatin.ToString();
        }

        return text;
    }

    private static string NormalizeWhitespace(string text)
    {
        StringBuilder sb = new StringBuilder(text.Length);

        char? prev = null;

        foreach (char c in text)
        {
            if (c != ' ' || prev != ' ')
            {
                sb.Append(c);
            }

            prev = c;
        }

        return sb.ToString();
    }

    private static string RemoveAddresses(string text)
    {
        text = UrlRegex.Replace(text, " ");
        text = EmailRegex.Replace(text, " ");
        return text;
    }
}