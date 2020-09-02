using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface ICityDescriptionRepository : IOrmRepository<CityDescription, CityDescription>
    {
        CityDescription Get(long id);

        CityDescription GetBycityId(long cityId);
    }

    public class CityDescriptionRepository : BaseLongOrmRepository<CityDescription>, ICityDescriptionRepository
    {
        public CityDescriptionRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public CityDescription Get(long id)
        {
            return Get(new CityDescription { Id = id });
        }

        public CityDescription GetBycityId(long cityId)
        {
            return GetAll(s => s.Where($"{nameof(CityDescription.CityId):C} = @cityId")
                .WithParameters(new { cityId = cityId })
            ).FirstOrDefault();
        }
    }
}