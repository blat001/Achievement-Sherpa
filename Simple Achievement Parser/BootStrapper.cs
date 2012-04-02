using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap;
using AchievementSherpa.Data.MongoDb;
using AchievementSherpa.Business.Parsers;
using AchievementSherpa.PageParser;
using AchievementSherpa.Business.Services;
using AchievementSherpa.WowApi;

namespace AchievementSherpa.Business
{
    public class BootStrapper
    {
        public static void Configure()
        {
            ObjectFactory.Initialize(x =>
            {
                x.For<ICharacterRepository>().Use<MongoCharacterRepository>();
                x.For<IAchievementRepository>().Use<MongoAchievementRepository>();
                x.For<ICharacterParser>().Use<WowApiCharacterParser>();
                x.For<ICharacterService>().Use<CharacterService>();
                x.For<IAchievementService>().Use<AchievementService>();
                x.For<IGuildParser>().Use<WowApiGuildParser>();
            });
        }
    }
}
