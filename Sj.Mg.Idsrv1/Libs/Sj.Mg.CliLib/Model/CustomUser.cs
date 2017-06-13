using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Sj.Mg.CliLib.Model
{
    public class CustomUser
    {
        [MongoDB.Bson.Serialization.Attributes.BsonId]
        //[MongoDB.Bson.Serialization.Attributes.BsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [MongoDB.Bson.Serialization.Attributes.BsonElement("id")]
        public MongoDB.Bson.ObjectId Id { get; set; }
        public string GivenName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool AcceptedEula { get; set; }
        public string Subject { get; set; }
        public string Provider { get; set; }
        public string ProviderID { get; set; }
        public string UniqueUserId { get; set; }
        public string Address { get; set; }
        public bool IsRegistered { get; set; }
        public bool IsProvider { get; set; }
        public string NPI { get; set; }
        List<CustomClaim> _custClaim = new List<CustomClaim>();
        [Newtonsoft.Json.JsonIgnore]
        public List<CustomClaim> CustomClaims
        {
            get
            {
                return _custClaim;
            }
            set
            {
                _custClaim = value;
            }
        }
        List<Claim> GetClaims()
        {
            _claims.Clear();
            _custClaim.ForEach(t =>
            {
                if (_claims.Find(t1 => t1.Type == t.type && t1.Value == t.value) == null)
                {
                    if (!string.IsNullOrEmpty(t.valueType))
                        _claims.Add(new Claim(t.type, t.value, t.valueType));
                    else
                        _claims.Add(new Claim(t.type, t.value));
                }
            });
            return _claims;
        }

        List<Claim> _claims = new List<Claim>();
        public List<Question> Questions { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public List<Claim> Claims
        {
            get
            {
                return GetClaims();
            }
            set
            {
                _claims = value;
            }
        }

        
    }

    public class Question
    {
        public string Ques { get;set; }
        public string Ans { get; set; }
    }
}
