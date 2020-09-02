using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels.ASI;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IEventTimeSlotMappingRepository : IOrmRepository<EventTimeSlotMapping, EventTimeSlotMapping>
    {
        EventTimeSlotMapping Get(long id);

        EventTimeSlotMapping GetByEventIdandTimeSlotId(long eventId, int timeSlotId);

        IEnumerable<EventTimeSlotMapping> GetByEventId(long eventId);

        IEnumerable<EventTimeSlotMapping> GetByIds(IEnumerable<long> eventTimeSlotIds);
    }

    public class EventTimeSlotMappingRepository : BaseLongOrmRepository<EventTimeSlotMapping>, IEventTimeSlotMappingRepository
    {
        public EventTimeSlotMappingRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public EventTimeSlotMapping Get(long id)
        {
            return Get(new EventTimeSlotMapping { Id = id });
        }

        public IEnumerable<EventTimeSlotMapping> GetByIds(IEnumerable<long> eventTimeSlotIds)
        {
            return GetAll(statement => statement
                .Where($"{nameof(EventTimeSlotMapping.Id):C} IN @Ids")
                .WithParameters(new { Ids = eventTimeSlotIds }));
        }

        public EventTimeSlotMapping GetByEventIdandTimeSlotId(long eventId, int timeSlotId)
        {
            return GetAll(statement => statement
                .Where($"{nameof(EventTimeSlotMapping.EventId):C}=@EventId AND {nameof(EventTimeSlotMapping.TimeSlotId):C} = @TimeSlotId")
                .WithParameters(new { EventId = eventId, TimeSlotId = timeSlotId })).FirstOrDefault();
        }

        public IEnumerable<EventTimeSlotMapping> GetByEventId(long eventId)
        {
            return GetAll(statement => statement
                .Where($"{nameof(EventTimeSlotMapping.EventId):C}=@EventId")
                .WithParameters(new { EventId = eventId }));
        }
    }
}