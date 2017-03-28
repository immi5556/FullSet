using IdentityServer3.Core.Models;
using MongoDB.Bson;
using Sj.Mg.Mongo;
using Sj.Mg.Mongo.Data;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using MongoDB.Driver;

namespace Sj.Mg.Idsrv1.Config
{
    public class Clients
    {
        static List<Client> _lstclients = new List<Client>
            {
                new Client
                {
                    ClientId = "MgClient1",
                    ClientName = "My Client (Hybrid)",
                    Flow = Flows.Hybrid,
                    AllowAccessToAllScopes = true,
                    RedirectUris = new List<string>
                    {
                        CliLib.Utils.Common.ReClientMvc
                    },
                    PostLogoutRedirectUris = new List<string>()
                    {
                        CliLib.Utils.Common.ReClientMvc
                    },
                    IncludeJwtId = true,
                    AllowRememberConsent = false,
                    Resources = new List<string>()
                    {
                        "Account", "Medication", "Observation", "Demographic"
                    }
                },
                new Client
                {
                     ClientId = "FHIR-Resource1",
                     ClientName = "MG- Resource Server Api (Authorization Code)",
                     //Flow = Flows.ClientCredentials,
                     Flow = Flows.Hybrid,
                     AllowAccessToAllScopes = true,
                     //AllowedScopes = new List<string>
                     //{
                     //   "patient.MedicationOrder",
                     //   "uma_protection",
                     //   "user.Observation",
                     //   "Patient/Account.Read",
                     //   "Patient/Account.*",
                     //   "Patient/Account.Write",
                     //   "Patient/Medication.Read",
                     //   "Patient/Medication.*",
                     //   "Patient/Medication.Write",
                     //   "Patient/Observation.Read",
                     //   "Patient/Observation.*",
                     //   "Patient/Observation.Write",
                     //   "Patient/Patient.Read",
                     //   "Patient/Patient.*",
                     //   "Patient/Patient.Write"
                     //},
                    // redirect = URI of the MVC application callback
                    RedirectUris = new List<string>
                    {
                        //AppConstants.Constants.ReClientMvcStsCallback
                        CliLib.Utils.Common.ReApi
                    },           

                    // client secret
                    ClientSecrets = new List<Secret>()
                    {
                        new Secret(CliLib.Utils.Common.ClientSecret.Sha256())
                    }
                },
                new Client
                {
                    ClientId = "mvc.owin.hybrid",
                    ClientName = "Mvc Sample Client",
                    Flow = Flows.Hybrid,
                    //AllowAccessToAllScopes = true,
                    AllowedScopes = new List<string>
                    {
                        "openid",
                        "profile",
                        "address",
                        "write1",
                        "uma_protection",
                    },
                    RedirectUris = new List<string>
                    {
                        "https://localhost:44300/"
                    },
                    PostLogoutRedirectUris = new List<string>()
                    {
                        "https://localhost:44300/"
                    },
                    IncludeJwtId = true,
                    AllowRememberConsent = false,
                    ClientSecrets = new List<Secret>()
                    {
                        //new Secret(AppConstants.Constants.ClientSecret.Sha256())
                        new Secret("secret".Sha256())
                    }
                },
                new Client
                {
                    ClientId = "mvc.api",
                    ClientName = "mvc sample api",
                    Flow = Flows.ClientCredentials,
                    //AllowAccessToAllScopes = true,
                    AllowedScopes = new List<string>
                    {
                         "write"
                    },
                    // redirect = URI of the MVC application callback
                    RedirectUris = new List<string>
                    {

                    },
                    // client secret
                    ClientSecrets = new List<Secret>()
                    {
                        //new Secret("secret".Sha256())
                        new Secret("secret".Sha256())
                    }
                },
                new Client
                {
                    ClientId = "IdSrv1",
                    ClientName = "IdSrv1 (All)",
                    Flow = Flows.Hybrid,
                    AllowAccessToAllScopes = true,
                    RedirectUris = new List<string>
                    {
                        CliLib.Utils.Common.Sts + "/scopes",
                    },
                    PostLogoutRedirectUris = new List<string>()
                    {
                        CliLib.Utils.Common.ReClientMvc
                    },
                    IncludeJwtId = true,
                    AllowRememberConsent = false
                },
            };

        static Clients()
        {
            addClientsinDB();
        }

        public static IEnumerable<Client> Get()
        {
            return _lstclients;
        }

