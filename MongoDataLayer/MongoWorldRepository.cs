using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AchievementSherpa.Business;
using AchievementSherpa.Business;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace AchievementSherpa.Data.MongoDb
{
    public class MongoWorldRepository : MongoCharacterRepository, IWorldRepository
    {

        public IList<string> GetServers()
        {
            return Collection.FindAll().Select(c => c.Server).Distinct().ToList();
        }

        public IList<Character> GetCharactersInGuild(string region, string server, string guild)
        {
            QueryDocument query = new QueryDocument();
            query.Add("Server", server);
            query.Add("Region", region);
            query.Add("Guild", guild);
            return Collection.Find(query).OrderBy(c => c.CurrentPoints).ToList();
        }


        public int NumberCharactersOnServer(string region, string server)
        {
            QueryDocument query = new QueryDocument();
            query.Add("Server", server);
            query.Add("Region", region);
            return Collection.Find(query).Count();
        }
        public IList<Character> ListCharactersOnServerByPoints(string region, string server, int start, int limit)
        {
            QueryDocument query = new QueryDocument();
            query.Add("Server", server);
            query.Add("Region", region);
            

            SortByBuilder sortBy = new SortByBuilder();
            sortBy.Descending("CurrentPoints");

            return Collection.Find(query).SetSortOrder(sortBy).SetSkip(start).SetLimit(limit).OrderByDescending(c => c.CurrentPoints).ToList();
        }
    }
}
