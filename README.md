# Language Detection

Detect the language of a text using naive a Bayesian filter with generated language profiles from Wikipedia abstract
xml, 99% over precision for 51 languages. Original author: Nakatani Shuyo.

.NET Port of [Language Detection Library for Java](https://code.google.com/p/language-detection/)
by [@shuyo](https://github.com/shuyo)
Forked from [TechnikEmpire/language-detection](https://github.com/TechnikEmpire/language-detection)

This package has been updated to C# 11 and .NET 7 and all external dependencies has been removed.
The execution has also been optimized a bit to use threads and other improvements.
The algorithm is now detecting languages with all 51 languages added in `0.5 ms` down from `1.12 ms`.

The LanguageDetector is now threadsafe and you can create a singleton instance of it that can be reused thoughout your application

Feel free to send pull requests to this repo

## The Naive Bayesian filter

The Naive Bayesian filter, which is a classification method based on Bayes' Theorem, works on the principle of
considering each feature to be independent of one another. In the context of language detection, these "features" could
be the frequency of certain words, characters, or n-grams (sequences of n characters) in a text.

Here's a high-level overview of how it works:

Training Phase: During training, the filter uses a set of labeled training data (in this case, the Wikipedia abstract
XML data for each language) to calculate the prior probability of each class (language) and the conditional probability
of each feature (word, character, or n-gram) given each class. The prior probability of a class is the overall
likelihood of that class in the training set, while the conditional probability of a feature given a class is the
likelihood of that feature occurring in instances of that class.

Prediction Phase: To predict the class of a new, unlabeled instance (in this case, a piece of text whose language we
want to detect), the filter first transforms the instance into a feature vector. It then applies Bayes' Theorem to
calculate the posterior probability of each class given this feature vector. The class with the highest posterior
probability is chosen as the prediction.

In the context of this Language Detector, the "naive" assumption of the Naive Bayesian filter—that every feature is
independent of every other feature—is not strictly true, as words and characters in a language are often dependent on
each other. However, despite this assumption, the Naive Bayesian filter often performs well in practice and is
particularly effective for language detection due to its ability to handle many features and its robustness to
irrelevant features.

It's important to note that the accuracy of the Naive Bayesian filter heavily depends on the quality and
representativeness of the training data. The language profiles generated from Wikipedia abstract XML data provide a
broad and diverse sample of language use, contributing to the high precision of this Language Detector.

## Supported Languages

This library provides language detection support for the following languages:

| Language   | ISO 639-2/T Code |
|------------|------------------|
| Afrikaans  | afr              |
| Arabic     | ara              |
| Bengali    | ben              |
| Bulgarian  | bul              |
| Czech      | ces              |
| Danish     | dan              |
| German     | deu              |
| Greek      | ell              |
| English    | eng              |
| Estonian   | est              |
| Persian    | fas              |
| Finnish    | fin              |
| French     | fra              |
| Gujarati   | guj              |
| Hebrew     | heb              |
| Hindi      | hin              |
| Croatian   | hrv              |
| Hungarian  | hun              |
| Indonesian | ind              |
| Italian    | ita              |
| Japanese   | jpn              |
| Kannada    | kan              |
| Korean     | kor              |
| Latvian    | lav              |
| Lithuanian | lit              |
| Malayalam  | mal              |
| Marathi    | mar              |
| Macedonian | mkd              |
| Nepali     | nep              |
| Dutch      | nld              |
| Norwegian  | nor              |
| Punjabi    | pan              |
| Polish     | pol              |
| Portuguese | por              |
| Romanian   | ron              |
| Russian    | rus              |
| Slovak     | slk              |
| Slovenian  | slv              |
| Somali     | som              |
| Spanish    | spa              |
| Albanian   | sqi              |
| Swahili    | swa              |
| Swedish    | swe              |
| Tamil      | tam              |
| Telugu     | tel              |
| Tagalog    | tgl              |
| Thai       | tha              |
| Turkish    | tur              |
| Twi        | twi              |
| Ukrainian  | ukr              |
| Urdu       | urd              |
| Vietnamese | vie              |
| Chinese    | zho              |

These language codes follow the ISO 639-2/T standard. Make sure to use the correct language code when invoking the
language detection methods.

## Install

```bash
dotnet add package LanguageDetection.Ai
```

## Use

```csharp
using LanguageDetection;
```

Load all supported languages

```csharp
LanguageDetector detector = new LanguageDetector();
detector.AddAllLanguages();
Assert.Equal("dan", detector.Detect("Denne tekst er skrevet i dansk"));
```

or a small subset

```csharp
LanguageDetector detector = new LanguageDetector();
detector.AddLanguages("dan", "eng", "swe");
Assert.Equal("dan", detector.Detect("Denne tekst er skrevet i dansk"));
```

You can also change parameters

```csharp
LanguageDetectorSettings settings = new LanguageDetectorSettings() {
    RandomSeed = 1,
    ConvergenceThreshold = 0.9,
    MaxIterations = 50,
};
LanguageDetector detector = new LanguageDetector();
```

## License

Apache 2.0