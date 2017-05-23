using IdentityServer3.AccessTokenValidation;
using Microsoft.Owin.Builder;
using Owin;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Web;

namespace Sj.Ah.Resource.Server
{
    public class Startup
    {
        public void Configuration(AppBuilder app)
        {
                JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();
                app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
                {
                    ClientId = "Athena-RS",
                    //IntrospectionHttpHandler = AppConstants.Constants.in
                    Authority = Mg.CliLib.Utils.Common.Sts,
                    ClientSecret = Mg.CliLib.Utils.Common.ClientSecret,
                    PreserveAccessToken = true,
                    RequiredScopes = new[]
                    {
                    "uma_protection",
                    "openid",
                    "profile",
                    "athena-rs"
                }
                });
        }
    }
}