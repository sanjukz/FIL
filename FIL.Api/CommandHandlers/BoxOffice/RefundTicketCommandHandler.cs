using FIL.Api.Repositories;
using FIL.Contracts.Commands.BoxOffice;
using FIL.Contracts.DataModels;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.BoxOffice
{
    public class RefundTicketCommandHandler : BaseCommandHandler<RefundTicketCommand>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionDetailRepository _transactionDetailRepository;
        private readonly IMatchSeatTicketDetailRepository _matchSeatTicketDetailRepository;
        private readonly ITicketRefundDetailRepository _ticketRefundDetailRepository;

        public RefundTicketCommandHandler(ITransactionRepository transactionRepository, ITransactionDetailRepository transactionDetailRepository, IMatchSeatTicketDetailRepository matchSeatTicketDetailRepository, ITicketRefundDetailRepository ticketRefundDetailRepository, IMediator mediator) : base(mediator)
        {
            _transactionRepository = transactionRepository;
            _transactionDetailRepository = transactionDetailRepository;
            _matchSeatTicketDetailRepository = matchSeatTicketDetailRepository;
            _ticketRefundDetailRepository = ticketRefundDetailRepository;
        }

        protected override async Task Handle(RefundTicketCommand command)
        {
            var ticketRefundDetails = _ticketRefundDetailRepository.GetByBarcodeNumber(command.BarcodeNumber);
            if (ticketRefundDetails == null)
            {
                var matchSeatTicketDetails = _matchSeatTicketDetailRepository.GetByBarcodeNumber(command.BarcodeNumber);
                if (matchSeatTicketDetails != null)
                {
                    var ticketRefundDetail = new TicketRefundDetail
                    {
                        BarcodeNumber = command.BarcodeNumber,
                        TransactionId = (long)matchSeatTicketDetails.TransactionId,
                        RefundedAmount = command.RefundedAmount,
                        IsEnabled = true,
                        CreatedUtc = DateTime.UtcNow,
                        ModifiedBy = command.ModifiedBy
                    };
                    _ticketRefundDetailRepository.Save(ticketRefundDetail);
                }
            }
        }
    }
}