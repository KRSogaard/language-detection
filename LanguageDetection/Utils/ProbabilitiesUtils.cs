using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace LanguageDetection.Utils;

public static class ProbabilitiesUtils
{
    public static double[] InitializeProbabilities(int languagesCount)
    {
        double[] prob = new double[languagesCount];
        for (int i = 0; i < prob.Length; i++)
        {
            prob[i] = 1.0 / languagesCount;
        }

        return prob;
    }

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

    public static IEnumerable<DetectedLanguage> SortProbabilities(ConcurrentDictionary<int, double> probs,
        double ProbabilityThreshold, List<LanguageProfile> languages)
    {
        List<DetectedLanguage> list = new List<DetectedLanguage>();

        for (int j = 0; j < probs.Count; j++)
        {
            if (languages[j].Code == "dan")
            {
                Console.WriteLine("Here");
            }

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