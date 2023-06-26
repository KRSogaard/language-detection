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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace LanguageDetection;

public class LanguageDetector
{
    private const string ResourceNamePrefix = "LanguageDetection.Profiles.";

    private static readonly Regex UrlRegex = new("https?://[-_.?&~;+=/#0-9A-Za-z]{1,2076}", RegexOptions.Compiled);

    private static readonly Regex EmailRegex = new("[-_.0-9A-Za-z]{1,64}@[-_0-9A-Za-z]{1,255}[-_.0-9A-Za-z]{1,255}",
        RegexOptions.Compiled);

    private readonly List<LanguageProfile> languages;
    private readonly Dictionary<string, Dictionary<LanguageProfile, double>> wordLanguageProbabilities;

    public LanguageDetector() {
        AlphaWidth = 0.05;
        MaxIterations = 1000;
        ProbabilityThreshold = 0.1;
        ConvergenceThreshold = 0.99999;
        BaseFrequency = 10000;

        Alpha = 0.5;
        RandomSeed = null;
        Trials = 7;
        NGramLength = 3;
        MaxTextLength = 10000;

        languages = new List<LanguageProfile>();
        wordLanguageProbabilities = new Dictionary<string, Dictionary<LanguageProfile, double>>();
    }

    private double _alpha;
    /// <summary>
    /// Alpha is a parameter of the Simple Good-Turing smoothing algorithm used for language detection.
    /// </summary>
    public double Alpha 
    {
        get { return _alpha; }
        set 
        {
            if (value < 0 || value > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(Alpha), "Alpha must be between 0 and 1.");
            }
            _alpha = value;
        }
    }

    private int? _randomSeed;
    /// <summary>
    /// Seed for the pseudo-random number generator used in the language detection process.
    /// </summary>
    public int? RandomSeed 
    {
        get { return _randomSeed; }
        set 
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(RandomSeed), "RandomSeed must be a non-negative number.");
            }
            _randomSeed = value;
        }
    }

    private int _trials;
    /// <summary>
    /// The number of times the language detection will be run. The final language prediction will be the most frequent result.
    /// </summary>
    public int Trials 
    {
        get { return _trials; }
        set 
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(Trials), "Trials must be a non-negative number.");
            }
            _trials = value;
        }
    }

    private int _nGramLength;
    /// <summary>
    /// The maximum length of the ngrams used for language detection.
    /// </summary>
    public int NGramLength 
    {
        get { return _nGramLength; }
        set 
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(NGramLength), "NGramLength must be greater than 0.");
            }
            _nGramLength = value;
        }
    }

    private int _maxTextLength;
    /// <summary>
    /// The maximum length of the text that will be used for detection. If the text is longer, it will be trimmed.
    /// </summary>
    public int MaxTextLength 
    {
        get { return _maxTextLength; }
        set 
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(MaxTextLength), "MaxTextLength must be greater than 0.");
            }
            _maxTextLength = value;
        }
    }

    private double _alphaWidth;
    /// <summary>
    /// The width of the range around Alpha from which a random alpha is selected for each trial.
    /// </summary>
    public double AlphaWidth 
    {
        get { return _alphaWidth; }
        set 
        {
            if (value < 0 || value > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(AlphaWidth), "AlphaWidth must be between 0 and 1.");
            }
            _alphaWidth = value;
        }
    }

    private int _maxIterations;
    /// <summary>
    /// The maximum number of iterations the algorithm will perform if it has not yet converged.
    /// </summary>
    public int MaxIterations 
    {
        get { return _maxIterations; }
        set 
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(MaxIterations), "MaxIterations must be greater than 0.");
            }
            _maxIterations = value;
        }
    }

    private double _probabilityThreshold;
    /// <summary>
    /// The minimum probability for a language to be included in the results.
    /// </summary>
    public double ProbabilityThreshold 
    {
        get { return _probabilityThreshold; }
        set 
        {
            if (value < 0 || value > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(ProbabilityThreshold), "ProbabilityThreshold must be between 0 and 1.");
            }
            _probabilityThreshold = value;
        }
    }

    private double _convergenceThreshold;
    /// <summary>
    /// The minimum probability increase between two iterations for the algorithm to continue iterating.
    /// </summary>
    public double ConvergenceThreshold 
    {
        get { return _convergenceThreshold; }
        set 
        {
            if (value < 0 || value > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(ConvergenceThreshold), "ConvergenceThreshold must be between 0 and 1.");
            }
            _convergenceThreshold = value;
        }
    }

    private int _baseFrequency;
    /// <summary>
    /// The frequency of the base word (the most common word) in the language profiles.
    /// </summary>
    public int BaseFrequency 
    {
        get { return _baseFrequency; }
        set 
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(BaseFrequency), "BaseFrequency must be greater than 0.");
            }
            _baseFrequency = value;
        }
    }

    /// <summary>
    /// Loads all available language profiles into the detector.
    /// The profiles are embedded as resources in the assembly, and each corresponds to a different language.
    /// </summary>
    public void AddAllLanguages() {
        var languages = GetType().Assembly.GetManifestResourceNames()
            .Where(name => name.StartsWith(ResourceNamePrefix))
            .Select(name => name.Substring(ResourceNamePrefix.Length))
            .ToArray();
        AddLanguages(languages);
    }

    /// <summary>
    /// Loads the specified language profiles into the detector.
    /// </summary>
    /// <param name="languages">The codes of the languages to add, corresponding to the names of the resource files.</param>
    public void AddLanguages(params string[] languages) {
        var assembly = GetType().Assembly;
        if (languages == null || languages.Length == 0)
        {
            throw new ArgumentException("Input languages array cannot be null or empty.");
        }

        foreach (var language in languages) {
            if (string.IsNullOrWhiteSpace(language))
            {
                throw new ArgumentException("Language code cannot be null, empty, or whitespace.");
            }
            
            using (var stream = assembly.GetManifestResourceStream(ResourceNamePrefix + language)) {
                if (stream == null)
                {
                    throw new ArgumentException("The language " + language + " was not found");
                }
                
                using (var sw = new StreamReader(stream)) {
                    var profile = new LanguageProfile();

                    var json = sw.ReadToEnd();
                    var jsonProfile = JsonSerializer.Deserialize<JsonLanguageProfile>(json);

                    profile.Code = jsonProfile.Name;
                    profile.Frequencies = jsonProfile.Freq;
                    profile.WordCount = jsonProfile.NWords;

                    //profile.Load(stream);
                    AddLanguageProfile(profile);
                }
            }
        }
    }

    private void AddLanguageProfile(LanguageProfile profile) {
        languages.Add(profile);

        foreach (var word in profile.Frequencies.Keys) {
            if (!wordLanguageProbabilities.ContainsKey(word))
                wordLanguageProbabilities[word] = new Dictionary<LanguageProfile, double>();

            if (word.Length >= 1 && word.Length <= NGramLength) {
                var prob = (double)profile.Frequencies[word] / profile.WordCount[word.Length - 1];
                wordLanguageProbabilities[word][profile] = prob;
            }
        }
    }

    /// <summary>
    /// Detects the most probable language for the provided text.
    /// The method runs the detection algorithm Trials times and returns the most frequently detected language.
    /// If no language could be detected with a probability above the ProbabilityThreshold, null is returned.
    /// </summary>
    /// <param name="text">The text to detect the language of. If longer than MaxTextLength, it will be trimmed.</param>
    /// <returns>The code of the most probable language, or null if no language could be reliably detected.</returns>
    public string Detect(string text) {
        var language = DetectAll(text).FirstOrDefault();
        return language != null ? language.Language : null;
    }
    
    /// <summary>
    /// Detects all possible languages for the provided text, with their respective probabilities.
    /// The method runs the detection algorithm Trials times and averages the probabilities of each detected language.
    /// Only languages detected with a probability above the ProbabilityThreshold are returned.
    /// </summary>
    /// <param name="text">The text to detect the languages of. If longer than MaxTextLength, it will be trimmed.</param>
    /// <returns>An enumerable of DetectedLanguage objects, each containing the code of a detected language and its average probability.</returns>
    public IEnumerable<DetectedLanguage> DetectAll(string text) {
        if (languages.Count == 0) {
            throw new Exception("No langauges has been added");
        }
        // Validate the input
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentException("Input text cannot be null, empty or consist only of whitespace.");
        }
        
        var ngrams = ExtractNGrams(NormalizeText(text));
        if (ngrams.Count == 0)
            return new DetectedLanguage[0];

        var languageProbabilities = new double[languages.Count];

        var random = RandomSeed != null ? new Random(RandomSeed.Value) : new Random();

        for (var t = 0; t < Trials; t++) {
            var probs = InitializeProbabilities();
            var alpha = Alpha + random.NextDouble() * AlphaWidth;

            for (var i = 0;; i++) {
                var r = random.Next(ngrams.Count);
                UpdateProbabilities(probs, ngrams[r], alpha);

                if (i % 5 == 0)
                    if (NormalizeProbabilities(probs) > ConvergenceThreshold || i >= MaxIterations)
                        break;
            }

            for (var j = 0; j < languageProbabilities.Length; j++)
                languageProbabilities[j] += probs[j] / Trials;
        }

        return SortProbabilities(languageProbabilities);
    }

    private List<string> ExtractNGrams(string text) {
        var list = new List<string>();

        var ngram = new NGram();

        foreach (var c in text) {
            ngram.Add(c);

            for (var n = 1; n <= NGram.N_GRAM; n++) {
                var w = ngram.Get(n);

                if (w != null && wordLanguageProbabilities.ContainsKey(w))
                    list.Add(w);
            }
        }

        return list;
    }

    private class JsonLanguageProfile
    {
        [JsonPropertyName("name")] public string Name { get; set; }

        [JsonPropertyName("freq")] public Dictionary<string, int> Freq { get; set; }

        [JsonPropertyName("n_words")] public int[] NWords { get; set; }
    }

    public class DetectedLanguage
    {
        public string Language { get; set; }
        public double Probability { get; set; }
    }

    private class NGram
    {
        public const int N_GRAM = 3;

        private StringBuilder buffer = new(" ", N_GRAM);
        private bool capital;

        public void Add(char c) {
            var lastChar = buffer[buffer.Length - 1];

            if (lastChar == ' ') {
                buffer = new StringBuilder(" ");
                capital = false;
                if (c == ' ') return;
            }
            else if (buffer.Length >= N_GRAM) {
                buffer.Remove(0, 1);
            }

            buffer.Append(c);

            if (char.IsUpper(c)) {
                if (char.IsUpper(lastChar))
                    capital = true;
            }
            else {
                capital = false;
            }
        }

        public string Get(int n) {
            if (capital)
                return null;

            if (n < 1 || n > N_GRAM || buffer.Length < n)
                return null;

            if (n == 1) {
                var c = buffer[buffer.Length - 1];
                if (c == ' ') return null;
                return c.ToString();
            }

            return buffer.ToString(buffer.Length - n, n);
        }
    }

    #region Normalize text

    protected virtual string NormalizeText(string text) {
        if (text.Length > MaxTextLength)
            text = text.Substring(0, MaxTextLength);

        text = RemoveAddresses(text);
        text = NormalizeAlphabet(text);
        text = NormalizeVietnamese(text);
        text = NormalizeWhitespace(text);

        return text;
    }

    private static string NormalizeAlphabet(string text) {
        var latinCount = 0;
        var nonLatinCount = 0;

        for (var i = 0; i < text.Length; ++i) {
            var c = text[i];

            if (c <= 'z' && c >= 'A')
                ++latinCount;
            else if (c >= '\u0300' && !(c >= 0x1e00 && c <= 0x1eff)) ++nonLatinCount;
        }

        if (latinCount * 2 < nonLatinCount) {
            var textWithoutLatin = new StringBuilder();
            for (var i = 0; i < text.Length; ++i) {
                var c = text[i];
                if (c > 'z' || c < 'A')
                    textWithoutLatin.Append(c);
            }

            text = textWithoutLatin.ToString();
        }

        return text;
    }

    private static string NormalizeVietnamese(string text) {
        // todo
        return text;
    }

    private static string NormalizeWhitespace(string text) {
        var sb = new StringBuilder(text.Length);

        char? prev = null;

        foreach (var c in text) {
            if (c != ' ' || prev != ' ')
                sb.Append(c);
            prev = c;
        }

        return sb.ToString();
    }

    private static string RemoveAddresses(string text) {
        text = UrlRegex.Replace(text, " ");
        text = EmailRegex.Replace(text, " ");
        return text;
    }

    #endregion

    #region Probabilities

    private double[] InitializeProbabilities() {
        var prob = new double[languages.Count];
        for (var i = 0; i < prob.Length; i++)
            prob[i] = 1.0 / languages.Count;
        return prob;
    }

    private void UpdateProbabilities(double[] prob, string word, double alpha) {
        if (word == null || !wordLanguageProbabilities.ContainsKey(word))
            return;

        var languageProbabilities = wordLanguageProbabilities[word];
        var weight = alpha / BaseFrequency;

        for (var i = 0; i < prob.Length; i++) {
            var profile = languages[i];
            prob[i] *= weight + (languageProbabilities.ContainsKey(profile) ? languageProbabilities[profile] : 0);
        }
    }

    private static double NormalizeProbabilities(double[] probs) {
        double maxp = 0, sump = 0;

        for (var i = 0; i < probs.Length; ++i)
            sump += probs[i];

        for (var i = 0; i < probs.Length; ++i) {
            var p = probs[i] / sump;
            if (maxp < p) maxp = p;
            probs[i] = p;
        }

        return maxp;
    }

    private IEnumerable<DetectedLanguage> SortProbabilities(double[] probs) {
        var list = new List<DetectedLanguage>();

        for (var j = 0; j < probs.Length; j++) {
            var p = probs[j];

            if (p > ProbabilityThreshold)
                for (var i = 0; i <= list.Count; i++)
                    if (i == list.Count || list[i].Probability < p) {
                        list.Insert(i, new DetectedLanguage {
                            Language = languages[j].Code,
                            Probability = p
                        });
                        break;
                    }
        }

        return list;
    }

    #endregion
}