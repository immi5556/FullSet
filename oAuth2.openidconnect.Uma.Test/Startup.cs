using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(oAuth2.openidconnect.Uma.Test.Startup))]
namespace oAuth2.openidconnect.Uma.Test
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
