using Autofac;
using FIL.Configuration.Repositories;
using FIL.Configuration.Utilities;
using FIL.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;

namespace FIL.Configuration.Modules
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
            builder.Register(c => _configurationRoot);

            // The Configuration repositories use their own RestHelper that has its host name pointing to Kz.Configuration.Api.
            builder
                .Register(c =>
                {
                    var configurationRoot = c.Resolve<IConfiguration>();
                    var baseAddress = new Uri(configurationRoot[Constants.ConfigurationApiEndpointEnvironmentVariable] ?? configurationRoot[Constants.ConfigurationApiEndpointProperty]);
                    return new RestHelper(baseAddress);
                })
                .Keyed<IRestHelper>("ConfigurationRestHelper");

            builder
                .Register(c => new ConfigurationRepository(c.ResolveKeyed<IRestHelper>("ConfigurationRestHelper"), c.Resolve<IMemoryCache>(), c.Resolve<IConfiguration>()))
                .As<IConfigurationRepository>();

            builder.RegisterType<Settings>().As<ISettings>();
        }
    }
}