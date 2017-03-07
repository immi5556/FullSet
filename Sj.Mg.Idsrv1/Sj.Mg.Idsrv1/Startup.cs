using Microsoft.Owin;
using Owin;
using Serilog;

[assembly: OwinStartupAttribute(typeof(Sj.Mg.Idsrv1.Startup))]
namespace Sj.Mg.Idsrv1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
