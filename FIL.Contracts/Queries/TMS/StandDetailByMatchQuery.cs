using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResult.TMS;
using System;

namespace FIL.Contracts.Queries.TMS
{
    public class StandDetailByMatchQuery : IQuery<StandDetailByMatchQueryResult>
    {
        public Guid? EventDetailAltId { get; set; }
        public Guid? EventAltId { get; set; }
        public Guid? VenueAltId { get; set; }
        public AllocationType AllocationType { get; set; }
        public long TicketCategoryId { get; set; }
    }
}