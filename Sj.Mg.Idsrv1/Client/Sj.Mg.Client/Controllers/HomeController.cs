using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace Sj.Mg.Client.Controllers
{
    [EnableCors("*", "*", "GET, POST, PATCH", "*")]
    public class HomeController : CliLib.Security.UmaController
    {
        log4net.ILog log = log4net.LogManager.GetLogger(typeof(HomeController));
        [Authorize]
        public async Task<ActionResult> Index()
        {
            var token = (User as ClaimsPrincipal).FindFirst("access_token").Value;
            string basetkn = "";
            var client = new HttpClient();
            dynamic dyn = new System.Dynamic.ExpandoObject();
            try
            {
                client.SetBearerToken(token);
                var actkn = Sj.Mg.CliLib.Utils.TokenHelper.DecodeAndWrite(token);
                var data = await client.GetStringAsync(CliLib.Utils.Common.StsRptTknEndpoint);
                basetkn = data.Replace("\"", "");
                client.SetBearerToken(basetkn);
            }
            catch (Exception exp)
            {
                log.Error("RptTknEndpoint Error" + exp.ToString());
                Console.WriteLine(exp.ToString());
            }
            try
            {
                dyn.Acct1 = Execute<List<Hl7.Fhir.Model.Account>>(CliLib.Utils.Common.ReApiAccount + "123", basetkn);
            }
            catch (Exception exp)
            {
                log.Error("Account Error" + exp.ToString());
                if (exp.Message.Contains("403 (Forbidden)"))
                {
                    dyn.Acct1 = "Acces Denied";
                }
                else
                {
                    dyn.Acct1 = "Unknown Error";
                }
                Console.WriteLine();
            }
            try
            {
                dyn.Medi1 = Execute<List<Hl7.Fhir.Model.Medication>>(CliLib.Utils.Common.ReApiMedication + "123", basetkn);
            }
            catch (Exception exp)
            {
                log.Error("Medication Error" + exp.ToString());
                if (exp.Message.Contains("403 (Forbidden)"))
                {
                    dyn.Medi1 = "Acces Denied";
                }
                else
                {
                    dyn.Medi1 = "Unknown Error";
                }
                Console.WriteLine();
            }
            try
            {
                dyn.Pati1 = Execute<List<dynamic>>(CliLib.Utils.Common.ReApiPatient, basetkn);
            }
            catch (Exception exp)
            {
                log.Error("Patient Error" + exp.ToString());
                if (exp.Message.Contains("403 (Forbidden)"))
                {
                    dyn.Pati1 = "Acces Denied";
                }
                else
                {
                    dyn.Pati1 = "Unknown Error";
                }
                Console.WriteLine();
            }
            try
            {
                dyn.Obsr1 = Execute<List<Hl7.Fhir.Model.Observation>>(CliLib.Utils.Common.ReApiObservation, basetkn);
            }
            catch (Exception exp)
            {
                log.Error("Observation Error" + exp.ToString());
                if (exp.Message.Contains("403 (Forbidden)"))
                {
                    dyn.Obsr1 = "Acces Denied";
                }
                else
                {
                    dyn.Obsr1 = "Unknown Error";
                }
                Console.WriteLine();
            }
            //string json = Newtonsoft.Json.JsonConvert.SerializeObject(dyn, Newtonsoft.Json.Formatting.None, new Newtonsoft.Json.JsonSerializerSettings() { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() });
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(dyn, Formatting.None);
            json = json.Replace("'", "");
            json = json.Replace("\\\"", "");
            json = json.Replace("<div class=atg:role: property - name id=ID xmlns=http://www.w3.org/1999/xhtml />", "");
            ViewBag.Model = json;
            return View();
        }
        [Authorize]
        public ActionResult Secure()
        {
            //var token1 = (User as ClaimsPrincipal).FindFirst("access_token").Value;
            var token1 = (User as ClaimsPrincipal).FindFirst("id_token").Value;
            ViewBag.FullName = (User as ClaimsPrincipal).FindFirst("given_name").Value + ", " + (User as ClaimsPrincipal).FindFirst("family_name").Value;
            var jo = Sj.Mg.CliLib.Utils.TokenHelper.DecodeAndWrite(token1);
            var token = (User as System.Security.Claims.ClaimsPrincipal);
            foreach (var tt in token.Claims)
            {
                Console.WriteLine(tt.Value);
            }
            return View();
        }

        [Authorize]
        [Route("permissionsData")]
        public JsonResult PermissionsData()
        {
            List<Sj.Mg.CliLib.Model.RequestPerm> gg = Sj.Mg.Mongo.MongoManage.GetReqUserPerms(User.Identity.Name);
            return Json(gg, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [Route("user/{id}")]
        public JsonResult SearchUser(string id)
        {
            //var token = (User as System.Security.Claims.ClaimsPrincipal);
            //foreach (var tt in token.Claims)
            //{
            //    Console.WriteLine(tt.Value);
            //}
            List< Sj.Mg.CliLib.Model.CustomUser> gg = Sj.Mg.Mongo.MongoManage.SearchUser(id);
            int index = gg.FindIndex(x => x.Subject == User.Identity.Name);
            
            if(index != -1)
                gg.RemoveAt(index);

            return Json(gg, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [Route("request/{toemail}/{toclient}/{toresrc}/{toscope}")]
        public JsonResult ReqAccess(string toemail, string toclient, string toresrc, string toscope)
        {
            var token = (User as System.Security.Claims.ClaimsPrincipal);
            foreach (var tt1 in token.Claims)
            {
                Console.WriteLine(tt1.Value);
            }
            var un = User.Identity.Name;
            var tt = Sj.Mg.Mongo.MongoManage.GetUserPerms();
            bool alreadyaccess = false;
            tt.ForEach(t =>
            {
                if (t.MyEmail == toemail)
                {
                    if (t.RequestedUsers.ContainsKey(toclient))
                    {
                        if (t.RequestedUsers[toclient].ContainsKey(toresrc))
                        {
                            if (t.RequestedUsers[toclient][toresrc].ContainsKey(toscope))
                            {
                                if (t.RequestedUsers[toclient][toresrc][toscope].Contains(un))
                                {
                                    alreadyaccess = true;
                                }
                                else // user doesnt exist
                                {
                                    t.RequestedUsers[toclient][toresrc][toscope].Add(un);
                                }
                            }
                            else // scope doesnt exist
                            {
                                t.RequestedUsers[toclient][toresrc].Add(toscope, new List<string>());
                                t.RequestedUsers[toclient][toresrc][toscope].Add(un);
                            }
                        } 
                        else // resrc doesnt exist
                        {
                            t.RequestedUsers[toclient].Add(toresrc, new Dictionary<string, List<string>>());
                            t.RequestedUsers[toclient][toresrc].Add(toscope, new List<string>());
                            t.RequestedUsers[toclient][toresrc][toscope].Add(un);
                        }
                    }
                    else // client doesnt exist
                    {
                        t.RequestedUsers.Add(toclient, new Dictionary<string, Dictionary<string, List<string>>>());
                        t.RequestedUsers[toclient].Add(toresrc, new Dictionary<string, List<string>>());
                        t.RequestedUsers[toclient][toresrc].Add(toscope, new List<string>());
                        t.RequestedUsers[toclient][toresrc][toscope].Add(un);
                    }
                    Sj.Mg.Mongo.MongoManage.ReplaceReqPerm(t);
                    alreadyaccess = true;
                }
            });
            if (!alreadyaccess)
            {
                Sj.Mg.CliLib.Model.RequestPerm perm = new CliLib.Model.RequestPerm();
                perm.MyEmail = toemail;
                perm.RequestedUsers.Add(toclient, new Dictionary<string, Dictionary<string, List<string>>>());
                perm.RequestedUsers[toclient].Add(toresrc, new Dictionary<string, List<string>>());
                perm.RequestedUsers[toclient][toresrc].Add(toscope, new List<string>());
                perm.RequestedUsers[toclient][toresrc][toscope].Add(un);
                Sj.Mg.Mongo.MongoManage.Insert<Sj.Mg.CliLib.Model.RequestPerm>(perm, "ReqPerms");
            }
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        [Route("provide/{toemail}/{toclient}/{toresrc}/{toscope}")]
        public JsonResult ProvAccess(string toemail, string toclient, string toresrc, string toscope)
        {
            var token = (User as System.Security.Claims.ClaimsPrincipal);
            foreach (var tt1 in token.Claims)
            {
                Console.WriteLine(tt1.Value);
            }
            var un = User.Identity.Name;
            var tt = Sj.Mg.Mongo.MongoManage.GetUserPerms();
            bool alreadyaccess = false;
            tt.ForEach(t =>
            {
                if (t.MyEmail == toemail)
                {
                    if (t.AllowedUsers.ContainsKey(toclient))
                    {
                        if (t.AllowedUsers[toclient].ContainsKey(toresrc))
                        {
                            if (t.AllowedUsers[toclient][toresrc].ContainsKey(toscope))
                            {
                                if (t.AllowedUsers[toclient][toresrc][toscope].Contains(un))
                                {
                                    alreadyaccess = true;
                                }
                                else // user doesnt exist
                                {
                                    t.AllowedUsers[toclient][toresrc][toscope].Add(un);
                                }
                            }
                            else // scope doesnt exist
                            {
                                t.AllowedUsers[toclient][toresrc].Add(toscope, new List<string>());
                                t.AllowedUsers[toclient][toresrc][toscope].Add(un);
                            }
                        }
                        else // resrc doesnt exist
                        {
                            t.AllowedUsers[toclient].Add(toresrc, new Dictionary<string, List<string>>());
                            t.AllowedUsers[toclient][toresrc].Add(toscope, new List<string>());
                            t.AllowedUsers[toclient][toresrc][toscope].Add(un);
                        }
                    }
                    else // client doesnt exist
                    {
                        t.AllowedUsers.Add(toclient, new Dictionary<string, Dictionary<string, List<string>>>());
                        t.AllowedUsers[toclient].Add(toresrc, new Dictionary<string, List<string>>());
                        t.AllowedUsers[toclient][toresrc].Add(toscope, new List<string>());
                        t.AllowedUsers[toclient][toresrc][toscope].Add(un);
                    }
                    Sj.Mg.Mongo.MongoManage.ReplaceReqPerm(t);
                    alreadyaccess = true;
                }
            });
            if (!alreadyaccess)
            {
                Sj.Mg.CliLib.Model.RequestPerm perm = new CliLib.Model.RequestPerm();
                perm.MyEmail = toemail;
                perm.AllowedUsers.Add(toclient, new Dictionary<string, Dictionary<string, List<string>>>());
                perm.AllowedUsers[toclient].Add(toresrc, new Dictionary<string, List<string>>());
                perm.AllowedUsers[toclient][toresrc].Add(toscope, new List<string>());
                perm.AllowedUsers[toclient][toresrc][toscope].Add(un);
                Sj.Mg.Mongo.MongoManage.Insert<Sj.Mg.CliLib.Model.RequestPerm>(perm, "ReqPerms");
            }
            AddToMySharedList(toemail, un, toclient, toresrc, toscope);
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        void AddToMySharedList(string un, string toemail, string toclient, string toresrc, string toscope)
        {
            var tt = Sj.Mg.Mongo.MongoManage.GetUserPerms();
            bool alreadyaccess = false;
            tt.ForEach(t =>
            {
                if (t.MyEmail == toemail)
                {
                    if (t.MyDetailsSharedWith.ContainsKey(toclient))
                    {
                        if (t.MyDetailsSharedWith[toclient].ContainsKey(toresrc))
                        {
                            if (t.MyDetailsSharedWith[toclient][toresrc].ContainsKey(toscope))
                            {
                                if (t.MyDetailsSharedWith[toclient][toresrc][toscope].Contains(un))
                                {
                                    alreadyaccess = true;
                                }
                                else // user doesnt exist
                                {
                                    t.MyDetailsSharedWith[toclient][toresrc][toscope].Add(un);
                                }
                            }
                            else // scope doesnt exist
                            {
                                t.MyDetailsSharedWith[toclient][toresrc].Add(toscope, new List<string>());
                                t.MyDetailsSharedWith[toclient][toresrc][toscope].Add(un);
                            }
                        }
                        else // resrc doesnt exist
                        {
                            t.MyDetailsSharedWith[toclient].Add(toresrc, new Dictionary<string, List<string>>());
                            t.MyDetailsSharedWith[toclient][toresrc].Add(toscope, new List<string>());
                            t.MyDetailsSharedWith[toclient][toresrc][toscope].Add(un);
                        }
                    }
                    else // client doesnt exist
                    {
                        t.MyDetailsSharedWith.Add(toclient, new Dictionary<string, Dictionary<string, List<string>>>());
                        t.MyDetailsSharedWith[toclient].Add(toresrc, new Dictionary<string, List<string>>());
                        t.MyDetailsSharedWith[toclient][toresrc].Add(toscope, new List<string>());
                        t.MyDetailsSharedWith[toclient][toresrc][toscope].Add(un);
                    }
                    Sj.Mg.Mongo.MongoManage.ReplaceReqPerm(t);
                    alreadyaccess = true;
                }
            });
            if (!alreadyaccess)
            {
                Sj.Mg.CliLib.Model.RequestPerm perm = new CliLib.Model.RequestPerm();
                perm.MyEmail = toemail;
                perm.MyDetailsSharedWith.Add(toclient, new Dictionary<string, Dictionary<string, List<string>>>());
                perm.MyDetailsSharedWith[toclient].Add(toresrc, new Dictionary<string, List<string>>());
                perm.MyDetailsSharedWith[toclient][toresrc].Add(toscope, new List<string>());
                perm.MyDetailsSharedWith[toclient][toresrc][toscope].Add(un);
                Sj.Mg.Mongo.MongoManage.Insert<Sj.Mg.CliLib.Model.RequestPerm>(perm, "ReqPerms");
            }
            removeRequest(un, toemail, toclient, toresrc, toscope);
        }

        public void removeRequest(string un, string toemail, string toclient, string toresrc, string toscope)
        {
            var tt = Sj.Mg.Mongo.MongoManage.GetUserPerms();
            tt.ForEach(t =>
            {
                if (t.MyEmail == toemail)
                {
                    if (t.RequestedUsers.ContainsKey(toclient))
                    {
                        if (t.RequestedUsers[toclient].ContainsKey(toresrc))
                        {
                            if (t.RequestedUsers[toclient][toresrc].ContainsKey(toscope))
                            {
                                if (t.RequestedUsers[toclient][toresrc][toscope].Contains(un))
                                {
                                    //alreadyaccess = true;
                                    t.RequestedUsers[toclient][toresrc][toscope].RemoveAt(t.RequestedUsers[toclient][toresrc][toscope].IndexOf(un));
                                }
                            }
                        }
                    }
                    Sj.Mg.Mongo.MongoManage.ReplaceReqPerm(t);
                    //alreadyaccess = true;
                }
            });
        }

        [Authorize]
        [HttpPost]
        public JsonResult ReqData(Sj.Mg.CliLib.Model.Params.ReqParam para)
        {
            var basetkn = GetEmptyRptToken();
            var acct = Execute<List<Hl7.Fhir.Model.Account>>(CliLib.Utils.Common.ReApiAccount + HttpUtility.UrlEncode(para.email).Replace(".", "^2E"), basetkn);
            return Json(acct, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [Route("account/{id}")]
        public JsonResult GetAccount(string id)
        {
            var basetkn = GetEmptyRptToken();
            var acct = Execute<List<Hl7.Fhir.Model.Account>>(CliLib.Utils.Common.ReApiAccount + id, basetkn);
            return Json(acct, JsonRequestBehavior.AllowGet);
        }
        T ExecuteProc<T>(string url, string basetkn)
        {
            string result = "";
            using (var client = new HttpClient())
            {
                client.SetBearerToken(basetkn);
                var response = client.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;
                    result = responseContent.ReadAsStringAsync().Result;
                    try
                    {
                        Newtonsoft.Json.Linq.JObject job = Newtonsoft.Json.Linq.JObject.Parse(result);
                        if (job.SelectToken("ticket", false) != null)
                        {
                            throw new UnauthorizedAccessException("Unauth exception..");
                        }
                    }
                    catch { }
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedAccessException("Unauth exception 2..");
                }
            }
            return JsonConvert.DeserializeObject<T>(result);
        }

        T Execute<T>(string url, string basetkn)
        {
            string result = "";
            using (var client = new HttpClient())
            {
                client.SetBearerToken(basetkn);
                var response = client.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;
                    result = responseContent.ReadAsStringAsync().Result;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {

                }
            }
            log.Info("Data Recieved" + result);
            string fintkn = ValidPermTkt(result, basetkn);
            if (fintkn != null)
                return ExecuteProc<T>(url, fintkn.Replace("\"", ""));
            //Execute<T>(url, fintkn.Replace("\"",""));
            //Execute<T>(url, Sj.Mg.CliLib.Utils.TokenHelper.CreateJwt(fintkn));
            return  JsonConvert.DeserializeObject<T>(result);
        }

        string ValidPermTkt(string data, string basetkn)
        {
            JsonConvert.DeserializeObject<Sj.Mg.CliLib.Model.permticket>(data);
            Newtonsoft.Json.Linq.JObject job = Newtonsoft.Json.Linq.JObject.Parse(data);
            if (job.SelectToken("ticket", false) != null)
            {
                var ptkt = job.Value<string>("ticket");
                Newtonsoft.Json.Linq.JObject actkn = Sj.Mg.CliLib.Utils.TokenHelper.DecodeAndWrite(basetkn);
                Newtonsoft.Json.Linq.JToken rtk = actkn.SelectToken("rptkn");
                Sj.Mg.CliLib.Model.permission perms = rtk.ToObject<Sj.Mg.CliLib.Model.permission>();
                perms.ticket = ptkt;
                actkn["rptkn"] = JObject.FromObject(perms);
                var http = (HttpWebRequest)WebRequest.Create(CliLib.Utils.Common.StsPermTktValidEndpoint);
                http.ContentType = "application/json";
                http.Headers.Add("Authorization", "Bearer " + Sj.Mg.CliLib.Utils.TokenHelper.CreateJwt(actkn.ToString()));
                http.Accept = "application/json";
                var resp = (HttpWebResponse)http.GetResponse();
                string result = "";
                using (var rdr = new StreamReader(resp.GetResponseStream()))
                {
                    result = rdr.ReadToEnd();
                }
                return result;
            }
            return null;
        }

        public ActionResult Signout()
        {
            Request.GetOwinContext().Authentication.SignOut();
            return Redirect("/");
        }
    }
}