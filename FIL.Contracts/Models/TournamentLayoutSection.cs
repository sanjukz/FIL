using FIL.Contracts.Enums;

namespace FIL.Contracts.Models
{
    public class TournamentLayoutSection
    {
        public int Id { get; set; }
        public string SectionName { get; set; }
        public int MasterVenueLayoutSectionId { get; set; }
        public int TournamentLayoutId { get; set; }
        public int TournamentLayoutSectionId { get; set; }
        public int Capacity { get; set; }
        public int EntryGateId { get; set; }
        public VenueLayoutArea VenueLayoutAreaId { get; set; }
    }
}