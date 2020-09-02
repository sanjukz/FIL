using FIL.Configuration.Api.Repositories;
using FIL.Configuration.Api.Utilities;
using FIL.Configuration.Contracts.Models;
using FIL.Utilities.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Configuration.Api.Controllers
{
    public class ConfigurationController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IConfigurationRepository _configurationRepository;
        private readonly IConfigurationKeyRepository _configurationKeyRepository;
        private readonly IConfigurationSetRepository _configurationSetRepository;

        public ConfigurationController(
            IConfiguration configuration,
            IConfigurationRepository configurationRepository,
            IConfigurationKeyRepository configurationKeyRepository,
            IConfigurationSetRepository configurationSetRepository)
        {
            _configuration = configuration;
            _configurationRepository = configurationRepository;
            _configurationKeyRepository = configurationKeyRepository;
            _configurationSetRepository = configurationSetRepository;
        }

        [Route("api/configuration/configurationSetName/{configurationSetName}")]
        [HttpGet]
        public Dictionary<string, Contracts.Models.Configuration> GetAll(string configurationSetName)
        {
            if (configurationSetName.IsNullOrWhiteSpace())
            {
                configurationSetName = _configuration[Constants.DefaultConfigSetName];
            }
            var configResults = GetConfigurations(configurationSetName.ToUpperInvariant()).ToList();
            Dictionary<string, Contracts.Models.Configuration> configurations = new Dictionary<string, Contracts.Models.Configuration>();
            if (configResults.Any())
            {
                var configKeys = _configurationKeyRepository.GetKeys(configResults.Select(c => c.ConfigurationKeyId)).ToDictionary(ck => ck.Id, ck => ck.Name);
                configurations = configResults.ToDictionary(c => configKeys[c.ConfigurationKeyId]);
            }
            return configurations;
        }

        [Route("api/configuration/key/{key:maxlength(255)}/")]
        [HttpGet]
        public IEnumerable<Contracts.Models.Configuration> GetConfigurationsByKey(string key)
        {
            var configurationKey = _configurationKeyRepository.FindByName(key);
            return configurationKey == null ? new List<Contracts.Models.Configuration>() : _configurationRepository.GetConfigurationSettingsByKey(configurationKey);
        }

        [Route("api/configuration/save")]
        [HttpPost]
        public Contracts.Models.Configuration Save(Contracts.Models.Configuration configuration)
        {
            return _configurationRepository.Save(configuration);
        }

        [Route("api/configuration/delete")]
        [HttpPost]
        public void Delete([FromBody] Contracts.Models.Configuration configuration)
        {
            _configurationRepository.Delete(configuration);
        }

        private IEnumerable<Contracts.Models.Configuration> GetConfigurations(string configSetName)
        {
            var configurations = new Dictionary<int, Contracts.Models.Configuration>();
            var configurationSets = _configurationSetRepository.GetAll().ToList();

            var selectedConfigurationSet = configurationSets.FirstOrDefault(c => string.Equals(c.Name, configSetName, StringComparison.CurrentCultureIgnoreCase));

            // Fill configurations by reference recursively.
            GetConfigurationsInHierarchicalOrder(configurationSets, configurations, selectedConfigurationSet);

            return configurations.Values;
        }

        private void GetConfigurationsInHierarchicalOrder(List<ConfigurationSet> configurationSets,
            Dictionary<int, Contracts.Models.Configuration> configurations,
            ConfigurationSet configurationSet)
        {
            if (configurationSet == null)
            {
                return;
            }

            if (configurationSet.Id == Constants.DefaultConfigSetId && !configurationSet.IsEnabled)
            {
                throw new Exception("The DEFAULT ConfigurationSet cannot be inactive.");
            }

            if (configurationSet.IsEnabled)
            {
                var configurationResults = _configurationRepository.GetAll(configurationSet.Id)
                    .ToDictionary(c => c.ConfigurationKeyId);

                foreach (var key in configurationResults.Keys)
                {
                    if (!configurations.ContainsKey(key) && configurationResults[key].IsEnabled)
                    {
                        configurations.Add(key, configurationResults[key]);
                    }
                }
            }

            if (configurationSet.ParentConfigurationSetId.HasValue)
            {
                var selectedConfigurationSet = configurationSets.FirstOrDefault(c => c.Id == configurationSet.ParentConfigurationSetId.Value);
                GetConfigurationsInHierarchicalOrder(configurationSets, configurations, selectedConfigurationSet);
            }
            else if (configurationSet.Name != Constants.DefaultConfigSetName)
            {
                var selectedConfigurationSet = configurationSets.FirstOrDefault(c => c.Name.Equals(Constants.DefaultConfigSetName));
                GetConfigurationsInHierarchicalOrder(configurationSets, configurations, selectedConfigurationSet);
            }
        }
    }
}