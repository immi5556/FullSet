using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AppConstants
{
    public class HelperHttpClient
    {
        public static HttpClient GetClient(string resourceclient)
        {
            ServicePointManager.ServerCertificateValidationCallback =
            delegate (object s, X509Certificate certificate,
                     X509Chain chain, SslPolicyErrors sslPolicyErrors)
            { return true; };

            HttpClient client = new HttpClient();

            var accessToken = RequestAccessTokenClientCredentials(resourceclient);
            client.SetBearerToken(accessToken);

            client.BaseAddress = new Uri(AppConstants.Constants.StsOrigin);

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        private static string RequestAccessTokenClientCredentials(string client)
        {
            ServicePointManager.ServerCertificateValidationCallback =
                delegate (object s, X509Certificate certificate,
                         X509Chain chain, SslPolicyErrors sslPolicyErrors)
                { return true; };

            var cookie = HttpContext.Current.Request.Cookies.Get("MedGrottoCookie");
            if (cookie != null && cookie["access_token"] != null)
            {
                return cookie["access_token"];
            }

            var oAuth2Client = new TokenClient(
                      AppConstants.Constants.StsTokenEndpoint,
                      client,
                      AppConstants.Constants.ClientSecret);

            var tokenResponse = oAuth2Client.RequestClientCredentialsAsync("uma_protection").Result;

            // decode & write out the token, so we can see what's in it
            AppConstants.TokenHelper.DecodeAndWrite(tokenResponse.AccessToken);

            // we save the token in a cookie for use later on
            HttpContext.Current.Response.Cookies["MedGrottoCookie"]["access_token"] = tokenResponse.AccessToken;

            // return the token
            return tokenResponse.AccessToken;
        }
    }
}
