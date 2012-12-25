

namespace NPatternRecognizer.Interface
{
    public class ClassificationProblem
    {
        public int CategoryCount 
        {
            get
            {
                if (CategoryCollection == null)
                    return 0;
                else
                    return CategoryCollection.Count;
            }
        }
        public int Dimension
        {
            get;
            set;
        }

        public CategoryCollection CategoryCollection { get; set; }        
        public ExampleSet TrainingSet { get; set; }
        public ExampleSet ValidationSet { get; set; }
        public DataProvider DataProvider { get; set; }

    }
}
