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
        public Newtonsoft.Json.Linq.JObject Get()
        {
            string url = "https://api.fitbit.com/1/user/-/profile.json";
            WebRequest request = WebRequest.Create(url);
            request.Method = "GET";
            request.Headers["Authorization"] = "Bearer eyJhbGciOiJIUzI1NiJ9.eyJzdWIiOiI1UTczNzMiLCJhdWQiOiIyMjhKSkYiLCJpc3MiOiJGaXRiaXQiLCJ0eXAiOiJhY2Nlc3NfdG9rZW4iLCJzY29wZXMiOiJyc29jIHJzZXQgcmFjdCBybG9jIHJ3ZWkgcmhyIHJwcm8gcm51dCByc2xlIiwiZXhwIjoxNDk1NjczMzM4LCJpYXQiOjE0OTU2NDQ1Mzh9.sT7wMh6BHsCspRLj7KfQXVyX0gLxH6rs8q4nPTGaqWw";
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
