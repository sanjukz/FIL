using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.MasterVenueLayoutSections;
using System;

namespace FIL.Contracts.Queries.MasterVenueLayoutSections
{
    public class MasterVenueLayoutSectionsUpdateQuery : IQuery<MasterVenueLayoutSectionsUpdateQueryResult>
    {
        public Guid AltId { get; set; }
    }
}