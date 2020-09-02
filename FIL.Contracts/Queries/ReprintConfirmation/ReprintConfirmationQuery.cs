using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.ReprintConfirmation;

namespace FIL.Contracts.Queries.ReprintConfirmation
{
    public class ReprintConfirmationQuery : IQuery<ReprintConfirmationQueryResult>
    {
        public long? TransactionId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}