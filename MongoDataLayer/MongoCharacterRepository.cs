using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AchievementSherpa.Business;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Diagnostics;
using System.Threading;
using System.Globalization;

namespace AchievementSherpa.Data.MongoDb
{
    public class MongoCharacterRepository : BaseRepository<Character>, ICharacterRepository
    {

        public const string CollectionName = "Characters";

        public MongoCharacterRepository()
            : base(CollectionName)
        {
        }

        public IList<Character> FindAll()
        {
            return Collection.FindAll().ToList();
        }

        public Character FindCharacter(Character character)
        {
            IDictionary<string, object> queryParams = new Dictionary<string, object>();

            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;
            queryParams.Add("Name", textInfo.ToTitleCase(character.Name));
            queryParams.Add("Server", textInfo.ToTitleCase(character.Server));
            queryParams.Add("Region", character.Region.ToUpperInvariant());
            return Collection.FindOne(new QueryDocument(queryParams));
        }

        public void SaveCharacter(Character character)
        {
            if (character._id == null)
            {
                character._id = ObjectId.GenerateNewId().ToString();
            }

            CultureInfo cultureInfo   = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;

            character.Name = textInfo.ToTitleCase(character.Name);
            character.Server = textInfo.ToTitleCase(character.Server);
            character.Region = character.Region.ToUpperInvariant();

            Collection.Save(character);
        }

        public void DeleteCharacter(Character character)
        {
           // Collection.Remove(
        }


        public int CalculateRankWithinGuild(Character character)
        {
            return -1;
        }

        public int CalculateRankWithinServer(Character character)
        {
            return -1;
        }
        public int CalculateRankWithinWord(Character character)
        {
            return -1;
        }

        public IList<Character> SearchByName(string pattern)
        {
            IDictionary<string, object> queryParams = new Dictionary<string, object>();
            queryParams.Add("Name", new BsonRegularExpression(string.Format("/{0}/i", pattern)));
            QueryDocument query = new QueryDocument(queryParams);
            Debug.WriteLine(query.ToJson());
            return Collection.Find(new QueryDocument(queryParams)).ToList();
        }
    }
}
