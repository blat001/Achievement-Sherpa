using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AchievementSherpa.Business;

namespace Web_UI.Models
{
    public class Player
    {
        public Character WowPlayer
        {
            get;
            set;
        }

        public IList<Achievement> RecommendedAchievements
        {
            get;
            set;
        }

        public IList<PlayerAchievement> RecentAchievements
        {
            get;
            set;
        }

        public int Postion { get; set; }

        public int GuildPostion { get; set; }

        public int ServerPosition { get; set; }

        public int WorldWidePositon { get; set; }
    }
}