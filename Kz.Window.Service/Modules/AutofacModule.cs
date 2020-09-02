using Autofac;
using Microsoft.Extensions.Configuration;

namespace Kz.Window.Service
{
    public class AutofacModule : Module
    {
        private readonly IConfigurationRoot _configurationRoot;

        public AutofacModule(IConfigurationRoot configurationRoot)
        {
            _configurationRoot = configurationRoot;
        }
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => _configurationRoot);
            builder.RegisterModule(new Logging.Modules.AutofacModule(_configurationRoot));
            builder.RegisterModule(new Configuration.Modules.AutofacModule(_configurationRoot));
            builder.RegisterModule(new Foundation.Modules.AutofacModule(_configurationRoot));
            builder.RegisterType<Synchronizer>().As<ISynchronizer>();
            builder.RegisterType<AlgoliaDataSync>().As<IAlgoliaDataSync>();
            builder.RegisterType<HohoSync>().As<IHohoSync>();
            builder.RegisterType<TiqetsDataSync>().As<ITiqetsDataSync>();
            builder.RegisterType<HubspotAbandonCart>().As<ISynchronizer>();
            builder.RegisterType<ValueRetailDataSync>().As<IValueRetailDataSync>();
            builder.RegisterType<POneSync>().As<IPOneSync>();
            builder.RegisterType<Messaging.Senders.AccountEmailSender>().As<Messaging.Senders.IAccountEmailSender>();
        }
    }
}
