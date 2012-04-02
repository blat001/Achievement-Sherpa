using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AchievementSherpa.Business.Parsers
{
    public interface ICharacterParser
    {
        /// <summary>
        /// Parses a character and updates their details
        /// </summary>
        /// <param name="character">The character to reparse</param>
        /// <param name="forceParse">if true, then reparse regardless, if false only reparse if the last parse was more than a week ago</param>
        /// <returns>A fully parsed character</returns>
        Character Parse(Character character, bool forceParse);
    }
}
