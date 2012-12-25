
namespace NPatternRecognizer.Algorithm.SVM
{
    using NPatternRecognizer.Interface;

    public class DAGSVM
    {
        #region Fields
        private ClassificationProblem m_problem;
        private int m_n;
        private int m_k;
        private Node m_dag;
        #endregion

        #region Methods
        public void Train()
        {
        }

        public double CrossValidate()
        {
            return 0;
        }

        #endregion

        public DAGSVM(ClassificationProblem problem)
        {
            this.m_problem = problem;
            this.m_n = problem.CategoryCount;
            this.m_k = m_n * (m_n - 1) / 2;
            this.m_dag = new Node(problem, 0, m_n - 1);
        }


    }

    class Node
    {
        private ClassificationProblem m_problem;
        private int m_first;
        private int m_second;
        private Node m_leftChild;
        private Node m_rightChild;
        private LinearLeraningMachine m_llm;
        //private bool m_isleaf;

        public Node(ClassificationProblem problem, int first, int second)
        {
            this.m_problem = problem;
            this.m_first = first;
            this.m_second = second;
            this.m_llm = new LinearLeraningMachine(problem);
            this.m_llm.Train();

            if (second > first + 1)
            {
                this.m_leftChild = new Node(problem, first + 1, second);
                this.m_rightChild = new Node(problem, first, second - 1);
                //this.m_isleaf = false;
            }
            else
            {
                //this.m_isleaf = true;
            }
        }

        public int Predict(Example example)
        {

            return +1;
        }

    }
}
