
namespace NPatternRecognizer.Algorithm.NaiveBayes
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using NPatternRecognizer.Interface;

    class NaiveBayesCategoryCollection
    {
        #region Fields
        private SortedDictionary<Category, NaiveBayesCategory> m_CategoryMap;
        #endregion

        #region Properties
        public Collection<NaiveBayesCategory> CategorySet
        {
            get
            {
                Collection<NaiveBayesCategory> collect = new Collection<NaiveBayesCategory>();
                foreach (Category c in m_CategoryMap.Keys)
                {
                    collect.Add(m_CategoryMap[c]);
                }
                return collect;
            }
        }
        #endregion

        #region Methods
        public void AddExample(TextExample example)
        {
            m_CategoryMap[example.Label].AddExample(example);
        }

        #endregion

        #region Constructors
        public NaiveBayesCategoryCollection(CategoryCollection categoryCollection)
        {
            m_CategoryMap = new SortedDictionary<Category, NaiveBayesCategory>();

            foreach (Category c in categoryCollection.Collection)
            {
                m_CategoryMap.Add(c, new NaiveBayesCategory());
            }
        }
        #endregion
    }
}
