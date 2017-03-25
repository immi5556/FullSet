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

        [Authorize]
        [CliLib.Security.UmaAuthz("Patient/Account.Read")]
        [HttpGet]
        public List<Hl7.Fhir.Model.Account> Get(string ids)
        {
            return Code.AccountManager.Get(ids);
        }

        [Authorize]
        [CliLib.Security.UmaAuthz("Patient/Account.Write", "Patient/Account.*")]
        [HttpPost]
        public void Post([FromBody]Hl7.Fhir.Model.Account act)
        {
            Console.WriteLine();
        }
    }
}
