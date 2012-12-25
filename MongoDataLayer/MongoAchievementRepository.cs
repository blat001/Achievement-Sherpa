using AchievementSherpa.Business;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Collections.Generic;
using System.Linq;

namespace AchievementSherpa.Data.MongoDb
{
    public class MongoAchievementRepository : BaseRepository<Achievement>, IAchievementRepository
    {

        public MongoAchievementRepository()
            : base("Achievements")
        {
        }

        public void DeleteAllAchievements()
        {
            Collection.RemoveAll();
        }

        public Achievement FindByAchievementId(string achievementId)
        {
            return Collection.FindOne(new QueryDocument("_id", achievementId));
        }

        public IList<Achievement> GetAllInSeries(AchievementSeries series)
        {
            IDictionary<string, object> param = new Dictionary<string, object>();
            BsonArray achivementIds = new BsonArray();
            achivementIds.AddRange(series.AchievementIds);

            return Collection.Find(Query.In("_id", achivementIds)).ToList();
        }

        public AchievementSherpa.Business.Achievement Find(int blizzardId)
        {
            return Collection.FindOne(new QueryDocument("BlizzardID", blizzardId));
        }

        public void Save(AchievementSherpa.Business.Achievement achievement)
        {
            if (achievement._id == null)
            {
                achievement._id = ObjectId.GenerateNewId().ToString();
            }
            Collection.Save(achievement);

            if (achievement.IsPartOfSeries)
            {
                IList<Achievement> series = GetAllInSeries(achievement.Series);
                foreach (Achievement seriesAchievement in series)
                {
                    Collection.Save(seriesAchievement);
                }
            }
        }

        public IList<AchievementSherpa.Business.Achievement> FindAll()
        {
            return Collection.FindAll().ToList();
        }
    }
}
