using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AchievementSherpa.Business;

namespace AchievementSherpa.Business
{
    public class Character
    {
        public Character(string name, string server, string region)
        {
            Name = name;
            Server = server;
            Region = region;
            Achievements = new List<AchievedAchievement>();
        }

        public string _id
        {
            get;
            set;
        }

        public DateTime? LastParseDate
        {
            get;
            set;
        }
        public int CurrentPoints
        {
            get;
            set;
        }

        public string Guild
        {
            get;
            set;
        }

        public string Class
        {
            get;
            set;
        }

        public string Race
        {
            get;
            set;
        }

        public int Level
        {
            get;
            set;
        }

        public int Side
        {
            get;
            set;
        }
        public IList<AchievedAchievement> Achievements
        {
            get;
            set;
        }


        public string Region
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }

        public string Server
        {
            get;
            set;
        }


        public int ClusterNumber
        {
            get;
            set;
        }

        public int TotalAchievementPoints
        {
            get
            {
                //return 0;
               return Achievements.Sum(a => a.Points);
            }
        }

        public bool HasAchieved(Achievement achievement)
        {
            if (achievement == null)
            {
                return false;
            }
            return Achievements.Where(aa => aa.BlizzardID == achievement.BlizzardID).Count() > 0;
        }


        public bool HasAchieved(int achievement)
        {
            return Achievements.Where(aa => aa.BlizzardID == achievement).Count() > 0;
        }

        public void AddNewAchivement(DateTime whenAchieved, Achievement achievement)
        {
            if (achievement != null)
            {
                Achievements.Add(new AchievedAchievement() { WhenAchieved = whenAchieved, BlizzardID = achievement.BlizzardID, Points = achievement.Points });
            }
            
        }

        public int GetHighestAchievementInSeries(AchievementSeries series)
        {
            int maxAchievement = 0;
            for (int i = 0; i < series.AchievementIds.Count; i++)
            {
                if (HasAchieved(series.AchievementIds[i]))
                {
                    maxAchievement = series.AchievementIds[i];
                }
            }
            return maxAchievement;
        }

        public string Thumbnail { get; set; }
    }
}
