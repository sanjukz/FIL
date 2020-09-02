using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.FeelCustomDefaultContent;
using FIL.Contracts.QueryResults.FeelCustomDefaultContent;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.FeelCustomDefaultContent
{
    public class FeelCustomDefaultContentQueryHandler : IQueryHandler<FeelCustomDefaultContentQuery, FeelCustomDefaultContentQueryResult>
    {
        private readonly IEventBannerMappingRepository _eventBannerMappingRepository;
        private readonly IEventSiteContentMappingRepository _eventSiteContentMappingRepository;
        private readonly IEventSiteIdMappingRepository _eventSiteIdMappingRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IVenueRepository _venueRepository;

        public FeelCustomDefaultContentQueryHandler(IEventSiteIdMappingRepository eventSiteIdMappingRepository,
            ICountryRepository countryRepository, IStateRepository stateRepository, ICityRepository cityRepository, IEventRepository eventRepository, IEventDetailRepository eventDetailRepository, IVenueRepository venueRepository, IEventSiteContentMappingRepository eventSiteContentMappingRepository, IEventBannerMappingRepository eventBannerMappingRepository)
        {
            _eventSiteIdMappingRepository = eventSiteIdMappingRepository;
            _countryRepository = countryRepository;
            _stateRepository = stateRepository;
            _cityRepository = cityRepository;
            _eventRepository = eventRepository;
            _eventDetailRepository = eventDetailRepository;
            _venueRepository = venueRepository;
            _eventSiteContentMappingRepository = eventSiteContentMappingRepository;
            _eventBannerMappingRepository = eventBannerMappingRepository;
        }

        public FeelCustomDefaultContentQueryResult Handle(FeelCustomDefaultContentQuery query)
        {
            var siteBannerModel = _eventBannerMappingRepository.GetBySiteId(query.SiteId);
            var siteBanners = AutoMapper.Mapper.Map<List<SiteBannerDetail>>(siteBannerModel);
            var siteContentResult = _eventSiteContentMappingRepository.GetBySiteId(query.SiteId);
            var siteContent = AutoMapper.Mapper.Map<FIL.Contracts.Models.FeelSiteContent>(siteContentResult);
            var venueDetails = _venueRepository.GetBySiteId((int)query.SiteId);
            List<FIL.Contracts.Models.City> cityResult = new List<FIL.Contracts.Models.City>();
            List<FIL.Contracts.Models.State> stateResult = new List<FIL.Contracts.Models.State>();
            List<FIL.Contracts.Models.Country> countryResult = new List<FIL.Contracts.Models.Country>();
            var cityIds = venueDetails.Select(v => v.CityId).Distinct();
            var cityDetails = _cityRepository.GetByCityIds(cityIds);
            var stateIds = cityDetails.Select(c => c.StateId).Distinct();
            var stateDetails = _stateRepository.GetByStateIds(stateIds);
            var countryIds = stateDetails.Select(c => c.CountryId).Distinct();
            var countryDetails = _countryRepository.GetByCountryIds(countryIds).GroupBy(p => p.CountryName).Select(grp => grp.FirstOrDefault());

            if (siteContent != null)
            {
                if (siteContent.SiteLevel == Contracts.Enums.SiteLevel.Global)
                {
                    cityResult = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.City>>(cityDetails);
                    stateResult = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.State>>(stateDetails);
                    countryResult = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.Country>>(countryDetails);
                }
                else if (siteContent.SiteLevel == Contracts.Enums.SiteLevel.Country)
                {
                    cityResult = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.City>>(cityDetails);
                    stateResult = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.State>>(stateDetails);
                }
                else if (siteContent.SiteLevel == Contracts.Enums.SiteLevel.State)
                {
                    cityResult = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.City>>(cityDetails);
                }
            }

            return new FeelCustomDefaultContentQueryResult
            {
                SiteBanners = siteBanners,
                FeelSiteContent = siteContent,
                Countries = countryResult,
                States = stateResult,
                Cities = cityResult,
            };
        }
    }
}