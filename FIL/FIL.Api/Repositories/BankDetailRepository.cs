using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IBankDetailRepository : IOrmRepository<BankDetail, BankDetail>
    {
        BankDetail Get(long id);
    }

    public class BankDetailRepository : BaseLongOrmRepository<BankDetail>, IBankDetailRepository
    {
        public BankDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public BankDetail Get(long id)
        {
            return Get(new BankDetail { Id = id });
        }

        public IEnumerable<BankDetail> GetAll()
        {
            return GetAll(null);
        }
    }
}