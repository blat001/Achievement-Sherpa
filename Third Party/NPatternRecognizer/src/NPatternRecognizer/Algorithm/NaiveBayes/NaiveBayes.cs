

namespace NPatternRecognizer.Algorithm.NaiveBayes
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization.Formatters.Binary;
    using NPatternRecognizer.Interface;
    using NPatternRecognizer.Common;

    /// <summary>
    /// 
    /// Naive Bayes Algorithm:
    /// ======================
    /// 
    /// Notation:
    ///     
    ///     V = {v1,...,vc}, all possible classes.
    ///     Vocabulary = {w1,...,wn}
    /// 
    /// Using Bayes' theorem, we write
    /// 
    ///     P(vj|w1,...,wn) = P(vj) * P(w1,...,wn|vj) / P(w1,...,wn)
    /// 
    /// Naive Bayes assumes:
    /// 
    ///     P(w1,...,wn|vj) = Mk P(wk|vj)
    ///     Mk P(wk|vj) = P(w1|vj) * ... * P(wn|vj)
    /// 
    ///     (For all a!=b, wa,wb conditionally independent assumption)
    /// 
    /// Maximum a Posteriori (MAP) estimate:
    /// 
    ///     classify(,...,) = argmax (P(vj) * Mk P(wk|vj))
    /// 
    /// P(vj) is easy to calculate, P(wk|vj) is not that easy. 
    /// For avoiding 0-occurence dominate, we use m-estimate:
    /// 
    ///     P = (nc+mp)/(n+m)
    /// 
    /// p is prior, p = 1/|Vocabulary|
    /// 
    /// When m = |Vocabulary|, we have
    /// 
    ///     P(wk|vj) = (nc+1)/(n+|Vocabulary|)
    /// 
    /// 
    /// Disadvantage:
    /// 
    /// Naive Bayes Algorithm: as a classical statistical inference is based on 
    /// the very strict assumption that probability distribution models or 
    /// probability-density functions are known.
    /// 
    /// Classical statistical inference is based on the
    ///     following three fundamental assumptions:
    /// 
    ///     -Data can be modeled by a set of linear in parameter
    ///         functions; this is a foundation of a parametric paradigm in
    ///         learning from experimental data.
    /// 
    ///     -In the most of real-life problems, a stochastic component of
    ///         data is the normal probability distribution law, i.e., the
    ///         underlying joint probability distribution is Gaussian.
    /// 
    ///     -Due to the second assumption, the induction paradigm for
    ///         parameter estimation is the maximum likelihood method that
    ///         is reduced to the minimization of the sum-of-errors-squares
    ///         cost function in most engineering applications.
    /// 
    /// </summary>
    public class NaiveBayesClassifier
    {
        #region Fields
        protected double[] m_prob_vj;
        //private double[,] m_prob_wk_vj;
        //use log to avoid double overflow.
        protected double[,] m_prob_wk_vj_log;

        //Vocabulary m_Vocabulary;
        protected CategoryCollection m_CategoryCollection;

        #endregion

        #region Methods

        public void Train(TextClassificationProblem problem)
        {
            this.CalculateProbabilities(problem);
        }

        public double CrossValidate(TextClassificationProblem problem)
        {
            ExampleSet v_Set;   // validation set

            //Logging.Info("Retrieving validation set");
            v_Set = problem.ValidationSet;

            int numExample = v_Set.Examples.Count;
            int numCorrect = 0;

            //Logging.Info("Cross Validating on validation set");

            foreach (TextExample example in v_Set.Examples)
            {
                ClassificationResult result = new ClassificationResult();
                this.Predict(problem, example, ref result);
                if (result.Vnb == example.Label.Name)
                {
                    numCorrect++;
                }
            }

            double correctRatio = 1.0 * numCorrect / numExample;
            Logger.Info(string.Format("Correct ratio: {0}", correctRatio));

            return correctRatio;
        }

        /// <summary>
        /// Calculate P(vj) and P(wk|vj)
        /// 
        /// In Total:
        /// Variables   Num
        /// P(vj)       c
        /// P(wk|vj)    n*c
        /// 
        /// </summary>
        private void CalculateProbabilities(TextClassificationProblem problem)
        {
            ExampleSet t_Set;   // training set
            int numCategory;
            CategoryCollection categoryCollection;
            Vocabulary voc;

            //Logging.Info("Retrieving vocabulary");
            voc = problem.TrainingSetVocabulary;
            //Logging.Info("Retrieving training set");
            t_Set = problem.TrainingSet;
            numCategory = problem.CategoryCount;
            categoryCollection = problem.CategoryCollection;

            //Logging.Info("Calculating probabilities");

            // Step1: Calculate Probabilities
            //
            int numVocabulary = voc.Count;
            m_prob_vj = new double[numCategory];
            m_prob_wk_vj_log = new double[numVocabulary, numCategory];


            // Step2: P(vj)
            //
            for (int i = 0; i < numCategory; i++)
            {
                m_prob_vj[i] = 1.0 / numCategory;
            }

            // Step3: P(wk|vj)
            //

            NaiveBayesCategoryCollection collection = new NaiveBayesCategoryCollection(categoryCollection);
            foreach (Example example in t_Set.Examples)
            {
                collection.AddExample((TextExample)example);
            }

            //  P(wk|vj) = (nc+1)/(n+|Vocabulary|)
            //
            //  nc: the occurence of wk in the n positions.
            //  n:  word position numbers for category vj.
            int nc, n;
            //
            // k: index in vacabulary;
            // j: index in categories

            int k = 0, j = 0;

            foreach (NaiveBayesCategory c in collection.CategorySet)
            {
                k = 0;  // reset

                foreach (string word in voc.WordBag.Keys)
                {
                    if (c.WordBag.ContainsKey(word))
                        nc = c.WordBag[word];
                    else
                        nc = 0;

                    n = c.WordBagOccurence;
                    //m_prob_wk_vj[k, j] = (nc + 1.0) / (c.Count + numVocabulary);
                    m_prob_wk_vj_log[k, j] = Math.Log((nc + 1.0) / (n + numVocabulary));

                    k++;    // next word
                }
                j++;    // next category
            }

        }


        /// <summary>
        /// Calculate this for each category:
        /// 
        ///     v = P(vj) * Mk P(wk|vj)
        /// 
        ///     logv = log(P(vj)) + P(w1|vj) + ... + P(wm|vj)
        ///     
        ///     use logv for avoiding double underflow.
        /// 
        /// </summary>
        /// <param name="text">The text we want to classify</param>
        /// <param name="result">The classification result</param>
        public virtual void Predict(TextClassificationProblem problem, TextExample text, ref ClassificationResult result)
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
        }

        #endregion

        #region Constructors
        public NaiveBayesClassifier(CategoryCollection collect)
        {
            m_CategoryCollection = collect;
        }
        #endregion


    }
}
