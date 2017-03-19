using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sj.Mg.CliLib.Utils
{
    class Common
    {
        public const string ReApi = "https://localhost:44306/";
        public const string ReApiStsCallback = "https://localhost:44306/home/stscallback";
        public const string ReClientMvc = "https://localhost:44383/";
        public const string ReClientMvcStsCallback = "https://localhost:44383/home/stscallback";
        public const string IssuerUri = "https://oidc.medgrotto.com/identity";
        public const string StsOrigin = "https://localhost:44305";


        public const string ClientSecret = "myrandomclientsecret";

        public const string Sts = StsOrigin + "/core";
        public const string StsTokenEndpoint = Sts + "/connect/token";
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
