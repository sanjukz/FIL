using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.FulFilment;

namespace FIL.Contracts.Queries.FulFilment
{
    public class ValidotpQuery : IQuery<ValidOtpQueryResult>
    {
        public long TransactionDetailId { get; set; }
        public long PickupOTP { get; set; }
    }
}