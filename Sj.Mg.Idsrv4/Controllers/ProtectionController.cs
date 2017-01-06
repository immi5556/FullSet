using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sj.Mg.Idsrv4.Controllers
{
    public class ProtectionController : Controller
    {
        // GET: Protection
        public ActionResult Index()
        {
            return View();
        }

        //[Authorize]
        //[HttpPost]
        //public JsonResult PremissionTicket(AppConstants.Model.PermissionRequest req)
        public JsonResult PremissionTicket()
        {
            return Json(Guid.NewGuid().ToString(), JsonRequestBehavior.AllowGet);
        }
        //[Authorize]
        public JsonResult RptToken(string id)
        {
            return Json(new
            {
                active = true,
                exp = 1256953732,
                iat = 1256912345,
                permissions = new
                {
                    resource_set_id = "53E3C716-3D65-4201-8FEF-55E271F79F23",
                    scopes = new List<String>()
                    {
                        "user.Observation"
                    },
                    exp = 1256953732
                },
            }, JsonRequestBehavior.AllowGet);
        }
    }
}