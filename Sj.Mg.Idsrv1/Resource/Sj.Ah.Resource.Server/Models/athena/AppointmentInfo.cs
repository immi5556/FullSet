using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Sj.Ah.Resource.Server.athena
{
    class AppointmentInfo
    {
        public static void AddAppointment(AthenaSetup setup)
        {
            AddAppointment(setup, null, null, null, null, null);
        }

        public static void AddAppointment(AthenaSetup setup, string appxdate, string patientid, string deptid, 
            string note, string patientinstructions)
        {
            string url = setup.baseurl + "preview1/" + setup.practiceid + "/appointments/appointmentreminders";
            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";
            request.Headers["Authorization"] = "Bearer " + setup.token;
            request.ContentType = "application/x-www-form-urlencoded";

            var postData = "&patientid=" + WebUtility.UrlEncode(setup.patientid ?? ("mgCity" + Utils.RandomString(3)));
            postData += "&departmentid=" + setup.depatmentid;
            postData += "&patientinstructions=" + WebUtility.UrlEncode(patientinstructions ?? "Reminder for patients " + setup.patientid);
            postData += "&approximatedate=" + WebUtility.UrlEncode(appxdate ?? string.Format("{0}/{1}/{2}", Utils.Random(10, 12), Utils.Random(10, 12), Utils.Random(2017, 2018))); //Patient's DOB (mm/dd/yyyy)

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
            Newtonsoft.Json.Linq.JObject reminder = Newtonsoft.Json.Linq.JObject.Parse(str);
            //File.WriteAllText(@"D:\Immi\Projects\git_projects\mg\Sj.Mg.Idsrv1\Test.Mongo\Data\appoint_1.json", reminder.ToString());
            string reminderid = reminder["appointmentreminderid"].ToString();
            Console.WriteLine(reminder.ToString());
            response.Close();
        }
    }
}
