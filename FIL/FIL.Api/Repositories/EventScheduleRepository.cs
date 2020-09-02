using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IEventScheduleRepository : IOrmRepository<EventSchedule, EventSchedule>
    {
        EventSchedule Get(long id);

        IEnumerable<EventSchedule> GetAllByEventId(long EventId);
    }

    public class EventScheduleRepository : BaseLongOrmRepository<EventSchedule>, IEventScheduleRepository
    {
        public EventScheduleRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public EventSchedule Get(long id)
        {
            return Get(new EventSchedule { Id = id });
        }

        public IEnumerable<EventSchedule> GetAllByEventId(long EventId)
        {
            return GetAll(s => s.Where($"{nameof(EventSchedule.EventId):C} = @Id")
               .WithParameters(new { Id = EventId }));
        }
    }
}