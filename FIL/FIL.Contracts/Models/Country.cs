using FIL.Caching.Contracts.Interfaces;
using System;

namespace FIL.Contracts.Models
{
    public class Country : ICacheable
    {
        public int Id { get; set; }
        public Guid AltId { get; set; }
        public string Name { get; set; }
        public string IsoAlphaTwoCode { get; set; }
        public string IsoAlphaThreeCode { get; set; }
        public int? Numcode { get; set; }
        public int? Phonecode { get; set; }

        string ICacheable.CacheKey => AltId.ToString();
    }
}