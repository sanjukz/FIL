using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Configuration.Contracts.Models;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Configuration.Api.Repositories
{
    public interface IConfigurationKeyRepository : IOrmRepository<ConfigurationKey, ConfigurationKey>
    {
        ConfigurationKey FindByName(string name);

        IEnumerable<ConfigurationKey> GetKeys(IEnumerable<int> configurationKeyIds);
    }

    public class ConfigurationKeyRepository : BaseOrmRepository<ConfigurationKey>, IConfigurationKeyRepository
    {
        public ConfigurationKeyRepository(IDataSettings dataSettings)
            : base(dataSettings)
        {
        }

        public ConfigurationKey FindByName(string name)
        {
            return GetAll(s => s
                .Where($"{nameof(ConfigurationKey.Name):C}=@Name")
                .WithParameters(new { Name = name })).FirstOrDefault();
        }

        public IEnumerable<ConfigurationKey> GetKeys(IEnumerable<int> configurationKeyIds)
        {
            return GetAll(s => s
                .Where($"{nameof(ConfigurationKey.Id):C} IN @Ids")
                .WithParameters(new { Ids = configurationKeyIds }));
        }
    }
}