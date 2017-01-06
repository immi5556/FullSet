using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Re.Api.Controllers
{
    [EnableCors("*", "*", "GET, POST, PATCH")]
    public class LabController : ApiController
    {
        [Route("api/labs")]
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            ServicePointManager.ServerCertificateValidationCallback =
    delegate (object s, X509Certificate certificate,
             X509Chain chain, SslPolicyErrors sslPolicyErrors)
    { return true; };
            if (Request.Headers.Authorization != null)
            {
                if ((Request.Headers.Authorization.Scheme ?? "").ToLower() != "bearer")
                {
                    throw new ApplicationException("Invalid token scheme.");
                }
                //Validate the RPT token with riht expiry & so on
                AppConstants.Helper.TokenHelper.DecodeAndWrite(Request.Headers.Authorization.Parameter);
            }
            else
            {
                var httpClient = Helper.HelperHttpClient.GetClient();
                var tt = await httpClient.GetAsync("/Protection/PremissionTicket").ConfigureAwait(false);
                //var serializedTrip = JsonConvert.SerializeObject(new AppConstants.Model.PermissionRequest() { resource_set_id = "53E3C716-3D65-4201-8FEF-55E271F79F23", scopes = new List<string>() { "user.Observation" } });
                //var tt = await httpClient.PostAsync("/Protection/PremissionTicket", new StringContent(serializedTrip, System.Text.Encoding.Unicode, "application/json")).ConfigureAwait(false);
                var msg = await tt.Content.ReadAsStringAsync();
                return Json(new { ticket = msg });
            }

            return Json(new { ObservationMethod = "Some data", Diagnosis = "Some details" });
        }
    }
}
