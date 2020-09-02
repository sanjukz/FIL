using Autofac;
using Microsoft.Extensions.Configuration;

namespace FIL.Logging.Modules
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
            // Explicitly passed and provided because it is required from a parent module.
            // this lets us ensure the contract, and Autofac is smart enough to find dupes.
            builder.Register(c => _configurationRoot);

            builder.Register(c => new LogFactory(c.Resolve<IConfiguration>())).As<ILogFactory>();
            builder.Register(c => c.Resolve<ILogFactory>().GetLogger(typeof(Logger))).As<ILogger>();
        }
    }
}