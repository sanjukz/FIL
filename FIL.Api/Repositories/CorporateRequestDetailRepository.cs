using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface ICorporateRequestDetailRepository : IOrmRepository<CorporateRequestDetail, CorporateRequestDetail>
    {
        CorporateRequestDetail Get(long id);
    }

    public class CorporateRequestDetailRepository : BaseLongOrmRepository<CorporateRequestDetail>, ICorporateRequestDetailRepository
    {
        public CorporateRequestDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public CorporateRequestDetail Get(long id)
        {
            return Get(new CorporateRequestDetail { Id = id });
        }

        public IEnumerable<CorporateRequestDetail> GetAll()
        {
            return GetAll(null);
        }
    }
}