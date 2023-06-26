# Language Detection Library

This is a robust .NET 7 language detection library, a port of the original [Language Detection Library for Java](https://code.google.com/p/language-detection/). With the capability of detecting languages in given texts, it boasts a precision of over 99% for 51 languages. The detection algorithm is rooted in the principles of a Naive Bayesian filter and uses language profiles generated from Wikipedia abstract xml data.

This .NET version is forked from [TechnikEmpire/language-detection](https://github.com/TechnikEmpire/language-detection), and has been updated to C# 11, with all external dependencies removed.

Contributions are welcome. Feel free to send pull requests to this repo.

## The Naive Bayesian Filter Explained

The Naive Bayesian filter is a classification method based on Bayes' Theorem, operating under the assumption that each feature is independent. In language detection, these "features" are the frequency of certain words, characters, or n-grams (sequences of n characters) in a text.

The filter works in two main phases:

- **Training Phase:** The filter uses labeled training data (Wikipedia abstract XML data for each language) to compute the prior probability of each class (language) and the conditional probability of each feature given each class. These calculations reflect the likelihood of a class in the training set and the likelihood of a feature occurring in instances of that class, respectively.

- **Prediction Phase:** To detect the language of a new, unlabeled text, the filter transforms the text into a feature vector and applies Bayes' Theorem to calculate the posterior probability of each class given this vector. The class (language) with the highest posterior probability becomes the prediction.

Though the "naive" assumption—that every feature is independent—may not strictly hold, the Naive Bayesian filter performs well due to its capacity to handle numerous features and its robustness to irrelevant features.

Please note, the precision of the filter depends heavily on the quality and representativeness of the training data. This library uses language profiles from Wikipedia abstract XML data, ensuring a broad and diverse sample of language use.

## Installation

To install, simply add a reference to `LanguageDetection.dll` in your project.

## Usage

Import the Language Detection library:
```csharp
using LanguageDetection;
```

To load all supported languages:
```csharp
LanguageDetector detector = new LanguageDetector();
detector.AddAllLanguages();
Assert.Equal("dan", detector.Detect("Denne tekst er skrevet i dansk"));
```

Or load a small subset of languages:
```csharp
LanguageDetector detector = new LanguageDetector();
detector.AddLanguages("dan", "eng", "swe");
Assert.Equal("dan", detector.Detect("Denne tekst er skrevet i dansk"));
```

It's also possible to modify certain parameters:
```csharp
LanguageDetector detector = new LanguageDetector();
detector.RandomSeed = 1;
detector.ConvergenceThreshold = 0.9;
detector.MaxIterations = 50;
```

## License

This library is licensed under Apache 2.0.
