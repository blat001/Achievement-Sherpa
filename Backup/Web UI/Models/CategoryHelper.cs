using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AchievementSherpa.Business;

namespace Web_UI.Models
{
    public class CategoryHelper
    {


        public static string CategoryDisplay(Achievement achievement)
        {
            if (achievement.ParentCategory != "-1")
            {
                return string.Format("{0} > {1}",
                    CategoryName(achievement.ParentCategory),
                    CategoryName(achievement.Category));
            }

            return string.Format("{0}",
                    CategoryName(achievement.Category));
        }

        public static string CategoryName(string categoryId)
        {
            switch (categoryId)
            {
                case "168":
                    return "Dungeons & Raids";
                case "14922":
                    return "Lich King Raid";
                case "15067" :
                    return "Cataclysm Dungeon";
                case "97" :
                    return "Exploration";
                case "15069":
                    return "Cataclysm";
                case "95":
                    return "Player vs. Player";
                case "155":
                    return "World Events";
                case "15073" :
                    return "Battle for Gilneas";
                case "156":
                    return "Winter Veil";
                case "14803" :
                    return "Eye of the Storm";
                case "15074" :
                    return "Twin Peaks";
                case "165":
                    return "Arena";
                case "15068":
                    return "Cataclysm Raid";
                case "14941" :
                    return "Argent Tournament";
                case "14804":
                    return "Warsong Gulch";
                case "96" :
                    return "Quests";
                case "14808" :
                    return "Classic";
                case "14806" :
                    return "Lich King Dungeon";
                case "15092" :
                    return "Rated Battleground";
                case "14862":
                    return "Outland";
                case "14863":
                    return "Northrend";
                case "92" :
                    return "General";
                case "169" :
                    return "Professions";
                case "171" :
                    return "Fishing";
                case "201" :
                    return "Reputation";
                case "14865" :
                    return "Outland";
                case "-1" :
                    return "";

            }

            return "Unknown - " + categoryId;
        }
    }
}