using IdentityModel.Client;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace Relief.Express.Mvc.Controllers
{
    [EnableCors("", "*", "GET, POST, PATCH")]
    //[Authorize]
    public class HomeController : Controller
    {
        //[Authorize]
        public async Task<ActionResult> Index()
        {
            ServicePointManager.ServerCertificateValidationCallback =
    delegate (object s, X509Certificate certificate,
             X509Chain chain, SslPolicyErrors sslPolicyErrors)
    { return true; };
            var idt = this.User.Identity as ClaimsIdentity;
            foreach (var t in idt.Claims)
            {
                System.Diagnostics.Debug.WriteLine(t.Type + "---" + t.Value);
            }
            //var token = idt.FindFirst("access_token").Value;
            //Call userinfo endpoint
            //UserInfoClient userInfoClient = new UserInfoClient(AppConstants.Constants.StsUserInfoEndpoint);
            //var userInfoResp = await userInfoClient.GetAsync(token);
            //ViewBag.Address = userInfoResp.Claims.First(c => c.Type == "address").Value;
            ViewBag.Address = "";

            //var vm = new AppConstants.PostData() { Name = "Lord Jesus my saviour", Age = 11 };
            //var serializedTrip = JsonConvert.SerializeObject(vm);

            //var http = Helper.HelperHttpClient.GetClient();
            //var tt = await http.PostAsync("api/postmed", new StringContent(serializedTrip, System.Text.Encoding.Unicode, "application/json")).ConfigureAwait(false);

            //if (tt.IsSuccessStatusCode)
            //{
            //    ViewBag.Messg = await tt.Content.ReadAsStringAsync();
            //    return View();
            //}
            //else
            //{
            //    //return View("Error");
            //    ViewBag.Messg = await tt.Content.ReadAsStringAsync();
            //    return View();
            //}

            //Below code - for auth code flow
            //var http = Helper.HelperHttpClient.GetClient();
            //var tt = await http.GetAsync("api/meds").ConfigureAwait(false);
            //if (!tt.IsSuccessStatusCode)
            //    Debug.Write("Error");
            //else
            //    Debug.Write(tt.Content);
            return View();
        }
        [Authorize]
        public async Task<ActionResult> Secure()
        {
            return View();
        }
        [Authorize]
        public async Task<ActionResult> GetData()
        {
            ServicePointManager.ServerCertificateValidationCallback =
    delegate (object s, X509Certificate certificate,
             X509Chain chain, SslPolicyErrors sslPolicyErrors)
    { return true; };
            var vm = new AppConstants.PostData() { Name = "Patient Name", Age = 11 };
            var serializedTrip = JsonConvert.SerializeObject(vm);
            var http = Helper.HelperHttpClient.GetClient();
            //var tt = await http.GetAsync("api/meds").ConfigureAwait(false);
            var tt = await http.PostAsync("api/postmed", new StringContent(serializedTrip, System.Text.Encoding.Unicode, "application/json")).ConfigureAwait(false);
            var msg = await tt.Content.ReadAsStringAsync();
            var tyy = "<pre>" + JsonConvert.SerializeObject(JsonConvert.DeserializeObject(msg), Formatting.Indented) +"</pre>";
            return Json(tyy, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public async Task<ActionResult> GetLabData()
        {
            ServicePointManager.ServerCertificateValidationCallback =
    delegate (object s, X509Certificate certificate,
             X509Chain chain, SslPolicyErrors sslPolicyErrors)
    { return true; };
            var msgg = "";
            var vm = new AppConstants.PostData() { Name = "Patient Name", Age = 11 };
            var serializedTrip = JsonConvert.SerializeObject(vm);
            //var http = Helper.HelperHttpClient.GetClient();
            //http.DefaultRequestHeaders.Add("Authorization", "Bearer " + );
            HttpClient http = new HttpClient();
            http.BaseAddress = new Uri(AppConstants.Constants.ReApi);
            http.DefaultRequestHeaders.Accept.Clear();
            http.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            var tt = await http.GetAsync("api/labs").ConfigureAwait(false);
            //var tt = await http.PostAsync("api/postmed", new StringContent(serializedTrip, System.Text.Encoding.Unicode, "application/json")).ConfigureAwait(false);
            var msg = await tt.Content.ReadAsStringAsync();
            msgg = "<div><li>Permission Ticket : <pre>" + JsonConvert.SerializeObject(JsonConvert.DeserializeObject(msg), Formatting.Indented) + "</pre></li>";
            http = new HttpClient();
            http.BaseAddress = new Uri(AppConstants.Constants.StsOrigin);
            http.DefaultRequestHeaders.Accept.Clear();
            http.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            var ty = JsonConvert.DeserializeObject<AppConstants.Model.Permission>(msg);
            var tt1 = await http.GetAsync("Protection/RptToken/" + ty.ticket).ConfigureAwait(false);
            var msg1 = await tt1.Content.ReadAsStringAsync();
            msgg = msgg + "<li>" + "Rpt Token : <pre>" + JsonConvert.SerializeObject(JsonConvert.DeserializeObject(msg1), Formatting.Indented) + "</pre></li>";

            http = new HttpClient();
            http.BaseAddress = new Uri(AppConstants.Constants.ReApi);
            http.DefaultRequestHeaders.Accept.Clear();
            http.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            //http.SetBearerToken(Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(msg1))));          

            http.SetBearerToken(Helper.TokenHelper.CreateJwt(msg1));
            var tt2 = await http.GetAsync("api/labs").ConfigureAwait(false);
            var msg2 = await tt2.Content.ReadAsStringAsync();
            msgg = msgg + "<li>" + "Observation Data : <pre>" + JsonConvert.SerializeObject(JsonConvert.DeserializeObject(msg2), Formatting.Indented) + "</pre></li></div>";

            return Json(msgg, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> stscallback()
        {
            ServicePointManager.ServerCertificateValidationCallback =
    delegate (object s, X509Certificate certificate,
             X509Chain chain, SslPolicyErrors sslPolicyErrors)
    { return true; };
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
            Response.Cookies["ReCookie"]["access_token"] = tokenResponse.AccessToken;

            // get the state (uri to return to)
            var state = Request.QueryString["state"];

            // redirect to the URI saved in state            
            return Redirect(state);
        }
        public ActionResult Logout()
        {
            Request.GetOwinContext().Authentication.SignOut();
            return Redirect("/");
        }
    }
}