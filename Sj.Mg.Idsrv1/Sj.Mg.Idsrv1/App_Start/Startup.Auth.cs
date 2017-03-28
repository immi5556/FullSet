using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using IdentityServer3.Core.Configuration;
using Sj.Mg.Idsrv1.Config;
using IdentityServer3.Core.Services;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Twitter;
using Serilog;
using Microsoft.Owin.Security.OpenIdConnect;
using System.Threading.Tasks;

namespace Sj.Mg.Idsrv1
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.LiterateConsole(outputTemplate: "{Timestamp:HH:MM} [{Level}] ({Name:l}){NewLine} {Message}{NewLine}{Exception}")
                .WriteTo.TraceSource(traceSourceName: "IdentityServer3")
                .CreateLogger();
            app.Map("/core", coreApp =>
            {
                var factory = new IdentityServerServiceFactory()
                    //.UseInMemoryUsers(Users.Get())
                    .UseInMemoryClients(Clients.Get())
                    .UseInMemoryScopes(Scopes.Get());

                factory.ViewService = new Registration<IViewService, Custom.MgViewService>();

                // this custom user service shows how to accept custom form params on login page
                factory.UserService = new Registration<IUserService, Custom.MgUserService>();

                var options = new IdentityServerOptions
                {
                    SiteName = "MedGrotto Host",

                    SigningCertificate = Certificate.Get(),
                    Factory = factory,
                    AuthenticationOptions = new AuthenticationOptions
                    {
                        IdentityProviders = ConfigureAdditionalIdentityProviders,
                        LoginPageLinks = new LoginPageLink[] {
                            new LoginPageLink{
                                Text = "Register",
                                Href = "localregistration"
                            }
                        },
                        EnablePostSignOutAutoRedirect = true
                    },
                    InputLengthRestrictions = new InputLengthRestrictions()
                    {
                        Scope = 1400
                    }
                };

                coreApp.UseIdentityServer(options);
            });

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                Authority = CliLib.Utils.Common.Sts,
                ClientId = "IdSrv1",
                RedirectUri = CliLib.Utils.Common.Sts + "/scopes",
                ResponseType = "id_token",

                SignInAsAuthenticationType = "Cookies"
            });
        }

        public static void ConfigureAdditionalIdentityProviders(IAppBuilder app, string signInAsType)
        {
            var google = new GoogleOAuth2AuthenticationOptions
            {
                AuthenticationType = "Google",
                Caption = "Sign-in with Google",
                SignInAsAuthenticationType = signInAsType,
                ClientId = "354165830384-tp8cbhf59riubedmel75t1i5uabq5h2d.apps.googleusercontent.com",
                ClientSecret = "mCWLDjLjItsd-E5bYmaTBvm6",
            };
            app.UseGoogleAuthentication(google);

            var fb = new FacebookAuthenticationOptions
            {
                AuthenticationType = "Facebook",
                SignInAsAuthenticationType = signInAsType,
                AppId = "1909035272666825",
                AppSecret = "1bba429b29e7cdb59e6d594e29ba717a",
                Provider = new FacebookAuthenticationProvider()
                {
                    OnAuthenticated = (context) =>
                    {
                        context.Identity.AddClaim(new System.Security.Claims.Claim("urn:facebook:access_token", context.AccessToken, System.Security.Claims.ClaimValueTypes.String, "Facebook"));
                        return Task.FromResult(0);
                    }
                }

            };
            fb.Scope.Add("email");
            app.UseFacebookAuthentication(fb);

            var twitter = new TwitterAuthenticationOptions
            {
                AuthenticationType = "Twitter",
                SignInAsAuthenticationType = signInAsType,
                ConsumerKey = "N8r8w7PIepwtZZwtH066kMlmq",
                ConsumerSecret = "df15L2x6kNI50E4PYcHS0ImBQlcGIt6huET8gQN41VFpUCwNjM"
            };
            app.UseTwitterAuthentication(twitter);
        }
    }
}