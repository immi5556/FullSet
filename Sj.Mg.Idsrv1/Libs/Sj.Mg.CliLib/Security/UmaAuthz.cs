using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace Sj.Mg.CliLib.Security
{
    public class UmaAuthz: Thinktecture.IdentityModel.WebApi.ScopeAuthorizeAttribute
    {
        public string[] AllowedScopes { get; set; }
        public UmaAuthz(params string[] scopes) : base(scopes)
        {
            AllowedScopes = scopes;
        }
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            var tte = actionContext.Request.Headers.Authorization.Parameter;
            JObject jo = Utils.TokenHelper.DecodeAndWrite(tte);
            base.HandleUnauthorizedRequest(actionContext);
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            bool vvv = base.IsAuthorized(actionContext);
            return vvv;
        }
    }
}
