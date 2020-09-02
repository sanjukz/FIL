using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.OrderConfirmation;

namespace FIL.Contracts.Queries.OrderConfirmation
{
    public class OrderConfirmationQuery : IQuery<OrderConfirmationQueryResult>
    {
        public long TransactionId { get; set; }
        public TransactionStatus TransactionStatus { get; set; } = TransactionStatus.Success;
    }
}