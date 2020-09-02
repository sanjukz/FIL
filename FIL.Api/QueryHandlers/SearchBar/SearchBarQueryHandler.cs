using FIL.Api.Repositories;
using FIL.Contracts.Queries.SearchBar;
using FIL.Contracts.QueryResults.SearchBar;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.SearchBar
{
    public class SearchBarQueryHandler : IQueryHandler<SearchBarQuery, SearchBarQueryResults>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IEventDetailRepository _eventDetailRepository;

        public SearchBarQueryHandler(IEventRepository eventRepository,
            IVenueRepository venueRepository,
            ICityRepository cityRepository,
            IEventDetailRepository eventDetailRepository)
        {
            _eventRepository = eventRepository;
            _venueRepository = venueRepository;
            _cityRepository = cityRepository;
            _eventDetailRepository = eventDetailRepository;
        }

        public SearchBarQueryResults Handle(SearchBarQuery searchBarQuery)
        {
            var citytDataModel = _cityRepository.SearchByCityName(searchBarQuery.Search);
            var venueByCityDataModel = _venueRepository.GetByCityIds(citytDataModel.Select(s => s.Id).ToList());
            var venueDataModel = _venueRepository.GetByVenueName(searchBarQuery.Search);
            var venueIds = venueByCityDataModel.Select(s => s.Id).ToList();
            venueIds.AddRange(venueDataModel.Select(s => s.Id));
            var eventDetails = _eventDetailRepository.GetByVenueds(venueIds.Distinct());
            var eventDataModel = _eventRepository.GetByNameAndEventId(searchBarQuery.Search, eventDetails.Select(s => s.EventId).Distinct());
            var searchedEventDetails = eventDataModel.Where(ed => ed.IsFeel == false);
            var eventModel = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.Event>>(searchedEventDetails);

            return new SearchBarQueryResults
            {
                Events = eventModel
            };
        }
    }
}