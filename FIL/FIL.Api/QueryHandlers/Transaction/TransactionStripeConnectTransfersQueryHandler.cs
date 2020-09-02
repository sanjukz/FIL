using FIL.Api.Repositories;
using FIL.Contracts.Queries.TransactionStripeConnectTransfers;
using FIL.Contracts.QueryResults.TransactionStripeConnectTransfers;
using System.Collections.Generic;

namespace FIL.Api.QueryHandlers.Transactions
{
    public class TransactionStripeConnectTransfersQueryHandler : IQueryHandler<TransactionStripeConnectTransfersQuery, TransactionStripeConnectTransfersQueryResult>
    {
        private readonly ITransactionStripeConnectTransfersRepository _transactionStripeConnectTransfersRepository;

        public TransactionStripeConnectTransfersQueryHandler(ITransactionStripeConnectTransfersRepository transactionStripeConnectTransfersRepository)
        {
            _transactionStripeConnectTransfersRepository = transactionStripeConnectTransfersRepository;
        }

        public TransactionStripeConnectTransfersQueryResult Handle(TransactionStripeConnectTransfersQuery query)
        {
            var transactionStripeConnectTransfersQueryResult = new TransactionStripeConnectTransfersQueryResult();
            transactionStripeConnectTransfersQueryResult.transactionStripeConnectTransfers = new List<TransactionStripeConnectTransfer>();
            foreach (var transactionStripeConnectTransfer in _transactionStripeConnectTransfersRepository.GetAllScheduledTransfers(query.TransferDate))
            {
                var dataRow = AutoMapper.Mapper.Map<TransactionStripeConnectTransfer>(transactionStripeConnectTransfer);
                transactionStripeConnectTransfersQueryResult.transactionStripeConnectTransfers.Add(dataRow);
            }
            return transactionStripeConnectTransfersQueryResult;
        }
    }
}