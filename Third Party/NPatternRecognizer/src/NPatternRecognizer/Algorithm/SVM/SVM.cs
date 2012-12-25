
// SVM:
//
// Each data point will be represented by a p-dimensional vector (a list of p numbers). 
// Each of these data points belongs to only one of two classes.We are interested in 
// whether we can separate them with a "p minus 1" dimensional hyperplane. 
// This is a typical form of linear classifier. There are many linear classifiers that 
// might satisfy this property. However, we are additionally interested in finding out 
// if we can achieve maximum separation (margin) between the two classes. By this we 
// mean that we pick the hyperplane so that the distance from the hyperplane to the 
// nearest data point is maximized.
// 
// Now, if such a hyperplane exists, it is clearly of interest and is known as the 
// maximum-margin hyperplane and such a linear classifier is known as a maximum margin 
// classifier.

namespace NPatternRecognizer.Algorithm.SVM
{
    using System;
    using NPatternRecognizer.Common;
    using NPatternRecognizer.Interface;

    /// <summary>
    /// 
    /// Support Vector Machines Algorithm:
    /// ==================================
    /// 
    /// Notation:
    /// 
    /// Standard SVM classifier:
    /// 
    /// Traininig set:
    ///     {xi, yi}, i=1 to N:
    ///     input xi belongs to R^n;
    ///     class labels yi belongs to {-1, +1}
    /// 
    /// Classifier:
    ///     y(x) = sign[wt phi(x) + b]
    ///     with phi(): R^n -> R^nh a mapping to a high demensional feature space
    ///     (which can be infinite demensional!)
    /// 
    /// For separable data, assume
    ///     wt phi(xi) + b >= +1, if yi = +1
    ///     wt phi(xi) + b <= +1, if yi = -1
    /// so for all i:
    ///     yi[wt phi(xi) + b] >= 1
    /// 
    /// Optimization problem (non-separable case):
    ///     min Gamma(w, epsilon) = 1/2wtw + c Sigma epsilon i, i=1 to N.
    /// s.t.    
    ///     yi[wt phi(xi) + b] >= 1 - epsilon i
    ///     epsilon i >= 0, i=1 to N.
    ///     
    /// See also
    /// 
    /// Kernel trick:
    /// -------------
    /// The kernel trick is a method for using a linear classifier algorithm to solve 
    /// a non-linear problem by mapping the original non-linear observations into a 
    /// higher-dimensional space, where the linear classifier is subsequently used; 
    /// this makes a linear classification in the new space equivalent to non-linear 
    /// classification in the original space.
    /// This is done using Mercer's theorem.
    /// 
    /// Mercer's theorem:
    /// -----------------
    /// Any continuous, symmetric, positive semi-definite kernel function K(x, y) 
    /// can be expressed as a dot product in a high-dimensional space.
    /// 
    /// </summary>
    public class Binary_SVM_GradientDescent
    {        

        #region Fields        
        private CategoryCollection m_categoryCollection;
        private double[] m_Alpha, m_newalpha;
        private SparseVector m_weight;
        private double W = 0.0;
        private double old_W = 0.0;
        private int l;  // training set size      
        // Kernel to use
        protected Kernel m_kernel;
        #endregion

        #region Methods

        //private void BuildExample(TextExample example, Vocabulary voc, int exampleCount)
        //{
        //    int dimension = voc.Count;
        //    SparseVector vector = new SparseVector(dimension);

        //    foreach (string word in example.Tokens.Keys)
        //    {
        //        int pos = voc.GetWordPosition(word);
        //        if (pos == Constants.KEY_NOT_FOUND)
        //            continue;

        //        // phi i(x) = tfi log(idfi) /k
        //        // tfi:     number of occurences of the term i in the document x
        //        // idfi:    the ratio between the total number of documents and the 
        //        //              number of documents containing the term
        //        // k:       normalisation constant ensuring that ||phi|| = 1 
        //        double phi = example.Tokens[word] * Math.Log(exampleCount / voc.WordExampleOccurMap[word]);
        //        vector.Components.Add(pos, phi);
                
        //    }
        //    vector.Normalize();
        //    example.X = vector;            
        //}

        //private void Preprocess(ClassificationProblem problem)
        //{
        //    Vocabulary voc;

        //    problem.RetrieveVocabulary(out voc);
        //    foreach (Category c in problem.CategoryCollection.Collection)
        //    {
        //        foreach (TextExample e in c.Examples)
        //        {
        //            BuildExample(e, voc, problem.ExampleCount);
        //        }
        //    }

        //    m_weight = new SparseVector(voc.Count);
        //}

