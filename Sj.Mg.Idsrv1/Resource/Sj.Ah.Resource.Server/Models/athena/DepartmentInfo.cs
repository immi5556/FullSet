using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Sj.Ah.Resource.Server.athena
{
    class DepartmentInfo
    {
        public static void SetDepartment(AthenaSetup setup)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
              {
                {"limit", "1"},
                { "offset", "0"},
                { "providerlist", "false" },
                { "showalldepartments", "true" }
              };
            string url = setup.baseurl + "preview1/" + setup.practiceid + "/departments?limit=100&offset=0";
            WebRequest request = WebRequest.Create(url);
            request.Method = "GET";
            request.Headers["Authorization"] = "Bearer " + setup.token;

            // Get the response, read and decode
            WebResponse response = request.GetResponse();
            Stream receive = response.GetResponseStream();
            StreamReader reader = new StreamReader(receive, setup.UTF8);
            Newtonsoft.Json.Linq.JObject departments = Newtonsoft.Json.Linq.JObject.Parse(reader.ReadToEnd());
            //File.WriteAllText(@"D:\Immi\Projects\git_projects\mg\Sj.Mg.Idsrv1\Test.Mongo\Data\dept_1.json", departments.ToString());
            int rnd = Utils.Random(departments["totalcount"].ToString());
            string dept = departments["departments"][rnd]["departmentid"].ToString();
            Console.WriteLine(dept);
            setup.depatmentid = dept;
            response.Close();
        }
    }
}
