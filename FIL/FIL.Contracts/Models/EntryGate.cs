using System;

namespace FIL.Contracts.Models
{
    public class EntryGate
    {
        public int Id { get; set; }
        public Guid AltId { get; set; }
        public string Name { get; set; }
        public string StreetInformation { get; set; }
    }
}