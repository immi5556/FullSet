using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Sj.Mg.Client.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public async Task<ActionResult> Index()
        {
            var token = (User as ClaimsPrincipal).FindFirst("access_token").Value;
            var client = new HttpClient();
            dynamic dyn = new System.Dynamic.ExpandoObject();
            try
            {
                client.SetBearerToken(token);
                var data = await client.GetStringAsync(@"https://localhost:44305/Service/RptToken");
                //var tkn = JsonConvert.DeserializeObject<Sj.Mg.CliLib.Model.Rpt>(data);
                //dyn.RptTkn = tkn;
                //var actkn = Sj.Mg.CliLib.Utils.TokenHelper.DecodeAndWrite(token);
                //actkn.Add("rptkn", Newtonsoft.Json.Linq.JObject.Parse(data));
                //token = Sj.Mg.CliLib.Utils.TokenHelper.CreateJwt(actkn.ToString());
                client.SetBearerToken(data);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.ToString());
            }
            try
            {
                var data = await client.GetStringAsync(@"https://localhost:44306/Api/Account");
                dyn.Acct1 = JsonConvert.DeserializeObject<List<Hl7.Fhir.Model.Account>>(data);
            }
            catch (Exception exp)
            {
                if (exp.Message.Contains("403 (Forbidden)"))
                {
                    dyn.Acct1 = "Acces Denied";
                }
                else
                {
                    dyn.Acct1 = exp.Message;
                }
                Console.WriteLine();
            }
            try
            {
                var data1 = await client.GetStringAsync(@"https://localhost:44306/Api/Medication");
                dyn.Medi1 = JsonConvert.DeserializeObject<List<Hl7.Fhir.Model.Medication>>(data1);
            }
            catch (Exception exp)
            {
                if (exp.Message.Contains("403 (Forbidden)"))
                {
                    dyn.Medi1 = "Acces Denied";
                }
                else
                {
                    dyn.Medi1 = exp.Message;
                }
                Console.WriteLine();
            }
            try
            {
                var data2 = await client.GetStringAsync(@"https://localhost:44306/Api/Patient");
                dyn.Pati1 = JsonConvert.DeserializeObject<List<dynamic>>(data2);
            }
            catch (Exception exp)
            {
                if (exp.Message.Contains("403 (Forbidden)"))
                {
                    dyn.Pati1 = "Acces Denied";
                }
                else
                {
                    dyn.Pati1 = exp.Message;
                }
                Console.WriteLine();
            }
            try
            {
                var data3 = await client.GetStringAsync(@"https://localhost:44306/Api/Observation");
                dyn.Obsr1 = JsonConvert.DeserializeObject<List<Hl7.Fhir.Model.Observation>>(data3);
            }
            catch (Exception exp)
            {
                if (exp.Message.Contains("403 (Forbidden)"))
                {
                    dyn.Obsr1 = "Acces Denied";
                }
                else
                {
                    dyn.Obsr1 = exp.Message;
                }
                Console.WriteLine();
            }
            //string json = Newtonsoft.Json.JsonConvert.SerializeObject(dyn, Newtonsoft.Json.Formatting.None, new Newtonsoft.Json.JsonSerializerSettings() { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() });
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(dyn, Formatting.None);
            json = json.Replace("'", "");
            json = json.Replace("\\\"", "");
            json = json.Replace("<div class=atg:role: property - name id=ID xmlns=http://www.w3.org/1999/xhtml />", "");
            ViewBag.Model = json;
            return View();
        }
        [Authorize]
        public ActionResult Secure()
        {
            return View();
        }
    }
}