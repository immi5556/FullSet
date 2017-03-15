using Sj.Mg.Idsrv1.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sj.Mg.Idsrv1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult AddScope(string name,string displayname,string description,string type, bool emphasize, bool claimsName, bool claimsFamilyName, bool claimsGivenName, bool claimsEmail)
        {
            Scopes ab = new Scopes();
            string result = ab.addNewScope(name, displayname, description, type, emphasize, claimsName, claimsGivenName, claimsFamilyName, claimsEmail);
            return Json(result);
        }

        [HttpPost]
        public ActionResult AddClient(string clientId, string clientName, string flow, Array allowedScopes, string redirectUris, string postLogoutRedirectUris, bool includeJwtId, bool allowRememberConsent, bool allowAccessToAllScopes)
        {
            Clients ab = new Clients();
            string result = ab.addNewClient( clientId, clientName, flow, allowedScopes, redirectUris, postLogoutRedirectUris, includeJwtId, allowRememberConsent, allowAccessToAllScopes);
            return Json(result);
        }

        [HttpPost]
        public ActionResult UpdateScopes(string clientId, Boolean allowAccessToAllScopes, Array allowedScopes)
        {
            Clients ab = new Clients();
            string result = ab.updateClientScope(clientId, allowAccessToAllScopes, allowedScopes);
            return Json(result);
        }

        [HttpPost]
        public ActionResult GetClients()
        {
            return Json(Clients.Get());
        }

        [HttpPost]
        public ActionResult GetScopes()
        {
            return Json(Scopes.Get());
        }
    }
}