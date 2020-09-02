using FIL.Contracts.Interfaces.Queries;

namespace FIL.Contracts.Queries.SeatLayout
{
    public class SeatLayoutQuery : IQuery<SeatLayoutQueryResult>
    {
        public long EventTicketAttributeId { get; set; }
    }
}