using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sj.Mg.CliLib.Model
{
    public class RequestPerm
    {
        public RequestPerm()
        {
            AllowedUsers = new Dictionary<string, Dictionary<string, Dictionary<string, List<UserData>>>>();
            RequestedUsers = new Dictionary<string, Dictionary<string, Dictionary<string, List<UserData>>>>();
            MyDetailsSharedWith = new Dictionary<string, Dictionary<string, Dictionary<string, List<UserData>>>>();
        }
        [MongoDB.Bson.Serialization.Attributes.BsonId]
        [Newtonsoft.Json.JsonIgnore]
        [MongoDB.Bson.Serialization.Attributes.BsonElement("id")]
        public MongoDB.Bson.ObjectId Id { get; set; }
        public string MyEmail { get; set;  }
        public Dictionary<string, Dictionary<string, Dictionary<string, List<UserData>>>> AllowedUsers { get; set; }
        public Dictionary<string, Dictionary<string, Dictionary<string, List<UserData>>>> RequestedUsers { get; set; }
        public Dictionary<string, Dictionary<string, Dictionary<string, List<UserData>>>> MyDetailsSharedWith { get; set; }
    }

    public class UserData
    {
        public string user { get; set; }
        public string relation { get; set; }
        public string sharedBy { get; set; }
    }
}
