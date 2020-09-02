namespace FIL.Configuration.Utilities
{
    public class Constants
    {
        public const string ConfigurationApiEndpointProperty = "configurationApiEndpoint";
        public const string ConfigurationApiEndpointEnvironmentVariable = "KZ_CONFIGURATION_API_ENDPOINT";

        internal const string ServiceConfigSettingsExpirationMinutesProperty = "serviceConfigSettingsExpirationMinutes";
        internal const string ServiceConfigSettingsExpirationMinutesEnvironmentVariable = "KZ_CONFIGURATION_SETTING_EXPIRATION_MINUTES";

        internal const string DefaultConfigSetName = "DEFAULT";

        internal const string ConfigSetPreferLocalProperty = "configSetPreferLocal";
        internal const string ConfigSetNameProperty = "configSetName";
        internal const string ConfigSetNameEnvironmentVariable = "ASPNETCORE_ENVIRONMENT";
    }
}