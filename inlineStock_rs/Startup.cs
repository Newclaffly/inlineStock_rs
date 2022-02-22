using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(inlineStock_rs.Startup))]
namespace inlineStock_rs
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
