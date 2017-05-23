using System.Text;

namespace Sj.Ah.Resource.Server.athena
{
    public class AthenaSetup
    {
        public AthenaSetup()
        {
            key = "cezsucv9zy2k3uu464qmkf4z";
            secret = "bHZfSdakTnCNmVH";
            version = "preview1";
            baseurl = "https://api.athenahealth.com/";
            UTF8 = System.Text.Encoding.GetEncoding("utf-8");
        }

        public string key { get; set; }
        public string secret { get; set; }
        public string version { get; set; }
        public string baseurl { get; set; }
        public string practiceid { get; set; }
        public string depatmentid { get; set; }
        public string patientid { get; set; }
        public string token { get; set; }
        public Encoding UTF8 { get; set; }
    }
}