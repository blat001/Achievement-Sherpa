using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AchievementSherpa.Business.Parsers;
using AchievementSherpa.Business;
using WowDotNetAPI;
using AchievementSherpa.Business.Services;
using System.Net;

namespace AchievementSherpa.WowApi
{
    public class WowApiCharacterParser : ICharacterParser
    {
        private IAchievementService _achievementRepository;
        public WowApiCharacterParser(IAchievementService achievementRepository)
        {
            _achievementRepository = achievementRepository;
        }

        public Business.Character Parse(Business.Character character, bool forceParse)
        {
            WowExplorer explorer = new WowExplorer(Region.US);
            try
            {
                WowDotNetAPI.Models.Character downloadedCharacter = explorer.GetCharacter(Region.US, character.Server, character.Name, CharacterOptions.GetAchievements | CharacterOptions.GetGuild);

                if (downloadedCharacter == null)
                {
                    return null;
                }

                character.Class = downloadedCharacter.Class.ToString();
                character.CurrentPoints = downloadedCharacter.AchievementPoints;
                character.LastParseDate = DateTime.UtcNow;
                character.Level = downloadedCharacter.Level;
                character.Race = downloadedCharacter.Race.ToString();
                character.Thumbnail = downloadedCharacter.Thumbnail;
                character.Guild = downloadedCharacter.Guild.Name;

                for (int i = 0; i < downloadedCharacter.Achievements.AchievementsCompleted.Count(); i++)
                {
                    int blizzardId = downloadedCharacter.Achievements.AchievementsCompleted.ElementAt(i);
                    DateTime completedOn = ConvertUnixEpochTime(downloadedCharacter.Achievements.AchievementsCompletedTimestamp.ElementAt(i));

                    Achievement achievement = _achievementRepository.FindAchivementByBlizzardId(blizzardId);
                    if (achievement != null && !character.HasAchieved(achievement))
                    {
                        character.AddNewAchivement(completedOn, achievement);
                    }
                }

                return character;
            }
            catch (WebException we)
            {
                HttpWebResponse webResponse = we.Response as HttpWebResponse;
                if (webResponse != null && webResponse.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }

                throw we;
            }
        }


        private DateTime ConvertUnixEpochTime(long seconds)
        {

            DateTime Fecha = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            return Fecha.ToLocalTime().AddMilliseconds(seconds);

        }
    }
}
