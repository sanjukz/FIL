using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.MasterVenueLayoutSections;

namespace FIL.Contracts.Queries.MasterVenueLayoutSections
{
    public class MasterVenueLayoutSectionsGetIdsQuery : IQuery<MasterVenueLayoutSectionsGetIdsQueryResult>
    {
        public string AltId { get; set; }
        public string MasterVenueLayoutAltId { get; set; }
        public string EntryGateAltId { get; set; }
        public string NewEntryGateName { get; set; }
    }
}