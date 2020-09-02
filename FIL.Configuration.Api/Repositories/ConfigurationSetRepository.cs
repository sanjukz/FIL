using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Configuration.Contracts.Models;

namespace FIL.Configuration.Api.Repositories
{
    public interface IConfigurationSetRepository : IOrmRepository<ConfigurationSet, ConfigurationSet>
    {
    }

    public class ConfigurationSetRepository : BaseOrmRepository<ConfigurationSet>, IConfigurationSetRepository
    {
        public ConfigurationSetRepository(IDataSettings dataSettings)
            : base(dataSettings)
        {
        }
    }
}