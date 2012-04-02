using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using AchievementSherpa.PageParser;

namespace AchievementSherpa.Business
{
    public class ProgressFinder : ParserBase
    {

        public IList<Guild> Parse()
        {

            //http://wow.guildprogress.com/US/Kilrogg/Cataclysm

            
            List<Guild> guilds = new List<Guild>();

            foreach (string serverName in GetServers())
            {
                if (serverName == "Kilrogg")
                {
                    Console.WriteLine("About to start Kilrogg");
                }
                
                IList<Guild> guildsOnServer = ParserGuilds(serverName);
                Console.WriteLine("Processed {0} guilds on server {1}",
                    guildsOnServer.Count, serverName);

                guilds.AddRange(guildsOnServer);
            }

            

            return guilds;

        }

        private IList<string> GetServers()
        {
            //http://wow.guildprogress.com/server-list/US
            IList<string> usServerNames = new List<String>();
            HtmlDocument doc = DownloadPage(@"http://wow.guildprogress.com/server-list/US", true);
            HtmlNodeCollection guildNodess = doc.DocumentNode.SelectNodes("//table[@class='chart serverChart']//a[contains(@href,'US')]");
            if (guildNodess != null)
            {
                foreach (HtmlNode guild in guildNodess)
                {
                    string server = GetValueAsString(guild, ".");
                    if (server != "Sort By Progression")
                    {
                        usServerNames.Add(server);
                    }
                }
            }
            return usServerNames;

        }


        private IList<Guild> ParserGuilds(string serverName)
        {
            IList<Guild> guilds = new List<Guild>();
            int counter = 1;

            bool shouldContinue = true;
            while (shouldContinue)
            {

                string page = string.Format("http://wow.guildprogress.com/US/{1}/Cataclysm/{0}", counter, serverName);
                if (counter == 1)
                {
                    page = string.Format("http://wow.guildprogress.com/US/{0}/Cataclysm", serverName);
                }
                HtmlDocument doc = DownloadPage(page, true);
                HtmlNodeCollection guildNodess = doc.DocumentNode.SelectNodes("//td[@class='guild']");
                if (guildNodess != null)
                {
                    foreach (HtmlNode guild in guildNodess)
                    {
                        Guild guildToParse = new Guild(serverName, GetValueAsString(guild, "./a"));
                        guilds.Add(guildToParse);
                    }
                }
                else
                {
                    shouldContinue = false;
                }
                counter++;
            }

            return guilds;
        }
    }
}
