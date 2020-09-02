using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.Invoice;

namespace FIL.Contracts.Queries.Invoice
{
    public class InvoiceDataQuery : IQuery<InvoiceDataQueryResult>
    {
        public long InvoiceId { get; set; }
    }
}