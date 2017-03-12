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
    public class PatientController : ApiController
    {
        [CliLib.Security.UmaAuthz("Patient/Patient.Read", "Patient/Patient.*")]
        [HttpGet]
        public List<Hl7.Fhir.Model.Patient> Get()
        {
            return Code.PatientManager.Get(null);
        }
        [CliLib.Security.UmaAuthz("Patient/Patient.Read", "Patient/Patient.*")]
        [HttpPost]
        public void Post([FromBody]Hl7.Fhir.Model.Observation act)
        {
            Console.WriteLine();
        }
    }
}
