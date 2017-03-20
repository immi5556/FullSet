using IdentityServer3.Core;
using IdentityServer3.Core.Extensions;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services.Default;
using MongoDB.Bson;
using MongoDB.Driver;
using Sj.Mg.Model;
using Sj.Mg.Mongo;
using Sj.Mg.Mongo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace Sj.Mg.Idsrv1.Custom
{
    public class MgUserService : UserServiceBase
    {
        public static List<CustomUser> Users = new List<CustomUser>()
        {
             new CustomUser()
                {
                    Subject = "818727", Username = "alice@bob.co", Password = "alice",
                        Claims = new List<Claim>()
                        {
                            new Claim(Constants.ClaimTypes.Name, "Alice Smith"),
                            new Claim(Constants.ClaimTypes.GivenName, "Alice"),
                            new Claim(Constants.ClaimTypes.FamilyName, "Smith"),
                            new Claim(Constants.ClaimTypes.Email, "AliceSmith@email.com"),
                            new Claim(Constants.ClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                            new Claim(Constants.ClaimTypes.Role, "Parent"),
                            new Claim(Constants.ClaimTypes.Role, "Patient"),
                            new Claim(Constants.ClaimTypes.WebSite, "http://alice.com"),
                            new Claim(Constants.ClaimTypes.Address, @"{ ""street_address"": ""One Hacker Way"", ""locality"": ""Heidelberg"", ""postal_code"": 69118, ""country"": ""Germany"" }", Constants.ClaimValueTypes.Json)
                        }
                },
                new CustomUser()
                {
                    Subject = "88421113", Username = "bob@bob.co", Password = "bob",
                        Claims = new List<Claim>()
                        {
                            new Claim(Constants.ClaimTypes.Name, "Bob Smith"),
                            new Claim(Constants.ClaimTypes.GivenName, "Bob"),
                            new Claim(Constants.ClaimTypes.FamilyName, "Smith"),
                            new Claim(Constants.ClaimTypes.Email, "BobSmith@email.com"),
                            new Claim(Constants.ClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                            new Claim(Constants.ClaimTypes.Role, "Patient"),
                            new Claim(Constants.ClaimTypes.WebSite, "http://bob.com"),
                            new Claim(Constants.ClaimTypes.Address, @"{ ""street_address"": ""One Hacker Way"", ""locality"": ""Heidelberg"", ""postal_code"": 69118, ""country"": ""Germany"" }", Constants.ClaimValueTypes.Json)
                        }
                },
                new CustomUser()
                {
                    Subject = "818956", Username = "admin@medgrotto.com", Password = "123",
                        Claims = new List<Claim>()
                        {
                            new Claim(Constants.ClaimTypes.Name, "Admin MG"),
                            new Claim(Constants.ClaimTypes.GivenName, "Admin"),
                            new Claim(Constants.ClaimTypes.FamilyName, "Medgrotto"),
                            new Claim(Constants.ClaimTypes.Email, "admin@medgrotto.com"),
                            new Claim(Constants.ClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                            new Claim(Constants.ClaimTypes.Role, "Admin"),
                            new Claim(Constants.ClaimTypes.Role, "Geek"),
                            new Claim(Constants.ClaimTypes.WebSite, "http://medgrotto.com"),
                            new Claim(Constants.ClaimTypes.Address, @"{ ""street_address"": ""One Hacker Way"", ""locality"": ""Texas"", ""postal_code"": 69118, ""country"": ""USA"" }", Constants.ClaimValueTypes.Json)
                        }
                }
        };

        public override Task AuthenticateLocalAsync(LocalAuthenticationContext context)
        {
            Dictionary<string, object> filter = new Dictionary<string, object>();
            filter.Add("Username", context.UserName);
            filter.Add("Password", context.Password);
            var tt = Sj.Mg.Mongo.MongoManage.Select<Sj.Mg.Model.CustomUser>(filter, "Users");
            var user = (tt == null || tt.Count == 0) ? null : tt[0];
            //var user = Users.SingleOrDefault(x => x.Username == context.UserName && x.Password == context.Password);
            if (user != null)
            {
                if (user.AcceptedEula)
                {
                    context.AuthenticateResult = new AuthenticateResult(user.Subject, user.Username);
                }
                else
                {
                    context.AuthenticateResult = new AuthenticateResult("~/eula", user.Subject, user.Username);
                }
            }

            return Task.FromResult(0);
        }
        public override Task AuthenticateExternalAsync(ExternalAuthenticationContext context)
        {
            // look for the user in our local identity system from the external identifiers
            var user = Users.SingleOrDefault(x => x.Provider == context.ExternalIdentity.Provider && x.ProviderID == context.ExternalIdentity.ProviderId);
            string name = "Unknown";
            if (user == null)
            {
                if (true) //TO DO: make sure condition is applied wheter auto register or 
                {
                    // new user, so add them here
                    var nameClaim = context.ExternalIdentity.Claims.First(x => x.Type == Constants.ClaimTypes.Name);
                    if (nameClaim != null) name = nameClaim.Value;

                    user = new CustomUser
                    {
                        Subject = Guid.NewGuid().ToString(),
                        Provider = context.ExternalIdentity.Provider,
                        ProviderID = context.ExternalIdentity.ProviderId,
                        Claims = context.ExternalIdentity.Claims.ToList()
                        //Claims = new List<Claim> { new Claim(Constants.ClaimTypes.Name, name) }
                    };
                    Users.Add(user);
                }
                else
                {
                    // user is not registered so redirect
                    context.AuthenticateResult = new AuthenticateResult("~/registerfirstexternalregistration", context.ExternalIdentity);
                }
            }

            name = user.Claims.First(x => x.Type == Constants.ClaimTypes.Name).Value;

            if (user.IsRegistered)
            {
                // user is registered so continue
                context.AuthenticateResult = new AuthenticateResult(user.Subject, name, identityProvider: user.Provider);
            }
            else
            {
                // user not registered so we will issue a partial login and redirect them to our registration page
                context.AuthenticateResult = new AuthenticateResult("~/externalregistration", user.Subject, name, identityProvider: user.Provider);
            }

            return Task.FromResult(0);
        }

        public override Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            // issue the claims for the user
            var user = Users.SingleOrDefault(x => x.Subject == context.Subject.GetSubjectId());
            if (user != null)
            {
                context.IssuedClaims = user.Claims.Where(x => context.RequestedClaimTypes.Contains(x.Type));
            }

            return Task.FromResult(0);
        }

        public string addUser(string firstName, string lastName, string password, string email, string phoneNumber)
        {
            var database = BaseMongo.GetDatabase();
            var collection = database.GetCollection<BsonDocument>("Users");

            var filter = Builders<BsonDocument>.Filter.Eq("Username", email);

            var response = collection.Find(filter).ToList();
            if (response.Count == 0)
            {
                var newUser = GetUser(firstName, lastName, password, email, phoneNumber);
                MongoManage.Insert<CustomUser>(newUser, "Users");
                Users.Add(newUser);
                return "success";
            }
            else
            {
                return "exists";
            }
        }

        public CustomUser GetUser(string firstName, string lastName, string password, string email, string phoneNumber)
        {
            return new CustomUser
            {
                Subject = Guid.NewGuid().ToString(),
                Username = email,
                Password = password,
                AcceptedEula = false,
                Provider = null,
                ProviderID = null,
                IsRegistered = false,
                Claims = new List<Claim>()
                        {
                            new Claim(Constants.ClaimTypes.Name, "Alice Smith"),
                            new Claim(Constants.ClaimTypes.GivenName, firstName),
                            new Claim(Constants.ClaimTypes.FamilyName, lastName),
                            new Claim(Constants.ClaimTypes.Email, email),
                        }
            };
        }
    }
}