using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.MasterVenueLayoutSections;

namespace FIL.Contracts.Queries.MasterVenueLayoutSections
{
    public class MasterVenueLayoutSectionsGetUpdateDataQuery : IQuery<MasterVenueLayoutSectionsGetUpdateDataQueryResult>
    {
        public string AltId { get; set; }
        public string MasterVenueLayoutAltId { get; set; }
        public string EntryGateAltId { get; set; }
        public bool IsMasterLayoutSectionIdUpdate { get; set; }
        public string Section { get; set; }
        public string Block { get; set; }
        public string Level { get; set; }
        public string Stand { get; set; }
    }
}