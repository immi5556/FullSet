using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sj.Mg.Mongo.Data
{
    public class BaseMongo
    {
        protected static IMongoClient _client;
        protected static IMongoDatabase _database;

        static BaseMongo()
        {
            _client = new MongoClient("mongodb://localhost:27017");
            _database = _client.GetDatabase("Medgrotto");
        }

        public static IMongoDatabase GetDatabase()
        {
            return _database;
        }
    }
}
