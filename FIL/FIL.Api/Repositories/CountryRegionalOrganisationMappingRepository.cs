using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;

namespace FIL.Api.Repositories
{
    public interface ICountryRegionalOrganisationMappingRepository : IOrmRepository<CountryRegionalOrganisationMapping, CountryRegionalOrganisationMapping>
    {
        CountryRegionalOrganisationMapping Get(int id);
    }

    public class CountryRegionalOrganisationMappingRepository : BaseOrmRepository<CountryRegionalOrganisationMapping>, ICountryRegionalOrganisationMappingRepository
    {
        public CountryRegionalOrganisationMappingRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public CountryRegionalOrganisationMapping Get(int id)
        {
            return Get(new CountryRegionalOrganisationMapping { Id = id });
        }
    }
}