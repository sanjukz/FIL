using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.CreateEventV1
{
    public class TicketQueryResult
    {
        public long EventId { get; set; }
        public long EventDetailId { get; set; }
        public bool Success { get; set; }
        public bool IsValidLink { get; set; }
        public bool IsDraft { get; set; }
        public List<FIL.Contracts.Models.CreateEventV1.TicketModel> Tickets { get; set; }
    }
}