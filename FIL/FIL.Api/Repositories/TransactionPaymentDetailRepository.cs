using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface ITransactionPaymentDetailRepository : IOrmRepository<TransactionPaymentDetail, TransactionPaymentDetail>
    {
        TransactionPaymentDetail Get(int id);

        TransactionPaymentDetail GetByTransactionId(long transactionId);

        IEnumerable<TransactionPaymentDetail> GetByUserCardDetailIds(IEnumerable<long> userCardDetailIds);

        IEnumerable<TransactionPaymentDetail> GetByUserCardDetailId(long userCardDetailId);

        IEnumerable<TransactionPaymentDetail> GetByTransactionIds(IEnumerable<long> transactionIds);

        IEnumerable<TransactionPaymentDetail> GetFailedTransactionsByIds(IEnumerable<long> transactionIds, PaymentGateway paymentGateway);

        IEnumerable<TransactionPaymentDetail> GetPaymentDetailByTransactionId(long transactionId);

        IEnumerable<TransactionPaymentDetail> GetAllByTransactionId(long transactionId);
    }

    public class TransactionPaymentDetailRepository : BaseLongOrmRepository<TransactionPaymentDetail>, ITransactionPaymentDetailRepository
    {
        public TransactionPaymentDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public TransactionPaymentDetail Get(int id)
        {
            return Get(new TransactionPaymentDetail { Id = id });
        }

        public IEnumerable<TransactionPaymentDetail> GetAll()
        {
            return GetAll(null);
        }

        public TransactionPaymentDetail GetByTransactionId(long transactionId)
        {
            return GetAll(s => s.Where($"{nameof(TransactionPaymentDetail.TransactionId):C} = @TransactionId")
                .WithParameters(new { TransactionId = transactionId })
            ).FirstOrDefault();
        }

        public IEnumerable<TransactionPaymentDetail> GetAllByTransactionId(long transactionId)
        {
            return GetAll(s => s.Where($"{nameof(TransactionPaymentDetail.TransactionId):C} = @TransactionId")
                .WithParameters(new { TransactionId = transactionId })
            );
        }

        public IEnumerable<TransactionPaymentDetail> GetPaymentDetailByTransactionId(long transactionId)
        {
            return GetAll(s => s.Where($"{nameof(TransactionPaymentDetail.TransactionId):C} = @TransactionId")
                .WithParameters(new { TransactionId = transactionId })
            );
        }

        public void DeleteTransactionPaymentDetail(TransactionPaymentDetail transactionPaymentDetail)
        {
            Delete(transactionPaymentDetail);
        }

        public TransactionPaymentDetail SaveTransactionPaymentDetail(TransactionPaymentDetail transactionPaymentDetail)
        {
            return Save(transactionPaymentDetail);
        }

        public IEnumerable<TransactionPaymentDetail> GetByUserCardDetailIds(IEnumerable<long> userCardDetailIds)
        {
            return GetAll(s => s.Where($"{nameof(TransactionPaymentDetail.UserCardDetailId):C}In @UserCardDetailIds")
              .WithParameters(new { UserCardDetailIds = userCardDetailIds })).Distinct();
        }

        public IEnumerable<TransactionPaymentDetail> GetByUserCardDetailId(long userCardDetailIds)
        {
            return GetAll(s => s.Where($"{nameof(TransactionPaymentDetail.UserCardDetailId):C} = @UserCardDetailIds")
              .WithParameters(new { UserCardDetailIds = userCardDetailIds })).Distinct();
        }

        public IEnumerable<TransactionPaymentDetail> GetByTransactionIds(IEnumerable<long> transactionIds)
        {
            var EventTrasactionmode = GetAll(statement => statement
                                 .Where($"{nameof(TransactionPaymentDetail.TransactionId):C} IN @TransactionIds")
                                 .WithParameters(new { TransactionIds = transactionIds }));
            return EventTrasactionmode;
        }

        public IEnumerable<TransactionPaymentDetail> GetFailedTransactionsByIds(IEnumerable<long> transactionIds, PaymentGateway paymentGateway)
        {
            var EventTrasactionmode = GetAll(statement => statement
                                 .Where($"{nameof(TransactionPaymentDetail.TransactionId):C} IN @TransactionIds AND                     {nameof(TransactionPaymentDetail.PaymentGatewayId):C} = @paymentGatewayId")
                                 .WithParameters(new { TransactionIds = transactionIds, paymentGatewayId = (PaymentGateway)paymentGateway }));
            return EventTrasactionmode;
        }
    }
}