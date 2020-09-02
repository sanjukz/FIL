using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.BoxOffice.SeatLayout;

namespace FIL.Contracts.Queries.BoxOffice.SeatLayout
{
    public class SeasonSeatQuery : IQuery<SeasonSeatQueryResult>
    {
        public long EventTicketAttributeId { get; set; }
        public string SeatTag { get; set; }
    }
}