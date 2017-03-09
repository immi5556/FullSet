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
    [Code.Security.UmaAuthz("Patient/Account")]
    [EnableCors("*", "*", "GET, POST, PATCH")]
    public class AccountController : ApiController
    {
        [HttpGet]
        public List<Hl7.Fhir.Model.Account> Get()
        {
            return Code.AccountManager.Get(null);
        }

        [HttpPost]
        public void Post([FromBody]Hl7.Fhir.Model.Account act)
        {
            Console.WriteLine();
        }
    }
}
