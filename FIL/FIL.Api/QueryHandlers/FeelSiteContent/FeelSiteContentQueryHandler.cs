using AutoMapper;
using FIL.Api.Repositories;
using FIL.Contracts.Queries.FeelSiteContent;
using FIL.Contracts.QueryResults.FeelSiteContent;

namespace FIL.Api.QueryHandlers.FeelSiteContent
{
    public class FeelSiteContentQueryHandler : IQueryHandler<FeelSiteContentQuery, FeelSiteContentQueryResult>
    {
        private readonly IEventSiteContentMappingRepository _eventSiteContentMappingRepository;

        public FeelSiteContentQueryHandler(IEventSiteContentMappingRepository eventSiteContentMappingRepository)
        {
            _eventSiteContentMappingRepository = eventSiteContentMappingRepository;
        }

        public FeelSiteContentQueryResult Handle(FeelSiteContentQuery query)
        {
            var siteContentResult = _eventSiteContentMappingRepository.GetBySiteId(query.SiteId);
            var siteContent = Mapper.Map<FIL.Contracts.Models.FeelSiteContent>(siteContentResult);
            return new FeelSiteContentQueryResult
            {
                FeelSiteContent = siteContent,
            };
        }
    }
}