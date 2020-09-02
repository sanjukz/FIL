using FIL.Api.CommandHandlers;
using FIL.Api.Repositories;
using FIL.Contracts.Commands.TransactionStripeConnectTransfers;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.QueryHandlers.TransactionStripeConnectTransfers
{
    public class TransactionStripeConnectTransfersCommandHandler : BaseCommandHandler<TransactionStripeConnectTransfersCommand>
    {
        private readonly ITransactionStripeConnectTransfersRepository _transactionStripeConnectTransfersRepository;

        public TransactionStripeConnectTransfersCommandHandler(ITransactionStripeConnectTransfersRepository transactionStripeConnectTransfersRepository, IMediator mediator) : base(mediator)
        {
            _transactionStripeConnectTransfersRepository = transactionStripeConnectTransfersRepository;
        }

        protected override async Task Handle(TransactionStripeConnectTransfersCommand command)
        {
            try
            {
                var transactionStripeConnectTransfers = _transactionStripeConnectTransfersRepository.Get(command.Id);
                transactionStripeConnectTransfers.TransferApiResponse = command.TransferApiResponse;
                transactionStripeConnectTransfers.TransferDateActual = command.TransferDateActual;
                _transactionStripeConnectTransfersRepository.Save(transactionStripeConnectTransfers);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}