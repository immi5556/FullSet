using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
            if (actionContext.Request.Headers.Authorization != null)
            {
                var tte = actionContext.Request.Headers.Authorization.Parameter;
                JObject jo = Utils.TokenHelper.DecodeAndWrite(tte);
                var httpClient = Utils.HelperHttpClient.GetClient();
                jo.Add("allowed_scope", AllowedScopes[0]);
                httpClient.SetBearerToken(Utils.TokenHelper.CreateJwt(jo.ToString()));
                var tt = httpClient.GetAsync("/Protection/PremissionTicket").Result;
                var msg = tt.Content.ReadAsStringAsync().Result;
            }
            base.HandleUnauthorizedRequest(actionContext);
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            bool vvv = base.IsAuthorized(actionContext);
            return vvv;
        }
    }
}
