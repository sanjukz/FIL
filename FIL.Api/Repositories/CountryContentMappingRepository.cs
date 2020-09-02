using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface ICountryContentMappingRepository : IOrmRepository<CountryContentMapping, CountryContentMapping>
    {
        CountryContentMapping Get(long id);

        IEnumerable<CountryContentMapping> GetByCountryId(long countryId);
    }

    public class CountryContentMappingRepository : BaseLongOrmRepository<CountryContentMapping>, ICountryContentMappingRepository
    {
        public CountryContentMappingRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public CountryContentMapping Get(long id)
        {
            return Get(new CountryContentMapping { Id = id });
        }

        public IEnumerable<CountryContentMapping> GetByCountryId(long countryId)
        {
            return GetAll(statement => statement
                .Where($"{nameof(CountryContentMapping.CountryId):C}=@Ids AND IsEnabled=1")
                .WithParameters(new { Ids = countryId }));
        }
    }
}