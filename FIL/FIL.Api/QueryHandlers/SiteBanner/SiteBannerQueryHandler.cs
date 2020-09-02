using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.SiteBanner;
using FIL.Contracts.QueryResults.SiteBanner;
using System.Collections.Generic;

namespace FIL.Api.QueryHandlers.SiteBanner
{
    public class SiteBannerQueryHandler : IQueryHandler<SiteBannerQuery, SiteBannerQueryResult>
    {
        private readonly IEventBannerMappingRepository _eventBannerMappingRepository;

        public SiteBannerQueryHandler(IEventBannerMappingRepository eventBannerMappingRepository)
        {
            _eventBannerMappingRepository = eventBannerMappingRepository;
        }

        public SiteBannerQueryResult Handle(SiteBannerQuery siteBannerQuery)
        {
            var siteBannerModel = _eventBannerMappingRepository.GetBySiteId(siteBannerQuery.SiteId);
            var siteBanners = AutoMapper.Mapper.Map<List<SiteBannerDetail>>(siteBannerModel);
            return new SiteBannerQueryResult
            {
                SiteBanners = siteBanners
            };
        }
    }
}