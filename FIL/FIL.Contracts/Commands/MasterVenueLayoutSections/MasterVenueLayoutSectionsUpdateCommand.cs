namespace FIL.Contracts.Commands.MasterVenueLayoutSections
{
    public class MasterVenueLayoutSectionsUpdateCommand : BaseCommand
    {
        public string SectionName { get; set; }
        public string AltId { get; set; }
        public string MasterVenueLayoutId { get; set; }
        public int MasterVenueLayoutSectionId { get; set; }
        public int Capacity { get; set; }
        public int EntryGateId { get; set; }
        public short? VenueLayoutAreaId { get; set; }
        public bool IsMasterVenueLayoutSectionIdUpdate { get; set; }
    }
}