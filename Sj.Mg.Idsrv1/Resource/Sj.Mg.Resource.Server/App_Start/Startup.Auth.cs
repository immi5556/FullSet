using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using System.Collections.Generic;
using IdentityServer3.AccessTokenValidation;
using System.IdentityModel.Tokens;

namespace Sj.Mg.Resource.Server
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            Sj.Mg.CliLib.Initializer.ConfigureResourceAuth(app, "FHIR-Resource1");
        }

        public static void LoadStart(IAppBuilder app)
        {
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();
            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                ClientId = "FHIR-Resource1",
                //IntrospectionHttpHandler = AppConstants.Constants.in
                Authority = CliLib.Utils.Common.Sts,
                ClientSecret = CliLib.Utils.Common.ClientSecret,
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
    }
}