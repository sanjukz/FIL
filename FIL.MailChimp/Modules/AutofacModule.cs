using Autofac;
using CurrencyConverter;
using Microsoft.Extensions.Configuration;
using Module = Autofac.Module;

namespace FIL.MailChimp.Modules
{
    public class AutofacModule : Module
    {
        private readonly IConfiguration _configurationRoot;

        public AutofacModule(IConfiguration configurationRoot)
        {
            _configurationRoot = configurationRoot;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new Configuration.Modules.AutofacModule(_configurationRoot));
            builder.RegisterModule(new Logging.Modules.AutofacModule(_configurationRoot));
            builder.RegisterType<IMailChimpProvider>().As<MailChimpProvider>();
        }
    }
}