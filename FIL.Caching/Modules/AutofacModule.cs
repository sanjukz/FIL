using Autofac;
using FIL.Caching.Contracts.Interfaces;
using FIL.Caching.Helpers;
using FIL.Caching.Managers;
using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Logging;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace FIL.Caching.Modules
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

            builder.RegisterType<RedisClientManager>().As<IRedisClientManager>().SingleInstance();
            builder
                .Register<ICacheHelper>(c =>
                {
                    if (c.Resolve<ISettings>().GetConfigSetting<bool>(SettingKeys.Redis.Enabled))
                    {
                        return new RedisHelper(c.Resolve<IRedisClientManager>(), c.Resolve<ILogFactory>());
                    }
                    if (c.TryResolve(out IMemoryCache cache))
                    {
                        return new CacheHelper(cache, c.Resolve<ILogFactory>());
                    }
                    return new NoopCacheHelper();
                })
                .As<ICacheHelper>()
                .SingleInstance();
        }
    }
}