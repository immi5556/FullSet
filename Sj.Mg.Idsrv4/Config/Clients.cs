﻿using IdentityServer3.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
                    },
                    IncludeJwtId = true,
                    AllowRememberConsent = false
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

        public static Client RegisterClients(NameValueCollection nv)
        {
            string clientid = nv["client_id"] ?? "";
            string clientname = nv["client_name"];
            string[] redircuri = new string[] {
                nv["redirect_uri"] ?? ""
            };
            string[] resptype = (nv["response_type"] ?? "").Split(' ');
            string[] scopes = (nv["scope"] ?? "").Split(' ');
            var nc = new Client
            {
                ClientId = string.IsNullOrEmpty(clientid) ? Guid.NewGuid().ToString().Replace("-", "") : clientid,
                ClientName = string.IsNullOrEmpty(clientname) ? "Client-" + Guid.NewGuid().ToString().Replace("-", "") : clientname,
                Flow = Flows.Hybrid,
                AllowedScopes = scopes.ToList(),
                ClientSecrets = new List<Secret>()
                {
                    new Secret()
                    {
                        Type = "RSA",
                        Value = ""
                    }
                },
                RedirectUris = redircuri.ToList(),
                PostLogoutRedirectUris = (redircuri ?? new string[] { }).ToList(),
                IncludeJwtId = true,
                AllowRememberConsent = false
            };
            _lstclients.Add(nc);
            return nc;
        }
    }
}