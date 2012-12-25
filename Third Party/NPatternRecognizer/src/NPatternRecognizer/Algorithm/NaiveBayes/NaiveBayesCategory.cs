

namespace NPatternRecognizer.Algorithm.NaiveBayes
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    class NaiveBayesCategory
    {
        #region Fields
        private SortedDictionary<String, int> m_WordCountPairCollection;
        private int m_WordBagOccurence;
        #endregion

        #region Properties
        public SortedDictionary<String, int> WordBag
        {
            get
            {
                return m_WordCountPairCollection;
            }
        }
        public int WordBagOccurence
        {
            get
            {
                return m_WordBagOccurence;
            }
        }
        #endregion

        #region Methods
        public void AddExample(TextExample example)
        {
            foreach (string word in example.Tokens.Keys)
            {
                Utility.AddToken(m_WordCountPairCollection, word, example.Tokens[word]);
                m_WordBagOccurence += example.Tokens[word];
            }
        }

        #endregion

        #region Constructors
        public NaiveBayesCategory()
        {
            m_WordCountPairCollection = new SortedDictionary<string, int>();
            m_WordBagOccurence = 0;
        }
        #endregion

    }
}
