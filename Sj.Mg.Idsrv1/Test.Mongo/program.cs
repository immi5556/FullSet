using IdentityServer3.Core;
using Sj.Mg.Model;
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
            SelectFilt();
            //Update();
            Console.ReadKey();
        }
        static void SelectFilt()
        {
            var tt = Sj.Mg.Mongo.MongoManage.Select<Sj.Mg.Model.CustomUser>(GetCustomer(), "Users");
            foreach (var t in tt)
            {
                Console.WriteLine(t.Username);
            }
        }
        static void SelectDict()
        {
            Dictionary<string, object> filter = new Dictionary<string, object>();
            filter.Add("Username", "smith@smith.co");
            var tt = Sj.Mg.Mongo.MongoManage.Select<Sj.Mg.Model.CustomUser>(filter, "Users");
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
            Sj.Mg.Mongo.MongoManage.Update<Sj.Mg.Model.CustomUser>(tofind, tochange, "Users");
        }
        static void Insert()
        {
            Sj.Mg.Mongo.MongoManage.Insert<Sj.Mg.Model.CustomUser>(GetCustomer(), "Users");
        }

        static Sj.Mg.Model.CustomUser GetCustomer()
        {
            return new Sj.Mg.Model.CustomUser()
            {
                Subject = "88421113",
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
    }
}
