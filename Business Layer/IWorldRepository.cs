using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AchievementSherpa.Business;

namespace AchievementSherpa.Business
{
    public interface IWorldRepository
    {
        IList<string> GetServers();

        IList<Character> GetCharactersInGuild(string region, string server, string guild);


        int NumberCharactersOnServer(string region, string server);

        IList<Character> ListCharactersOnServerByPoints(string region, string server, int start, int limit);
    }
}
