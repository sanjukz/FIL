using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IEventBannerMappingRepository : IOrmRepository<EventBannerMapping, EventBannerMapping>
    {
        EventBannerMapping Get(int id);

        IEnumerable<EventBannerMapping> GetBySiteId(Site siteId);
    }

    public class EventBannerMappingRepository : BaseOrmRepository<EventBannerMapping>, IEventBannerMappingRepository
    {
        public EventBannerMappingRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public EventBannerMapping Get(int id)
        {
            return Get(new EventBannerMapping { Id = id });
        }

        public IEnumerable<EventBannerMapping> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteEventBanner(EventBannerMapping eventBanner)
        {
            Delete(eventBanner);
        }

        public EventBannerMapping SaveEventBanner(EventBannerMapping eventBanner)
        {
            return Save(eventBanner);
        }

        public IEnumerable<EventBannerMapping> GetBySiteId(Site siteId)
        {
            var eventBannerList = (GetAll(s => s.Where($"{nameof(EventBannerMapping.SiteId):C}=@SiteId AND IsEnabled = 1")
               .WithParameters(new { SiteId = siteId })
             ));
            return eventBannerList;
        }
    }
}