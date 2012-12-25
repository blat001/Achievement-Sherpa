

namespace NPatternRecognizer.Algorithm.NaiveBayes
{
    using System;
    using NPatternRecognizer.Common;
    using NPatternRecognizer.Interface;

    public class CostSensitiveNaiveBayes : NaiveBayesClassifier
    {
        // costMat[i,j]:
        // the cost of deciding for class i when the true class is j.
        protected double[,] costMat = {   { 0,    1e+6 }, 
                                    { 1, 0 } };
        // namely:  
        //  Correct:        costMat[0,0]=costMat[1,1]=0
        //  False Positive: costMat[0,1]=1e+6
        //  False Negative: costMat[1,0]=1

        // Punish "False Positive" by 1e+6 times cost.

        protected string[] className = new string[2];


        public void CrossValidate(TextClassificationProblem problem, out double correctRatio, out double falsePositive, out double falseNegative)
        {
            ExampleSet v_Set;   // validation set

            //Logging.Info("Retrieving validation set");
            v_Set = problem.ValidationSet;

            int numExample = v_Set.Examples.Count;
            int numCorrect = 0;
            int numFP = 0, numFN = 0;

            //Logging.Info("Cross Validating on validation set");

            foreach (TextExample example in v_Set.Examples)
            {
                ClassificationResult result = new ClassificationResult();
                this.Predict(problem, example, ref result);
                string res = this.MinCostClassName(result);

                if (res == example.Label.Name)
                {
                    numCorrect++;
                }
                else if (res == className[0] && example.Label.Name == className[1])
                {
                    numFP++;
                }
                else if (res == className[1] && example.Label.Name == className[0])
                {
                    numFN++;
                }
            }

            correctRatio = 1.0 * numCorrect / numExample;
            falsePositive = 1.0 * numFP / numExample;
            falseNegative = 1.0 * numFN / numExample;
            Logger.Info("Correct ratio: {0}", correctRatio);
            Logger.Info("False Positive: {0}", falsePositive);
            Logger.Info("False Negative: {0}", falseNegative);
        }

        public override void Predict(TextClassificationProblem problem, TextExample text, ref ClassificationResult result)
        {
            int j = 0, k;   // j is index in categories, while k is index in vocabulary.
            double logv;
            Vocabulary voc;

            voc = problem.TrainingSetVocabulary;

            foreach (Category c in m_CategoryCollection.Collection)
            {
                logv = 0.0; // reset
                logv += Math.Log(m_prob_vj[j]);

                // for all the word (token) in the text
                foreach (string token in text.Tokens.Keys)
                {
                    if (voc.WordBag.ContainsKey(token))
                    {
                        // Get the position of this token
                        //  in m_TotalTrainingSetTokens.
                        k = voc.WordPositionMap[token];
                        // Look up the probability in the table.
                        logv += m_prob_wk_vj_log[k, j];
                    }
                }
                result.CategoryName2LogVMap.Add(c.Name, logv);
                j++;    // next category
            }
            result.Normalize();
        }

        private string MinCostClassName(ClassificationResult result)
        {
            double[] cost = new double[2];

            for (int i = 0; i < 2; i++)
            {
                cost[i] = 0;
                for (int j = 0; j < 2; j++)
                {
                    cost[i] += costMat[i, j] * result.CategoryName2LogVMap[className[j]];
                }
            }

            // find classname with minimized cost
            if (cost[0] <= cost[1])
                return className[0];
            else
                return className[1];
        }

        public CostSensitiveNaiveBayes(CategoryCollection collect)
            : base(collect)
        {
            int i = 0;

            foreach (Category c in collect.Collection)
            {
                className[i++] = c.Name;
            }
        }



    }
}
