using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.TMS;
using System;

namespace FIL.Contracts.Queries.TMS
{
    public class SponsorOrderDetailQuery : IQuery<SponsorOrderDetailQueryResult>
    {
        public Guid EventAltId { get; set; }
        public Guid VenueAltId { get; set; }
        public Guid EventDetailAltId { get; set; }
    }
}