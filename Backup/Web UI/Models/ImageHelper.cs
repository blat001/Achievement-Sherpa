using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AchievementSherpa.Business;

namespace Web_UI.Models
{
    public static class ImageHelper
    {

        public static string SmallClassImage(Character character)
        {
            if (!string.IsNullOrEmpty(character.Class))
            {
                return string.Format("http://static.wowhead.com/images/wow/icons/small/class_{0}.jpg",
                    character.Class.ToLowerInvariant().Replace(" ", ""));
            }

            return "";
        }

         public static string SmallRaceImage(Character character)
        {
            if (!string.IsNullOrEmpty(character.Race))
            {
                return string.Format("http://static.wowhead.com/images/wow/icons/small/race_{0}_male.jpg",
                    character.Race.ToLowerInvariant().Replace(" ", ""));
            }

            return "";
        }

         public static string MediumImage(Achievement achievement)
         {
             if (!string.IsNullOrEmpty(achievement.Icon))
             {
                 return string.Format("http://static.wowhead.com/images/wow/icons/medium/{0}.jpg",
                     achievement.Icon.ToLowerInvariant().Replace(" ", ""));
             }

             return "http://static.wowhead.com/images/wow/icons/medium/inv_bannerpvp_01.jpg";
         }

         public static string LargeImage(Achievement achievement)
         {
             if (!string.IsNullOrEmpty(achievement.Icon))
             {
                 return string.Format("http://static.wowhead.com/images/wow/icons/large/{0}.jpg",
                     achievement.Icon.ToLowerInvariant().Replace(" ", ""));
             }

             return "http://static.wowhead.com/images/wow/icons/medium/inv_bannerpvp_01.jpg";
         }


        //background-image: url(http://static.wowhead.com/images/wow/icons/medium/inv_bannerpvp_01.jpg); 
    }
}