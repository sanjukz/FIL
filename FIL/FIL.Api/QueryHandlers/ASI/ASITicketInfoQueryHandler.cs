using FIL.Api.Repositories;
using FIL.Contracts.Models.ASI;
using FIL.Contracts.Queries.ASI;
using FIL.Contracts.QueryResults.ASI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.Account
{
    public class ASITicketInfoQueryHandler : IQueryHandler<ASITicketInfoQuery, ASITicketInfoQueryResult>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionDetailRepository _transactionDetailRepository;
        private readonly IGuestDetailRepository _guestDetailRepository;
        private readonly IASIPaymentResponseDetailRepository _aSIPaymentResponseDetailRepository;
        private readonly IASIPaymentResponseDetailTicketMappingRepository _aSIPaymentResponseDetailTicketMappingRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IEventTimeSlotMappingRepository _eventTimeSlotMappingRepository;
        private readonly IASITransactionDetailTimeSlotIdMappingRepository _aSITransactionDetailTimeSlotIdMappingRepository;

        public ASITicketInfoQueryHandler(
           ITransactionRepository transactionRepository,
           IGuestDetailRepository guestDetailRepository,
           ITransactionDetailRepository transactionDetailRepository,
           IEventTicketDetailRepository eventTicketDetailRepository,
           ITicketCategoryRepository ticketCategoryRepository,
           IEventDetailRepository eventDetailRepository,
           IEventTicketAttributeRepository eventTicketAttributeRepository,
           IEventRepository eventRepository,
           IVenueRepository venueRepository,
           ICityRepository cityRepository,
           IStateRepository stateRepository,
           ICountryRepository countryRepository,
           IEventTimeSlotMappingRepository eventTimeSlotMappingRepository,
           IASITransactionDetailTimeSlotIdMappingRepository aSITransactionDetailTimeSlotIdMappingRepository,
           IASIPaymentResponseDetailRepository aSIPaymentResponseDetailRepository,
           IASIPaymentResponseDetailTicketMappingRepository aSIPaymentResponseDetailTicketMappingRepository
            )
        {
            _transactionRepository = transactionRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _eventDetailRepository = eventDetailRepository;
            _eventRepository = eventRepository;
            _transactionRepository = transactionRepository;
            _transactionDetailRepository = transactionDetailRepository;
            _guestDetailRepository = guestDetailRepository;
            _venueRepository = venueRepository;
            _cityRepository = cityRepository;
            _stateRepository = stateRepository;
            _countryRepository = countryRepository;
            _aSIPaymentResponseDetailRepository = aSIPaymentResponseDetailRepository;
            _aSIPaymentResponseDetailTicketMappingRepository = aSIPaymentResponseDetailTicketMappingRepository;
            _eventTimeSlotMappingRepository = eventTimeSlotMappingRepository;
            _aSITransactionDetailTimeSlotIdMappingRepository = aSITransactionDetailTimeSlotIdMappingRepository;
        }

        public ASITicketInfoQueryResult Handle(ASITicketInfoQuery query)
        {
            List<FIL.Contracts.DataModels.Transaction> transactions = new List<FIL.Contracts.DataModels.Transaction>();
            List<FIL.Contracts.DataModels.TransactionDetail> transactionDetails = new List<FIL.Contracts.DataModels.TransactionDetail>();
            List<FIL.Contracts.DataModels.GuestDetail> guestDetails = new List<FIL.Contracts.DataModels.GuestDetail>();
            if (query.IsASIQR)
            {
                var transactionIds = query.TransactionIds.Split(",");
                List<long> allTransactionIds = new List<long>();
                foreach (var transactionId in transactionIds)
                {
                    allTransactionIds.Add(Convert.ToInt64(transactionId));
                }
                transactions = _transactionRepository.GetByTransactionIds(allTransactionIds).ToList();
                transactionDetails = _transactionDetailRepository.GetByTransactionIds(transactions.Select(s => s.Id)).ToList();
                guestDetails = _guestDetailRepository.GetByTransactionDetailIds(transactionDetails.Select(s => s.Id)).ToList();
            }
            else
            {
                var transaction = _transactionRepository.GetSuccessfulTransactionDetails(query.TransactionId);
                transactionDetails = _transactionDetailRepository.GetByTransactionId(transaction.Id).ToList();
                guestDetails = _guestDetailRepository.GetByTransactionDetailIds(transactionDetails.Select(s => s.Id)).ToList();
            }
            List<ASITicketInfo> aSITicketInfos = new List<ASITicketInfo>();
            foreach (FIL.Contracts.DataModels.GuestDetail currentGuest in guestDetails)
            {
                ASITicketInfo aSITicketInfo = new ASITicketInfo();
                var currentTransactionDetail = _transactionDetailRepository.Get(currentGuest.TransactionDetailId);
                var eventTicketAttributeDetail = _eventTicketAttributeRepository.Get(currentTransactionDetail.EventTicketAttributeId);
                var eventTicketDetail = _eventTicketDetailRepository.Get(eventTicketAttributeDetail.EventTicketDetailId);
                var eventDetail = _eventDetailRepository.Get(eventTicketDetail.EventDetailId);
                var ticketCat = _ticketCategoryRepository.Get((int)eventTicketDetail.TicketCategoryId);
                var events = _eventRepository.Get(eventDetail.EventId);
                var tVenue = _venueRepository.Get(eventDetail.VenueId);
                var tCity = _cityRepository.Get(tVenue.CityId);
                var tState = _stateRepository.Get(tCity.StateId);
                var tCountry = _countryRepository.Get(tState.CountryId);
                var asiTicket = _aSIPaymentResponseDetailTicketMappingRepository.GetByVisitorId(currentGuest.Id);
                var asiTransactionTimeSlot = _aSITransactionDetailTimeSlotIdMappingRepository.GetByTransactionDetailId(currentGuest.TransactionDetailId);
                var eventTimeSlot = _eventTimeSlotMappingRepository.Get(asiTransactionTimeSlot.EventTimeSlotMappingId);

                aSITicketInfo.CityName = tCity.Name;
                aSITicketInfo.StateName = tState.Name;
                aSITicketInfo.CountryeName = tCountry.Name;
                aSITicketInfo.EventName = eventDetail.Name;
                aSITicketInfo.Price = currentTransactionDetail.PricePerTicket;
                aSITicketInfo.TicketCategory = ticketCat.Name;
                aSITicketInfo.TicketType = currentTransactionDetail.TicketTypeId == 10 ? "Adult" : "Child";
                aSITicketInfo.VenueName = tVenue.Name;
                aSITicketInfo.VisitDate = Convert.ToDateTime(currentTransactionDetail.VisitDate).AddHours(5).AddMinutes(30);
                aSITicketInfo.VisitTime = eventTimeSlot.StartTime.Split(" ")[0].Split(":")[0] + ":" + eventTimeSlot.StartTime.Split(" ")[0].Split(":")[1] + " " + eventTimeSlot.StartTime.Split(" ")[1].ToUpper();
                aSITicketInfo.FirstName = currentGuest.FirstName;
                aSITicketInfo.LastName = currentGuest.LastName;
                aSITicketInfo.QRCode = asiTicket.QrCode;
                aSITicketInfo.TransactionId = query.TransactionId;
                aSITicketInfo.TicketNo = asiTicket.TicketNo;
                aSITicketInfos.Add(aSITicketInfo);
            }

            return new ASITicketInfoQueryResult
            {
                ASITicketInfo = aSITicketInfos
            };
        }
    }
}