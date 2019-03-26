using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ConvertHTMLToBootstrap.Startup))]

namespace ConvertHTMLToBootstrap
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}