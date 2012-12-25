
namespace NPatternRecognizer.Test
{
    using System;
    using NPatternRecognizer.Algorithm.ANN;
    using NPatternRecognizer.Algorithm.Boost;
    using NPatternRecognizer.Algorithm.KNN;
    using NPatternRecognizer.Algorithm.SVM;
    using NPatternRecognizer.Common;
    using NPatternRecognizer.Interface;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass()]
    public partial class BasicTest
    {

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion
        #region SVM

        [TestMethod()]
        //[Ignore("Ignore as Binary_SVM_SMO_Test is time-consuming.")]
        public void Binary_SVM_SMO_Test()
        {
            Logger.Info("Binary_SVM_SMO_Test");

            int hit = 0;
            double correctRatio;

            ClassificationProblem problem = ProblemFactory.CreateClassificationProblem(ClassificationProblemType.ChessBoard);

            ExampleSet t_set = problem.TrainingSet;
            ExampleSet v_set = problem.ValidationSet;   

            Binary_SVM_SMO classifier= new Binary_SVM_SMO(problem);
            classifier.TrainSet = t_set;
            classifier.Kernel = new GaussianRBFKernel(0.001); 
            
            classifier.Train();

            Logger.Info("Doing cross-validation.");

            foreach (Example e in v_set.Examples)
            {
                int iResult = classifier.Predict(e.X);
                if (e.Label.Id == iResult)
                {
                    hit++;
                }
            }

            correctRatio = 1.0 * hit / v_set.Count;

            Assert.IsTrue(correctRatio > 0.950, string.Format("SVM-SMO (2-class) Correct Ratio, expected: greater than 0.970, actual: {0}.", correctRatio));
        }        

        #endregion


        #region KNN

        [TestMethod()]
        public void KNN_Test()
        {
            Logger.Info("KNN_Test");

            int hit = 0;
            double correctRatio;

            ClassificationProblem problem = ProblemFactory.CreateClassificationProblem(ClassificationProblemType.ChessBoard);
            KNN classifier;

            Logger.Info("Loading training data.");

            ExampleSet t_set = problem.TrainingSet;
            ExampleSet v_set= problem.ValidationSet;           

            classifier = new KNN();
            classifier.KNN_K = 7;
            classifier.TrainSet = t_set;
            classifier.Train();            

            Logger.Info("Doing cross-validation.");

            foreach (Example e in v_set.Examples)
            {
                int iResult = classifier.Predict(e.X);
                if (e.Label.Id == iResult)
                {
                    hit++;
                }
            }

            correctRatio = 1.0 * hit / v_set.Count;

            Logger.Info("CorrectRatio: {0}", correctRatio);

            Assert.IsTrue(correctRatio > 0.930, string.Format("KNN (2-class) Correct Ratio, expected: greater than 0.930, actual: {0}.", correctRatio));
        }

        #endregion


        #region ANN_BP

        [TestMethod()]
        //[Ignore("Not ready for daily execution.")]
        public void ANN_BP_Test()
        {
            Logger.Info("ANN_BP_Test");

            int hit = 0;
            double correctRatio;

            ClassificationProblem problem = ProblemFactory.CreateClassificationProblem(ClassificationProblemType.ChessBoard);
            ANN_BP classifier;

            Logger.Info("Loading training data.");

            ExampleSet t_set = problem.TrainingSet;
            ExampleSet v_set= problem.ValidationSet;

            classifier = new ANN_BP(problem.Dimension);            
            classifier.TrainSet = t_set;           
            classifier.MaximumIteration = Int32.MaxValue;
            classifier.ANN_Eta = 0.5;
            classifier.ANN_Epsilon = 1e-3;
            classifier.LogInterval = 1000;
            classifier.Train();                        

            Logger.Info("Doing cross-validation.");

            foreach (Example e in v_set.Examples)
            {
                int iResult = classifier.Predict(e.X);
                
                if (e.Label.Id == iResult)
                {
                    hit++;
                }
            }

            correctRatio = 1.0 * hit / v_set.Count;

            Logger.Info("CorrectRatio: {0}", correctRatio);

            Assert.IsTrue(correctRatio > 0.900, string.Format("ANN_BP (2-class) Correct Ratio, expected: greater than 0.930, actual: {0}.", correctRatio));
        }

        #endregion       


        #region AdaBoost

        [TestMethod()]
        public void AdaBoost_Test()
        {
            Logger.Info("AdaBoost_Test");

            int hit = 0;
            double correctRatio;

            ClassificationProblem problem = ProblemFactory.CreateClassificationProblem(ClassificationProblemType.ChessBoard);
            AdaBoost classifier; 

            ExampleSet t_set = problem.TrainingSet;
            ExampleSet v_set = problem.ValidationSet;

            classifier = new AdaBoost(t_set, 1000, problem.Dimension);
            classifier.Train();            

            foreach (Example e in v_set.Examples)
            {
                int iResult = classifier.Predict(e.X);               
                
                if (e.Label.Id == iResult)
                {
                    hit++;
                }
            }

            correctRatio = 1.0*hit / v_set.Count;

            Logger.Info("CorrectRatio: {0}", correctRatio);

            Assert.IsTrue(correctRatio > 0.900, string.Format("AdaBoost (2-class) Correct Ratio, expected: greater than 0.900, actual: {0}.", correctRatio));
        }

        #endregion

    }
}
