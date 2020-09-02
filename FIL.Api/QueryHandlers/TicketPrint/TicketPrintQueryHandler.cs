using FIL.Api.Repositories;
using FIL.Contracts.Queries.TicketPrint;
using FIL.Contracts.QueryResults.TicketPrint;

namespace FIL.Api.QueryHandlers.TicketPrint
{
    public class TicketPrintQueryHandler : IQueryHandler<TicketPrintQuery, TicketPrintQueryResult>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IEventAttributeRepository _eventAttributeRepository;
        private readonly IEntryGateRepository _entryGateRepository;
        private readonly IMatchLayoutSectionRepository _matchLayoutSectionRepository;
        private readonly IMatchLayoutSectionSeatRepository _matchLayoutSectionSeatRepository;
        private readonly IMatchSeatTicketDetailRepository _matchSeatTicketDetailRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionDetailRepository _transactionDetailRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly ICurrencyTypeRepository _currencyTypeRepository;

        public TicketPrintQueryHandler(IEventRepository eventRepository,
         IEventDetailRepository eventDetailRepository,
         IVenueRepository venueRepository,
         ICityRepository cityRepository,
         IEventAttributeRepository eventAttributeRepository,
         IEntryGateRepository entryGateRepository,
         IMatchLayoutSectionRepository matchLayoutSectionRepository,
         IMatchLayoutSectionSeatRepository matchLayoutSectionSeatRepository,
         IMatchSeatTicketDetailRepository matchSeatTicketDetailRepository,
         ITransactionRepository transactionRepository,
         ITransactionDetailRepository transactionDetailRepository,
         IEventTicketDetailRepository eventTicketDetailRepository,
         ICurrencyTypeRepository currencyTypeRepository)

        {
            _eventRepository = eventRepository;
            _eventDetailRepository = eventDetailRepository;
            _venueRepository = venueRepository;
            _cityRepository = cityRepository;
            _eventAttributeRepository = eventAttributeRepository;
            _entryGateRepository = entryGateRepository;
            _matchLayoutSectionRepository = matchLayoutSectionRepository;
            _matchLayoutSectionSeatRepository = matchLayoutSectionSeatRepository;
            _matchSeatTicketDetailRepository = matchSeatTicketDetailRepository;
            _transactionRepository = transactionRepository;
            _transactionDetailRepository = transactionDetailRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _currencyTypeRepository = currencyTypeRepository;
        }

        public TicketPrintQueryResult Handle(TicketPrintQuery query)
        {
            //var matchSeatTicketDetails=_matchSeatTicketDetailRepository.GetByTransactionId(query.TranscationId)
            //var ticketContainer = tEventDetail.Select(edetail => {
            //    var tEventDetails = _eventDetailRepository.GetByEventIdAndEventDetailIds(eId.Id, edetail.Id);
            //    var tVenue = _venueRepository.Get(tEventDetails.VenueId);
            //    var tCity = _cityRepository.Get(tVenue.CityId);
            //    var tState = _stateRepository.Get(tCity.StateId);
            //    var tCountry = _countryRepository.Get(tState.CountryId);
            //    var tEventTicketDetail = _eventTicketDetailRepository.GetByEventDetailIds(tEventDetail.Select(s => s.Id));
            //    var tEventTicketAttribute = _eventTicketAttributeRepository.GetByEventTicketDetailIds(tEventTicketDetail.Select(s => s.Id)).Distinct();
            //    var tTicketCategory = _ticketCategoryRepository.GetByTicketCategoryIds(tEventTicketDetail.Select(s => s.TicketCategoryId));

            //    var tTransactionDetail = _transactionDetailsRepository.GetByEventTicketAttributeandTransactionId(tEventTicketAttribute.Select(s => s.Id), transactionModel.Id).Distinct();

            //    var tTransactionSeatDetail = _transactionSeatDetailRepository.GetByTransactionDetailIds(tTransactionDetail.Select(s => s.Id));

            //    var tMatchSeatTicketDetails = _matchSeatTicketDetailRepository.GetbyMatchSeatTicketDetailId(tTransactionSeatDetail.Select(s => s.Id));

            //    var tMatchLayoutSectionSeats = _matchLayoutSectionSeatRepository.GetByIds(tMatchSeatTicketDetails.Select(s => s.MatchLayoutSectionSeatId));

            //    var tEventDeliveryTypeDetail = _eventDeliveryTypeDetailRepository.GetByEventDetailIds(tEventDetail.Select(s => s.Id));

            //    var tTransactionDeliveryDetail = _transactionDeliveryDetailRepository.GetByTransactionDetailIds(tTransactionDetail.Select(s => s.Id));
            //    return new TicketContainer
            //    {
            //        Event = AutoMapper.Mapper.Map<Event>(tEvent),
            //        EventDetail = AutoMapper.Mapper.Map<EventDetail>(tEventDetails),
            //        Venue = AutoMapper.Mapper.Map<Venue>(tVenue),
            //        City = AutoMapper.Mapper.Map<City>(tCity),
            //        State = AutoMapper.Mapper.Map<State>(tState),
            //        Country = AutoMapper.Mapper.Map<Country>(tCountry),
            //        EventTicketDetail = AutoMapper.Mapper.Map<IEnumerable<EventTicketDetail>>(tEventTicketDetail),
            //        EventTicketAttribute = AutoMapper.Mapper.Map<IEnumerable<EventTicketAttribute>>(tEventTicketAttribute),
            //        TicketCategory = AutoMapper.Mapper.Map<IEnumerable<Kz.Contracts.Models.TicketCategory>>(tTicketCategory),
            //        TransactionDetail = AutoMapper.Mapper.Map<IEnumerable<TransactionDetail>>(tTransactionDetail),
            //        TransactionSeatDetail = AutoMapper.Mapper.Map<IEnumerable<TransactionSeatDetail>>(tTransactionSeatDetail),
            //        MatchSeatTicketDetail = AutoMapper.Mapper.Map<IEnumerable<MatchSeatTicketDetail>>(tMatchSeatTicketDetails),
            //        MatchLayoutSectionSeat = AutoMapper.Mapper.Map<IEnumerable<MatchLayoutSectionSeat>>(tMatchLayoutSectionSeats),
            //        EventDeliveryTypeDetail = AutoMapper.Mapper.Map<IEnumerable<EventDeliveryTypeDetail>>(tEventDeliveryTypeDetail),
            //        TransactionDeliveryDetail = AutoMapper.Mapper.Map<IEnumerable<TransactionDeliveryDetail>>(tTransactionDeliveryDetail)

            //    };
            //});

            return new TicketPrintQueryResult
            {
            };
        }
    }
}