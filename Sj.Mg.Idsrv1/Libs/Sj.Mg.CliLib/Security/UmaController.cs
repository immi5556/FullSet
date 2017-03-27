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
        public string GetEmptyRptToken()
        {
            var token = (User as ClaimsPrincipal).FindFirst("access_token").Value;
            string basetkn = "";
            var client = new HttpClient();
            try
            {
                client.SetBearerToken(token);
                var actkn = Sj.Mg.CliLib.Utils.TokenHelper.DecodeAndWrite(token);
                var data = client.GetStringAsync(@"https://localhost:44305/Service/RptToken").Result;
                basetkn = data.Replace("\"", "");
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.ToString());
            }
            return basetkn;
        }
    }
}
