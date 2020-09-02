using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.BoxOffice;
using FIL.Contracts.QueryResults.BoxOffice;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.BoxOffice
{
    public class ReprintRequestQueryHandler : IQueryHandler<ReprintRequestQuery, ReprintRequestQueryResult>
    {
        private readonly IMatchSeatTicketDetailRepository _matchSeatTicketDetailRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly ICityRepository _cityRepository;

        public ReprintRequestQueryHandler(IMatchSeatTicketDetailRepository matchSeatTicketDetailRepository, IEventTicketDetailRepository eventTicketDetailRepository, ITicketCategoryRepository ticketCategoryRepository, IEventDetailRepository eventDetailRepository, IVenueRepository venueRepository, ICityRepository cityRepository)
        {
            _matchSeatTicketDetailRepository = matchSeatTicketDetailRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _eventDetailRepository = eventDetailRepository;
            _venueRepository = venueRepository;
            _cityRepository = cityRepository;
        }

        public ReprintRequestQueryResult Handle(ReprintRequestQuery query)
        {
            var matchSeatTicketDetail = _matchSeatTicketDetailRepository.GetbyTransactionId(query.TransactionId);
            var eventTicketdetailDataModel = _eventTicketDetailRepository.GetAllByEventTicketDetailIds(matchSeatTicketDetail.Select(mstd => mstd.EventTicketDetailId).Distinct()).ToDictionary(etd => etd.Id);
            var ticketCategoriesDataModel = _ticketCategoryRepository.GetByTicketCategoryIds(eventTicketdetailDataModel.Values.Select(tc => tc.TicketCategoryId).Distinct()).ToDictionary(tc => tc.Id);
            var eventDetailDataModel = _eventDetailRepository.GetByIds(eventTicketdetailDataModel.Values.Select(etd => etd.EventDetailId).Distinct()).ToDictionary(ed => ed.Id);
            var venuesDataModel = _venueRepository.GetByVenueIds(eventDetailDataModel.Values.Select(ed => ed.VenueId).Distinct()).ToDictionary(v => v.Id);
            var cityDataModel = _cityRepository.GetByCityIds(venuesDataModel.Values.Select(v => v.Id).Distinct()).ToDictionary(c => c.Id);

            var reprintRequestContainer = matchSeatTicketDetail.Select(cb =>
             {
                 var eventTicketdetail = eventTicketdetailDataModel[cb.EventTicketDetailId];
                 var ticketCategories = ticketCategoriesDataModel[(int)eventTicketdetail.TicketCategoryId];
                 var eventDetails = eventDetailDataModel[eventTicketdetail.EventDetailId];
                 var venues = venuesDataModel[eventDetails.VenueId];
                 var city = cityDataModel[venues.Id];

                 return new ReprintRequestContainer
                 {
                     EventDetail = AutoMapper.Mapper.Map<EventDetail>(eventDetails),
                     City = AutoMapper.Mapper.Map<City>(city),
                     TicketCategory = AutoMapper.Mapper.Map<FIL.Contracts.Models.TicketCategory>(ticketCategories),
                 };
             });
            var matchData = AutoMapper.Mapper.Map<IEnumerable<FIL.Contracts.Models.MatchSeatTicketDetail>>(matchSeatTicketDetail).ToList();
            return new ReprintRequestQueryResult
            {
                MatchSeatTicketDetail = matchData,
                ReprintRequestContainers = reprintRequestContainer.ToList(),
            };
        }
    }
}