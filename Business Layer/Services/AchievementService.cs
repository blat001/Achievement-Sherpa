using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace AchievementSherpa.Business.Services
{
    public class AchievementService : IAchievementService
    {
        private IAchievementRepository _achievementRepository;
        private IDictionary<int, Achievement> _achievementCache = new Dictionary<int, Achievement>();

        public AchievementService(IAchievementRepository achievementRepository)
        {
            _achievementRepository = achievementRepository;
        }


        protected virtual IAchievementRepository AchievementRepository
        {
            get
            {
                return _achievementRepository;
            }
        }

        public virtual void RankAchievements()
        {
        }


        public Achievement FindAchivementByBlizzardId(int blizzardId)
        {
            if (_achievementCache.ContainsKey(blizzardId))
            {
                return _achievementCache[blizzardId];
            }

            Achievement a = _achievementRepository.Find(blizzardId);
            _achievementCache[blizzardId] = a;
            return a;
        }

        public virtual IList<Achievement> GetRecommendedAchievements(Character character)
        {
            var achievements = AchievementRepository.FindAll().Where(a => (a.Side == character.Side || a.Side == Achievement.BothSides) && a.Rank != null).ToList();


            IList<Achievement> recommended = new List<Achievement>();
            int counter = 0;
            IList<AchievedAchievement> existingAchievements = character.Achievements.ToList();
            while (counter < achievements.Count())
            {
                if (existingAchievements.FirstOrDefault(a => a.BlizzardID == achievements[counter].BlizzardID) == null)
                {
                    if (achievements[counter].Category != Achievement.FeatsOfStrengthCategory)
                    {
                        recommended.Add(achievements[counter]);
                    }
                }
                counter++;
            }

            return CleanupRecommendedAchievements(recommended, character).OrderBy(a => a.Rank).ToList();
        }

        private IList<Achievement> CleanupRecommendedAchievements(IList<Achievement> recommended, Character character)
        {
            List<Achievement> cleanedAchievements = new List<Achievement>();
            cleanedAchievements.AddRange(recommended);
            int numberRemoved = 0;
            foreach (Achievement achievement in recommended)
            {
                // Check to see if we have already removed that achievement
                if (cleanedAchievements.Where(a => a._id == achievement._id).FirstOrDefault() == null)
                {
                    continue;
                }
                if (achievement.Category == Achievement.FeatsOfStrengthCategory)
                {
                    cleanedAchievements.Remove(achievement);
                }
                Debug.WriteLine(string.Format("Processing Achievment : {0}", achievement));
                if (achievement.IsPartOfSeries)
                {
                    // remove all the achivements from a series
                    foreach (int seriesAchievement in achievement.Series.AchievementIds)
                    {
                        Achievement achievementToRemove = cleanedAchievements.Where(a => a.BlizzardID == seriesAchievement).FirstOrDefault();
                        if (achievementToRemove != null)
                        {
                            Debug.WriteLine(string.Format("\tRemoving Achievment : {0}", achievementToRemove));
                            numberRemoved++;
                            cleanedAchievements.Remove(achievementToRemove);
                        }
                    }

                    // Get Players highest achievement in series
                    Achievement nextAchievementInSeries = null;
                    int highestAchievementId = character.GetHighestAchievementInSeries(achievement.Series);
                    
                    if (highestAchievementId != null)
                    {
                        int nextAchievment = 0;
                        for (int i = 0; i < achievement.Series.AchievementIds.Count; i++)
                        {
                            if (achievement.Series.AchievementIds.ElementAt(i) == highestAchievementId)
                            {
                                nextAchievment = i + 1;
                            }
                        }

                        if (nextAchievment < achievement.Series.AchievementIds.Count)
                        {
                            //nextAchievementInSeries = _achievementRepository.FindByAchievementId(achievement.Series.AchievementIds.ElementAt(nextAchievment));
                        }
                    }
                    else{
                        //nextAchievementInSeries = _achievementRepository.FindByAchievementId(achievement.Series.AchievementIds[0]);
                    }

                    Debug.WriteLine(string.Format("\tAdding Achievment : {0}", nextAchievementInSeries));
                    if (nextAchievementInSeries != null)
                    {
                        cleanedAchievements.Add(nextAchievementInSeries);
                        numberRemoved--;
                    }
                    
                }
            }
            return cleanedAchievements;
        }
    }
}
