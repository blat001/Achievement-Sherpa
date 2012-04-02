using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AchievementSherpa.Business
{
    public interface ICharacterRepository
    {

        IList<Character> SearchByName(string pattern);

        IList<Character> FindAll();

        Character FindCharacter(Character character);

        void SaveCharacter(Character character);

        void DeleteCharacter(Character character);

        int CalculateRankWithinGuild(Character character);
        int CalculateRankWithinServer(Character character);
        int CalculateRankWithinWord(Character character);
    }
}
