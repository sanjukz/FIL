using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface ICorporateOrderInvoiceMappingRepository : IOrmRepository<CorporateOrderInvoiceMapping, CorporateOrderInvoiceMapping>
    {
        CorporateOrderInvoiceMapping Get(long id);
    }

    public class CorporateOrderInvoiceMappingRepository : BaseLongOrmRepository<CorporateOrderInvoiceMapping>, ICorporateOrderInvoiceMappingRepository
    {
        public CorporateOrderInvoiceMappingRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public CorporateOrderInvoiceMapping Get(long id)
        {
            return Get(new CorporateOrderInvoiceMapping { Id = id });
        }

        public IEnumerable<CorporateOrderInvoiceMapping> GetAll()
        {
            return GetAll(null);
        }
    }
}