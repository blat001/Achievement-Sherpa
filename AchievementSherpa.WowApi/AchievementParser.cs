using AchievementSherpa.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WowDotNetAPI;
using WowDotNetAPI.Models;

namespace AchievementSherpa.WowApi
{
    public class AchievementListParser
    {
        IAchievementRepository _achievementRepository;

        public AchievementListParser(IAchievementRepository achievementRepository)
        {
            _achievementRepository = achievementRepository;
        }


        public void Parse(bool download)
        {
            WowExplorer explorer = new WowExplorer(Region.US);

            IEnumerable<AchievementList> achievementList = explorer.GetAchievements();
            IList<Achievement> achievements = new List<Achievement>();

            foreach (AchievementList mainAchievementCategory in achievementList)
            {
                Console.WriteLine(mainAchievementCategory.Name);
                if (mainAchievementCategory.Categories != null)
                {
                    // First handle the sub categories
                    foreach (AchievementCategory subCategory in mainAchievementCategory.Categories)
                    {
                        ProcessAchievementList(subCategory.Achievements, subCategory.Name, subCategory.Id, mainAchievementCategory.Id);
                        Console.WriteLine("\t{0}", subCategory.Name);
                    }
                }

                ProcessAchievementList(mainAchievementCategory.Achievements, mainAchievementCategory.Name, mainAchievementCategory.Id, -1);

                
            }
        }

        private void ProcessAchievementList(IEnumerable<AchievementInfo> achievements, string categoryName, int categoryId, int parentCategoryId)
        {
            foreach (AchievementInfo achievementDetails in achievements)
            {
                Achievement achievement = _achievementRepository.Find(achievementDetails.Id);
                if (achievement == null)
                {
                    achievement = new Achievement();    
                }

                UpdateAchivementDetails(achievement, achievementDetails, categoryName, categoryId, parentCategoryId);
                _achievementRepository.Save(achievement);
            }
        }

        private Achievement CreateFromAchivementInfo(AchievementInfo achievementDetails, string category, int categoryId, int parentCategoryId)
        {
            Achievement achievement = new Achievement();
            UpdateAchivementDetails(achievement, achievementDetails, category, categoryId, parentCategoryId);
            return achievement; 
        }

        
        public void UpdateAchivementDetails(Achievement achievement, AchievementInfo achievementDetails, string category, int categoryId, int parentCategoryId)
        {
            achievement.BlizzardID = achievementDetails.Id;
            achievement.Name = achievementDetails.Title;
            achievement.Side = ReturnSide(achievementDetails.FactionId);
            achievement.Points = achievementDetails.Points;
            achievement.Icon = achievementDetails.Icon;
            achievement.Category = category;
            achievement.CategoryID = categoryId;
            achievement.ParentCategoryID = parentCategoryId;
            achievement.Description = achievementDetails.Description;
            foreach (AchievementCriteria criteria in achievementDetails.Criteria)
            {
                achievement.Criteria.Add(criteria.Id);
            }
        }


        public int ReturnSide(string factionId)
        {
            int value = 3;
            if (int.TryParse(factionId, out value))
            {
                return value;
            }

            return value;
        }
    }
}
