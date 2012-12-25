using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cluster_Users
{

    public class Point
    {
        public int Id { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
    }

    public class PointCollection : List<Point>
    {
        public Point Centroid { get; set; }
    }



    class Program
    {
        static void Main(string[] args)
        {
        }


        public static List<PointCollection> DoKMeans(PointCollection points, int clusterCount)
        {
            //divide points into equal clusters
            List<PointCollection> allClusters = new List<PointCollection>();
            List<List<Point>> allGroups = ListUtility.SplitList<Point>(points, clusterCount);
            foreach (List<Point> group in allGroups)
            {
                PointCollection cluster = new PointCollection();
                cluster.AddRange(group);
                allClusters.Add(cluster);
            }

            //start k-means clustering
            int movements = 1;
            while (movements > 0)
            {
                movements = 0;

                foreach (PointCollection cluster in allClusters) //for all clusters
                {
                    for (int pointIndex = 0; pointIndex < cluster.Count; pointIndex++) //for all points in each cluster
                    {
                        Point point = cluster[pointIndex];

                        int nearestCluster = FindNearestCluster(allClusters, point);
                        if (nearestCluster != allClusters.IndexOf(cluster)) //if point has moved
                        {
                            if (cluster.Count > 1) //each cluster shall have minimum one point
                            {
                                Point removedPoint = cluster.RemovePoint(point);
                                allClusters[nearestCluster].AddPoint(removedPoint);
                                movements += 1;
                            }
                        }
                    }
                }
            }

            return allClusters;
        }
    }
}
