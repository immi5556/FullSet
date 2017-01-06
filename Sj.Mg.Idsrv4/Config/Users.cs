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
        public static List<InMemoryUser> Get()
        {
            return new List<InMemoryUser>()
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
                }
            };
        }
    }
}