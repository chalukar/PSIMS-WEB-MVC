using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PSIMS.Startup))]
namespace PSIMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
