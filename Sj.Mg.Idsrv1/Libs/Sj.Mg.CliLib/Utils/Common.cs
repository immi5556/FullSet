using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sj.Mg.CliLib.Utils
{
    public class Common
    {
        public const string ReApi = "https://localhost:44306/";
        public const string ReApiStsCallback = "https://localhost:44306/home/stscallback";
        public const string ReClientMvc = "https://localhost:44383/";
        public const string ReClientMvcStsCallback = "https://localhost:44383/home/stscallback";
        public const string IssuerUri = "https://oidc.medgrotto.com/identity";
        public const string StsOrigin = "https://localhost:44305";


        public const string ReApiAccount = ReApi + "Api/Account/";
        public const string ReApiObservation = ReApi + "Api/Observation/";
        public const string ReApiPatient = ReApi + "Api/Patient/";
        public const string ReApiMedication = ReApi + "Api/Medication/";

        public const string ClientSecret = "myrandomclientsecret";

        public const string StsIntrospectionEndPoint = Sts + "/connect/introspect";
        public const string StsAuthorizationEndpoint = Sts + "/connect/authorize";

        public const string Sts = StsOrigin + "/core";
        public const string StsTokenEndpoint = StsOrigin + "/connect/token";
        public const string StsPermTktEndpoint = StsOrigin + "/Service/PermTkt";
        public const string StsPermTktValidEndpoint = StsOrigin + "/Service/ValidatePermTkt";
        public const string StsRptTknEndpoint = StsOrigin + "/Service/RptToken";

        public const string UmaProtectionPermEndPoint = StsOrigin + "/Protection/PremissionTicket";
        public const string UmaProtectionRptEndPoint = StsOrigin + "/Protection/RptToken";
        public const string UmaResourceSetEndPoint = StsOrigin + "/Resource/ResourceGet/{id}";
        public const string UmaDynClientEndPoint = StsOrigin + "/Uma/ClientRegister";
        public const string UmaRptEndPoint = StsOrigin + "/Protection/RptToken";

        public const string UmaDiscoveryConfiguration = "core/.well-known/uma-configuration";
    }

    public static class AsyncHelper
    {
        private static readonly TaskFactory _myTaskFactory = new TaskFactory(CancellationToken.None, TaskCreationOptions.None, TaskContinuationOptions.None, TaskScheduler.Default);

        public static void RunSync(Func<Task> func)
        {
            _myTaskFactory.StartNew(func).Unwrap().GetAwaiter().GetResult();
        }

        public static TResult RunSync<TResult>(Func<Task<TResult>> func)
        {
            return _myTaskFactory.StartNew(func).Unwrap().GetAwaiter().GetResult();
        }
    }
}
