
namespace NPatternRecognizer.Interface
{
    using System;
    using System.Collections.Generic;

    public class CategoryCollection
    {
        #region Fields
        private SortedDictionary<String, Category> m_Categories = new SortedDictionary<String, Category>();
        #endregion

        #region Properties
        public int Count
        {
            get
            {
                return m_Categories.Count;
            }
        }
        public List<Category> Collection
        {
            get
            {
                List<Category> collect = new List<Category>();
                foreach (String name in m_Categories.Keys)
                {
                    collect.Add(m_Categories[name]);
                }
                return collect;
            }
        }
        #endregion

        #region Methods

        public void Add(Category c)
        {
            m_Categories.Add(c.Name, c);
        }

        public Category GetCategoryById(int id)
        {
            foreach (String name in m_Categories.Keys)
            {
                if (m_Categories[name].Id == id)
                {
                    return m_Categories[name];
                }
            }
            return null;
        }

        public Category GetCategoryByName(string name)
        {
            if (m_Categories.ContainsKey(name))
            {
                return m_Categories[name];
            }
            return null;
        }

        #endregion

        #region Constructors

        public CategoryCollection()
        {
        }

        #endregion
    }
}
