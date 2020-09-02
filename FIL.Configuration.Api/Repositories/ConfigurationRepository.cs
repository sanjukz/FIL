using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Configuration.Contracts.Models;
using System.Collections.Generic;

namespace FIL.Configuration.Api.Repositories
{
    public interface IConfigurationRepository : IOrmRepository<Contracts.Models.Configuration, Contracts.Models.Configuration>
    {
        IEnumerable<Contracts.Models.Configuration> GetConfigurationSettingsByKey(ConfigurationKey configurationKey);

        IEnumerable<Contracts.Models.Configuration> GetAll(int configurationSetId);
    }

    public class ConfigurationRepository : BaseOrmRepository<Contracts.Models.Configuration>, IConfigurationRepository
    {
        public ConfigurationRepository(IDataSettings dataSettings)
            : base(dataSettings)
        {
        }

        public IEnumerable<Contracts.Models.Configuration> GetAll(int configurationSetId)
        {
            return GetAll(x => x.Where($"ConfigurationSetId={configurationSetId}"));
        }

        public IEnumerable<Contracts.Models.Configuration> GetConfigurationSettingsByKey(ConfigurationKey configurationKey)
        {
            return GetAll(s => s
                .Where($"{nameof(Contracts.Models.Configuration.ConfigurationKeyId):C}=@ConfigurationKeyId")
                .WithParameters(new { ConfigurationKeyId = configurationKey.Id }));
        }
    }
}