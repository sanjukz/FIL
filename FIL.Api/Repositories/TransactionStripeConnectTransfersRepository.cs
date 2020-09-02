using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface ITransactionStripeConnectTransfersRepository : IOrmRepository<TransactionStripeConnectTransfers, TransactionStripeConnectTransfers>
    {
        TransactionStripeConnectTransfers Get(long id);

        IEnumerable<TransactionStripeConnectTransfers> GetAllScheduledTransfers(System.DateTime TransferDate);
    }

    public class TransactionStripeConnectTransfersRepository : BaseLongOrmRepository<TransactionStripeConnectTransfers>, ITransactionStripeConnectTransfersRepository
    {
        public TransactionStripeConnectTransfersRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public TransactionStripeConnectTransfers Get(long id)
        {
            return Get(new TransactionStripeConnectTransfers { Id = id });
        }

        public IEnumerable<TransactionStripeConnectTransfers> GetAll()
        {
            return GetAll(null);
        }

        public IEnumerable<TransactionStripeConnectTransfers> GetAllScheduledTransfers(System.DateTime TransferDate)
        {
            return GetAll(s => s.Where($"{nameof(TransactionStripeConnectTransfers.TransferDateProposed):C} >= @TransferDateProposedStart AND {nameof(TransactionStripeConnectTransfers.TransferDateProposed):C} < @TransferDateProposedEnd AND {nameof(TransactionStripeConnectTransfers.TransferApiResponse):C} IS NULL").OrderBy($"{nameof(TransactionStripeConnectTransfers.Id):C} ASC").Top(100)
            .WithParameters(new { TransferDateProposedStart = TransferDate, TransferDateProposedEnd = TransferDate.AddDays(1) }));
        }

        public void DeleteActivity(TransactionStripeConnectTransfers transactionStripeConnectTransfers)
        {
            Delete(transactionStripeConnectTransfers);
        }

        public TransactionStripeConnectTransfers SaveActivity(TransactionStripeConnectTransfers transactionStripeConnectTransfers)
        {
            return Save(transactionStripeConnectTransfers);
        }
    }
}