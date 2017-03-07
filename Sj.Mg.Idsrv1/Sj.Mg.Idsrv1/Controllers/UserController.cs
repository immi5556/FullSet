using IdentityServer3.Core;
using IdentityServer3.Core.Extensions;
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

            if (ModelState.IsValid)
            {
                // update the "database" for our users with the registration data
                var subject = partial_user.GetSubjectId();
                var db_user = Custom.MgUserService.Users.Single(x => x.Subject == subject);
                db_user.Claims.Add(new Claim(Constants.ClaimTypes.GivenName, model.First));
                db_user.Claims.Add(new Claim(Constants.ClaimTypes.FamilyName, model.Last));

                // replace the name captured from the external identity provider
                var nameClaim = db_user.Claims.Single(x => x.Type == Constants.ClaimTypes.Name);
                db_user.Claims.Remove(nameClaim);
                nameClaim = new Claim(Constants.ClaimTypes.Name, model.First + " " + model.Last);
                db_user.Claims.Add(nameClaim);

                // mark user as registered
                db_user.IsRegistered = true;

                // this replaces the name issued in the partial signin cookie
                // the reason for doing is so when we redriect back to IdSvr it will
                // use the updated name for display purposes. this is only needed if
                // the registration process needs to use a different name than the one
                // we captured from the external provider
                var partialClaims = partial_user.Claims.Where(x => x.Type != Constants.ClaimTypes.Name).ToList();
                partialClaims.Add(nameClaim);
                await ctx.Environment.UpdatePartialLoginClaimsAsync(partialClaims);

                // find the URL to continue with the process to the issue the token to the RP
                var resumeUrl = await ctx.Environment.GetPartialLoginResumeUrlAsync();
                return Redirect(resumeUrl);
            }

            return View();
        }
    }
}