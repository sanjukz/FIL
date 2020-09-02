using System.Collections.Generic;

namespace FIL.Contracts.Models.ValueRetail
{
    public class Village
    {
        public string VillageCode { get; set; }
        public string CultureCode { get; set; }
        public string CurrencyCode { get; set; }
        public string VillageName { get; set; }
    }

    public class VillageResponse
    {
        public List<Village> Villages { get; set; }
    }
}