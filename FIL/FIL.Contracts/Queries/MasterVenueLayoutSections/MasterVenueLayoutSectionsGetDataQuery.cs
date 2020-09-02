using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.MasterVenueLayoutSections;
using System;

namespace FIL.Contracts.Queries.MasterVenueLayoutSections
{
    public class MasterVenueLayoutSectionsGetDataQuery : IQuery<MasterVenueLayoutSectionsGetDataQueryResult>
    {
        public Guid AltId { get; set; }
        public bool isGetByMasterVenueLayoutSectionId { get; set; }
        public int VenueLayoutAreaId { get; set; }
    }
}