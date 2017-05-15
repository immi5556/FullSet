using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Test.Mongo.athena
{
    class Setup
    {
        static AthenaSetup _athna = null;
        public static AthenaSetup Init()
        {
            return Init(false);
        }
        public static AthenaSetup Init(bool forcerefresh)
        {
            if (_athna == null || forcerefresh)
            {
                _athna = new AthenaSetup();


                Dictionary<string, string> auth_prefixes = new Dictionary<string, string>()
      {
        {"v1", "/oauth"},
        {"preview1", "/oauthpreview"},
        {"openpreview1", "/oauthopenpreview"},
      };


                // Basic access authentication
                Dictionary<string, string> parameters = new Dictionary<string, string>()
                  {
                    {"grant_type", "client_credentials"},
                  };

                // Create and set up a request
                WebRequest request = WebRequest.Create(Utils.PathJoin(_athna.baseurl, auth_prefixes[_athna.version], "/token"));
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";

                // Make sure to add the Authorization header
                string auth = System.Convert.ToBase64String(_athna.UTF8.GetBytes(_athna.key + ":" + _athna.secret));
                request.Headers["Authorization"] = "Basic " + auth;

                // Encode the parameters, convert it to bytes (because that's how the streams want it)
                string encoded = Utils.UrlEncode(parameters);
                byte[] content = _athna.UTF8.GetBytes(encoded);

                // Write the parameters to the body
                Stream writer = request.GetRequestStream();
                writer.Write(content, 0, content.Length);
                writer.Close();

                // Get the response, read it out, and decode it
                WebResponse response = request.GetResponse();
                Stream receive = response.GetResponseStream();
                StreamReader reader = new StreamReader(receive, _athna.UTF8);
                //JsonValue authorization = JsonValue.Parse(reader.ReadToEnd());
                //Object authorization = Newtonsoft.Json.JsonConvert.DeserializeObject(reader.ReadToEnd());
                Newtonsoft.Json.Linq.JObject authorization = Newtonsoft.Json.Linq.JObject.Parse(reader.ReadToEnd());
                // Make sure to grab the token!
                string token = authorization["access_token"].ToString();
                Console.WriteLine(token);
                _athna.token = token;
            }
            return _athna;
        }
    }
}
