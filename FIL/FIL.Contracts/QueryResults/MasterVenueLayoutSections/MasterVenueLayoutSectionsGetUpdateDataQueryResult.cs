namespace FIL.Contracts.QueryResults.MasterVenueLayoutSections
{
    public class MasterVenueLayoutSectionsGetUpdateDataQueryResult
    {
        public int Id { get; set; }
        public int MasterVenueLayoutId { get; set; }
        public int MasterVenueLayoutSectionId { get; set; }
        public int EntryGateId { get; set; }
        public int VenueLayoutAreaId { get; set; }
        public int ParentId { get; set; }
    }
}