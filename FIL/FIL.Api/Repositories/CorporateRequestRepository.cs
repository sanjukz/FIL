using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface ICorporateRequestRepository : IOrmRepository<CorporateRequest, CorporateRequest>
    {
        CorporateRequest Get(long id);
    }

    public class CorporateRequestRepository : BaseLongOrmRepository<CorporateRequest>, ICorporateRequestRepository
    {
        public CorporateRequestRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public CorporateRequest Get(long id)
        {
            return Get(new CorporateRequest { Id = id });
        }

        public IEnumerable<CorporateRequest> GetAll()
        {
            return GetAll(null);
        }
    }
}