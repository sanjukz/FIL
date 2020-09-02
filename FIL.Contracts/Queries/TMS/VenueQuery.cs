using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResult.TMS;
using System;

namespace FIL.Contracts.Queries.TMS
{
    public class VenueQuery : IQuery<VenueQueryResult>
    {
        public Guid UserAltId { get; set; }
        public AllocationType AllocationType { get; set; }
    }
}