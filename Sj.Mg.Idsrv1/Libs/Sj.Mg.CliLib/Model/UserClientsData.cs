using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using MongoDB.Bson;

namespace Sj.Mg.CliLib.Model
{
    public class UserClientsList
    {
        [Newtonsoft.Json.JsonIgnore]
        [MongoDB.Bson.Serialization.Attributes.BsonElement("id")]
        public ObjectId Id { get; set; }
        public string userId { get; set; }
        public string email { get; set; }
        public List<UserClientsData> UserClientsData { get; set; }
    }
    public class UserClientsData
    {
        public string clientTypeName { get; set; }
        public List<Client> Clients { get; set; }
    }

    public class Client
    {
        public string clientName { get; set; }
        public List<UserScopes> UserScopes { get; set; }
    }

    public class UserScopes
    {
        public string scopeName { get; set; }
        public string icon { get; set; }
        public string activeIcon { get; set; }
        public string description { get; set; }
    }
}
