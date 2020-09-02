using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface ISeasonDaysMappingsRepository : IOrmRepository<SeasonDaysMapping, SeasonDaysMapping>
    {
        IEnumerable<SeasonDaysMapping> GetByPlaceSeasonDetailId(long placeSeasonDetailId);

        SeasonDaysMapping Get(long id);

        IEnumerable<SeasonDaysMapping> GetByPlaceSeasonDetailIds(List<long> placeSeasonDetailId);
    }

    public class SeasonDaysMappingsRepository : BaseLongOrmRepository<SeasonDaysMapping>, ISeasonDaysMappingsRepository
    {
        public SeasonDaysMappingsRepository(IDataSettings dataSettings)
            : base(dataSettings)
        {
        }

        public SeasonDaysMapping Get(long id)
        {
            return Get(new SeasonDaysMapping { Id = id });
        }

        public IEnumerable<SeasonDaysMapping> GetByPlaceSeasonDetailId(long placeSeasonDetailId)
        {
            return GetAll(statement => statement
                .Where($"{nameof(SeasonDaysMapping.PlaceSeasonDetailId):C}=@placeSeasonId")
                .WithParameters(new { placeSeasonId = placeSeasonDetailId }));
        }

        public IEnumerable<SeasonDaysMapping> GetByPlaceSeasonDetailIds(List<long> placeSeasonDetailId)
        {
            return GetAll(statement => statement
                .Where($"{nameof(SeasonDaysMapping.PlaceSeasonDetailId):C} IN @placeSeasonId")
                .WithParameters(new { placeSeasonId = placeSeasonDetailId }));
        }
    }
}