using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppConstants.Model
{
    public class UmaWellKnownConfig
    {
        public string version { get; set; }
        public string issuer { get; set; }
        public List<string> pat_profiles_supported { get; set; }
        public List<string> aat_profiles_supported { get; set; }
        public List<string> rpt_profiles_supported { get; set; }
        public List<string> pat_grant_types_supported { get; set; }
        public List<string> aat_grant_types_supported { get; set; }
        public string token_endpoint { get; set; }
        public string authorization_endpoint { get; set; }
        public string introspection_endpoint { get; set; }
        public string resource_set_registration_endpoint { get; set; }
        public string permission_registration_endpoint { get; set; }
        public string claim_token_profiles_supported { get; set; }
        public string uma_profiles_supported { get; set; }
        public string dynamic_client_endpoint { get; set; }
        public string requesting_party_claims_endpoint { get; set; }
        public string rpt_endpoint { get; set; }
    }
}