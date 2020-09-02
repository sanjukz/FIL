using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IEventSiteContentMappingRepository : IOrmRepository<EventSiteContentMapping, EventSiteContentMapping>
    {
        EventSiteContentMapping Get(int id);

        EventSiteContentMapping GetBySiteId(Site siteId);
    }

    public class EventSiteContentMappingRepository : BaseOrmRepository<EventSiteContentMapping>, IEventSiteContentMappingRepository
    {
        public EventSiteContentMappingRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public EventSiteContentMapping Get(int id)
        {
            return Get(new EventSiteContentMapping { Id = id });
        }

        public IEnumerable<EventSiteContentMapping> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteEventSiteContent(EventSiteContentMapping eventSiteContent)
        {
            Delete(eventSiteContent);
        }

        public EventSiteContentMapping SaveEventSiteContent(EventSiteContentMapping eventSiteContent)
        {
            return Save(eventSiteContent);
        }

        public EventSiteContentMapping GetBySiteId(Site siteId)
        {
            var eventSiteContentList = (GetAll(s => s.Where($"{nameof(EventSiteContentMapping.SiteId):C}=@SiteId AND IsEnabled = 1")
               .WithParameters(new { SiteId = siteId })
             )).FirstOrDefault();
            return eventSiteContentList;
        }
    }
}