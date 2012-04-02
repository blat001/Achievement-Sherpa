using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AchievementSherpa.Data.MongoDb;

namespace AchievementSherpa.Business
{
    public class UpdateAchievementRanking
    {



        public void Process()
        {

            MongoAchievementService service = new MongoAchievementService();
            service.RankAchievements();

        }
    }
}
