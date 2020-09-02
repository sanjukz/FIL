using FIL.Api.Repositories;
using FIL.Contracts.Models.Export;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Providers.Export
{
    public interface IFeelExportIAProvider
    {
        List<FeelExportContainer> Get();
    }

    public class FeelExportIAProvider : IFeelExportIAProvider
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventCategoryRepository _eventCategoryRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly ICurrencyTypeRepository _currencyTypeRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IEventSiteIdMappingRepository _eventSiteIdMappingRepository;

        public FeelExportIAProvider(IEventRepository eventRepository,
            IEventCategoryRepository eventCategoryRepository,
            IEventDetailRepository eventDetailRepository,
            IEventTicketDetailRepository eventTicketDetailRepository,
            IEventTicketAttributeRepository eventTicketAttributeRepository,
            ITicketCategoryRepository ticketCategoryRepository,
            ICurrencyTypeRepository currencyTypeRepository,
            IVenueRepository venueRepository,
            ICityRepository cityRepository,
            IStateRepository stateRepository,
            ICountryRepository countryRepository,
            IEventSiteIdMappingRepository eventSiteIdMappingRepository)
        {
            _eventRepository = eventRepository;
            _eventCategoryRepository = eventCategoryRepository;
            _eventDetailRepository = eventDetailRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _currencyTypeRepository = currencyTypeRepository;
            _venueRepository = venueRepository;
            _cityRepository = cityRepository;
            _stateRepository = stateRepository;
            _countryRepository = countryRepository;
            _eventSiteIdMappingRepository = eventSiteIdMappingRepository;
        }

        public List<FeelExportContainer> Get()
        {
            var eventDataModel = _eventRepository.GetEventsForIA(true);
            var feelExportContainers = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.Export.FeelExportContainer>>(eventDataModel);
            foreach (Contracts.Models.Export.FeelExportContainer item in feelExportContainers)
            {
                var eventCategoryDataModel = _eventCategoryRepository.Get(item.CategoryId);
                var eventCategoryModel = AutoMapper.Mapper.Map<Contracts.Models.EventCategory>(eventCategoryDataModel);

                var eventCategoryParentDataModel = _eventCategoryRepository.Get(eventCategoryModel.EventCategoryId);
                var eventCategoryParentModel = AutoMapper.Mapper.Map<Contracts.Models.EventCategory>(eventCategoryParentDataModel);

                item.ParentImage1 = "https://static1.feelaplace.com/images/places/about/" + item.ParentId.ToString().ToUpper() + "-about.jpg";
                item.ParentUrl = "https://www.feelaplace.com/place/" + eventCategoryParentModel.DisplayName.Replace("&", "and").Replace(" ", "-").ToLower() + "/" + item.Slug.ToString() + "/" + eventCategoryModel.DisplayName.Replace("&", "and").Replace(" ", "-").ToLower();
                item.CategoryName = eventCategoryModel.DisplayName;
                item.ParentCategoryId = eventCategoryParentModel.Id;
                item.ParentCategoryName = eventCategoryParentModel.Category;
            }
            return feelExportContainers.ToList();
            //return eventDataModel.SelectMany(eId =>
            //{
            //    var tEventDetails = _eventDetailRepository.GetByEventId(eId.Id);
            //    if (tEventDetails == null)
            //    {
            //        return new List<FeelExportContainer>();
            //    }
            //    var tEventTicketDetail = _eventTicketDetailRepository.GetByEventDetailId(tEventDetails.Id).ToList();

            //    var tEventTickDetailLookup = tEventTicketDetail
            //        .ToDictionary(etd => etd.Id);
            //    var ticketCategories = _ticketCategoryRepository
            //        .GetByTicketCategoryIds(tEventTicketDetail.Select(etd => etd.TicketCategoryId).Distinct())
            //        .ToDictionary(tc => tc.Id);
            //    var tEventTicketAttribute = _eventTicketAttributeRepository.GetByEventTicketDetailId(tEventTickDetailLookup.Keys);
            //    var tSiteDetail = _eventSiteIdMappingRepository.GetByEventId(tEventDetails.EventId);
            //    var tVenueDetail = _venueRepository.GetByVenueId(tEventDetails.VenueId);
            //    var tCityDetail = _cityRepository.GetByCityId(tVenueDetail.CityId);
            //    var tStateDetail = _stateRepository.GetByStateId(tCityDetail.StateId);
            //    var tCountryDetail = _countryRepository.GetByCountryId(tStateDetail.CountryId);

            //    var eventCategoryDataModel = _eventCategoryRepository.Get(eId.EventCategoryId);
            //    var eventCategoryModel = AutoMapper.Mapper.Map<Contracts.Models.EventCategory>(eventCategoryDataModel);

            //    var eventCategoryParentDataModel = _eventCategoryRepository.Get(eventCategoryModel.EventCategoryId);
            //    var eventCategoryParentModel = AutoMapper.Mapper.Map<Contracts.Models.EventCategory>(eventCategoryParentDataModel);

            //    return tEventTicketAttribute.Select(etd =>
            //    {
            //        var tEventTicketDetails = tEventTickDetailLookup[etd.EventTicketDetailId];
            //        var tTicketCategory = ticketCategories[(int)tEventTicketDetails.TicketCategoryId];

            //        return new FeelExportContainer
            //        {
            //            Id = etd.Id,
            //            SiteId = (int)tSiteDetail.SiteId,
            //            ParentId = eId.AltId,
            //            ParentName = eId.Name,
            //            ParentDescription = eId.Description,
            //            ParentImage1 = "https://static1.feelaplace.com/images/event/AboutEvent/" + eId.AltId.ToString().ToUpper() + "-about.jpg",
            //            ParentUrl = "https://www.feelaplace.com/place/" + eventCategoryParentModel.DisplayName.Replace("&", "and").Replace(" ", "-").ToLower() + "/" + eId.Slug.ToString() + "/" + eventCategoryModel.DisplayName.Replace("&", "and").Replace(" ", "-").ToLower(),
            //            CategoryId = (int)eId.EventCategoryId,
            //            CategoryName = eId.EventCategoryId.ToString(),
            //            ParentCategoryId = eventCategoryParentModel.Id,
            //            ParentCategoryName = eventCategoryParentModel.Category,
            //            Name = tTicketCategory.Name,
            //            Description = etd.TicketCategoryDescription,
            //            Price = etd.Price,
            //            CityId = tCityDetail.Id,
            //            CityName = tCityDetail.Name,
            //            StateId = tStateDetail.Id,
            //            StateName = tStateDetail.Name,
            //            CountryId = tCountryDetail.Id,
            //            CountryName = tCountryDetail.Name
            //        };
            //    });
            //}).ToList();
        }
    }
}