using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services.Default;
using Owin;
using Sj.Mg.Idsrv4.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace Sj.Mg.Idsrv4
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Map("/identity", idsrv =>
            {
                var idsrvfactory = new IdentityServerServiceFactory()
                .UseInMemoryClients(Clients.Get())
                .UseInMemoryScopes(Scopes.Get())
                .UseInMemoryUsers(Users.Get());

                var corsPolicyService = new DefaultCorsPolicyService()
                {
                    AllowAll = true
                };

                idsrvfactory.CorsPolicyService = new
                    Registration<IdentityServer3.Core.Services.ICorsPolicyService>(corsPolicyService);

                var opts = new IdentityServerOptions
                {
                    Factory = idsrvfactory,
                    SiteName = "MedGrotto Identity Server",
                    IssuerUri = AppConstants.Constants.IssuerUri,
                    PublicOrigin = AppConstants.Constants.StsOrigin,
                    SigningCertificate = LoadCertificate(),
                    RequireSsl = false,
                };

                idsrv.UseIdentityServer(opts);
            });
        }

        X509Certificate2 LoadCertificate()
        {
            return new X509Certificate2(
                string.Format(@"{0}\certificates\idsrv3test.pfx",
                AppDomain.CurrentDomain.BaseDirectory), "idsrv3test");
        }
    }
}