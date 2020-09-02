using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IPaymentOptionDetailRepository : IOrmRepository<PaymentOptionDetail, PaymentOptionDetail>
    {
        PaymentOptionDetail Get(int id);
    }

    public class PaymentOptionDetailRepository : BaseOrmRepository<PaymentOptionDetail>, IPaymentOptionDetailRepository
    {
        public PaymentOptionDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public PaymentOptionDetail Get(int id)
        {
            return Get(new PaymentOptionDetail { Id = id });
        }

        public IEnumerable<PaymentOptionDetail> GetAll()
        {
            return GetAll(null);
        }

        public void DeletePaymentOptionDetail(PaymentOptionDetail paymentOptionDetail)
        {
            Delete(paymentOptionDetail);
        }

        public PaymentOptionDetail SavePaymentOptionDetail(PaymentOptionDetail paymentOptionDetail)
        {
            return Save(paymentOptionDetail);
        }
    }
}