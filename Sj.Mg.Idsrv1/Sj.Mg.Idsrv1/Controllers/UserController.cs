using IdentityServer3.Core;
using IdentityServer3.Core.Extensions;
using Sj.Mg.CliLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Sj.Mg.Idsrv1.Controllers
{
    public class UserController : Controller
    {
        [Route("core/externalregistration")]
        [HttpGet]
        public async Task<ActionResult> ExternalRegistration()
        {
            var ctx = Request.GetOwinContext();
            var partial_user = await ctx.Environment.GetIdentityServerPartialLoginAsync();
            if (partial_user == null)
            {
                return View("Error");
            }
            return View();
        }

        [Route("core/externalregistration")]
        [HttpPost]
        public async Task<ActionResult> Index(Models.ExternalRegistrationModel model)
        {
            var ctx = Request.GetOwinContext();
            var partial_user = await ctx.Environment.GetIdentityServerPartialLoginAsync();
            if (partial_user == null)
            {
                return View("Error");
            }
            int count = 0;
            if (model.Ans1 != null)
                count++;
            if (model.Ans2 != null)
                count++;
            if (model.Ans3 != null)
                count++;
            if (model.Ans4 != null)
                count++;
            if (model.Ans5 != null)
                count++;
            if (model.Ans6 != null)
                count++;
            if (model.Ans7 != null)
                count++;

            if (ModelState.IsValid && count > 2)
            {
                // update the "database" for our users with the registration data
                var subject = partial_user.GetSubjectId();
                string idp = "";
                var tte = partial_user.Claims.ToList().Find(t => t.Type == "idp");
                if (tte !=  null)
                {
                    idp = tte.Value;
                }
                var dict = new Dictionary<string, object>();
                dict.Add("Subject", subject);
                //if (!string.IsNullOrEmpty(idp))
                    //dict.Add("Provider", idp);
                var db_user = Sj.Mg.Mongo.MongoManage.Select<Sj.Mg.CliLib.Model.CustomUser>(dict, "Users").FirstOrDefault();// Custom.MgUserService.Users.Single(x => x.Subject == subject);
                db_user.Claims.Add(new Claim(Constants.ClaimTypes.GivenName, model.First));
                db_user.Claims.Add(new Claim(Constants.ClaimTypes.FamilyName, model.Last));
                db_user.Password = model.Password;
                db_user.Username = model.Email;
                db_user.Questions = new List<Question>()
                {
                    new Question() {
                        Ques = Constants.Questions.Question1,
                        Ans = model.Ans1
                    },
                    new Question() {
                        Ques = Constants.Questions.Question2,
                        Ans = model.Ans2
                    },
                    new Question() {
                        Ques = Constants.Questions.Question3,
                        Ans = model.Ans3
                    },
                    new Question() {
                        Ques = Constants.Questions.Question4,
                        Ans = model.Ans4
                    },
                    new Question() {
                        Ques = Constants.Questions.Question5,
                        Ans = model.Ans5
                    },
                    new Question() {
                        Ques = Constants.Questions.Question6,
                        Ans = model.Ans6
                    },
                    new Question() {
                        Ques = Constants.Questions.Question7,
                        Ans = model.Ans7
                    },
                };
                // replace the name captured from the external identity provider
                var nameClaim = db_user.Claims.Single(x => x.Type == Constants.ClaimTypes.Name);
                db_user.Claims.Remove(nameClaim);
                nameClaim = new Claim(Constants.ClaimTypes.Name, model.First + " " + model.Last);
                db_user.Claims.Add(nameClaim);

                // mark user as registered
                db_user.IsRegistered = true;
                //db_user.Claims.ForEach(t =>
                //{
                //    db_user.CustomClaims.Add(new CliLib.Model.CustomClaim(t.Type, t.Value));
                //});
                db_user.CustomClaims.Add(new CliLib.Model.CustomClaim(Constants.ClaimTypes.GivenName, model.First));
                db_user.CustomClaims.Add(new CliLib.Model.CustomClaim(Constants.ClaimTypes.FamilyName, model.Last));
                db_user.CustomClaims.Add(new CliLib.Model.CustomClaim(Constants.ClaimTypes.Name, subject));
                // this replaces the name issued in the partial signin cookie
                // the reason for doing is so when we redriect back to IdSvr it will
                // use the updated name for display purposes. this is only needed if
                // the registration process needs to use a different name than the one
                // we captured from the external provider
                var partialClaims = partial_user.Claims.Where(x => x.Type != Constants.ClaimTypes.Name).ToList();
                partialClaims.Add(nameClaim);
                await ctx.Environment.UpdatePartialLoginClaimsAsync(partialClaims);
                Sj.Mg.Mongo.MongoManage.ReplaceUser(db_user);
                db_user.Subject = db_user.Username;
                Sj.Mg.Mongo.MongoManage.ReplaceUserByUserName(db_user);
                // find the URL to continue with the process to the issue the token to the RP
                var resumeUrl = await ctx.Environment.GetPartialLoginResumeUrlAsync();
                return Redirect(resumeUrl);
            }

            return View();
        }

        [Authorize]
        [Route("core/testauth")]
        [HttpGet]
        public async Task<ActionResult> Test()
        {
            return View("Eula");
        }

        [Route("core/eula")]
        [HttpGet]
        public async Task<ActionResult> Eula()
        {
            var ctx = Request.GetOwinContext();
            var partial_user = await ctx.Environment.GetIdentityServerPartialLoginAsync();
            if (partial_user == null)
            {
                return View("Error");
            }
            return View();
        }

        [Route("core/eula")]
        [HttpPost]
        public async Task<ActionResult> Eula(string button)
        {
            var ctx = Request.GetOwinContext();
            var partial_user = await ctx.Environment.GetIdentityServerPartialLoginAsync();
            if (partial_user == null)
            {
                return View("Error");
            }

            if (button == "yes")
            {
                // update the "database" for our users with the outcome
                var subject = partial_user.GetSubjectId();
                Dictionary<string, object> filter = new Dictionary<string, object>();
                filter.Add("Username", subject);
                var tt = Sj.Mg.Mongo.MongoManage.Select<Sj.Mg.CliLib.Model.CustomUser>(filter, "Users");
                var user = (tt == null || tt.Count == 0) ? null : tt[0];
                //var user = Users.SingleOrDefault(x => x.Username == context.UserName && x.Password == context.Password);
                if (user != null)
                {
                    user.AcceptedEula = true;
                }
                //var user = EulaAtLoginUserService.Users.Single(x => x.Subject == subject);
                //user.AcceptedEula = true;

                // find the URL to continue with the process to the issue the token to the RP
                var resumeUrl = await ctx.Environment.GetPartialLoginResumeUrlAsync();
                return Redirect(resumeUrl);
            }

            ViewBag.Message = "Well, until you accept you can't continue.";
            return View();
        }

        [Route("core/loginchallenge")]
        [HttpGet]
        public async Task<ActionResult> LoginChallenge()
        {
            var ctx = Request.GetOwinContext();
            var partial_user = await ctx.Environment.GetIdentityServerPartialLoginAsync();
            if (partial_user == null)
            {
                return View("Error");
            }
            ViewBag.user = partial_user.Name;
            return View();
        }

        [Route("core/loginchallenge")]
        [HttpPost]
        public async Task<ActionResult> LoginChallenge(string button)
        {
            var ctx = Request.GetOwinContext();
            var partial_user = await ctx.Environment.GetIdentityServerPartialLoginAsync();
            if (partial_user == null)
            {
                return View("Error");
            }

            if (button == "Submit")
            {
                // update the "database" for our users with the outcome
                var subject = partial_user.GetSubjectId();
                Dictionary<string, object> filter = new Dictionary<string, object>();
                filter.Add("Username", subject);
                var tt = Sj.Mg.Mongo.MongoManage.Select<Sj.Mg.CliLib.Model.CustomUser>(filter, "Users");
                var user = (tt == null || tt.Count == 0) ? null : tt[0];
                //var user = Users.SingleOrDefault(x => x.Username == context.UserName && x.Password == context.Password);
                var resumeUrl = await ctx.Environment.GetPartialLoginResumeUrlAsync();
                if (user != null)
                {
                    if (user.AcceptedEula)
                    {
                        resumeUrl = await ctx.Environment.GetPartialLoginResumeUrlAsync();
                        return Redirect(resumeUrl);
                    }
                    else
                    {
                        //resumeUrl = await ctx.Environment.GetPartialLoginResumeUrlAsync();
                        return Redirect("/core/eula");
                    }
                }
                return Redirect(resumeUrl);
            }

            ViewBag.Message = "Well, until you accept you can't continue.";
            return View();
        }        
    }
}