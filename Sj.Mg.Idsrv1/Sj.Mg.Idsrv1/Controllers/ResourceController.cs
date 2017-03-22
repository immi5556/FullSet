using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sj.Mg.Idsrv1.Controllers
{
    [IdentityServer3.Extensions.Mvc.Filters.IdentityServerFullLoginAttribute]
    public class ResourceController : Controller
    {
        [Route("core/resources")]
        [HttpPost]
        [Authorize]
        public JsonResult Add()
        {
            return Json(new { });
        }
    }
}