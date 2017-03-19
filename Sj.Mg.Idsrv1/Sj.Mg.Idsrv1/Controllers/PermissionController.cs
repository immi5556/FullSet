using IdentityServer3.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Sj.Mg.Idsrv1.Controllers
{
    [IdentityServer3.Extensions.Mvc.Filters.IdentityServerFullLoginAttribute]
    public class PermissionController : Controller
    {
        [Route("core/scopes")]
        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
    }
}