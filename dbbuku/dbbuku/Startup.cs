using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(dbbuku.Startup))]
namespace dbbuku
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
