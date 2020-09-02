using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels.ASI;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IASIPaymentResponseDetailRepository : IOrmRepository<ASIPaymentResponseDetail, ASIPaymentResponseDetail>
    {
        ASIPaymentResponseDetail Get(long id);

        IEnumerable<ASIPaymentResponseDetail> GetByTransactionId(long transactionId);
    }

    public class ASIPaymentResponseDetailRepository : BaseLongOrmRepository<ASIPaymentResponseDetail>, IASIPaymentResponseDetailRepository
    {
        public ASIPaymentResponseDetailRepository(IDataSettings dataSettings)
            : base(dataSettings)
        {
        }

        public ASIPaymentResponseDetail Get(long id)
        {
            return Get(new ASIPaymentResponseDetail { Id = id });
        }

        public IEnumerable<ASIPaymentResponseDetail> GetByTransactionId(long transactionId)
        {
            return GetAll(s => s.Where($"{nameof(ASIPaymentResponseDetail.TransactionId):C} = @TransactionId")
               .WithParameters(new { TransactionId = transactionId })
           );
        }
    }
}