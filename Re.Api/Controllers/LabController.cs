using IdentityModel.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Re.Api.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Re.Api.Controllers
{
    [EnableCors("*", "*", "GET, POST, PATCH")]
    public class LabController : ApiController
    {
        [AuthzExt(new string[] { "patient.MedicationOrder" })]
        [Route("api/medic")]
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            ServicePointManager.ServerCertificateValidationCallback =
            delegate (object s, X509Certificate certificate,
             X509Chain chain, SslPolicyErrors sslPolicyErrors)
            { return true; };
            try
            {
                var tt = Request.Headers.Authorization.Parameter;
                var rpt = AppConstants.Helper.TokenHelper.DecodeAndWrite(tt);
                var token = (string)rpt.SelectToken("access_token");
                var idt = this.User.Identity as ClaimsIdentity;
                UserInfoClient userInfoClient = new UserInfoClient(new Uri(AppConstants.Constants.StsUserInfoEndpoint), token);
                var userInfoResp = await userInfoClient.GetAsync();
                //var principal = User as ClaimsPrincipal;
            }
            catch { }
            //var idt = from c in principal.Identities.First().Claims
            //                 select new
            //                 {
            //                     c.Type,
            //                     c.Value
            //                 };
            //var idt = this.User.Identity as ClaimsIdentity;
            //foreach (var t in idt)
            //{
            //    System.Diagnostics.Debug.WriteLine(t.Type + "---" + t.Value);
            //}
            return Json(new List<object>()
            {
                new { Code = "5182-1", Diagnosis = "Hepatitis A Virus IgM Serum Antibody EIA" },
                new { Code = "7059-9", Diagnosis = "Vancomycin Susceptibility, Gradient Strip" },
                new { Code = "CULT", Diagnosis = "**Corrected Micro Report** Rhodotorula glutinis**" }
                });
        }

        [AuthzExt(new string[] { "user.Observation" })]
        [Route("api/obs")]
        [HttpPost]
        public IHttpActionResult Post([FromBody]AppConstants.PostData data)
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
            Console.WriteLine(data);
            return Json(new
            {
                Patient = data,
                Medication = new
                {
                    Detail = "Ranitidine Inj 25 mg/mL (IV)|50|mg",
                    Pharam = "Sodium Chloride 0.9% 100 mL|100|mL"
                }
            });
        }
    }
}
