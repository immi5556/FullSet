using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Re.Api.App_Start
{
    public class AuthzExt : AuthorizeAttribute
    {
        public string[] AllowedScopes { get; set; }
        public AuthzExt(string[] scopes)
        {
            AllowedScopes = scopes;
        }
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization != null)
            {
                if ((actionContext.Request.Headers.Authorization.Scheme ?? "").ToLower() != "bearer")
                {
                    throw new ApplicationException("Invalid token scheme.");
                }
                //Validate the RPT token with riht expiry & so on
                AppConstants.Helper.TokenHelper.DecodeAndWrite(actionContext.Request.Headers.Authorization.Parameter);
                //actionContext.Request.GetOwinContext().Authentication.SignIn();
            }
            base.OnAuthorization(actionContext);
        }

        public override Task OnAuthorizationAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            return base.OnAuthorizationAsync(actionContext, cancellationToken);
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            //base.HandleUnauthorizedRequest(actionContext);
            ServicePointManager.ServerCertificateValidationCallback =
    delegate (object s, X509Certificate certificate,
             X509Chain chain, SslPolicyErrors sslPolicyErrors)
    { return true; };
            if (!b1)
            {
                var httpClient = Helper.HelperHttpClient.GetClient();
                var tte = actionContext.Request.Headers.Authorization.Parameter;
                JObject jo = AppConstants.Helper.TokenHelper.DecodeAndWrite(tte);
                jo.Add("allowed_scope", AllowedScopes[0]);
                httpClient.SetBearerToken(AppConstants.Helper.TokenHelper.CreateJwt(jo.ToString()));
                var tt = httpClient.GetAsync("/Protection/PremissionTicket").Result;
                //var serializedTrip = JsonConvert.SerializeObject(new AppConstants.Model.PermissionRequest() { resource_set_id = "53E3C716-3D65-4201-8FEF-55E271F79F23", scopes = new List<string>() { "user.Observation" } });
                //var tt = await httpClient.PostAsync("/Protection/PremissionTicket", new StringContent(serializedTrip, System.Text.Encoding.Unicode, "application/json")).ConfigureAwait(false);
                var msg = tt.Content.ReadAsStringAsync().Result;
                HttpResponseMessage responseMessage = new HttpResponseMessage()
                {
                    Content = new StringContent("{ 'ticket': '" + msg + "', 'expires' : " + DateTime.UtcNow
               .Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))
               .TotalMilliseconds + "}")
                };
                responseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                responseMessage.Content.Headers.Add("Access-Control-Allow-Origin", "*");

                actionContext.Response = responseMessage;
            }
            if (b1 && !b2)
                base.HandleUnauthorizedRequest(actionContext);
        }
        bool b1 = false;
        bool b2 = false;
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            //var bbl = base.IsAuthorized(actionContext);
            //return bbl;
            JObject rpt = new JObject();
            if (actionContext.Request.Headers.Authorization != null)
            {
                try
                {
                    b1 = b2 = false;
                    rpt = AppConstants.Helper.TokenHelper.DecodeAndWrite(actionContext.Request.Headers.Authorization.Parameter);
                    b1 = (bool)rpt.SelectToken("active");
                    b2 = (bool)rpt.SelectToken("permissions.amiallowed");
                }
                catch { }
            }
            return b1 && b2;
        }
    }
}