using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.Payment;
using System;

namespace FIL.Contracts.Queries.Payment
{
    public class PaymentOptionsQuery : IQuery<PaymentOptionsQueryResult>
    {
        public Guid AltId { get; set; }
    }
}