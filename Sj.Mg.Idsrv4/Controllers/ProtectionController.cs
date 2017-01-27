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
        [HttpGet]
        //public JsonResult PremissionTicket(AppConstants.Model.PermissionRequest req)
        public JsonResult PremissionTicket()
        {
            var tte = this.Request.Headers["Authorization"].Replace("Bearer ", "");
            var tkt = Config.Permissions.AddTicket(tte);
            return Json(tkt, JsonRequestBehavior.AllowGet);
        }
        //[Authorize]
        public JsonResult RptToken(string id)
        {
            if (string.IsNullOrEmpty(id) || !Config.Permissions.ContainsTicket(id))
            {
                return Json(new
                {
                    active = true,
                    exp = 1256953732,
                    iat = 1256912345,
                    permissions = new
                    {
                        scopes = new List<String>()
                        {
                            "uma_protection"
                        }
                    },
                }, JsonRequestBehavior.AllowGet);
            }
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
                        "patient.MedicationOrder",
                        "uma_protection"
                    },
                    amiallowed = Config.Permissions.GetAllowedUsers(id),
                    exp = 1256953732
                },
            }, JsonRequestBehavior.AllowGet);
        }
    }
}