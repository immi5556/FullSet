﻿using System;
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
            int ObsCount = 0;
            int MedicationCount = 0;
            var uapp = Config.Users.GetDetails().Find(t => t.UserName == Session ["CurUser"].ToString());
            if (uapp.ScopeUsers.Count > 0)
            {
                foreach(var scope in uapp.ScopeUsers)
                {
                    string key = Convert.ToString(scope.Key);
                    if(key== "user.Observation")
                    {
                        ObsCount = uapp.ScopeUsers[key].Count();
                    }
                    if(key== "patient.MedicationOrder")
                    {
                        MedicationCount = uapp.ScopeUsers[key].Count();
                    }
                }
            }
            ViewBag.ObsCount = ObsCount;
            ViewBag.MedicationCount = MedicationCount;
            return View(uapp);
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

        [HttpPost]
        public JsonResult SaveDelegation(Constants.Model.ResShare share)
        {
            Config.Users.UpdateDetails(share);
            //TO call RS server with PAT 
            return Json(share, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AllowRequest(Constants.Model.ResShare share)
        {
            Config.Users.UpdateDetails(share);
            Config.Users.RemoveRequest(share);
            //TO call RS server with PAT 
            return Json(share, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult RemoveRequest(Constants.Model.ResShare share)
        {
            Config.Users.RemoveRequest(share);
            //TO call RS server with PAT 
            return Json(share, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteDelegation(Constants.Model.ResShare share)
        {
            Config.Users.DeleteDetails(share);
            //TO call RS server with PAT 
            return Json(share, JsonRequestBehavior.AllowGet);
        }
    }
}