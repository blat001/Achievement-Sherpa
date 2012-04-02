using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simple_Achievement_Parser;
using Norm;
using System.IO;
using EFTracingProvider;

namespace Data_Library
{
    public class AchievementService
    {

        public Achievement FindAchievement(string id)
        {
            var db = DBFactory.CreateConnection();

            //get LINQ access to your Posts
            //Mongo created the database and collection on the fly!
            Achievement achievement = db.Achievements.FirstOrDefault(a => a.BlizzardID == id);
            return achievement;
        }

        public void AddNewAchievement(Achievement achievement)
        {
            UpdateAchievement(achievement);
        }



        public IList<Achievement> AllAchievements()
        {
            var db = DBFactory.CreateConnection();

            //get LINQ access to your Posts
            //Mongo created the database and collection on the fly!
            return db.Achievements.ToList();

        }



        public void UpdateAchievement(Achievement achievement)
        {

            var db = DBFactory.CreateConnection();

            Achievement existingAchievement = db.Achievements.FirstOrDefault(a => a.BlizzardID == achievement.BlizzardID);

            if (existingAchievement != null)
            {
                existingAchievement.Name = achievement.Name;
                existingAchievement.Description = achievement.Description;
                existingAchievement.Points = achievement.Points;

                achievement = existingAchievement;
            }
            else
            {
                db.Achievements.AddObject(achievement);
            }

            //EFTracingProviderConfiguration.LogToConsole = true;


            //db.SetTraceOutput(Console.Out);
              //  db.SaveChanges();
           
        }

    }

}