using FIL.Api.Repositories;
using FIL.Contracts.Commands.BoxOffice;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.BoxOffice
{
    public class ApprovePartialVoidCommandHandler : BaseCommandHandler<ApprovePartialVoidCommand>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionDetailRepository _transactionDetailRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly IMatchSeatTicketDetailRepository _matchSeatTicketDetailRepository;
        private readonly IVoidRequestDetailRepository _voidRequestDetailRepository;
        private readonly IPartialVoidRequestDetailRepository _partialVoidRequestDetailRepository;
        private readonly ITransactionBarcodeReversalLogRepository _transactionBarcodeReversalLogRepository;
        private readonly ITicketRefundDetailRepository _ticketRefundDetailRepository;
        private readonly Logging.ILogger _logger;

        public ApprovePartialVoidCommandHandler(ITransactionRepository transactionRepository, ITransactionDetailRepository transactionDetailRepository,
            IEventTicketAttributeRepository eventTicketAttributeRepository, IEventTicketDetailRepository eventTicketDetailRepository,
            IMatchSeatTicketDetailRepository matchSeatTicketDetailRepository, IVoidRequestDetailRepository voidRequestDetailRepository, IPartialVoidRequestDetailRepository partialVoidRequestDetailRepository,
            ITransactionBarcodeReversalLogRepository transactionBarcodeReversalLogRepository, ITicketRefundDetailRepository ticketRefundDetailRepository, Logging.ILogger logger, IMediator mediator) : base(mediator)
        {
            _transactionRepository = transactionRepository;
            _transactionDetailRepository = transactionDetailRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _matchSeatTicketDetailRepository = matchSeatTicketDetailRepository;
            _voidRequestDetailRepository = voidRequestDetailRepository;
            _transactionBarcodeReversalLogRepository = transactionBarcodeReversalLogRepository;
            _ticketRefundDetailRepository = ticketRefundDetailRepository;
            _partialVoidRequestDetailRepository = partialVoidRequestDetailRepository;
            _logger = logger;
        }

        protected override async Task Handle(ApprovePartialVoidCommand command)
        {
            try
            {
                var matchSeatTicketDetails = _matchSeatTicketDetailRepository.GetByBarcodeList(command.Barcodes.Distinct());

                var eventTicketDetailsModel = _eventTicketDetailRepository
                          .GetByIds(matchSeatTicketDetails.Select(msd => msd.EventTicketDetailId).Distinct())
                          .ToDictionary(etd => etd.Id);

                var eventTicketAttributeModel = _eventTicketAttributeRepository
                    .GetByEventTicketDetailIds(eventTicketDetailsModel.Values.Select(etd => etd.Id).Distinct())
                .ToDictionary(eta => eta.EventTicketDetailId);

                var transactionsModel = _transactionRepository
                    .GetByTransactionIds(matchSeatTicketDetails.Select(mstd => mstd.TransactionId))
                    .Distinct()
                    .ToDictionary(t => t.Id);

                if (matchSeatTicketDetails.Any())
                {
                    foreach (MatchSeatTicketDetail item in matchSeatTicketDetails)
                    {
                        int remainingTicketForSale = 0;
                        int childQTY = 0;
                        int srCitizenQTY = 0;
                        decimal? discountAmount = 0;
                        short remainingTicket = 0;
                        decimal? netTicketAmount = 0;
                        decimal? grossTicketAmount = 0;

                        var eventTicketDetails = eventTicketDetailsModel[item.EventTicketDetailId];
                        var eventTicketAttributes = eventTicketAttributeModel[eventTicketDetails.Id];
                        var transactions = transactionsModel[(long)item.TransactionId];
                        var transactionDetails = _transactionDetailRepository
                            .GetByEventTicketAttributeIdAndTransactionIdAndTicketTypeId(transactions.Id, eventTicketAttributes.Id, (short)item.TicketTypeId);
                        remainingTicket = Convert.ToInt16(transactionDetails.TotalTickets - 1);

                        if (transactionDetails.DiscountAmount > 0)
                        {
                            if (transactionDetails.TotalTickets == 1)
                            {
                                discountAmount = 0;
                                grossTicketAmount = 0;
                                netTicketAmount = 0;
                            }
                            else
                            {
                                discountAmount = (transactionDetails.DiscountAmount / transactionDetails.TotalTickets);
                                grossTicketAmount = transactions.GrossTicketAmount - (transactionDetails.PricePerTicket - discountAmount);
                                netTicketAmount = transactions.NetTicketAmount - (transactionDetails.PricePerTicket);
                            }
                        }
                        else
                        {
                            discountAmount = 0;
                            grossTicketAmount = transactions.GrossTicketAmount - transactionDetails.PricePerTicket;
                            netTicketAmount = transactions.NetTicketAmount - (transactionDetails.PricePerTicket - discountAmount);
                        }
                        var transactionDetailData = _transactionDetailRepository.Get(transactionDetails.Id);
                        transactionDetailData.TotalTickets = Convert.ToInt16(transactionDetailData.TotalTickets - 1);
                        transactionDetailData.DiscountAmount = discountAmount;
                        transactionDetailData.ModifiedBy = command.ModifiedBy;
                        _transactionDetailRepository.Save(transactionDetailData);

                        var transactionData = _transactionRepository.Get(transactions.Id);
                        if (transactionData.TotalTickets == 1)
                        {
                            transactionData.TotalTickets = remainingTicket;
                            transactionData.DiscountAmount = discountAmount;
                            transactionData.GrossTicketAmount = grossTicketAmount;
                            transactionData.NetTicketAmount = netTicketAmount;
                            transactionData.ModifiedBy = command.ModifiedBy;
                            transactionData.TransactionStatusId = TransactionStatus.Reverted;
                        }
                        else
                        {
                            transactionData.TotalTickets = Convert.ToInt16(transactionData.TotalTickets - 1);
                            transactionData.DiscountAmount = discountAmount;
                            transactionData.GrossTicketAmount = grossTicketAmount;
                            transactionData.NetTicketAmount = netTicketAmount;
                            transactionData.ModifiedBy = command.ModifiedBy;
                        }

                        _transactionRepository.Save(transactionData);

                        if (item.TicketTypeId == TicketType.Regular || item.TicketTypeId == TicketType.SeasonPackage)
                        {
                            remainingTicketForSale = eventTicketAttributes.RemainingTicketForSale + 1;
                            childQTY = (int)eventTicketAttributes.ChildQTY;
                            srCitizenQTY = (int)eventTicketAttributes.SRCitizenQTY;
                        }
                        if (item.TicketTypeId == TicketType.Child)
                        {
                            remainingTicketForSale = eventTicketAttributes.RemainingTicketForSale + 1;
                            childQTY = (int)eventTicketAttributes.ChildQTY + 1;
                            srCitizenQTY = (int)eventTicketAttributes.SRCitizenQTY;
                        }
                        if (item.TicketTypeId == TicketType.SeniorCitizen)
                        {
                            remainingTicketForSale = eventTicketAttributes.RemainingTicketForSale + 1;
                            childQTY = (int)eventTicketAttributes.ChildQTY;
                            srCitizenQTY = (int)eventTicketAttributes.SRCitizenQTY + 1;
                        }

                        eventTicketAttributes.RemainingTicketForSale = remainingTicketForSale;
                        eventTicketAttributes.ChildQTY = (short)childQTY;
                        eventTicketAttributes.SRCitizenQTY = (short)srCitizenQTY;
                        _eventTicketAttributeRepository.Save(eventTicketAttributes);

                        var matchSeatTicketDetailData = _matchSeatTicketDetailRepository.Get(item.Id);
                        matchSeatTicketDetailData.Price = item.Price;
                        matchSeatTicketDetailData.SeatStatusId = SeatStatus.UnSold;
                        matchSeatTicketDetailData.BarcodeNumber = null;
                        matchSeatTicketDetailData.PrintStatusId = PrintStatus.NotPrinted;
                        matchSeatTicketDetailData.PrintCount = 0;
                        matchSeatTicketDetailData.TicketTypeId = TicketType.None;
                        matchSeatTicketDetailData.TransactionId = null;
                        matchSeatTicketDetailData.EntryGateName = null;
                        matchSeatTicketDetailData.SponsorId = null;
                        matchSeatTicketDetailData.ModifiedBy = command.ModifiedBy;
                        _matchSeatTicketDetailRepository.Save(matchSeatTicketDetailData);

                        var transactionBarcodeReversalLogData = new TransactionBarcodeReversalLog
                        {
                            TransactionId = transactions.Id,
                            BarcodeNumber = item.BarcodeNumber,
                            IsEnabled = true,
                            ModifiedBy = command.ModifiedBy,
                        };
                        _transactionBarcodeReversalLogRepository.Save(transactionBarcodeReversalLogData);
                        var a = command.Barcodes.First();
                        var partialvoidrequestDetails = _partialVoidRequestDetailRepository.GetByBarcodeNumber(item.BarcodeNumber);
                        partialvoidrequestDetails.IsPartialVoid = true;
                        partialvoidrequestDetails.PartialVoidDateTime = DateTime.UtcNow;
                        partialvoidrequestDetails.ModifiedBy = command.ModifiedBy;
                        _partialVoidRequestDetailRepository.Save(partialvoidrequestDetails);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
            }
        }
    }
}