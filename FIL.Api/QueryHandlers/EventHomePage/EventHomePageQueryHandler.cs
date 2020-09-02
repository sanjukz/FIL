using FIL.Api.Repositories;
using FIL.Contracts.Enums;
using FIL.Contracts.Queries.EventHomePage;
using FIL.Contracts.QueryResults.EventHomePage;

namespace FIL.Api.QueryHandlers.EventHomePage
{
    public class EventHomePageQueryHandler : IQueryHandler<EventHomePageQuery, EventHomePageQueryResult>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventCategoryRepository _eventCategoryRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICountryRepository _countryRepository;

        public EventHomePageQueryHandler(IEventRepository eventRepository,
            IEventCategoryRepository eventCategoryRepository,
            IEventDetailRepository eventDetailRepository,
            IVenueRepository venueRepository,
            ICountryRepository countryRepository,
            ICityRepository cityRepository,
            IStateRepository stateRepository)
        {
            _eventRepository = eventRepository;
            _eventCategoryRepository = eventCategoryRepository;
            _eventDetailRepository = eventDetailRepository;
            _venueRepository = venueRepository;
            _cityRepository = cityRepository;
            _countryRepository = countryRepository;
            _stateRepository = stateRepository;
        }

        public EventHomePageQueryResult Handle(EventHomePageQuery query)
        {
            var eventDataModel = _eventRepository.GetByAltId(query.EventAltId);
            var eventModel = AutoMapper.Mapper.Map<Contracts.Models.Event>(eventDataModel);

            var eventDetailDataModel = _eventDetailRepository.GetByEventId(eventDataModel.Id);
            var eventDetailModel = AutoMapper.Mapper.Map<Contracts.Models.EventDetail>(eventDetailDataModel);

            var venueDataModel = _venueRepository.Get(eventDetailDataModel.VenueId);
            var venueModel = AutoMapper.Mapper.Map<Contracts.Models.Venue>(venueDataModel);

            var cityDataModel = _cityRepository.Get(venueDataModel.CityId);
            var cityModel = AutoMapper.Mapper.Map<Contracts.Models.City>(cityDataModel);

            var stateDataModel = _stateRepository.Get(cityDataModel.StateId);
            var stateModel = AutoMapper.Mapper.Map<Contracts.Models.State>(stateDataModel);

            var countryDataModel = _countryRepository.Get(stateDataModel.CountryId);
            var countryModel = AutoMapper.Mapper.Map<Contracts.Models.Country>(countryDataModel);

            var eventCategoryDataModel = _eventCategoryRepository.Get(eventModel.EventCategoryId);
            var eventCategoryModel = AutoMapper.Mapper.Map<Contracts.Models.EventCategory>(eventCategoryDataModel);

            return new EventHomePageQueryResult
            {
                EventType = (EventType)eventModel.EventTypeId,
                EventCategory = eventCategoryModel,
                Event = eventModel,
                EventDetail = eventDetailModel,
                Venue = venueModel,
                City = cityModel,
                State = stateModel,
                Country = countryModel
            };
        }
    }
}