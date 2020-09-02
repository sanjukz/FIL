namespace FIL.Database.Migration.Core
{
    public class Constants
    {
        public static string ConfigFilenameDefault = "config.json";

        public static string MigratorProfileName = "Default";
        public static string ConfigSetNameProperty = "configSetName";
        public static string ConfigSetNameEnvironmentVariable = "ASPNETCORE_ENVIRONMENT";

        public static string DatabaseConnectionStringProperty = "connectionString";
        public static string DatabaseConnectionStringEnvironmentVariable = "CONNECTION_STRING";
    }
}