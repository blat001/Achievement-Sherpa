using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simple_Achievement_Parser;

namespace Business_Layer
{
    public interface IRankingService
    {
        int GetCharactersServerGuildRanking(Character character);

        int GetCharactersServerServerRanking(Character character);

        int GetCharactersServerWorldWideRanking(Character character);
    }
}
