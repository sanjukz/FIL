using System;

namespace FIL.Contracts.DataModels
{
    public class CountryPlace
    {
        public string Name { get; set; }
        public Guid AltId { get; set; }
        public int Id { get; set; }
        public int Count { get; set; }
    }
}