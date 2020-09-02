using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.DynamicLayout;
using System;

namespace FIL.Contracts.Queries.DynamicLayout
{
    public class DynamicLayoutQuery : IQuery<DynamicLayoutQueryResult>
    {
        public int VenueId { get; set; }
        public Guid VenueAltId { get; set; }
    }
}