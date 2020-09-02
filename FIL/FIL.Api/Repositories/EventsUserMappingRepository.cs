using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IEventsUserMappingRepository : IOrmRepository<EventsUserMapping, EventsUserMapping>
    {
        EventsUserMapping Get(long id);

        IEnumerable<EventsUserMapping> GetByUserId(long userId);

        IEnumerable<EventsUserMapping> GetByUserIdAndEventId(long userId, long eventId);

        IEnumerable<EventsUserMapping> GetByUserIdAndEventIds(long userId, List<long> eventId);
    }

    public class EventsUserMappingRepository : BaseLongOrmRepository<EventsUserMapping>, IEventsUserMappingRepository
    {
        public EventsUserMappingRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public EventsUserMapping Get(long id)
        {
            return Get(new EventsUserMapping { Id = id });
        }

        public IEnumerable<EventsUserMapping> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteEventsUserMapping(EventsUserMapping EventsUserMapping)
        {
            Delete(EventsUserMapping);
        }

        public EventsUserMapping SaveEventsUserMapping(EventsUserMapping EventsUserMapping)
        {
            return Save(EventsUserMapping);
        }

        public IEnumerable<EventsUserMapping> GetByUserId(long userId)
        {
            return GetAll(statement => statement
              .Where($"{nameof(EventsUserMapping.UserId):C} = @UserId AND IsEnabled=1")
              .WithParameters(new { UserId = userId }));
        }

        public IEnumerable<EventsUserMapping> GetByUserIdAndEventId(long userId, long eventId)
        {
            return GetAll(statement => statement
               .Where($"{nameof(EventsUserMapping.UserId):C} = @UserId AND { nameof(EventsUserMapping.EventId):C} = @EventId")
               .WithParameters(new { UserId = userId, EventId = eventId }));
        }

        public IEnumerable<EventsUserMapping> GetByUserIdAndEventIds(long userId, List<long> eventId)
        {
            return GetAll(statement => statement
               .Where($"{nameof(EventsUserMapping.UserId):C} = @UserId AND { nameof(EventsUserMapping.EventId):C} IN @EventId")
               .WithParameters(new { UserId = userId, EventId = eventId }));
        }
    }
}