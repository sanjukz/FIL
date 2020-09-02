using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IEventVenueMappingTimeRepository : IOrmRepository<EventVenueMappingTime, EventVenueMappingTime>
    {
        EventVenueMappingTime GetById(int id);

        IEnumerable<EventVenueMappingTime> GetAllByEventVenueMappingIds(IEnumerable<int> eventVenueMappingId);

        IEnumerable<EventVenueMappingTime> GetAllByEventVenueMappingId(int eventVenueMappingId);
    }

    public class EventVenueMappingTimeRepository : BaseOrmRepository<EventVenueMappingTime>, IEventVenueMappingTimeRepository
    {
        public EventVenueMappingTimeRepository(IDataSettings dataSettings)
            : base(dataSettings)
        {
        }

        public EventVenueMappingTime GetById(int Id)
        {
            return Get(new EventVenueMappingTime { Id = Id });
        }

        public IEnumerable<EventVenueMappingTime> GetAllByEventVenueMappingIds(IEnumerable<int> eventVenueMappingIds)
        {
            return GetAll(s => s.Where($"{nameof(EventVenueMappingTime.EventVenueMappingId):C} IN @EventVenueMappingTimeIds")
                .WithParameters(new { EventVenueMappingTimeIds = eventVenueMappingIds }));
        }

        public IEnumerable<EventVenueMappingTime> GetAllByEventVenueMappingId(int eventVenueMappingId)
        {
            return GetAll(s => s.Where($"{nameof(EventVenueMappingTime.EventVenueMappingId):C} = @EventVenueMappingId")
                .WithParameters(new { EventVenueMappingId = eventVenueMappingId }));
        }

        public EventVenueMappingTime SaveEventVenueMappingTime(EventVenueMappingTime eventVenueMappingTime)
        {
            return Save(eventVenueMappingTime);
        }
    }
}