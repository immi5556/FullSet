using IdentityModel.Client;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

[assembly: OwinStartupAttribute(typeof(Relief.Express.Mvc.Startup))]
namespace Relief.Express.Mvc
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies"
            });
            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                ClientId = "ReliefExpress",
                Authority = AppConstants.Constants.Sts,
                RedirectUri = AppConstants.Constants.ReClientMvc,
                SignInAsAuthenticationType = "Cookies",
                ResponseType = "code id_token token",
                Scope = "openid profile address patient.MedicationOrder roles uma_authorization",
                PostLogoutRedirectUri = AppConstants.Constants.ReClientMvc,
                Notifications = new OpenIdConnectAuthenticationNotifications()
                {

                    SecurityTokenValidated = async n =>
                    {
                        Helper.TokenHelper.DecodeAndWrite(n.ProtocolMessage.IdToken);
                        Helper.TokenHelper.DecodeAndWrite(n.ProtocolMessage.AccessToken);

                        var givenNameClaim = n.AuthenticationTicket
                            .Identity.FindFirst(IdentityModel.JwtClaimTypes.GivenName);

                        var familyNameClaim = n.AuthenticationTicket
                            .Identity.FindFirst(IdentityModel.JwtClaimTypes.FamilyName);

                        var subClaim = n.AuthenticationTicket
                            .Identity.FindFirst(IdentityModel.JwtClaimTypes.Subject);

                        var roleClaim = n.AuthenticationTicket
                            .Identity.FindFirst(IdentityModel.JwtClaimTypes.Role);

                        //create a new claims, issuer + sub as unique identifier
                       var nameClaim = new Claim(IdentityModel.JwtClaimTypes.Name,
                                   AppConstants.Constants.IssuerUri + subClaim.Value);

                        var newClaimsIdentity = new ClaimsIdentity(
                           n.AuthenticationTicket.Identity.AuthenticationType,
                           IdentityModel.JwtClaimTypes.Name,
                           IdentityModel.JwtClaimTypes.Role);

                        if (nameClaim != null)
                        {
                            newClaimsIdentity.AddClaim(nameClaim);
                        }

                        if (givenNameClaim != null)
                        {
                            newClaimsIdentity.AddClaim(givenNameClaim);
                        }

                        if (familyNameClaim != null)
                        {
                            newClaimsIdentity.AddClaim(familyNameClaim);
                        }

                        if (roleClaim != null)
                        {
                            newClaimsIdentity.AddClaim(roleClaim);
                        }

                        newClaimsIdentity.AddClaim(new Claim("access_token", n.ProtocolMessage.AccessToken));

                        newClaimsIdentity.AddClaim(new Claim("id_token", n.ProtocolMessage.IdToken));

                        //     create a new authentication ticket, overwriting the old one.
                        n.AuthenticationTicket = new AuthenticationTicket(
                                                 newClaimsIdentity,
                                                 n.AuthenticationTicket.Properties);
                    },
                    RedirectToIdentityProvider = async n =>
                    {
                        if (n.ProtocolMessage.RequestType == Microsoft.IdentityModel.Protocols.OpenIdConnectRequestType
                        .LogoutRequest)
                        {
                            var identhinr = n.OwinContext.Authentication.User.FindFirst("id_token");
                            if (identhinr != null)
                            {
                                n.ProtocolMessage.IdTokenHint = identhinr.Value;
                            }
                        }
                    }
                }
            });
            //ConfigureAuth(app);
        }
    }
}
