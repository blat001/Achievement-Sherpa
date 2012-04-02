using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AchievementSherpa.Business.Parsers
{
    public interface IGuildParser
    {
        IEnumerable<Character> ParserRoster(string region, string server, string name);
    }
}
