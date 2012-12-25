using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AchievementSherpa.Business;

namespace Web_UI.Models
{
    public class CategoryHelper
    {


        public static string NiceEnumName(AchievementCategories value)
        {
            string name = Enum.GetName(typeof(AchievementCategories), value);
            return string.Concat(name.Select(x => Char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');
        }


        public static string CategoryDisplay(Achievement achievement)
        {
                     return string.Format("{0}",
                    achievement.Category);
        }

        public static string CategoryName(int categoryId)
        {
            switch (categoryId)
            {
                case 168:
                    return "Dungeons & Raids";
                case 14922:
                    return "Lich King Raid";
                case 15067 :
                    return "Cataclysm Dungeon";
                case 97 :
                    return "Exploration";
                case 96 :
                    return "Quests";
                case 95 :
                    return "Player vs. Player";
                case 15117 :
                    return "Pet Battles";
                case 155 :
                    return "World Events";
                case  201 :
                    return "Reputation";
                case 169 :
                    return "Professions";
                

            }

            return "Unknown - " + categoryId;
        }
    }
}