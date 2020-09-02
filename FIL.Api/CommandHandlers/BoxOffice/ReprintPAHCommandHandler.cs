using FIL.Api.Repositories;
using FIL.Contracts.Commands.BoxOffice;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.BoxOffice
{
    public class ReprintPAHCommandHandler : BaseCommandHandlerWithResult<ReprintPAHCommand, ReprintPAHCommandResult>
    {
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionDetailRepository _transactionDetailRepository;
        private readonly ITransactionSeatDetailRepository _transactionSeatDetailRepository;
        private readonly IMatchLayoutSectionSeatRepository _matchLayoutSectionSeatRepository;
        private readonly IMatchSeatTicketDetailRepository _matchSeatTicketDetailRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;

        public ReprintPAHCommandHandler(IEventDetailRepository eventDetailRepository, ITransactionRepository transactionRepository, ITransactionDetailRepository transactionDetailRepository, ITransactionSeatDetailRepository transactionSeatDetailRepository, IMatchLayoutSectionSeatRepository matchLayoutSectionSeatRepository, IMatchSeatTicketDetailRepository matchSeatTicketDetailRepository, IEventTicketDetailRepository eventTicketDetailRepository, IEventTicketAttributeRepository eventTicketAttributeRepository, IMediator mediator) : base(mediator)
        {
            _eventDetailRepository = eventDetailRepository;
            _transactionRepository = transactionRepository;
            _transactionDetailRepository = transactionDetailRepository;
            _transactionSeatDetailRepository = transactionSeatDetailRepository;
            _matchLayoutSectionSeatRepository = matchLayoutSectionSeatRepository;
            _matchSeatTicketDetailRepository = matchSeatTicketDetailRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
        }

        protected override Task<ICommandResult> Handle(ReprintPAHCommand command)
        {
            ReprintPAHCommandResult reprintPAHCommandResult = new ReprintPAHCommandResult();
            try
            {
                var transaction = AutoMapper.Mapper.Map<FIL.Contracts.DataModels.Transaction>(_transactionRepository.Get(command.TransactionId));
                if (transaction != null)
                {
                    if (transaction.TransactionStatusId == TransactionStatus.Success)
                    {
                        var matchSeatTicketDetails = _matchSeatTicketDetailRepository.GetByTransactionId(transaction.Id);
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
                                        matchSeatTicketDetail.BarcodeNumber = BarcodeNumber + "0" + ((short)command.ChannelId).ToString() + matchSeatTicketDetail.MatchLayoutSectionSeatId + uniqueId;
                                        matchSeatTicketDetail.Price = transactionDetail.PricePerTicket;
                                        matchSeatTicketDetail.SeatStatusId = SeatStatus.Sold;
                                        matchSeatTicketDetail.TransactionId = transactionDetail.TransactionId;
                                        matchSeatTicketDetail.ModifiedBy = command.ModifiedBy;
                                        matchSeatTicketDetail.TicketTypeId = (TicketType)transactionDetail.TicketTypeId;
                                        matchSeatTicketDetail.ChannelId = command.ChannelId;
                                        _matchSeatTicketDetailRepository.Save(matchSeatTicketDetail);
                                    }
                                }
                                else
                                {
                                    if (transactionDetail.TotalTickets > 0)
                                    {
                                        for (int i = 1; i <= transactionDetail.TotalTickets; i++)
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
                                            matchSeatTicketDetail.BarcodeNumber = BarcodeNumber + "0" + ((short)command.ChannelId).ToString() + matchSeatTicketDetail.MatchLayoutSectionSeatId + uniqueId;
                                            matchSeatTicketDetail.Price = transactionDetail.PricePerTicket;
                                            matchSeatTicketDetail.SeatStatusId = SeatStatus.Sold;
                                            matchSeatTicketDetail.TransactionId = transactionDetail.TransactionId;
                                            matchSeatTicketDetail.ModifiedBy = command.ModifiedBy;
                                            matchSeatTicketDetail.TicketTypeId = (TicketType)transactionDetail.TicketTypeId;
                                            matchSeatTicketDetail.ChannelId = command.ChannelId;
                                            _matchSeatTicketDetailRepository.Save(matchSeatTicketDetail);
                                        }
                                    }
                                }
                            }
                            var matchSeatTicketDetaillist = _matchSeatTicketDetailRepository.GetbyTransactionId(transaction.Id)
                                .ToList();

                            if (matchSeatTicketDetaillist.Count != 0)
                            {
                                foreach (var matchDetailsItem in matchSeatTicketDetaillist)
                                {
                                    if (matchDetailsItem.EntryStatus != true)
                                    {
                                        matchDetailsItem.PrintStatusId = PrintStatus.NotPrinted;
                                        _matchSeatTicketDetailRepository.Save(matchDetailsItem);
                                    }
                                }

                                reprintPAHCommandResult.Id = transaction.Id;
                                reprintPAHCommandResult.Success = true;
                                reprintPAHCommandResult.MatchSeatTicketDetail = matchSeatTicketDetaillist;
                            }
                        }
                        else
                        {
                            List<MatchSeatTicketDetail> matchSeatTicketDetaillist = new List<MatchSeatTicketDetail>();
                            matchSeatTicketDetaillist = AutoMapper.Mapper.Map<List<MatchSeatTicketDetail>>(_matchSeatTicketDetailRepository.GetbyTransactionId(transaction.Id));

                            if (matchSeatTicketDetaillist != null && matchSeatTicketDetaillist.Count != 0)
                            {
                                foreach (var matchDetailsItem in matchSeatTicketDetaillist)
                                {
                                    if (matchDetailsItem.EntryStatus != true)
                                    {
                                        MatchSeatTicketDetail matchSeatTicketDetail = _matchSeatTicketDetailRepository.Get(matchDetailsItem.Id);
                                        matchSeatTicketDetail.PrintStatusId = PrintStatus.NotPrinted;
                                        _matchSeatTicketDetailRepository.Save(matchSeatTicketDetail);
                                    }
                                }
                                reprintPAHCommandResult.Id = transaction.Id;
                                reprintPAHCommandResult.Success = true;
                                reprintPAHCommandResult.MatchSeatTicketDetail = matchSeatTicketDetaillist;
                            }
                        }
                    }
                    else
                    {
                        reprintPAHCommandResult.Id = -1;
                        reprintPAHCommandResult.Success = false;
                        reprintPAHCommandResult.MatchSeatTicketDetail = null;
                    }
                }
                else
                {
                    reprintPAHCommandResult.Id = -1;
                    reprintPAHCommandResult.Success = false;
                    reprintPAHCommandResult.MatchSeatTicketDetail = null;
                }
            }
            catch (Exception ex)
            {
                reprintPAHCommandResult.Id = -1;
                reprintPAHCommandResult.Success = false;
                reprintPAHCommandResult.MatchSeatTicketDetail = null;
            }
            return Task.FromResult<ICommandResult>(reprintPAHCommandResult);
        }
    }
}