﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Thinktecture.IdentityModel.WebApi;

namespace Sj.Mg.Resource.Server.Controllers
{
    [ScopeAuthorize("Patient/Observation")]
    [EnableCors("*", "*", "GET, POST, PATCH")]
    public class ObservationController : ApiController
    {
        [HttpGet]
        public List<Hl7.Fhir.Model.Observation> Get()
        {
            return Code.ObsManager.Get(null);
        }
        [HttpPost]
        public void Post([FromBody]Hl7.Fhir.Model.Observation act)
        {
            Console.WriteLine();
        }
    }
}
