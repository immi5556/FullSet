using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppConstants
{
    public class Constants
    {
        /// <summary>
        /// Localhost
        /// </summary>
        //public const string ReApi = "https://localhost:44325/";
        //public const string ReApiStsCallback = "https://localhost:44325/home/stscallback";
        //public const string ReClientMvc = "https://localhost:44359/";
        //public const string ReClientMvcStsCallback = "https://localhost:44359/home/stscallback";
        //public const string IssuerUri = "https://oidc.medgrotto.com/identity";
        //public const string StsOrigin = "https://localhost:44398";

        /// <summary>
        /// /Https
        /// </summary>
        //public const string ReApi = "https://oidc.medgrotto.com:9002/";
        //public const string ReApiStsCallback = "https://oidc.medgrotto.com:9002/home/stscallback";
        //public const string ReClientMvc = "https://oidc.medgrotto.com:9001/";
        //public const string ReClientMvcStsCallback = "https://oidc.medgrotto.com:9001/home/stscallback";
        //public const string IssuerUri = "https://oidc.medgrotto.com:9011/identity";
        //public const string StsOrigin = "https://oidc.medgrotto.com:9011";

        /// <summary>
        /// /Http
        /// </summary>

        public const string ReApi = "http://oidc.medgrotto.com:9022/";
        public const string ReApiStsCallback = "http://oidc.medgrotto.com:9022/home/stscallback";
        public const string ReClientMvc = "http://oidc.medgrotto.com:9021/";
        public const string ReClientMvcStsCallback = "http://oidc.medgrotto.com:9021/home/stscallback";
        public const string IssuerUri = "http://oidc.medgrotto.com:9033/identity";
        public const string StsOrigin = "http://oidc.medgrotto.com:9033";


        public const string ClientSecret = "myrandomclientsecret";

        public const string Sts = StsOrigin + "/identity";
        public const string StsTokenEndpoint = Sts + "/connect/token";
        public const string StsAuthorizationEndpoint = Sts + "/connect/authorize";
        public const string StsUserInfoEndpoint = Sts + "/connect/userinfo";

        public const string UmaProtectionPermEndPoint = StsOrigin + "/Protection/PremissionTicket";
        public const string UmaProtectionRptEndPoint = StsOrigin + "/Protection/RptToken";
        public const string UmaResourceSetEndPoint = StsOrigin + "/Resource/ResourceGet/{id}";
        public const string UmaDynClientEndPoint = StsOrigin + "/Uma/ClientRegister";
        public const string UmaIntrospectionEndPoint = StsOrigin + "TBD";

        public const string UmaDiscoveryConfiguration = "identity/.well-known/uma-configuration";
    }

    public class PostData
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
