using FluentMigrator.Runner;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Initialization;
using FluentMigrator.Runner.Processors;
using FluentMigrator.Runner.Processors.SqlServer;
using FIL.Database.Migration.Core.Profiles;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace FIL.Database.Migration.Core
{
    public class Runner
    {
        private static Options _options;

        private string DatabaseConnectionString { get; set; }

        private static IConfiguration _configuration;

        internal static IConfiguration Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    var configBuilder = new ConfigurationBuilder().AddJsonFile(Constants.ConfigFilenameDefault);
                    var memDict = new Dictionary<string, string>();
                    foreach (string key in Environment.GetEnvironmentVariables().Keys)
                    {
                        memDict.Add(key, Environment.GetEnvironmentVariable(key));
                    }
                    configBuilder.AddInMemoryCollection(memDict);

                    _configuration = configBuilder.Build();
                }
                return _configuration;
            }
        }

        public Runner(Options options)
        {
            DatabaseConnectionString = options.ConnectionString
                ?? Configuration[Constants.DatabaseConnectionStringEnvironmentVariable]
                ?? Configuration[Constants.DatabaseConnectionStringProperty];
            _options = options;
        }

        public int Run()
        {
            var announcer = new TextWriterAnnouncer(s => System.Diagnostics.Debug.WriteLine(s))
            {
                ShowSql = true
            };

            IRunnerContext migrationContext = new RunnerContext(announcer)
            {
                Profile = Constants.MigratorProfileName
            };

            var processor = new SqlServer2014ProcessorFactory().Create(DatabaseConnectionString, announcer, new ProcessorOptions
            {
                PreviewOnly = false,
                Timeout = _options.TimeoutSeconds
            });

            var runner = new MigrationRunner(Assembly.GetEntryAssembly(), migrationContext, processor);
            try
            {
                if (_options.GenerateEnums)
                {
                    runner.Up(new EnumTableGenerator());
                }
                if (_options.MigrationRequest.HasValue)
                {
                    runner.MigrateUp(_options.MigrationRequest.Value, true);
                }
                else
                {
                    runner.MigrateUp(true);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                return -1;
            }

            return 0;
        }
    }
}