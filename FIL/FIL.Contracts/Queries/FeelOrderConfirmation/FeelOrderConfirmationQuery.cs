using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.FeelOrderConfirmation;
using System;

namespace FIL.Contracts.Queries.FeelOrderConfirmation
{
    public class FeelOrderConfirmationQuery : IQuery<FeelOrderConfirmationQueryResult>
    {
        public Guid TransactionAltId { get; set; }
        public FIL.Contracts.Enums.Channels Channel { get; set; }
        public bool confirmationFrmMyOrders { get; set; }
    }
}