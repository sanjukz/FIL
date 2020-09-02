using FIL.Api.Repositories;
using FIL.Contracts.Enums;
using FIL.Contracts.Queries.EventLearnPage;
using FIL.Contracts.QueryResults.EventLearnPage;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.EventLearnPage
{
    public class EventLearnPageQueryHandler : IQueryHandler<EventLearnPageQuery, WebEventLearnPageQueryResults>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventCategoryRepository _eventCategoryRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRatingRepository _ratingRepository;
        private readonly IEventGalleryImageRepository _eventGalleryImageRepository;

        public EventLearnPageQueryHandler(IEventRepository eventRepository,
            IEventCategoryRepository eventCategoryRepository,
            IEventDetailRepository eventDetailRepository,
            IVenueRepository venueRepository,
            ICountryRepository countryRepository,
            ICityRepository cityRepository,
            IStateRepository stateRepository,
            IUserRepository userRepository,
            IRatingRepository ratingRepository,
            IEventGalleryImageRepository eventGalleryImageRepository)
        {
            _eventRepository = eventRepository;
            _eventCategoryRepository = eventCategoryRepository;
            _eventDetailRepository = eventDetailRepository;
            _venueRepository = venueRepository;
            _cityRepository = cityRepository;
            _countryRepository = countryRepository;
            _userRepository = userRepository;
            _ratingRepository = ratingRepository;
            _stateRepository = stateRepository;
            _eventGalleryImageRepository = eventGalleryImageRepository;
        }

        public WebEventLearnPageQueryResults Handle(EventLearnPageQuery query)
        {
            var eventDataModel = _eventRepository.GetByAltId(query.EventAltId);
            var eventModel = AutoMapper.Mapper.Map<Contracts.Models.Event>(eventDataModel);
            if (eventModel != null)
            {
                var eventDetailDataModel = _eventDetailRepository.GetAllByEventId(eventDataModel.Id);
                var eventDetailModel = AutoMapper.Mapper.Map<List<Contracts.Models.EventDetail>>(eventDetailDataModel);

                var venueDataModel = _venueRepository.GetByVenueIds(eventDetailDataModel.Select(s => s.VenueId).Distinct());
                var venueModel = AutoMapper.Mapper.Map<List<Contracts.Models.Venue>>(venueDataModel);

                var cityDataModel = _cityRepository.GetByCityIds(venueDataModel.Select(s => s.CityId));
                var cityModel = AutoMapper.Mapper.Map<List<Contracts.Models.City>>(cityDataModel);

                var stateDataModel = _stateRepository.GetByStateIds(cityDataModel.Select(s => s.StateId));
                var stateModel = AutoMapper.Mapper.Map<List<Contracts.Models.State>>(stateDataModel);

                var countryDataModel = _countryRepository.GetByCountryIds(stateDataModel.Select(s => s.CountryId));
                var countryModel = AutoMapper.Mapper.Map<List<Contracts.Models.Country>>(countryDataModel);

                var eventGallaryDataModel = _eventGalleryImageRepository.GetByEventId(eventModel.Id);
                var galleryDataModel = AutoMapper.Mapper.Map<List<Contracts.Models.EventGalleryImage>>(eventGallaryDataModel);

                var ratingDataModel = _ratingRepository.GetByEventId(eventDataModel.Id);
                var ratingModel = AutoMapper.Mapper.Map<List<Contracts.Models.Rating>>(ratingDataModel);

                var userdataModel = _userRepository.GetByUserIds(ratingModel.Select(s => s.UserId).Distinct());
                var userModel = AutoMapper.Mapper.Map<List<Contracts.Models.UserProfile>>(userdataModel);

                var eventCategoryDataModel = _eventCategoryRepository.Get(eventModel.EventCategoryId);
                var eventCategoryModel = AutoMapper.Mapper.Map<Contracts.Models.EventCategory>(eventCategoryDataModel);

                return new WebEventLearnPageQueryResults
                {
                    EventType = (EventType)eventModel.EventTypeId != 0 ? (EventType)eventModel.EventTypeId : EventType.Regular,
                    EventCategory = eventCategoryModel,
                    Event = eventModel,
                    EventDetail = eventDetailModel,
                    Venue = venueModel,
                    City = cityModel,
                    Rating = ratingModel,
                    User = userModel,
                    State = stateModel,
                    Country = countryModel,
                    EventGalleryImage = galleryDataModel
                };
            }
            else
            {
                return new WebEventLearnPageQueryResults
                {
                };
            }
        }
    }
}