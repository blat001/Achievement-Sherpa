using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AchievementSherpa.Business.Parsers;

namespace AchievementSherpa.Business.Services
{
    public class CharacterService : ICharacterService
    {
        private ICharacterRepository _characterRepository;
        private ICharacterParser _parser;

        public CharacterService(ICharacterRepository characterRepository, ICharacterParser parser)
        {
            _characterRepository = characterRepository;
            _parser = parser;
        }

        public void ForceCharacterParse(string server, string name, string region)
        {
            ParseCharacter(server, name, region, true);
        }

        public void UpdateCharacterDetails(string server, string name, string region)
        {
            ParseCharacter(server, name, region, false);
        }

        protected virtual void ParseCharacter(string server, string name, string region, bool force)
        {
             Character character = new Character(name, server, region);

            Character foundCharacter = _characterRepository.FindCharacter(character);
            if (foundCharacter != null)
            {
                character = foundCharacter;
            }

            character = _parser.Parse(character, force);
            if (character !=null && character.CurrentPoints > 0)
            {
                _characterRepository.SaveCharacter(character);
            }
        }


        public Ranking RankCharacter(Character character)
        {
            return new Ranking();
        }

        public Character FindCharacter(string region, string server, string player)
        {
            return _characterRepository.FindCharacter(new Character(player, server, region));
        }
    }
}
