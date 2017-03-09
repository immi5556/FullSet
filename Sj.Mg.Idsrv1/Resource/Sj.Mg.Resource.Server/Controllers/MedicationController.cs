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
    [ScopeAuthorize("Patient/Medication")]
    [EnableCors("*", "*", "GET, POST, PATCH")]
    public class MedicationController : ApiController
    {
        [HttpGet]
        public List<Hl7.Fhir.Model.Medication> Get()
        {
            return Code.MedicManager.Get(null);
        }
        [HttpPost]
        public void Post([FromBody]Hl7.Fhir.Model.Medication act)
        {
            Console.WriteLine();
        }
    }
}
