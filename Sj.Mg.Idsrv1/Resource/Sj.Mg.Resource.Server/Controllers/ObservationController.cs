using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Thinktecture.IdentityModel.WebApi;

namespace Sj.Mg.Resource.Server.Controllers
{
    public class ObservationController : ApiController
    {
        [HttpGet]
        [ScopeAuthorize("Patient/Observation.Read", "Patient/Observation.*")]
        public List<Hl7.Fhir.Model.Observation> Get()
        {
            return Code.ObsManager.Get(null);
        }

        // POST: api/Account
        [ScopeAuthorize("Patient/Observation.Write, Patient/Observation.*")]
        [HttpPost]
        public void Post([FromBody]Hl7.Fhir.Model.Observation act)
        {
            Console.WriteLine();
        }
    }
}
