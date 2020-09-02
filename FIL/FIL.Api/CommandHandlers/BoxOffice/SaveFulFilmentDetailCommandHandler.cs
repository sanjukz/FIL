using FIL.Api.Repositories;
using FIL.Contracts.Commands.BoxOffice;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.BoxOffice
{
    public class SaveFulFilmentDetailCommandHandler : BaseCommandHandler<SaveFulFilmentDetailCommand>
    {
        private readonly ITransactionDeliveryDetailRepository _transactionDeliveryDetailRepository;

        public SaveFulFilmentDetailCommandHandler(ITransactionDeliveryDetailRepository transactionDeliveryDetailRepository, IMediator mediator) : base(mediator)
        {
            _transactionDeliveryDetailRepository = transactionDeliveryDetailRepository;
        }

        protected override Task Handle(SaveFulFilmentDetailCommand command)
        {
            var transactionDeliveryDetail = _transactionDeliveryDetailRepository.GetByTransactionDetailId(Convert.ToInt64(command.TransactionDetailId));
            transactionDeliveryDetail.PickupBy = 1;
            transactionDeliveryDetail.TicketNumber = command.TicketNumber;
            _transactionDeliveryDetailRepository.Save(transactionDeliveryDetail);
            return Task.FromResult(0);
        }
    }
}