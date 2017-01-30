using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
//using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace Sj.Mg.Idsrv4.Controllers
{
    public class UmaController : Controller
    {
        public JsonResult UmaWellKnownConfig()
        {
            return Json(new AppConstants.Model.UmaWellKnownConfig()
            {
                version = "1.0.0",
                issuer = AppConstants.Constants.IssuerUri,
                pat_profiles_supported = new List<string>()
                {
                    "bearer"
                },
                aat_profiles_supported = new List<string>()
                {
                    "bearer"
                },
                rpt_profiles_supported = new List<string>()
                {
                    "bearer"
                },
                pat_grant_types_supported = new List<string>()
                {
                    "authorization_code"
                },
                aat_grant_types_supported = new List<string>()
                {
                    "authorization_code"
                },
                dynamic_client_endpoint = AppConstants.Constants.UmaDynClientEndPoint,
                rpt_endpoint = AppConstants.Constants.UmaRptEndPoint,
                permission_registration_endpoint = AppConstants.Constants.UmaResourceSetEndPoint,
                resource_set_registration_endpoint = AppConstants.Constants.UmaResourceSetEndPoint,
                introspection_endpoint = AppConstants.Constants.StsIntrospectionEndPoint,
                requesting_party_claims_endpoint = "TBD",
                authorization_endpoint = AppConstants.Constants.StsAuthorizationEndpoint,
                token_endpoint = AppConstants.Constants.StsTokenEndpoint
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult ClientRegister()
        {
            var nv = Request.Url.ParseQueryString();
            var cll = (nv["client_id"] ?? "");
            var cli = Config.Clients.Get().ToList().Find(t => t.ClientId == cll);
            if (cli == null)
                return Json(Config.Clients.RegisterClients(nv), JsonRequestBehavior.AllowGet);
            return Json(cli, JsonRequestBehavior.AllowGet);
        }
    }
}