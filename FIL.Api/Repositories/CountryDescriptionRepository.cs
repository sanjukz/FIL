using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface ICountryDescriptionRepository : IOrmRepository<CountryDescription, CountryDescription>
    {
        CountryDescription Get(long id);

        CountryDescription GetByCountryId(long countryId);
    }

    public class CountryDescriptionRepository : BaseLongOrmRepository<CountryDescription>, ICountryDescriptionRepository
    {
        public CountryDescriptionRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public CountryDescription Get(long id)
        {
            return Get(new CountryDescription { Id = id });
        }

        public CountryDescription GetByCountryId(long countryId)
        {
            return GetAll(s => s.Where($"{nameof(CountryDescription.CountryId):C}=@CountryId")
                .WithParameters(new { CountryId = countryId })
            ).FirstOrDefault();
        }
    }
}