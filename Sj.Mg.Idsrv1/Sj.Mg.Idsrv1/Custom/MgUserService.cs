using Facebook;
using IdentityServer3.Core;
using IdentityServer3.Core.Extensions;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services.Default;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sj.Mg.CliLib.Model;
using Sj.Mg.Mongo;
using Sj.Mg.Mongo.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace Sj.Mg.Idsrv1.Custom
{
    public class MgUserService : UserServiceBase
    {
        
        

        public override Task AuthenticateLocalAsync(LocalAuthenticationContext context)
        {
            Dictionary<string, object> filter = new Dictionary<string, object>();
            filter.Add("Username", context.UserName);
            filter.Add("Password", context.Password);
            var tt = Sj.Mg.Mongo.MongoManage.Select<Sj.Mg.CliLib.Model.CustomUser>(filter, "Users");
            var user = (tt == null || tt.Count == 0) ? null : tt[0];
            //var user = Users.SingleOrDefault(x => x.Username == context.UserName && x.Password == context.Password);
            if (user != null)
            {
                var database = BaseMongo.GetDatabase();
                var collection = database.GetCollection<BsonDocument>("UsersClientsData");

                var filter2 = Builders<BsonDocument>.Filter.Eq("email", context.UserName);

                var response = collection.Find(filter2).ToList();
                if (response.Count == 0)
                {
                    UserClientsList data = GetUserClients(user.Id, context.UserName);
                    MongoManage.Insert<UserClientsList>(data, "UsersClientsData");
                }
                if (user.Questions != null)
                {
                    context.AuthenticateResult = new AuthenticateResult("~/loginchallenge", user.Subject, user.Username);
                    //context.AuthenticateResult = new AuthenticateResult(user.Subject, user.Username);
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
            //var user = Users.SingleOrDefault(x => x.Provider == context.ExternalIdentity.Provider && x.ProviderID == context.ExternalIdentity.ProviderId);
            string name = "Unknown";
            string email = "";
            if (context.ExternalIdentity.Provider == "Facebook")
            {
                var claimsList = context.ExternalIdentity.Claims.ToList();
                var extraClaims = GetAdditionalFacebookClaims(context.ExternalIdentity.Claims.First(claim => claim.Type == "urn:facebook:access_token"));
                email = extraClaims.SelectToken("email").ToString();
                claimsList.Add(new Claim("email", extraClaims.SelectToken("email").ToString()));
                claimsList.Add(new Claim("given_name", extraClaims.SelectToken("first_name").ToString()));
                claimsList.Add(new Claim("family_name", extraClaims.SelectToken("last_name").ToString()));
                context.ExternalIdentity.Claims = claimsList;
            }
            if (context.ExternalIdentity.Provider == "Google")
            {
                email = context.ExternalIdentity.Claims.First(x => x.Type == Constants.ClaimTypes.Email).Value;
            }
            if (context.ExternalIdentity.Provider == "Twitter")
            {
                email = context.ExternalIdentity.Claims.First(x => x.Type == Constants.ClaimTypes.Email).Value;
            }

            Dictionary<string, object> filter = new Dictionary<string, object>();
            //filter.Add("Provider", context.ExternalIdentity.Provider);
            //filter.Add("ProviderID", context.ExternalIdentity.ProviderId);
            filter.Add("Subject", email);
            var tt = Sj.Mg.Mongo.MongoManage.Select<Sj.Mg.CliLib.Model.CustomUser>(filter, "Users");
            CustomUser user = (tt == null || tt.Count == 0) ? null : tt[0];
            if (user == null)
            {
                if (true) //TO DO: make sure condition is applied wheter auto register or 
                {
                    // new user, so add them here
                    var nameClaim = context.ExternalIdentity.Claims.First(x => x.Type == Constants.ClaimTypes.Name);
                    if (nameClaim != null) name = nameClaim.Value;

                    user = new CustomUser()
                    {
                        Subject = email,
                        Username = email,
                        Provider = context.ExternalIdentity.Provider,
                        ProviderID = context.ExternalIdentity.ProviderId,
                        //Claims = context.ExternalIdentity.Claims.ToList()
                        //Claims = new List<Claim> { new Claim(Constants.ClaimTypes.Name, name) }
                    };
                    context.ExternalIdentity.Claims.ToList().ForEach(t =>
                    {
                        user.CustomClaims.Add(new CustomClaim(t.Type, t.Value));
                    });
                    Sj.Mg.Mongo.MongoManage.Insert<CustomUser>(user, "Users");

                    var database = BaseMongo.GetDatabase();
                    var collection = database.GetCollection<BsonDocument>("UsersClientsData");

                    var filter2 = Builders<BsonDocument>.Filter.Eq("email", email);

                    var response = collection.Find(filter2).ToList();
                    if (response.Count == 0)
                    {
                        UserClientsList data = GetUserClients(user.Id, email);
                        MongoManage.Insert<UserClientsList>(data, "UsersClientsData");
                    }
                    //Users.Add(user);
                }
                else
                {
                    // user is not registered so redirect -- TO DO: confirm  do we need this
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
        protected static Newtonsoft.Json.Linq.JObject GetAdditionalFacebookClaims(Claim accessToken)
        {
            var fb = new FacebookClient(accessToken.Value);
            //var tt = fb.Get("me", new { fields = new[] { "email", "first_name", "last_name" } });
            fb.AppSecret = "e29cd683ecbf9df1d5adf6fe35d9cc50";
            fb.AppId = "1909035272666825";
            var tt = fb.Get("me?fields=id,name,email");
            return Newtonsoft.Json.Linq.JObject.FromObject(tt);
        }
        public override Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var dict = new Dictionary<string, object>();
            dict.Add("Subject", context.Subject.GetSubjectId());
            // issue the claims for the user
            var user = Sj.Mg.Mongo.MongoManage.Select<CustomUser>(dict, "Users").FirstOrDefault();
            if (user != null)
            {
                context.IssuedClaims = user.Claims.Where(x => context.RequestedClaimTypes.Contains(x.Type));
            }

            return Task.FromResult(0);
        }

        public string addUser(string firstName, string lastName, string password, string email, string phoneNumber, bool provider, string npi, string address, string ans1, string ans2, string ans3, string ans4, string ans5, string ans6, string ans7)
        {

            var database = BaseMongo.GetDatabase();
            var collection = database.GetCollection<BsonDocument>("Users");

            var filter = Builders<BsonDocument>.Filter.Eq("Username", email);

            var response = collection.Find(filter).ToList();
            if (response.Count == 0)
            {
                try
                {
                    var newUser = GetUser(firstName, lastName, password, email, phoneNumber, provider, npi, address, ans1, ans2, ans3, ans4, ans5, ans6, ans7);
                    MongoManage.Insert<CustomUser>(newUser, "Users");
                    Dictionary<string, object> filter1 = new Dictionary<string, object>();
                    filter1.Add("Subject", email);
                    var tt = Sj.Mg.Mongo.MongoManage.Select<Sj.Mg.CliLib.Model.CustomUser>(filter1, "Users");
                    CustomUser user = (tt == null || tt.Count == 0) ? null : tt[0];
                    if (user != null)
                    {
                        UserClientsList data = GetUserClients(user.Id, email);
                        MongoManage.Insert<UserClientsList>(data, "UsersClientsData");
                    }
                    
                }
                catch (Exception ex)
                {

                    string filePath = @"D:\_deploy\_mg_idrv\error.txt";

                    using (StreamWriter writer = new StreamWriter(filePath, true))
                    {
                        writer.WriteLine("Message :" + ex.Message + "<br/>" + Environment.NewLine + "StackTrace :" + ex.StackTrace +
                           "" + Environment.NewLine + "Date :" + DateTime.Now.ToString());
                        writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
                    }
                }
                return "success";
            }
            else
            {
                return "exists";
            }
        }

        public CustomUser GetUser(string firstName, string lastName, string password, string email, string phoneNumber, bool provider, string npi, string address, string ans1, string ans2, string ans3, string ans4, string ans5, string ans6, string ans7)
        {
             CustomUser ab = new CustomUser
            {
                //Subject = Guid.NewGuid().ToString(),
                Subject = email,
                Username = email,
                Password = password,
                AcceptedEula = false,
                Provider = null,
                ProviderID = null,
                IsRegistered = false,
                IsProvider = provider,
                NPI = npi,
                Address = address,
                Claims = new List<Claim>()
                {
                    new Claim(Constants.ClaimTypes.Name, firstName+" "+lastName),
                    new Claim(Constants.ClaimTypes.GivenName, firstName),
                    new Claim(Constants.ClaimTypes.FamilyName, lastName),
                    new Claim(Constants.ClaimTypes.Email, email),
                },
                CustomClaims = new List<CustomClaim>()
                {
                    new CustomClaim(Constants.ClaimTypes.Name, email),
                    new CustomClaim(Constants.ClaimTypes.GivenName, firstName),
                    new CustomClaim(Constants.ClaimTypes.FamilyName, lastName),
                    new CustomClaim(Constants.ClaimTypes.Email, email)
                },
                 Questions = new List<Question>()
                {
                    new Question() {
                        Ques = Constants.Questions.Question1,
                        Ans = ans1
                    },
                    new Question() {
                        Ques = Constants.Questions.Question2,
                        Ans = ans2
                    },
                    new Question() {
                        Ques = Constants.Questions.Question3,
                        Ans = ans3
                    },
                    new Question() {
                        Ques = Constants.Questions.Question4,
                        Ans = ans4
                    },
                    new Question() {
                        Ques = Constants.Questions.Question5,
                        Ans = ans5
                    },
                    new Question() {
                        Ques = Constants.Questions.Question6,
                        Ans = ans6
                    },
                    new Question() {
                        Ques = Constants.Questions.Question7,
                        Ans = ans7
                    },
                }
             };
            return ab;
        }

        public UserClientsList GetUserClients(ObjectId id, string email)
        {
            var fpath = HttpContext.Current.Server.MapPath("/App_Data/tabsData.json");
            //local: E:\Vamsi\Medgrotto\FullSet\Sj.Mg.Idsrv1\Sj.Mg.Idsrv1\Content\medg\js\tabsData.json
            //server: D:\_deploy\_mg_idrv\Content\medg\js\tabsData.json
            string data = System.IO.File.ReadAllText(fpath);
            var obj= JsonConvert.DeserializeObject<List<UserClientsData>>(data);
            UserClientsList userClnts = new UserClientsList();
            userClnts.userId = id.ToString();
            userClnts.email = email;
            userClnts.UserClientsData = obj;
            return userClnts;
        }
    }
}