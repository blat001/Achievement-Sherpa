
namespace NPatternRecognizer.Test
{
    using System;
    using System.IO;
    using System.Linq;
    using NPatternRecognizer.Algorithm.NaiveBayes;
    using NPatternRecognizer.Common;
    using NPatternRecognizer.Interface;

    public enum ClassificationProblemType
    {
        ChessBoard,
        Text,
    }

    public static class ProblemFactory
    {
        public static ClassificationProblem CreateClassificationProblem(ClassificationProblemType problemType)
        {
            switch (problemType)
            {
                case ClassificationProblemType.ChessBoard:
                    return CreateChessBoard();
                case ClassificationProblemType.Text:
                    return CreateText();
                default:
                    throw new ArgumentException();
            }

        }

        #region ChessBoard

        private static ClassificationProblem CreateChessBoard()
        {
            ClassificationProblem problem = new ClassificationProblem();

            CategoryCollection collect = new CategoryCollection();
            collect.Add(new Category(+1, "+1"));
            collect.Add(new Category(-1, "-1"));

            problem.Dimension = 2;
            problem.CategoryCollection = collect;
            problem.TrainingSet = GetExamples(collect);
            problem.ValidationSet = GetExamples(collect);


            return problem;

        }

        /// <summary>
        /// foamliu, 2009/04/15, 生成样本.
        /// </summary>
        /// <param name="set"></param>
        private static ExampleSet GetExamples(CategoryCollection collect)
        {
            const int Rows = 4;
            const int Columns = 4;
            const int CellWidth = 100;
            const int CellHeight = 100;
            const int ExampleNumber = 640;

            ExampleSet set = new ExampleSet();
            set.Examples.Clear();
            Random rand = new Random();

            for (int i = 0; i < ExampleNumber; i++)
            {
                int x = (int)(rand.NextDouble() * Columns * CellWidth);
                int y = (int)(rand.NextDouble() * Rows * CellHeight);

                Example e = new Example();
                e.X = new SparseVector(2);
                e.X[0] = x;
                e.X[1] = y;
                e.Label = collect.GetCategoryById(
                    GetCat(x, y, CellWidth, CellHeight));

                set.AddExample(e);
            }

            return set;
        }

        private static int GetCat(int x, int y, int cellWidth, int cellHeight)
        {
            int toCheck = x / cellWidth + y / cellHeight;
            if ((toCheck & 1) == 0)
                return +1;
            else
                return -1;
        }


        #endregion


        #region Text

        /// <summary>
        /// foamliu, 2009/12/21, please make sure you've uncompressed "2_newsgroups.7z" in the "data" folder.
        /// </summary>
        /// <returns></returns>
        private static ClassificationProblem CreateText()
        {
            const string DataFolder = @"..\data\2_newsgroups";

            ClassificationProblem problem = new ClassificationProblem();

            ExampleSet t_set = new ExampleSet();
            ExampleSet v_set = new ExampleSet();

            CategoryCollection collect = new CategoryCollection();
            collect.Add(new Category(+1, "+1"));
            collect.Add(new Category(-1, "-1"));

            problem.Dimension = 2;
            problem.CategoryCollection = collect;

            DirectoryInfo dataFolder = new DirectoryInfo(DataFolder);
            DirectoryInfo[] subfolders = dataFolder.GetDirectories();
            int count = 0;

            for (int i = 0; i < subfolders.Count(); i++)
            {
                DirectoryInfo categoryFolder = subfolders[i];
                int cat = i * 2 - 1;
                // for all the text files in each category
                FileInfo[] files = categoryFolder.GetFiles();

                count = 0;
                int trainSetCount = Convert.ToInt32(Constants.TrainingSetRatio * files.Count());
                for (int j = 0; j < files.Count(); j++)
                {
                    FileInfo textFile = files[j];
                    Example e = new Example();

                    if (++count < trainSetCount)
                    {
                        t_set.AddExample(e);
                    }
                    else
                    {
                        v_set.AddExample(e);
                    }


                }
            }

            problem.TrainingSet = t_set;
            problem.ValidationSet = v_set;


            return problem;
        }

        private static void BuildExample(TextExample example, Vocabulary voc, int exampleCount)
        {
            int dimension = voc.Count;
            SparseVector vector = new SparseVector(dimension);

            foreach (string word in example.Tokens.Keys)
            {
                int pos = voc.GetWordPosition(word);
                if (pos == Constants.KEY_NOT_FOUND)
                    continue;

                // phi i(x) = tfi log(idfi) /k
                // tfi:     number of occurences of the term i in the document x
                // idfi:    the ratio between the total number of documents and the 
                //              number of documents containing the term
                // k:       normalisation constant ensuring that ||phi|| = 1 
                double phi = example.Tokens[word] * Math.Log(exampleCount / voc.WordExampleOccurMap[word]);
                vector.Components.Add(pos, phi);

            }
            vector.Normalize();
            example.X = vector;
        }

        #endregion
    }
}
