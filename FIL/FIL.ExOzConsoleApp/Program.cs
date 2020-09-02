using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Net;
using System.Xml;
using System.IO;
using System.Text;
using FIL.ExOzConsoleApp.Entities.Classes;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIL.Logging;
using FIL.Utilities.Extensions;
using FIL.Contracts.DataModels;
using FIL.Foundation.Senders;
using FIL.Http;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using FIL.Configuration.Extensions;
using FIL.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FIL.ExOzConsoleApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            IConfigurationBuilder tempConfigBuilder = new ConfigurationBuilder();
            tempConfigBuilder.AddJsonFile(
                @"C:\Program Files\Amazon\ElasticBeanstalk\config\containerconfiguration",
                optional: true,
                reloadOnChange: true
            );
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
                builder.RegisterModule(new Modules.AutofacModule(tempConfigBuilder.Build()));
                builder.Populate(services);
                var container = builder.Build();

                using (var scope = container.BeginLifetimeScope())
                {
                    await scope.Resolve<ISynchronizer>().Synchronize();
                }
            }
            catch (Exception e)
            {
                // TODO: Log and report
                Console.WriteLine($"EXCEPTION: {e.Message}");
            }
            Console.WriteLine("ExperienceOz Sync Completed..!");
        }
    }
}