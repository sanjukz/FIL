using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IGuestDetailRepository : IOrmRepository<GuestDetail, GuestDetail>
    {
        GuestDetail Get(int id);

        IEnumerable<GuestDetail> GetByTransactionDetailId(long transactionDetailId);

        IEnumerable<GuestDetail> GetByTransactionDetailIds(IEnumerable<long> transactionDetailId);
    }

    public class GuestDetailRepository : BaseOrmRepository<GuestDetail>, IGuestDetailRepository
    {
        public GuestDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public GuestDetail Get(int id)
        {
            return Get(new GuestDetail { Id = id });
        }

        public IEnumerable<GuestDetail> GetByTransactionDetailId(long transactionDetailId)
        {
            return GetAll(statement => statement
                                .Where($"{nameof(GuestDetail.TransactionDetailId):C} = @Id")
                                .WithParameters(new { Id = transactionDetailId }));
        }

        public IEnumerable<GuestDetail> GetByTransactionDetailIds(IEnumerable<long> transactionDetailId)
        {
            return GetAll(statement => statement
                                .Where($"{nameof(GuestDetail.TransactionDetailId):C} IN @Ids")
                                .WithParameters(new { Ids = transactionDetailId }));
        }
    }
}