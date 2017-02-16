using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using MongoDB.Driver.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using MongoDB.Driver.Builders;

namespace DataBaseConnection
{
    public class DataBaseFunc
    {
        MongoDatabase databaseConnection;

        public DataBaseFunc()
        {
            MongoClient client = new MongoClient("mongodb://localhost:27017");
            MongoServer server = client.GetServer();
            databaseConnection = server.GetDatabase("Medgrotto");
        }

        public void insertData(String name, String email, String familyName, String accessToken)
        {
            MongoCollection<BsonDocument> userData =
        databaseConnection.GetCollection<BsonDocument>("UserData");
            BsonDocument department = new BsonDocument {
                { "name", name },
                { "email", email },
                { "familyName", familyName },
                { "accessToken", accessToken }
                };

            userData.Insert(department);
        }

        public Boolean getData(String email)
        {
            Boolean exists = false;
            var collection = databaseConnection.GetCollection<BsonDocument>("UserData");
            var query = Query.EQ("email", email);
            var userFound = collection.Find(query).ToList();
            if(userFound.Count>0)
            {
                exists = true;
            }
            return exists;
        }
    }
}
