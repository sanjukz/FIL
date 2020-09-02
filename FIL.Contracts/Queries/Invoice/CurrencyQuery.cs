using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.Invoice;
using System;

namespace FIL.Contracts.Queries.Invoice
{
    public class CurrencyQuery : IQuery<CurrencyQueryResult>
    {
        public Guid eventAltId { get; set; }
    }
}