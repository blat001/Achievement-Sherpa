using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AchievementSherpa.Business.Services
{
    public interface ICharacterService
    {

        void UpdateCharacterDetails(string server, string character, string region);

        void ForceCharacterParse(string server, string character, string region);


        Ranking RankCharacter(Character character);

        Character FindCharacter(string region, string server, string player);
    }
}
