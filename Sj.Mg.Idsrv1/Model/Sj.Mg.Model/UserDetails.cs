using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sj.Mg.Model
{
    public class UserDetails
    {
        public UserDetails()
        {
            Current = new CustomUser();
            Clients = new List<UserClients>();
        }
        public CustomUser Current { get; set; }

        public List<UserClients> Clients { get; set; }
    }

    public class UserClients
    {
        public UserClients()
        {
            Client = new IdentityServer3.Core.Models.Client();
            AllowedUsers = new List<CustomUser>();
        }
        public IdentityServer3.Core.Models.Client Client { get; set; }
        public List<CustomUser> AllowedUsers { get; set; }
    }
}
