using Microsoft.Owin;
using Owin;

//[assembly: OwinStartupAttribute(typeof(Sj.Mg.Client.Startup))]
namespace Sj.Mg.Client
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
