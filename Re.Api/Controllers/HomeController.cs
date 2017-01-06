using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Re.Api.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public async Task<ActionResult> stscallback()
        {
            // get the authorization code from the query string
            var authCode = Request.QueryString["code"];

            // with the auth code, we can request an access token.
            var client = new TokenClient(
                AppConstants.Constants.StsTokenEndpoint,
                "ReliefExpress-Api",
                 AppConstants.Constants.ClientSecret);

            var tokenResponse = await client.RequestAuthorizationCodeAsync(
                authCode,
                AppConstants.Constants.ReClientMvcStsCallback);

            // we save the token in a cookie for use later on
            Response.Cookies["MedGrottoCookie"]["access_token"] = tokenResponse.AccessToken;

            // get the state (uri to return to)
            var state = Request.QueryString["state"];

            // redirect to the URI saved in state            
            return Redirect(state);
        }
    }
}
