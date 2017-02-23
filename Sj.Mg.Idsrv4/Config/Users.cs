using IdentityServer3.Core;
using IdentityServer3.Core.Services.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using DataBaseConnection;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using System.Web.Script.Serialization;
using System.Collections;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Constants.Model;
using Newtonsoft.Json.Linq;

namespace Sj.Mg.Idsrv4.Config
{
    public class Users
    {
        static List<InMemoryUser> _lstusers = new List<InMemoryUser>
        {
            
        };
        public static List<InMemoryUser> Get()
        {
            DataBaseFunc db = new DataBaseFunc();
            var users = db.getMedgrottoUsers();

            foreach (var e in users)
                {
                    Constants.Model.UserDetails obj = new Constants.Model.UserDetails();

                    obj.UserName = Convert.ToString(e["userName"]);
                    obj.Password = Convert.ToString(e["password"]);
                    obj.Subject = Convert.ToString(e["subject"]);
                    obj.GivenName = Convert.ToString(e["claims"]["givenName"]);
                    obj.FamilyName = Convert.ToString(e["claims"]["familyName"]);
                    obj.Address = Convert.ToString(e["claims"]["address"]);
                    obj.Role = Convert.ToString(e["claims"]["role"]);
                    obj.Clients = JsonConvert.DeserializeObject<List<string>>(e["clients"].ToJson());
                    obj.PendingRequests = JsonConvert.DeserializeObject<List<ResShare>>(e["pendingRequest"].ToJson());
                    if(Convert.ToString(e["scopeUsers"].ToJson()) != null)
                    {
                        JArray json = JArray.Parse(e["scopeUsers"].ToJson());
                        foreach(var scus in json)
                        {
                            string key = Convert.ToString(scus["k"]);
                            List<string> users1 = new List<string>();
                            JArray json1 = JArray.Parse(Convert.ToString(scus["v"]));
                            foreach(var valueItem in json1)
                            {
                                users1.Add(valueItem.ToString());
                            }

                            obj.ScopeUsers.Add(key, users1);
                        }
                    }
                    
                    _lstusers.Add(new InMemoryUser()
                        {
                            Username = obj.UserName,
                            Password = obj.Password,
                            Subject = obj.Subject,
                            Claims = new[]
                            {
                            new Claim(IdentityServer3.Core.Constants.ClaimTypes.GivenName, obj.GivenName),
                            new Claim(IdentityServer3.Core.Constants.ClaimTypes.FamilyName, obj.FamilyName),
                            new Claim(IdentityServer3.Core.Constants.ClaimTypes.Address, obj.Address),
                            new Claim("role", obj.Role)
                        }
                        });
                    _lstuserdetails.Add(new Constants.Model.UserDetails()
                    {

                        UserName = obj.UserName,
                        Clients = obj.Clients,
                        ScopeUsers = obj.ScopeUsers,
                        PendingRequests = obj.PendingRequests
                    });
                }
            return _lstusers;
        }

        public static void UpdateDetails(Constants.Model.ResShare share)
        {
            var usr = _lstuserdetails.Find(t => t.UserName == share.user);
            if (usr.ScopeUsers.ContainsKey(share.scope))
            {
                if (!usr.ScopeUsers[share.scope].Exists(t => t == share.touser))
                {
                    usr.ScopeUsers[share.scope].Add(share.touser);
                }
            }
            else
            {
                usr.ScopeUsers.Add(share.scope, new List<string>() { share.touser });
            }
            DataBaseFunc db = new DataBaseFunc();
            db.updateScopeUsers(usr, share);
        }
        public static void DeleteDetails(Constants.Model.ResShare share)
        {
            var usr = _lstuserdetails.Find(t => t.UserName == share.user);
            if (usr.ScopeUsers.ContainsKey(share.scope))
            {
                if (usr.ScopeUsers[share.scope].Exists(t => t == share.touser))
                {
                    usr.ScopeUsers[share.scope].Remove(share.touser);
                }
            }
            else
            {
                usr.ScopeUsers.Add(share.scope, new List<string>() { share.touser });
            }
            DataBaseFunc db = new DataBaseFunc();
            db.updateScopeUsers(usr, share);
        }
        public static void RegisterRequest(Constants.Model.ResShare share)
        {
            var usr = _lstuserdetails.Find(t => t.UserName == share.touser);
            if (usr.PendingRequests.Find(t => t.scope == share.scope && t.user == share.user) == null)
            {
                usr.PendingRequests.Add(share);
                DataBaseFunc db = new DataBaseFunc();
                foreach ( var item in usr.PendingRequests ){
                    db.registerRequest(item);
                }
            }
        }
        public static void RemoveRequest(Constants.Model.ResShare share)
        {
            var usr = _lstuserdetails.Find(t => t.UserName == share.user);
            var torem = usr.PendingRequests.Find(t => t.scope == share.scope && t.user == share.touser);
            if (torem != null)
            {
                usr.PendingRequests.Remove(torem);
                DataBaseFunc db = new DataBaseFunc();
                db.removeRequest(torem, usr.PendingRequests);
            }
        }

        public static List<Constants.Model.UserDetails> GetDetails()
        {
            return _lstuserdetails;
        }

        static List<Constants.Model.UserDetails> _lstuserdetails = new List<Constants.Model.UserDetails>()
        {
            
        };
    }
}