using FIL.Api.Repositories;
using FIL.Contracts.Commands.OrderConfirmationSuccess;
using FIL.Contracts.Enums;
using MediatR;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.OrderConfirmationSuccess
{
    public class OrderConfirmationSuccessCommandHandler : BaseCommandHandler<OrderConfirmationSuccessCommand>
    {
        private readonly ITransactionRepository _transactionRepository;

        public OrderConfirmationSuccessCommandHandler(ITransactionRepository transactionRepository, IMediator mediator)
            : base(mediator)
        {
            _transactionRepository = transactionRepository;
        }

        protected override async Task Handle(OrderConfirmationSuccessCommand command)
        {
            FIL.Contracts.DataModels.Transaction transaction = _transactionRepository.GetUnderPaymentTransactionDetails(command.TransactionId);

            if (transaction != null)
            {
                transaction.TransactionStatusId = TransactionStatus.Success;
                _transactionRepository.Save(transaction);
            }
        }
    }
}