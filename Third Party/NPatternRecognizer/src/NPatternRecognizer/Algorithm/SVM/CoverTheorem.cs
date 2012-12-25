
// Cover theorem:
//  The probability that classes are linearly separable increases when 
//  the features are nonlinearly mapped to a higher dimensional feature space.
namespace NPatternRecognizer.Algorithm.SVM
{
    using System;
    using NPatternRecognizer.Common;
    using NPatternRecognizer.Interface;

    public class CoverTheorem
    {
        private Random m_rand;

        private Category GetRandCategory()
        {
            int id;
            if (m_rand.NextDouble() > 0.5)
            {
                id = +1;
            }
            else
            {
                id = -1;
            }

            Category c = new Category(id, null);
            return c;
        }

        public CoverTheorem()
        {
            m_rand = new Random();
        }

        public void Test()
        {            
            int l = 30;
            int k = 10;
            double ratioSeparable = 0;
            int numSeparable = 0;
            ExampleSet set = new ExampleSet();


            for (int d = 10; d < 50; d=d+10)
            {
                numSeparable = 0;

                for (int n = 0; n < k; n++)
                {
                    set.Examples.Clear();

                    for (int i = 0; i < l; i++)
                    {
                        SparseVector x = new SparseVector(d);

                        for (int j = 0; j < d; j++)
                        {
                            x[j] = m_rand.NextDouble();
                        }

                        Category c = GetRandCategory();
                        Example e = new Example(c);
                        e.X = x;
                        set.AddExample(e);
                    }

                    SimpleLLM llm = new SimpleLLM(set, d);
                    //Logging.Info(string.Format("IsLinearSeparable: {0}", llm.IsLinearSeparable()));
                    //System.Console.WriteLine(string.Format("IsLinearSeparable: {0}", llm.IsLinearSeparable()));
                    if (llm.IsLinearSeparable())
                    {
                        numSeparable++;
                    }

                }

                ratioSeparable = 1.0 * numSeparable / k;

                System.Console.WriteLine(string.Format("d: {0}, l: {1}, Separable ratio: {2}", d, l, ratioSeparable));
            }
        }

        //public static void Main()
        //{
        //    CoverTheorem theorem = new CoverTheorem();
        //    theorem.Test();

        //}
    }

    public class SimpleLLM
    {
        #region Constants
        private const double eta = 0.1;       

        #endregion


        #region Fields        
        private SparseVector m_weight;        
        private int m_l;
        private ExampleSet m_t_set;
        
        #endregion               

        public bool IsLinearSeparable()
        {
            int numTime = 0;
            int numberErrors;

            do
            {
                numberErrors = 0;

                for (int i = 0; i < m_l; i++)
                {
                    if (y(i) * SparseVector.DotProduct(x(i), m_weight) <= 0)
                    {
                        this.m_weight.Add((eta * y(i)) * x(i));                        

                        numberErrors++;
                    }
                }              

            }
            while (numberErrors > 0 && ++numTime<10);

            if (numberErrors == 0)
                return true;
            else
                return false;
        }

        private int y(int i)
        {
            return m_t_set.Examples[i].Label.Id;
        }

        private SparseVector x(int i)
        {
            return m_t_set.Examples[i].X;
        }

        public SimpleLLM(ExampleSet t_set, int d)
        {
            this.m_t_set = t_set;
            this.m_l = t_set.Examples.Count;
            this.m_weight = new SparseVector(d);
        }
    }

}
