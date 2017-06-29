using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Sj.Ah.Resource.Server.Controllers
{
    public class PatientController : ApiController
    {
        [Mg.CliLib.Security.UmaAuthz("Patient/Patient.Read", "Patient/Patient.*")]
        [HttpGet]
        public Newtonsoft.Json.Linq.JArray Get(string patid)
        {
            var tt = athena.Setup.Init();
            athena.PractieInfo.SetPractice(tt);
            athena.DepartmentInfo.SetDepartment(tt);
            //PatientInfo.CreatePatient(tt);
            tt.patientid = patid;
            return athena.PatientInfo.GetPatient(tt);
        }
    }
}
