using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sj.Mg.Idsrv1.Controllers
{
    public class UmaController : Controller
    {
        [Route(CliLib.Utils.Common.UmaDiscoveryConfiguration)]
        public JsonResult Index()
        {
            return Json(new Models.UmaWellKnownConfig()
            {
                version = "1.0.0",
                issuer = CliLib.Utils.Common.IssuerUri,
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
                dynamic_client_endpoint = CliLib.Utils.Common.UmaDynClientEndPoint,
                rpt_endpoint = CliLib.Utils.Common.UmaRptEndPoint,
                permission_registration_endpoint = CliLib.Utils.Common.UmaResourceSetEndPoint,
                resource_set_registration_endpoint = CliLib.Utils.Common.UmaResourceSetEndPoint,
                introspection_endpoint = CliLib.Utils.Common.StsIntrospectionEndPoint,
                requesting_party_claims_endpoint = "TBD",
                authorization_endpoint = CliLib.Utils.Common.StsAuthorizationEndpoint,
                token_endpoint = CliLib.Utils.Common.StsTokenEndpoint
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ClientRegister(string ss)
        {
            return Json(new { });
        }
    }
}