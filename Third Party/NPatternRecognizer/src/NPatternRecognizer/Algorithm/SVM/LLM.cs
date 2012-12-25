using System;
using System.Collections.Generic;
using System.Text;
using NPatternRecognizer.Common;
using NPatternRecognizer.Interface;

namespace NPatternRecognizer.Algorithm.SVM
{
    public class LinearLeraningMachine
    {
        #region Constants
        private const double eta = 0.1;
        private const double R = 1.0;
        //private int m_first = 0;
        //private int m_second = 1;
        #endregion


        #region Fields
        private ClassificationProblem m_problem;
        private SparseVector m_weight;
        //private double m_b;
        private int m_l;
        private ExampleSet m_t_set;        
        #endregion

        public ExampleSet TrainSet
        {
            set
            {
                this.m_t_set = value;
                this.m_l = m_t_set.Examples.Count;
            }
        }

        public void Train()
        {
            //this.m_b = 0;
            
            int numberErrors;

            do
            {
                numberErrors = 0;

                for (int i = 0; i < m_l; i++)
                {
                    if (y(i) * SparseVector.DotProduct(x(i), m_weight) <= 0)
                    {
                        this.m_weight.Add((eta * y(i)) * x(i));
                        //this.m_b += eta * y(i) * R * R;
                        
                        numberErrors++;
                    }
                }

                Logger.Info(string.Format("Number Errors: {0}", numberErrors));

            } 
            while (numberErrors > 0);
        }

        public double CrossValidate()
        {
            ExampleSet v_Set;   // validation set

            //Logging.Info("Retrieving validation set");
            v_Set = this.m_problem.ValidationSet;

            int numExample = v_Set.Examples.Count;
            int numCorrect = 0;            

            //Logging.Info("Cross Validating on validation set");

            foreach (Example example in v_Set.Examples)
            {
                ClassificationResult result = new ClassificationResult();

                if (this.PredictText(example) == example.Label.Id)
                {
                    numCorrect++;
                }

            }

            double correctRatio = 1.0 * numCorrect / numExample;
            Logger.Info(string.Format("Correct ratio: {0}", correctRatio));

            return correctRatio;
        }

        private int PredictText(Example example)
        {
            double f;

            f = SparseVector.DotProduct(m_weight, example.X)/* + m_b*/;
            if (f >= 0)
                return +1;
            else
                return -1;
        }

        //public int Predict(SparseVector x)
        //{
        //    double f;

        //    f = SparseVector.DotProduct(m_weight, x)/* + m_b*/;
        //    if (f >= 0)
        //        return +1;
        //    else
        //        return -1;
        //}

        public void Predict(SparseVector x, out int result, out double confidence)
        {
            double f;

            f = SparseVector.DotProduct(m_weight, x)/* + m_b*/;

            confidence = S(Math.Abs(f));

            if (f >= 0)
                result = + 1;
            else
                result = -1;
        }

        /// <summary>
        /// foamliu, 2008/12/29, Sigmoid Activation Function.
        ///                1
        /// s(x)=  --------------------               
        ///        1 + exp(-alpha * x )
        /// </summary>
        /// <param name="total"></param>
        /// <returns>Output range of the function: [0, 1]</returns>
        private double S(double x)
        {
            // a is chosen between 0.5 and 2
            double alpha = 1.0;
            double s = 1.0 / (1.0 + Math.Exp(-alpha * x));

            return s;
        }

        private int y(int i)
        {
            int id = m_t_set.Examples[i].Label.Id;
            //if (id == m_first)
            //    return +1;
            //else
            //    return -1;
            return id;
        }

        private SparseVector x(int i)
        {
            return m_t_set.Examples[i].X;
        }


        public LinearLeraningMachine(ClassificationProblem problem)
        {

            this.m_problem = problem;
            this.m_t_set = problem.TrainingSet;
            //this.m_problem.RetrieveVocabulary(out this.m_voc);
            this.m_l = m_t_set.Examples.Count;
            //this.m_weight = new SparseVector(m_voc.Count);
        }

        public LinearLeraningMachine(int n)
        {
            this.m_weight = new SparseVector(n);            
        }
    }
}
