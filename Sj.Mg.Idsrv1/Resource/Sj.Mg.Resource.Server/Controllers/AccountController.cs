using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Thinktecture.IdentityModel.WebApi;

namespace Sj.Mg.Resource.Server.Controllers
{
    [EnableCors("*", "*", "GET, POST, PATCH")]
    public class AccountController : ApiController
    {
        [CliLib.Security.UmaAuthz("Patient/Account.*")]
        [HttpGet]
        public List<Hl7.Fhir.Model.Account> Get()
        {
            return Code.AccountManager.Get();
        }

        [Route("api/account/{ids}")]
        [CliLib.Security.UmaAuthz("Patient/Account.Read")]
        [HttpGet]
        public List<Hl7.Fhir.Model.Account> GetActs(string ids)
        {
            var id = System.Web.HttpUtility.UrlDecode(ids ?? "");
            return Code.AccountManager.Get();
        }

        //[CliLib.Security.UmaAuthz("Patient/Account.Write", "Patient/Account.*")]
        [HttpPost]
        public bool Post([FromBody]Hl7.Fhir.Model.Account act)
        {
            return Code.AccountManager.update(act);
        }
    }
}
