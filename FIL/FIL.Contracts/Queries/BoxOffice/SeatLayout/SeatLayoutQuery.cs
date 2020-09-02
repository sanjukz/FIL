using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.Boxoffice.SeatLayout;

namespace FIL.Contracts.Queries.BoxOffice.SeatLayout
{
    public class SeatLayoutQuery : IQuery<SeatLayoutQueryResult>
    {
        public long? EventDetailId { get; set; }
        public long? TicketCategoryId { get; set; }
        public long? EventTicketAttributeId { get; set; }
        public AllocationType AllocationType { get; set; }
        public Channels Channels { get; set; }
    }
}