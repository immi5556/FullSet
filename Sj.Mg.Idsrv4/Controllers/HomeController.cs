using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sj.Mg.Idsrv4.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Access(string un, string secret)
        {
            if (Config.Users.Get().Find(t => t.Username == un && t.Password == secret) != null)
                return Json(new { Message = "Sucessfull." });
            Response.StatusCode = 401;
            return Json("Access Denied");
        }
    }
}