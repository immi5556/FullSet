using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services.Default;
using Microsoft.Owin.Security.Google;
using Owin;
using Sj.Mg.Idsrv4.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using DataBaseConnection;

namespace Sj.Mg.Idsrv4
{
    public class Startup
    {
        private readonly object OnAuthenticated;

        public void Configuration(IAppBuilder app)
        {
            app.Map("/identity", idsrv =>
            {
                idsrv.UseIdentityServer(new IdentityServerOptions
                {
                    SiteName = "Embedded IdentityServer",
                    SigningCertificate = LoadCertificate(),

                    Factory = new IdentityServerServiceFactory()
                                    .UseInMemoryUsers(Users.Get())
                                    .UseInMemoryClients(Clients.Get())
                                    .UseInMemoryScopes(Scopes.Get()),

                    AuthenticationOptions = new IdentityServer3.Core.Configuration.AuthenticationOptions
                    {
                        IdentityProviders = ConfigureIdentityProviders
                    }
                });
            });
        }

        private void ConfigureIdentityProviders(IAppBuilder app, string signInAsType)
        {
            app.UseGoogleAuthentication(new Microsoft.Owin.Security.Google.GoogleOAuth2AuthenticationOptions
            {
                AuthenticationType = "Google",
                Caption = "Sign-in with Google",
                SignInAsAuthenticationType = signInAsType,

                ClientId = "354165830384-tp8cbhf59riubedmel75t1i5uabq5h2d.apps.googleusercontent.com",
                ClientSecret = "mCWLDjLjItsd-E5bYmaTBvm6",
                Provider = new GoogleOAuth2AuthenticationProvider()
                {
                    OnAuthenticated = async ctx =>
                    {
                        DataBaseFunc db = new DataBaseFunc();                       
                        Boolean userExists = db.getData(ctx.Email);
                        if (!userExists)
                        {
                            db.insertGoogleUserData(ctx.Name, ctx.Email, ctx.FamilyName);
                        }
                        db.trackGoogleUserData(ctx.Email, ctx.AccessToken);
                        //AppConstants.Helper.TokenHelper.DecodeAndWrite(ctx.AccessToken);
                        //JWT.JsonWebToken.Decode(ctx.AccessToken, Convert.FromBase64String("mCWLDjLjItsd+E5bYmaTBvm6"));
                    }
                }
            });
        }

        X509Certificate2 LoadCertificate()
        {
            return new X509Certificate2(
                string.Format(@"{0}\certificates\idsrv3test.pfx",
                AppDomain.CurrentDomain.BaseDirectory), "idsrv3test");
        }

    }
}