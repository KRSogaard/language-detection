using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using LanguageDetection.Models;

namespace LanguageDetection.Utils;

public static class ProbabilitiesUtils
{
    /// <summary>
    /// Initializes an array of probabilities for each language.
    /// </summary>
    /// <param name="languagesCount">The number of languages.</param>
    /// <returns>An array of initialized probabilities.</returns>
    public static double[] InitializeProbabilities(int languagesCount)
    {
        double[] prob = new double[languagesCount];
        for (int i = 0; i < prob.Length; i++)
        {
            prob[i] = 1.0 / languagesCount;
        }

        return prob;
    }

    /// <summary>
    /// Updates the probabilities based on the given word, alpha, base frequency, language profiles, and word-language probabilities.
    /// </summary>
    /// <param name="prob">The array of probabilities to be updated.</param>
    /// <param name="word">The word to update probabilities for.</param>
    /// <param name="alpha">The alpha value for weight calculation.</param>
    /// <param name="BaseFrequency">The base frequency value.</param>
    /// <param name="languages">The list of language profiles.</param>
    /// <param name="wordLanguageProbabilities">The dictionary containing word-language probabilities.</param>

    public static void UpdateProbabilities(double[] prob, string word, double alpha, int BaseFrequency,
        List<LanguageProfile> languages,
        Dictionary<string, Dictionary<LanguageProfile, double>> wordLanguageProbabilities)
    {
        if (word == null || !wordLanguageProbabilities.ContainsKey(word))
        {
            return;
        }

        Dictionary<LanguageProfile, double> languageProbabilities = wordLanguageProbabilities[word];
        double weight = alpha / BaseFrequency;

        for (int i = 0; i < prob.Length; i++)
        {
            LanguageProfile profile = languages[i];

            if (!languageProbabilities.TryGetValue(profile, out double profileProb))
            {
                profileProb = 0;
            }

            prob[i] *= weight + profileProb;
        }
    }

    /// <summary>
    /// Normalizes the probabilities array and returns the maximum probability.
    /// </summary>
    /// <param name="probs">The array of probabilities to be normalized.</param>
    /// <returns>The maximum probability.</returns>

    public static double NormalizeProbabilities(double[] probs)
    {
        double maxp = 0, sump = 0;

        for (int i = 0; i < probs.Length; ++i)
        {
            sump += probs[i];
        }

        for (int i = 0; i < probs.Length; ++i)
        {
            double p = probs[i] / sump;
            if (maxp < p)
            {
                maxp = p;
            }

            probs[i] = p;
        }

        return maxp;
    }

    /// <summary>
    /// Sorts the probabilities and returns a list of detected languages based on the given probability threshold.
    /// </summary>
    /// <param name="probs">The concurrent dictionary of probabilities.</param>
    /// <param name="ProbabilityThreshold">The probability threshold for language detection.</param>
    /// <param name="languages">The list of language profiles.</param>
    /// <returns>A sorted list of detected languages.</returns>
    public static IEnumerable<DetectedLanguage> SortProbabilities(ConcurrentDictionary<int, double> probs,
        double ProbabilityThreshold, List<LanguageProfile> languages)
    {
        List<DetectedLanguage> list = new List<DetectedLanguage>();

        for (int j = 0; j < probs.Count; j++)
        {
            double p = probs[j];

            if (p > ProbabilityThreshold)
            {
                for (int i = 0; i <= list.Count; i++)
                {
                    if (i == list.Count || list[i].Probability < p)
                    {
                        list.Insert(i, new DetectedLanguage
                        {
                            Language = languages[j].Code,
                            Probability = p,
                        });
                        break;
                    }
                }
            }
        }

        return list;
    }
}