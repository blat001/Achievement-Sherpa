using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AchievementSherpa.Business;

namespace AchievementSherpa.Business
{
    public interface IAchievementRepository
    {

        
        IList<Achievement> GetAllInSeries(AchievementSeries series);

        Achievement Find(int blizzardId);

        void Save(Achievement achievement);

        IList<Achievement> FindAll();

        void DeleteAllAchievements();
    }
}
