using Sj.Mg.Idsrv1.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sj.Mg.Idsrv1.Controllers
{
    public class ServiceController : Controller
    {
        [HttpPost]
        public ActionResult AddScope(string name, string displayname, string description, string type, bool emphasize, bool claimsName, bool claimsFamilyName, bool claimsGivenName, bool claimsEmail, bool enable)
        {
            Scopes ab = new Scopes();
            string result = ab.addNewScope(name, displayname, description, type, emphasize, claimsName, claimsGivenName, claimsFamilyName, claimsEmail, enable);
            return Json(result);
        }
        
        [HttpPost]
        public ActionResult UpdateScope(string name, string displayname, string description, string type, bool emphasize, bool claimsName, bool claimsFamilyName, bool claimsGivenName, bool claimsEmail, bool enable)
        {
            Scopes ab = new Scopes();
            string result = ab.updateScope(name, displayname, description, type, emphasize, claimsName, claimsGivenName, claimsFamilyName, claimsEmail, enable);
            return Json(result);
        }

        [HttpPost]
        public ActionResult AddClient(string clientId, string clientName, string flow, Array allowedScopes, string redirectUris, string postLogoutRedirectUris, bool includeJwtId, bool allowRememberConsent, bool allowAccessToAllScopes, bool enable)
        {
            Clients ab = new Clients();
            string result = ab.addNewClient(clientId, clientName, flow, allowedScopes, redirectUris, postLogoutRedirectUris, includeJwtId, allowRememberConsent, allowAccessToAllScopes, enable);
            return Json(result);
        }

        [HttpPost]
        public ActionResult UpdateClient(string clientId, string clientName, string flow, Array allowedScopes, string redirectUris, string postLogoutRedirectUris, bool includeJwtId, bool allowRememberConsent, bool allowAccessToAllScopes, bool enable)
        {
            Clients ab = new Clients();
            string result = ab.updateClient(clientId, clientName, flow, allowedScopes, redirectUris, postLogoutRedirectUris, includeJwtId, allowRememberConsent, allowAccessToAllScopes, enable);
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