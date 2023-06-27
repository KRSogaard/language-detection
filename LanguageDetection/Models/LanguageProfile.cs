using System.Collections.Generic;

namespace LanguageDetection.Models;

public class LanguageProfile
{
    public string Code { get; set; }
    public Dictionary<string, int> Frequencies { get; set; }
    public int[] WordCount { get; set; }
}