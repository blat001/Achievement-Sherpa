using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using AchievementSherpa.Business;
using AchievementSherpa.Business.Parsers;

namespace AchievementSherpa.PageParser
{
    public class GuildParser : ParserBase, IGuildParser
    {

        public IEnumerable<Character> ParserRoster(string region, string server, string name)
        {
            IList<Character> members = new List<Character>();

            string guildRosterUrl = string.Format("http://{0}.battle.net/wow/en/guild/{1}/{2}/roster?page={{0}}", region, server, name);

            bool foundMembers = true;
            int page = 1;

            while (foundMembers)
            {
                string guildPage = string.Format(guildRosterUrl, page);

                HtmlDocument document = DownloadPage(guildPage);

                HtmlNodeCollection roster = document.DocumentNode.SelectNodes("//td[@class='name']");
                if (roster != null)
                {

                    foreach (HtmlNode member in roster)
                    {
                        members.Add(new Character(member.InnerText, server, region) { Guild = name });
                    }
                }
                else
                {
                    foundMembers = false;
                }
                page++;
            }
            ///simple
            ///
            return members;
        }
    }
}
