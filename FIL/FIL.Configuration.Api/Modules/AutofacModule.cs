using Autofac;
using FIL.Api.Core.Utilities;
using FIL.Configuration.Api.Utilities;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace FIL.Configuration.Api.Modules
{
    public class AutofacModule : Autofac.Module
    {
        private readonly IConfiguration _configurationRoot;

        public AutofacModule(IConfiguration configurationRoot)
        {
            _configurationRoot = configurationRoot;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new Logging.Modules.AutofacModule(_configurationRoot));

            builder.Register(c => _configurationRoot);
            builder.RegisterType<DataSettings>().As<IDataSettings>().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(Assembly.GetEntryAssembly())
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}