using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AchievementSherpa.Business;

namespace AchievementSherpa.Business.Services
{
    public interface IAchievementService
    {
        IList<Achievement> GetRecommendedAchievements(Character character);

        void RankAchievements();

        Achievement FindAchivementByBlizzardId(int blizzardId);
    }
}
