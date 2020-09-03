using Microsoft.AspNetCore.Hosting;
using FIL.Web.Core.Startup;
using Autofac;
using AutoMapper;
using FIL.Web.Admin.Profiles;
using Microsoft.Extensions.Configuration;

namespace FIL.Web.Admin
{
    public class Startup : BaseStartup
    {
        public Startup(IHostingEnvironment env, IConfiguration configuration)
            : base(env, configuration)
        {
        }

        protected override void RegisterModules(ContainerBuilder builder)
        {
            // Meant for other projects to override to register their own modules
            builder.RegisterModule(new Modules.AutofacModule());
        }

        protected override void RegisterProfiles(IMapperConfigurationExpression cfg)
        {
            // Meant for other projects to override to register their own automapper profiles
            cfg.AddProfile<AutoMapperProfile>();
        }
    }
}
