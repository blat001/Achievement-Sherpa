using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using HtmlAgilityPack;
using AchievementSherpa.Business;

namespace AchievementSherpa.Business
{
    public class WowHeadAchievementParser
    {

        IAchievementRepository _achievementRepository;

        public WowHeadAchievementParser(IAchievementRepository achievementRepository)
        {
            _achievementRepository = achievementRepository;
        }



        public void Parse(bool download)
        {
            
            IList<string> achievementLinks = new List<string>();

            achievementLinks.Add("http://www.wowhead.com/achievements=1.92");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.96");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.96.14861");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.96.15081");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.96.14862");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.96.14863");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.96.15070");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.97");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.97.14777");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.97.14778");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.97.14779");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.97.14780");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.97.15069");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.95");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.95.165");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.95.14801");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.95.14802");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.95.14803");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.95.14804");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.95.14881");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.95.14901");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.95.15003");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.95.15073");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.95.15074");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.95.15075");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.95.15092");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.168");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.168.14808");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.168.14805");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.168.14806");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.168.14922");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.168.15067");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.168.15068");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.169");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.169.170");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.169.171");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.169.172");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.169.15071");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.201");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.201.14865");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.201.14864");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.201.14866");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.201.15072");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.155");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.155.160");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.155.187");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.155.159");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.155.163");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.155.161");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.155.162");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.155.158");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.155.14981");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.155.156");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.155.14941");
            achievementLinks.Add("http://www.wowhead.com/achievements=1.81");

            if (download)
            {
                foreach (string link in achievementLinks)
                {
                    IList<Achievement> achievements = ParseAchievementsOnPage(link);

                    if (achievements != null)
                    {

                        foreach (Achievement achievement in achievements)
                        {

                            Achievement savedAchievement = _achievementRepository.Find(achievement.BlizzardID);

                            if (savedAchievement == null)
                            {
                                _achievementRepository.Save(achievement);
                            }
                            else
                            {
                                // Update only
                                savedAchievement.Side = achievement.Side;
                                savedAchievement.Icon = achievement.Icon;
                                _achievementRepository.Save(savedAchievement);
                            }
                        }
                        Console.WriteLine("URL : {0} returned {1} achievements", link, achievements.Count);
                    }
                    else
                    {
                        Console.WriteLine("URL : {0} returned no achievements", link);
                    }
                }
            }

            if (checkSeries)
            {
                // now lets find achievement series 
                foreach (Achievement achievement in _achievementRepository.FindAll())
                {
                    CheckForSeriesOfCriteria(achievement, true);
                }
            }

        }


        Regex seriesLinkRegex = new Regex(@"achievement=(?<achievementid>\d+)");
        private bool checkSeries = true;
        public void CheckForSeriesOfCriteria(Achievement achievement, bool checkCriteria)
        {
            if (achievement.Series != null)
            {
                return;
            }
            string html = DownloadHtml.GetHtmlFromUrl(new Uri("http://www.wowhead.com/achievement=" + achievement.BlizzardID));
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);

            HtmlNode seriesNode = document.DocumentNode.SelectSingleNode("//table[@class='series']");
            HtmlNode criteriaNode = document.DocumentNode.SelectSingleNode("//h3[contains(text(),'Criteria')]");

            bool seriesOrMeta = false; 

            if (seriesNode != null)
            {
                seriesOrMeta = true;
                // First check if this quest is already in a series 
                if (achievement.Series == null)
                {

                    Console.WriteLine("Adding Achivement {0} to a new series", achievement.Name);
                    // lets fill in series
                    HtmlNodeCollection tableRows = seriesNode.SelectNodes(".//tr");
                    int order = 10;
                    AchievementSeries series = new AchievementSeries();
                    
                    foreach (HtmlNode row in tableRows)
                    {
                        HtmlNode linkNode = row.SelectSingleNode(".//a");
                        if (linkNode == null)
                        {
                            series.AddAchievementToSeries(achievement);
                            achievement.SeriesOrder = order;
                            Console.WriteLine("\t [{0}] Series includes Achievement {1}", order, achievement.Name);
                        }
                        else
                        {
                            Match m = seriesLinkRegex.Match(linkNode.Attributes["href"].Value);
                            if (m.Success)
                            {
                                string achivementid = m.Groups["achievementid"].Value;
                                int blizzardId = 0;
                                int.TryParse(achivementid, out blizzardId);
                                Achievement nextAchievement = _achievementRepository.Find(blizzardId);
                                if (nextAchievement != null)
                                {
                                    Console.WriteLine("\t [{0}] Series includes Achievement {1}", order, nextAchievement.Name);
                                    series.AddAchievementToSeries(nextAchievement);
                                    nextAchievement.SeriesOrder = order;
                                }
                            }
                        }

                        order += 10;
                    }
                }

                // We have a series, this achievement will be blanked out
            }
            if (checkCriteria && criteriaNode != null)
            {
                HtmlNodeCollection linkNodes = criteriaNode.SelectNodes("..//table[@class='iconlist']//a[contains(@href,'achievement')]");
                if (linkNodes != null)
                {
                    seriesOrMeta = true;
                    Console.WriteLine("Found Meta Achivement : {0}, adding sub achivements", achievement.Name);

                    if (achievement.SubAchievements == null)
                    {
                        achievement.SubAchievements = new List<int>();
                    }

                    achievement.Criteria.Clear();
                    foreach (HtmlNode linkNode in linkNodes)
                    {
                        Match m = seriesLinkRegex.Match(linkNode.Attributes["href"].Value);
                        if (m.Success)
                        {
                            string achivementid = m.Groups["achievementid"].Value;
                            int blizzardId = 0;
                            int.TryParse(achivementid, out blizzardId);
                            Achievement requiredAchievement = _achievementRepository.Find(blizzardId);
                            if (requiredAchievement != null)
                            {
                                achievement.SubAchievements.Add(requiredAchievement.BlizzardID);
                                Console.WriteLine("\t Criteria includes Achievement {0}", requiredAchievement.Name);
                            }
                        }
                    }
                }
            }

            if (!seriesOrMeta)
            {
                Console.WriteLine("Single achivement '{0}' for '{1}' points", achievement.Name, achievement.Points);
            }
            else
            {
                _achievementRepository.Save(achievement);
            }

        }



        private IList<Achievement> ParseAchievementsOnPage(string url)
        {
            Regex josnMatch = new Regex(@"data: (?<json>\[.+}\])}\);");
            Regex iconMatch = new Regex(@"_\[(?<achievementid>\d+)\]={name_enus:'[^']+',icon:'(?<icon>[^']+)'};");

            string html = DownloadHtml.GetHtmlFromUrl(new Uri(url));
            Match m = josnMatch.Match(html);
            if (josnMatch.IsMatch(html))
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                WowAchievement[] data = ser.Deserialize<WowAchievement[]>(m.Groups["json"].Value);

                IList<Achievement> achievements = data.Select(a => a.ToAchievement()).ToList();

                foreach (Match iconMatches in iconMatch.Matches(html))
                {
                    string achivementid = iconMatches.Groups["achievementid"].Value;

                    int blizzardId = 0;
                    int.TryParse(achivementid, out blizzardId);
                    string icon = iconMatches.Groups["icon"].Value;

                    if (achievements.FirstOrDefault(a => a.BlizzardID == blizzardId) != null)
                    {
                        achievements.FirstOrDefault(a => a.BlizzardID == blizzardId).Icon = icon;
                    }

                }

                return achievements;

            }

            return null;
        }

    }

    public class WowAchievement
    {
        public string category
        {
            get;
            set;
        }

        public string description
        {
            get;
            set;
        }

        public string id
        {
            get;
            set;
        }

        public string name
        {
            get;
            set;
        }

        public string points
        {
            get;
            set;
        }

        public string parentcat
        {
            get;
            set;
        }

        public string side
        {
            get;
            set;
        }

        public string type
        {
            get;
            set;
        }


        public int AchievementPoints
        {
            get
            {
                int value = 0;
                int.TryParse(points, out value);
                return value;
            }
        }

        public int SideNumber
        {
            get
            {
                int value = 3;
                if (int.TryParse(side, out value))
                {
                    if (value == 1)
                    {
                        value = 2;
                    }
                    else if (value == 2)
                    {
                        value = 1;
                    }
                }
                
                return value;
            }
        }
        public Achievement ToAchievement()
        {
            int blizzardId = 0;
            int.TryParse(id, out blizzardId);

            return new Achievement()
            {
                Name = name,
                Description = description,
                BlizzardID = blizzardId,
                Category = category,
                Points = AchievementPoints,
                ParentCategoryID = -1,
                Type = type,
                Side = SideNumber
            };
        }
    }
}
