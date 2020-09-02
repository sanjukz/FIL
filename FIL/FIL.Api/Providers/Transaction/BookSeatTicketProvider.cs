using FIL.Api.Repositories;
using FIL.Configuration;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using FIL.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Providers.Transaction
{
    public interface IBookSeatTicketProvider
    {
        void BookSeats(long transactionId, Guid modifiedBy);
    }

    public class BookSeatTicketProvider : IBookSeatTicketProvider
    {
        private readonly ITransactionPaymentDetailRepository _transactionPaymentDetailRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMatchSeatTicketDetailRepository _matchSeatTicketDetailRepository;
        private readonly ITransactionSeatDetailRepository _transactionSeatDetailRepository;
        private readonly ITransactionDetailRepository _transactionDetailRepository;
        private readonly IMatchLayoutSectionSeatRepository _matchLayoutSectionSeatRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly ILogger _logger;

        public BookSeatTicketProvider(ILogger logger,
             ISettings settings,
            ITransactionPaymentDetailRepository transactionPaymentDetailRepository,
            ITransactionRepository transactionRepository,
            ITransactionSeatDetailRepository transactionSeatDetailRepository,
            ITransactionDetailRepository transactionDetailRepository,
            IMatchLayoutSectionSeatRepository matchLayoutSectionSeatRepository,
            IMatchSeatTicketDetailRepository matchSeatTicketDetailRepository,
            IEventTicketDetailRepository eventTicketDetailRepository,
            IEventDetailRepository eventDetailRepository,
            IEventTicketAttributeRepository eventTicketAttributeRepository
            )
        {
            _transactionPaymentDetailRepository = transactionPaymentDetailRepository;
            _transactionRepository = transactionRepository;
            _transactionDetailRepository = transactionDetailRepository;
            _matchSeatTicketDetailRepository = matchSeatTicketDetailRepository;
            _transactionSeatDetailRepository = transactionSeatDetailRepository;
            _matchLayoutSectionSeatRepository = matchLayoutSectionSeatRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _eventDetailRepository = eventDetailRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _logger = logger;
        }

        public void BookSeats(long transactionId, Guid modifiedBy)
        {
            try
            {
                var transaction = _transactionRepository.Get(transactionId);
                try
                {
                    if (transaction != null)
                    {
                        var matchSeatTicketDetails = _matchSeatTicketDetailRepository.GetByTransactionId(transactionId);
                        if (matchSeatTicketDetails == null)
                        {
                            var transactionDetailList = _transactionDetailRepository.GetByTransactionId(transaction.Id);
                            var transactionDetailModel = AutoMapper.Mapper.Map<IEnumerable<TransactionDetail>>(transactionDetailList);
                            var BarcodeNumber = "";
                            foreach (var transactionDetail in transactionDetailModel)
                            {
                                var eventTicketAttributes = _eventTicketAttributeRepository.Get(transactionDetail.EventTicketAttributeId);
                                var eventTicketDetails = _eventTicketDetailRepository.Get(eventTicketAttributes.EventTicketDetailId);
                                var eventDetails = _eventDetailRepository.Get(eventTicketDetails.EventDetailId);
                                var transactionSeatDetails = _transactionSeatDetailRepository.GetByTransactionDetailId(transactionDetail.Id);
                                var transactionSeatDeatilsList = AutoMapper.Mapper.Map<List<TransactionSeatDetail>>(transactionSeatDetails);

                                if (transactionSeatDeatilsList.Any())
                                {
                                    foreach (var item in transactionSeatDeatilsList)
                                    {
                                        Random rd = new Random();
                                        BarcodeNumber = eventDetails.StartDateTime.ToString("ddMM");
                                        int uniqueIdLength = 4;
                                        string uniqueIdChars = "1234567890";
                                        char[] uniqueIdChar = new char[uniqueIdLength];
                                        for (int j = 0; j < uniqueIdLength; j++)
                                        {
                                            uniqueIdChar[j] = uniqueIdChars[rd.Next(0, uniqueIdChars.Length)];
                                        }
                                        string uniqueId = new string(uniqueIdChar);
                                        MatchSeatTicketDetail matchSeatTicketDetail = _matchSeatTicketDetailRepository.Get(item.MatchSeatTicketDetailId);
                                        matchSeatTicketDetail.BarcodeNumber = BarcodeNumber + "0" + ((short)Channels.Website).ToString() + matchSeatTicketDetail.MatchLayoutSectionSeatId + uniqueId;
                                        matchSeatTicketDetail.Price = transactionDetail.PricePerTicket;
                                        matchSeatTicketDetail.SeatStatusId = SeatStatus.Sold;
                                        matchSeatTicketDetail.TransactionId = transactionDetail.TransactionId;
                                        matchSeatTicketDetail.ModifiedBy = modifiedBy;
                                        matchSeatTicketDetail.TicketTypeId = (TicketType)transactionDetail.TicketTypeId;
                                        matchSeatTicketDetail.ChannelId = Channels.Website;
                                        _matchSeatTicketDetailRepository.Save(matchSeatTicketDetail);
                                    }
                                }
                                else
                                {
                                    if (transactionDetail.TotalTickets > 0)
                                    {
                                        if (transactionDetail.EventTicketAttributeId == 1657017 || transactionDetail.EventTicketAttributeId == 1657022 || transactionDetail.EventTicketAttributeId == 1657039 || transactionDetail.EventTicketAttributeId == 1657042) // For 2 adults and 2 child
                                        {
                                            MatchSeatTicketDetail matchSeatTicketDetail = new MatchSeatTicketDetail();
                                            for (int i = 1; i <= transactionDetail.TotalTickets * 2; i++)
                                            {
                                                Random rd = new Random();
                                                BarcodeNumber = eventDetails.StartDateTime.ToString("ddMM");
                                                int uniqueIdLength = 4;
                                                string uniqueIdChars = "1234567890";
                                                char[] uniqueIdChar = new char[uniqueIdLength];
                                                for (int j = 0; j < uniqueIdLength; j++)
                                                {
                                                    uniqueIdChar[j] = uniqueIdChars[rd.Next(0, uniqueIdChars.Length)];
                                                }
                                                string uniqueId = new string(uniqueIdChar);
                                                matchSeatTicketDetail = _matchSeatTicketDetailRepository.GetByEventTicketDetailsId(eventTicketDetails.Id);
                                                matchSeatTicketDetail.BarcodeNumber = BarcodeNumber + "0" + ((short)Channels.Website).ToString() + matchSeatTicketDetail.MatchLayoutSectionSeatId + uniqueId;
                                                matchSeatTicketDetail.Price = transactionDetail.PricePerTicket;
                                                matchSeatTicketDetail.SeatStatusId = SeatStatus.Sold;
                                                matchSeatTicketDetail.TransactionId = transactionDetail.TransactionId;
                                                matchSeatTicketDetail.ModifiedBy = modifiedBy;
                                                matchSeatTicketDetail.TicketTypeId = TicketType.Regular;
                                                matchSeatTicketDetail.ChannelId = Channels.Website;
                                                _matchSeatTicketDetailRepository.Save(matchSeatTicketDetail);
                                            }
                                            for (int i = 1; i <= transactionDetail.TotalTickets * 2; i++)
                                            {
                                                Random rd = new Random();
                                                BarcodeNumber = eventDetails.StartDateTime.ToString("ddMM");
                                                int uniqueIdLength = 4;
                                                string uniqueIdChars = "1234567890";
                                                char[] uniqueIdChar = new char[uniqueIdLength];
                                                for (int j = 0; j < uniqueIdLength; j++)
                                                {
                                                    uniqueIdChar[j] = uniqueIdChars[rd.Next(0, uniqueIdChars.Length)];
                                                }
                                                string uniqueId = new string(uniqueIdChar);
                                                matchSeatTicketDetail = _matchSeatTicketDetailRepository.GetByEventTicketDetailsId(eventTicketDetails.Id);
                                                matchSeatTicketDetail.BarcodeNumber = BarcodeNumber + "0" + ((short)Channels.Website).ToString() + matchSeatTicketDetail.MatchLayoutSectionSeatId + uniqueId;
                                                matchSeatTicketDetail.Price = transactionDetail.PricePerTicket;
                                                matchSeatTicketDetail.SeatStatusId = SeatStatus.Sold;
                                                matchSeatTicketDetail.TransactionId = transactionDetail.TransactionId;
                                                matchSeatTicketDetail.ModifiedBy = modifiedBy;
                                                matchSeatTicketDetail.TicketTypeId = TicketType.Child;
                                                matchSeatTicketDetail.ChannelId = Channels.Website;
                                                _matchSeatTicketDetailRepository.Save(matchSeatTicketDetail);
                                            }
                                        }
                                        else if (transactionDetail.EventTicketAttributeId == 1657018 || transactionDetail.EventTicketAttributeId == 1657040) //1 adult 3 child
                                        {
                                            for (int i = 1; i <= transactionDetail.TotalTickets * 1; i++)
                                            {
                                                Random rd = new Random();
                                                BarcodeNumber = eventDetails.StartDateTime.ToString("ddMM");
                                                int uniqueIdLength = 4;
                                                string uniqueIdChars = "1234567890";
                                                char[] uniqueIdChar = new char[uniqueIdLength];
                                                for (int j = 0; j < uniqueIdLength; j++)
                                                {
                                                    uniqueIdChar[j] = uniqueIdChars[rd.Next(0, uniqueIdChars.Length)];
                                                }
                                                string uniqueId = new string(uniqueIdChar);
                                                MatchSeatTicketDetail matchSeatTicketDetail = _matchSeatTicketDetailRepository.GetByEventTicketDetailsId(eventTicketDetails.Id);
                                                matchSeatTicketDetail.BarcodeNumber = BarcodeNumber + "0" + ((short)Channels.Website).ToString() + matchSeatTicketDetail.MatchLayoutSectionSeatId + uniqueId;
                                                matchSeatTicketDetail.Price = transactionDetail.PricePerTicket;
                                                matchSeatTicketDetail.SeatStatusId = SeatStatus.Sold;
                                                matchSeatTicketDetail.TransactionId = transactionDetail.TransactionId;
                                                matchSeatTicketDetail.ModifiedBy = modifiedBy;
                                                matchSeatTicketDetail.TicketTypeId = TicketType.Regular;
                                                matchSeatTicketDetail.ChannelId = Channels.Website;
                                                _matchSeatTicketDetailRepository.Save(matchSeatTicketDetail);
                                            }
                                            for (int i = 1; i <= transactionDetail.TotalTickets * 3; i++)
                                            {
                                                Random rd = new Random();
                                                BarcodeNumber = eventDetails.StartDateTime.ToString("ddMM");
                                                int uniqueIdLength = 4;
                                                string uniqueIdChars = "1234567890";
                                                char[] uniqueIdChar = new char[uniqueIdLength];
                                                for (int j = 0; j < uniqueIdLength; j++)
                                                {
                                                    uniqueIdChar[j] = uniqueIdChars[rd.Next(0, uniqueIdChars.Length)];
                                                }
                                                string uniqueId = new string(uniqueIdChar);
                                                MatchSeatTicketDetail matchSeatTicketDetail = _matchSeatTicketDetailRepository.GetByEventTicketDetailsId(eventTicketDetails.Id);
                                                matchSeatTicketDetail.BarcodeNumber = BarcodeNumber + "0" + ((short)Channels.Website).ToString() + matchSeatTicketDetail.MatchLayoutSectionSeatId + uniqueId;
                                                matchSeatTicketDetail.Price = transactionDetail.PricePerTicket;
                                                matchSeatTicketDetail.SeatStatusId = SeatStatus.Sold;
                                                matchSeatTicketDetail.TransactionId = transactionDetail.TransactionId;
                                                matchSeatTicketDetail.ModifiedBy = modifiedBy;
                                                matchSeatTicketDetail.TicketTypeId = TicketType.Child;
                                                matchSeatTicketDetail.ChannelId = Channels.Website;
                                                _matchSeatTicketDetailRepository.Save(matchSeatTicketDetail);
                                            }
                                        }
                                        else if (transactionDetail.EventTicketAttributeId == 1657038) //2 day vip pass
                                        {
                                            for (int i = 1; i <= transactionDetail.TotalTickets * 4; i++)
                                            {
                                                Random rd = new Random();
                                                BarcodeNumber = eventDetails.StartDateTime.ToString("ddMM");
                                                int uniqueIdLength = 4;
                                                string uniqueIdChars = "1234567890";
                                                char[] uniqueIdChar = new char[uniqueIdLength];
                                                for (int j = 0; j < uniqueIdLength; j++)
                                                {
                                                    uniqueIdChar[j] = uniqueIdChars[rd.Next(0, uniqueIdChars.Length)];
                                                }
                                                string uniqueId = new string(uniqueIdChar);
                                                MatchSeatTicketDetail matchSeatTicketDetail = _matchSeatTicketDetailRepository.GetByEventTicketDetailsId(eventTicketDetails.Id);
                                                matchSeatTicketDetail.BarcodeNumber = BarcodeNumber + "0" + ((short)Channels.Website).ToString() + matchSeatTicketDetail.MatchLayoutSectionSeatId + uniqueId;
                                                matchSeatTicketDetail.Price = transactionDetail.PricePerTicket;
                                                matchSeatTicketDetail.SeatStatusId = SeatStatus.Sold;
                                                matchSeatTicketDetail.TransactionId = transactionDetail.TransactionId;
                                                matchSeatTicketDetail.ModifiedBy = modifiedBy;
                                                matchSeatTicketDetail.TicketTypeId = TicketType.Regular;
                                                matchSeatTicketDetail.ChannelId = Channels.Website;
                                                _matchSeatTicketDetailRepository.Save(matchSeatTicketDetail);
                                            }
                                        }
                                        else
                                        {
                                            for (int i = 1; i <= transactionDetail.TotalTickets; i++)
                                            {
                                                Random rd = new Random();
                                                MatchSeatTicketDetail matchSeatTicketDetail = new MatchSeatTicketDetail();

                                                BarcodeNumber = eventDetails.StartDateTime.ToString("ddMM");
                                                int uniqueIdLength = 4;
                                                string uniqueIdChars = "1234567890";
                                                char[] uniqueIdChar = new char[uniqueIdLength];
                                                for (int j = 0; j < uniqueIdLength; j++)
                                                {
                                                    uniqueIdChar[j] = uniqueIdChars[rd.Next(0, uniqueIdChars.Length)];
                                                }
                                                string uniqueId = new string(uniqueIdChar);

                                                if (eventTicketDetails != null)
                                                {
                                                    matchSeatTicketDetail = _matchSeatTicketDetailRepository.GetByEventTicketDetailsId(eventTicketDetails.Id);
                                                }
                                                matchSeatTicketDetail.BarcodeNumber = BarcodeNumber + "0" + ((short)Channels.Website).ToString() + matchSeatTicketDetail.MatchLayoutSectionSeatId + uniqueId;
                                                matchSeatTicketDetail.Price = transactionDetail.PricePerTicket;
                                                matchSeatTicketDetail.SeatStatusId = SeatStatus.Sold;
                                                matchSeatTicketDetail.TransactionId = transactionDetail.TransactionId;
                                                matchSeatTicketDetail.ModifiedBy = modifiedBy;
                                                matchSeatTicketDetail.TicketTypeId = (TicketType)transactionDetail.TicketTypeId;
                                                matchSeatTicketDetail.ChannelId = Channels.Website;
                                                _matchSeatTicketDetailRepository.Save(matchSeatTicketDetail);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.Log(Logging.Enums.LogCategory.Error, ex);
                }
            }
            catch (Exception e)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, e);
            }
        }
    }
}