using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IFeaturedEventRepository : IOrmRepository<FeaturedEvent, FeaturedEvent>
    {
        FeaturedEvent Get(int id);

        FeaturedEvent GetByEventId(long id);

        FeaturedEvent GetByEventIdAndSiteId(long eventId, Site siteId);

        IEnumerable<FeaturedEvent> GetBySiteIds(Site siteId);
    }

    public class FeaturedEventRepository : BaseOrmRepository<FeaturedEvent>, IFeaturedEventRepository
    {
        public FeaturedEventRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public FeaturedEvent Get(int id)
        {
            return Get(new FeaturedEvent { Id = id });
        }

        public IEnumerable<FeaturedEvent> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteFeaturedEvent(FeaturedEvent featuredEvent)
        {
            Delete(featuredEvent);
        }

        public FeaturedEvent SaveFeaturedEvent(FeaturedEvent featuredEvent)
        {
            return Save(featuredEvent);
        }

        public FeaturedEvent GetByEventId(long id)
        {
            var featuredEvent = (GetAll(s => s.Where($"{nameof(FeaturedEvent.EventId):C}=@EventId")
                                          .WithParameters(new { EventId = id })).FirstOrDefault());

            return featuredEvent;
        }

        public FeaturedEvent GetByEventIdAndSiteId(long eventId, Site siteId)
        {
            var featuredEvent = (GetAll(s => s.Where($"{nameof(FeaturedEvent.EventId):C}=@EventId AND {nameof(FeaturedEvent.SiteId):C} = @SiteId")
                                           .WithParameters(new { EventId = eventId, SiteId = siteId })
                                           ).FirstOrDefault());
            return featuredEvent;
        }

        public IEnumerable<FeaturedEvent> GetBySiteIds(Site siteId)
        {
            var featuredEventList = (GetAll(s => s.Where($"{nameof(FeaturedEvent.SiteId):C}=@SiteId AND IsEnabled = 1")
                                           .WithParameters(new { SiteId = siteId })
             ));
            return featuredEventList;
        }
    }
}