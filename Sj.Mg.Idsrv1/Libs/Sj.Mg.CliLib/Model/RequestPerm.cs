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
            AllowedUsers = new Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>>();
        }
        [MongoDB.Bson.Serialization.Attributes.BsonId]
        [Newtonsoft.Json.JsonIgnore]
        [MongoDB.Bson.Serialization.Attributes.BsonElement("id")]
        public MongoDB.Bson.ObjectId Id { get; set; }
        public string MyEmail { get; set;  }
        public Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>> AllowedUsers { get; set; }
    }
}
