using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data_Library
{
    public partial class Character
    {
        public Character(string name, string server) : this (name, server, "us")
        {
        }

        public Character(string name, string server, string region)
        {
            Name = name;
            Server = server;
            Region = region;

            
        }

        public Character()
        {
        }

        public bool HasAchieved(Achievement achievement)
        {
            return Achievements.FirstOrDefault(a => a.AchievementID == achievement.AchievementID) != null;
        }

        public int TotalAchievementPoints
        {
            get
            {
                return this.Achievements.Sum(cach => cach.achievement.Points.Value);
            }
        }

        public void AddNewAchivement(DateTime when, Achievement achievement)
        {

            if (Achievements.FirstOrDefault(a => a.AchievementID == achievement.AchievementID) == null)
            {
                CharacterAchievement actualAchievement = new CharacterAchievement()
                       {
                           WhenAchieved = when,
                           achievement = achievement,
                           wowcharacter = this
                       };

                Achievements.Add(actualAchievement);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is Character)
            {
                return (CharacterID == ((Character)obj).CharacterID);
            }
            return base.Equals(obj);
        }



        public Achievement GetHighestAchievementInSeries(AchievementSeries achievementSeries)
        {
            return Achievements.Where(a => a.achievement.AchievementSeriesID == achievementSeries.achievementseriesid).OrderByDescending(a => a.achievement.SeriesOrder).Select(a=>a.achievement).FirstOrDefault();

        }
    }
}
