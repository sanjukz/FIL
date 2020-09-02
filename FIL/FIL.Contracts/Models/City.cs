using System;

namespace FIL.Contracts.Models
{
    public class City
    {
        public int Id { get; set; }
        public Guid AltId { get; set; }
        public string Name { get; set; }
        public int StateId { get; set; }
    }
}