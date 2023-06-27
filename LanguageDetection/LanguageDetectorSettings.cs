using System;

namespace LanguageDetection;

public class LanguageDetectorSettings
{
    private double _alpha;

    private double _alphaWidth;

    private int _baseFrequency;

    private double _convergenceThreshold;

    private int _maxIterations;

    private int _maxTextLength;

    private int _nGramLength;

    private double _probabilityThreshold;

    private int? _randomSeed;

    private int _trials;

    /// <summary>
    ///     Alpha is a parameter of the Simple Good-Turing smoothing algorithm used for language detection.
    /// </summary>
    public double Alpha
    {
        get => _alpha;
        set
        {
            if (value < 0 || value > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(Alpha), "Alpha must be between 0 and 1.");
            }

            _alpha = value;
        }
    }

    /// <summary>
    ///     Seed for the pseudo-random number generator used in the language detection process.
    /// </summary>
    public int? RandomSeed
    {
        get => _randomSeed;
        set
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(RandomSeed), "RandomSeed must be a non-negative number.");
            }

            _randomSeed = value;
        }
    }

    /// <summary>
    ///     The number of times the language detection will be run. The final language prediction will be the most frequent
    ///     result.
    /// </summary>
    public int Trials
    {
        get => _trials;
        set
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(Trials), "Trials must be a non-negative number.");
            }

            _trials = value;
        }
    }

    /// <summary>
    ///     The maximum length of the ngrams used for language detection.
    /// </summary>
    public int NGramLength
    {
        get => _nGramLength;
        set
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(NGramLength), "NGramLength must be greater than 0.");
            }

            _nGramLength = value;
        }
    }

    /// <summary>
    ///     The maximum length of the text that will be used for detection. If the text is longer, it will be trimmed.
    /// </summary>
    public int MaxTextLength
    {
        get => _maxTextLength;
        set
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(MaxTextLength), "MaxTextLength must be greater than 0.");
            }

            _maxTextLength = value;
        }
    }

    /// <summary>
    ///     The width of the range around Alpha from which a random alpha is selected for each trial.
    /// </summary>
    public double AlphaWidth
    {
        get => _alphaWidth;
        set
        {
            if (value < 0 || value > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(AlphaWidth), "AlphaWidth must be between 0 and 1.");
            }

            _alphaWidth = value;
        }
    }

    /// <summary>
    ///     The maximum number of iterations the algorithm will perform if it has not yet converged.
    /// </summary>
    public int MaxIterations
    {
        get => _maxIterations;
        set
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(MaxIterations), "MaxIterations must be greater than 0.");
            }

            _maxIterations = value;
        }
    }

    /// <summary>
    ///     The minimum probability for a language to be included in the results.
    /// </summary>
    public double ProbabilityThreshold
    {
        get => _probabilityThreshold;
        set
        {
            if (value < 0 || value > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(ProbabilityThreshold),
                    "ProbabilityThreshold must be between 0 and 1.");
            }

            _probabilityThreshold = value;
        }
    }

    /// <summary>
    ///     The minimum probability increase between two iterations for the algorithm to continue iterating.
    /// </summary>
    public double ConvergenceThreshold
    {
        get => _convergenceThreshold;
        set
        {
            if (value < 0 || value > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(ConvergenceThreshold),
                    "ConvergenceThreshold must be between 0 and 1.");
            }

            _convergenceThreshold = value;
        }
    }

    /// <summary>
    ///     The frequency of the base word (the most common word) in the language profiles.
    /// </summary>
    public int BaseFrequency
    {
        get => _baseFrequency;
        set
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(BaseFrequency), "BaseFrequency must be greater than 0.");
            }

            _baseFrequency = value;
        }
    }

    public static LanguageDetectorSettings Default()
    {
        return new LanguageDetectorSettings
        {
            AlphaWidth = 0.05,
            MaxIterations = 1000,
            ProbabilityThreshold = 0.1,
            ConvergenceThreshold = 0.99999,
            BaseFrequency = 10000,
            Alpha = 0.5,
            RandomSeed = null,
            Trials = 7,
            NGramLength = 3,
            MaxTextLength = 10000,
        };
    }
}