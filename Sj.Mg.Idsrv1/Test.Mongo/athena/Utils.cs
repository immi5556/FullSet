using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Test.Mongo.athena
{
    class Utils
    {
        static public string UrlEncode(Dictionary<string, string> dict)
        {
            return string.Join("&", dict.Select(
              kvp => WebUtility.UrlEncode(kvp.Key) + "=" + WebUtility.UrlEncode(kvp.Value)
            ).ToList());
        }

        static public string PathJoin(params string[] args)
        {
            return string.Join("/", args
                               .Select(arg => arg.Trim(new char[] { '/' }))
                               .Where(arg => !String.IsNullOrEmpty(arg))
            );
        }
        static public int Random(string val)
        {
            int vv = 0;
            if (int.TryParse(val, out vv))
                return Random(vv);
            return 0;
        }
        static public int Random(int max)
        {
            return Random(0, max);
        }
        static public int Random(int min, int max)
        {
            return new Random().Next(min, max);
        }
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        static public void Main()
        {
            var tt = Setup.Init();
            PractieInfo.SetPractice(tt);
            DepartmentInfo.SetDepartment(tt);
            //PatientInfo.CreatePatient(tt);
            tt.patientid = "29493";
            PatientInfo.GetPatient(tt);
            AppointmentInfo.AddAppointment(tt);
            Console.ReadKey();
        }
    }
}
