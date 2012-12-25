using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using AchievementSherpa.Business;

namespace AchievementSherpa.PageParser
{
    public class AchievementParser : ParserBase
    {
        // "Tooltip.show(this, '#ach-tooltip-126')
        Regex parseTooltip = new Regex(@"#ach-tooltip-(?<achievementid>\d+)'", RegexOptions.Compiled);
        IAchievementRepository _service;

        public AchievementParser(IAchievementRepository achievementRepository)
        {
            _service = achievementRepository;
        }

        public IList<AchievedAchievement> Parse(HtmlNode achievementNode, Character character)
        {
            List<Achievement> achievements = new List<Achievement>();
            List<AchievedAchievement> actualAchievements = new List<AchievedAchievement>();
            if (achievementNode == null)
            {
                throw new ArgumentNullException("achievementNode");
            }

            string achievementId = achievementNode.Attributes["data-id"].Value;
            int blizzardId = 0;
            int.TryParse(achievementId, out blizzardId);

            Achievement achievement = _service.Find(blizzardId);

            if (achievement == null)
            {
                Console.WriteLine("Found unknown achievement {0}", achievementId);
            }



            if (!achievementNode.Attributes["class"].Value.Contains("locked"))
            {
                //
                DateTime whenAchieved = GetValueAsDateTime(achievementNode, "./div[@class='points-big border-8']/span[@class='date']");

                if (!character.HasAchieved(achievement))
                {
                    character.AddNewAchivement(whenAchieved, achievement);
                }

                // check to see if achievement is part of a series 
                if (achievement != null && achievement.Series != null)
                {
                    // get all achievements under the one we have displayed



                    foreach (Achievement seriesAchievement in _service.GetAllInSeries(achievement.Series).Where(a => a.SeriesOrder < achievement.SeriesOrder))
                    {
                        if (!character.HasAchieved(achievement))
                        {
                            character.AddNewAchivement(whenAchieved.AddDays(-1), seriesAchievement);
                        }
                        
                    }
                }      
            }
            return actualAchievements;
        }

     


        private IList<Achievement> ParseSubAchievements(Achievement achievement, HtmlNode subAchievementNode)
        {

            IList<Achievement> subAchievements = new List<Achievement>();
            HtmlNodeCollection achievements = subAchievementNode.SelectNodes("./li");
            if (achievements != null)
            {
                foreach (HtmlNode subNode in achievements)
                {
                    // TODO : Parse out achievement id
                    if ( subNode.Attributes["onmousemove"] != null )
                    {
                        Match match = parseTooltip.Match(subNode.Attributes["onmousemove"].Value);
                        if ( match.Success )
                        {
                            string subAchievementId = match.Groups["achievementid"].Value;
                            int blizzardId = 0;
                            int.TryParse(subAchievementId, out blizzardId);
                            Achievement subAchievement = _service.Find(blizzardId);
                            if (subAchievement == null)
                            {
                                subAchievement = new Achievement() { BlizzardID = blizzardId };
                                subAchievement.Name = GetValueAsString(subNode, ".//h3");
                                subAchievement.Description = GetValueAsString(subNode, ".//div[@class='color-tooltip-yellow']");
                                subAchievement.Points = GetValueAsInt32(subNode, ".//span[@class='points border-3']");
                                _service.Save(subAchievement);
                            }
                            achievement.Points = achievement.Points - subAchievement.Points;
                            subAchievements.Add(subAchievement);
                        }
                    }
                }
            }

            return subAchievements;

        }


      

    }
}
