using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using Simple_Achievement_Parser;

namespace Data_Library
{
    public class CharacterService : ICharacterService
    {
        public Character FindCharacter(Character character)
        {
            var db = DBFactory.CreateConnection();

            Character existing = db.Characters.FirstOrDefault(c => c.Name == character.Name && c.Server == character.Server && c.Region == character.Region);

            if (existing != null)
            {
                return existing;
            }
            else
            {
                db.Characters.AddObject(character);
                db.SaveChanges();
            }

            return character;

        }

        public void SaveCharacter(Character character)
        {
            var db = DBFactory.CreateConnection();
            IList<CharacterAchievement> cas = new List<CharacterAchievement>();

           
            db.SaveChanges();
        }

        public void DeleteCharacter(Character character)
        {
            var db = DBFactory.CreateConnection();


            var person = db.Characters.FirstOrDefault(c => c.Name == character.Name && c.Server == character.Server);

            if (person != null)
            {
                db.Characters.DeleteObject(person);
            }



            db.SaveChanges();
        }
    }
}
