using Autofac;
using Autofac.Extensions.DependencyInjection;
using FIL.Configuration.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FIL.Window.Service
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Timer t = new Timer(TimerCallBack, null, 0, 900000);
            //Timer t = new Timer(TimerCallBack, null, 0, 6000);
            Console.ReadKey();
        }

        private static async void TimerCallBack(Object o)
        {
            IConfigurationBuilder tempConfigBuilder = new ConfigurationBuilder();
            //tempConfigBuilder.AddJsonFile(
            //    @"C:\Program Files\Amazon\ElasticBeanstalk\config\containerconfiguration",
            //    optional: true,
            //    reloadOnChange: true
            //);
            IConfigurationRoot tempConfig = tempConfigBuilder.Build();

            Dictionary<string, string> ebConfig = ConfigurationBuilderExtensions.GetConfig(tempConfig);
            // CHECK IF EBCONFIG HAS ENVIRONMENT KEY IN IT
            // IF SO THEN CHANGE THE BUILDERS ENVIRONMENT
            const string envKey = "ASPNETCORE_ENVIRONMENT";

            var ebEnvironment = "Development";
            if (ebConfig.ContainsKey(envKey))
            {
                ebEnvironment = ebConfig[envKey];
            }

            tempConfigBuilder.AddJsonFile(
                        "appsettings.json",
                        optional: false,
                        reloadOnChange: true
                    )
                    .AddJsonFile(
                        $"appsettings.{ebEnvironment}.json",
                        optional: true,
                        reloadOnChange: true
                    )
                    .AddInMemoryCollection(ebConfig);

            try
            {
                IServiceCollection services = new ServiceCollection();
                services.AddMemoryCache();
                var builder = new ContainerBuilder();
                builder.RegisterModule(new AutofacModule(tempConfigBuilder.Build()));
                builder.Populate(services);
                var container = builder.Build();

                using (var scope = container.BeginLifetimeScope())
                {
                    //await scope.Resolve<ISynchronizer>().Synchronize();
                    //await scope.Resolve<IValueRetailDataSync>().Synchronize();
                    //await scope.Resolve<IPOneSync>().Synchronize();
                    await scope.Resolve<ITiqetsDataSync>().Synchronize();
                    //await scope.Resolve<IHohoSync>().Synchronize();
                    //await scope.Resolve<IAlgoliaDataSync>().Synchronize();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception : " + DateTime.Now);
            }

            Console.WriteLine("Timer: " + DateTime.Now);
            GC.Collect();
        }
    }
}
