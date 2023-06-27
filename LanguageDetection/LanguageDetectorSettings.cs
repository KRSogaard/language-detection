using System;

namespace LanguageDetection;

public class LanguageDetectorSettings
{
    private double alpha;

    private double alphaWidth;

    private int baseFrequency;

    private double convergenceThreshold;

    private int maxIterations;

    private int maxTextLength;

    private int nGramLength;

    private double probabilityThreshold;

    private int? randomSeed;

    private int trials;

    /// <summary>
    ///     Alpha is a parameter of the Simple Good-Turing smoothing algorithm used for language detection.
    /// </summary>
    public double Alpha
    {
        get => alpha;
        set
        {
            if (value < 0 || value > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(Alpha), "Alpha must be between 0 and 1.");
            }

            alpha = value;
        }
    }

    /// <summary>
    ///     Seed for the pseudo-random number generator used in the language detection process.
    /// </summary>
    public int? RandomSeed
    {
        get => randomSeed;
        set
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(RandomSeed), "RandomSeed must be a non-negative number.");
            }

            randomSeed = value;
        }
    }

    /// <summary>
    ///     The number of times the language detection will be run. The final language prediction will be the most frequent
    ///     result.
    /// </summary>
    public int Trials
    {
        get => trials;
        set
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(Trials), "Trials must be a non-negative number.");
            }

            trials = value;
        }
    }

    /// <summary>
    ///     The maximum length of the ngrams used for language detection.
    /// </summary>
    public int NGramLength
    {
        get => nGramLength;
        set
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(NGramLength), "NGramLength must be greater than 0.");
            }

            nGramLength = value;
        }
    }

    /// <summary>
    ///     The maximum length of the text that will be used for detection. If the text is longer, it will be trimmed.
    /// </summary>
    public int MaxTextLength
    {
        get => maxTextLength;
        set
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(MaxTextLength), "MaxTextLength must be greater than 0.");
            }

            maxTextLength = value;
        }
    }

    /// <summary>
    ///     The width of the range around Alpha from which a random alpha is selected for each trial.
    /// </summary>
    public double AlphaWidth
    {
        get => alphaWidth;
        set
        {
            if (value < 0 || value > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(AlphaWidth), "AlphaWidth must be between 0 and 1.");
            }

            alphaWidth = value;
        }
    }

    /// <summary>
    ///     The maximum number of iterations the algorithm will perform if it has not yet converged.
    /// </summary>
    public int MaxIterations
    {
        get => maxIterations;
        set
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(MaxIterations), "MaxIterations must be greater than 0.");
            }

            maxIterations = value;
        }
    }

    /// <summary>
    ///     The minimum probability for a language to be included in the results.
    /// </summary>
    public double ProbabilityThreshold
    {
        get => probabilityThreshold;
        set
        {
            if (value < 0 || value > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(ProbabilityThreshold),
                    "ProbabilityThreshold must be between 0 and 1.");
            }

            probabilityThreshold = value;
        }
    }

    /// <summary>
    ///     The minimum probability increase between two iterations for the algorithm to continue iterating.
    /// </summary>
    public double ConvergenceThreshold
    {
        get => convergenceThreshold;
        set
        {
            if (value < 0 || value > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(ConvergenceThreshold),
                    "ConvergenceThreshold must be between 0 and 1.");
            }

            convergenceThreshold = value;
        }
    }

    /// <summary>
    ///     The frequency of the base word (the most common word) in the language profiles.
    /// </summary>
    public int BaseFrequency
    {
        get => baseFrequency;
        set
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(BaseFrequency), "BaseFrequency must be greater than 0.");
            }

            baseFrequency = value;
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