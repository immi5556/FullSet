using IdentityServer3.Core.Models;
using Sj.Mg.Idsrv1.Config;
using Sj.Mg.Idsrv1.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sj.Mg.Idsrv1.Controllers
{
    [IdentityServer3.Extensions.Mvc.Filters.IdentityServerFullLoginAttribute]
    public class ServiceController : Controller
    {
        [HttpPost]
        public ActionResult AddScope(string name, string displayname, string description, string type, bool emphasize, List<ScopeClaim> claimsObj, bool enable)
        {
            Scopes ab = new Scopes();
            string result = ab.addNewScope(name, displayname, description, type, emphasize, claimsObj, enable);
            return Json(result);
        }
        
        [HttpPost]
        public ActionResult UpdateScope(string name, string displayname, string description, string type, bool emphasize, List<ScopeClaim> claimsObj, bool enable)
        {
            Scopes ab = new Scopes();
            string result = ab.updateScope(name, displayname, description, type, emphasize, claimsObj, enable);
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

        [HttpPost]
        public ActionResult Registration(string firstName, string lastName, string password, string email, string phoneNumber)
        {
            MgUserService temp = new MgUserService();
            string result = temp.addUser(firstName, lastName, password, email, phoneNumber);
            return Json(result);
        }

        public JsonResult RptToken()
        {
            var tte = this.Request.Headers["Authorization"].Replace("Bearer ", "");
            var actkn = Sj.Mg.CliLib.Utils.TokenHelper.DecodeAndWrite(tte);
            actkn.Add("rptkn", Newtonsoft.Json.Linq.JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(new Sj.Mg.CliLib.Model.Rpt() { })));
            return Json(actkn.ToObject(typeof(object)), JsonRequestBehavior.AllowGet);
        }
    }
}