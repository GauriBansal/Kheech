using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Kheech.Web.Startup))]
namespace Kheech.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
