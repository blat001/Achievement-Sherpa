
namespace NPatternRecognizer.Interface
{
    using System;
    using System.Collections.Generic;

    public class ClassificationResult
    {
        #region Fields
        /// <summary>
        /// For each category: name to their logv map
        /// </summary>
        private SortedDictionary<string, double> m_CategoryName2LogVMap = new SortedDictionary<string, double>();

        /// <summary>
        /// The category name which has the max logv.
        /// </summary>
        private string m_vnb = null;

        private int m_ResultCategoryId;

        #endregion

        #region Properties
        // read-only
        public SortedDictionary<string, double> CategoryName2LogVMap
        {
            get { return m_CategoryName2LogVMap; }
            //set { m_categories = value; }
        }

        /// <summary>
        /// Confidence for the result Vnb (0-1)
        /// Provide this interface so that upper rule engine can
        ///     "refuse to guess" when confidence is too low.
        /// </summary>
        //public double Confidence
        //{
        //    get
        //    {
        //        this.Normalize();
        //        return m_CategoryName2LogVMap[this.Vnb];
        //    }
        //}

        /// <summary>
        /// The category name which has the max logv.
        /// </summary>
        public string Vnb
        {
            get
            {
                if (string.IsNullOrEmpty(m_vnb))
                {
                    this.CalculateVnb();
                }
                return m_vnb;
            }
        }

        public int ResultCategoryId
        {
            get
            {
                return m_ResultCategoryId;
            }
            set
            {
                m_ResultCategoryId = value;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// debug use
        /// </summary>
        public void Dump()
        {
            System.Console.WriteLine("Classification Result");
            System.Console.WriteLine("==========================================");
            foreach (string key in m_CategoryName2LogVMap.Keys)
            {
                System.Console.WriteLine("{0}:   {1}", key, m_CategoryName2LogVMap[key]);
            }
            System.Console.WriteLine("==========================================");

            CalculateVnb();
            System.Console.WriteLine("Vnb {0}:   {1}", Vnb, m_CategoryName2LogVMap[Vnb]);
        }
        #endregion

        #region Helpers
        /// <summary>
        /// Normalize the CategoryName2LogVMap
        /// </summary>
        public void Normalize()
        {
            int j = 0;
            string[] names = new string[m_CategoryName2LogVMap.Count];
            double[] v = new double[m_CategoryName2LogVMap.Count];

            // Step1: 1st scan
            //
            //  1.Find out max logv
            //  2.Collect names
            //  3.Collect logv
            //
            double maxLogv = double.MinValue;
            foreach (string name in m_CategoryName2LogVMap.Keys)
            {
                // 1.Find out max logv.
                if (m_CategoryName2LogVMap[name] > maxLogv)
                {
                    maxLogv = m_CategoryName2LogVMap[name];
                }
                // 2.Collect names.
                names[j] = name;
                // 3.Collect logv.
                v[j] = m_CategoryName2LogVMap[name];

                j++;    // next category
            }

            // Step2: 2nd scan
            //
            // Max logv normalize to 0
            double sum = 0.0;
            for (j = 0; j < v.Length; j++)
            {
                v[j] = v[j] - maxLogv;
                v[j] = Math.Exp(v[j]);
                sum += v[j];
            }
            System.Diagnostics.Debug.Assert(0.0 != sum);

            // Step2: 3rd scan
            //
            for (j = 0; j < v.Length; j++)
            {
                m_CategoryName2LogVMap[names[j]] = v[j] / sum;
            }

        }
        /// <summary>
        /// Calculate Vnb
        /// </summary>
        private void CalculateVnb()
        {
            string maxCat = string.Empty;
            double maxProb = double.MinValue;

            foreach (string key in m_CategoryName2LogVMap.Keys)
            {
                if (m_CategoryName2LogVMap[key] > maxProb)
                {
                    maxProb = m_CategoryName2LogVMap[key];
                    maxCat = key;
                }
            }

            m_vnb = maxCat;
        }
        #endregion

        #region Constructors
        #endregion
    }
}
