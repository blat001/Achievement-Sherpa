using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AchievementSherpa.Business;

namespace AchievementSherpa.Business
{
    public class AchievementSeries
    {
        public AchievementSeries()
        {
            AchievementIds = new List<string>();
        }

        public void AddAchievementToSeries(Achievement achievement)
        {
            AchievementIds.Add(achievement._id);
            achievement.Series = this;
        }

        public IList<string> AchievementIds
        {
            get;
            set;
        }

        /*
        public IList<Achievement> Achievements
        {
            get;
            set;
        }
         */

        

        public int Order
        {
            get;
            set;
        }
    }
}
