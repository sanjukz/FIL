using FIL.Configuration.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;

namespace FIL.Configuration.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHostBuilder(args).UseStartup<Startup>().Build().Run();
        }

        public static IWebHostBuilder BuildWebHostBuilder(string[] args)
        {
            // TEMP CONFIG BUILDER TO GET THE VALUES IN THE ELASTIC BEANSTALK CONFIG
            IConfigurationBuilder tempConfigBuilder = new ConfigurationBuilder();

            tempConfigBuilder.AddJsonFile(
                @"C:\Program Files\Amazon\ElasticBeanstalk\config\containerconfiguration",
                optional: true,
                reloadOnChange: true
            );

            IConfigurationRoot tempConfig = tempConfigBuilder.Build();

            Dictionary<string, string> ebConfig = ConfigurationBuilderExtensions.GetConfig(tempConfig);

            // START WEB HOST BUILDER
            IWebHostBuilder builder = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory());

            // CHECK IF EBCONFIG HAS ENVIRONMENT KEY IN IT
            // IF SO THEN CHANGE THE BUILDERS ENVIRONMENT
            const string envKey = "ASPNETCORE_ENVIRONMENT";

            if (ebConfig.ContainsKey(envKey))
            {
                string ebEnvironment = ebConfig[envKey];
                builder.UseEnvironment(ebEnvironment);
            }

            // CONTINUE WITH WEB HOST BUILDER AS NORMAL
            builder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                IHostingEnvironment env = hostingContext.HostingEnvironment;

                // ADD THE ELASTIC BEANSTALK CONFIG DICTIONARY
                config.AddJsonFile(
                        "appsettings.json",
                        optional: false,
                        reloadOnChange: true
                    )
                    .AddJsonFile(
                        $"appsettings.{env.EnvironmentName}.json",
                        optional: true,
                        reloadOnChange: true
                    )
                    .AddInMemoryCollection(ebConfig);

                config.AddEnvironmentVariables();

                if (args != null)
                {
                    config.AddCommandLine(args);
                }
            })
            .UseIISIntegration();

            return builder;
        }
    }
}