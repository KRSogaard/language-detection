using System.Text;
using System.Text.RegularExpressions;

namespace LanguageDetection.Utils;

/// <summary>
/// Provides text normalization methods for language detection.
/// </summary>
public static class TextNormalization
{
    private static readonly Regex
        UrlRegex = new Regex("https?://[-_.?&~;+=/#0-9A-Za-z]{1,2076}", RegexOptions.Compiled);

    private static readonly Regex EmailRegex =
        new Regex("[-_.0-9A-Za-z]{1,64}@[-_0-9A-Za-z]{1,255}[-_.0-9A-Za-z]{1,255}", RegexOptions.Compiled);

    /// <summary>
    /// Normalizes the given text based on the specified language detection settings.
    /// </summary>
    /// <param name="text">The text to be normalized.</param>
    /// <param name="settings">The language detector settings.</param>
    /// <returns>The normalized text.</returns>
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

    /// <summary>
    /// Normalizes the alphabet of the given text by removing non-Latin characters if they dominate the text.
    /// </summary>
    /// <param name="text">The text to normalize.</param>
    /// <returns>The normalized text.</returns>
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

    /// <summary>
    /// Normalizes the whitespace in the given text by removing consecutive spaces.
    /// </summary>
    /// <param name="text">The text to normalize.</param>
    /// <returns>The normalized text.</returns>
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

    /// <summary>
    /// Removes URL addresses and email addresses from the given text.
    /// </summary>
    /// <param name="text">The text to remove addresses from.</param>
    /// <returns>The text with addresses removed.</returns>
    private static string RemoveAddresses(string text)
    {
        text = UrlRegex.Replace(text, " ");
        text = EmailRegex.Replace(text, " ");
        return text;
    }
}