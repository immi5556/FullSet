using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Thinktecture.IdentityModel.WebApi;

namespace Sj.Mg.Resource.Server.Controllers
{
    public class MedicationController : ApiController
    {
        [HttpGet]
        [ScopeAuthorize("Patient/Medication.Read", "Patient/Medication.*")]
        public List<Hl7.Fhir.Model.Medication> Get()
        {
            return Code.MedicManager.Get(null);
        }

        // POST: api/Account
        [ScopeAuthorize("Patient/Medication.Write, Patient/Medication.*")]
        [HttpPost]
        public void Post([FromBody]Hl7.Fhir.Model.Medication act)
        {
            Console.WriteLine();
        }
    }
}
