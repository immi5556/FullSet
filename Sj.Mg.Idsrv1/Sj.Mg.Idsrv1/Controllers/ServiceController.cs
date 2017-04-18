using IdentityServer3.Core.Models;
using Sj.Mg.Idsrv1.Config;
using Sj.Mg.Idsrv1.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public ActionResult Registration(string firstName, string lastName, string password, string email, string phoneNumber, bool provider)
        {
            MgUserService temp = new MgUserService();
            string result = temp.addUser(firstName, lastName, password, email, phoneNumber, provider);
            return Json(result);
        }
        public JsonResult RptToken() //From CLient - Empty RPT
        {
            try
            {
                var tte = this.Request.Headers["Authorization"].Replace("Bearer ", "");
                var actkn = Sj.Mg.CliLib.Utils.TokenHelper.DecodeAndWrite(tte);
                var ty = Newtonsoft.Json.Linq.JObject.FromObject(new Sj.Mg.CliLib.Model.Rpt() { });
                actkn.Add("rptkn", Newtonsoft.Json.Linq.JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(new Sj.Mg.CliLib.Model.Rpt()
                {
                    exp = DateTime.UtcNow.AddMinutes(30).Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds,
                    iat = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds
                })));
                return Json(Sj.Mg.CliLib.Utils.TokenHelper.CreateJwt(actkn.ToString()), JsonRequestBehavior.AllowGet);
            }
            catch (Exception exp)
            {
                return Json(new { Error = "Invalid token." }, JsonRequestBehavior.AllowGet);
            }
        }

        [Route("Service/PermTkt/{ids}")]
        public JsonResult PermTkt(string ids) // From rsource server with scopes epected
        {
            try
            {
                var tte = this.Request.Headers["Authorization"].Replace("Bearer ", "");
                Newtonsoft.Json.Linq.JObject actkn = Sj.Mg.CliLib.Utils.TokenHelper.DecodeAndWrite(tte);
                Newtonsoft.Json.Linq.JToken rtk = actkn.SelectToken("rptkn").SelectToken("permissions", false);
                List<Sj.Mg.CliLib.Model.resource> perms = rtk.ToObject<List<Sj.Mg.CliLib.Model.resource>>();
                //create a new permission ticket with list of scopes expected
                Sj.Mg.CliLib.Model.permission rsr = new CliLib.Model.permission();
                rsr.ticket = rsr.Id.ToString();
                rsr.subjects = (ids ?? "").Replace("^2E", ".").Split(',').ToList();
                rsr.requestedscopes = perms;
                Sj.Mg.Mongo.MongoManage.Insert<Sj.Mg.CliLib.Model.permission>(rsr, "PermTkt");
                //return Json(Sj.Mg.CliLib.Utils.TokenHelper.CreateJwt(actkn.ToString()), JsonRequestBehavior.AllowGet);
                return Json(new Sj.Mg.CliLib.Model.permticket() { ticket = rsr.Id.ToString() }, JsonRequestBehavior.AllowGet);
                //return Json(Newtonsoft.Json.Linq.JObject.Parse("{ 'ticket': '" + rsr.Id.ToString() + "' }"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception exp)
            {
                return Json(new { Error = "Invalid token perm tkt." }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ValidatePermTkt() //From Client
        {
            try
            {
                var tte = this.Request.Headers["Authorization"].Replace("Bearer ", "");
                Newtonsoft.Json.Linq.JObject actkn = Sj.Mg.CliLib.Utils.TokenHelper.DecodeAndWrite(tte);
                Newtonsoft.Json.Linq.JToken rtk = actkn.SelectToken("rptkn");
                Sj.Mg.CliLib.Model.permission perms = rtk.ToObject<Sj.Mg.CliLib.Model.permission>();
                Dictionary<string, object> rr = new Dictionary<string, object>();
                rr.Add("_id", perms.ticket);
                List<Sj.Mg.CliLib.Model.permission> rett = Sj.Mg.Mongo.MongoManage.Select(perms.ticket, "PermTkt");
                //((Newtonsoft.Json.Linq.JArray)actkn.SelectToken("scope")).Add("Patient/Medication.Read");
                rett.ForEach(t =>
                {
                    t.requestedscopes.ForEach(t1 =>
                    {
                        t1.scopes.ForEach(t2 =>
                        {
                            try
                            {
                                ((Newtonsoft.Json.Linq.JArray)actkn.SelectToken("scope")).Add(t2);
                            }
                            catch (InvalidCastException e)
                            {
                                var items = new Newtonsoft.Json.Linq.JArray();
                                items.Add((Newtonsoft.Json.Linq.JValue)actkn.SelectToken("scope"));
                                //items.Add(t2); //Stopping to provide acces denied
                                actkn["scope"] = items;
                            }
                        });
                    });
                });

                string ww = JsonWebToken.Encode(actkn, "myrandomclientsecret", JwtHashAlgorithm.RS256);

                //Need to validate whether the user has acees for scopes
                //Sj.Mg.CliLib.Model.permission rsr = new CliLib.Model.permission();
                //rsr.requestedscopes = perms;
                //Sj.Mg.Mongo.MongoManage.Select<Sj.Mg.CliLib.Model.permission>(, "PermTkt");
                return Json(ww, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exp)
            {
                return Json(new { Error = "Invalid token perm tkt." }, JsonRequestBehavior.AllowGet);
            }
        }

        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding)
        {
            return base.Json(data, contentType, contentEncoding);
        }

        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new MgJson()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = 2147483644,
            };
        }
    }

    public class MgJson : JsonResult
    {
        private const string _dateFormat = "yyyy-MM-dd HH:mm:ss";
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            System.Web.HttpResponseBase response = context.HttpContext.Response;

            if (!String.IsNullOrEmpty(ContentType))
            {
                response.ContentType = ContentType;
            }
            else
            {
                response.ContentType = "application/json";
            }
            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }
            if (Data != null)
            {
                response.Write(MySerialize());
            }
        }

        string MySerialize()
        {
            string _dateFormat = "yyyy-MM-dd HH:mm:ss";
            var isoConvert = new Newtonsoft.Json.Converters.IsoDateTimeConverter();
            isoConvert.DateTimeFormat = _dateFormat;
            var refloop = new Newtonsoft.Json.JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            };

            refloop.Converters.Add(isoConvert);
            refloop.Converters.Add(new ObjectHandler());
            return Newtonsoft.Json.JsonConvert.SerializeObject(Data, Newtonsoft.Json.Formatting.None, refloop);
        }

        public class ObjectHandler : Newtonsoft.Json.JsonConverter
        {

            public override bool CanConvert(Type objectType)
            {
                return false;
            }

            public override bool CanRead
            {
                get { return false; }
            }

            public override object ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }

            public override void WriteJson(Newtonsoft.Json.JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
            {

                throw new NotImplementedException();
            }
        }
    }
}