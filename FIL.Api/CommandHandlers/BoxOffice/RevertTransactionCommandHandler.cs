using FIL.Api.Repositories;
using FIL.Contracts.Commands.BoxOffice;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using MediatR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.BoxOffice
{
    public class RevertTransactionCommandHandler : BaseCommandHandler<RevertTransactionCommand>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionDetailRepository _transactionDetailRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly IMatchSeatTicketDetailRepository _matchSeatTicketDetailRepository;
        private readonly IVoidRequestDetailRepository _voidRequestDetailRepository;
        private readonly ITransactionBarcodeReversalLogRepository _transactionBarcodeReversalLogRepository;

        public RevertTransactionCommandHandler(ITransactionRepository transactionRepository, ITransactionDetailRepository transactionDetailRepository, IEventTicketAttributeRepository eventTicketAttributeRepository, IMatchSeatTicketDetailRepository matchSeatTicketDetailRepository, IVoidRequestDetailRepository voidRequestDetailRepository, ITransactionBarcodeReversalLogRepository transactionBarcodeReversalLogRepository, IMediator mediator) : base(mediator)
        {
            _transactionRepository = transactionRepository;
            _transactionDetailRepository = transactionDetailRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _matchSeatTicketDetailRepository = matchSeatTicketDetailRepository;
            _voidRequestDetailRepository = voidRequestDetailRepository;
            _transactionBarcodeReversalLogRepository = transactionBarcodeReversalLogRepository;
        }

        protected override async Task Handle(RevertTransactionCommand command)
        {
            var transactionDetails = _transactionDetailRepository.GetByTransactionId(command.TransactionId)
                .ToList();
            var matchSeatTicketDetails = _matchSeatTicketDetailRepository.GetbyTransactionId(command.TransactionId);
            var eventTicketAttributeLookup = _eventTicketAttributeRepository.GetByIds(transactionDetails.Select(i => i.EventTicketAttributeId).Distinct())
                .ToDictionary(eta => eta.Id);
            foreach (TransactionDetail item in transactionDetails)
            {
                var eventTicketAttributes = eventTicketAttributeLookup[item.EventTicketAttributeId];
                var remainingTicketForSale = 0;
                var childQTY = 0;
                var srCitizenQTY = 0;
                if (item.TicketTypeId == 1)
                {
                    remainingTicketForSale = eventTicketAttributes.RemainingTicketForSale + item.TotalTickets;
                    childQTY = (int)eventTicketAttributes.ChildQTY;
                    srCitizenQTY = (int)eventTicketAttributes.SRCitizenQTY;
                }
                if (item.TicketTypeId == 2)
                {
                    remainingTicketForSale = eventTicketAttributes.RemainingTicketForSale + item.TotalTickets;
                    childQTY = (int)eventTicketAttributes.ChildQTY + item.TotalTickets;
                    srCitizenQTY = (int)eventTicketAttributes.SRCitizenQTY;
                }
                if (item.TicketTypeId == 3)
                {
                    remainingTicketForSale = eventTicketAttributes.RemainingTicketForSale + item.TotalTickets;
                    childQTY = (int)eventTicketAttributes.ChildQTY;
                    srCitizenQTY = (int)eventTicketAttributes.SRCitizenQTY + item.TotalTickets;
                }
                eventTicketAttributes.Id = item.EventTicketAttributeId;
                eventTicketAttributes.RemainingTicketForSale = remainingTicketForSale;
                eventTicketAttributes.ChildQTY = (short)childQTY;
                eventTicketAttributes.SRCitizenQTY = (short)srCitizenQTY;
                _eventTicketAttributeRepository.Save(eventTicketAttributes);
            }

            var matchSeatTicketDetailData = new MatchSeatTicketDetail();
            var transactionBarcodeReversalLogData = new TransactionBarcodeReversalLog();
            foreach (MatchSeatTicketDetail item in matchSeatTicketDetails)
            {
                matchSeatTicketDetailData = new MatchSeatTicketDetail
                {
                    Id = item.Id,
                    AltId = item.AltId,
                    MatchLayoutSectionSeatId = item.MatchLayoutSectionSeatId,
                    EventTicketDetailId = item.EventTicketDetailId,
                    Price = item.Price,
                    IsEnabled = item.IsEnabled,
                    CreatedUtc = item.CreatedUtc,
                    SeatStatusId = SeatStatus.UnSold,
                    BarcodeNumber = null,
                    PrintStatusId = PrintStatus.NotPrinted,
                    PrintCount = 0,
                    TicketTypeId = 0,
                    TransactionId = null,
                    EntryGateName = null,
                };
                _matchSeatTicketDetailRepository.Save(matchSeatTicketDetailData);

                transactionBarcodeReversalLogData = new TransactionBarcodeReversalLog
                {
                    TransactionId = command.TransactionId,
                    BarcodeNumber = item.BarcodeNumber,
                    IsEnabled = true,
                    CreatedBy = command.ModifiedBy,
                };
                _transactionBarcodeReversalLogRepository.Save(transactionBarcodeReversalLogData);
            }

            var transactions = _transactionRepository.Get(command.TransactionId);
            transactions.Id = command.TransactionId;
            transactions.TransactionStatusId = TransactionStatus.None;
            _transactionRepository.Save(transactions);

            var voidRequestDetails = _voidRequestDetailRepository.GetByTransId(command.TransactionId);
            voidRequestDetails.TransactionId = command.TransactionId;
            voidRequestDetails.IsVoid = true;
            voidRequestDetails.VoidDateTime = DateTime.UtcNow;
            _voidRequestDetailRepository.Save(voidRequestDetails);
        }
    }
}