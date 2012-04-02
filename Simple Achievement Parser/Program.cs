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

namespace AchievementSherpa.Business
{
    class Program
    {
        // /wow/en/character/kilrogg/debz/achievement#92
        // /wow/en/character/kilrogg/debz/achievement#169:170

        static readonly string GuildFileName = "guilds.dat";
        static bool parseAchivements = false;
        static bool parseGuilds = false;
        private static bool restrictToKilrogg = true;
        static int maxDaysOld = 30; 
        
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
                    WowHeadAchievementParser parser = new WowHeadAchievementParser(achievementRepository);
                    parser.Parse(true);
                   //return;
                }

                // Get a list of guilds to use
                IList<Guild> guildsToParseSource = GetListOfGuilds(parseGuilds);
                int guildCounter = 1;

                ICharacterService characterService = ObjectFactory.GetInstance<ICharacterService>();
                ICharacterRepository characterRepository = ObjectFactory.GetInstance<ICharacterRepository>();
                IGuildParser guildParser = ObjectFactory.GetInstance<IGuildParser>();

                IList<Guild> guildsToParse = guildsToParseSource;
                if (restrictToKilrogg)
                {
                    guildsToParse = guildsToParseSource.Where(g => string.Compare(g.Server, "Kilrogg", true) == 0).ToList();
                }
                

                foreach (Guild guild in guildsToParse)
                {
                    
                    IEnumerable<Character> roster = guildParser.ParserRoster("us", guild.Server, guild.Name).OrderByDescending( c=> c.CurrentPoints );

                    Console.WriteLine("Parsing Guild {0} of {1} {2} of {3} guilds parsed", guild.Name, guild.Server, guildCounter, guildsToParse.Count);

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
                            Console.WriteLine("\t{0} [{1} of {2}]",
                                character.Name, counter, roster.Count());
                            Character existing = characterRepository.FindCharacter(character);
                            if (existing == null || DateTime.Now.Subtract(existing.LastParseDate.Value).Days >= maxDaysOld)
                            {
                                characterService.UpdateCharacterDetails(character.Server, character.Name, character.Region);
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
