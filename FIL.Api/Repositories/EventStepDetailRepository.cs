using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IEventStepDetailRepository : IOrmRepository<EventStepDetail, EventStepDetail>
    {
        EventStepDetail Get(int id);

        EventStepDetail GetByEventId(long eventId);
    }

    public class EventStepDetailRepository : BaseOrmRepository<EventStepDetail>, IEventStepDetailRepository
    {
        public EventStepDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public EventStepDetail Get(int id)
        {
            return Get(new EventStepDetail { Id = id });
        }

        public EventStepDetail GetByEventId(long eventId)
        {
            return GetAll(s => s.Where($"{nameof(EventStepDetail.EventId):C} = @Id")
               .WithParameters(new { Id = eventId })
           ).FirstOrDefault();
        }
    }
}