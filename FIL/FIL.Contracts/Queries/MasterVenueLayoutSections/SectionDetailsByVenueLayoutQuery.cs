using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.MasterVenueLayoutSections;
using System;

namespace FIL.Contracts.Queries.MasterVenueLayoutSections
{
    public class SectionDetailsByVenueLayoutQuery : IQuery<SectionDetailsByVenueLayoutQueryResult>
    {
        public Guid AltId { get; set; }
        public int VenueId { get; set; }
    }
}