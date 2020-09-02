using System;

namespace FIL.Contracts.Models
{
    public class State
    {
        public int Id { get; set; }
        public Guid AltId { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public int CountryId { get; set; }
    };
}