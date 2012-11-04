using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StructureMap.Configuration.DSL;
using AchievementSherpa.Business;
using AchievementSherpa.Data.MongoDb;
using AchievementSherpa.Business.Services;
using AchievementSherpa.Business.Parsers;
using AchievementSherpa.PageParser;
using AchievementSherpa.WowApi;

namespace Web_UI
{
    public class ScanningRegistry : Registry
    {

        public ScanningRegistry()
        {
            Scan(x =>
            {
                x.LookForRegistries();

                
            });

            For<ICharacterRepository>().Use<MongoCharacterRepository>();
            For<IAchievementRepository>().Use<MongoAchievementRepository>();
            For<IWorldRepository>().Use<MongoWorldRepository>();
            For<IAchievementService>().Use<AchievementService>();
            For<ICharacterService>().Use<CharacterService>();
            For<ICharacterParser>().Use<WowApiCharacterParser>();
        }

    }
}