using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Thinktecture.IdentityModel.WebApi;

namespace Sj.Mg.Resource.Server.Controllers
{
    public class PatientController : ApiController
    {
        [HttpGet]
        [ScopeAuthorize("Patient/Patient.Read", "Patient/Patient.*")]
        public List<Hl7.Fhir.Model.Patient> Get()
        {
            return Code.PatientManager.Get(null);
        }

        // POST: api/Account
        [ScopeAuthorize("Patient/Patient.Write, Patient/Patient.*")]
        [HttpPost]
        public void Post([FromBody]Hl7.Fhir.Model.Observation act)
        {
            Console.WriteLine();
        }
    }
}
