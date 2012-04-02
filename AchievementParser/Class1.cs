using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WowDotNetAPI;
using WowDotNetAPI.Models;


namespace AchievementParser
{
    public class Class1
    {

        public static void Main(string[] args)
        {
            WowExplorer explorer = new WowExplorer(Region.US);

            Guild immortalityGuild = explorer.GetGuild("Kilrogg", "PVP Guild", GuildOptions.GetEverything);

            Console.WriteLine("\n\nGUILD EXPLORER SAMPLE\n");

            Console.WriteLine("{0} is a guild of level {1} and has {2} members.",
                immortalityGuild.Name,
                immortalityGuild.Level,
                immortalityGuild.Members.Count());

            //Print out first top 20 ranked members of Immortality
            foreach (GuildMember member in immortalityGuild.Members.OrderBy(m => m.Rank).Take(20))
            {
                Console.WriteLine(member.Character.Name + " has rank " + member.Rank);
            }

            Console.WriteLine("\n\nCHARACTER EXPLORER SAMPLE\n");
            Character briandekCharacter =
                explorer.GetCharacter("kilrogg", "debz", CharacterOptions.GetStats | CharacterOptions.GetAchievements);

            Console.WriteLine("{0} is a retired warrior of level {1} who has {2} achievement points having completed {3} achievements",
                briandekCharacter.Name,
                briandekCharacter.Level,
                briandekCharacter.AchievementPoints,
                briandekCharacter.Achievements.AchievementsCompleted.Count());

            

            foreach (KeyValuePair<string, object> stat in briandekCharacter.Stats)
            {
                Console.WriteLine(stat.Key + " : " + stat.Value);
            }

            //Get one realm
            IEnumerable<Realm> usRealms = explorer.GetRealms(Region.US);
            Realm skullcrusher = usRealms.GetRealm("skullcrusher");

            //Get all pvp realms only
            IEnumerable<Realm> pvpRealmsOnly = usRealms.WithType(RealmType.PVP);
            Console.WriteLine("\n\nREALMS EXPLORER SAMPLE\n");
            foreach (var realm in pvpRealmsOnly)
            {
                Console.WriteLine("{0} has {1} population", realm.Name, realm.Population);
            }

            Console.WriteLine("\n\nGUILD PERKS\n");

            IEnumerable<GuildPerkInfo> perks = explorer.GetGuildPerks();
            foreach (var perk in perks)
            {
                Console.WriteLine("{0} perk at guild level {1}", perk.Spell.Name, perk.GuildLevel);
            }

            Console.WriteLine("\n\nGUILD REWARDS\n");

            IEnumerable<GuildRewardInfo> rewards = explorer.GetGuildRewards();
            foreach (var reward in rewards)
            {
                Console.WriteLine("{0} reward at min guild level {1}", reward.Item.Name, reward.MinGuildLevel);
            }

            Console.WriteLine("\n\nCHARACTER RACES\n");

            IEnumerable<CharacterRaceInfo> races = explorer.GetCharacterRaces();
            foreach (var race in races.OrderBy(r => r.Id))
            {
                Console.WriteLine("{0} race with numeric value {1}", race.Name, race.Id);
            }

            Console.WriteLine("\n\nCHARACTER CLASSES\n");

            IEnumerable<CharacterClassInfo> classes = explorer.GetCharacterClasses();
            foreach (var @class in classes.OrderBy(c => c.Id))
            {
                Console.WriteLine("{0} class with numeric value {1}", @class.Name, @class.Id);
            }
        }

    }
}
