using Autofac;
using FIL.Configuration;
using FIL.Configuration.Repositories;
using FIL.Configuration.Utilities;
using FIL.Contracts.Commands.ExOz;
using FIL.Contracts.Models.Integrations.ExOz;
using FIL.Foundation.Senders;
using FIL.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using FIL.Contracts.DataModels;
using static FIL.Contracts.Models.Integrations.ExOz.ExOzProductResponse;
using static FIL.Contracts.Models.Integrations.ExOz.ExOzProductOptionResponse;
using static FIL.Contracts.Models.Integrations.ExOz.ExOzSessionResponse;

namespace FIL.ExOzConsoleApp.Modules
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

            builder.RegisterType<ConsoleLogger>().As<IConsoleLogger>();
            builder.RegisterType<Synchronizer>().As<ISynchronizer>();

            // All Synchronizers
            builder.RegisterType<SyncExOzCountries>().As<ISynchronizer<SaveExOzCountryCommandResult, object>>();
            builder.RegisterType<SyncExOzStates>().As<ISynchronizer<SaveExOzStateCommandResult, string>>();
            builder.RegisterType<SyncExOzRegions>().As<ISynchronizer<SaveExOzRegionCommandResult, string>>();
            builder.RegisterType<SyncExOzOperators>().As<ISynchronizer<ProductList, OperatorList>>();
            builder.RegisterType<SyncExOzProducts>().As<ISynchronizer<SessionList, ProductList>>();
            builder.RegisterType<SyncExOzProductSessions>().As<ISynchronizer<ProductOptionList, SessionList>>();
            builder.RegisterType<SyncExOzProductOptions>().As<ISynchronizer<List<ExOzProductOption>, ProductOptionList>>();
        }
    }
}