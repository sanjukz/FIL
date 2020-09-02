using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IEventSiteIdMappingRepository : IOrmRepository<EventSiteIdMapping, EventSiteIdMapping>
    {
        EventSiteIdMapping Get(int id);

        IEnumerable<EventSiteIdMapping> GetBySiteId(Site siteId);

        EventSiteIdMapping GetByEventId(long eventId);

        IEnumerable<EventSiteIdMapping> GetAllByEventId(long eventId);
    }

    public class EventSiteIdMappingRepository : BaseOrmRepository<EventSiteIdMapping>, IEventSiteIdMappingRepository
    {
        public EventSiteIdMappingRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public EventSiteIdMapping Get(int id)
        {
            return Get(new EventSiteIdMapping { Id = id });
        }

        public IEnumerable<EventSiteIdMapping> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteSiteEvent(EventSiteIdMapping siteEvent)
        {
            Delete(siteEvent);
        }

        public EventSiteIdMapping SaveSiteEvent(EventSiteIdMapping siteEvent)
        {
            return Save(siteEvent);
        }

        public IEnumerable<EventSiteIdMapping> GetBySiteId(Site siteId)
        {
            var siteEventList = (GetAll(s => s.Where($"{nameof(EventSiteIdMapping.SiteId):C}=@SiteId AND IsEnabled = 1")
              .WithParameters(new { SiteId = siteId })
             ));
            return siteEventList;
        }

        public EventSiteIdMapping GetByEventId(long eventId)
        {
            return GetAll(s => s.Where($"{nameof(EventSiteIdMapping.EventId):C} = @EventId")
                .WithParameters(new { EventId = eventId })
            ).FirstOrDefault();
        }

        public IEnumerable<EventSiteIdMapping> GetAllByEventId(long eventId)
        {
            return GetAll(s => s.Where($"{nameof(EventSiteIdMapping.EventId):C} = @EventId")
                .WithParameters(new { EventId = eventId })
            );
        }
    }
}