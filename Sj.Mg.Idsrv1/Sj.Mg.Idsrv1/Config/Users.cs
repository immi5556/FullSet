using IdentityServer3.Core;
using IdentityServer3.Core.Services.InMemory;
using Sj.Mg.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace Sj.Mg.Idsrv1.Config
{
    public class Users
    {
        public static List<CustomUser> Get()
        {
            var users = new List<CustomUser>()
            {
               
            };

            return users;
        }
    }
}