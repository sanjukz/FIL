using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface ICorporateRepresentativeDetailRepository : IOrmRepository<CorporateRepresentativeDetail, CorporateRepresentativeDetail>
    {
        CorporateRepresentativeDetail Get(long id);
    }

    public class CorporateRepresentativeDetailRepository : BaseLongOrmRepository<CorporateRepresentativeDetail>, ICorporateRepresentativeDetailRepository
    {
        public CorporateRepresentativeDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public CorporateRepresentativeDetail Get(long id)
        {
            return Get(new CorporateRepresentativeDetail { Id = id });
        }

        public IEnumerable<CorporateRepresentativeDetail> GetAll()
        {
            return GetAll(null);
        }
    }
}