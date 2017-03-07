using IdentityServer3.AccessTokenValidation;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Sj.Mg.CliLib
{
    public class Initializer
    {
        public static void ConfigureResourceAuth(IAppBuilder app, string client)
        {
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();
            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                //ClientId = "FHIR-Resource1",
                //IntrospectionHttpHandler = AppConstants.Constants.in
                Authority = Utils.Common.Sts,
                //ClientSecret = Utils.Common.ClientSecret,
                PreserveAccessToken = true,
                RequiredScopes = new[]
                {
                    "uma_protection",
                    "openid",
                    "profile",
                    "fhir-res1"
                }
            });
        }
        public static void ConfigureClientAuth(IAppBuilder app, string client)
        {
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies"
            });

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                ClientId = client,
                Authority = Utils.Common.Sts,
                RedirectUri = Utils.Common.ReClientMvc,
                SignInAsAuthenticationType = "Cookies",
                ResponseType = "code id_token token",
                Scope = "openid profile address roles uma_authorization",
                PostLogoutRedirectUri = Utils.Common.ReClientMvc,
                Notifications = new OpenIdConnectAuthenticationNotifications()
                {

                    SecurityTokenValidated = async n =>
                    {
                        Utils.TokenHelper.DecodeAndWrite(n.ProtocolMessage.IdToken);
                        Utils.TokenHelper.DecodeAndWrite(n.ProtocolMessage.AccessToken);

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
                                    Utils.Common.IssuerUri + subClaim.Value);

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
        }
    }
}
