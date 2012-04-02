using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AchievementSherpa.Business.Parsers;

using WowDotNetAPI;
using WowDotNetAPI.Models;

namespace AchievementSherpa.WowApi
{
    public class WowApiGuildParser : IGuildParser
    {
        public IEnumerable<Business.Character> ParserRoster(string region, string server, string name)
        {
            WowExplorer explorer = new WowExplorer(Region.US);
            try
            {

                Guild guildMembers = explorer.GetGuild(server, name, GuildOptions.GetMembers | GuildOptions.GetAchievements);


                if (guildMembers == null)
                {
                    return new List<Business.Character>();
                }
                return guildMembers.Members.Select(m => new Business.Character(m.Character.Name, server, region) { CurrentPoints = m.Character.AchievementPoints, Level = m.Character.Level });
            }
            catch (Exception ex)
            {
                return new List<Business.Character>();
            }

        }
    }
}
