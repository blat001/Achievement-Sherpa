
namespace NPatternRecognizer.Algorithm.NaiveBayes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NPatternRecognizer.Interface;

    public class TextClassificationProblem : ClassificationProblem
    {
        public Vocabulary Vocabulary { get; set; }
        public Vocabulary TrainingSetVocabulary { get; set; }
    }
}
