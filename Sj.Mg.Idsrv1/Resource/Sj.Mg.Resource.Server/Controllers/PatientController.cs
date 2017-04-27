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
        [Route("api/patient/{ids}")]
        [CliLib.Security.UmaAuthz("Patient/Patient.Read")]
        [HttpGet]
        public List<Hl7.Fhir.Model.Patient> GetPats(string ids)
        {
            var id = System.Web.HttpUtility.UrlDecode(ids ?? "").Replace("^2E", ".");
            return Code.PatientManager.Get(id);
        }
        //[CliLib.Security.UmaAuthz("Patient/Patient.Read", "Patient/Patient.*")]
        [HttpPost]
        public bool Post([FromBody]Hl7.Fhir.Model.Patient act)
        {
            return Code.PatientManager.Update(act);
        }
    }
}
