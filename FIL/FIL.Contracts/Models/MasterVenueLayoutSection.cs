using System;

namespace FIL.Contracts.Models
{
    public class MasterVenueLayoutSection
    {
        public int Id { get; set; }
        public Guid AltId { get; set; }
        public string SectionName { get; set; }
        public int MasterVenueLayoutId { get; set; }
        public int MasterVenueLayoutSectionId { get; set; }
        public int Capacity { get; set; }
        public int EntryGateId { get; set; }
        public short? VenueLayoutAreaId { get; set; }
    }
}