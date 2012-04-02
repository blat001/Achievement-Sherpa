using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using AchievementSherpa.PageParser;

namespace AchievementSherpa.Business
{
    public class CharacterDownloader : ParserBase
    {
        private const string InfoFormat = "http://{2}.battle.net/wow/en/character/{0}/{1}/achievement";
        private const string InfoPageFormat = "./{0}/{1}/{2}/info.html";
        private const string DirectoryFormat = "./{0}/{1}/{2}/";
        private const string AchievementPageFormat = "./{0}/{1}/{2}/achievement_{3}.html";
        private const string AchievementFormat = "http://{2}.battle.net/wow/en/character/{0}/{1}/achievement/{3}";
        private readonly string[] achievementPages = new string[] { "92", "96", "14861", "15081", "14862", "14863", "15070", "97", "14777", "14778", "14779", "14780", "15069", "95", "165", "14801", "14802", "14803", "14804", "14881", "14901", "15003", "15073", "15074", "15075", "15092", "168", "14808", "14805", "14806", "14922", "15067", "15068", "169", "170", "171", "172", "15071", "201", "14864", "14865", "14866", "15072", "155", "160", "187", "159", "163", "161", "162", "158", "14981", "156", "14941", "81" };



        public DateTime MaxAge
        {
            get
            {
                return DateTime.Now.AddDays(-7);
            }
        }


        public object DownloadCharacter(object state)
        {
            Character character = state as Character;

            

            if (character != null)
            {

                string infoPath = string.Format(InfoPageFormat, character.Region, character.Server, character.Name);

                if (File.Exists(infoPath) && File.GetLastWriteTime(infoPath) > MaxAge)
                {
                    // Means that we have already recently downloaded this character
                    return null;
                }

                string data = DownloadHtml.GetHtmlFromUrl(new Uri(string.Format(InfoFormat, character.Server, character.Name, character.Region)));

                if ( !string.IsNullOrEmpty(data))
                {

                    Directory.CreateDirectory(string.Format(DirectoryFormat, character.Region, character.Server, character.Name));
                    File.WriteAllText(infoPath,
                    data
                    );

                    foreach(string achivementId in achievementPages)
                    {
                        string achievementPath = string.Format(AchievementPageFormat,character.Region, character.Server, character.Name, achivementId);
                        if (!File.Exists(achievementPath) || File.GetLastWriteTime(achievementPath) < MaxAge)
                        {
                            string achData = DownloadHtml.GetHtmlFromUrl(new Uri(string.Format(AchievementFormat, character.Server, character.Name, character.Region, achivementId)));
                            if (!string.IsNullOrEmpty(data))
                            {
                                File.WriteAllText(achievementPath, achData);
                            }
                        }
                    }    
                }   
            }

            return null;
        }
    }
}
