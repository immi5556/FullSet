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
    public class MedicationController : ApiController
    {
        [CliLib.Security.UmaAuthz("Patient/Medication.Read", "Patient/Medication.*")]
        [HttpGet]
        public List<Hl7.Fhir.Model.Medication> Get()
        {
            return Code.MedicManager.Get(null);
        }
        [Route("api/medication/{ids}")]
        [CliLib.Security.UmaAuthz("Patient/Medication.Read")]
        [HttpGet]
        public List<Hl7.Fhir.Model.Medication> GetMeds(string ids)
        {
            var id = System.Web.HttpUtility.UrlDecode(ids ?? "").Replace("^2E", ".");
            return Code.MedicManager.Get(id);
        }
        [CliLib.Security.UmaAuthz("Patient/Medication.Write", "Patient/Medication.*")]
        [HttpPost]
        public void Post([FromBody]Hl7.Fhir.Model.Medication act)
        {
            Console.WriteLine();
        }
    }
}
