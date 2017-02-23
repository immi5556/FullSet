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
using Constants.Model;
using MongoDB.Bson.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
using System.Collections;

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

        public void insertGoogleUserData(String name, String email, String familyName)
        {
            MongoCollection<BsonDocument> userData =
        databaseConnection.GetCollection<BsonDocument>("UserData");
            BsonDocument department = new BsonDocument {
                { "name", name },
                { "email", email },
                { "familyName", familyName }
                };

            userData.Insert(department);
        }

        public void registerRequest(Constants.Model.ResShare share)
        {
            MongoCollection<BsonDocument> userData =
        databaseConnection.GetCollection<BsonDocument>("MedgrottoUsers");
            var query = Query.EQ("userName", share.touser);
            var response = userData.Find(query).ToList();
            if (response.Count > 0)
            {
                var result = userData.Update(
                        query,
                        Update.Set("pendingRequest." + response[0]["pendingRequest"].AsBsonArray.Values.Count(), share.ToBsonDocument())
                );
            }
        }

        public void removeRequest(Constants.Model.ResShare share, List<ResShare> pendingRequests)
        {
            MongoCollection<BsonDocument> userData =
        databaseConnection.GetCollection<BsonDocument>("MedgrottoUsers");
            var query = Query.And(Query.EQ("userName", share.touser),
                         Query.EQ("pendingRequest.scope", share.scope),
                         Query.EQ("pendingRequest.user", share.user));
            var update = Update.Pull("pendingRequest", new BsonDocument() { });
            userData.Update(query, update);

            foreach (var item in pendingRequests)
            {
                registerRequest(item);
            }
        }

        public void updateScopeUsers(UserDetails details, ResShare share)
        {
            MongoCollection<BsonDocument> userData =
        databaseConnection.GetCollection<BsonDocument>("MedgrottoUsers");
            var query = Query.EQ("userName", details.UserName);
            var update = Update.Pull("scopeUsers", new BsonDocument() { });
            userData.Update(query, update);
            var temp = 0;
            foreach (var item in details.ScopeUsers)
            {
                var result = userData.Update(
                        query,
                        Update.Set("scopeUsers." + temp, item.ToBsonDocument())
                );
                temp++;
            }
        }


        public void trackGoogleUserData(String email, String accessToken)
        {
            DateTime date = DateTime.Now;
            MongoCollection<BsonDocument> userData =
        databaseConnection.GetCollection<BsonDocument>("UserLoginTrack");
            BsonDocument department = new BsonDocument {
                { "email", email },
                { "accessToken", accessToken },
                { "loginTime", date  }
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

        public List<BsonDocument> getMedgrottoUsers()
        {
            //var collection = databaseConnection.GetCollection<BsonDocument>("MedgrottoUsers");
            //var query = Query.EQ("_id", true);
            //var userFound = collection.Find(query).ToList();
            //return userFound;

            return databaseConnection.GetCollection("MedgrottoUsers").FindAll().ToList();
        }
    }
}
