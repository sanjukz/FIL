using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IChequeDetailRepository : IOrmRepository<ChequeDetail, ChequeDetail>
    {
        ChequeDetail Get(int id);
    }

    public class ChequeDetailRepository : BaseOrmRepository<ChequeDetail>, IChequeDetailRepository
    {
        public ChequeDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public ChequeDetail Get(int id)
        {
            return Get(new ChequeDetail { Id = id });
        }

        public IEnumerable<ChequeDetail> GetAll()
        {
            return GetAll(null);
        }
    }
}