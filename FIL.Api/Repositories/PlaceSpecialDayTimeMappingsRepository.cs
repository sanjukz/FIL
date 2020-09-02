using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IPlaceSpecialDayTimeMappingsRepository : IOrmRepository<PlaceSpecialDayTimeMappings, PlaceSpecialDayTimeMappings>
    {
        PlaceSpecialDayTimeMappings Get(long id);

        IEnumerable<PlaceSpecialDayTimeMappings> GetAllByEventId(long id);
    }

    public class PlaceSpecialDayTimeMappingsRepository : BaseLongOrmRepository<PlaceSpecialDayTimeMappings>, IPlaceSpecialDayTimeMappingsRepository
    {
        public PlaceSpecialDayTimeMappingsRepository(IDataSettings dataSettings)
            : base(dataSettings)
        {
        }

        public PlaceSpecialDayTimeMappings Get(long id)
        {
            return Get(new PlaceSpecialDayTimeMappings { Id = id });
        }

        public IEnumerable<PlaceSpecialDayTimeMappings> GetAllByEventId(long id)
        {
            var placeWeekOpen = GetAll(statement => statement
                .Where($"{nameof(PlaceSpecialDayTimeMappings.EventId):C} = @Ids")
                .WithParameters(new { Ids = id }));
            return placeWeekOpen;
        }
    }
}