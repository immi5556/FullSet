using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Sj.Mg.Resource.Server.Controllers
{
    [EnableCors("*", "*", "GET, POST, PATCH")]
    public class AccountController : ApiController
    {
        // GET: api/Account/5
        [Code.Security.MgAuthz(new string[] { "Patient/Account.Read", "Patient/Account.*" })]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Account
        [Code.Security.MgAuthz(new string[] { "Patient/Account.*" })]
        public void Post([FromBody]string value)
        {
        }

        // DELETE: api/Account/5
        public void Delete(int id)
        {
        }
    }
}
