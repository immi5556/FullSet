using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services.Default;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.Facebook;
using Owin;
using Sj.Mg.Idsrv4.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using DataBaseConnection;
using System.Net.Http;
using MongoDB.Bson.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Constants.Model;
using Newtonsoft.Json.Linq;

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
            app.UseFacebookAuthentication(new FacebookAuthenticationOptions()
            {
                AppId = "1909035272666825",
                AppSecret = "1bba429b29e7cdb59e6d594e29ba717a",
                
                Provider = new FacebookAuthenticationProvider()
                {
                    OnAuthenticated = async ctx =>
                    {
                        getFacebookUserDetails(ctx);
                    }
                },
                SignInAsAuthenticationType = signInAsType,
                Caption = "Sign-in with Facebook",
                AuthenticationType = "Facebook"
            });
       
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
                            db.insertGoogleUserData(ctx.Name, ctx.Email, ctx.FamilyName, "Google");
                        }
                        db.trackGoogleUserData(ctx.Email, ctx.AccessToken);
                    }
                }
            });          
        }

        public void getFacebookUserDetails(FacebookAuthenticatedContext ctx)
        {
            using (var client = new HttpClient())
            {
                var result = client.GetAsync("https://graph.facebook.com/me?fields=email,first_name,last_name&access_token=" + ctx.AccessToken).Result;

                if (result.IsSuccessStatusCode)
                {
                    var userInformation = result.Content.ReadAsStringAsync().Result;
                    var fbUser = Newtonsoft.Json.JsonConvert.DeserializeObject<FBData>(userInformation);
                    ctx.Identity.AddClaim(new System.Security.Claims.Claim(IdentityServer3.Core.Constants.ClaimTypes.GivenName, fbUser.last_name));
                    ctx.Identity.AddClaim(new System.Security.Claims.Claim(IdentityServer3.Core.Constants.ClaimTypes.FamilyName, fbUser.first_name));
                    ctx.Identity.AddClaim(new System.Security.Claims.Claim(IdentityServer3.Core.Constants.ClaimTypes.Email, fbUser.email));
                    DataBaseFunc db = new DataBaseFunc();
                    Boolean userExists = db.getData(fbUser.email);
                    if (!userExists)
                    {
                        db.insertGoogleUserData(fbUser.last_name, fbUser.email, fbUser.first_name, "Facebook");
                    }
                    db.trackGoogleUserData(fbUser.email, ctx.AccessToken);
                }

            }
        }

        X509Certificate2 LoadCertificate()
        {
            return new X509Certificate2(
                string.Format(@"{0}\certificates\idsrv3test.pfx",
                AppDomain.CurrentDomain.BaseDirectory), "idsrv3test");
        }

    }

    public class FBData
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
    }
}