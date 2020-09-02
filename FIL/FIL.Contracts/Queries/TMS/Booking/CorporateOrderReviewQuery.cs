using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.TMS.Booking;

namespace FIL.Contracts.Queries.TMS.Booking
{
    public class CorporateOrderReviewQuery : IQuery<CorporateOrderReviewQueryResult>
    {
        public long TransactionId { get; set; }
    }
}