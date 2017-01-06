using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using IdentityServer3.AccessTokenValidation;
using System.IdentityModel.Tokens;
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
                RequiredScopes = new[]
                {
                    "patient.MedicationOrder",
                    "uma_protection",
                    "user.Observation"
                }
            });

            //var config = WebApiConfig.Register();

            //app.UseWebApi(config);
        }
    }
}