        /// <summary>
        /// simple on-line algorithm for the 1-norm soft margin:
        /// training SVMs in the non-bias case.
        /// </summary>
        /// <param name="problem"></param>
        public void Train(ClassificationProblem problem)
        {
            ExampleSet t_Set;   // training set             

            //Logging.Info("Retrieving training set");
            t_Set = problem.TrainingSet;
            l = t_Set.Examples.Count;

            //Logging.Info("Preprocessing all the examples");
            //this.Preprocess(problem);

            m_Alpha = new double[l];
            m_newalpha = new double[l];

            for (int i=0;i<m_Alpha.Length;i++)
            {
                m_Alpha[i] = 0.0;
            }

            //Logging.Info("Gradient descent");

            while (true)
            {
                for (int i = 0; i < l; i++)
                {
                    double temp = 0.0;                    

                    for (int j=0;j<l;j++)
                    {
                        temp += m_Alpha[j] * t_Set.Examples[j].Label.Id * m_kernel.Compute(t_Set.Examples[i].X, t_Set.Examples[j].X);
                    }
                    m_newalpha[i] = m_Alpha[i] + Constants.SVM_Eta * (1.0 - t_Set.Examples[i].Label.Id * temp);

                    if (m_newalpha[i] < 0.0)
                    {
                        m_newalpha[i] = 0.0;
                    }
                    else if (m_newalpha[i] > Constants.SVM_C)
                    {
                        m_newalpha[i] = Constants.SVM_C;
                    }
                }

                this.CopyAlphas();

                W = this.CalculateSVM_W(t_Set);

                if (Math.Abs((W - old_W) / W) < Constants.SVM_Tolerance)
                {
                    break;
                }


                Logger.Info(string.Format("SVM W = {0}", W));


                old_W = W;
            }

            this.CalculateWeight(t_Set);
            //this.CalculateB(t_Set);

        }

        public double CrossValidate(ClassificationProblem problem)
        {
            ExampleSet t_Set;
            ExampleSet v_Set;   // validation set            

            //Logging.Info("Retrieving training set");
            t_Set = problem.TrainingSet;
            //Logging.Info("Retrieving validation set");
            v_Set = problem.ValidationSet;

            int numExample = v_Set.Examples.Count;
            int numCorrect = 0;

            //Logging.Info("Cross Validating on validation set");

            foreach (Example example in v_Set.Examples)
            {
                ClassificationResult result = new ClassificationResult();
                this.PredictText(t_Set, example, ref result);
                if (result.ResultCategoryId == example.Label.Id)
                {
                    numCorrect++;
                }
            }

            double correctRatio = 1.0 * numCorrect / numExample;
            Logger.Info(string.Format("Correct ratio: {0}", correctRatio));

            return correctRatio;
        }

        public void PredictText(ExampleSet t_Set, Example text, ref ClassificationResult result)
        {
            double f;

            f = Calculate_F(t_Set, text.X);

            if (f >= 0)
            {
                result.ResultCategoryId = +1;
            }
            else
            {
                result.ResultCategoryId = -1;
            }
        }

        /// <summary>
        /// No bias
        /// </summary>
        /// <param name="t_Set"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        private double Calculate_F(ExampleSet t_Set, SparseVector x)
        {
            double f = 0.0;

            for (int i = 0; i < m_Alpha.Length; i++)
            {
                f += m_Alpha[i] * t_Set.Examples[i].Label.Id * m_kernel.Compute(t_Set.Examples[i].X, x);
            }

            return f;
        }

        private double CalculateSVM_W(ExampleSet t_Set)
        {
            double W = 0.0;
            double temp = 0.0;
            
            for (int i = 0; i < m_Alpha.Length; i++)
            {
                temp += m_Alpha[i];
            }
            W = temp;

            temp = 0.0;
            for (int i = 0; i < m_Alpha.Length; i++)
            {
                for (int j = 0; j < m_Alpha.Length; j++)
                {
                    temp += t_Set.Examples[i].Label.Id * t_Set.Examples[j].Label.Id * m_Alpha[i] * m_Alpha[j] * m_kernel.Compute(t_Set.Examples[i].X, t_Set.Examples[j].X);
                }
            }
            W = W - temp / 2;

            return W;
        }

        private void CopyAlphas()
        {
            for (int i = 0; i < m_Alpha.Length; i++)
            {
                m_Alpha[i] = m_newalpha[i];
            }
        }

        private void CalculateWeight(ExampleSet t_Set)
        {
            for (int i = 0; i < l; i++)
            {
                m_weight.Add((t_Set.Examples[i].Label.Id * m_Alpha[i]) * t_Set.Examples[i].X);
            }
        }


        #endregion

        #region Constructors
        public Binary_SVM_GradientDescent(CategoryCollection collect)
        {            
            m_categoryCollection = collect;
            // It was said that for text classification, linear kernel is the best choice, 
            //  because of the already-high-enough feature dimension
            m_kernel = new LinearKernel();
        }
        #endregion

    }
}
