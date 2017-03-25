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
    public class UmaAuthz : Thinktecture.IdentityModel.WebApi.ScopeAuthorizeAttribute
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
                if (jo.SelectToken("rptkn", false) == null)
                {
                    base.HandleUnauthorizedRequest(actionContext);
                    return;
                }
                var scp = jo.SelectToken("scope", false);
                if (scp == null)
                {
                    base.HandleUnauthorizedRequest(actionContext);
                    return;
                }
                bool allow = false;
                (AllowedScopes).ToList().ForEach(t =>
                {
                    JArray items = (JArray)scp;
                    var tim = items.ToObject<string[]>();
                    foreach (string o in tim)
                    {
                        if (AllowedScopes.Contains(o))
                        {
                            allow = true;
                            break;
                        }
                    }
                });

                //JToken rtk = jo.SelectToken("rptkn").SelectToken("permissions", false);
                //List<Model.resource> perms = rtk.ToObject<List<Model.resource>>();
                //bool allow = false;
                //perms.ForEach(t =>
                //{
                //    if (t.scopes.Intersect(AllowedScopes).Any())
                //    {
                //        allow = true;
                //        return;
                //    }
                //});
                if (!allow)
                {
                    var httpClient = Utils.HelperHttpClient.GetClient();
                    Model.resource rsrc = new Model.resource();
                    rsrc.scopes.AddRange(AllowedScopes);
                    List<Model.resource> perms = new List<Model.resource>();
                    perms.Clear();
                    perms.Add(rsrc);
                    jo.SelectToken("rptkn")["permissions"] = JArray.FromObject(perms);
                    //jo["rptkn"] =   perms;
                    httpClient.SetBearerToken(Utils.TokenHelper.CreateJwt(jo.ToString()));
                    var tt = httpClient.GetAsync("https://localhost:44305/Service/PermTkt").Result;
                    var msg = tt.Content.ReadAsStringAsync().Result;
                    HttpResponseMessage responseMessage = new HttpResponseMessage()
                    {
                        Content = new StringContent(msg),
                        ReasonPhrase = "Permission ticket"
                    };
                    responseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    responseMessage.Content.Headers.Add("Access-Control-Allow-Origin", "*");
                    actionContext.Response = responseMessage;
                }
                else if (allow)
                {

                }
                else
                {
                    if (jo.SelectToken("rptkn").SelectToken("permissions", false) == null)
                    {
                        base.HandleUnauthorizedRequest(actionContext);
                        return;
                    }
                }
            }
            else
            {
                base.HandleUnauthorizedRequest(actionContext);
            }
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            bool vvv = base.IsAuthorized(actionContext);
            return vvv;
        }
    }
}
