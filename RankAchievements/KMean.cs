using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Collections;
namespace RankAchievements
{
    /// <summary>
    /// Summary description for KMean
    /// </summary>
    /// 
    struct Point
    {
        public int Cluster;
        public int X;
        public int Y;
        public int Count;

        public Point(int pCluster, int pX, int pY)
        {
            Cluster = pCluster;
            X = pX;
            Y = pY;
            Count = 0;
        }
        public Point(Point pPoint)
            : this(pPoint.Cluster, pPoint.X,
        pPoint.Y)
        {
        }
    }
    public class KMean
    {
        private ArrayList _data = new ArrayList();
        private Point[] _centres = new Point[3];
        public KMean()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        /*
         * 
         *  ###################################################################
         *  FUNCTIONS
         *  + kMeanCluster:
         *  + dist: calculate distance
         *  + min2: return minimum value between two numbers
         *  ###################################################################
         * 
         *  input: + Data matrix (0 to 2, 1 to TotalData); Row 0 = cluster, 1 =X, 2= Y; data in columns
         *         + numCluster: number of cluster user want the data to be clustered
         *         + private variables: Centroid, TotalData
         *  ouput: o) update centroid
         *         o) assign cluster number to the Data (= row 0 of Data)
         * 
         */
        private void kMeanCluster()
        {
            Boolean running = true;
            double min = 10000000.0; // big number
            double distance = 0.0;
            int cluster = 0;
            Point p1 = (Point)_data[_data.Count - 1];
            foreach (Point p2 in _centres)
            {
                distance = Distance(p1, p2);
                if (distance < min)
                {
                    min = distance;
                    cluster = p2.Cluster;
                }
            }
            p1 = (Point)_data[_data.Count - 1];
            p1.Cluster = cluster;
            _data[_data.Count - 1] = p1;

            Point[] sums;
            int c;
            int d;

            while (running)
            {
                //this loop will surely convergent
                //calculate new centroids
                sums = new Point[3];
                for (c = 0; c < _data.Count; c++)
                {
                    p1 = (Point)_data[c];
                    sums[p1.Cluster].X += p1.X;
                    sums[p1.Cluster].Y += p1.Y;
                    sums[p1.Cluster].Count++;
                }
                for (c = 0; c <= _centres.GetUpperBound(0); c++)
                {
                    if (sums[c].Count > 0)
                    {
                        _centres[c].X = sums[c].X / sums[c].Count;
                        _centres[c].Y = sums[c].Y / sums[c].Count;
                    }
                }

                running = false;

                for (c = 0; c < _data.Count; c++)
                {
                    min = 10000000.0; //big number
                    for (d = 0; d <= _centres.GetUpperBound(0); d++)
                    {
                        distance = Distance((Point)_data[c], _centres[d]);
                        if (distance < min)
                        {
                            min = distance;
                            cluster = d;
                        }
                    }
                    p1 = (Point)_data[c];
                    if (p1.Cluster != cluster)
                    {
                        p1.Cluster = cluster;
                        running = true;
                        _data[c] = p1;
                    }
                }
            }
        }

        private double Dist(int pX1, int pY1, int pX2, int pY2)
        {
            //calculate Euclidean distance
            return Math.Sqrt(Math.Pow((pY2 - pY1), 2) + Math.Pow((pX2 - pX1), 2));
        }
        private double Distance(Point pP1, Point pP2)
        {
            return Dist(pP1.X, pP1.Y, pP2.X, pP2.Y);
        }
        private int min2(int num1, int num2)
        {
            //return minimum value between two numbers
            if (num1 < num2)
                return num1;
            else return num2;
        }
    }

}