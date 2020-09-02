using FIL.Configuration.Repositories;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;

namespace FIL.Configuration
{
    public interface ISettings
    {
        T GetConfigSetting<T>(string key);

        T GetConfigSettingJson<T>(string key);

        Contracts.Models.Configuration GetConfigSetting(string key);

        string GetMachineEnvironmentConfigSetting(string key);

        string ResolveSetting(string preferredKey, string fallbackKey);
    }

    public class Settings : ISettings
    {
        private readonly IConfigurationRepository _configurationRepository;
        private readonly IConfiguration _configurationRoot;

        public Settings(IConfigurationRepository configurationRepository, IConfiguration configurationRoot)
        {
            _configurationRepository = configurationRepository;
            _configurationRoot = configurationRoot;
        }

        public T GetConfigSetting<T>(string key)
        {
            Contracts.Models.Configuration configuration = _configurationRepository.Get(key).Result;
            return (T)Convert.ChangeType(configuration.Value, typeof(T));
        }

        public T GetConfigSettingJson<T>(string key)
        {
            Contracts.Models.Configuration configuration = _configurationRepository.Get(key).Result;
            return JsonConvert.DeserializeObject<T>(configuration.Value);
        }

        public Contracts.Models.Configuration GetConfigSetting(string key)
        {
            return _configurationRepository.Get(key).Result;
        }

        public string ResolveSetting(string preferredKey, string fallbackKey)
        {
            return _configurationRoot[preferredKey] ?? _configurationRoot[fallbackKey];
        }

        public string GetMachineEnvironmentConfigSetting(string key)
        {
            return _configurationRoot[key];
        }
    }
}