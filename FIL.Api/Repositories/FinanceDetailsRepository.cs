using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IFinanceDetailsRepository : IOrmRepository<FinanceDetail, FinanceDetail>
    {
        FinanceDetail Get(int id);
    }

    public class FinanceDetailsRepository : BaseOrmRepository<FinanceDetail>, IFinanceDetailsRepository
    {
        public FinanceDetailsRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public FinanceDetail Get(int id)
        {
            return Get(new FinanceDetail { Id = id });
        }

        public IEnumerable<FinanceDetail> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteFinanceDetail(FinanceDetail financeDetail)
        {
            Delete(financeDetail);
        }

        public FinanceDetail SaveFinanceDetaile(FinanceDetail financeDetail)
        {
            return Save(financeDetail);
        }
    }
}