using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace Sj.Fb.Resource.Server.Controllers
{
    public class PatientController : ApiController
    {
        [Mg.CliLib.Security.UmaAuthz("Patient/Patient.Read", "Patient/Patient.*")]
        [Route("api/Patient/{id}/{idp}")]
        public Newtonsoft.Json.Linq.JObject Get(string id, string idp)
        {
            string email = (id ?? "").Replace("^2E", ".");
            var dd = Sj.Mg.Mongo.MongoManage.SearchUserClients(email, idp);
            string acctkn = "";
            dd[0].UserClientsData.ForEach(t =>
            {
                if (t.Clients != null && t.Clients.Count > 0 && t.Clients[0] != null)
                {
                    t.Clients.ForEach(t1 =>
                    {
                        if (t1.clientName == "FITBIT")
                        {
                            acctkn = t1.AccessToken;
                        }
                    });
                }
            });
            if (string.IsNullOrEmpty(acctkn))
            {
                throw new UnauthorizedAccessException("Access not provided");
            }
            Newtonsoft.Json.Linq.JObject job = Newtonsoft.Json.Linq.JObject.Parse(acctkn);
            var tkn = job.SelectToken("access_token", false).ToString();
            if (job.SelectToken("access_token", false) == null)
            {
                throw new UnauthorizedAccessException("Unauth exception..");
            }
            string url = "https://api.fitbit.com/1/user/-/profile.json";
            WebRequest request = WebRequest.Create(url);
            request.Method = "GET";
            request.Headers["Authorization"] = "Bearer " + tkn;
            request.ContentType = "application/x-www-form-urlencoded";
            WebResponse response = request.GetResponse();
            Stream receive = response.GetResponseStream();
            StreamReader reader = new StreamReader(receive, Encoding.UTF8);
            var tt = reader.ReadToEnd();
            Newtonsoft.Json.Linq.JObject patient = Newtonsoft.Json.Linq.JObject.Parse(tt);
            return patient;
        }
    }
}
