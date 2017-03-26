using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sj.Mg.CliLib.Model
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

            AllowedUsers = new Dictionary<string, Dictionary<string, List<string>>>();
            Client = new List<string>();
        }
        public string MyEmail { get; set; }
        public List<string> Client { get; set; }
        //Client -> Scope -> UsersList 
        public Dictionary<string, Dictionary<string, List<string>>> AllowedUsers { get; set; }
    }
}
