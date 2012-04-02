using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Amib.Threading;
using AchievementSherpa.Business;
using AchievementSherpa.Business.Parsers;

namespace AchievementSherpa.PageParser

{
    public class CharacterParser : ParserBase, ICharacterParser
    {
        private Regex achievementLinkPattern = new Regex(@"(?<url>.*/achievement)#(?<categoryid>\d+)$");
        private Regex achievementSubLinkPattern = new Regex(@"(?<url>.*/achievement)#\d+:(?<categoryid>\d+)");
        private AchievementParser _achievementParser;
        private Character _current;

        public CharacterParser(IAchievementRepository achievementRepository)
        {
            _achievementParser = new AchievementParser(achievementRepository);
        }

        

        private IList<HtmlDocument> pagesToParse = new List<HtmlDocument>();
        public Character Parse(Character character, bool force)
        {
            stopwatch.Reset();
            Stopwatch watch = new Stopwatch();
            watch.Start();


            _current = character;


            // Only parse the character once a day unless forced
            if (character.LastParseDate.HasValue &&
                character.LastParseDate.Value.AddDays(7) > DateTime.Now && !force)
            {
                return character;
            }

            ParseCharacterInformation(character);

            //character.Achievements.Clear();

            string mainAchievementPageUrl = string.Format("http://{2}.battle.net/wow/en/character/{0}/{1}/achievement", character.Server, character.Name, character.Region);
            HtmlDocument doc = DownloadPage(mainAchievementPageUrl);
            List<AchievedAchievement> achievements = new List<AchievedAchievement>();

            ProcessPageForAchievements(doc.DocumentNode, character);
            pagesToParse = new List<HtmlDocument>();
            IList<string> extraPages = FindSubAchievementPages(doc.DocumentNode);

            SmartThreadPool pool = new SmartThreadPool();
            foreach (string pageUrl in extraPages)
            {
                pool.QueueWorkItem(new WorkItemCallback(ProcessPageOnNewThread), pageUrl);
            }

            pool.Start();
            pool.WaitForIdle();

            try
            {
                foreach (HtmlDocument page in pagesToParse)
                {
                    ProcessPageForAchievements(page.DocumentNode, character);
                }
            }
            catch (IndexOutOfRangeException)
            {
            }
            character.LastParseDate = DateTime.Now;
            character.CurrentPoints = character.TotalAchievementPoints;
            watch.Stop();


            return character;
        }

        void ParseCharacterInformation(Character character)
        {

            string mainPageUrl = string.Format("http://{2}.battle.net/wow/en/character/{0}/{1}/simple", character.Server, character.Name, character.Region);
            HtmlDocument doc = DownloadPage(mainPageUrl);

            character.Guild = GetValueAsString(doc.DocumentNode, "//div[@class='guild']");
            character.Level = GetValueAsInt32(doc.DocumentNode, "//span[@class='level']/strong");
            character.Class = GetValueAsString(doc.DocumentNode, "//a[@class='class']");
            character.Race = GetValueAsString(doc.DocumentNode, "//a[@class='race']");

            HtmlNode sideNode = doc.DocumentNode.SelectSingleNode("//div[@id='profile-wrapper']");
            if (sideNode != null)
            {
                if (sideNode.Attributes["class"].Value.Contains("horde"))
                {
                    character.Side = 1;
                }
                else
                {
                    character.Side = 2;
                }
            }

            //guild
        }

        object ProcessPageOnNewThread(object url)
        {
            string pageUrl = url as string;

            if (pageUrl != null)
            {
                pagesToParse.Add(DownloadPage(pageUrl));

            }
            return null;
        }


        IList<string> FindSubAchievementPages(HtmlNode page)
        {
            IList<string> achievementLinks = new List<string>();

            HtmlNodeCollection allLinks = page.SelectNodes("//a");
            if (allLinks == null)
            {
                return achievementLinks;
            }
            foreach (HtmlNode individualLink in allLinks)
            {


                if (individualLink.Attributes["href"] != null)
                {
                    string href = individualLink.Attributes["href"].Value;
                    Match match = achievementLinkPattern.Match(href);
                    Match match2 = achievementSubLinkPattern.Match(href);

                    string link = string.Empty;
                    if (match.Success)
                    {
                        link = CreateLinkFromMatch(match);
                    }
                    else if (match2.Success)
                    {
                        link = CreateLinkFromMatch(match2);
                    }
                    else
                    {
                        if (link.Contains("achievement"))
                        {
                            Console.WriteLine("FAILED Link : {0}", link);
                        }
                    }

                    if (!string.IsNullOrEmpty(link) && !achievementLinks.Contains(link))
                    {
                        achievementLinks.Add(link);
                    }
                }
            }


            return achievementLinks;
        }

        private string CreateLinkFromMatch(Match match)
        {
            string link = string.Format("http://us.battle.net{0}/{1}",
    match.Groups["url"].Value,
    match.Groups["categoryid"].Value);
            return link;
        }


        private void ProcessPageForAchievements(HtmlNode page, Character character)
        {
            HtmlNode achievementList = page.SelectSingleNode("//div[@class='container']");

            if (achievementList != null)
            {
                ProcessAchievementList(achievementList, character);
            }

        }



        private void ProcessAchievementList(HtmlNode achievementList, Character character)
        {

            HtmlNodeCollection achievementNodes = achievementList.SelectNodes("//li");
            if (achievementNodes != null)
            {

                foreach (HtmlNode individualAchievement in achievementNodes)
                {
                    if (IsAchievementContainer(individualAchievement))
                    {
                        _achievementParser.Parse(individualAchievement, character);

                    }
                }
            }

        }

        private static bool IsAchievementContainer(HtmlNode individualAchievement)
        {
            return individualAchievement.Attributes["class"] != null &&
                                    individualAchievement.Attributes["class"].Value.Contains("achievement") &&
                                    individualAchievement.Attributes["data-id"] != null;
        }
    }
}
