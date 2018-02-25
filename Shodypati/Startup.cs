using log4net.Config;
using Owin;

namespace Shodypati
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            XmlConfigurator.Configure();
        }
    }
}
