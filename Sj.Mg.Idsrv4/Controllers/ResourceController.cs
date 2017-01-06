using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sj.Mg.Idsrv4.Controllers
{
    public class ResourceController : Controller
    {
        // GET: Resource
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ResourceSet(AppConstants.Model.ResourceSet set)
        {
            return Json(Config.Scopes.Insert(set));
        }

        [HttpGet]
        public JsonResult ResourceGet(string id)
        {
            return Json(Config.Scopes.GetResource(id));
        }

        [HttpPost]
        public JsonResult ResourceUpdate(string id, AppConstants.Model.ResourceSet set)
        {
            return Json(Config.Scopes.UpdateResource(id, set));
        }

        [HttpGet]
        public void ResourceDelete(string id)
        {
            Config.Scopes.DeleteResource(id);
        }
    }
}