using FIL.Api.Repositories;
using FIL.Contracts.Commands.BoxOffice;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.BoxOffice
{
    public class SaveOTPCommandHandler : BaseCommandHandler<SaveOTPCommand>
    {
        private readonly ITransactionDeliveryDetailRepository _transactionDeliveryDetailRepository;
        private readonly ITransactionDetailRepository _transactionDetailRepository;

        public SaveOTPCommandHandler(ITransactionDeliveryDetailRepository transactionDeliveryDetailRepository, ITransactionDetailRepository transactionDetailRepository, IMediator mediator) : base(mediator)
        {
            _transactionDeliveryDetailRepository = transactionDeliveryDetailRepository;
            _transactionDetailRepository = transactionDetailRepository;
        }

        protected override Task Handle(SaveOTPCommand command)
        {
            var transactionDetail = _transactionDetailRepository.GetByAltId((System.Guid)command.TransDetailAltId);
            var transactionDeliveryDetails = _transactionDeliveryDetailRepository.GetByTransactionDetailId(transactionDetail.Id);
            transactionDeliveryDetails.PickupOTP = Convert.ToInt32(command.PickupOTP);
            _transactionDeliveryDetailRepository.Save(transactionDeliveryDetails);
            return Task.FromResult(0);
        }
    }
}