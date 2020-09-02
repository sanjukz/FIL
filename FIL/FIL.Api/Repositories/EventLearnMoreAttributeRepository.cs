using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IEventLearnMoreAttributeRepository : IOrmRepository<EventLearnMoreAttribute, EventLearnMoreAttribute>
    {
        EventLearnMoreAttribute Get(int id);

        IEnumerable<EventLearnMoreAttribute> GetByEventId(long eventId);
    }

    public class EventLearnMoreAttributeRepository : BaseOrmRepository<EventLearnMoreAttribute>, IEventLearnMoreAttributeRepository
    {
        public EventLearnMoreAttributeRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public EventLearnMoreAttribute Get(int id)
        {
            return Get(new EventLearnMoreAttribute { Id = id });
        }

        public IEnumerable<EventLearnMoreAttribute> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteEventLearnMoreAttribute(EventLearnMoreAttribute eventLearnMoreAttribute)
        {
            Delete(eventLearnMoreAttribute);
        }

        public EventLearnMoreAttribute SaveEventLearnMoreAttribute(EventLearnMoreAttribute eventLearnMoreAttribute)
        {
            return Save(eventLearnMoreAttribute);
        }

        public IEnumerable<EventLearnMoreAttribute> GetByEventId(long eventId)
        {
            var eventLearnMoreAttributeList = (GetAll(s => s.Where($"{nameof(EventLearnMoreAttribute.EventId):C}=@EventId")
               .WithParameters(new { EventId = eventId })
             ));
            return eventLearnMoreAttributeList;
        }
    }
}