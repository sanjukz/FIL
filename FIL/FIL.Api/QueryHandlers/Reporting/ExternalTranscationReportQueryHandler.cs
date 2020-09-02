using FIL.Api.Repositories;
using FIL.Contracts.Enums;
using FIL.Contracts.Models.Report;
using FIL.Contracts.Queries.Reporting;
using FIL.Contracts.QueryResults.Reporting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.Reporting
{
    public class ExternalTranscationReportQueryHandler : IQueryHandler<ExternalReportQuery, ExternalReportQueryResult>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionDetailRepository _transactionDetailRepository;
        private readonly IUserRepository _userRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly IIPDetailRepository _ipDetailRepository;
        private readonly ITransactionDeliveryDetailRepository _transactionDeliveryDetailRepository;
        private readonly ICurrencyTypeRepository _currencyTypeRepository;
        private readonly ITransactionPaymentDetailRepository _transactionPaymentDetailRepository;
        private readonly IEventAttributeRepository _eventAttributeRepository;
        private readonly IUserCardDetailRepository _userCardDetailRepository;
        private readonly ITicketFeeDetailRepository _ticketFeeDetail;
        private readonly IBoUserVenueRepository _boUserVenueRepository;
        private readonly IBoCustomerDetailRepository _boCustomerDetailRepository;
        private readonly ITransactionSeatDetailRepository _transactionSeatDetailRepository;
        private readonly IMatchSeatTicketDetailRepository _matchSeatTicketDetailRepository;
        private readonly IMatchLayoutSectionSeatRepository _matchLayoutSectionSeatRepository;
        private readonly FIL.Logging.ILogger _logger;

        public ExternalTranscationReportQueryHandler(
            IEventRepository eventRepository,
            IEventDetailRepository eventDetailRepository,
            IEventTicketDetailRepository eventTicketDetailRepository,
            IEventTicketAttributeRepository eventTicketAttributeRepository,
            ITransactionRepository transactionRepository,
            ITransactionDetailRepository transactionDetailRepository,
            IUserRepository userRepository,
            IVenueRepository venueRepository,
            ICityRepository cityRepository,
            IStateRepository stateRepository,
            ICountryRepository countryRepository,
            ITicketCategoryRepository ticketCategoryRepository,
            IIPDetailRepository ipDetailRepository,
            ITransactionDeliveryDetailRepository transactionDeliveryDetailRepository,
            ICurrencyTypeRepository currencyTypeRepository,
            ITransactionPaymentDetailRepository transactionPaymentDetailRepository,
            IEventAttributeRepository eventAttributeRepository,
            IBoUserVenueRepository boUserVenueRepository,
            IUserCardDetailRepository userCardDetailRepository,
            ITicketFeeDetailRepository ticketFeeDetail,
            IBoCustomerDetailRepository boCustomerDetailRepository,
            ITransactionSeatDetailRepository transactionSeatDetailRepository,
            IMatchSeatTicketDetailRepository matchSeatTicketDetailRepository,
            IMatchLayoutSectionSeatRepository matchLayoutSectionSeatRepository,
            FIL.Logging.ILogger logger
        )
        {
            _eventRepository = eventRepository;
            _eventDetailRepository = eventDetailRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _transactionRepository = transactionRepository;
            _transactionDetailRepository = transactionDetailRepository;
            _userRepository = userRepository;
            _venueRepository = venueRepository;
            _cityRepository = cityRepository;
            _stateRepository = stateRepository;
            _countryRepository = countryRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _ipDetailRepository = ipDetailRepository;
            _transactionDeliveryDetailRepository = transactionDeliveryDetailRepository;
            _currencyTypeRepository = currencyTypeRepository;
            _transactionPaymentDetailRepository = transactionPaymentDetailRepository;
            _eventAttributeRepository = eventAttributeRepository;
            _boUserVenueRepository = boUserVenueRepository;
            _userCardDetailRepository = userCardDetailRepository;
            _ticketFeeDetail = ticketFeeDetail;
            _boCustomerDetailRepository = boCustomerDetailRepository;
            _transactionSeatDetailRepository = transactionSeatDetailRepository;
            _matchSeatTicketDetailRepository = matchSeatTicketDetailRepository;
            _matchLayoutSectionSeatRepository = matchLayoutSectionSeatRepository;
            _logger = logger;
        }

        public ExternalReportQueryResult Handle(ExternalReportQuery query)
        {
            try
            {
                DateTime FromDate = new DateTime(2000, 01, 01);
                DateTime ToDate = DateTime.UtcNow;
                ReportExportStatus ExportStatus = ReportExportStatus.None;

                if (query.FromDate != null)
                {
                    FromDate = (DateTime)query.FromDate;
                }

                if (query.ToDate != null)
                {
                    ToDate = (DateTime)query.ToDate;
                }

                if (query.ExportStatus != null)
                {
                    ExportStatus = (ReportExportStatus)query.ExportStatus;
                }

                var transactionsList = new List<FIL.Contracts.DataModels.Transaction>();

                transactionsList = AutoMapper.Mapper.Map<List<FIL.Contracts.DataModels.Transaction>>(_transactionRepository.GetTransactionsByUserDatesAndStatus(query.UserAltId, FromDate, ToDate, ExportStatus).ToList());

                if (transactionsList.Any())
                {
                    var externalTranscationReportContainer = transactionsList.Select(t =>
                    {
                        var FirstName = t.FirstName;
                        var LastName = t.LastName;
                        var PhoneCode = t.PhoneCode;
                        var PhoneNumber = t.PhoneNumber;
                        var TransactionAltId = t.AltId;
                        var UserAltId = t.CreatedBy;
                        var UserName = _userRepository.GetByAltId(t.CreatedBy).UserName;
                        var TransactionDateTime = t.CreatedUtc;

                        var Currency = _currencyTypeRepository.GetByCurrencyId(t.CurrencyId);
                        var CurrencyCode = Currency.Code;
                        var ExchangeRate = Currency.ExchangeRate;
                        var PaymentType = "Cash";
                        var TotalAmount = t.GrossTicketAmount;
                        var DiscountAmount = t.DiscountAmount;
                        ReportExportStatus ReportExportStatus = (ReportExportStatus)t.ReportExportStatus;

                        var transactionDetailsList = AutoMapper.Mapper.Map<List<FIL.Contracts.DataModels.TransactionDetail>>(_transactionDetailRepository.GetByTransactionId(t.Id));
                        if (transactionDetailsList.Any())
                        {
                            var ticketCategoryContainer = transactionDetailsList.Select(td =>
                            {
                                var eventTicketAttributes = _eventTicketAttributeRepository.Get(td.EventTicketAttributeId);
                                var ticketCategoryId = _eventTicketDetailRepository.Get(eventTicketAttributes.EventTicketDetailId).TicketCategoryId;
                                var eventDetailId = _eventTicketDetailRepository.Get(eventTicketAttributes.EventTicketDetailId).EventDetailId;
                                var ticketCategoryName = _ticketCategoryRepository.Get((int)ticketCategoryId).Name;
                                var transactionSeatDetails = _transactionSeatDetailRepository.GetByTransactionDetailId(td.Id);
                                var eventDetails = _eventDetailRepository.Get(eventDetailId);
                                var events = _eventRepository.Get(eventDetails.EventId);
                                var seatNumbers = "";
                                if (transactionSeatDetails.Any())
                                {
                                    var matchSeatTicketDetails = _matchSeatTicketDetailRepository.GetbyTranscationSeatTicketDetailId(transactionSeatDetails.Select(s => s.MatchSeatTicketDetailId));
                                    var matchLayoutSectionSeats = _matchLayoutSectionSeatRepository.GetByIds((IEnumerable<long>)matchSeatTicketDetails.Select(s => s.MatchLayoutSectionSeatId)).ToList();
                                    if (matchLayoutSectionSeats.Any())
                                    {
                                        foreach (var item in matchLayoutSectionSeats)
                                        {
                                            if (seatNumbers != "")
                                            {
                                                seatNumbers = ", " + item.SeatTag;
                                            }
                                            else
                                            {
                                                seatNumbers += item.SeatTag;
                                            }
                                        };
                                    }
                                }
                                return new FIL.Contracts.Models.Report.TicketCategoryContainer
                                {
                                    EventName = eventDetails.Name,
                                    EventSource = (int)events.EventSourceId == 0 ? "Zoonga" : events.EventSourceId.ToString(),
                                    EventDateTime = eventDetails.StartDateTime,
                                    TicketCategoryName = ticketCategoryName,
                                    TicketType = (TicketType)td.TicketTypeId,
                                    TicketQuantity = td.TotalTickets,
                                    TicketPrice = td.PricePerTicket,
                                    CurrencyCode = CurrencyCode,
                                    SeatNumbers = seatNumbers,
                                    ExchangeRate = ExchangeRate
                                };
                            });

                            return new ExternalTranscationReportContainer
                            {
                                FirstName = FirstName,
                                LastName = LastName,
                                PhoneCode = PhoneCode,
                                PhoneNumber = PhoneNumber,
                                TransactionAltId = t.AltId,
                                UserAltId = UserAltId,
                                UserName = UserName,
                                TransactionDateTime = TransactionDateTime,
                                CurrencyCode = CurrencyCode,
                                PaymentType = PaymentType,
                                TotalAmount = TotalAmount,
                                DiscountAmount = DiscountAmount,
                                ExportStatus = ReportExportStatus.ToString(),
                                ticketCategoryContainer = ticketCategoryContainer.ToList()
                            };
                        }
                        else
                        {
                            return new ExternalTranscationReportContainer
                            {
                                FirstName = FirstName,
                                LastName = LastName,
                                PhoneCode = PhoneCode,
                                PhoneNumber = PhoneNumber,
                                TransactionAltId = t.AltId,
                                UserAltId = UserAltId,
                                UserName = UserName,
                                TransactionDateTime = TransactionDateTime,
                                CurrencyCode = CurrencyCode,
                                PaymentType = t.FirstName,
                                TotalAmount = TotalAmount,
                                DiscountAmount = DiscountAmount,
                                ExportStatus = ReportExportStatus.ToString(),
                                ticketCategoryContainer = null
                            };
                        }
                    });

                    return new ExternalReportQueryResult
                    {
                        externalTranscationReportContainer = externalTranscationReportContainer.ToList(),
                        IsValid = true
                    };
                }
                else
                {
                    return new ExternalReportQueryResult
                    {
                        externalTranscationReportContainer = null,
                        IsValid = false
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
            }

            return new ExternalReportQueryResult
            {
                externalTranscationReportContainer = null,
                IsValid = false
            };
        }
    }
}