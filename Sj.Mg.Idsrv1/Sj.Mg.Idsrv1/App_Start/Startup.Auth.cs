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
using Owin.Security.Providers.Yahoo;
using Microsoft.Owin.Security.MicrosoftAccount;
using System.Net;
using System.IO;

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
                AppSecret = "e29cd683ecbf9df1d5adf6fe35d9cc50",
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

            //var twitter = new TwitterAuthenticationOptions
            //{
            //    AuthenticationType = "Twitter",
            //    SignInAsAuthenticationType = signInAsType,
            //    ConsumerKey = "44ZSTIW0onsNqERkFv8LGOV9t",
            //    ConsumerSecret = "wbTb6WcZz5yJbNBBtqYGILEuL4CPVadJmm6YPT1XnGl1WmpQ5g",
            //    BackchannelCertificateValidator = new Microsoft.Owin.Security.CertificateSubjectKeyIdentifierValidator(new[]
            //    {
            //        "A5EF0B11CEC04103A34A659048B21CE0572D7D47", // VeriSign Class 3 Secure Server CA - G2
            //        "0D445C165344C1827E1D20AB25F40163D8BE79A5", // VeriSign Class 3 Secure Server CA - G3
            //        "7FD365A7C2DDECBBF03009F34339FA02AF333133", // VeriSign Class 3 Public Primary Certification Authority - G5
            //        "39A55D933676616E73A761DFA16A7E59CDE66FAD", // Symantec Class 3 Secure Server CA - G4
            //        "5168FF90AF0207753CCCD9656462A212B859723B", //DigiCert SHA2 High Assurance Server C‎A 
            //        "B13EC36903F8BF4701D498261A0802EF63642BC3" //DigiCert High Assurance EV Root CA
            //    })

            //};
            //app.UseTwitterAuthentication(twitter);

            //AWS
            var twitter = new TwitterAuthenticationOptions
            {
                AuthenticationType = "Twitter",
                SignInAsAuthenticationType = signInAsType,
                ConsumerKey = "ffnDTEpds7dalrXnP70zxyB05",
                ConsumerSecret = "UJPVNO0AxFr0fu6rRtOBdl3Yk12TSQL8TWX3aEVgvipfwGAZDJ",
                BackchannelCertificateValidator = new Microsoft.Owin.Security.CertificateSubjectKeyIdentifierValidator(new[]
                {
                    "A5EF0B11CEC04103A34A659048B21CE0572D7D47", // VeriSign Class 3 Secure Server CA - G2
                    "0D445C165344C1827E1D20AB25F40163D8BE79A5", // VeriSign Class 3 Secure Server CA - G3
                    "7FD365A7C2DDECBBF03009F34339FA02AF333133", // VeriSign Class 3 Public Primary Certification Authority - G5
                    "39A55D933676616E73A761DFA16A7E59CDE66FAD", // Symantec Class 3 Secure Server CA - G4
                    "5168FF90AF0207753CCCD9656462A212B859723B", //DigiCert SHA2 High Assurance Server C‎A 
                    "B13EC36903F8BF4701D498261A0802EF63642BC3" //DigiCert High Assurance EV Root CA
                })

            };
            app.UseTwitterAuthentication(twitter);

            var microsoft = new MicrosoftAccountAuthenticationOptions
            {
                AuthenticationType = "Microsoft",
                SignInAsAuthenticationType = signInAsType,
                ClientId = "e097d6e4-7be5-4792-872f-956c0a07ea1d",
                ClientSecret = "LimpeO70gK3XJsyucMU5G9d"
            };

            app.UseMicrosoftAccountAuthentication(microsoft);

            var yahoo = new YahooAuthenticationOptions
            {
                AuthenticationType = "Yahoo",
                SignInAsAuthenticationType = signInAsType,
                ConsumerKey = "dj0yJmk9OENSeENBQk45WnU4JmQ9WVdrOWJtTmthakJETm1jbWNHbzlNQS0tJnM9Y29uc3VtZXJzZWNyZXQmeD0wNA--",
                ConsumerSecret = "b0bc2050a37d968f8549ed6ee72e9bf909f87ebd",
                Provider = new YahooAuthenticationProvider()
            };
            app.UseYahooAuthentication(yahoo);
        }
    }
    //public class TwitterImpl : Microsoft.Owin.Security.ISecureDataFormat<Microsoft.Owin.Security.Twitter.Messages.RequestToken>
    //{
    //    public TwitterImpl()
    //    {
    //        new Microsoft.Owin.Security.Twitter.Messages.RequestToken()
    //        {
    //            Token = "745933407834771457-Ak3yXrhpqdjf6tWC7hpk6zgxi307qg6",
    //            TokenSecret = "4Fobwtkz1ReLSHeffyOwYIwDkvMoay7wh29gp5Vp0KzEk"
    //        };
    //    }
    //    public string Protect(Microsoft.Owin.Security.Twitter.Messages.RequestToken data)
    //    {
    //        return data.Token;
    //    }

    //    public Microsoft.Owin.Security.Twitter.Messages.RequestToken Unprotect(string protectedText)
    //    {
    //        return new Microsoft.Owin.Security.Twitter.Messages.RequestToken()
    //        {
    //            Token = "745933407834771457-Ak3yXrhpqdjf6tWC7hpk6zgxi307qg6",
    //            TokenSecret = "4Fobwtkz1ReLSHeffyOwYIwDkvMoay7wh29gp5Vp0KzEk"
    //        };
    //    }
    //}

    public class YahooAuthenticationProvider : IYahooAuthenticationProvider
    {
        public Task Authenticated(YahooAuthenticatedContext context)
        {
            //var uid = context.UserId;
            //string url = "https://social.yahooapis.com/v1/user/" + uid + "/profile/usercard";
            //WebRequest request = WebRequest.Create(url);
            //request.Method = "GET";
            //request.Headers["Authorization"] = "Bearer " + context.AccessToken;
            //request.ContentType = "application/x-www-form-urlencoded";
            //WebResponse response = request.GetResponse();
            //Stream receive = response.GetResponseStream();
            //StreamReader reader = new StreamReader(receive, System.Text.Encoding.UTF8);
            //var tt = reader.ReadToEnd();
            //Console.WriteLine();
            return Task.FromResult(0);
        }

        public Task ReturnEndpoint(YahooReturnEndpointContext context)
        {
            Console.WriteLine();
            return Task.FromResult(0);
        }
    }
}