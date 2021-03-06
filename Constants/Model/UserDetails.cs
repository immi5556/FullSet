﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constants.Model
{
    public class UserDetails
    {
        public UserDetails()
        {
            Clients = new List<string>();
            ScopeUsers = new Dictionary<string, List<string>>();
            PendingRequests = new List<ResShare>();
        }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Subject { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string Address { get; set; }
        public string Role { get; set; }     
        public List<string> Clients { get; set; }
        public Dictionary<string, List<string>> ScopeUsers { get; set; }
        public List<ResShare> PendingRequests { get; set; }
    }
}
