using Autofac;
using FluentValidation;
using FIL.Caching.Contracts.Interfaces;
using FIL.Caching.Coordinators;
using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Contracts.Interfaces;
using FIL.Foundation.Senders;
using FIL.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;

namespace FIL.Foundation.Modules
{
    public sealed class AutofacModule : Autofac.Module
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
            builder.RegisterModule(new Caching.Modules.AutofacModule(_configurationRoot));

            builder.Register(c => new RestHelper(new Uri(c.Resolve<ISettings>().GetMachineEnvironmentConfigSetting("API_ENDPOINT") ?? c.Resolve<ISettings>().GetConfigSetting<string>(SettingKeys.Foundation.Http.ApiEndpoint))))
            .As<IRestHelper>();

            // We generally want to use a full coordinator if enabled, otherwise noop.
            // If we want to replace with WriteOnly, etc. Just provide a new Coordinator in a proceeding Module (ie. KITMS)
            builder
                .Register<ICacheCoordinator>(c =>
                {
                    if (c.Resolve<ISettings>().GetConfigSetting<bool>(SettingKeys.Redis.Enabled))
                    {
                        return new RedisCoordinator(c.Resolve<ICacheHelper>());
                    }
                    return new NoopRedisCoordinator();
                })
                .As<ICacheCoordinator>();

            builder.RegisterType<CommandSender>().As<ICommandSender>();
            builder.RegisterType<QuerySender>().As<IQuerySender>();

            // Register all the repositories.
            RegisterGroupAsImplementedInterface(builder, "Repository");

            // Register all the validators.
            AssemblyScanner
                .FindValidatorsInAssembly(typeof(IFILValidator).GetTypeInfo().Assembly)
                .ForEach(result => builder.RegisterType(result.ValidatorType));
        }

        private void RegisterGroupAsImplementedInterface(ContainerBuilder builder, string sharedSuffix)
        {
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.Name.EndsWith(sharedSuffix))
                .AsImplementedInterfaces();
        }
    }
}