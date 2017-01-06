using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
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
                dynamic_client_endpoint = "TBD",
                rpt_endpoint = "TBD",
                permission_registration_endpoint = AppConstants.Constants.UmaResourceSetEndPoint,
                resource_set_registration_endpoint = AppConstants.Constants.UmaResourceSetEndPoint,
                introspection_endpoint = "TBD",
                requesting_party_claims_endpoint = "TBD",
                authorization_endpoint = AppConstants.Constants.StsAuthorizationEndpoint,
                token_endpoint = AppConstants.Constants.StsTokenEndpoint
            }, JsonRequestBehavior.AllowGet);
        }
    }
}