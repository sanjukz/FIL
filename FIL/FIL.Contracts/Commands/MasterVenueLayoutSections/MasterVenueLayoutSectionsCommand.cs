namespace FIL.Contracts.Commands.MasterVenueLayoutSections
{
    public class MasterVenueLayoutSectionsCommand : BaseCommand
    {
        public string SectionName { get; set; }
        public int MasterVenueLayoutId { get; set; }
        public int MasterVenueLayoutSectionId { get; set; }
        public int Capacity { get; set; }
        public int EntryGateId { get; set; }
        public int VenueLayoutAreaId { get; set; }
        public bool IsEnabled { get; set; }
    }
}