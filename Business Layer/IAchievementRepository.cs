using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AchievementSherpa.Business;

namespace AchievementSherpa.Business
{
    public interface IAchievementRepository
    {

        Achievement FindByAchievementId(string achievementId);

        IList<Achievement> GetAllInSeries(AchievementSeries series);

        Achievement Find(string blizzardId);

        void Save(Achievement achievement);

        IList<Achievement> FindAll();
    }
}
