using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using IdentityServer3.AccessTokenValidation;
using System.IdentityModel.Tokens;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using System.Security.Claims;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security.Jwt;
//using System.IdentityModel.Tokens.Jwt;

[assembly: OwinStartup(typeof(Re.Api.Startup))]

namespace Re.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();
            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap = new Dictionary<string, string>();

            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                ClientId = "ReliefExpress-Api",
                //IntrospectionHttpHandler = AppConstants.Constants.in
                Authority = AppConstants.Constants.Sts,
                ClientSecret = AppConstants.Constants.ClientSecret,
                PreserveAccessToken = true,
                RequiredScopes = new[]
                {
                    "patient.MedicationOrder",
                    "uma_protection",
                    "user.Observation",
                    "re-api"
                }
            });



            //setup(app);

            //var config = WebApiConfig.Register();

            //app.UseWebApi(config);
        }

        static void setup(IAppBuilder app)
        {
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
                        TokenHelper.DecodeAndWrite(n.ProtocolMessage.IdToken);
                        TokenHelper.DecodeAndWrite(n.ProtocolMessage.AccessToken);

                        var givenNameClaim = n.AuthenticationTicket
                            .Identity.FindFirst(IdentityModel.JwtClaimTypes.GivenName);

                        var familyNameClaim = n.AuthenticationTicket
                            .Identity.FindFirst(IdentityModel.JwtClaimTypes.FamilyName);

                        var subClaim = n.AuthenticationTicket
                            .Identity.FindFirst(IdentityModel.JwtClaimTypes.Subject);

                        var roleClaim = n.AuthenticationTicket
                            .Identity.FindFirst(IdentityModel.JwtClaimTypes.Role);

                        // create a new claims, issuer + sub as unique identifier
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

                        // create a new authentication ticket, overwriting the old one.
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
