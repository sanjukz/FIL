using FIL.Api.Repositories;
using FIL.Contracts.Enums;
using FIL.Contracts.Models;
using FIL.Contracts.QueryResults.EventLearnPage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Providers
{
    public interface IEventLearnPageProvider
    {
        EventLearnPageQueryResults GetBySlug(string slug);

        EventLearnPageQueryResults GetByAltId(Guid EventAltId);
    }

    public class EventLeanPageProvider : IEventLearnPageProvider
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventCategoryRepository _eventCategoryRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IEventGalleryImageRepository _eventGalleryImageRepository;
        private readonly IEventCategoryMappingRepository _eventCategoryMappingRepository;

        public EventLeanPageProvider(IEventRepository eventRepository,
            IEventCategoryRepository eventCategoryRepository,
            IEventDetailRepository eventDetailRepository,
            IVenueRepository venueRepository,
            ICountryRepository countryRepository,
            ICityRepository cityRepository,
            IStateRepository stateRepository,
            IEventGalleryImageRepository eventGalleryImageRepository,
            IEventCategoryMappingRepository eventCategoryMappingRepository)
        {
            _eventRepository = eventRepository;
            _eventCategoryRepository = eventCategoryRepository;
            _eventDetailRepository = eventDetailRepository;
            _venueRepository = venueRepository;
            _cityRepository = cityRepository;
            _countryRepository = countryRepository;
            _stateRepository = stateRepository;
            _eventGalleryImageRepository = eventGalleryImageRepository;
            _eventCategoryMappingRepository = eventCategoryMappingRepository;
        }

        public EventLearnPageQueryResults GetBySlug(string slug)
        {
            var eventDataModel = AutoMapper.Mapper.Map<Contracts.Models.Event>(_eventRepository.GetBySlug(slug));
            if (eventDataModel != null)
            {
                EventLearnPageQueryResults eventLearnPageQueryResults = GetEventResult(eventDataModel);
                return new EventLearnPageQueryResults
                {
                    EventType = eventLearnPageQueryResults.EventType,
                    EventCategory = eventLearnPageQueryResults.EventCategory,
                    Event = eventLearnPageQueryResults.Event,
                    EventDetail = eventLearnPageQueryResults.EventDetail,
                    Venue = eventLearnPageQueryResults.Venue,
                    City = eventLearnPageQueryResults.City,
                    State = eventLearnPageQueryResults.State,
                    Country = eventLearnPageQueryResults.Country,
                    EventGalleryImage = eventLearnPageQueryResults.EventGalleryImage
                };
            }
            return new EventLearnPageQueryResults { };
        }

        public EventLearnPageQueryResults GetByAltId(Guid EventAltId)
        {
            var eventDataModel = AutoMapper.Mapper.Map<Contracts.Models.Event>(_eventRepository.GetByAltId(EventAltId));
            if (eventDataModel != null)
            {
                EventLearnPageQueryResults eventLearnPageQueryResults = GetEventResult(eventDataModel);
                return new EventLearnPageQueryResults
                {
                    EventType = eventLearnPageQueryResults.EventType,
                    EventCategory = eventLearnPageQueryResults.EventCategory,
                    Event = eventLearnPageQueryResults.Event,
                    EventDetail = eventLearnPageQueryResults.EventDetail,
                    Venue = eventLearnPageQueryResults.Venue,
                    City = eventLearnPageQueryResults.City,
                    State = eventLearnPageQueryResults.State,
                    Country = eventLearnPageQueryResults.Country,
                    EventGalleryImage = eventLearnPageQueryResults.EventGalleryImage
                };
            }
            return new EventLearnPageQueryResults { };
        }

        public EventLearnPageQueryResults GetEventResult(Event eventDataModel)
        {
            var eventModel = AutoMapper.Mapper.Map<Contracts.Models.Event>(eventDataModel);

            var eventDetailDataModel = _eventDetailRepository.GetByEventId(eventDataModel.Id);
            var eventDetailModel = AutoMapper.Mapper.Map<Contracts.Models.EventDetail>(eventDetailDataModel);
            if (eventDetailModel == null)
            {
                return new EventLearnPageQueryResults { };
            }
            var venueDataModel = _venueRepository.Get(eventDetailDataModel.VenueId);
            var venueModel = AutoMapper.Mapper.Map<Contracts.Models.Venue>(venueDataModel);

            var cityDataModel = _cityRepository.Get(venueDataModel.CityId);
            var cityModel = AutoMapper.Mapper.Map<Contracts.Models.City>(cityDataModel);

            var stateDataModel = _stateRepository.Get(cityDataModel.StateId);
            var stateModel = AutoMapper.Mapper.Map<Contracts.Models.State>(stateDataModel);

            var countryDataModel = _countryRepository.Get(stateDataModel.CountryId);
            var countryModel = AutoMapper.Mapper.Map<Contracts.Models.Country>(countryDataModel);

            var eventGallaryDataModel = _eventGalleryImageRepository.GetByEventId(eventModel.Id);
            var galleryDataModel = AutoMapper.Mapper.Map<List<Contracts.Models.EventGalleryImage>>(eventGallaryDataModel);

            //if (eventModel.EventSourceId == EventSource.ExperienceOz)
            //{
            var eventCategoryIds = new HashSet<int>(_eventCategoryMappingRepository.GetByEventId((int)eventModel.Id)
            .Select(ce => ce.EventCategoryId));
            eventModel.EventCategoryId = (Int16)eventCategoryIds.FirstOrDefault();
            //}

            var eventCategoryDataModel = _eventCategoryRepository.Get(eventModel.EventCategoryId);
            var eventCategoryModel = AutoMapper.Mapper.Map<Contracts.Models.EventCategory>(eventCategoryDataModel);

            return new EventLearnPageQueryResults
            {
                EventType = (EventType)eventModel.EventTypeId,
                EventCategory = eventCategoryModel,
                Event = eventModel,
                EventDetail = eventDetailModel,
                Venue = venueModel,
                City = cityModel,
                State = stateModel,
                Country = countryModel,
                EventGalleryImage = galleryDataModel
            };
        }
    }
}