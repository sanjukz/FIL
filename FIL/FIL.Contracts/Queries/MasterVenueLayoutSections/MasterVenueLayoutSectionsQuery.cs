using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.MasterVenueLayoutSections;

namespace FIL.Contracts.Queries.MasterVenueLayoutSections
{
    public class MasterVenueLayoutSectionsQuery : IQuery<MasterVenueLayoutSectionsQueryResult>
    {
        public string SectionName { get; set; }
        public int MasterVenueLayoutId { get; set; }
        public int MasterVenueLayoutSectionId { get; set; }
        public int VenueLayoutAreaId { get; set; }
        public bool IsEnabled { get; set; }
        public int Capacity { get; set; }
        public bool IsUpdate { get; set; }
        public int SelectedSectionId { get; set; }
    }
}