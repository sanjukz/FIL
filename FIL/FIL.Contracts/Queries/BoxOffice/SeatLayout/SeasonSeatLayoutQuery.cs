using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.Boxoffice.SeatLayout;

namespace FIL.Contracts.Queries.BoxOffice.SeatLayout
{
    public class SeasonSeatLayoutQuery : IQuery<SeasonSeatLayoutQueryResult>
    {
        public long EventTicketAttributeId { get; set; }
    }
}