using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Test.Mongo.athena
{
    class PractieInfo
    {
        public static void SetPractice(AthenaSetup setup)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
              {
                {"limit", "1"},
              };
            string url = setup.baseurl + "preview1/1/practiceinfo?limit=100&offset=0";
            WebRequest request = WebRequest.Create(url);
            request.Method = "GET";
            request.Headers["Authorization"] = "Bearer " + setup.token;

            // Get the response, read and decode
            WebResponse response = request.GetResponse();
            Stream receive = response.GetResponseStream();
            StreamReader reader = new StreamReader(receive, setup.UTF8);
            Newtonsoft.Json.Linq.JObject practice = Newtonsoft.Json.Linq.JObject.Parse(reader.ReadToEnd());
            //File.WriteAllText(@"D:\Immi\Projects\git_projects\mg\Sj.Mg.Idsrv1\Test.Mongo\Data\pract_1.json", practice.ToString());
            string practicid = practice["practiceinfo"][0]["practiceid"].ToString();
            setup.practiceid = practicid;
            Console.WriteLine(practicid.ToString());
            response.Close();
        }
    }
}
