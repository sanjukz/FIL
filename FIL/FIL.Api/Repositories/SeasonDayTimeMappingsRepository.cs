using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface ISeasonDayTimeMappingsRepository : IOrmRepository<SeasonDaysTimeMapping, SeasonDaysTimeMapping>
    {
        SeasonDaysTimeMapping Get(long id);

        IEnumerable<SeasonDaysTimeMapping> GetSeasonDaysMappings(long seasonDaysMappingId);

        IEnumerable<SeasonDaysTimeMapping> GetAllSeasonDaysMappings(List<long> seasonDaysMappingId);
    }

    public class SeasonDayTimeMappingsRepository : BaseLongOrmRepository<SeasonDaysTimeMapping>, ISeasonDayTimeMappingsRepository
    {
        public SeasonDayTimeMappingsRepository(IDataSettings dataSettings)
            : base(dataSettings)
        {
        }

        public SeasonDaysTimeMapping Get(long id)
        {
            return Get(new SeasonDaysTimeMapping { Id = id });
        }

        public IEnumerable<SeasonDaysTimeMapping> GetSeasonDaysMappings(long seasonDaysMappingId)
        {
            return GetAll(statement => statement
                .Where($"{nameof(SeasonDaysTimeMapping.SeasonDaysMappingId):C}=@placeSeasonDaysId")
                .WithParameters(new { placeSeasonDaysId = seasonDaysMappingId }));
        }

        public IEnumerable<SeasonDaysTimeMapping> GetAllSeasonDaysMappings(List<long> seasonDaysMappingId)
        {
            return GetAll(statement => statement
                .Where($"{nameof(SeasonDaysTimeMapping.SeasonDaysMappingId):C} IN @placeSeasonDaysId")
                .WithParameters(new { placeSeasonDaysId = seasonDaysMappingId }));
        }
    }
}