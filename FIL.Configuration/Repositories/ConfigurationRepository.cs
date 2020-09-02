using FIL.Configuration.Exceptions;
using FIL.Configuration.Utilities;
using FIL.Http;
using FIL.Http.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Configuration.Repositories
{
    public interface IConfigurationRepository
    {
        Task<Contracts.Models.Configuration> Get(string key);

        Task<Dictionary<int, Contracts.Models.Configuration>> GetAllByKey(string key);

        Task<Dictionary<string, Contracts.Models.Configuration>> GetAll();

        Task<Contracts.Models.Configuration> Save(Contracts.Models.Configuration configuration);

        Task Delete(Contracts.Models.Configuration configuration);
    }

    public class ConfigurationRepository : BaseRepository<Contracts.Models.Configuration>, IConfigurationRepository
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configurationRoot;
        private const string CacheKey = "Kz.Configuration.Repositories.ConfigurationRepository.GetAll";
        private readonly int _expirationMinutes;

        public ConfigurationRepository(IRestHelper restHelper, IMemoryCache memoryCache, IConfiguration configurationRoot)
            : base(restHelper)
        {
            _expirationMinutes = Convert.ToInt32(configurationRoot[Constants.ServiceConfigSettingsExpirationMinutesEnvironmentVariable] ?? configurationRoot[Constants.ServiceConfigSettingsExpirationMinutesProperty]);
            _memoryCache = memoryCache;
            _configurationRoot = configurationRoot;
        }

        public async Task<Contracts.Models.Configuration> Get(string key)
        {
            var configurations = await GetAll().ConfigureAwait(false);
            if (!configurations.ContainsKey(key))
            {
                throw new MissingSettingException(key);
            }
            return configurations[key];
        }

        public async Task<Dictionary<string, Contracts.Models.Configuration>> GetAll()
        {
            return await GetCachedInstance().ConfigureAwait(false);
        }

        public async Task<Dictionary<int, Contracts.Models.Configuration>> GetAllByKey(string key)
        {
            return (await GetAllResults($"api/configuration/key/{key}/")).ToDictionary(k => k.ConfigurationSetId, v => v);
        }

        public Task<Contracts.Models.Configuration> Save(Contracts.Models.Configuration configuration)
        {
            return PostResult(configuration, "api/configuration/save");
        }

        public Task Delete(Contracts.Models.Configuration configuration)
        {
            return PostVoidResult(configuration, "api/configuration/delete");
        }

        private async Task<Dictionary<string, Contracts.Models.Configuration>> GetCachedInstance()
        {
            Dictionary<string, Contracts.Models.Configuration> configurations;
            if (!_memoryCache.TryGetValue(CacheKey, out configurations))
            {
                string configurationSetName = null;
                if (_configurationRoot[Constants.ConfigSetPreferLocalProperty] != null)
                {
                    // Do not let this happen in prod.
                    if (_configurationRoot[Constants.ConfigSetNameEnvironmentVariable] == null
                        || !_configurationRoot[Constants.ConfigSetNameEnvironmentVariable]
                            .ToUpperInvariant()
                            .Contains("PROD"))
                    {
                        // boolean setting to prefer whats in your appsettings file
                        if (bool.Parse(_configurationRoot[Constants.ConfigSetPreferLocalProperty]))
                        {
                            configurationSetName = _configurationRoot[Constants.ConfigSetNameProperty]
                                                   ?? _configurationRoot[Constants.ConfigSetNameEnvironmentVariable]
                                                   ?? _configurationRoot[Constants.DefaultConfigSetName];
                        }
                    }
                }
                if (configurationSetName == null)
                {
                    configurationSetName = _configurationRoot[Constants.ConfigSetNameEnvironmentVariable]
                                           ?? _configurationRoot[Constants.ConfigSetNameProperty]
                                           ?? _configurationRoot[Constants.DefaultConfigSetName];
                }

                configurations = (await GetResult<Dictionary<string, Contracts.Models.Configuration>>($"api/configuration/configurationSetName/{configurationSetName}").ConfigureAwait(false));

                _memoryCache.Set(CacheKey, configurations, DateTime.Now.AddMinutes(_expirationMinutes));
            }
            return configurations;
        }
    }
}