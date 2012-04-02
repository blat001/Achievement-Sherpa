using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Business_Layer;

namespace RankAchievements
{
    class Program
    {
        static void Main(string[] args)
        {
            IAchievementRepository achievementRepository = null;

            var achievements = achievementRepository.FindAll().OrderBy(a => a.BlizzardID);

            int rowCount = 0;
            IList<IDataList> data = new List<IDataList>();
            foreach (Character character in db.Characters.ToList())
            {

                data.Add(new CharacterData(character, achievements));
                rowCount++;
            }

            ClusterCollection clusters = KMeans.ClusterDataSet(6, data);


            int clusterNumber = 1;
            foreach (Cluster cluster in clusters)
            {
                double points = 0;
                for (int i = 0; i < cluster.Count; i++)
                {
                    points += ((CharacterData)cluster[i]).Character.TotalAchievementPoints;

                    ((CharacterData)cluster[i]).Character.ClusterNumber = clusterNumber.ToString();
                }
                Console.WriteLine("Cluster contains {0} characters, Avg Achievement Points {1}", cluster.Count, points / cluster.Count);


                clusterNumber++;
            }


            db.SaveChanges();



        }
    }

    public class CharacterData : IDataList
    {

        private double[] _achievementList;

        public Character Character
        {
            get;
            set;
        }

        public CharacterData(Character character, IList<Achievement> allAchievements)
        {
            Character = character;
            _achievementList = new double[allAchievements.Count];
            foreach (var individualAchivement in character.Achievements)
            {
                int fieldIndex = allAchievements.IndexOf(allAchievements.First(a => a.AchievementID == individualAchivement.AchievementID));
                _achievementList[fieldIndex] = 1;
            }
        }



        public double[] Data
        {
            get { return _achievementList; }
        }
    }
}
