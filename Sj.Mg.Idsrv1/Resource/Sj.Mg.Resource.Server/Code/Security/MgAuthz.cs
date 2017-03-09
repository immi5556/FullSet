using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Http.Controllers;

namespace Sj.Mg.Resource.Server.Code.Security
{
    public class UmaAuthz : Thinktecture.IdentityModel.WebApi.ScopeAuthorizeAttribute
    {
        public string[] AllowedScopes { get; set; }
        public UmaAuthz(params string[] scopes) : base(scopes)
        {
        }
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            Console.WriteLine();
            base.HandleUnauthorizedRequest(actionContext);
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            bool vvv = base.IsAuthorized(actionContext);
            return vvv;
        }
    }
}