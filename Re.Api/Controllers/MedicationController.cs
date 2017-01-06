using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Re.Api.Controllers
{
    [EnableCors("*", "*", "GET, POST, PATCH")]
    //[System.Web.Http.Authorize]
    public class MedicationController : System.Web.Http.ApiController
    {
        //[Authorize(Roles = "Hospitalist")]
        [Route("api/meds")]
        [HttpGet]
        public IHttpActionResult Get()
        {
            return Json(new { action = "value1" });
        }

        [Authorize(Roles = "Hospitalist")]
        [Route("api/postmed")]
        [HttpPost]
        public IHttpActionResult Post([FromBody]AppConstants.PostData data)
        {
            Console.WriteLine(data);
            return Json(new
            {
                Patient = data,
                Medication = new
                {
                    Detail = "Ranitidine Inj 25 mg/mL (IV)|50|mg",
                    Pharam = "Sodium Chloride 0.9% 100 mL|100|mL"
                }
            });
        }

    }
}