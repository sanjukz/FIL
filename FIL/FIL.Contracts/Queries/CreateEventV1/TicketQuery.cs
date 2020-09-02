using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.CreateEventV1;

namespace FIL.Contracts.Queries.CreateEventV1
{
    public class TicketQuery : IQuery<TicketQueryResult>
    {
        public long EventId { get; set; }
        public long TicketCategoryTypeId { get; set; }
    }
}