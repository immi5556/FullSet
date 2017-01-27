using IdentityServer3.Core;
using IdentityServer3.Core.Services.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace Sj.Mg.Idsrv4.Config
{
    public class Users
    {
        static List<InMemoryUser> _lstusers = new List<InMemoryUser>
        {
            new InMemoryUser()
                {
                    Username = "Kevin",
                    Password = "secret",
                    Subject = "UniqueSubject-1",
                    Claims = new[]
                    {
                        new Claim(IdentityServer3.Core.Constants.ClaimTypes.GivenName, "Kevin"),
                        new Claim(IdentityServer3.Core.Constants.ClaimTypes.FamilyName, "KFly"),
                        new Claim(IdentityServer3.Core.Constants.ClaimTypes.Address, "1, Main Street, Antwerp, Belgium"),
                        new Claim("role", "FloorNurse")
                    }
                },
                new InMemoryUser()
                {
                    Username = "Sven",
                    Password = "secret",
                    Subject = "UniqueSubject-2",
                    Claims = new[]
                    {
                        new Claim(IdentityServer3.Core.Constants.ClaimTypes.GivenName, "Sven"),
                        new Claim(IdentityServer3.Core.Constants.ClaimTypes.FamilyName, "SFly"),
                        new Claim(IdentityServer3.Core.Constants.ClaimTypes.Address, "2, Main Road, Antwerp, Belgium"),
                        new Claim("role", "Hospitalist")
                    }
                },
                new InMemoryUser()
                {
                    Username = "John",
                    Password = "secret",
                    Subject = "UniqueSubject-3",
                    Claims = new[]
                    {
                        new Claim(IdentityServer3.Core.Constants.ClaimTypes.GivenName, "John"),
                        new Claim(IdentityServer3.Core.Constants.ClaimTypes.FamilyName, "Jly"),
                        new Claim(IdentityServer3.Core.Constants.ClaimTypes.Address, "1, Main Street, Dallas, TX."),
                        new Claim("role", "Patient")
                    }
                },
                new InMemoryUser()
                {
                    Username = "Andrea",
                    Password = "secret",
                    Subject = "UniqueSubject-4",
                    Claims = new[]
                    {
                        new Claim(IdentityServer3.Core.Constants.ClaimTypes.GivenName, "John"),
                        new Claim(IdentityServer3.Core.Constants.ClaimTypes.FamilyName, "Jly"),
                        new Claim(IdentityServer3.Core.Constants.ClaimTypes.Address, "1, Main Street, Dallas, TX."),
                        new Claim("role", "Patient")
                    }
                },
                new InMemoryUser()
                {
                    Username = "Bob",
                    Password = "secret",
                    Subject = "UniqueSubject-5",
                    Claims = new[]
                    {
                        new Claim(IdentityServer3.Core.Constants.ClaimTypes.GivenName, "John"),
                        new Claim(IdentityServer3.Core.Constants.ClaimTypes.FamilyName, "Jly"),
                        new Claim(IdentityServer3.Core.Constants.ClaimTypes.Address, "1, Main Street, Dallas, TX."),
                        new Claim("role", "Patient")
                    }
                }
        };
        public static List<InMemoryUser> Get()
        {
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
        }
        public static void RegisterRequest(Constants.Model.ResShare share)
        {
            var usr = _lstuserdetails.Find(t => t.UserName == share.touser);
            if (usr.PendingRequests.Find(t => t.scope == share.scope && t.user == share.user) == null)
            {
                usr.PendingRequests.Add(share);
            }
        }
        public static void RemoveRequest(Constants.Model.ResShare share)
        {
            var usr = _lstuserdetails.Find(t => t.UserName == share.user);
            var torem = usr.PendingRequests.Find(t => t.scope == share.scope && t.user == share.touser);
            if (torem != null)
                usr.PendingRequests.Remove(torem);
        }

        public static List<Constants.Model.UserDetails> GetDetails()
        {
            return _lstuserdetails;
        }

        static List<Constants.Model.UserDetails> _lstuserdetails = new List<Constants.Model.UserDetails>()
        {
            new Constants.Model.UserDetails()
            {
                UserName = "Sven",
                Clients = new List<string>()
                {
                    "ReliefExpress",
                    "Medinova-EHR",
                    "BMI-Device",
                },
                PendingRequests = new List<Constants.Model.ResShare>()
                {

                }
            },
            new Constants.Model.UserDetails()
            {
                UserName = "Kevin",
                Clients = new List<string>()
                {
                    "ReliefExpress",
                    "Medinova-EHR",
                },
                PendingRequests = new List<Constants.Model.ResShare>()
                {

                }
            },
            new Constants.Model.UserDetails()
            {
                UserName = "John",
                Clients = new List<string>()
                {
                    "ReliefExpress",
                    "BMI-Device",
                    "Medinova-EHR"
                },
                ScopeUsers = new Dictionary<string, List<string>>()
                {
                    {
                        "user.Observation", new List<string>()
                        {
                            "Sven"
                        }
                    },
                    {
                        "patient.MedicationOrder", new List<string>() // Lab data
                        {
                            "Sven"
                        }
                    }
                },
                PendingRequests = new List<Constants.Model.ResShare>()
                {

                }
            },
            new Constants.Model.UserDetails()
            {
                UserName = "Andrea",
                Clients = new List<string>()
                {
                    "ReliefExpress"
                },
                ScopeUsers = new Dictionary<string, List<string>>()
                {
                    {
                        "user.Observation", new List<string>()
                        {
                            
                        }
                    },
                    {
                        "patient.MedicationOrder", new List<string>() // Lab data
                        {
                            
                        }
                    }
                },
                PendingRequests = new List<Constants.Model.ResShare>()
                {

                }
            },
            new Constants.Model.UserDetails()
            {
                UserName = "Bob",
                Clients = new List<string>()
                {
                    "ReliefExpress"
                },
                ScopeUsers = new Dictionary<string, List<string>>()
                {
                    {
                        "user.Observation", new List<string>()
                        {
                            
                        }
                    },
                    {
                        "patient.MedicationOrder", new List<string>() // Lab data
                        {
                            
                        }
                    }
                },
                PendingRequests = new List<Constants.Model.ResShare>()
                {
                    new Constants.Model.ResShare()
                    {
                        scope = "user.Observation",
                        touser = "Bob",
                        user = "Sven"
                    }
                }
            }
        };
    }
}