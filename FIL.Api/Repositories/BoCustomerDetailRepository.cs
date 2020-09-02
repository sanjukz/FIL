using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IBoCustomerDetailRepository : IOrmRepository<BoCustomerDetail, BoCustomerDetail>
    {
        BoCustomerDetail Get(long id);

        BoCustomerDetail GetByTransactionId(long transactionId);

        IEnumerable<BoCustomerDetail> GetAllByTransactionId(long transactionIds);
    }

    public class BoCustomerDetailRepository : BaseLongOrmRepository<BoCustomerDetail>, IBoCustomerDetailRepository
    {
        public BoCustomerDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public BoCustomerDetail Get(long id)
        {
            return Get(new BoCustomerDetail { Id = id });
        }

        public IEnumerable<BoCustomerDetail> GetAll()
        {
            return GetAll(null);
        }

        public BoCustomerDetail GetByTransactionId(long transactionId)
        {
            return GetAll(s => s.Where($"{nameof(BoCustomerDetail.TransactionId):C}=@TransactionId")
                 .WithParameters(new { TransactionId = transactionId })
             ).FirstOrDefault();
        }

        public IEnumerable<BoCustomerDetail> GetAllByTransactionId(long transactionIds)
        {
            return GetAll(statement => statement
                .Where($"{nameof(BoCustomerDetail.TransactionId):C}=@Ids")
                .WithParameters(new { Ids = transactionIds }));
        }
    }
}