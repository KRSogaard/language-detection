using System.Collections.Generic;
using LanguageDetection.Models;

namespace LanguageDetection;

public interface ILanguageDetector
{
    /// <summary>
    ///     Loads all available language profiles into the detector.
    ///     The profiles are embedded as resources in the assembly, and each corresponds to a different language.
    /// </summary>
    void AddAllLanguages();

    /// <summary>
    ///     Loads the specified language profiles into the detector.
    /// </summary>
    /// <param name="languages">The codes of the languages to add, corresponding to the names of the resource files.</param>
    void AddLanguages(params string[] languages);

    /// <summary>
    ///     Detects the most probable language for the provided text.
    ///     The method runs the detection algorithm Trials times and returns the most frequently detected language.
    ///     If no language could be detected with a probability above the ProbabilityThreshold, null is returned.
    /// </summary>
    /// <param name="text">The text to detect the language of. If longer than MaxTextLength, it will be trimmed.</param>
    /// <returns>The code of the most probable language, or null if no language could be reliably detected.</returns>
    string Detect(string text);

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
    IEnumerable<DetectedLanguage> DetectAll(string text);
}