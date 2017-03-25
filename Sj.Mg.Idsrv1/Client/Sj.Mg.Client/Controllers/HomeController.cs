using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Sj.Mg.Client.Controllers
{
    public class HomeController : CliLib.Security.UmaController
    {
        [Authorize]
        public async Task<ActionResult> Index()
        {
            var token = (User as ClaimsPrincipal).FindFirst("access_token").Value;
            string basetkn = "";
            var client = new HttpClient();
            dynamic dyn = new System.Dynamic.ExpandoObject();
            try
            {
                client.SetBearerToken(token);
                var actkn = Sj.Mg.CliLib.Utils.TokenHelper.DecodeAndWrite(token);
                var data = await client.GetStringAsync(@"https://localhost:44305/Service/RptToken");
                basetkn = data.Replace("\"", "");
                client.SetBearerToken(basetkn);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.ToString());
            }
            try
            {
                //var data = await client.GetStringAsync(@"https://localhost:44306/Api/Account");
                //dyn.Acct1 = JsonConvert.DeserializeObject<List<Hl7.Fhir.Model.Account>>(data);
                dyn.Acct1 = Execute<List<Hl7.Fhir.Model.Account>>(@"https://localhost:44306/Api/Account", basetkn);
            }
            catch (Exception exp)
            {
                if (exp.Message.Contains("403 (Forbidden)"))
                {
                    dyn.Acct1 = "Acces Denied";
                }
                else
                {
                    dyn.Acct1 = "Unknown Error";
                }
                Console.WriteLine();
            }
            try
            {
                //var data1 = await client.GetStringAsync(@"https://localhost:44306/Api/Medication");
                //dyn.Medi1 = JsonConvert.DeserializeObject<List<Hl7.Fhir.Model.Medication>>(data1);
                dyn.Medi1 = Execute<List<Hl7.Fhir.Model.Medication>>(@"https://localhost:44306/Api/Medication", basetkn);
            }
            catch (Exception exp)
            {
                if (exp.Message.Contains("403 (Forbidden)"))
                {
                    dyn.Medi1 = "Acces Denied";
                }
                else
                {
                    dyn.Medi1 = "Unknown Error";
                }
                Console.WriteLine();
            }
            try
            {
                //var data2 = await client.GetStringAsync(@"https://localhost:44306/Api/Patient");
                //dyn.Pati1 = JsonConvert.DeserializeObject<List<dynamic>>(data2);
                dyn.Pati1 = Execute<List<dynamic>>(@"https://localhost:44306/Api/Patient", basetkn);
            }
            catch (Exception exp)
            {
                if (exp.Message.Contains("403 (Forbidden)"))
                {
                    dyn.Pati1 = "Acces Denied";
                }
                else
                {
                    dyn.Pati1 = "Unknown Error";
                }
                Console.WriteLine();
            }
            try
            {
                //var data3 = await client.GetStringAsync(@"https://localhost:44306/Api/Observation");
                //dyn.Obsr1 = JsonConvert.DeserializeObject<List<Hl7.Fhir.Model.Observation>>(data3);
                dyn.Obsr1 = Execute<List<Hl7.Fhir.Model.Observation>>(@"https://localhost:44306/Api/Observation", basetkn);
            }
            catch (Exception exp)
            {
                if (exp.Message.Contains("403 (Forbidden)"))
                {
                    dyn.Obsr1 = "Acces Denied";
                }
                else
                {
                    dyn.Obsr1 = "Unknown Error";
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
        T ExecuteProc<T>(string url, string basetkn)
        {
            string result = "";
            using (var client = new HttpClient())
            {
                client.SetBearerToken(basetkn);
                var response = client.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;
                    result = responseContent.ReadAsStringAsync().Result;
                    try
                    {
                        Newtonsoft.Json.Linq.JObject job = Newtonsoft.Json.Linq.JObject.Parse(result);
                        if (job.SelectToken("ticket", false) != null)
                        {
                            throw new UnauthorizedAccessException("Unauth exception..");
                        }
                    }
                    catch { }
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedAccessException("Unauth exception 2..");
                }
            }
            return JsonConvert.DeserializeObject<T>(result);
        }

        T Execute<T>(string url, string basetkn)
        {
            string result = "";
            using (var client = new HttpClient())
            {
                client.SetBearerToken(basetkn);
                var response = client.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;
                    result = responseContent.ReadAsStringAsync().Result;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {

                }
            }
            string fintkn = ValidPermTkt(result, basetkn);
            if (fintkn != null)
                return ExecuteProc<T>(url, fintkn.Replace("\"", ""));
            //Execute<T>(url, fintkn.Replace("\"",""));
            //Execute<T>(url, Sj.Mg.CliLib.Utils.TokenHelper.CreateJwt(fintkn));
            return  JsonConvert.DeserializeObject<T>(result);
        }

        string ValidPermTkt(string data, string basetkn)
        {
            JsonConvert.DeserializeObject<Sj.Mg.CliLib.Model.permticket>(data);
            Newtonsoft.Json.Linq.JObject job = Newtonsoft.Json.Linq.JObject.Parse(data);
            if (job.SelectToken("ticket", false) != null)
            {
                var ptkt = job.Value<string>("ticket");
                Newtonsoft.Json.Linq.JObject actkn = Sj.Mg.CliLib.Utils.TokenHelper.DecodeAndWrite(basetkn);
                Newtonsoft.Json.Linq.JToken rtk = actkn.SelectToken("rptkn");
                Sj.Mg.CliLib.Model.permission perms = rtk.ToObject<Sj.Mg.CliLib.Model.permission>();
                perms.ticket = ptkt;
                actkn["rptkn"] = JObject.FromObject(perms);
                var http = (HttpWebRequest)WebRequest.Create("https://localhost:44305/Service/ValidatePermTkt");
                http.ContentType = "application/json";
                http.Headers.Add("Authorization", "Bearer " + Sj.Mg.CliLib.Utils.TokenHelper.CreateJwt(actkn.ToString()));
                http.Accept = "application/json";
                var resp = (HttpWebResponse)http.GetResponse();
                string result = "";
                using (var rdr = new StreamReader(resp.GetResponseStream()))
                {
                    result = rdr.ReadToEnd();
                }
                return result;
            }
            return null;
        }
    }
}