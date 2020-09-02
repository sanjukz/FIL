using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IEventAttributeRepository : IOrmRepository<EventAttribute, EventAttribute>
    {
        EventAttribute Get(int id);

        EventAttribute GetByEventDetailId(long eventDetailId);

        IEnumerable<EventAttribute> GetByEventDetailIds(IEnumerable<long> eventDetailIds);
    }

    public class EventAttributeRepository : BaseLongOrmRepository<EventAttribute>, IEventAttributeRepository
    {
        public EventAttributeRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public EventAttribute Get(int id)
        {
            return Get(new EventAttribute { Id = id });
        }

        public IEnumerable<EventAttribute> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteEventAttribute(EventAttribute eventAttribute)
        {
            Delete(eventAttribute);
        }

        public EventAttribute SaveEventAttribute(EventAttribute eventAttribute)
        {
            return Save(eventAttribute);
        }

        public EventAttribute GetByEventDetailId(long eventDetailId)
        {
            return GetAll(s => s.Where($"{nameof(EventAttribute.EventDetailId):C}=@EventDetailId")
                .WithParameters(new { EventDetailId = eventDetailId })
            ).FirstOrDefault();
        }

        public IEnumerable<EventAttribute> GetByEventDetailIds(IEnumerable<long> eventDetailIds)
        {
            return GetAll(s => s.Where($"{nameof(EventAttribute.EventDetailId):C} IN @EventDetailIds")
            .WithParameters(new { EventDetailIds = eventDetailIds }));
        }
    }
}