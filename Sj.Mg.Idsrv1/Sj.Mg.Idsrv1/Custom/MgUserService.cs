using IdentityServer3.Core;
using IdentityServer3.Core.Extensions;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services.Default;
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
        public class CustomUser
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public bool AcceptedEula { get; set; }
            public string Subject { get; set; }
            public string Provider { get; set; }
            public string ProviderID { get; set; }
            public bool IsRegistered { get; set; }
            public List<Claim> Claims { get; set; }
        }
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
                        new Claim(Constants.ClaimTypes.Role, "Admin"),
                        new Claim(Constants.ClaimTypes.Role, "Geek"),
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
                        new Claim(Constants.ClaimTypes.Role, "Developer"),
                        new Claim(Constants.ClaimTypes.Role, "Geek"),
                        new Claim(Constants.ClaimTypes.WebSite, "http://bob.com"),
                        new Claim(Constants.ClaimTypes.Address, @"{ ""street_address"": ""One Hacker Way"", ""locality"": ""Heidelberg"", ""postal_code"": 69118, ""country"": ""Germany"" }", Constants.ClaimValueTypes.Json)
                    }
            }
        };

        public override Task AuthenticateLocalAsync(LocalAuthenticationContext context)
        {
            var user = Users.SingleOrDefault(x => x.Username == context.UserName && x.Password == context.Password);
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
    }
}