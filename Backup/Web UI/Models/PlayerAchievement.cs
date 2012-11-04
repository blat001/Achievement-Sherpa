using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AchievementSherpa.Business;
using AchievementSherpa.Business;

namespace Web_UI.Models
{
    public class PlayerAchievement : AchievedAchievement
    {
        public Achievement Achievement
        {
            get;
            set;
        }
    }
}