using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using System.Configuration;

namespace AchievementSherpa.Data.MongoDb
{
    public class BaseRepository<TDocument>
    {
        private readonly string _collectionName;
        MongoDatabase _database;
        MongoCollection<TDocument> _collection;

        public BaseRepository(string collectionName)
        {
            _collectionName = collectionName;
            string databaseName = "Achievements";

            if (ConfigurationManager.AppSettings["MongoDbName"] != null)
            {
                databaseName = ConfigurationManager.AppSettings["MongoDbName"];
            }
            // TODO : Get connection string from app.config
            // TODO : Get database name from app.config
            
            MongoServer server = MongoServer.Create(ConfigurationManager.ConnectionStrings["AchievementDatabase"].ConnectionString);
            

            _database = server.GetDatabase(databaseName);
        }

         protected MongoCollection<TDocument> Collection
        {
            get
            {
                if (_collection == null)
                {

                    _collection = Database.GetCollection<TDocument>(_collectionName);
                }

                return _collection;
            }
        }

        protected MongoDatabase Database
        {
            get
            {
                return _database;
            }
        }
    }
}
