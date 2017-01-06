using IdentityServer3.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sj.Mg.Idsrv4.Config
{
    public class Clients
    {
        static List<Client> _lstclients = new List<Client>
            {
                new Client
                {
                    ClientId = "ReliefExpress",
                    ClientName = "Relief Express (Hybrid)",
                    Flow = Flows.Hybrid,
                    AllowAccessToAllScopes = true,
                    RedirectUris = new List<string>
                    {
                        AppConstants.Constants.ReClientMvc
                    },
                    PostLogoutRedirectUris = new List<string>()
                    {
                        AppConstants.Constants.ReClientMvc
                    }
                },
                new Client
                {
                     ClientId = "ReliefExpress-Api",
                     ClientName = "ReliefExpress-Api (Authorization Code)",
                     Flow = Flows.ClientCredentials,
                     //AllowAccessToAllScopes = true,
                     AllowedScopes = new List<string>
                     {
                         "patient.MedicationOrder",
                         "uma_protection",
                         "user.Observation"
                     },
                    // redirect = URI of the MVC application callback
                    RedirectUris = new List<string>
                    {
                        AppConstants.Constants.ReClientMvcStsCallback
                    },           

                    // client secret
                    ClientSecrets = new List<Secret>()
                    {
                        new Secret(AppConstants.Constants.ClientSecret.Sha256())
                    }
                }
            };
        public static IEnumerable<Client> Get()
        {
            return _lstclients;
        }

        public static void RegisterClients()
        {

        }
    }
}