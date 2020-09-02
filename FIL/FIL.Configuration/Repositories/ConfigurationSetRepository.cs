using FIL.Configuration.Contracts.Models;
using FIL.Configuration.Utilities;
using FIL.Http;
using FIL.Http.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Configuration.Repositories
{
    public interface IConfigurationSetRepository
    {
        Task<ConfigurationSet> GetCurrentConfigurationSet();

        Task<IEnumerable<ConfigurationSet>> GetAll();
    }

    public class ConfigurationSetRepository : BaseRepository<ConfigurationSet>, IConfigurationSetRepository
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;
        private const string CacheKey = "Kz.Configuration.Repositories.ConfigurationSetRepository.GetAll";

        public ConfigurationSetRepository(IRestHelper restHelper, IMemoryCache memoryCache, IConfiguration configuration)
            : base(restHelper)
        {
            _memoryCache = memoryCache;
            _configuration = configuration;
        }

        public async Task<ConfigurationSet> GetCurrentConfigurationSet()
        {
            string key = _configuration[Constants.ConfigSetNameEnvironmentVariable]
                ?? _configuration[Constants.ConfigSetNameProperty]
                ?? Constants.DefaultConfigSetName;
            IEnumerable<ConfigurationSet> configSets = await GetAll().ConfigureAwait(false);
            return configSets.FirstOrDefault(c => c.Name.ToUpper() == key.ToUpper().Trim());
        }

        public async Task<IEnumerable<ConfigurationSet>> GetAll()
        {
            IEnumerable<ConfigurationSet> configurationSets = await GetCachedInstance().ConfigureAwait(false);
            return configurationSets;
        }

        private async Task<IEnumerable<ConfigurationSet>> GetCachedInstance()
        {
            IEnumerable<ConfigurationSet> configurationSets;
            if (!_memoryCache.TryGetValue(CacheKey, out configurationSets))
            {
                configurationSets = await GetAllResults("api/configurationset").ConfigureAwait(false);
                _memoryCache.Set(CacheKey, configurationSets);
            }
            return configurationSets;
        }
    }
}