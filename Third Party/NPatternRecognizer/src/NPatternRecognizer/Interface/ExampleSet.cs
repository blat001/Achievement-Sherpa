

namespace NPatternRecognizer.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Collections.ObjectModel;

    [Serializable()]
    public class ExampleSet
    {
        #region Fields
        private Collection<Example> m_Collection = new Collection<Example>();
        #endregion

        #region Properties
        public Collection<Example> Examples
        {
            get
            {
                return m_Collection;
            }
        }
        public int Count
        {
            get
            {
                return this.m_Collection.Count;
            }
        }
        public Example this[int index]
        {
            get
            {
                if (index < 0 || index >= this.Count)
                    throw new ArgumentOutOfRangeException("index");

                return this.m_Collection[index];
            }

        }
        #endregion

        #region Methods
        public void AddExample(Example example)
        {
            m_Collection.Add(example);
        }
        #endregion

        #region Constructors
        public ExampleSet()
        {
        }

        #endregion


    }
}
