using System;

namespace FIL.Contracts.QueryResults.Country
{
    public class CountrySearchInfoQueryResult
    {
        public int Id { get; set; }
        public Guid AltId { get; set; }
        public string Name { get; set; }
        public string IsoAlphaTwoCode { get; set; }
        public string IsoAlphaThreeCode { get; set; }
    }
}