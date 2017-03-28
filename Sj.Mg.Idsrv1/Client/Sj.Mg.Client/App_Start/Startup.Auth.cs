using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using System.Collections.Generic;
using Microsoft.Owin.Security.OpenIdConnect;
using System.Security.Claims;
using Microsoft.Owin.Security;
using System.IdentityModel.Tokens;

namespace Sj.Mg.Client
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                (s, cert, chain, sslPolicyErrors) => true;
            Sj.Mg.CliLib.Initializer.ConfigureClientAuth(app, "MgClient1");
        }

        public static void StartLocal(IAppBuilder app)
        {
            //JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();
            //app.UseCookieAuthentication(new CookieAuthenticationOptions
            //{
            //    AuthenticationType = "Cookies"
            //});

            //app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            //{
            //    ClientId = "MgClient1",
            //    Authority = AppConstants.Constants.Sts,
            //    RedirectUri = AppConstants.Constants.ReClientMvc,
            //    SignInAsAuthenticationType = "Cookies",
            //    ResponseType = "code id_token token",
            //    Scope = "openid profile address patient.MedicationOrder roles uma_authorization",
            //    PostLogoutRedirectUri = AppConstants.Constants.ReClientMvc,
            //    Notifications = new OpenIdConnectAuthenticationNotifications()
            //    {

            //        SecurityTokenValidated = async n =>
            //        {
            //            AppConstants.TokenHelper.DecodeAndWrite(n.ProtocolMessage.IdToken);
            //            AppConstants.TokenHelper.DecodeAndWrite(n.ProtocolMessage.AccessToken);

            //            var givenNameClaim = n.AuthenticationTicket
            //                .Identity.FindFirst(IdentityModel.JwtClaimTypes.GivenName);

            //            var familyNameClaim = n.AuthenticationTicket
            //                .Identity.FindFirst(IdentityModel.JwtClaimTypes.FamilyName);

            //            var subClaim = n.AuthenticationTicket
            //                .Identity.FindFirst(IdentityModel.JwtClaimTypes.Subject);

            //            var roleClaim = n.AuthenticationTicket
            //                .Identity.FindFirst(IdentityModel.JwtClaimTypes.Role);

            //            //create a new claims, issuer + sub as unique identifier
            //            var nameClaim = new Claim(IdentityModel.JwtClaimTypes.Name,
            //                        AppConstants.Constants.IssuerUri + subClaim.Value);

            //            var newClaimsIdentity = new ClaimsIdentity(
            //               n.AuthenticationTicket.Identity.AuthenticationType,
            //               IdentityModel.JwtClaimTypes.Name,
            //               IdentityModel.JwtClaimTypes.Role);

            //            if (nameClaim != null)
            //            {
            //                newClaimsIdentity.AddClaim(nameClaim);
            //            }

            //            if (givenNameClaim != null)
            //            {
            //                newClaimsIdentity.AddClaim(givenNameClaim);
            //            }

            //            if (familyNameClaim != null)
            //            {
            //                newClaimsIdentity.AddClaim(familyNameClaim);
            //            }

            //            if (roleClaim != null)
            //            {
            //                newClaimsIdentity.AddClaim(roleClaim);
            //            }

            //            newClaimsIdentity.AddClaim(new Claim("access_token", n.ProtocolMessage.AccessToken));

            //            newClaimsIdentity.AddClaim(new Claim("id_token", n.ProtocolMessage.IdToken));

            //            //     create a new authentication ticket, overwriting the old one.
            //            n.AuthenticationTicket = new AuthenticationTicket(
            //                                     newClaimsIdentity,
            //                                     n.AuthenticationTicket.Properties);
            //        },
            //        RedirectToIdentityProvider = async n =>
            //        {
            //            if (n.ProtocolMessage.RequestType == Microsoft.IdentityModel.Protocols.OpenIdConnectRequestType
            //            .LogoutRequest)
            //            {
            //                var identhinr = n.OwinContext.Authentication.User.FindFirst("id_token");
            //                if (identhinr != null)
            //                {
            //                    n.ProtocolMessage.IdTokenHint = identhinr.Value;
            //                }
            //            }
            //        }
            //    }
            //});
        }
    }
}