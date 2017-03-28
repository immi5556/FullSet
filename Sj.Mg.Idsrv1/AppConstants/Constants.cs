using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppConstants
{
    public class Constants1
    {
        /// <summary>
        /// Localhost
        /// </summary>
        public const string ReApi = "https://localhost:44306/";
        public const string ReApiStsCallback = "https://localhost:44306/home/stscallback";
        public const string ReClientMvc = "https://localhost:44383/";
        public const string ReClientMvcStsCallback = "https://localhost:44383/home/stscallback";
        public const string IssuerUri = "https://oidc.medgrotto.com/identity";
        public const string StsOrigin = "https://localhost:44305";


        public const string ClientSecret = "myrandomclientsecret";

        public const string Sts = StsOrigin + "/core";
        public const string StsTokenEndpoint = Sts + "/connect/token";

        public const string StsAuthorizationEndpoint = Sts + "/connect/authorize";
        public const string StsIntrospectionEndPoint = StsOrigin + "connect/introspect";

        // UMA
        public const string UmaProtectionPermEndPoint = StsOrigin + "/Protection/PremissionTicket";
        public const string UmaProtectionRptEndPoint = StsOrigin + "/Protection/RptToken";
        public const string UmaResourceSetEndPoint = StsOrigin + "/Resource/ResourceGet/{id}";
        public const string UmaDynClientEndPoint = StsOrigin + "/Uma/ClientRegister";
        public const string UmaRptEndPoint = StsOrigin + "/Protection/RptToken";

        public const string UmaDiscoveryConfiguration = "core/.well-known/uma-configuration";
    }
}
