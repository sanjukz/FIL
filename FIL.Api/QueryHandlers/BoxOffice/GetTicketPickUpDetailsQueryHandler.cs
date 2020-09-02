using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.BoxOffice;
using FIL.Contracts.QueryResults.BoxOffice;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.BoxOffice
{
    public class GetTicketPickUpDetailsQueryHandler : IQueryHandler<GetTicketPickUpDetailsQuery, GetTicketPickUpDetailsQueryResult>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionDetailRepository _transactionDetailsRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventAttributeRepository _eventAttributeRepository;
        private readonly IEventRepository _eventRepository;
        private readonly ITransactionDeliveryDetailRepository _transactionDeliveryDetailRepository;
        private readonly ITransactionSeatDetailRepository _transactionSeatDetailRepository;
        private readonly ITransactionPaymentDetailRepository _transactionPaymentDetailRepository;
        private readonly ICurrencyTypeRepository _currencyTypeRepository;
        private readonly IEventDeliveryTypeDetailRepository _eventDeliveryTypeDetailRepository;
        private readonly IUserCardDetailRepository _userCardDetailRepository;
        private readonly IMatchSeatTicketDetailRepository _matchSeatTicketDetailRepository;
        private readonly IMatchLayoutSectionSeatRepository _matchLayoutSectionSeatRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IUserAddressDetailRepository _userAddressDetailRepository;
        private readonly IUserRepository _userRepository;
        private readonly IZipcodeRepository _zipcodeRepository;

        public GetTicketPickUpDetailsQueryHandler(ITransactionRepository transactionRepository,
        ITransactionDetailRepository transactionDetailsRepository,
        IEventTicketDetailRepository eventTicketDetailRepository,
        ITicketCategoryRepository ticketCategoryRepository,
        IEventDetailRepository eventDetailRepository,
        IEventAttributeRepository eventAttributeRepository,
        IEventTicketAttributeRepository eventTicketAttributeRepository,
        IEventRepository eventRepository,
        ITransactionDeliveryDetailRepository transactionDeliveryDetailRepository,
        ITransactionSeatDetailRepository transactionSeatDetailRepository,
        ITransactionPaymentDetailRepository transactionPaymentDetailRepository,
        ICurrencyTypeRepository currencyTypeRepository,
        IEventDeliveryTypeDetailRepository eventDeliveryTypeDetailRepository,
        IUserCardDetailRepository userCardDetailRepository,
        IMatchSeatTicketDetailRepository matchSeatTicketDetailRepository,
        IMatchLayoutSectionSeatRepository matchLayoutSectionSeatRepository,
        IVenueRepository venueRepository,
        ICityRepository cityRepository,
        IStateRepository stateRepository,
        ICountryRepository countryRepository,
        IUserAddressDetailRepository userAddressDetailRepository,
        IUserRepository userRepository,
        IZipcodeRepository zipcodeRepository)
        {
            _transactionRepository = transactionRepository;
            _transactionDetailsRepository = transactionDetailsRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _eventDetailRepository = eventDetailRepository;
            _eventAttributeRepository = eventAttributeRepository;
            _eventRepository = eventRepository;
            _transactionDeliveryDetailRepository = transactionDeliveryDetailRepository;
            _transactionSeatDetailRepository = transactionSeatDetailRepository;
            _transactionPaymentDetailRepository = transactionPaymentDetailRepository;
            _currencyTypeRepository = currencyTypeRepository;
            _eventDeliveryTypeDetailRepository = eventDeliveryTypeDetailRepository;
            _userCardDetailRepository = userCardDetailRepository;
            _matchSeatTicketDetailRepository = matchSeatTicketDetailRepository;
            _matchLayoutSectionSeatRepository = matchLayoutSectionSeatRepository;
            _venueRepository = venueRepository;
            _cityRepository = cityRepository;
            _stateRepository = stateRepository;
            _countryRepository = countryRepository;
            _userAddressDetailRepository = userAddressDetailRepository;
            _userRepository = userRepository;
            _zipcodeRepository = zipcodeRepository;
        }

        public GetTicketPickUpDetailsQueryResult Handle(GetTicketPickUpDetailsQuery query)
        {
            var transaction = _transactionRepository.GetSuccessfulTransactionDetails(query.TransactionId);
            var transactionModel = AutoMapper.Mapper.Map<FIL.Contracts.Models.Transaction>(transaction);

            if (transactionModel == null)
            {
                return new GetTicketPickUpDetailsQueryResult();
            }
            var user = _userRepository.GetByAltId(transaction.CreatedBy);
            var transactionDetails = _transactionDetailsRepository.GetByTransactionId(transactionModel.Id);
            var transactionDetailModel = AutoMapper.Mapper.Map<List<TransactionDetail>>(transactionDetails);

            var eventTicketAttributeDetails = _eventTicketAttributeRepository.GetByEventTicketAttributeIds(transactionDetails.Select(s => s.EventTicketAttributeId));

            var eventTicketDetails = _eventTicketDetailRepository.GetByEventTicketDetailIds(eventTicketAttributeDetails.Select(s => s.EventTicketDetailId));
            var eventTicketDetailModel = AutoMapper.Mapper.Map<List<EventTicketDetail>>(eventTicketDetails);

            var eventDetails = _eventDetailRepository.GetByEventDetailIds(eventTicketDetails.Select(s => s.EventDetailId).Distinct());
            var eventAttribute = _eventAttributeRepository.GetByEventDetailIds(eventTicketDetails.Select(s => s.EventDetailId).Distinct());
            var eventDetailModel = AutoMapper.Mapper.Map<List<EventDetail>>(eventDetails);

            IEnumerable<FIL.Contracts.DataModels.Event> events = new List<FIL.Contracts.DataModels.Event>();
            events = _eventRepository.GetByAllEventIds(eventDetails.Select(s => s.EventId).Distinct());

            var transactionPaymentDetails = _transactionPaymentDetailRepository.GetAllByTransactionId(query.TransactionId);
            var transactionPaymentDetail = transactionPaymentDetails.Where(s => s.UserCardDetailId != null).FirstOrDefault();
            var currencyDetail = _currencyTypeRepository.GetByCurrencyId(transactionModel.CurrencyId);
            var paymentOption = (dynamic)null;
            var userCardDetail = (dynamic)null;
            if (transactionPaymentDetail != null)
            {
                paymentOption = transactionPaymentDetail.PaymentOptionId.ToString();
                userCardDetail = _userCardDetailRepository.GetByUserCardDetailId(transactionPaymentDetail.UserCardDetailId);
            }
            var cardTypes = (dynamic)null;
            if (userCardDetail != null)
            {
                cardTypes = userCardDetail.CardTypeId.ToString();
            }
            var eventsContainer = events.Select(eId =>
            {
                var tEvent = _eventRepository.GetByEventId(eId.Id);
                var tEventDetail = _eventDetailRepository.GetByEventIdAndEventDetailId(eId.Id, eventDetails.Select(edId => edId.Id)).OrderBy(s => s.StartDateTime).OrderByDescending(od => od.Id);

                var subEventContainer = tEventDetail.Select(edetail =>
                {
                    var tEventDetails = _eventDetailRepository.GetByEventIdAndEventDetailIds(eId.Id, edetail.Id);
                    var tEventAttributes = _eventAttributeRepository.GetByEventDetailId(tEventDetails.Id);
                    var tVenue = _venueRepository.Get(tEventDetails.VenueId);
                    var tCity = _cityRepository.Get(tVenue.CityId);
                    var tState = _stateRepository.Get(tCity.StateId);
                    var tCountry = _countryRepository.Get(tState.CountryId);

                    var tEventTicketDetail = _eventTicketDetailRepository.GetByEventDetailIdsAndIds(tEventDetail.Where(w => w.Id == edetail.Id).Select(s => s.Id), eventTicketDetailModel.Select(s => s.Id));

                    var tEventTicketAttribute = _eventTicketAttributeRepository.GetByEventTicketDetailIds(tEventTicketDetail.Select(s => s.Id)).Distinct();
                    var tTicketCategory = _ticketCategoryRepository.GetByTicketCategoryIds(tEventTicketDetail.Select(s => s.TicketCategoryId));

                    var tTransactionDetail = _transactionDetailsRepository.GetByEventTicketAttributeandTransactionId(tEventTicketAttribute.Select(s => s.Id), transactionModel.Id).Distinct();

                    var tTransactionSeatDetail = _transactionSeatDetailRepository.GetByTransactionDetailIds(tTransactionDetail.Select(s => s.Id));

                    var tMatchSeatTicketDetails = _matchSeatTicketDetailRepository.GetbyMatchSeatTicketDetailId(tTransactionSeatDetail.Select(s => s.MatchSeatTicketDetailId));

                    var tMatchLayoutSectionSeats = _matchLayoutSectionSeatRepository.GetByIds((IEnumerable<long>)tMatchSeatTicketDetails.Select(s => s.MatchLayoutSectionSeatId));

                    var tEventDeliveryTypeDetail = _eventDeliveryTypeDetailRepository.GetByEventDetailIds(tEventDetail.Select(s => s.Id));

                    var tTransactionDeliveryDetail = _transactionDeliveryDetailRepository.GetByTransactionDetailIds(tTransactionDetail.Select(s => s.Id));

                    var tUser = _userRepository.GetByAltId(transaction.CreatedBy);
                    var tUserAddress = (dynamic)null;
                    if (tUser != null)
                    {
                        tUserAddress = _userAddressDetailRepository.GetByUserId(tUser.Id).LastOrDefault();
                    }
                    var tZipCode = (dynamic)null;
                    if (tUserAddress != null)
                    {
                        tZipCode = _zipcodeRepository.Get(tUserAddress.Zipcode);
                    }
                    return new SubEventContainer
                    {
                        Event = AutoMapper.Mapper.Map<Event>(tEvent),
                        EventAttribute = AutoMapper.Mapper.Map<EventAttribute>(tEventAttributes),
                        EventDetail = AutoMapper.Mapper.Map<EventDetail>(tEventDetails),
                        Venue = AutoMapper.Mapper.Map<Venue>(tVenue),
                        City = AutoMapper.Mapper.Map<City>(tCity),
                        State = AutoMapper.Mapper.Map<State>(tState),
                        Country = AutoMapper.Mapper.Map<Country>(tCountry),
                        Zipcode = AutoMapper.Mapper.Map<Zipcode>(tZipCode),
                        EventTicketDetail = AutoMapper.Mapper.Map<IEnumerable<EventTicketDetail>>(tEventTicketDetail),
                        EventTicketAttribute = AutoMapper.Mapper.Map<IEnumerable<EventTicketAttribute>>(tEventTicketAttribute),
                        TicketCategory = AutoMapper.Mapper.Map<IEnumerable<FIL.Contracts.Models.TicketCategory>>(tTicketCategory),
                        TransactionDetail = AutoMapper.Mapper.Map<IEnumerable<TransactionDetail>>(tTransactionDetail),
                        MatchSeatTicketDetail = AutoMapper.Mapper.Map<IEnumerable<MatchSeatTicketDetail>>(tMatchSeatTicketDetails),
                        MatchLayoutSectionSeat = AutoMapper.Mapper.Map<IEnumerable<MatchLayoutSectionSeat>>(tMatchLayoutSectionSeats),
                        EventDeliveryTypeDetail = AutoMapper.Mapper.Map<IEnumerable<EventDeliveryTypeDetail>>(tEventDeliveryTypeDetail),
                        TransactionDeliveryDetail = AutoMapper.Mapper.Map<IEnumerable<TransactionDeliveryDetail>>(tTransactionDeliveryDetail)
                    };
                });

                return new FIL.Contracts.Models.BoxOffice.EventContainer
                {
                    Event = AutoMapper.Mapper.Map<Event>(tEvent),
                    subEventContainer = subEventContainer.ToList()
                };
            });

            return new GetTicketPickUpDetailsQueryResult
            {
                Transaction = transactionModel,
                User = AutoMapper.Mapper.Map<User>(user),
                TransactionPaymentDetail = AutoMapper.Mapper.Map<FIL.Contracts.Models.TransactionPaymentDetail>(transactionPaymentDetail),
                UserCardDetail = AutoMapper.Mapper.Map<FIL.Contracts.Models.UserCardDetail>(userCardDetail),
                CurrencyType = AutoMapper.Mapper.Map<FIL.Contracts.Models.CurrencyType>(currencyDetail),
                PaymentOption = paymentOption,
                cardTypes = cardTypes,
                eventsContainer = eventsContainer.ToList(),
                TicketQuantity = eventTicketAttributeDetails.ToArray().Length
            };
        }
    }
}