using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StructuredDebate.Startup))]
namespace StructuredDebate
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
