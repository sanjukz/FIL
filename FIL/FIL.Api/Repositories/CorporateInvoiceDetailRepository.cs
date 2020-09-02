using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface ICorporateInvoiceDetailRepository : IOrmRepository<CorporateInvoiceDetail, CorporateInvoiceDetail>
    {
        CorporateInvoiceDetail Get(long id);
    }

    public class CorporateInvoiceDetailRepository : BaseLongOrmRepository<CorporateInvoiceDetail>, ICorporateInvoiceDetailRepository
    {
        public CorporateInvoiceDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public CorporateInvoiceDetail Get(long id)
        {
            return Get(new CorporateInvoiceDetail { Id = id });
        }

        public IEnumerable<CorporateInvoiceDetail> GetAll()
        {
            return GetAll(null);
        }
    }
}