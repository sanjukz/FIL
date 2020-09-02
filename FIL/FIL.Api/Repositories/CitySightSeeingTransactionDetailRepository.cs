using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface ICitySightSeeingTransactionDetailRepository : IOrmRepository<CitySightSeeingTransactionDetail, CitySightSeeingTransactionDetail>
    {
        CitySightSeeingTransactionDetail Get(int id);

        CitySightSeeingTransactionDetail GetByTransactionId(long transactionId);
    }

    public class CitySightSeeingTransactionDetailRepository : BaseLongOrmRepository<CitySightSeeingTransactionDetail>, ICitySightSeeingTransactionDetailRepository
    {
        public CitySightSeeingTransactionDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public CitySightSeeingTransactionDetail Get(int id)
        {
            return Get(new CitySightSeeingTransactionDetail { Id = id });
        }

        public CitySightSeeingTransactionDetail GetByTransactionId(long transactionId)
        {
            return GetAll(s => s.Where($"{nameof(CitySightSeeingTransactionDetail.TransactionId):C} = @TransactionId")
                .WithParameters(new { TransactionId = transactionId })
            ).FirstOrDefault();
        }

        public IEnumerable<CitySightSeeingTransactionDetail> GetAll()
        {
            return GetAll(null);
        }
    }
}