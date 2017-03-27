using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sj.Mg.CliLib.Model
{
    public class permission
    {
        public permission()
        {
            requestedscopes = new List<resource>();
            subjects = new List<string>();
        }
        [MongoDB.Bson.Serialization.Attributes.BsonId]
        //[MongoDB.Bson.Serialization.Attributes.BsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [MongoDB.Bson.Serialization.Attributes.BsonElement("id")]
        public MongoDB.Bson.ObjectId Id { get; set; }
        public string ticket { get; set; }
        public List<resource> requestedscopes { get; set; }
        public List<string> subjects { get; set; }
    }
}
