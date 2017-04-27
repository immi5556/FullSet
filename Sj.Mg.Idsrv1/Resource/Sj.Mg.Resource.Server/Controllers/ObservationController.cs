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
    public class ObservationController : ApiController
    {
        [CliLib.Security.UmaAuthz("Patient/Observation.Read", "Patient/Observation.*")]
        [HttpGet]
        public List<Hl7.Fhir.Model.Observation> Get()
        {
            return Code.ObsManager.Get(null);
        }
        [Route("api/observation/{ids}")]
        [CliLib.Security.UmaAuthz("Patient/Observation.Read")]
        [HttpGet]
        public List<Hl7.Fhir.Model.Observation> GetObs(string ids)
        {
            var id = System.Web.HttpUtility.UrlDecode(ids ?? "").Replace("^2E", ".");
            return Code.ObsManager.Get(id);
        }
        //[CliLib.Security.UmaAuthz("Patient/Observation.Read", "Patient/Observation.*")]
        [HttpPost]
        public bool Post(Hl7.Fhir.Model.Observation act)
        {
            return Code.ObsManager.update(act);
        }
    }
}
