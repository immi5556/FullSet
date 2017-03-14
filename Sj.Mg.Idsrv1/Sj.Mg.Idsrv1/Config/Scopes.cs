using IdentityServer3.Core.Models;
using Sj.Mg.Mongo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson.IO;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Sj.Mg.Idsrv1.Config
{
    public class Scopes
    {
        static List<Scope> _lstscopes = new List<Scope>
            {
                StandardScopes.OpenId,
                StandardScopes.ProfileAlwaysInclude,
                StandardScopes.Address,
                StandardScopes.OfflineAccess,
                new Scope()
                {
                    Name = "user.Observation",
                    DisplayName = "observation for users",
                    Description = "patient's observation",
                    Type = ScopeType.Resource,
                    Claims = new List<ScopeClaim>()
                    {
                        new ScopeClaim("role", false)
                    }
                },
                new Scope()
                {
                    Name = "patient.MedicationOrder",
                    DisplayName = "orders for medications",
                    Description = "patient's orders for medications",
                    Type = ScopeType.Resource,
                    Claims = new List<ScopeClaim>()
                    {
                        new ScopeClaim("role", false)
                    }
                },
                new Scope()
                {
                    Name = "roles",
                    DisplayName = "Role(s)",
                    Description = "Allow apps to see your role(s)",
                    Type = ScopeType.Identity,
                    Claims = new List<ScopeClaim>()
                    {
                        new ScopeClaim("role", true)
                    }
                },
                new Scope()
                {
                    Name = "uma_authorization",
                    DisplayName = "Uma Authz",
                    Description = "Uma authorization to enable AAT token to client",
                    Type = ScopeType.Resource,
                    Claims = new List<ScopeClaim>()
                    {
                        new ScopeClaim("role", true)
                    }
                },
                new Scope()
                {
                    Name = "uma_protection",
                    DisplayName = "Uma protect",
                    Description = "Uma Protection to enable PAT token to resource server",
                    Type = ScopeType.Resource,
                    Claims = new List<ScopeClaim>()
                    {
                        new ScopeClaim("role", true)
                    }
                },
                new Scope()
                {
                    Name = "write1",
                    Type = ScopeType.Resource,
                    Description = "Some detail desd",
                    DisplayName = "Some detail display..",
                    Emphasize = true,
                    Claims = new List<ScopeClaim>()
                    {
                        new ScopeClaim("name", true),
                        new ScopeClaim("given_name", true),
                        new ScopeClaim("family_name", true),
                        new ScopeClaim("email", true)
                    }
                }
            };
        
        static Scopes()
        {
            addScopesinDB();
            AddFhirScopes();
        }
        public static IEnumerable<Scope> Get()
        {
            return _lstscopes;
        }

        public static void AddFhirScopes()
        {
            //Patient Accounts
            _lstscopes.Add(
                new Scope()
                {
                    Name = "Patient/Account.*",
                    DisplayName = "Patient/Account.*",
                    Description = "full access single patient's account information.",
                    Type = ScopeType.Resource
                });

            _lstscopes.Add(
                new Scope()
                {
                    Name = "Patient/Account.Read",
                    DisplayName = "Patient/Account.Read",
                    Description = "read single patient's account information.",
                    Type = ScopeType.Resource
                });

            _lstscopes.Add(
                new Scope()
                {
                    Name = "Patient/Account.Write",
                    DisplayName = "Patient/Account.Write",
                    Description = "write single patient's account information.",
                    Type = ScopeType.Resource
                });

            //Patient Medication
            _lstscopes.Add(
                new Scope()
                {
                    Name = "Patient/Medication.*",
                    DisplayName = "Patient/Medication.*",
                    Description = "Full access single patient's Medication information.",
                    Type = ScopeType.Resource
                });

            _lstscopes.Add(
                new Scope()
                {
                    Name = "Patient/Medication.Read",
                    DisplayName = "Patient/Medication.Read",
                    Description = "read single patient's Medication information.",
                    Type = ScopeType.Resource
                });

            _lstscopes.Add(
                new Scope()
                {
                    Name = "Patient/Medication.Write",
                    DisplayName = "Patient/Medication.Write",
                    Description = "write single patient's Medication information.",
                    Type = ScopeType.Resource
                });

            //Patient Observation
            _lstscopes.Add(
                new Scope()
                {
                    Name = "Patient/Observation.*",
                    DisplayName = "Patient/Observation.*",
                    Description = "Full access to a single patient's Observation information.",
                    Type = ScopeType.Resource
                });

            _lstscopes.Add(
                new Scope()
                {
                    Name = "Patient/Observation.Read",
                    DisplayName = "Patient/Observation.Read",
                    Description = "read access to a single patient's Observation information.",
                    Type = ScopeType.Resource
                });

            _lstscopes.Add(
                new Scope()
                {
                    Name = "Patient/Observation.Write",
                    DisplayName = "Patient/Observation.Write",
                    Description = "write access to a single patient's Observation information.",
                    Type = ScopeType.Resource
                });

            //Patient 
            _lstscopes.Add(
                new Scope()
                {
                    Name = "Patient/Patient.Read",
                    DisplayName = "Patient/Patient.Read",
                    Description = "Read access to a single patient's demographic information.",
                    Type = ScopeType.Resource
                });

            _lstscopes.Add(
                new Scope()
                {
                    Name = "Patient/Patient.Write",
                    DisplayName = "Patient/Patient.Write",
                    Description = "Only write access to a single patient's demographic information.",
                    Type = ScopeType.Resource
                });

            _lstscopes.Add(
                new Scope()
                {
                    Name = "Patient/Patient.*",
                    DisplayName = "Patient/Patient.*",
                    Description = "Read and write access to a single patient's demographic information.",
                    Type = ScopeType.Resource
                });

            _lstscopes.Add(
                new Scope()
                {
                    Name = "patient/MedicationOrder.read",
                    DisplayName = "patient/MedicationOrder.read",
                    Description = "Read access to a single patient's orders for medications.",
                    Type = ScopeType.Resource
                });

            //Medication order
            _lstscopes.Add(
                new Scope()
                {
                    Name = "patient/MedicationOrder.write",
                    DisplayName = "patient/MedicationOrder.write",
                    Description = "Read and write access to a single patient's orders for medications.",
                    Type = ScopeType.Resource
                });
            _lstscopes.Add(
                new Scope()
                {
                    Name = "patient/MedicationOrder.*",
                    DisplayName = "patient/MedicationOrder.*",
                    Description = "Full access to a single patient's orders for medications.",
                    Type = ScopeType.Resource
                });
            _lstscopes.Add(
                new Scope()
                {
                    Name = "patient/MedicationDispense.read",
                    DisplayName = "patient/MedicationDispense.read",
                    Description = "Read access to supply of medications to a single patient.",
                    Type = ScopeType.Resource
                });
            _lstscopes.Add(
                new Scope()
                {
                    Name = "patient/MedicationDispense.write",
                    DisplayName = "patient/MedicationDispense.write",
                    Description = "Read and write access to supply of medications to a single patient.",
                    Type = ScopeType.Resource
                });
            _lstscopes.Add(
                new Scope()
                {
                    Name = "patient/MedicationDispense.*",
                    DisplayName = "patient/MedicationDispense.*",
                    Description = "Full access to supply of medications to a single patient.",
                    Type = ScopeType.Resource
                });
            _lstscopes.Add(
                new Scope()
                {
                    Name = "patient/MedicationAdministration.read",
                    DisplayName = "patient/MedicationAdministration.read",
                    Description = "Read access to a single patient's medications consumption or other administration.",
                    Type = ScopeType.Resource
                });
            _lstscopes.Add(
                new Scope()
                {
                    Name = "patient/MedicationAdministration.write",
                    DisplayName = "patient/MedicationAdministration.write",
                    Description = "Read and write access to a single patient's medications consumption or other administration.",
                    Type = ScopeType.Resource
                });
        }

        public string addNewScope(string name, string displayname, string description, string type, Boolean emphasize, Boolean claimsName, Boolean claimsFamilyName, Boolean claimsGivenName, Boolean claimsEmail)
        {
            _lstscopes.Add(
                new Scope()
                {
                    Name = name,
                    DisplayName = displayname,
                    Description = description,
                    Type = (type == "Identity" ? ScopeType.Identity : ScopeType.Resource),
                    Emphasize = emphasize,
                    Claims = new List<ScopeClaim>()
                    {
                        new ScopeClaim("name", claimsName),
                        new ScopeClaim("given_name", claimsGivenName),
                        new ScopeClaim("family_name", claimsFamilyName),
                        new ScopeClaim("email", claimsEmail)
                    }
                }
            );
            BsonDocument scope = new BsonDocument {
                { "name", name },
                { "displayName", displayname },
                { "description", description },
                { "type", type },
                { "emphasize", emphasize},
                { "claims", new BsonDocument
                    {
                        { "name", claimsName },
                        { "givenName", claimsGivenName },
                        { "familyName", claimsFamilyName },
                        { "email", claimsEmail }
                    }
                }
            };
            IMongoCollection<BsonDocument> userData =
        BaseMongo.GetDatabase().GetCollection<BsonDocument>("Scopes");
            try
            {
                userData.InsertOne(scope);
                return "success";
            }
            catch 
            {
                return "error";
            }
            
        }

        public static async void addScopesinDB()
        {
            var collection = BaseMongo.GetDatabase().GetCollection<BsonDocument>("Scopes");
            var filter = new BsonDocument();
            using (var cursor = await collection.FindAsync(filter))
            {
                while (await cursor.MoveNextAsync())
                {
                    var batch = cursor.Current;
                    foreach (var document in batch)
                    {
                        _lstscopes.Add(
                            new Scope()
                            {
                                Name = document["name"].ToString(),
                                DisplayName = document["displayName"].ToString(),
                                Description = document["description"].ToString(),
                                Type = document["type"].ToString() == "Identity" ? ScopeType.Identity : ScopeType.Resource,
                                Emphasize = document["emphasize"].ToBoolean(),
                                Claims = new List<ScopeClaim>()
                                {
                                    new ScopeClaim("name", document["claims"]["name"].ToBoolean()),
                                    new ScopeClaim("given_name", document["claims"]["givenName"].ToBoolean()),
                                    new ScopeClaim("family_name", document["claims"]["familyName"].ToBoolean()),
                                    new ScopeClaim("email", document["claims"]["email"].ToBoolean())
                                }
                            }
                        );

                    }
                }
            }
        }
    }
}