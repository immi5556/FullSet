using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Test.Mongo.athena
{
    class PatientInfo
    {
        public static void CreatePatient(AthenaSetup setup)
        {
            CreatePatient(setup, null, null, null, null, null, null, null, null, null, null, null);
        }

        public static void CreatePatient(AthenaSetup setup, string add1, string add2, string city, string dob, string email,
            string fn, string ln, string hp, string mp, string state, string zip)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
              {
                {"limit", "1"},
              };
            string url = setup.baseurl + "preview1/" + setup.practiceid + "/patients";
            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";
            request.Headers["Authorization"] = "Bearer " + setup.token;
            request.ContentType = "application/x-www-form-urlencoded";

            var postData = "address1=" + WebUtility.UrlEncode(add1 ?? ("mgAdd1" + Utils.RandomString(3)));
            postData += "&address2=" + WebUtility.UrlEncode(add2 ?? ("mgAdd2" + Utils.RandomString(3)));
            postData += "&city=" + WebUtility.UrlEncode(city ?? ("mgCity" + Utils.RandomString(3)));
            postData += "&departmentid=" + setup.depatmentid;
            postData += "&dob=" + WebUtility.UrlEncode(dob ?? string.Format("{0}/{1}/{2}", Utils.Random(10, 12), Utils.Random(10, 12), Utils.Random(1900, 2000))); //Patient's DOB (mm/dd/yyyy)
            postData += "&email=" + WebUtility.UrlEncode(email ?? ("mgemail" + Utils.RandomString(3) + "@medgrotto.com"));
            postData += "&firstname=" + WebUtility.UrlEncode(fn ?? ("mgfn" + Utils.RandomString(3))); // Mandatory
            postData += "&homephone=" + WebUtility.UrlEncode((hp ?? "484 484 4848"));
            postData += "&lastname=" + WebUtility.UrlEncode(ln ?? "ZZTEST"); //Mandatory
            postData += "&mobilephone=" + WebUtility.UrlEncode(mp ?? "484 484 4848"); // 000 000 0000
            postData += "&state=" + WebUtility.UrlEncode(state ?? "TX"); //Patient's state (2 letter abbreviation)
            postData += "&zip=" + WebUtility.UrlEncode(zip ?? "75023"); //Patient's zip. Matching occurs on first 5 characters

            //var postData = @"address1=Add1&address2=Add2&city=Dallas&departmentid=1&dob=01%2F01%2F1960&email=bob%40bob.co&firstname=Bob&homephone=484+484+4848&lastname=Smith&mobilephone=484+484+4848&state=TX";

            var data = Encoding.ASCII.GetBytes(postData);

            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            // Get the response, read and decode
            WebResponse response = request.GetResponse();
            Stream receive = response.GetResponseStream();
            StreamReader reader = new StreamReader(receive, setup.UTF8);
            var str = reader.ReadToEnd();
            Newtonsoft.Json.Linq.JArray patient = Newtonsoft.Json.Linq.JArray.Parse(str);
            //File.WriteAllText(@"D:\Immi\Projects\git_projects\mg\Sj.Mg.Idsrv1\Test.Mongo\Data\pat_1.json", patient.ToString());
            string patid = patient[0]["patientid"].ToString();
            Console.WriteLine(patid.ToString());
            setup.patientid = patid;
            response.Close();
        }

        public static void GetPatient(AthenaSetup setup)
        {
            string url = setup.baseurl + "preview1/" + setup.practiceid + "/patients/" + setup.patientid;
            WebRequest request = WebRequest.Create(url);
            request.Method = "GET";
            request.Headers["Authorization"] = "Bearer " + setup.token;

            // Get the response, read and decode
            WebResponse response = request.GetResponse();
            Stream receive = response.GetResponseStream();
            StreamReader reader = new StreamReader(receive, setup.UTF8);
            Newtonsoft.Json.Linq.JArray patient = Newtonsoft.Json.Linq.JArray.Parse(reader.ReadToEnd());
            //File.WriteAllText(@"D:\Immi\Projects\git_projects\mg\Sj.Mg.Idsrv1\Test.Mongo\Data\pat_2.json", patient.ToString());
            string patid = patient[0]["patientid"].ToString();
            Console.WriteLine(patient);
            response.Close();
        }
    }
}
