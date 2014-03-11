using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Grid4Guys.Startup))]
namespace Grid4Guys
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
