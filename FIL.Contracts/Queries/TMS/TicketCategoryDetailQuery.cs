using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.TMS;
using System;

namespace FIL.Contracts.Queries.TMS
{
    public class TicketCategoryDetailQuery : IQuery<TicketCategoryDetailQueryResult>
    {
        public Guid EventDetailAltId { get; set; }
        public long? TicketCategoryId { get; set; }
    }
}