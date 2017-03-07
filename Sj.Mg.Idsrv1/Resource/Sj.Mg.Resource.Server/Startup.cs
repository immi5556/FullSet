using Microsoft.Owin;
using Owin;

//[assembly: OwinStartupAttribute(typeof(Sj.Mg.Resource.Server.Startup))]
namespace Sj.Mg.Resource.Server
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
