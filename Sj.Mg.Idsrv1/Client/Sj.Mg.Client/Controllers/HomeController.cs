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
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

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
            var idp = prof.FindFirst("idp") != null ? prof.FindFirst("idp").Value : "";
            if (idp == "idsrv")
                idp = "MgIdp";
            if (name != "")
            {
                string pattern = @"^[a-z][a-z|0-9|]*([_][a-z|0-9]+)*([.][a-z|0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]*\.([a-z][a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$";
                Match match = Regex.Match(name.Trim(), pattern, RegexOptions.IgnoreCase);

                if (!match.Success)
                {
                    List<Sj.Mg.CliLib.Model.CustomUser> data = Sj.Mg.Mongo.MongoManage.SearchUserByProviderId(name);
                    if(data.Count > 0)
                    {
                        ViewBag.FullName = data[0].GivenName;
                        ViewBag.Email = data[0].Email;
                    }
                }else
                {
                    if (!string.IsNullOrEmpty(gname) || !string.IsNullOrEmpty(lname))
                    {
                        ViewBag.FullName = lname + ", " + gname;
                        ViewBag.Email = name;
                    }
                }
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
            var idp = prof.FindFirst("idp") != null ? prof.FindFirst("idp").Value : "";
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
            var prof = (User as ClaimsPrincipal);
            var idp = prof.FindFirst("idp") != null ? prof.FindFirst("idp").Value : "";
            var name = prof.FindFirst("Name") != null ? prof.FindFirst("Name").Value : "";
            if (idp == "idsrv")
                idp = "MgIdp";
            List<Sj.Mg.CliLib.Model.RequestPerm> gg = Sj.Mg.Mongo.MongoManage.GetReqUserPerms(name, idp);
            return Json(gg, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        public JsonResult GetUserData()
        {
            var prof = (User as ClaimsPrincipal);
            var idp = prof.FindFirst("idp") != null ? prof.FindFirst("idp").Value : "";
            var name = prof.FindFirst("Name") != null ? prof.FindFirst("Name").Value : "";
            if (idp == "idsrv")
                idp = "MgIdp";
            if (name != "")
            {
                string pattern = @"^[a-z][a-z|0-9|]*([_][a-z|0-9]+)*([.][a-z|0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]*\.([a-z][a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$";
                Match match = Regex.Match(name.Trim(), pattern, RegexOptions.IgnoreCase);

                if (!match.Success)
                {
                    List<Sj.Mg.CliLib.Model.CustomUser> data = Sj.Mg.Mongo.MongoManage.SearchUserByProviderId(name);
                    return Json(data, JsonRequestBehavior.AllowGet);
                }else
                {
                    List<Sj.Mg.CliLib.Model.CustomUser> gg = Sj.Mg.Mongo.MongoManage.SearchUser(name, idp);
                    return Json(gg, JsonRequestBehavior.AllowGet);
                }
            }else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
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
            List<Sj.Mg.CliLib.Model.CustomUser> gg = Sj.Mg.Mongo.MongoManage.SearchByName(id);
            int index = gg.FindIndex(x => x.Subject == User.Identity.Name);

            if (index != -1)
                gg.RemoveAt(index);

            return Json(gg, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [Route("users")]
        public JsonResult GetUsers(string id)
        {
            List<Sj.Mg.CliLib.Model.CustomUser> gg = Sj.Mg.Mongo.MongoManage.GetUsers();
            int index = gg.FindIndex(x => x.Subject == User.Identity.Name);

            if (index != -1)
                gg.RemoveAt(index);

            return Json(gg, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [Route("revokeaccess/{toemail}/{toclient}/{toresrc}/{toscope}/{usrProvide}/{toUsrProvide}")]
        public JsonResult RevokeAccess(string toemail, string toclient, string toresrc, string toscope, string usrProvide, string toUsrProvide)
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
                if (t.MyEmail == toemail && t.Provider == toUsrProvide)
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
                                    if (item.user == un && item.provider == usrProvide)
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
                if (t.MyEmail == un && t.Provider == usrProvide)
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
                                    if (item.user == toemail && item.provider == toUsrProvide)
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
        [Route("updaterequest/{toemail}/{toclient}/{toresrc}/{oldScope}/{toscope}/{relation}/{usrProvide}/{toUsrProvide}")]
        public JsonResult UpdateReqAccess(string toemail, string toclient, string toresrc, string oldScope, string toscope, string relation, string usrProvide, string toUsrProvide)
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
                if (t.MyEmail == toemail && t.Provider == toUsrProvide)
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
                                    if (item.user == un && item.provider == usrProvide)
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
                if (t.MyEmail == un && t.Provider == usrProvide)
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
                                    if (item.user == toemail && item.provider == toUsrProvide)
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
            ProvAccess(toemail, toclient, toresrc, toscope, relation, usrProvide, toUsrProvide);
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [Route("request/{toemail}/{toclient}/{toresrc}/{toscope}/{relation}/{usrProvide}/{toUsrProvide}")]
        public JsonResult ReqAccess(string toemail, string toclient, string toresrc, string toscope, string relation, string usrProvide, string toUsrProvide)
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
                if (t.MyEmail == toemail && t.Provider == toUsrProvide)
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
                                    userData.provider = usrProvide;
                                    t.RequestedUsers[toclient][toresrc][toscope].Add(userData);
                                }
                            }
                            else // scope doesnt exist
                            {
                                t.RequestedUsers[toclient][toresrc].Add(toscope, new List<UserData>());
                                UserData userData = new UserData();
                                userData.user = un;
                                userData.relation = relation;
                                userData.provider = usrProvide;
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
                            userData.provider = usrProvide;
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
                        userData.provider = usrProvide;
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
                perm.Provider = toUsrProvide;
                perm.RequestedUsers.Add(toclient, new Dictionary<string, Dictionary<string, List<UserData>>>());
                perm.RequestedUsers[toclient].Add(toresrc, new Dictionary<string, List<UserData>>());
                perm.RequestedUsers[toclient][toresrc].Add(toscope, new List<UserData>());
                UserData userData = new UserData();
                userData.user = un;
                userData.relation = relation;
                userData.provider = usrProvide;
                perm.RequestedUsers[toclient][toresrc][toscope].Add(userData);
                Sj.Mg.Mongo.MongoManage.Insert<Sj.Mg.CliLib.Model.RequestPerm>(perm, "ReqPerms");
            }
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        [Route("provide/{toemail}/{user}/{toclient}/{toresrc}/{toscope}/{relation}/{usrProvide}/{toUsrProvide}/{provider}")]
        public JsonResult ProvAccess(string toemail, string user, string toclient, string toresrc, string toscope, string relation, string usrProvide, string toUsrProvide, string provider)
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
                if (t.MyEmail == toemail && t.Provider == toUsrProvide)
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
                                    if (item.user == user && item.provider == usrProvide)
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
                                    userData.user = user;
                                    userData.provider = usrProvide;
                                    userData.sharedBy = un;
                                    userData.sharedByProvider = provider;
                                    userData.relation = relation;
                                    t.AllowedUsers[toclient][toresrc][toscope].Add(userData);
                                }
                            }
                            else // scope doesnt exist
                            {
                                t.AllowedUsers[toclient][toresrc].Add(toscope, new List<UserData>());
                                UserData userData = new UserData();
                                userData.user = user;
                                userData.provider = usrProvide;
                                userData.sharedBy = un;
                                userData.sharedByProvider = provider;
                                userData.relation = relation;
                                t.AllowedUsers[toclient][toresrc][toscope].Add(userData);
                            }
                        }
                        else // resrc doesnt exist
                        {
                            t.AllowedUsers[toclient].Add(toresrc, new Dictionary<string, List<UserData>>());
                            t.AllowedUsers[toclient][toresrc].Add(toscope, new List<UserData>());
                            UserData userData = new UserData();
                            userData.user = user;
                            userData.provider = usrProvide;
                            userData.sharedBy = un;
                            userData.sharedByProvider = provider;
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
                        userData.user = user;
                        userData.provider = usrProvide;
                        userData.sharedBy = un;
                        userData.sharedByProvider = provider;
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
                perm.Provider = toUsrProvide;
                perm.AllowedUsers.Add(toclient, new Dictionary<string, Dictionary<string, List<UserData>>>());
                perm.AllowedUsers[toclient].Add(toresrc, new Dictionary<string, List<UserData>>());
                perm.AllowedUsers[toclient][toresrc].Add(toscope, new List<UserData>());
                UserData userData = new UserData();
                userData.user = user;
                userData.provider = usrProvide;
                userData.sharedBy = un;
                userData.sharedByProvider = provider;
                userData.relation = relation;
                perm.AllowedUsers[toclient][toresrc][toscope].Add(userData);
                Sj.Mg.Mongo.MongoManage.Insert<Sj.Mg.CliLib.Model.RequestPerm>(perm, "ReqPerms");
            }
            AddToMainUserSharedList(toemail, un, user, toclient, toresrc, toscope, relation, usrProvide, toUsrProvide, provider);
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }
        void AddToMainUserSharedList(string un, string toemail, string mainUser, string toclient, string toresrc, string toscope, string relation, string usrProvide, string toUsrProvide, string provider)
        {
            var tt = Sj.Mg.Mongo.MongoManage.GetUserPerms();
            bool alreadyaccess = false;
            tt.ForEach(t =>
            {
                if (t.MyEmail == mainUser && t.Provider == usrProvide)
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
                                    if (item.user == un && item.provider == toUsrProvide)
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
                                    userData.provider = toUsrProvide;
                                    userData.sharedBy = toemail;
                                    userData.sharedByProvider = provider;
                                    userData.relation = relation;
                                    t.MyDetailsSharedWith[toclient][toresrc][toscope].Add(userData);
                                }
                            }
                            else // scope doesnt exist
                            {
                                t.MyDetailsSharedWith[toclient][toresrc].Add(toscope, new List<UserData>());
                                UserData userData = new UserData();
                                userData.user = un;
                                userData.provider = toUsrProvide;
                                userData.sharedBy = toemail;
                                userData.sharedByProvider = provider;
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
                            userData.provider = toUsrProvide;
                            userData.sharedBy = toemail;
                            userData.sharedByProvider = provider;
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
                        userData.provider = toUsrProvide;
                        userData.sharedBy = toemail;
                        userData.sharedByProvider = provider;
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
                perm.Provider = usrProvide;
                perm.MyDetailsSharedWith.Add(toclient, new Dictionary<string, Dictionary<string, List<UserData>>>());
                perm.MyDetailsSharedWith[toclient].Add(toresrc, new Dictionary<string, List<UserData>>());
                perm.MyDetailsSharedWith[toclient][toresrc].Add(toscope, new List<UserData>());
                UserData userData = new UserData();
                userData.user = un;
                userData.provider = toUsrProvide;
                userData.sharedBy = toemail;
                userData.sharedByProvider = provider;
                userData.relation = relation;
                perm.MyDetailsSharedWith[toclient][toresrc][toscope].Add(userData);
                Sj.Mg.Mongo.MongoManage.Insert<Sj.Mg.CliLib.Model.RequestPerm>(perm, "ReqPerms");
            }
        }
        [Authorize]
        [Route("provide/{toemail}/{toclient}/{toresrc}/{toscope}/{relation}/{usrProvide}/{toUsrProvide}")]
        public JsonResult ProvAccess(string toemail, string toclient, string toresrc, string toscope, string relation, string usrProvide, string toUsrProvide)
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
                if (t.MyEmail == toemail && t.Provider == toUsrProvide)
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
                                    userData.provider = usrProvide;
                                    t.AllowedUsers[toclient][toresrc][toscope].Add(userData);
                                }
                            }
                            else // scope doesnt exist
                            {
                                t.AllowedUsers[toclient][toresrc].Add(toscope, new List<UserData>());
                                UserData userData = new UserData();
                                userData.user = un;
                                userData.relation = relation;
                                userData.provider = usrProvide;
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
                            userData.provider = usrProvide;
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
                        userData.provider = usrProvide;
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
                perm.Provider = toUsrProvide;
                perm.AllowedUsers.Add(toclient, new Dictionary<string, Dictionary<string, List<UserData>>>());
                perm.AllowedUsers[toclient].Add(toresrc, new Dictionary<string, List<UserData>>());
                perm.AllowedUsers[toclient][toresrc].Add(toscope, new List<UserData>());
                UserData userData = new UserData();
                userData.user = un;
                userData.relation = relation;
                userData.provider = usrProvide;
                perm.AllowedUsers[toclient][toresrc][toscope].Add(userData);
                Sj.Mg.Mongo.MongoManage.Insert<Sj.Mg.CliLib.Model.RequestPerm>(perm, "ReqPerms");
            }
            AddToMySharedList(toemail, un, toclient, toresrc, toscope, relation, usrProvide, toUsrProvide);
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }
        
        void AddToMySharedList(string un, string toemail, string toclient, string toresrc, string toscope, string relation, string usrProvide, string toUsrProvide)
        {
            var tt = Sj.Mg.Mongo.MongoManage.GetUserPerms();
            bool alreadyaccess = false;
            tt.ForEach(t =>
            {
                if (t.MyEmail == toemail && t.Provider == usrProvide)
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
                                    userData.provider = toUsrProvide;
                                    t.MyDetailsSharedWith[toclient][toresrc][toscope].Add(userData);
                                }
                            }
                            else // scope doesnt exist
                            {
                                t.MyDetailsSharedWith[toclient][toresrc].Add(toscope, new List<UserData>());
                                UserData userData = new UserData();
                                userData.user = un;
                                userData.relation = relation;
                                userData.provider = toUsrProvide;
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
                            userData.provider = toUsrProvide;
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
                        userData.provider = toUsrProvide;
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
                perm.Provider = usrProvide;
                perm.MyDetailsSharedWith.Add(toclient, new Dictionary<string, Dictionary<string, List<UserData>>>());
                perm.MyDetailsSharedWith[toclient].Add(toresrc, new Dictionary<string, List<UserData>>());
                perm.MyDetailsSharedWith[toclient][toresrc].Add(toscope, new List<UserData>());
                UserData userData = new UserData();
                userData.user = un;
                userData.relation = relation;
                userData.provider = toUsrProvide;
                perm.MyDetailsSharedWith[toclient][toresrc][toscope].Add(userData);
                Sj.Mg.Mongo.MongoManage.Insert<Sj.Mg.CliLib.Model.RequestPerm>(perm, "ReqPerms");
            }
            removeRequest(un, toemail, toclient, toresrc, toscope, usrProvide, toUsrProvide);
        }

        public void removeRequest(string un, string toemail, string toclient, string toresrc, string toscope, string usrProvide, string toUsrProvide)
        {
            var tt = Sj.Mg.Mongo.MongoManage.GetUserPerms();
            tt.ForEach(t =>
            {
                if (t.MyEmail == toemail && t.Provider == usrProvide)
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
                                    if (item.user == un && item.provider == toUsrProvide)
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
        [Route("denyrequest/{toemail}/{toclient}/{toresrc}/{toscope}/{usrProvide}/{toUsrProvide}")]
        public JsonResult DenyRequest(string toemail, string toclient, string toresrc, string toscope, string usrProvide, string toUsrProvide)
        {
            var un = User.Identity.Name;
            var tt = Sj.Mg.Mongo.MongoManage.GetUserPerms();
            tt.ForEach(t =>
            {
                if (t.MyEmail == un && t.Provider == usrProvide)
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
                                    if (item.user == toemail && item.provider == toUsrProvide)
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
        public ActionResult GetAthenaPatientId( string provider, string patientId)
        {
            var response = checkForPatientId(User.Identity.Name, provider, patientId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public string checkForPatientId(string email, string provider, string patientId)
        {
            List<Sj.Mg.CliLib.Model.UserClientsList> gg = Sj.Mg.Mongo.MongoManage.SearchUserClients(email, provider);
            string response = "";
            if (gg != null && gg.Count > 0 && gg[0].UserClientsData.Count > 0)
            {
                gg[0].UserClientsData.ForEach(clientType =>
                {
                    if (clientType!= null && clientType.Clients != null)
                    {
                        clientType.Clients.ForEach(clients =>
                        {
                            if (clients != null && clients.clientName == "Athena")
                            {
                                if (clients.PatientId != null)
                                {
                                    response = "patientId exists";
                                }
                                else
                                {
                                    if(patientId != null && patientId != "")
                                    {
                                        clients.PatientId = patientId;
                                        Sj.Mg.Mongo.MongoManage.UpdateUserClients(gg[0]);
                                        response = "inserted";
                                    }
                                    else
                                    {
                                        response = "patientId not exists";
                                    }
                                }
                            }
                        });
                    }
                    else
                    {
                        response = "no client";
                    }
                });
            }
            else
            {
                response = "no client";
            }
            return response;
        }

        [Authorize]
        [HttpPost]
        public JsonResult ReqData(Sj.Mg.CliLib.Model.Params.ReqParam para)
        {
            var basetkn = GetEmptyRptToken();            
            if (para.client == "Athena" || para.client == "Athena- Resource Server Api (Authorization Code)")
            {
                    string patiendId = null;
                    List<Sj.Mg.CliLib.Model.UserClientsList> gg = Sj.Mg.Mongo.MongoManage.SearchUserClients(para.email, para.provider);
                    if (gg != null && gg.Count > 0 && gg[0].UserClientsData.Count > 0)
                    {
                        gg[0].UserClientsData.ForEach(clientType =>
                        {
                            if (clientType != null && clientType.Clients != null)
                            {
                                clientType.Clients.ForEach(clients =>
                                {
                                    if (clients != null && clients.clientName == "Athena")
                                    {
                                        if (clients.PatientId != null)
                                        {
                                            patiendId = clients.PatientId;
                                        }
                                    }
                                });
                            }
                        });
                    }
                    if (patiendId != null || patiendId != "")
                    {
                        //var pats = Execute(CliLib.Utils.Common.ReAhApiPatient + HttpUtility.UrlEncode(para.email).Replace(".", "^2E"), basetkn);
                        var pats = Execute(CliLib.Utils.Common.ReAhApiPatient + patiendId, basetkn);
                        return Json(pats, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(patiendId, JsonRequestBehavior.AllowGet);
                    }
            }
            else if (para.client == "FITBIT")
            {
                var pats = Execute(CliLib.Utils.Common.ReFbApiPatient + HttpUtility.UrlEncode(para.email).Replace(".", "^2E") + "/" + para.provider, basetkn);
                return Json(pats, JsonRequestBehavior.AllowGet);
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
        public ActionResult GetUserClientsData(string email, string provider)
        {
            List<Sj.Mg.CliLib.Model.UserClientsList> gg = Sj.Mg.Mongo.MongoManage.SearchUserClients(email, provider);
            return Json(gg, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        public ActionResult UpdateUserClientsData(UpdateUsersClientData clientData)
        {
            var data = clientData.userClientsList;
            List<Sj.Mg.CliLib.Model.UserClientsList> gg = Sj.Mg.Mongo.MongoManage.SearchUserClients(data.email ,data.provider);
            var temp1 = gg[0].UserClientsData;
            gg[0].UserClientsData = data.UserClientsData;
            Sj.Mg.Mongo.MongoManage.UpdateUserClients(gg[0]);
            gg[0].UserClientsData = temp1;
            if (clientData.delClient)
            {
                var scopes = new List<string>();
                if(gg != null && gg.Count > 0 && gg[0].UserClientsData.Count > 0)
                {
                    gg[0].UserClientsData.ForEach(clientType =>
                    {
                        if(clientType.Clients != null)
                        {
                            clientType.Clients.ForEach(clients =>
                            {
                                if (clients != null && (clients.clientName == clientData.delItem))
                                {
                                    if(clients.UserScopes != null)
                                    {
                                        clients.UserScopes.ForEach(item =>
                                        {
                                            scopes.Add(item.scopeName);
                                        });
                                    }
                                }
                            });
                        }
                    });
                }
                
                var tt = Sj.Mg.Mongo.MongoManage.GetReqUserPerms(data.email, data.provider);
                var allowedUsers = new List<ReqParamObj>();
                var sharedUsers = new List<ReqParamObj>();

                tt.ForEach(t =>
                {
                    if (t.MyEmail == data.email)
                    {
                        if (t.RequestedUsers.ContainsKey(clientData.delItem))
                        {
                            t.RequestedUsers[clientData.delItem].Clear();
                            Sj.Mg.Mongo.MongoManage.ReplaceReqPerm(t);
                        }
                        if (t.AllowedUsers.ContainsKey(clientData.delItem))
                        {
                            if (scopes.Count > 0)
                            {
                                scopes.ForEach(scopeName =>
                                {
                                    if (t.AllowedUsers[clientData.delItem].ContainsKey(scopeName))
                                    {
                                        if (t.AllowedUsers[clientData.delItem][scopeName].ContainsKey("Read"))
                                        {
                                            t.AllowedUsers[clientData.delItem][scopeName]["Read"].ForEach(userItem =>
                                            {
                                                ReqParamObj temp = new ReqParamObj();
                                                temp.user = userItem.user;
                                                temp.scope = scopeName;
                                                temp.scopeType = "Read";
                                                allowedUsers.Add(temp);
                                            });
                                        }
                                        if (t.AllowedUsers[clientData.delItem][scopeName].ContainsKey("Share"))
                                        {
                                            t.AllowedUsers[clientData.delItem][scopeName]["Share"].ForEach(userItem =>
                                            {
                                                ReqParamObj temp = new ReqParamObj();
                                                temp.user = userItem.user;
                                                temp.scope = scopeName;
                                                temp.scopeType = "Share";
                                                allowedUsers.Add(temp);
                                            });
                                        }
                                    }
                                });
                            }
                            t.AllowedUsers[clientData.delItem].Clear();
                            Sj.Mg.Mongo.MongoManage.ReplaceReqPerm(t);
                            if (allowedUsers.Count > 0)
                            {
                                allowedUsers.ForEach(userData =>
                                {
                                    var customeUser = Sj.Mg.Mongo.MongoManage.GetReqUserPerms(userData.user, userData.provider);
                                    customeUser.ForEach(userItem =>
                                    {
                                        if (userData.scopeType == "Read")
                                        {
                                            if (userItem.MyDetailsSharedWith[clientData.delItem][userData.scope].ContainsKey("Read"))
                                            {
                                                int index = -1;
                                                userItem.MyDetailsSharedWith[clientData.delItem][userData.scope]["Read"].ForEach(scopeItem =>
                                                {
                                                    index++;
                                                    if (scopeItem.user == data.email)
                                                    {
                                                        userItem.MyDetailsSharedWith[clientData.delItem][userData.scope]["Read"].Remove(userItem.MyDetailsSharedWith[clientData.delItem][userData.scope]["Read"][index]);
                                                        Sj.Mg.Mongo.MongoManage.ReplaceReqPerm(userItem);
                                                    }
                                                });
                                            }
                                        }
                                        else
                                        {
                                            if (userItem.MyDetailsSharedWith[clientData.delItem][userData.scope].ContainsKey("Share"))
                                            {
                                                int index = -1;
                                                userItem.MyDetailsSharedWith[clientData.delItem][userData.scope]["Share"].ForEach(scopeItem =>
                                                {
                                                    index++;
                                                    if (scopeItem.user == data.email)
                                                    {
                                                        userItem.MyDetailsSharedWith[clientData.delItem][userData.scope]["Share"].Remove(userItem.MyDetailsSharedWith[clientData.delItem][userData.scope]["Read"][index]);
                                                        Sj.Mg.Mongo.MongoManage.ReplaceReqPerm(userItem);
                                                    }
                                                });
                                            }
                                        }

                                    });
                                });
                            }
                        }
                        if (t.MyDetailsSharedWith.ContainsKey(clientData.delItem))
                        {
                            if (scopes.Count > 0)
                            {
                                scopes.ForEach(scopeName =>
                                {
                                    if (t.MyDetailsSharedWith[clientData.delItem].ContainsKey(scopeName))
                                    {
                                        if (t.MyDetailsSharedWith[clientData.delItem][scopeName].ContainsKey("Read"))
                                        {
                                            t.MyDetailsSharedWith[clientData.delItem][scopeName]["Read"].ForEach(userItem =>
                                            {
                                                ReqParamObj temp = new ReqParamObj();
                                                temp.user = userItem.user;
                                                temp.scope = scopeName;
                                                temp.scopeType = "Read";
                                                sharedUsers.Add(temp);
                                            });
                                        }
                                        if (t.MyDetailsSharedWith[clientData.delItem][scopeName].ContainsKey("Share"))
                                        {
                                            t.MyDetailsSharedWith[clientData.delItem][scopeName]["Share"].ForEach(userItem =>
                                            {
                                                ReqParamObj temp = new ReqParamObj();
                                                temp.user = userItem.user;
                                                temp.scope = scopeName;
                                                temp.scopeType = "Share";
                                                sharedUsers.Add(temp);
                                            });
                                        }
                                    }
                                });
                            }
                            t.MyDetailsSharedWith[clientData.delItem].Clear();
                            Sj.Mg.Mongo.MongoManage.ReplaceReqPerm(t);
                            if (sharedUsers.Count > 0)
                            {
                                sharedUsers.ForEach(userData =>
                                {
                                    var customeUser = Sj.Mg.Mongo.MongoManage.GetReqUserPerms(userData.user, userData.provider);
                                    customeUser.ForEach(userItem =>
                                    {
                                        if (userData.scopeType == "Read")
                                        {
                                            if (userItem.AllowedUsers[clientData.delItem][userData.scope].ContainsKey("Read"))
                                            {
                                                int index = -1;
                                                userItem.AllowedUsers[clientData.delItem][userData.scope]["Read"].ForEach(scopeItem =>
                                                {
                                                    index++;
                                                    if (scopeItem.user == data.email)
                                                    {
                                                        userItem.AllowedUsers[clientData.delItem][userData.scope]["Read"].Remove(userItem.AllowedUsers[clientData.delItem][userData.scope]["Read"][index]);
                                                        Sj.Mg.Mongo.MongoManage.ReplaceReqPerm(userItem);
                                                    }
                                                });
                                            }
                                        }
                                        else
                                        {
                                            if (userItem.AllowedUsers[clientData.delItem][userData.scope].ContainsKey("Share"))
                                            {
                                                int index = -1;
                                                userItem.AllowedUsers[clientData.delItem][userData.scope]["Share"].ForEach(scopeItem =>
                                                {
                                                    index++;
                                                    if (scopeItem.user == data.email)
                                                    {
                                                        userItem.AllowedUsers[clientData.delItem][userData.scope]["Share"].Remove(userItem.AllowedUsers[clientData.delItem][userData.scope]["Read"][index]);
                                                        Sj.Mg.Mongo.MongoManage.ReplaceReqPerm(userItem);
                                                    }
                                                });
                                            }
                                        }
                                    });
                                });
                            }
                        }
                    }
                });
            }
            else if (clientData.delScope)
            {
                var tt = Sj.Mg.Mongo.MongoManage.GetReqUserPerms(data.email, data.provider);
                var allowedUsers = new List<ReqParamObj>();
                var sharedUsers = new List<ReqParamObj>();
                var scope = clientData.delItem.Split(',')[1];
                clientData.delItem = clientData.delItem.Split(',')[0];

                tt.ForEach(t =>
                {
                    if (t.MyEmail == data.email)
                    {
                        if (t.RequestedUsers.ContainsKey(clientData.delItem))
                        {
                            if (t.RequestedUsers[clientData.delItem].ContainsKey(scope))
                            {
                                t.RequestedUsers[clientData.delItem][scope].Clear();
                                Sj.Mg.Mongo.MongoManage.ReplaceReqPerm(t);
                            }
                            
                        }
                        if (t.AllowedUsers.ContainsKey(clientData.delItem))
                        {
                            if (t.AllowedUsers[clientData.delItem].ContainsKey(scope))
                            {
                                if (t.AllowedUsers[clientData.delItem][scope].ContainsKey("Read"))
                                {
                                    t.AllowedUsers[clientData.delItem][scope]["Read"].ForEach(userItem =>
                                    {
                                        ReqParamObj temp = new ReqParamObj();
                                        temp.user = userItem.user;
                                        temp.scope = scope;
                                        temp.scopeType = "Read";
                                        allowedUsers.Add(temp);
                                    });
                                }
                                if (t.AllowedUsers[clientData.delItem][scope].ContainsKey("Share"))
                                {
                                    t.AllowedUsers[clientData.delItem][scope]["Share"].ForEach(userItem =>
                                    {
                                        ReqParamObj temp = new ReqParamObj();
                                        temp.user = userItem.user;
                                        temp.scope = scope;
                                        temp.scopeType = "Share";
                                        allowedUsers.Add(temp);
                                    });
                                }
                                t.AllowedUsers[clientData.delItem][scope].Clear();
                                Sj.Mg.Mongo.MongoManage.ReplaceReqPerm(t);
                            }
                            if (allowedUsers.Count > 0)
                            {
                                allowedUsers.ForEach(userData =>
                                {
                                    var customeUser = Sj.Mg.Mongo.MongoManage.GetReqUserPerms(userData.user, userData.provider);
                                    customeUser.ForEach(userItem =>
                                    {
                                        if (userData.scopeType == "Read")
                                        {
                                            if (userItem.MyDetailsSharedWith[clientData.delItem][userData.scope].ContainsKey("Read"))
                                            {
                                                int index = -1;
                                                userItem.MyDetailsSharedWith[clientData.delItem][userData.scope]["Read"].ForEach(scopeItem =>
                                                {
                                                    index++;
                                                    if (scopeItem.user == data.email)
                                                    {
                                                        userItem.MyDetailsSharedWith[clientData.delItem][userData.scope]["Read"].Remove(userItem.MyDetailsSharedWith[clientData.delItem][userData.scope]["Read"][index]);
                                                        Sj.Mg.Mongo.MongoManage.ReplaceReqPerm(userItem);
                                                    }
                                                });
                                            }
                                        }
                                        else
                                        {
                                            if (userItem.MyDetailsSharedWith[clientData.delItem][userData.scope].ContainsKey("Share"))
                                            {
                                                int index = -1;
                                                userItem.MyDetailsSharedWith[clientData.delItem][userData.scope]["Share"].ForEach(scopeItem =>
                                                {
                                                    index++;
                                                    if (scopeItem.user == data.email)
                                                    {
                                                        userItem.MyDetailsSharedWith[clientData.delItem][userData.scope]["Share"].Remove(userItem.MyDetailsSharedWith[clientData.delItem][userData.scope]["Read"][index]);
                                                        Sj.Mg.Mongo.MongoManage.ReplaceReqPerm(userItem);
                                                    }
                                                });
                                            }
                                        }

                                    });
                                });
                            }
                        }
                        if (t.MyDetailsSharedWith.ContainsKey(clientData.delItem))
                        {
                            if (t.MyDetailsSharedWith[clientData.delItem].ContainsKey(scope))
                            {
                                if (t.MyDetailsSharedWith[clientData.delItem][scope].ContainsKey("Read"))
                                {
                                    t.MyDetailsSharedWith[clientData.delItem][scope]["Read"].ForEach(userItem =>
                                    {
                                        ReqParamObj temp = new ReqParamObj();
                                        temp.user = userItem.user;
                                        temp.scope = scope;
                                        temp.scopeType = "Read";
                                        sharedUsers.Add(temp);
                                    });
                                }
                                if (t.MyDetailsSharedWith[clientData.delItem][scope].ContainsKey("Share"))
                                {
                                    t.MyDetailsSharedWith[clientData.delItem][scope]["Share"].ForEach(userItem =>
                                    {
                                        ReqParamObj temp = new ReqParamObj();
                                        temp.user = userItem.user;
                                        temp.scope = scope;
                                        temp.scopeType = "Share";
                                        sharedUsers.Add(temp);
                                    });
                                }
                                t.MyDetailsSharedWith[clientData.delItem][scope].Clear();
                                Sj.Mg.Mongo.MongoManage.ReplaceReqPerm(t);
                            }
                            
                            if (sharedUsers.Count > 0)
                            {
                                sharedUsers.ForEach(userData =>
                                {
                                    var customeUser = Sj.Mg.Mongo.MongoManage.GetReqUserPerms(userData.user, userData.provider);
                                    customeUser.ForEach(userItem =>
                                    {
                                        if (userData.scopeType == "Read")
                                        {
                                            if (userItem.AllowedUsers[clientData.delItem][userData.scope].ContainsKey("Read"))
                                            {
                                                int index = -1;
                                                userItem.AllowedUsers[clientData.delItem][userData.scope]["Read"].ForEach(scopeItem =>
                                                {
                                                    index++;
                                                    if (scopeItem.user == data.email)
                                                    {
                                                        userItem.AllowedUsers[clientData.delItem][userData.scope]["Read"].Remove(userItem.AllowedUsers[clientData.delItem][userData.scope]["Read"][index]);
                                                        Sj.Mg.Mongo.MongoManage.ReplaceReqPerm(userItem);
                                                    }
                                                });
                                            }
                                        }
                                        else
                                        {
                                            if (userItem.AllowedUsers[clientData.delItem][userData.scope].ContainsKey("Share"))
                                            {
                                                int index = -1;
                                                userItem.AllowedUsers[clientData.delItem][userData.scope]["Share"].ForEach(scopeItem =>
                                                {
                                                    index++;
                                                    if (scopeItem.user == data.email)
                                                    {
                                                        userItem.AllowedUsers[clientData.delItem][userData.scope]["Share"].Remove(userItem.AllowedUsers[clientData.delItem][userData.scope]["Read"][index]);
                                                        Sj.Mg.Mongo.MongoManage.ReplaceReqPerm(userItem);
                                                    }
                                                });
                                            }
                                        }
                                    });
                                });
                            }
                        }
                    }
                });
            }

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
            var idp = prof.FindFirst("idp") != null ? prof.FindFirst("idp").Value : "";
            if (idp == "idsrv")
                idp = "MgIdp";
            if (email != "")
            {
                string pattern = @"^[a-z][a-z|0-9|]*([_][a-z|0-9]+)*([.][a-z|0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]*\.([a-z][a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$";
                Match match = Regex.Match(email.Trim(), pattern, RegexOptions.IgnoreCase);

                if (!match.Success)
                {
                    List<Sj.Mg.CliLib.Model.CustomUser> data = Sj.Mg.Mongo.MongoManage.SearchUserByProviderId(email);
                    if (data.Count > 0)
                    {
                        email = data[0].Email;
                    }
                }
            }
            var dd = Sj.Mg.Mongo.MongoManage.SearchUserClients(email, idp); 
            var code = Request.QueryString["code"];
            string url = "https://api.fitbit.com/oauth2/token?clientId=228JJF&grant_type=authorization_code&redirect_uri=https%3A%2F%2Foidc.medgrotto.com%3A9001%2FHome%2FCallback&code=" + code;
            //AWS Url
            //string url = "https://api.fitbit.com/oauth2/token?clientId=228JJD&grant_type=authorization_code&redirect_uri=https%3A%2F%2Faws1.medgrotto.com%3A9001%2FHome%2FCallback&code=" + code;
            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";
            request.Headers["Authorization"] = "Basic MjI4SkpGOjNjMjY4YTllYzFiYTMzNDJkNTEyNzIyMDkzMDc5NGYx";
            //AWS app
            //request.Headers["Authorization"] = "Basic MjI4SkpEOjc0ZjY2MTg3MTkyZmJlNWUwYmRmM2NlZDVhZDBkNTQ4";
            request.ContentType = "application/x-www-form-urlencoded";
            WebResponse response = request.GetResponse();
            Stream receive = response.GetResponseStream();
            StreamReader reader = new StreamReader(receive, System.Text.Encoding.UTF8);
            var tt = reader.ReadToEnd();
            Newtonsoft.Json.Linq.JObject token = Newtonsoft.Json.Linq.JObject.Parse(tt);
            if(dd.Count > 0)
            {
                dd[0].UserClientsData.ForEach(t =>
                {
                    if (t.Clients != null && t.Clients.Count > 0 && t.Clients[0] != null)
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
            }
            
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