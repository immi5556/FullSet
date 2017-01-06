using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace Re.Api.Helper
{
    public class HelperHttpClient
    {
        public static HttpClient GetClient()
        {
            ServicePointManager.ServerCertificateValidationCallback =
    delegate (object s, X509Certificate certificate,
             X509Chain chain, SslPolicyErrors sslPolicyErrors)
    { return true; };
            HttpClient client = new HttpClient();

            //var accessToken = RequestAccessTokenAuthorizationCode();
            //if (accessToken != null)
            //{
            //    client.SetBearerToken(accessToken);
            //}

            //client credentials flow
           var accessToken = RequestAccessTokenClientCredentials();
            client.SetBearerToken(accessToken);

            client.BaseAddress = new Uri(AppConstants.Constants.StsOrigin);

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        private static string RequestAccessTokenAuthorizationCode()
        {
            ServicePointManager.ServerCertificateValidationCallback =
    delegate (object s, X509Certificate certificate,
             X509Chain chain, SslPolicyErrors sslPolicyErrors)
    { return true; };
            // did we store the token before?
            var cookie = HttpContext.Current.Request.Cookies.Get("MedGrottoCookie");
            if (cookie != null && cookie["access_token"] != null)
            {
                return cookie["access_token"];
            }

            // no token found - request one

            // we'll pass through the URI we want to return to as state
            var state = HttpContext.Current.Request.Url.OriginalString;

            var authorizeRequest = new IdentityModel.Client.AuthorizeRequest(
                AppConstants.Constants.StsAuthorizationEndpoint);

            var url = authorizeRequest.CreateAuthorizeUrl("ReliefExpress-Api", "code", "uma_protection",
                AppConstants.Constants.ReApiStsCallback, state);

            HttpContext.Current.Response.Redirect(url);

            return null;
        }

        private static string RequestAccessTokenClientCredentials()
        {
            ServicePointManager.ServerCertificateValidationCallback =
    delegate (object s, X509Certificate certificate,
             X509Chain chain, SslPolicyErrors sslPolicyErrors)
    { return true; };
            // did we store the token before?
            var cookie = HttpContext.Current.Request.Cookies.Get("MedGrottoCookie");
            if (cookie != null && cookie["access_token"] != null)
            {
                return cookie["access_token"];
            }

            // no token found - get one

            // create an oAuth2 Client
            var oAuth2Client = new TokenClient(
                      AppConstants.Constants.StsTokenEndpoint,
                      "ReliefExpress-Api",
                      AppConstants.Constants.ClientSecret);

            // ask for a token, containing the gallerymanagement scope
            var tokenResponse = oAuth2Client.RequestClientCredentialsAsync("uma_protection").Result;

            // decode & write out the token, so we can see what's in it
            AppConstants.Helper.TokenHelper.DecodeAndWrite(tokenResponse.AccessToken);

            // we save the token in a cookie for use later on
            HttpContext.Current.Response.Cookies["MedGrottoCookie"]["access_token"] = tokenResponse.AccessToken;

            // return the token
            return tokenResponse.AccessToken;
        }
    }
}