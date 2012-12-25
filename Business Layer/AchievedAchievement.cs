using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AchievementSherpa.Business;

namespace AchievementSherpa.Business
{
    public class AchievedAchievement
    {
        public DateTime WhenAchieved
        {
            get;
            set;
        }
        public int Points
        {
            get;
            set;
        }

        public int BlizzardID
        {
            get;
            set;
        }
    }
}
