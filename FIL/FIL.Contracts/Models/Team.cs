using System;

namespace FIL.Contracts.Models
{
    public class Team
    {
        public long Id { get; set; }
        public Guid AltId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}