using System.Collections.Generic;
using System.Text;
using LanguageDetection.Models;

namespace LanguageDetection;

/// <summary>
///     Represents an N-gram model for extracting contiguous sequences of characters from text.
/// </summary>
/// <remarks>
///     An N-gram is a contiguous sequence of N items (characters in this case) within a given text.
///     The NGram class provides methods to add characters to the N-gram buffer and retrieve N-grams
///     of a specified length from the buffer.
/// </remarks>
public class NGram
{
    /// <summary>
    ///     The default value for N-gram length.
    /// </summary>
    public const int N_GRAM = 3;

    private StringBuilder buffer = new StringBuilder(" ", N_GRAM);
    private bool capital;

    /// <summary>
    ///     Adds a character to the N-gram buffer.
    /// </summary>
    /// <param name="c">The character to add.</param>
    public void Add(char c)
    {
        char lastChar = buffer[buffer.Length - 1];

        if (lastChar == ' ')
        {
            buffer = new StringBuilder(" ");
            capital = false;
            if (c == ' ')
            {
                return;
            }
        }
        else if (buffer.Length >= N_GRAM)
        {
            buffer.Remove(0, 1);
        }

        buffer.Append(c);

        if (char.IsUpper(c))
        {
            if (char.IsUpper(lastChar))
            {
                capital = true;
            }
        }
        else
        {
            capital = false;
        }
    }

    /// <summary>
    ///     Gets the N-gram of the specified length from the buffer.
    /// </summary>
    /// <param name="n">The length of the N-gram.</param>
    /// <returns>The N-gram string or null if it cannot be obtained.</returns>
    public string Get(int n)
    {
        if (capital)
        {
            return null;
        }

        if (n < 1 || n > N_GRAM || buffer.Length < n)
        {
            return null;
        }

        if (n == 1)
        {
            char c = buffer[buffer.Length - 1];
            if (c == ' ')
            {
                return null;
            }

            return c.ToString();
        }

        return buffer.ToString(buffer.Length - n, n);
    }

    /// <summary>
    ///     Extracts N-grams from the given text based on the N-gram model and word probabilities.
    /// </summary>
    /// <param name="text">The text to extract N-grams from.</param>
    /// <param name="MaxTextLength">The maximum length of the text.</param>
    /// <param name="wordLanguageProbabilities">The word probabilities based on language profiles.</param>
    /// <returns>A list of extracted N-grams.</returns>
    public static List<string> ExtractNGrams(string text, int MaxTextLength,
        Dictionary<string, Dictionary<LanguageProfile, double>> wordLanguageProbabilities)
    {
        // Preallocate list with maximum possible size.
        List<string> list = new List<string>(MaxTextLength * N_GRAM);

        NGram ngram = new NGram();

        foreach (char c in text)
        {
            ngram.Add(c);

            for (int n = 1; n <= N_GRAM; n++)
            {
                string w = ngram.Get(n);

                // Use TryGetValue to avoid double lookup.
                if (w != null && wordLanguageProbabilities.TryGetValue(w, out _))
                {
                    list.Add(w);
                }
            }
        }

        return list;
    }
}