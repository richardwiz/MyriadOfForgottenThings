using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Cerberus.Web.Startup))]
namespace Cerberus.Web
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
