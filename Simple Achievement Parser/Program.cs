using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.IO;
using System.Text.RegularExpressions;
using Amib.Threading;
using System.Threading;
using AchievementSherpa.Business;
using AchievementSherpa.Data.MongoDb;
using AchievementSherpa.Business.Parsers;
using AchievementSherpa.PageParser;
using AchievementSherpa.Business.Services;
using StructureMap;
using AchievementSherpa.WowApi;

namespace AchievementSherpa.Business
{
    class Program
    {
        static readonly string GuildFileName = "guilds.dat";
        static bool parseAchivements = false;
        static bool parseGuilds = false;
        private static bool restrictToKilrogg = true;
        static int maxDaysOld = 30;
        private static bool rankAchievements = true;
        private static bool deleteCharacters = false;
        private static bool deleteAchievements = false;
        
        static void Main(string[] args)
        {
            BootStrapper.Configure();

            // TODO : Read flags from the command line
            try
            {
                IAchievementRepository achievementRepository = new MongoAchievementRepository();

                // Parse new achievements from www.wowhead.com if the parse achievement flag is set
                if (parseAchivements)
                {
                    // Remove achievements first
                    if (deleteAchievements)
                    {
                        achievementRepository.DeleteAllAchievements();
                    }
                    AchievementListParser achievementParser = new AchievementListParser(achievementRepository);

                    achievementParser.Parse(true);
                    WowHeadAchievementParser parser = new WowHeadAchievementParser(achievementRepository);

                    foreach (Achievement achievement in achievementRepository.FindAll())
                    {
                        parser.CheckForSeriesOfCriteria(achievement, false);
                    }

                    //parser.Parse(true);
                   //return;
                }

                // Get a list of guilds to use
                IList<Guild> guildsToParseSource = GetListOfGuilds(parseGuilds);
                int guildCounter = 1;

                ICharacterService characterService = ObjectFactory.GetInstance<ICharacterService>();
                ICharacterRepository characterRepository = ObjectFactory.GetInstance<ICharacterRepository>();
                IGuildParser guildParser = ObjectFactory.GetInstance<IGuildParser>();

                // Remove all characters to reset achivement rankings
                if (deleteCharacters)
                {
                    characterRepository.DeleteAllCharacters();
                }

                if (rankAchievements)
                {
                    MongoAchievementService service = new MongoAchievementService();
                    service.RankAchievements();
                }

                IList<Guild> guildsToParse = guildsToParseSource;
                if (restrictToKilrogg)
                {
                    guildsToParse = guildsToParseSource.Where(g => string.Compare(g.Server, "Kilrogg", true) == 0).ToList();
                }
                

                foreach (Guild guild in guildsToParse)
                {
                    
                    IEnumerable<Character> roster = guildParser.ParserRoster("us", guild.Server, guild.Name).OrderByDescending( c=> c.CurrentPoints );

                    Console.WriteLine("{0} of {1} ({2} of {3})", guild.Name, guild.Server, guildCounter, guildsToParse.Count);

                    int counter = 1;
                    foreach (Character character in roster)
                    {
                        if (character.Level < 85)
                        {
                            // Skip low level characters
                            continue;
                        }
                        try
                        {
                            
                            Character existing = characterRepository.FindCharacter(character);
                            if (existing == null || DateTime.Now.Subtract(existing.LastParseDate.Value).Days >= maxDaysOld)
                            {
                                Console.WriteLine("\t{0} [{1} of {2}]",
                                character.Name, counter, roster.Count());
                                characterService.UpdateCharacterDetails(character.Server, character.Name, character.Region);
                            }
                            else
                            {
                                Console.WriteLine("\t{0} [{1} of {2}] SKIPPED",
                                character.Name, counter, roster.Count());
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("\tFAILED Parsing {0} {1}", character.Name, ex);
                        }
                        finally
                        {
                            counter++;
                        }
                    }
                    guildCounter++;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadLine();
            }

        }

        private static IList<Guild> GetListOfGuilds(bool downloadNewList)
        {
            // Redownload the guild list if requested or if we do not have a file or existing guilds
            if (downloadNewList || !File.Exists(GuildFileName))
            {
                BinarySerializerHelper.Serialize(new ProgressFinder().Parse(), GuildFileName);
            }

            return BinarySerializerHelper.Deserialize(GuildFileName) as IList<Guild>;
        }

    }
}