        public static Client RegisterClients(NameValueCollection nv)
        {
            string clientid = nv["client_id"] ?? "";
            string clientname = nv["client_name"];
            string[] redircuri = new string[] {
                nv["redirect_uri"] ?? ""
            };
            string[] resptype = (nv["response_type"] ?? "").Split(' ');
            string[] scopes = (nv["scope"] ?? "").Split(' ');
            var nc = new Client
            {
                ClientId = string.IsNullOrEmpty(clientid) ? Guid.NewGuid().ToString().Replace("-", "") : clientid,
                ClientName = string.IsNullOrEmpty(clientname) ? "Client-" + Guid.NewGuid().ToString().Replace("-", "") : clientname,
                Flow = Flows.Hybrid,
                AllowedScopes = scopes.ToList(),
                ClientSecrets = new List<Secret>()
                {
                    new Secret()
                    {
                        Type = "RSA",
                        Value = ""
                    }
                },
                RedirectUris = redircuri.ToList(),
                PostLogoutRedirectUris = (redircuri ?? new string[] { }).ToList(),
                IncludeJwtId = true,
                AllowRememberConsent = false
            };
            _lstclients.Add(nc);
            return nc;
        }

        public string addNewClient(string clientId, string clientName, string flow, Array allowedScopes, string redirectUris, string postLogoutRedirectUris, bool includeJwtId, bool allowRememberConsent, bool allowAccessToAllScopes, bool enable)
        {
            var newClient = GetClient(clientId, clientName, flow, allowedScopes, redirectUris, postLogoutRedirectUris, includeJwtId, allowRememberConsent, allowAccessToAllScopes, enable);
            _lstclients.Add(newClient);
            MongoManage.Insert<Client>(newClient, "Clients");
            return "Success";
        }

        public string updateClient(string clientId, string clientName, string flow, Array allowedScopes, string redirectUris, string postLogoutRedirectUris, bool includeJwtId, bool allowRememberConsent, bool allowAccessToAllScopes, bool enable)
        {
            var newClient = GetClient(clientId, clientName, flow, allowedScopes, redirectUris, postLogoutRedirectUris, includeJwtId, allowRememberConsent, allowAccessToAllScopes, enable);
            
            var i = 0;
            foreach (var document in _lstclients)
            {
                if (document.ClientId == clientId)
                {
                    _lstclients[i] = newClient;
                    break;
                }
                i++;
            }
            var database = BaseMongo.GetDatabase();
            var collection = database.GetCollection<BsonDocument>("Clients");

            var filter = Builders<BsonDocument>.Filter.Eq("ClientId", clientId);

            collection.ReplaceOneAsync(filter, newClient.ToBsonDocument());

            return "success";
        }

        public static async void addClientsinDB()
        {
            var collection = BaseMongo.GetDatabase().GetCollection<BsonDocument>("Clients");
            var filter = new BsonDocument();
            using (var cursor = await collection.FindAsync(filter))
            {
                while (await cursor.MoveNextAsync())
                {
                    var batch = cursor.Current;
                    foreach (var document in batch)
                    {
                        var newScope = GetClient(document["ClientId"].ToString(), document["ClientName"].ToString(), document["Flow"].ToString(), document["AllowedScopes"].AsBsonArray.ToArray(), document["RedirectUris"].ToString(), document["PostLogoutRedirectUris"].ToString(), document["IncludeJwtId"].ToBoolean(), document["AllowRememberConsent"].ToBoolean(), document["AllowAccessToAllScopes"].ToBoolean(), document["Enabled"].ToBoolean());
                        _lstclients.Add(newScope);
                    }
                }
            }
        }

        static Client GetClient(string clientId, string clientName, string flow, Array allowedScopes, string redirectUris, string postLogoutRedirectUris, bool includeJwtId, bool allowRememberConsent, bool allowAccessToAllScopes, bool enable)
        {
            List<string> scopes = new List<string>();
            foreach (var document in allowedScopes)
            {
                scopes.Add(document.ToString());
            }
            return new Client
            {
                Enabled = enable,
                ClientId = clientId,
                ClientName = clientName,
                Flow = (flow == "AuthorizationCode" ? Flows.AuthorizationCode : (flow == "Implicit" ? Flows.Implicit : (flow == "Hybrid" ? Flows.Hybrid : (flow == "ClientCredentials" ? Flows.ClientCredentials : (flow == "ResourceOwner" ? Flows.ResourceOwner : (flow == "Custom" ? Flows.Custom : (flow == "AuthorizationCodeWithProofKey" ? Flows.AuthorizationCodeWithProofKey : Flows.HybridWithProofKey))))))),
                AllowAccessToAllScopes = allowAccessToAllScopes,
                AllowedScopes = scopes,
                RedirectUris = new List<string>
                    {
                        redirectUris
                    },
                PostLogoutRedirectUris = new List<string>()
                    {
                        postLogoutRedirectUris
                    },
                IncludeJwtId = includeJwtId,
                AllowRememberConsent = allowRememberConsent
            };
        }
    }
}