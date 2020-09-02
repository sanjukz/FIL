using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.QueryResults.OrderConfirmation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Providers
{
    public interface IOrderConfirmationProvider
    {
        OrderConfirmationQueryResult Get(long transactionId,
            bool isConfirmationFromMyOrders,
            FIL.Contracts.Enums.Channels channel);
    }

    public class OrderConfirmationProvider : IOrderConfirmationProvider
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionDetailRepository _transactionDetailsRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventCategoryRepository _eventCategoryRepository;
        private readonly IEventCategoryMappingRepository _eventCategoryMappingRepository;
        private readonly IEventAttributeRepository _eventAttributeRepository;
        private readonly IEventRepository _eventRepository;
        private readonly ITransactionDeliveryDetailRepository _transactionDeliveryDetailRepository;
        private readonly ITransactionPaymentDetailRepository _transactionPaymentDetailRepository;
        private readonly ICurrencyTypeRepository _currencyTypeRepository;
        private readonly IEventDeliveryTypeDetailRepository _eventDeliveryTypeDetailRepository;
        private readonly IUserCardDetailRepository _userCardDetailRepository;
        private readonly IMatchSeatTicketDetailRepository _matchSeatTicketDetailRepository;
        private readonly IMatchLayoutSectionSeatRepository _matchLayoutSectionSeatRepository;
        private readonly IMatchLayoutCompanionSeatMappingRepository _matchLayoutCompanionSeatMappingRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IUserAddressDetailRepository _userAddressDetailRepository;
        private readonly IUserRepository _userRepository;
        private readonly IZipcodeRepository _zipcodeRepository;
        private readonly ITicketFeeDetailRepository _ticketFeeDetailRepository;
        private readonly IEventTimeSlotMappingRepository _eventTimeSlotMappingRepository;
        private readonly IASITransactionDetailTimeSlotIdMappingRepository _aSITransactionDetailTimeSlotIdMappingRepository;
        private readonly IGuestDetailRepository _guestDetailRepository;
        private readonly IASIPaymentResponseDetailTicketMappingRepository _aSIPaymentResponseDetailTicketMappingRepository;
        private readonly ITransactionScheduleDetail _transactionScheduleDetail;
        private readonly IScheduleDetailRepository _scheduleDetailRepository;

        public OrderConfirmationProvider(ITransactionRepository transactionRepository,
        ITransactionDetailRepository transactionDetailsRepository,
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
        ITransactionPaymentDetailRepository transactionPaymentDetailRepository,
        ICurrencyTypeRepository currencyTypeRepository,
        IEventDeliveryTypeDetailRepository eventDeliveryTypeDetailRepository,
        IUserCardDetailRepository userCardDetailRepository,
        IMatchSeatTicketDetailRepository matchSeatTicketDetailRepository,
        IMatchLayoutSectionSeatRepository matchLayoutSectionSeatRepository,
        IMatchLayoutCompanionSeatMappingRepository matchLayoutCompanionSeatMappingRepository,
        IVenueRepository venueRepository,
        ICityRepository cityRepository,
        IStateRepository stateRepository,
        ICountryRepository countryRepository,
        IUserAddressDetailRepository userAddressDetailRepository,
        ITicketFeeDetailRepository ticketFeeDetailRepository,
        IUserRepository userRepository,
        IEventTimeSlotMappingRepository eventTimeSlotMappingRepository,
        IASITransactionDetailTimeSlotIdMappingRepository aSITransactionDetailTimeSlotIdMappingRepository,
        IGuestDetailRepository guestDetailRepository,
        IASIPaymentResponseDetailTicketMappingRepository aSIPaymentResponseDetailTicketMappingRepository,
        ITransactionScheduleDetail transactionScheduleDetail,
        IScheduleDetailRepository scheduleDetailRepository,
        IZipcodeRepository zipcodeRepository)
        {
            _transactionRepository = transactionRepository;
            _transactionDetailsRepository = transactionDetailsRepository;
            _eventCategoryRepository = eventCategoryRepository;
            _eventCategoryMappingRepository = eventCategoryMappingRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _eventDetailRepository = eventDetailRepository;
            _eventAttributeRepository = eventAttributeRepository;
            _eventRepository = eventRepository;
            _transactionDeliveryDetailRepository = transactionDeliveryDetailRepository;
            _transactionPaymentDetailRepository = transactionPaymentDetailRepository;
            _currencyTypeRepository = currencyTypeRepository;
            _eventDeliveryTypeDetailRepository = eventDeliveryTypeDetailRepository;
            _userCardDetailRepository = userCardDetailRepository;
            _matchSeatTicketDetailRepository = matchSeatTicketDetailRepository;
            _matchLayoutSectionSeatRepository = matchLayoutSectionSeatRepository;
            _matchLayoutCompanionSeatMappingRepository = matchLayoutCompanionSeatMappingRepository;
            _venueRepository = venueRepository;
            _cityRepository = cityRepository;
            _stateRepository = stateRepository;
            _countryRepository = countryRepository;
            _userAddressDetailRepository = userAddressDetailRepository;
            _userRepository = userRepository;
            _zipcodeRepository = zipcodeRepository;
            _ticketFeeDetailRepository = ticketFeeDetailRepository;
            _eventTimeSlotMappingRepository = eventTimeSlotMappingRepository;
            _aSITransactionDetailTimeSlotIdMappingRepository = aSITransactionDetailTimeSlotIdMappingRepository;
            _guestDetailRepository = guestDetailRepository;
            _transactionScheduleDetail = transactionScheduleDetail;
            _scheduleDetailRepository = scheduleDetailRepository;
            _aSIPaymentResponseDetailTicketMappingRepository = aSIPaymentResponseDetailTicketMappingRepository;
        }

        public OrderConfirmationQueryResult Get(
            long transactionId,
            bool isConfirmationFromMyOrders,
            FIL.Contracts.Enums.Channels channel)
        {
            var transactionScheduleDetails = new List<FIL.Contracts.DataModels.TransactionScheduleDetail>();
            var scheduleDetails = new List<FIL.Contracts.DataModels.ScheduleDetail>();
            var transaction = _transactionRepository.GetSuccessfulTransactionDetails(transactionId);
            var transactionModel = AutoMapper.Mapper.Map<FIL.Contracts.Models.Transaction>(transaction);
            var IsASI = false; var IsHoho = false;
            if (transactionModel == null)
            {
                return new OrderConfirmationQueryResult();
            }

            var transactionDetails = _transactionDetailsRepository.GetByTransactionId(transactionModel.Id);
            var transactionDetailModel = AutoMapper.Mapper.Map<List<TransactionDetail>>(transactionDetails);

            var eventTicketAttributeDetails = _eventTicketAttributeRepository.GetByEventTicketAttributeIds(transactionDetails.Select(s => s.EventTicketAttributeId));

            var eventTicketDetails = _eventTicketDetailRepository.GetByEventTicketDetailIds(eventTicketAttributeDetails.Select(s => s.EventTicketDetailId));
            var eventTicketDetailModel = AutoMapper.Mapper.Map<List<EventTicketDetail>>(eventTicketDetails);

            var ticketCategories = _ticketCategoryRepository.GetByTicketCategoryIds(eventTicketDetails.Select(s => s.TicketCategoryId));
            var eventDetails = _eventDetailRepository.GetByEventDetailIds(eventTicketDetails.Select(s => s.EventDetailId).Distinct());
            var eventAttribute = _eventAttributeRepository.GetByEventDetailIds(eventTicketDetails.Select(s => s.EventDetailId).Distinct());

            var eventDetailModel = AutoMapper.Mapper.Map<List<EventDetail>>(eventDetails);
            var venues = _venueRepository.GetByVenueIds(eventDetailModel.Select(s => s.VenueId));
            var city = _cityRepository.GetByCityIds(venues.Select(s => s.CityId));
            var state = _stateRepository.GetByStateIds(city.Select(s => s.StateId));
            var country = _countryRepository.GetByCountryIds(state.Select(s => s.CountryId));
            if (channel == Contracts.Enums.Channels.Feel && eventDetailModel.FirstOrDefault().EventFrequencyType == Contracts.Enums.EventFrequencyType.Recurring)
            {
                transactionScheduleDetails = _transactionScheduleDetail.GetAllByTransactionDetails(transactionDetailModel.Select(s => s.Id).ToList()).ToList();
                scheduleDetails = _scheduleDetailRepository.GetAllByIds(transactionScheduleDetails.Select(s => s.ScheduleDetailId).ToList()).ToList();
            }
            List<FIL.Contracts.DataModels.Event> events = new List<FIL.Contracts.DataModels.Event>();
            if (isConfirmationFromMyOrders == true)
            {
                events = _eventRepository.GetByAllEventIds(eventDetails.Select(s => s.EventId).Distinct()).ToList();
            }
            else
            {
                events = _eventRepository.GetByAllTypeEventIds(eventDetails.Select(s => s.EventId).Distinct()).ToList();
            }

            var transactionPaymentDetails = _transactionPaymentDetailRepository.GetAllByTransactionId(transactionId);
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

            var orderConfirmationSubContainer = events.Select(eId =>
            {
                var tEvent = events.Where(s => s.Id == eId.Id).FirstOrDefault();
                var tEventDetail = _eventDetailRepository.GetByEventIdAndEventDetailId(eId.Id, eventDetails.Select(edId => edId.Id)).OrderBy(s => s.StartDateTime).OrderByDescending(od => od.Id);

                var subEventContainer = tEventDetail.Select(edetail =>
                {
                    var teventCategory = new FIL.Contracts.DataModels.EventCategory();
                    if (channel == FIL.Contracts.Enums.Channels.Feel)
                    {
                        var eventCategotyMappings = _eventCategoryMappingRepository.GetByEventId(tEvent.Id).FirstOrDefault();
                        if (eventCategotyMappings != null)
                        {
                            teventCategory = _eventCategoryRepository.Get(eventCategotyMappings.EventCategoryId);
                        }
                    }
                    else
                    {
                        teventCategory = _eventCategoryRepository.Get(tEvent.EventCategoryId);
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
                    List<MatchLayoutSectionSeat> tMatchLayoutSectionSeats = new List<MatchLayoutSectionSeat>();
                    List<FIL.Contracts.DataModels.MatchLayoutCompanionSeatMapping> tMatchLayoutCompanionSeatMapping = new List<FIL.Contracts.DataModels.MatchLayoutCompanionSeatMapping>();

                    var tMatchSeatTicketDetails = _matchSeatTicketDetailRepository.GetByTransactionIdAndTicketDetailIds(tTransactionDetail.Select(w => w.TransactionId).FirstOrDefault(), tEventTicketDetail.Select(w => w.Id));
                    if (tMatchSeatTicketDetails.Any())
                    {
                        var matchSeatTicketDetails = tMatchSeatTicketDetails.Where(w => w.MatchLayoutSectionSeatId != null).FirstOrDefault();
                        if (matchSeatTicketDetails != null)
                        {
                            tMatchLayoutSectionSeats = AutoMapper.Mapper.Map<List<MatchLayoutSectionSeat>>(_matchLayoutSectionSeatRepository.GetByIds(tMatchSeatTicketDetails.Select(s => (long)s.MatchLayoutSectionSeatId).Distinct()));
                            tMatchLayoutCompanionSeatMapping = AutoMapper.Mapper.Map<List<FIL.Contracts.DataModels.MatchLayoutCompanionSeatMapping>>(_matchLayoutCompanionSeatMappingRepository.GetByWheelChairSeatIds(tMatchLayoutSectionSeats.Select(s => s.Id).Distinct()));
                        }
                    }

                    var taSITransactionDetailTimeSlotIdMapping = _aSITransactionDetailTimeSlotIdMappingRepository.GetByTransactionDetailIds(tTransactionDetail.Select(s => s.Id));
                    var tGuestDetails = _guestDetailRepository.GetByTransactionDetailIds(tTransactionDetail.Select(s => s.Id));
                    var taSIPaymentResponseDetailTicketMappings = _aSIPaymentResponseDetailTicketMappingRepository.GetByVisitorIds(tGuestDetails.Select(s => (long)s.Id).ToList());
                    var teventTimeSlotMapping = _eventTimeSlotMappingRepository.GetByIds(taSITransactionDetailTimeSlotIdMapping.Select(s => s.EventTimeSlotMappingId));
                    if (tEvent.EventSourceId == Contracts.Enums.EventSource.ASI)
                    {
                        IsASI = true;
                        foreach (FIL.Contracts.DataModels.TransactionDetail currentTransacitionDetail in tTransactionDetail)
                        {
                            var visitDate = (DateTime)currentTransacitionDetail.VisitDate;
                            currentTransacitionDetail.VisitDate = visitDate.AddHours(5).AddMinutes(30);
                        }
                    }

                    if (tEvent.EventSourceId == Contracts.Enums.EventSource.CitySightSeeing)
                    {
                        IsHoho = true;
                    }
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
                    var transctionScheduleDetails = transactionScheduleDetails.Where(s => tTransactionDetail.Any(x => x.Id == s.TransactionDetailId)).ToList();
                    var scheduleDetail = scheduleDetails.Where(s => transctionScheduleDetails.Any(x => x.ScheduleDetailId == s.Id)).ToList();
                    return new SubEventContainer
                    {
                        Event = AutoMapper.Mapper.Map<Event>(tEvent),
                        EventCategory = AutoMapper.Mapper.Map<EventCategory>(teventCategory),
                        EventAttribute = AutoMapper.Mapper.Map<EventAttribute>(tEventAttributes),
                        EventDetail = AutoMapper.Mapper.Map<EventDetail>(tEventDetails),
                        Venue = AutoMapper.Mapper.Map<Venue>(tVenue),
                        City = AutoMapper.Mapper.Map<City>(tCity),
                        State = AutoMapper.Mapper.Map<State>(tState),
                        Country = AutoMapper.Mapper.Map<Country>(tCountry),
                        Zipcode = AutoMapper.Mapper.Map<Zipcode>(tZipCode),
                        EventTicketDetail = AutoMapper.Mapper.Map<IEnumerable<EventTicketDetail>>(tEventTicketDetail),
                        EventTicketAttribute = AutoMapper.Mapper.Map<IEnumerable<EventTicketAttribute>>(tEventTicketAttribute),
                        EventDeliveryTypeDetail = AutoMapper.Mapper.Map<IEnumerable<EventDeliveryTypeDetail>>(tEventDeliveryTypeDetail),
                        TicketCategory = AutoMapper.Mapper.Map<IEnumerable<FIL.Contracts.Models.TicketCategory>>(tTicketCategory),
                        TransactionDetail = AutoMapper.Mapper.Map<IEnumerable<TransactionDetail>>(tTransactionDetail),
                        MatchSeatTicketDetail = AutoMapper.Mapper.Map<IEnumerable<MatchSeatTicketDetail>>(tMatchSeatTicketDetails),
                        MatchLayoutSectionSeat = AutoMapper.Mapper.Map<IEnumerable<MatchLayoutSectionSeat>>(tMatchLayoutSectionSeats),
                        MatchLayoutCompanionSeatMappings = AutoMapper.Mapper.Map<IEnumerable<FIL.Contracts.DataModels.MatchLayoutCompanionSeatMapping>>(tMatchLayoutCompanionSeatMapping),
                        TransactionDeliveryDetail = AutoMapper.Mapper.Map<IEnumerable<TransactionDeliveryDetail>>(tTransactionDeliveryDetail),
                        EventTimeSlotMappings = AutoMapper.Mapper.Map<IEnumerable<FIL.Contracts.Models.ASI.EventTimeSlotMapping>>(teventTimeSlotMapping),
                        ASITransactionDetailTimeSlotIdMappings = AutoMapper.Mapper.Map<IEnumerable<FIL.Contracts.Models.ASI.ASITransactionDetailTimeSlotIdMapping>>(taSITransactionDetailTimeSlotIdMapping),
                        ASIPaymentResponseDetailTicketMappings = AutoMapper.Mapper.Map<IEnumerable<FIL.Contracts.Models.ASI.ASIPaymentResponseDetailTicketMapping>>(taSIPaymentResponseDetailTicketMappings),
                        TransactionScheduleDetails = transctionScheduleDetails,
                        ScheduleDetails = scheduleDetail
                    };
                });

                return new OrderConfirmationSubContainer
                {
                    Event = AutoMapper.Mapper.Map<Event>(tEvent),
                    subEventContainer = subEventContainer.ToList()
                };
            });

            decimal TotalGst = 0;
            decimal GST = 0;
            decimal AdditionalGST = 0;
            try
            {
                if (transactionModel != null)
                {
                    if (orderConfirmationSubContainer.Any() && orderConfirmationSubContainer.ToList().ElementAt(0).Event.AltId.ToString().ToUpper() == "1F0257FA-EEA6-4469-A7BC-B878A215C8A9") //RASV Email
                    {
                        TotalGst = (decimal)transactionModel.ConvenienceCharges / 11;
                    }
                }
            }
            catch (Exception e)
            {
            }

            return new OrderConfirmationQueryResult
            {
                Transaction = transactionModel,
                TransactionPaymentDetail = AutoMapper.Mapper.Map<FIL.Contracts.Models.TransactionPaymentDetail>(transactionPaymentDetail),
                UserCardDetail = AutoMapper.Mapper.Map<FIL.Contracts.Models.UserCardDetail>(userCardDetail),
                CurrencyType = AutoMapper.Mapper.Map<FIL.Contracts.Models.CurrencyType>(currencyDetail),
                PaymentOption = paymentOption,
                GoodsAndServiceTax = TotalGst,
                cardTypes = cardTypes,
                orderConfirmationSubContainer = orderConfirmationSubContainer.ToList(),
                TicketQuantity = eventTicketAttributeDetails.ToArray().Length,
                IsASI = IsASI,
                IsHoho = IsHoho
            };
        }
    }
}