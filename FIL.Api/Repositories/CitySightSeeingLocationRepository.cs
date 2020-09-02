using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface ICitySightSeeingLocationRepository : IOrmRepository<CitySightSeeingLocation, CitySightSeeingLocation>
    {
        CitySightSeeingLocation Get(int id);

        CitySightSeeingLocation GetByName(string name);

        IEnumerable<CitySightSeeingLocation> GetAll();
    }

    public class CitySightSeeingLocationRepository : BaseOrmRepository<CitySightSeeingLocation>, ICitySightSeeingLocationRepository
    {
        public CitySightSeeingLocationRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public CitySightSeeingLocation Get(int id)
        {
            return Get(new CitySightSeeingLocation { Id = id });
        }

        public CitySightSeeingLocation GetByName(string name)
        {
            return GetAll(s => s.Where($"{nameof(CitySightSeeingLocation.Name):C} = @Name")
                .WithParameters(new { Name = name })
            ).FirstOrDefault();
        }

        public IEnumerable<CitySightSeeingLocation> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteCitySightSeeingLocation(CitySightSeeingLocation CitySightSeeingLocation)
        {
            Delete(CitySightSeeingLocation);
        }

        public CitySightSeeingLocation SaveCitySightSeeingLocation(CitySightSeeingLocation CitySightSeeingLocation)
        {
            return Save(CitySightSeeingLocation);
        }
    }
}