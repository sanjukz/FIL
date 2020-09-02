using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.Invoice;

namespace FIL.Contracts.Queries.Invoice
{
    public class BankDetailsQuery : IQuery<BankDetailsQueryResult>
    {
        public long CompanyDetailId { get; set; }
    }
}