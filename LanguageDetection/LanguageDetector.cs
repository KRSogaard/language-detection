// Copyright 2014 Pēteris Ņikiforovs
// Copyright 2014 Nakatani Shuyo
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using LanguageDetection.Utils;

namespace LanguageDetection;

public class LanguageDetector
{
    private const string ResourceNamePrefix = "LanguageDetection.Profiles.";

    private readonly List<LanguageProfile> languages = new List<LanguageProfile>();
    private readonly LanguageDetectorSettings settings = LanguageDetectorSettings.Default();

    private readonly Dictionary<string, Dictionary<LanguageProfile, double>> wordLanguageProbabilities =
        new Dictionary<string, Dictionary<LanguageProfile, double>>();

    public LanguageDetector()
    { }

    public LanguageDetector(LanguageDetectorSettings settings)
    {
        this.settings = settings;
    }

    /// <summary>
    ///     Loads all available language profiles into the detector.
    ///     The profiles are embedded as resources in the assembly, and each corresponds to a different language.
    /// </summary>
    public void AddAllLanguages()
    {
        string[] languages = GetType().Assembly.GetManifestResourceNames()
            .Where(name => name.StartsWith(ResourceNamePrefix))
            .Select(name => name.Substring(ResourceNamePrefix.Length))
            .ToArray();
        AddLanguages(languages);
    }

    /// <summary>
    ///     Loads the specified language profiles into the detector.
    /// </summary>
    /// <param name="languages">The codes of the languages to add, corresponding to the names of the resource files.</param>
    public void AddLanguages(params string[] languages)
    {
        Assembly assembly = GetType().Assembly;
        if (languages == null || languages.Length == 0)
        {
            throw new ArgumentException("Input languages array cannot be null or empty.");
        }

        foreach (string language in languages)
        {
            if (string.IsNullOrWhiteSpace(language))
            {
                throw new ArgumentException("Language code cannot be null, empty, or whitespace.");
            }

            using (Stream stream = assembly.GetManifestResourceStream(ResourceNamePrefix + language))
            {
                if (stream == null)
                {
                    throw new ArgumentException("The language " + language + " was not found");
                }

                using (StreamReader sw = new StreamReader(stream))
                {
                    LanguageProfile profile = new LanguageProfile();

                    string json = sw.ReadToEnd();
                    JsonLanguageProfile jsonProfile = JsonSerializer.Deserialize<JsonLanguageProfile>(json);

                    profile.Code = jsonProfile.Name;
                    profile.Frequencies = jsonProfile.Freq;
                    profile.WordCount = jsonProfile.NWords;

                    //profile.Load(stream);
                    AddLanguageProfile(profile);
                }
            }
        }
    }

    private void AddLanguageProfile(LanguageProfile profile)
    {
        languages.Add(profile);

        foreach (string word in profile.Frequencies.Keys)
        {
            if (!wordLanguageProbabilities.ContainsKey(word))
            {
                wordLanguageProbabilities[word] = new Dictionary<LanguageProfile, double>();
            }

            if (word.Length >= 1 && word.Length <= settings.NGramLength)
            {
                double prob = (double)profile.Frequencies[word] / profile.WordCount[word.Length - 1];
                wordLanguageProbabilities[word][profile] = prob;
            }
        }
    }

    /// <summary>
    ///     Detects the most probable language for the provided text.
    ///     The method runs the detection algorithm Trials times and returns the most frequently detected language.
    ///     If no language could be detected with a probability above the ProbabilityThreshold, null is returned.
    /// </summary>
    /// <param name="text">The text to detect the language of. If longer than MaxTextLength, it will be trimmed.</param>
    /// <returns>The code of the most probable language, or null if no language could be reliably detected.</returns>
    public string Detect(string text)
    {
        DetectedLanguage language = DetectAll(text).FirstOrDefault();
        return language != null ? language.Language : null;
    }

    /// <summary>
    ///     Detects all possible languages for the provided text, with their respective probabilities.
    ///     The method runs the detection algorithm Trials times and averages the probabilities of each detected language.
    ///     Only languages detected with a probability above the ProbabilityThreshold are returned.
    /// </summary>
    /// <param name="text">The text to detect the languages of. If longer than MaxTextLength, it will be trimmed.</param>
    /// <returns>
    ///     An enumerable of DetectedLanguage objects, each containing the code of a detected language and its average
    ///     probability.
    /// </returns>
    public IEnumerable<DetectedLanguage> DetectAll(string text)
    {
        if (languages.Count == 0)
        {
            throw new Exception("No langauges has been added");
        }

        // Validate the input
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentException("Input text cannot be null, empty or consist only of whitespace.");
        }

        List<string> ngrams = NGram.ExtractNGrams(TextNormalization.NormalizeText(text, settings), settings.MaxTextLength, wordLanguageProbabilities);
        if (ngrams.Count == 0)
        {
            return new DetectedLanguage[0];
        }

        ConcurrentDictionary<int, double> languageProbabilities = new ConcurrentDictionary<int, double>();

        Parallel.For(0, settings.Trials, t =>
        {
            Random random = settings.RandomSeed != null ? new Random(settings.RandomSeed.Value) : new Random();
            
            double[] probs = ProbabilitiesUtils.InitializeProbabilities(languages.Count());
            double alpha = settings.Alpha + random.NextDouble() * settings.AlphaWidth;

            for (int i = 0; i <= settings.MaxIterations; i++)
            {
                // We randomly select n-grams (Monte Carlo sampling) instead of sequential looping for three reasons:
                // 1) Enhances efficiency with large texts, 2) Handles unpredictability in multi-language regions,
                // 3) Aids in converging on most likely languages using an iterative probabilistic approach.
                int r = random.Next(ngrams.Count);
                ProbabilitiesUtils.UpdateProbabilities(probs, ngrams[r], alpha, settings.BaseFrequency, languages,
                    wordLanguageProbabilities);

                if (i % 5 == 0 && ProbabilitiesUtils.NormalizeProbabilities(probs) > settings.ConvergenceThreshold)
                {
                    break;
                }
            }

            for (int j = 0; j < languages.Count; j++)
            {
                double value = probs[j] / settings.Trials;
                languageProbabilities.AddOrUpdate(j, value, (key, oldValue) => oldValue + value);
            }
        });

        return ProbabilitiesUtils.SortProbabilities(languageProbabilities, settings.ProbabilityThreshold, languages);
    }
}