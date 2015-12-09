using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(herental.Startup))]
namespace herental
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
