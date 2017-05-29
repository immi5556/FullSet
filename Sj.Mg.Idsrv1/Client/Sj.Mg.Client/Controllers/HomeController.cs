using Hl7.Fhir.Model;
using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sj.Mg.CliLib.Model;
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
using Sj.Mg.Mongo.Data;
using IdentityServer3.Core.Models;
using MongoDB.Driver;
using Sj.Mg.Mongo;
using System.Diagnostics;

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
                dyn.Acct1 = Execute(CliLib.Utils.Common.ReApiAccount + "123", basetkn);
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
                //dyn.Medi1 = Execute<List<Hl7.Fhir.Model.Medication>>(CliLib.Utils.Common.ReApiMedication + "123", basetkn);
                dyn.Medi1 = Execute(CliLib.Utils.Common.ReApiMedication + "123", basetkn);
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
                //dyn.Pati1 = Execute<List<dynamic>>(CliLib.Utils.Common.ReApiPatient, basetkn);
                dyn.Pati1 = Execute(CliLib.Utils.Common.ReApiPatient, basetkn);
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
                dyn.Obsr1 = Execute(CliLib.Utils.Common.ReApiObservation, basetkn);
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
            var prof = (User as ClaimsPrincipal);
            var name = prof.FindFirst("Name") != null ? prof.FindFirst("Name").Value : "";
            var acctknn = prof.FindFirst("access_token") != null ? prof.FindFirst("access_token").Value : "";
            var idtkn = prof.FindFirst("id_token") != null ? prof.FindFirst("id_token").Value : "";
            var gname = prof.FindFirst("given_name") != null ? prof.FindFirst("given_name").Value : "";
            var lname = prof.FindFirst("family_name") != null ? prof.FindFirst("family_name").Value : "";
            if (!string.IsNullOrEmpty(gname) || !string.IsNullOrEmpty(lname))
            {
                ViewBag.FullName = lname + ", " + gname;
                ViewBag.Email = name;
            }
            return View();
        }

        [Authorize]
        public ActionResult UserData()
        {
            var prof = (User as ClaimsPrincipal);
            var name = prof.FindFirst("Name") != null ? prof.FindFirst("Name").Value : "";
            var acctknn = prof.FindFirst("access_token") != null ? prof.FindFirst("access_token").Value : "";
            var idtkn = prof.FindFirst("id_token") != null ? prof.FindFirst("id_token").Value : "";
            var gname = prof.FindFirst("given_name") != null ? prof.FindFirst("given_name").Value : "";
            var lname = prof.FindFirst("family_name") != null ? prof.FindFirst("family_name").Value : "";
            if (!string.IsNullOrEmpty(gname) || !string.IsNullOrEmpty(lname))
            {
                ViewBag.FullName = lname + ", " + gname;
                ViewBag.Email = name;
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
        [HttpPost]
        public JsonResult GetUserData()
        {
            List<Sj.Mg.CliLib.Model.CustomUser> gg = Sj.Mg.Mongo.MongoManage.SearchUser(User.Identity.Name);

            return Json(gg, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        public JsonResult GetClients()
        {
            var _lstclients = MongoManage.SelectClients("Clients");
            return Json(_lstclients, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [Route("user/{id}")]
        public JsonResult SearchUser(string id)
        {
            List<Sj.Mg.CliLib.Model.CustomUser> gg = Sj.Mg.Mongo.MongoManage.SearchUser(id);
            int index = gg.FindIndex(x => x.Subject == User.Identity.Name);

            if (index != -1)
                gg.RemoveAt(index);

            return Json(gg, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [Route("revokeaccess/{toemail}/{toclient}/{toresrc}/{toscope}")]
        public JsonResult RevokeAccess(string toemail, string toclient, string toresrc, string toscope)
        {
            var token = (User as System.Security.Claims.ClaimsPrincipal);
            foreach (var tt1 in token.Claims)
            {
                Console.WriteLine(tt1.Value);
            }
            var un = User.Identity.Name;
            var tt = Sj.Mg.Mongo.MongoManage.GetUserPerms();
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
                                bool itemFound = false;
                                int index = -1;
                                t.AllowedUsers[toclient][toresrc][toscope].ForEach(item =>
                                {
                                    index++;
                                    if (item.user == un)
                                    {
                                        itemFound = true;
                                    }
                                });
                                if (itemFound)
                                {
                                    //alreadyaccess = true;
                                    t.AllowedUsers[toclient][toresrc][toscope].RemoveAt(index);
                                }
                            }
                        }
                    }
                    Sj.Mg.Mongo.MongoManage.ReplaceReqPerm(t);
                }
                if (t.MyEmail == un)
                {
                    if (t.MyDetailsSharedWith.ContainsKey(toclient))
                    {
                        if (t.MyDetailsSharedWith[toclient].ContainsKey(toresrc))
                        {
                            if (t.MyDetailsSharedWith[toclient][toresrc].ContainsKey(toscope))
                            {
                                bool itemFound = false;
                                int index = -1;
                                t.MyDetailsSharedWith[toclient][toresrc][toscope].ForEach(item =>
                                {
                                    index++;
                                    if (item.user == toemail)
                                    {
                                        itemFound = true;
                                    }
                                });
                                if (itemFound)
                                {
                                    //alreadyaccess = true;
                                    t.MyDetailsSharedWith[toclient][toresrc][toscope].RemoveAt(index);
                                }
                            }
                        }
                    }
                    Sj.Mg.Mongo.MongoManage.ReplaceReqPerm(t);
                }
            });
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [Route("updaterequest/{toemail}/{toclient}/{toresrc}/{oldScope}/{toscope}/{relation}")]
        public JsonResult UpdateReqAccess(string toemail, string toclient, string toresrc, string oldScope, string toscope, string relation)
        {
            var token = (User as System.Security.Claims.ClaimsPrincipal);
            foreach (var tt1 in token.Claims)
            {
                Console.WriteLine(tt1.Value);
            }
            var un = User.Identity.Name;
            var tt = Sj.Mg.Mongo.MongoManage.GetUserPerms();
            tt.ForEach(t =>
            {
                if (t.MyEmail == toemail)
                {
                    if (t.AllowedUsers.ContainsKey(toclient))
                    {
                        if (t.AllowedUsers[toclient].ContainsKey(toresrc))
                        {
                            if (t.AllowedUsers[toclient][toresrc].ContainsKey(oldScope))
                            {
                                bool itemFound = false;
                                int index = -1;
                                t.AllowedUsers[toclient][toresrc][oldScope].ForEach(item =>
                                {
                                    index++;
                                    if (item.user == un)
                                    {
                                        itemFound = true;
                                    }
                                });
                                if (itemFound)
                                {
                                    //alreadyaccess = true;
                                    t.AllowedUsers[toclient][toresrc][oldScope].RemoveAt(index);
                                }
                            }
                        }
                    }
                    Sj.Mg.Mongo.MongoManage.ReplaceReqPerm(t);
                }
                if (t.MyEmail == un)
                {
                    if (t.MyDetailsSharedWith.ContainsKey(toclient))
                    {
                        if (t.MyDetailsSharedWith[toclient].ContainsKey(toresrc))
                        {
                            if (t.MyDetailsSharedWith[toclient][toresrc].ContainsKey(oldScope))
                            {
                                bool itemFound = false;
                                int index = -1;
                                t.MyDetailsSharedWith[toclient][toresrc][oldScope].ForEach(item =>
                                {
                                    index++;
                                    if (item.user == toemail)
                                    {
                                        itemFound = true;
                                    }
                                });
                                if (itemFound)
                                {
                                    //alreadyaccess = true;
                                    t.MyDetailsSharedWith[toclient][toresrc][oldScope].RemoveAt(index);
                                }
                            }
                        }
                    }
                    Sj.Mg.Mongo.MongoManage.ReplaceReqPerm(t);
                }
            });
            ProvAccess(toemail, toclient, toresrc, toscope, relation);
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [Route("request/{toemail}/{toclient}/{toresrc}/{toscope}/{relation}")]
        public JsonResult ReqAccess(string toemail, string toclient, string toresrc, string toscope, string relation)
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
                                bool itemFound = false;
                                t.RequestedUsers[toclient][toresrc][toscope].ForEach(item =>
                                {
                                    if (item.user == un)
                                    {
                                        itemFound = true;
                                    }
                                });
                                if (itemFound)
                                {
                                    alreadyaccess = true;
                                }
                                else // user doesnt exist
                                {

                                    UserData userData = new UserData();
                                    userData.user = un;
                                    userData.relation = relation;
                                    t.RequestedUsers[toclient][toresrc][toscope].Add(userData);
                                }
                            }
                            else // scope doesnt exist
                            {
                                t.RequestedUsers[toclient][toresrc].Add(toscope, new List<UserData>());
                                UserData userData = new UserData();
                                userData.user = un;
                                userData.relation = relation;
                                t.RequestedUsers[toclient][toresrc][toscope].Add(userData);
                            }
                        }
                        else // resrc doesnt exist
                        {
                            t.RequestedUsers[toclient].Add(toresrc, new Dictionary<string, List<UserData>>());
                            t.RequestedUsers[toclient][toresrc].Add(toscope, new List<UserData>());
                            UserData userData = new UserData();
                            userData.user = un;
                            userData.relation = relation;
                            t.RequestedUsers[toclient][toresrc][toscope].Add(userData);
                        }
                    }
                    else // client doesnt exist
                    {
                        t.RequestedUsers.Add(toclient, new Dictionary<string, Dictionary<string, List<UserData>>>());
                        t.RequestedUsers[toclient].Add(toresrc, new Dictionary<string, List<UserData>>());
                        t.RequestedUsers[toclient][toresrc].Add(toscope, new List<UserData>());
                        UserData userData = new UserData();
                        userData.user = un;
                        userData.relation = relation;
                        t.RequestedUsers[toclient][toresrc][toscope].Add(userData);
                    }
                    Sj.Mg.Mongo.MongoManage.ReplaceReqPerm(t);
                    alreadyaccess = true;
                }
            });
            if (!alreadyaccess)
            {
                Sj.Mg.CliLib.Model.RequestPerm perm = new CliLib.Model.RequestPerm();
                perm.MyEmail = toemail;
                perm.RequestedUsers.Add(toclient, new Dictionary<string, Dictionary<string, List<UserData>>>());
                perm.RequestedUsers[toclient].Add(toresrc, new Dictionary<string, List<UserData>>());
                perm.RequestedUsers[toclient][toresrc].Add(toscope, new List<UserData>());
                UserData userData = new UserData();
                userData.user = un;
                userData.relation = relation;
                perm.RequestedUsers[toclient][toresrc][toscope].Add(userData);
                Sj.Mg.Mongo.MongoManage.Insert<Sj.Mg.CliLib.Model.RequestPerm>(perm, "ReqPerms");
            }
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        [Route("provide/{toemail}/{toclient}/{toresrc}/{toscope}/{relation}")]
        public JsonResult ProvAccess(string toemail, string toclient, string toresrc, string toscope, string relation)
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
                                bool itemFound = false;
                                t.AllowedUsers[toclient][toresrc][toscope].ForEach(item =>
                                {
                                    if (item.user == un)
                                    {
                                        itemFound = true;
                                    }
                                });
                                if (itemFound)
                                {
                                    alreadyaccess = true;
                                }
                                else // user doesnt exist
                                {
                                    UserData userData = new UserData();
                                    userData.user = un;
                                    userData.relation = relation;
                                    t.AllowedUsers[toclient][toresrc][toscope].Add(userData);
                                }
                            }
                            else // scope doesnt exist
                            {
                                t.AllowedUsers[toclient][toresrc].Add(toscope, new List<UserData>());
                                UserData userData = new UserData();
                                userData.user = un;
                                userData.relation = relation;
                                t.AllowedUsers[toclient][toresrc][toscope].Add(userData);
                            }
                        }
                        else // resrc doesnt exist
                        {
                            t.AllowedUsers[toclient].Add(toresrc, new Dictionary<string, List<UserData>>());
                            t.AllowedUsers[toclient][toresrc].Add(toscope, new List<UserData>());
                            UserData userData = new UserData();
                            userData.user = un;
                            userData.relation = relation;
                            t.AllowedUsers[toclient][toresrc][toscope].Add(userData);
                        }
                    }
                    else // client doesnt exist
                    {
                        t.AllowedUsers.Add(toclient, new Dictionary<string, Dictionary<string, List<UserData>>>());
                        t.AllowedUsers[toclient].Add(toresrc, new Dictionary<string, List<UserData>>());
                        t.AllowedUsers[toclient][toresrc].Add(toscope, new List<UserData>());
                        UserData userData = new UserData();
                        userData.user = un;
                        userData.relation = relation;
                        t.AllowedUsers[toclient][toresrc][toscope].Add(userData);
                    }
                    Sj.Mg.Mongo.MongoManage.ReplaceReqPerm(t);
                    alreadyaccess = true;
                }
            });
            if (!alreadyaccess)
            {
                Sj.Mg.CliLib.Model.RequestPerm perm = new CliLib.Model.RequestPerm();
                perm.MyEmail = toemail;
                perm.AllowedUsers.Add(toclient, new Dictionary<string, Dictionary<string, List<UserData>>>());
                perm.AllowedUsers[toclient].Add(toresrc, new Dictionary<string, List<UserData>>());
                perm.AllowedUsers[toclient][toresrc].Add(toscope, new List<UserData>());
                UserData userData = new UserData();
                userData.user = un;
                userData.relation = relation;
                perm.AllowedUsers[toclient][toresrc][toscope].Add(userData);
                Sj.Mg.Mongo.MongoManage.Insert<Sj.Mg.CliLib.Model.RequestPerm>(perm, "ReqPerms");
            }
            AddToMySharedList(toemail, un, toclient, toresrc, toscope, relation);
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        void AddToMySharedList(string un, string toemail, string toclient, string toresrc, string toscope, string relation)
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
                                bool itemFound = false;
                                t.MyDetailsSharedWith[toclient][toresrc][toscope].ForEach(item =>
                                {
                                    if (item.user == un)
                                    {
                                        itemFound = true;
                                    }
                                });
                                if (itemFound)
                                {
                                    alreadyaccess = true;
                                }
                                else // user doesnt exist
                                {
                                    UserData userData = new UserData();
                                    userData.user = un;
                                    userData.relation = relation;
                                    t.MyDetailsSharedWith[toclient][toresrc][toscope].Add(userData);
                                }
                            }
                            else // scope doesnt exist
                            {
                                t.MyDetailsSharedWith[toclient][toresrc].Add(toscope, new List<UserData>());
                                UserData userData = new UserData();
                                userData.user = un;
                                userData.relation = relation;
                                t.MyDetailsSharedWith[toclient][toresrc][toscope].Add(userData);
                            }
                        }
                        else // resrc doesnt exist
                        {
                            t.MyDetailsSharedWith[toclient].Add(toresrc, new Dictionary<string, List<UserData>>());
                            t.MyDetailsSharedWith[toclient][toresrc].Add(toscope, new List<UserData>());
                            UserData userData = new UserData();
                            userData.user = un;
                            userData.relation = relation;
                            t.MyDetailsSharedWith[toclient][toresrc][toscope].Add(userData);
                        }
                    }
                    else // client doesnt exist
                    {
                        t.MyDetailsSharedWith.Add(toclient, new Dictionary<string, Dictionary<string, List<UserData>>>());
                        t.MyDetailsSharedWith[toclient].Add(toresrc, new Dictionary<string, List<UserData>>());
                        t.MyDetailsSharedWith[toclient][toresrc].Add(toscope, new List<UserData>());
                        UserData userData = new UserData();
                        userData.user = un;
                        userData.relation = relation;
                        t.MyDetailsSharedWith[toclient][toresrc][toscope].Add(userData);
                    }
                    Sj.Mg.Mongo.MongoManage.ReplaceReqPerm(t);
                    alreadyaccess = true;
                }
            });
            if (!alreadyaccess)
            {
                Sj.Mg.CliLib.Model.RequestPerm perm = new CliLib.Model.RequestPerm();
                perm.MyEmail = toemail;
                perm.MyDetailsSharedWith.Add(toclient, new Dictionary<string, Dictionary<string, List<UserData>>>());
                perm.MyDetailsSharedWith[toclient].Add(toresrc, new Dictionary<string, List<UserData>>());
                perm.MyDetailsSharedWith[toclient][toresrc].Add(toscope, new List<UserData>());
                UserData userData = new UserData();
                userData.user = un;
                userData.relation = relation;
                perm.MyDetailsSharedWith[toclient][toresrc][toscope].Add(userData);
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
                                bool itemFound = false;
                                int index = -1;
                                t.RequestedUsers[toclient][toresrc][toscope].ForEach(item =>
                                {
                                    index++;
                                    if (item.user == un)
                                    {
                                        itemFound = true;
                                    }
                                });
                                if (itemFound)
                                {
                                    //alreadyaccess = true;
                                    t.RequestedUsers[toclient][toresrc][toscope].RemoveAt(index);
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
        [Route("denyrequest/{toemail}/{toclient}/{toresrc}/{toscope}")]
        public JsonResult DenyRequest(string toemail, string toclient, string toresrc, string toscope)
        {
            var un = User.Identity.Name;
            var tt = Sj.Mg.Mongo.MongoManage.GetUserPerms();
            tt.ForEach(t =>
            {
                if (t.MyEmail == un)
                {
                    if (t.RequestedUsers.ContainsKey(toclient))
                    {
                        if (t.RequestedUsers[toclient].ContainsKey(toresrc))
                        {
                            if (t.RequestedUsers[toclient][toresrc].ContainsKey(toscope))
                            {
                                bool itemFound = false;
                                int index = -1;
                                t.RequestedUsers[toclient][toresrc][toscope].ForEach(item =>
                                {
                                    index++;
                                    if (item.user == toemail)
                                    {
                                        itemFound = true;
                                    }
                                });
                                if (itemFound)
                                {
                                    //alreadyaccess = true;
                                    t.RequestedUsers[toclient][toresrc][toscope].RemoveAt(index);
                                }
                            }
                        }
                    }
                    Sj.Mg.Mongo.MongoManage.ReplaceReqPerm(t);
                }
            });
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        public JsonResult ReqData(Sj.Mg.CliLib.Model.Params.ReqParam para)
        {
            var basetkn = GetEmptyRptToken();
            if (para.client == "Athena" || para.client == "Athena- Resource Server Api (Authorization Code)")
            {
                if (para.resource == "Demographic")
                {
                    var pats = Execute(CliLib.Utils.Common.ReAhApiPatient + HttpUtility.UrlEncode(para.email).Replace(".", "^2E"), basetkn);
                    return Json(pats, JsonRequestBehavior.AllowGet);
                }
            }
            else if (para.client == "FITBIT")
            {
                if (para.resource == "Demographic")
                {
                    var pats = Execute(CliLib.Utils.Common.ReFbApiPatient + HttpUtility.UrlEncode(para.email).Replace(".", "^2E"), basetkn);
                    return Json(pats, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                if (para.resource == "Demographic")
                {
                    var pats = Execute(CliLib.Utils.Common.ReApiPatient + HttpUtility.UrlEncode(para.email).Replace(".", "^2E"), basetkn);
                    return Json(pats, JsonRequestBehavior.AllowGet);
                }
                if (para.resource == "Diagnostics")
                {
                    var acts = Execute(CliLib.Utils.Common.ReApiAccount + HttpUtility.UrlEncode(para.email).Replace(".", "^2E"), basetkn);
                    return Json(acts, JsonRequestBehavior.AllowGet);
                }
                if (para.resource == "Medication")
                {
                    var meds = Execute(CliLib.Utils.Common.ReApiMedication + HttpUtility.UrlEncode(para.email).Replace(".", "^2E"), basetkn);
                    return Json(meds, JsonRequestBehavior.AllowGet);
                }
                if (para.resource == "Observation")
                {
                    var obs = Execute(CliLib.Utils.Common.ReApiObservation + HttpUtility.UrlEncode(para.email).Replace(".", "^2E"), basetkn);
                    return Json(obs, JsonRequestBehavior.AllowGet);
                }
            }

            return Json("No Access Provided", JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetUserClientsData(string email)
        {
            List<Sj.Mg.CliLib.Model.UserClientsList> gg = Sj.Mg.Mongo.MongoManage.SearchUserClients(email);
            return Json(gg, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        public ActionResult UpdateUserClientsData(UserClientsList data)
        {
            List<Sj.Mg.CliLib.Model.UserClientsList> gg = Sj.Mg.Mongo.MongoManage.SearchUserClients(data.email);
            gg[0].UserClientsData = data.UserClientsData;
            Sj.Mg.Mongo.MongoManage.UpdateUserClients(gg[0]);

            return Json("Success", JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [Route("account/{id}")]
        public JsonResult GetAccount(string id)
        {
            var basetkn = GetEmptyRptToken();
            var acct = Execute(CliLib.Utils.Common.ReApiAccount + id, basetkn);
            return Json(acct, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Callback()
        {
            var prof = (User as ClaimsPrincipal);
            var email = prof.FindFirst("Name") != null ? prof.FindFirst("Name").Value : "";
            var dd = Sj.Mg.Mongo.MongoManage.SearchUserClients(email); 
            var code = Request.QueryString["code"];
            string url = "https://api.fitbit.com/oauth2/token?clientId=228JJF&grant_type=authorization_code&redirect_uri=https%3A%2F%2Flocalhost%3A44383%2FHome%2FCallback&code=" + code;
            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";
            request.Headers["Authorization"] = "Basic MjI4SkpGOjNjMjY4YTllYzFiYTMzNDJkNTEyNzIyMDkzMDc5NGYx";
            request.ContentType = "application/x-www-form-urlencoded";
            WebResponse response = request.GetResponse();
            Stream receive = response.GetResponseStream();
            StreamReader reader = new StreamReader(receive, System.Text.Encoding.UTF8);
            var tt = reader.ReadToEnd();
            Newtonsoft.Json.Linq.JObject token = Newtonsoft.Json.Linq.JObject.Parse(tt);
            dd[0].UserClientsData.ForEach(t =>
            {
                if ( t.Clients != null && t.Clients.Count > 0 && t.Clients[0] != null)
                {
                    t.Clients.ForEach(t1 =>
                    {
                        if (t1.clientName == "FITBIT")
                        {
                            t1.AccessToken = token.ToString();
                        }
                    });
                }
            });
            Sj.Mg.Mongo.MongoManage.UpdateUserClients(dd[0]);
            return RedirectToAction("Secure");
        }

        string ExecuteProc(string url, string basetkn)
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
            //return JsonConvert.DeserializeObject<T>(result);
            return result;
        }

        string Execute(string url, string basetkn)
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
                    Console.WriteLine("");
                }
                else
                {
                    var responseContent = response.Content;
                    result = responseContent.ReadAsStringAsync().Result;
                }
            }
            log.Info("Data Recieved" + result);
            string fintkn = ValidPermTkt(result, basetkn);
            if (fintkn != null)
                return ExecuteProc(url, fintkn.Replace("\"", ""));
            //Execute<T>(url, fintkn.Replace("\"",""));
            //Execute<T>(url, Sj.Mg.CliLib.Utils.TokenHelper.CreateJwt(fintkn));
            //return  JsonConvert.DeserializeObject<T>(result);
            return result;
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

        public JsonResult updateobs(string comment, string language, string email)
        {
            var token = (User as ClaimsPrincipal).FindFirst("access_token").Value;
            string basetkn = "";
            var client = new HttpClient();
            try
            {
                client.SetBearerToken(token);
                var actkn = Sj.Mg.CliLib.Utils.TokenHelper.DecodeAndWrite(token);
                //var obs = Execute(CliLib.Utils.Common.ReApiObservation + HttpUtility.UrlEncode(email).Replace(".", "^2E"), basetkn);
                Hl7.Fhir.Model.Observation dataModel = new Hl7.Fhir.Model.Observation();
                dataModel.Comments = comment;
                dataModel.Id = email;
                dataModel.Language = language;
                using (HttpResponseMessage response = client.PostAsJsonAsync(CliLib.Utils.Common.ReApiObservation, dataModel).Result)
                    return Json("success");
            }
            catch (Exception exp)
            {
                log.Error("RptTknEndpoint Error" + exp.ToString());
                Console.WriteLine(exp.ToString());
                return Json("failed");
            }
        }

        public JsonResult updatediag(string description, string status, string email)
        {
            var token = (User as ClaimsPrincipal).FindFirst("access_token").Value;
            string basetkn = "";
            var client = new HttpClient();
            try
            {
                client.SetBearerToken(token);
                var actkn = Sj.Mg.CliLib.Utils.TokenHelper.DecodeAndWrite(token);
                Hl7.Fhir.Model.Account dataModel = new Hl7.Fhir.Model.Account();
                dataModel.Description = description;
                dataModel.Id = email;
                dataModel.Status = status;
                using (HttpResponseMessage response = client.PostAsJsonAsync(CliLib.Utils.Common.ReApiAccount, dataModel).Result)
                    return Json("success");
            }
            catch (Exception exp)
            {
                log.Error("RptTknEndpoint Error" + exp.ToString());
                Console.WriteLine(exp.ToString());
                return Json("failed");
            }
        }

        public JsonResult updatedemographic(string familyName, string givenName, string birthday, string email)
        {
            var token = (User as ClaimsPrincipal).FindFirst("access_token").Value;
            string basetkn = "";
            var client = new HttpClient();
            try
            {
                client.SetBearerToken(token);
                var actkn = Sj.Mg.CliLib.Utils.TokenHelper.DecodeAndWrite(token);
                Hl7.Fhir.Model.Patient dataModel = new Hl7.Fhir.Model.Patient();
                dataModel.Id = email;
                dataModel.BirthDate = birthday;
                dataModel.Name = new List<HumanName>()
                {
                    new HumanName()
                    {
                        Family = new string[] { familyName },
                        Given = new string[] { givenName }
                    },
                    new HumanName()
                    {
                        Given = new string[] { givenName }

                    }
                };
                using (HttpResponseMessage response = client.PostAsJsonAsync(CliLib.Utils.Common.ReApiPatient, dataModel).Result)
                    return Json("success");
            }
            catch (Exception exp)
            {
                log.Error("RptTknEndpoint Error" + exp.ToString());
                Console.WriteLine(exp.ToString());
                return Json("failed");
            }
        }

        public JsonResult updatemed(string language, string email, string dateAss)
        {
            var token = (User as ClaimsPrincipal).FindFirst("access_token").Value;
            string basetkn = "";
            var client = new HttpClient();
            try
            {
                client.SetBearerToken(token);
                var actkn = Sj.Mg.CliLib.Utils.TokenHelper.DecodeAndWrite(token);
                Hl7.Fhir.Model.MedicationStatement dataModel = new Hl7.Fhir.Model.MedicationStatement();
                dataModel.Language = language;
                dataModel.Id = email;
                dataModel.DateAsserted = dateAss;

                using (HttpResponseMessage response = client.PostAsJsonAsync(CliLib.Utils.Common.ReApiMedication, dataModel).Result)
                    return Json("success");
            }
            catch (Exception exp)
            {
                log.Error("RptTknEndpoint Error" + exp.ToString());
                Console.WriteLine(exp.ToString());
                return Json("failed");
            }
        }
    }
}