using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.TMS;
using System;

namespace FIL.Contracts.Queries.TMS
{
    public class CategorySponsorQuery : IQuery<CategorySponsorQueryResult>
    {
        public long? EventTicketAttributeId { get; set; }
        public Guid? EventAltId { get; set; }
        public Guid? VenueAltId { get; set; }
        public long? TicketCategoryId { get; set; }
        public AllocationType AllocationType { get; set; }
    }
}