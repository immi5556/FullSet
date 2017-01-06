using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppConstants
{
    public class Constants
    {
        //public const string ReApi = "https://localhost:44325/";
        //public const string ReApiStsCallback = "https://localhost:44325/home/stscallback";

        //public const string ReClientMvc = "https://localhost:44359/";
        //public const string ReClientMvcStsCallback = "https://localhost:44359/home/stscallback";

        //public const string ClientSecret = "myrandomclientsecret";

        //public const string IssuerUri = "https://oidc.medgrotto.com/identity";
        //public const string StsOrigin = "https://localhost:44398";

        public const string ReApi = "https://oidc.medgrotto.com:9002/";
        public const string ReApiStsCallback = "https://oidc.medgrotto.com:9002/home/stscallback";
        public const string ReClientMvc = "https://oidc.medgrotto.com:9001/";
        public const string ReClientMvcStsCallback = "https://oidc.medgrotto.com:9001/home/stscallback";

        public const string ClientSecret = "myrandomclientsecret";

        public const string IssuerUri = "https://oidc.medgrotto.com/identity";
        public const string StsOrigin = "https://oidc.medgrotto.com:9011";

        public const string Sts = StsOrigin + "/identity";
        public const string StsTokenEndpoint = Sts + "/connect/token";
        public const string StsAuthorizationEndpoint = Sts + "/connect/authorize";
        public const string StsUserInfoEndpoint = Sts + "/connect/userinfo";

        public const string UmaProtectionPermEndPoint = StsOrigin + "/Protection/PremissionTicket";
        public const string UmaProtectionRptEndPoint = StsOrigin + "/Protection/RptToken";
        public const string UmaResourceSetEndPoint = StsOrigin + "/Resource/ResourceGet/{id}";
    }

    public class PostData
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
