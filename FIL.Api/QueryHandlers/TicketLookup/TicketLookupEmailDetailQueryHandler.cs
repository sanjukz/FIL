using FIL.Api.Core.Utilities;
using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.TicketLookup;
using FIL.Contracts.QueryResults.TicketLookup;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.TicketLookup
{
    public class TicketLookupEmailDetailQueryHandler : IQueryHandler<TicketLookupEmailDetailQuery, TicketLookupEmailDetailQueryResult>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionDetailRepository _transactionDetailsRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
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
        private readonly IDataSettings _dataSettings;

        public TicketLookupEmailDetailQueryHandler
         (
         ITransactionRepository transactionRepository,
         ITransactionDetailRepository transactionDetailsRepository,
         IEventTicketDetailRepository eventTicketDetailRepository,
         ITicketCategoryRepository ticketCategoryRepository,
         IEventDetailRepository eventDetailRepository,
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
         IDataSettings dataSettings)
        {
            _transactionRepository = transactionRepository;
            _transactionDetailsRepository = transactionDetailsRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _eventDetailRepository = eventDetailRepository;
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
            _dataSettings = dataSettings;
        }

        public TicketLookupEmailDetailQueryResult Handle(TicketLookupEmailDetailQuery query)
        {
            List<Contracts.DataModels.Transaction> transactions;
            _dataSettings.UnitOfWork.BeginReadUncommittedTransaction();
            try
            {
                if (query.Name == null && query.Phone == null)
                {
                    transactions = _transactionRepository.GetByEmailId(query.Email).ToList();
                }
                else if (query.Phone == null && query.Email == null)
                {
                    transactions = _transactionRepository.GetByName(query.Name).Distinct().ToList();
                }
                else
                {
                    transactions = _transactionRepository.GetByPhone(query.Phone).ToList();
                }
                if (transactions.Any())
                {
                    var ticketLookupEmailDetailContainer = transactions.Select(t =>
                    {
                        string payConfig = "";
                        var transactionDetails = _transactionDetailsRepository.GetByTransactionId(t.Id).ToList();
                        var eventTicketAttributes = _eventTicketAttributeRepository.GetByEventTicketAttributeIds(transactionDetails.Select(s => s.EventTicketAttributeId).Distinct());
                        var eventTicketDetails = _eventTicketDetailRepository.GetByIds(eventTicketAttributes.Select(s => s.EventTicketDetailId).Distinct());
                        var eventDetails = _eventDetailRepository.GetByEventDetailIds(eventTicketDetails.Select(s => s.EventDetailId).Distinct());
                        var events = _eventRepository.GetByAllEventIds(eventDetails.Select(s => s.EventId).Distinct());
                        var transactionPaymentDetail = _transactionPaymentDetailRepository.GetByTransactionId(t.Id);
                        var transactionPaymentDetailList = _transactionPaymentDetailRepository.GetPaymentDetailByTransactionId(t.Id);
                        var currencyDetail = _currencyTypeRepository.GetByCurrencyId(t.CurrencyId);

                        var ticketLookupSubContainer = events.Select(e =>
                        {
                            var tEventDetail = eventDetails.Where(ed => ed.EventId == e.Id).ToList();
                            var venues = _venueRepository.GetByVenueIds(tEventDetail.Select(d => d.VenueId).Distinct()).ToList();
                            var cities = _cityRepository.GetByCityIds(venues.Select(d => d.CityId).Distinct()).ToList();
                            var states = _stateRepository.GetByStateIds(cities.Select(d => d.StateId).Distinct()).ToList();
                            var countryLookup = _countryRepository.GetByCountryIds(states.Select(d => d.CountryId).Distinct())
                                .ToDictionary(c => c.Id);
                            var stateLookup = states.ToDictionary(s => s.Id);
                            var cityLookup = cities.ToDictionary(s => s.Id);
                            var venueLookup = venues.ToDictionary(s => s.Id);

                            var subEventContainer = tEventDetail.Select(ed =>
                            {
                                var pEventTicketDetail = eventTicketDetails
                             .Where(etd => ed.Id == etd.EventDetailId);

                                var pEventTicketAttribute = eventTicketAttributes
                                    .Where(eta => pEventTicketDetail.Any(etd => etd.Id == eta.EventTicketDetailId));

                                var tTransactionDetail = transactionDetails
                                    .Where(td => pEventTicketAttribute.Any(eta => td.EventTicketAttributeId == eta.Id)).ToList();

                                var tEventTicketAttribute = eventTicketAttributes
                                    .Where(eta => tTransactionDetail.Any(td => td.EventTicketAttributeId == eta.Id));

                                var tEventTicketDetail = eventTicketDetails
                                    .Where(etd => tEventTicketAttribute.Any(s => s.EventTicketDetailId == etd.Id));

                                var tTicketCategory = _ticketCategoryRepository.GetByTicketCategoryIds(tEventTicketDetail.Select(s => s.TicketCategoryId).Distinct());
                                var tTransactionSeatDetail = _transactionSeatDetailRepository.GetByTransactionDetailIds(tTransactionDetail.Select(s => s.Id));
                                var matchSeatTicketDetailContainer = tEventTicketDetail.Select(etd =>
                                {
                                    var tMatchSeatTicketDetail = _matchSeatTicketDetailRepository.GetByTransactionIdAndTicketDetailId(t.Id, etd.Id);
                                    return new MatchSeatTicketDetailContainer
                                    {
                                        MatchSeatTicketDetail = AutoMapper.Mapper.Map<IEnumerable<MatchSeatTicketDetail>>(tMatchSeatTicketDetail),
                                    };
                                });
                                var tEventDeliveryTypeDetail = _eventDeliveryTypeDetailRepository.GetByEventDetailIds(tEventDetail.Select(s => s.Id).Distinct());
                                var tTransactionDeliveryDetail = _transactionDeliveryDetailRepository.GetByTransactionDetailIds(tTransactionDetail.Select(s => s.Id));

                                var tVenue = venueLookup[ed.VenueId];
                                var tCity = cityLookup[tVenue.CityId];
                                var tState = stateLookup[tCity.StateId];
                                var tCountry = countryLookup[tState.CountryId];

                                return new SubEventContainer
                                {
                                    EventDetail = AutoMapper.Mapper.Map<EventDetail>(ed),
                                    Venue = AutoMapper.Mapper.Map<Venue>(tVenue),
                                    City = AutoMapper.Mapper.Map<City>(tCity),
                                    State = AutoMapper.Mapper.Map<State>(tState),
                                    Country = AutoMapper.Mapper.Map<Country>(tCountry),
                                    EventTicketDetail = AutoMapper.Mapper.Map<IEnumerable<EventTicketDetail>>(tEventTicketDetail),
                                    TicketCategory = AutoMapper.Mapper.Map<IEnumerable<FIL.Contracts.Models.TicketCategory>>(tTicketCategory),
                                    TransactionDetail = AutoMapper.Mapper.Map<IEnumerable<TransactionDetail>>(tTransactionDetail),
                                    EventDeliveryTypeDetail = AutoMapper.Mapper.Map<IEnumerable<EventDeliveryTypeDetail>>(tEventDeliveryTypeDetail),
                                    TransactionDeliveryDetail = AutoMapper.Mapper.Map<IEnumerable<TransactionDeliveryDetail>>(tTransactionDeliveryDetail),
                                };
                            });
                            return new TicketLookupSubContainer
                            {
                                Event = AutoMapper.Mapper.Map<Event>(e),
                                SubEventContainer = subEventContainer.ToList()
                            };
                        });

                        foreach (var transactionPaymentDetails in transactionPaymentDetailList)
                        {
                            if (transactionPaymentDetails.PayConfNumber != null && transactionPaymentDetails.PayConfNumber != "")
                            {
                                payConfig = transactionPaymentDetails.PayConfNumber;
                                break;
                            }
                        }

                        return new TicketLookupEmailDetailContainer
                        {
                            Transaction = AutoMapper.Mapper.Map<FIL.Contracts.Models.Transaction>(t),
                            CurrencyType = AutoMapper.Mapper.Map<FIL.Contracts.Models.CurrencyType>(currencyDetail),
                            PaymentOption = transactionPaymentDetail != null ? transactionPaymentDetail.PaymentOptionId?.ToString() : string.Empty,
                            PayconfigNumber = payConfig,
                            TicketLookupSubContainer = ticketLookupSubContainer.ToList()
                        };
                    });
                    _dataSettings.UnitOfWork.Commit();
                    return new TicketLookupEmailDetailQueryResult
                    {
                        TicketLookupEmailDetailContainer = ticketLookupEmailDetailContainer.ToList()
                    };
                }
                else
                {
                    _dataSettings.UnitOfWork.Commit();
                    return new TicketLookupEmailDetailQueryResult();
                }
            }
            catch (System.Exception ex)
            {
                _dataSettings.UnitOfWork.Rollback();
                return new TicketLookupEmailDetailQueryResult();
            }
        }
    }
}