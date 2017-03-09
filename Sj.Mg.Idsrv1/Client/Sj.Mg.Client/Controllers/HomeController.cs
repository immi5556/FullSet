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
            client.SetBearerToken(token);
            var data = await client.GetStringAsync(@"https://localhost:44306/Api/Account");
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.None, new Newtonsoft.Json.JsonSerializerSettings() { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() });
            ViewBag.Acct = Microsoft.Security.Application.Encoder.HtmlEncode(json);
            ViewBag.Acct1 = json;
            return View();
        }
        [Authorize]
        public ActionResult Secure()
        {
            return View();
        }
    }
}