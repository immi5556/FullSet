using IdentityServer3.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Sj.Mg.Idsrv1.Controllers
{
    public class PermissionController : Controller
    {
        [Route("core/scopes")]
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Index()
        {
            var ctx = Request.GetOwinContext();
            var partial_user = await ctx.Environment.GetIdentityServerPartialLoginAsync();
            if (partial_user == null)
            {
                return View("Error");
            }
            return View();
        }
    }
}