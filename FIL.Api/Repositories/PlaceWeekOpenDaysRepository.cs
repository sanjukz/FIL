using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IPlaceWeekOpenDaysRepository : IOrmRepository<PlaceWeekOpenDays, PlaceWeekOpenDays>
    {
        PlaceWeekOpenDays Get(long id);

        IEnumerable<PlaceWeekOpenDays> GetByEventId(long eventId);

        PlaceWeekOpenDays GetByEventIdandDayId(long eventId, long dayId);

        IEnumerable<PlaceWeekOpenDays> GetByEventIds(List<long> eventIds);
    }

    public class PlaceWeekOpenDaysRepository : BaseLongOrmRepository<PlaceWeekOpenDays>, IPlaceWeekOpenDaysRepository
    {
        public PlaceWeekOpenDaysRepository(IDataSettings dataSettings)
            : base(dataSettings)
        {
        }

        public PlaceWeekOpenDays Get(long id)
        {
            return Get(new PlaceWeekOpenDays { Id = id });
        }

        public IEnumerable<PlaceWeekOpenDays> GetByEventId(long eventId)
        {
            return GetAll(statement => statement
                .Where($"{nameof(PlaceWeekOpenDays.EventId):C}=@EventId")
                .WithParameters(new { EventId = eventId }));
        }

        public IEnumerable<PlaceWeekOpenDays> GetByEventIds(List<long> eventIds)
        {
            return GetAll(statement => statement
                .Where($"{nameof(PlaceWeekOpenDays.EventId):C} IN @EventId")
                .WithParameters(new { EventId = eventIds }));
        }

        public PlaceWeekOpenDays GetByEventIdandDayId(long eventId, long dayId)
        {
            return GetAll(statement => statement
                .Where($"{nameof(PlaceWeekOpenDays.EventId):C}=@EventId AND {nameof(PlaceWeekOpenDays.DayId):C} = @DayId")
                .WithParameters(new { EventId = eventId, DayId = dayId })).FirstOrDefault();
        }
    }
}