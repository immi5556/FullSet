using IdentityServer3.Core;
using Sj.Mg.CliLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Test.Mongo
{
    public class program
    {
        public static void Main()
        {
            //Insert();
            //SelectDict();
            //SelectFilt();
            //Update();
            PopulateUsers();
            //PopulateResource();
            PopulateAddlUsers();
            Console.ReadKey();
        }
        static void SelectFilt()
        {
            var tt = Sj.Mg.Mongo.MongoManage.Select<Sj.Mg.CliLib.Model.CustomUser>(GetCustomer(), "Users");
            foreach (var t in tt)
            {
                Console.WriteLine(t.Username);
            }
        }
        static void SelectDict()
        {
            Dictionary<string, object> filter = new Dictionary<string, object>();
            filter.Add("Username", "alice@bob.co");
            var tt = Sj.Mg.Mongo.MongoManage.Select<Sj.Mg.CliLib.Model.CustomUser>(filter, "Users");
            foreach(var t in tt)
            {
                Console.WriteLine(t.Username);
            }
        }
        static void Update()
        {
            var tofind = GetCustomer();
            var tochange = GetCustomer();
            tochange.Username = "feed@feed.co";
            Sj.Mg.Mongo.MongoManage.Update<Sj.Mg.CliLib.Model.CustomUser>(tofind, tochange, "Users");
        }
        static void Insert()
        {
            Sj.Mg.Mongo.MongoManage.Insert<Sj.Mg.CliLib.Model.CustomUser>(GetCustomer(), "Users");
        }

        static Sj.Mg.CliLib.Model.CustomUser GetCustomer()
        {
            return new Sj.Mg.CliLib.Model.CustomUser()
            {
                Subject = "bob@smith.co",
                Username = "bob@smith.co",
                Password = "bob",
                CustomClaims = new List<CustomClaim>()
                    {
                        new CustomClaim(Constants.ClaimTypes.Name, "Bob Smith"),
                        new CustomClaim(Constants.ClaimTypes.GivenName, "Bob"),
                        new CustomClaim(Constants.ClaimTypes.FamilyName, "Smith"),
                        new CustomClaim(Constants.ClaimTypes.Email, "BobSmith@email.com"),
                        new CustomClaim(Constants.ClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new CustomClaim(Constants.ClaimTypes.Role, "Developer"),
                        new CustomClaim(Constants.ClaimTypes.Role, "Geek"),
                        new CustomClaim(Constants.ClaimTypes.WebSite, "http://bob.com"),
                        new CustomClaim(Constants.ClaimTypes.Address, @"{ ""street_address"": ""One Hacker Way"", ""locality"": ""Heidelberg"", ""postal_code"": 69118, ""country"": ""Germany"" }", Constants.ClaimValueTypes.Json)
                    }
            };
        }
        static void PopulateAddlUsers()
        {
            Sj.Mg.Mongo.MongoManage.Insert<Sj.Mg.CliLib.Model.CustomUser>(new CustomUser()
            {
                Subject = "john@john.co",
                Username = "john@john.co",
                Password = "123",
                CustomClaims = new List<CustomClaim>()
                        {
                            new CustomClaim(Constants.ClaimTypes.Name, "John Doe"),
                            new CustomClaim(Constants.ClaimTypes.GivenName, "John"),
                            new CustomClaim(Constants.ClaimTypes.FamilyName, "Doe"),
                            new CustomClaim(Constants.ClaimTypes.Email, "john@john.com"),
                            new CustomClaim(Constants.ClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                            new CustomClaim(Constants.ClaimTypes.Role, "Parent"),
                            new CustomClaim(Constants.ClaimTypes.Role, "Patient"),
                            new CustomClaim(Constants.ClaimTypes.WebSite, "http://johndoe.com"),
                            new CustomClaim(Constants.ClaimTypes.Address, @"{ ""street_address"": ""some st."", ""locality"": ""place"", ""postal_code"": 69118, ""country"": ""USA"" }", Constants.ClaimValueTypes.Json)
                        }
            }, "Users");

            Sj.Mg.Mongo.MongoManage.Insert<Sj.Mg.CliLib.Model.CustomUser>(new CustomUser()
            {
                Subject = "sam@sam.co",
                Username = "sam@sam.co",
                Password = "123",
                CustomClaims = new List<CustomClaim>()
                        {
                            new CustomClaim(Constants.ClaimTypes.Name, "Sam pari"),
                            new CustomClaim(Constants.ClaimTypes.GivenName, "Sam"),
                            new CustomClaim(Constants.ClaimTypes.FamilyName, "Pari"),
                            new CustomClaim(Constants.ClaimTypes.Email, "sam@sam.com"),
                            new CustomClaim(Constants.ClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                            new CustomClaim(Constants.ClaimTypes.Role, "Parent"),
                            new CustomClaim(Constants.ClaimTypes.Role, "Patient"),
                            new CustomClaim(Constants.ClaimTypes.WebSite, "http://sampari.com"),
                            new CustomClaim(Constants.ClaimTypes.Address, @"{ ""street_address"": ""some st."", ""locality"": ""place"", ""postal_code"": 69118, ""country"": ""USA"" }", Constants.ClaimValueTypes.Json)
                        }
            }, "Users");

            Sj.Mg.Mongo.MongoManage.Insert<Sj.Mg.CliLib.Model.CustomUser>(new CustomUser()
            {
                Subject = "saphire@saphire.co",
                Username = "saphire@saphire.co",
                Password = "123",
                CustomClaims = new List<CustomClaim>()
                        {
                            new CustomClaim(Constants.ClaimTypes.Name, "Saphire Gold"),
                            new CustomClaim(Constants.ClaimTypes.GivenName, "Saphire"),
                            new CustomClaim(Constants.ClaimTypes.FamilyName, "Gold"),
                            new CustomClaim(Constants.ClaimTypes.Email, "saphire@saphire.com"),
                            new CustomClaim(Constants.ClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                            new CustomClaim(Constants.ClaimTypes.Role, "Parent"),
                            new CustomClaim(Constants.ClaimTypes.Role, "Patient"),
                            new CustomClaim(Constants.ClaimTypes.WebSite, "http://saphire.com"),
                            new CustomClaim(Constants.ClaimTypes.Address, @"{ ""street_address"": ""some st."", ""locality"": ""place"", ""postal_code"": 69118, ""country"": ""USA"" }", Constants.ClaimValueTypes.Json)
                        }
            }, "Users");
        }
        static void PopulateUsers()
        {
            Sj.Mg.Mongo.MongoManage.Insert<Sj.Mg.CliLib.Model.CustomUser>(new CustomUser()
            {
                Subject = "alice@bob.co",
                Username = "alice@bob.co",
                Password = "alice",
                CustomClaims = new List<CustomClaim>()
                        {
                            new CustomClaim(Constants.ClaimTypes.Name, "Alice Smith"),
                            new CustomClaim(Constants.ClaimTypes.GivenName, "Alice"),
                            new CustomClaim(Constants.ClaimTypes.FamilyName, "Smith"),
                            new CustomClaim(Constants.ClaimTypes.Email, "AliceSmith@email.com"),
                            new CustomClaim(Constants.ClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                            new CustomClaim(Constants.ClaimTypes.Role, "Parent"),
                            new CustomClaim(Constants.ClaimTypes.Role, "Patient"),
                            new CustomClaim(Constants.ClaimTypes.WebSite, "http://alice.com"),
                            new CustomClaim(Constants.ClaimTypes.Address, @"{ ""street_address"": ""One Hacker Way"", ""locality"": ""Heidelberg"", ""postal_code"": 69118, ""country"": ""Germany"" }", Constants.ClaimValueTypes.Json)
                        }
            }, "Users");

            Sj.Mg.Mongo.MongoManage.Insert<Sj.Mg.CliLib.Model.CustomUser>(new CustomUser()
            {
                Subject = "bob@bob.co",
                Username = "bob@bob.co",
                Password = "bob",
                CustomClaims = new List<CustomClaim>()
                        {
                            new CustomClaim(Constants.ClaimTypes.Name, "Bob Smith"),
                            new CustomClaim(Constants.ClaimTypes.GivenName, "Bob"),
                            new CustomClaim(Constants.ClaimTypes.FamilyName, "Smith"),
                            new CustomClaim(Constants.ClaimTypes.Email, "BobSmith@email.com"),
                            new CustomClaim(Constants.ClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                            new CustomClaim(Constants.ClaimTypes.Role, "Patient"),
                            new CustomClaim(Constants.ClaimTypes.WebSite, "http://bob.com"),
                            new CustomClaim(Constants.ClaimTypes.Address, @"{ ""street_address"": ""One Hacker Way"", ""locality"": ""Heidelberg"", ""postal_code"": 69118, ""country"": ""Germany"" }", Constants.ClaimValueTypes.Json)
                        }
            }, "Users");
            Sj.Mg.Mongo.MongoManage.Insert<Sj.Mg.CliLib.Model.CustomUser>(new CustomUser()
            {
                Subject = "admin@medgrotto.com",
                Username = "admin@medgrotto.com",
                Password = "123",
                CustomClaims = new List<CustomClaim>()
                        {
                            new CustomClaim(Constants.ClaimTypes.Name, "Admin MG"),
                            new CustomClaim(Constants.ClaimTypes.GivenName, "Admin"),
                            new CustomClaim(Constants.ClaimTypes.FamilyName, "Medgrotto"),
                            new CustomClaim(Constants.ClaimTypes.Email, "admin@medgrotto.com"),
                            new CustomClaim(Constants.ClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                            new CustomClaim(Constants.ClaimTypes.Role, "Admin"),
                            new CustomClaim(Constants.ClaimTypes.Role, "Geek"),
                            new CustomClaim(Constants.ClaimTypes.WebSite, "http://medgrotto.com"),
                            new CustomClaim(Constants.ClaimTypes.Address, @"{ ""street_address"": ""One Hacker Way"", ""locality"": ""Texas"", ""postal_code"": 69118, ""country"": ""USA"" }", Constants.ClaimValueTypes.Json)
                        }
            }, "Users");
        }
        static void PopulateResource()
        {
            Sj.Mg.Mongo.MongoManage.Insert<Sj.Mg.CliLib.Model.resource>(new resource()
            {
                resource_set_id = "Account",
                scopes = new List<string>()
                {
                    "Patient/Account.*",
                    "Patient/Account.Read",
                    "Patient/Account.Write",
                    "Patient/Account.Share"
                }
            }, "Resources");

            Sj.Mg.Mongo.MongoManage.Insert<Sj.Mg.CliLib.Model.resource>(new resource()
            {
                resource_set_id = "Medication",
                scopes = new List<string>()
                {
                    "Patient/Medication.*",
                    "Patient/Medication.Read",
                    "Patient/Medication.Write",
                    "Patient/Medication.Share"
                }
            }, "Resources");

            Sj.Mg.Mongo.MongoManage.Insert<Sj.Mg.CliLib.Model.resource>(new resource()
            {
                resource_set_id = "Observation",
                scopes = new List<string>()
                {
                    "Patient/Observation.*",
                    "Patient/Observation.Read",
                    "Patient/Observation.Write",
                    "Patient/Observation.Share"
                }
            }, "Resources");

            Sj.Mg.Mongo.MongoManage.Insert<Sj.Mg.CliLib.Model.resource>(new resource()
            {
                resource_set_id = "Demographic",
                scopes = new List<string>()
                {
                    "Patient/Patient.*",
                    "Patient/Patient.Read",
                    "Patient/Patient.Write",
                    "Patient/Patient.Share"
                }
            }, "Resources");
        }
        static void PopulateUserDetails()
        {

        }
    }
}
