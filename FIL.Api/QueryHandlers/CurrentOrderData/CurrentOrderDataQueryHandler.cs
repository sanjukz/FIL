using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.CurrentOrderData;
using FIL.Contracts.QueryResults.CurrentOrderData;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.CurrentOrderData
{
    public class CurrentOrderDataQueryHandler : IQueryHandler<CurrentOrderDataQuery, CurrentOrderDataQueryResult>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionDetailRepository _transactionDetailRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventCategoryRepository _eventCategoryRepository;
        private readonly IEventCategoryMappingRepository _eventCategoryMappingRepository;
        private readonly IEventAttributeRepository _eventAttributeRepository;
        private readonly IEventRepository _eventRepository;
        private readonly ITransactionDeliveryDetailRepository _transactionDeliveryDetailRepository;
        private readonly ICurrencyTypeRepository _currencyTypeRepository;
        private readonly IEventDeliveryTypeDetailRepository _eventDeliveryTypeDetailRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IUserAddressDetailRepository _userAddressDetailRepository;
        private readonly IUserRepository _userRepository;
        private readonly IZipcodeRepository _zipcodeRepository;
        private readonly ITicketFeeDetailRepository _ticketFeeDetailRepository;
        private readonly IGuestDetailRepository _guestDetailRepository;

        public CurrentOrderDataQueryHandler(ITransactionRepository transactionRepository,
            ITransactionDetailRepository transactionDetailRepository,
        IEventTicketDetailRepository eventTicketDetailRepository,
        ITicketCategoryRepository ticketCategoryRepository,
        IEventCategoryRepository eventCategoryRepository,
        IEventCategoryMappingRepository eventCategoryMappingRepository,
        IEventDetailRepository eventDetailRepository,
        IEventAttributeRepository eventAttributeRepository,
        IEventTicketAttributeRepository eventTicketAttributeRepository,
        IEventRepository eventRepository,
        ITransactionDeliveryDetailRepository transactionDeliveryDetailRepository,
        ITransactionSeatDetailRepository transactionSeatDetailRepository,
        ICurrencyTypeRepository currencyTypeRepository,
        IEventDeliveryTypeDetailRepository eventDeliveryTypeDetailRepository,
        IVenueRepository venueRepository,
        ICityRepository cityRepository,
        IStateRepository stateRepository,
        ICountryRepository countryRepository,
        IUserAddressDetailRepository userAddressDetailRepository,
        ITicketFeeDetailRepository ticketFeeDetailRepository,
        IUserRepository userRepository,
        IGuestDetailRepository guestDetailRepository,
        IZipcodeRepository zipcodeRepository)
        {
            _transactionRepository = transactionRepository;
            _eventCategoryRepository = eventCategoryRepository;
            _eventCategoryMappingRepository = eventCategoryMappingRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _eventDetailRepository = eventDetailRepository;
            _eventAttributeRepository = eventAttributeRepository;
            _eventRepository = eventRepository;
            _transactionDeliveryDetailRepository = transactionDeliveryDetailRepository;
            _currencyTypeRepository = currencyTypeRepository;
            _eventDeliveryTypeDetailRepository = eventDeliveryTypeDetailRepository;
            _venueRepository = venueRepository;
            _cityRepository = cityRepository;
            _stateRepository = stateRepository;
            _countryRepository = countryRepository;
            _userAddressDetailRepository = userAddressDetailRepository;
            _userRepository = userRepository;
            _zipcodeRepository = zipcodeRepository;
            _ticketFeeDetailRepository = ticketFeeDetailRepository;
            _guestDetailRepository = guestDetailRepository;
            _transactionDetailRepository = transactionDetailRepository;
        }

        public CurrentOrderDataQueryResult Handle(CurrentOrderDataQuery query)
        {
            var transaction = _transactionRepository.GetByAltId(query.TransactionAltId);
            if (transaction == null)
            {
                return new CurrentOrderDataQueryResult
                { };
            }

            var transactionDetails = _transactionDetailRepository.GetByTransactionId(transaction.Id);
            var transactionDetailModel = AutoMapper.Mapper.Map<List<Contracts.Models.TransactionDetail>>(transactionDetails);

            var eventTicketAttributeDetails = _eventTicketAttributeRepository.GetByEventTicketAttributeIds(transactionDetails.Select(s => s.EventTicketAttributeId));

            var eventTicketDetails = _eventTicketDetailRepository.GetByEventTicketDetailIds(eventTicketAttributeDetails.Select(s => s.EventTicketDetailId));
            var eventTicketDetailModel = AutoMapper.Mapper.Map<List<Contracts.Models.EventTicketDetail>>(eventTicketDetails);

            var ticketCategories = _ticketCategoryRepository.GetByTicketCategoryIds(eventTicketDetails.Select(s => s.TicketCategoryId));
            var eventDetails = _eventDetailRepository.GetByEventDetailIds(eventTicketDetails.Select(s => s.EventDetailId).Distinct());
            var eventAttribute = _eventAttributeRepository.GetByEventDetailIds(eventTicketDetails.Select(s => s.EventDetailId).Distinct());

            var eventDetailModel = AutoMapper.Mapper.Map<List<Contracts.Models.EventDetail>>(eventDetails);
            var venues = _venueRepository.GetByVenueIds(eventDetailModel.Select(s => s.VenueId));
            var city = _cityRepository.GetByCityIds(venues.Select(s => s.CityId));
            var state = _stateRepository.GetByStateIds(city.Select(s => s.StateId));
            var country = _countryRepository.GetByCountryIds(state.Select(s => s.CountryId));

            List<FIL.Contracts.DataModels.Event> events = events = _eventRepository.GetByAllEventIds(eventDetails.Select(s => s.EventId).Distinct()).ToList();

            var currencyDetail = _currencyTypeRepository.GetByCurrencyId(transaction.CurrencyId);
            var orderConfirmationSubContainer = events.Select(eId =>
            {
                var tEvent = events.Where(s => s.Id == eId.Id).FirstOrDefault();
                var tEventDetail = _eventDetailRepository.GetByEventIdAndEventDetailId(eId.Id, eventDetails.Select(edId => edId.Id)).OrderBy(s => s.StartDateTime).OrderByDescending(od => od.Id);
                var subEventContainer = tEventDetail.Select(edetail =>
                {
                    var teventCategory = new FIL.Contracts.DataModels.EventCategory();
                    var eventCategotyMappings = _eventCategoryMappingRepository.GetByEventId(tEvent.Id).FirstOrDefault();
                    if (eventCategotyMappings != null)
                    {
                        teventCategory = _eventCategoryRepository.Get(eventCategotyMappings.EventCategoryId);
                    }
                    var tEventDetails = eventDetails.Where(s => s.Id == edetail.Id).FirstOrDefault();
                    var tEventAttributes = eventAttribute.Where(s => s.EventDetailId == tEventDetails.Id).FirstOrDefault();
                    var tVenue = venues.Where(s => s.Id == edetail.VenueId).FirstOrDefault();
                    var tCity = city.Where(s => s.Id == tVenue.CityId).FirstOrDefault();
                    var tState = state.Where(s => s.Id == tCity.StateId).FirstOrDefault();
                    var tCountry = country.Where(s => s.Id == tState.CountryId).FirstOrDefault();
                    var tEventTicketDetail = _eventTicketDetailRepository.GetByEventDetailIdsAndIds(tEventDetail.Where(w => w.Id == edetail.Id).Select(s => s.Id), eventTicketDetailModel.Select(s => s.Id));
                    var tEventTicketAttribute = eventTicketAttributeDetails.Where(x => tEventTicketDetail.Any(y => y.Id == x.EventTicketDetailId));
                    var tTicketCategory = ticketCategories.Where(x => tEventTicketDetail.Any(y => y.TicketCategoryId == x.Id));
                    var tTransactionDetail = transactionDetails.Where(x => tEventTicketAttribute.Any(y => y.Id == x.EventTicketAttributeId)).ToList();
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
                        Event = AutoMapper.Mapper.Map<Contracts.Models.Event>(tEvent),
                        EventCategory = AutoMapper.Mapper.Map<Contracts.Models.EventCategory>(teventCategory),
                        EventAttribute = AutoMapper.Mapper.Map<Contracts.Models.EventAttribute>(tEventAttributes),
                        EventDetail = AutoMapper.Mapper.Map<Contracts.Models.EventDetail>(tEventDetails),
                        Venue = AutoMapper.Mapper.Map<Contracts.Models.Venue>(tVenue),
                        City = AutoMapper.Mapper.Map<Contracts.Models.City>(tCity),
                        State = AutoMapper.Mapper.Map<Contracts.Models.State>(tState),
                        Country = AutoMapper.Mapper.Map<Contracts.Models.Country>(tCountry),
                        Zipcode = AutoMapper.Mapper.Map<Contracts.Models.Zipcode>(tZipCode),
                        EventTicketDetail = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.EventTicketDetail>>(tEventTicketDetail),
                        EventTicketAttribute = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.EventTicketAttribute>>(tEventTicketAttribute),
                        EventDeliveryTypeDetail = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.EventDeliveryTypeDetail>>(tEventDeliveryTypeDetail),
                        TicketCategory = AutoMapper.Mapper.Map<IEnumerable<FIL.Contracts.Models.TicketCategory>>(tTicketCategory),
                        TransactionDetail = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.TransactionDetail>>(tTransactionDetail),
                        TransactionDeliveryDetail = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.TransactionDeliveryDetail>>(tTransactionDeliveryDetail),
                    };
                });

                return new OrderConfirmationSubContainer
                {
                    Event = AutoMapper.Mapper.Map<Contracts.Models.Event>(tEvent),
                    subEventContainer = subEventContainer.ToList()
                };
            });

            return new CurrentOrderDataQueryResult
            {
                Transaction = AutoMapper.Mapper.Map<Contracts.Models.Transaction>(transaction),
                CurrencyType = AutoMapper.Mapper.Map<FIL.Contracts.Models.CurrencyType>(currencyDetail),
                orderConfirmationSubContainer = orderConfirmationSubContainer.ToList(),
                TicketQuantity = eventTicketAttributeDetails.ToArray().Length,
            };
        }
    }
}