using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Sj.Mg.CliLib.Security
{
    public class UmaController : Controller
    {

        public virtual string ValidateUma()
        {
            var claims = (User as ClaimsPrincipal);
            if (!claims.Identity.IsAuthenticated)
            {
                //Error can not happen
                return null;
            }
            var rpttoken = claims.FindFirst("access_token").Value;
            Newtonsoft.Json.Linq.JObject actkn = Sj.Mg.CliLib.Utils.TokenHelper.DecodeAndWrite(rpttoken);
            Newtonsoft.Json.Linq.JToken rtk = actkn.SelectToken("rptkn");
            Sj.Mg.CliLib.Model.permission perms = rtk.ToObject<Sj.Mg.CliLib.Model.permission>();
            string permtkn = CallPermTkn("https://localhost:44305/Service/RptToken");
            Newtonsoft.Json.Linq.JObject job = Newtonsoft.Json.Linq.JObject.Parse(permtkn);
            if (job.SelectToken("ticket", false) != null)
            {
                var ptkt = job.Value<string>("ticket");
                perms.ticket = ptkt;
                actkn["rptkn"] = Newtonsoft.Json.Linq.JObject.FromObject(perms);
                var http = (HttpWebRequest)WebRequest.Create("https://localhost:44305/Service/ValidatePermTkt");
                http.ContentType = "application/json";
                http.Headers.Add("Authorization", "Bearer " + Sj.Mg.CliLib.Utils.TokenHelper.CreateJwt(actkn.ToString()));
                http.Accept = "application/json";
                var resp = (HttpWebResponse)http.GetResponse();
                string result = "";
                using (var rdr = new StreamReader(resp.GetResponseStream()))
                {
                    result = rdr.ReadToEnd();
                    result = result.Replace("\"", "");
                }

                return result;
            }
            return permtkn;
        }

        string CallPermTkn(string url)
        {
            var claims = (User as ClaimsPrincipal);
            var token = claims.FindFirst("access_token").Value;
            var http = (HttpWebRequest)WebRequest.Create(url);
            http.ContentType = "application/json";
            http.Headers.Add("Authorization", "Bearer " + token);
            http.Accept = "application/json";
            var resp = (HttpWebResponse)http.GetResponse();
            string result = "";
            using (var rdr = new StreamReader(resp.GetResponseStream()))
            {
                result = rdr.ReadToEnd();
                result = result.Replace("\"", "");
            }

            return result;
        }
    }
}
