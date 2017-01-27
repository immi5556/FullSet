using IdentityServer3.AccessTokenValidation;
using Microsoft.Owin;
using Owin;
using System.Collections.Generic;
using System.IdentityModel.Tokens;

[assembly: OwinStartupAttribute(typeof(Medinova.Emr.Fhir.Startup))]
namespace Medinova.Emr.Fhir
{
    public partial class Startup
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
            ConfigureAuth(app);
        }
    }
}
