using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AchievementSherpa.Business.Services;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using AchievementSherpa.Business;
using System.Configuration;

namespace AchievementSherpa.Data.MongoDb
{
    public class MongoAchievementService : AchievementService
    {
        private readonly string AchievementUsageCollectionName = "AchievementUsage";
        private MongoDatabase _database;
        private BsonJavaScript _mapFunction;
        private BsonJavaScript _reduceFunction;


        public MongoAchievementService()
            : base(new MongoAchievementRepository())
        {
            _mapFunction = new BsonJavaScript(@"function()
            {
                this.Achievements.forEach(function(achieved)
                    {
                        emit(achieved.AchievementId, {count : 1 });
                    });   
            }");


            _reduceFunction = new BsonJavaScript(@"function(key, values)
            {
                var total = 0;
                for ( var i =0; i < values.length; i++)
                {
                    total += values[i].count;
                }
                return {count : total }; 
            }");
        }

        public override void RankAchievements()
        {
            string databaseName = "Achievements";


            if (ConfigurationManager.AppSettings["MongoDbName"] != null)
            {
                databaseName = ConfigurationManager.AppSettings["MongoDbName"];
            }
            // TODO : Get connection string from app.config
            // TODO : Get database name from app.config

            MongoServer server = MongoServer.Create(ConfigurationManager.ConnectionStrings["AchievementDatabase"].ConnectionString);

            //server.Settings.SocketTimeout = new TimeSpan(TimeSpan.TicksPerMinute * 5);

            _database = server.GetDatabase(databaseName);
            

            if (_database.CollectionExists(AchievementUsageCollectionName))
            {
                _database.DropCollection(AchievementUsageCollectionName);
            }


            MongoCollection<Character> characterCollection = _database.GetCollection<Character>(MongoCharacterRepository.CollectionName);
           
            characterCollection.MapReduce(_mapFunction, _reduceFunction, MapReduceOptions.SetOutput(AchievementUsageCollectionName));

            MongoCollection<BsonDocument> ranking = _database.GetCollection(AchievementUsageCollectionName);
            IList<BsonDocument> rankings = ranking.FindAll().SetSortOrder(SortBy.Descending("value.count")).ToList();
            int maxRank = 0;
            for (int i = 0; i < rankings.Count; i++)
            {
                string achievementId = rankings[i]["_id"].AsString;
                Achievement achievement = AchievementRepository.FindByAchievementId(achievementId);
                if (achievement != null)
                {
                    achievement.Rank = i + 1;
                    maxRank++;
                    AchievementRepository.Save(achievement);
                }
            }

            var unrankedAchievements = AchievementRepository.FindAll().Where(a => a.Rank == 0);
            foreach (Achievement unrankedAchievement in unrankedAchievements)
            {
                unrankedAchievement.Rank = int.MaxValue;
                AchievementRepository.Save(unrankedAchievement);
            }

        }
    }
}
