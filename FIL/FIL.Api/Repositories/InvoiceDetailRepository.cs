using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IInvoiceDetailRepository : IOrmRepository<InvoiceDetail, InvoiceDetail>
    {
        InvoiceDetail Get(long id);
    }

    public class InvoiceDetailRepository : BaseLongOrmRepository<InvoiceDetail>, IInvoiceDetailRepository
    {
        public InvoiceDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public InvoiceDetail Get(long id)
        {
            return Get(new InvoiceDetail { Id = id });
        }

        public IEnumerable<InvoiceDetail> GetAll()
        {
            return GetAll(null);
        }
    }
}