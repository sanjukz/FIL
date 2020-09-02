using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IEventVenueMappingRepository : IOrmRepository<EventVenueMapping, EventVenueMapping>
    {
        EventVenueMapping Get(int id);

        EventVenueMapping GetByVenueId(int venueId);

        IEnumerable<EventVenueMapping> GetByEventId(long eventId);

        EventVenueMapping GetOneByEventId(long eventId);

        EventVenueMapping GetByEventIdAndVenueId(long eventId, int venueId);
    }

    public class EventVenueMappingRepository : BaseOrmRepository<EventVenueMapping>, IEventVenueMappingRepository
    {
        public EventVenueMappingRepository(IDataSettings dataSettings)
            : base(dataSettings)
        {
        }

        public EventVenueMapping Get(int Id)
        {
            return Get(new EventVenueMapping { Id = Id });
        }

        public EventVenueMapping GetByVenueId(int venueId)
        {
            return GetAll(s => s.Where($"{nameof(EventVenueMapping.VenueId):C} = @VenueIdParam")
                .WithParameters(new { VenueIdParam = venueId })).FirstOrDefault();
        }

        public IEnumerable<EventVenueMapping> GetByEventId(long eventId)
        {
            return GetAll(s => s.Where($"{nameof(EventVenueMapping.EventId):C} = @EventIdParam").WithParameters(new { EventIdParam = eventId }));
        }

        public EventVenueMapping GetOneByEventId(long eventId)
        {
            return GetAll(s => s.Where($"{nameof(EventVenueMapping.EventId):C} = @EventIdParam").WithParameters(new { EventIdParam = eventId })).FirstOrDefault();
        }

        public EventVenueMapping GetByEventIdAndVenueId(long eventId, int venueId)
        {
            return GetAll(s => s.Where($"{nameof(EventVenueMapping.EventId):C} = @EventIdParam AND {nameof(EventVenueMapping.VenueId): C} = @VenueIdParam").WithParameters(new { EventIdParam = eventId, VenueIdParam = venueId })).FirstOrDefault();
        }

        public EventVenueMapping SaveEventVenueMapping(EventVenueMapping eventVenueMapping)
        {
            return Save(eventVenueMapping);
        }

        public void DeleteeventVenueMapping(EventVenueMapping eventVenueMapping)
        {
            Delete(eventVenueMapping);
        }
    }
}