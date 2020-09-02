using CommandLine;

namespace FIL.Database.Migration.Core
{
    public class Options
    {
        [Option('t', "timeout", HelpText = "TimeoutSeconds", Default = 60)]
        public int TimeoutSeconds { get; set; }

        [Option('m', "migration", HelpText = "MigrationRequest")]
        public long? MigrationRequest { get; set; }

        [Option('r', "RefreshTestData", Default = "true")]
        public string RefreshTestData { get; set; }

        [Option('c', "ConnectionString", Default = null)]
        public string ConnectionString { get; set; }

        public bool GenerateEnums { get; set; }
    }
}