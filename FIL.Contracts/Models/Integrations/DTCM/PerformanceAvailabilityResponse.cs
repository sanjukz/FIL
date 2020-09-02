using System.Collections.Generic;

namespace FIL.Contracts.Models.Integrations.DTCM.PerformanceAvailabilities
{
    public class PerformanceAvailabilityResponse
    {
        public List<PriceCategory> PriceCategories { get; set; }
    }

    public class PriceCategory
    {
        public Availability Availability { get; set; }
        public int PriceCategoryId { get; set; }
        public string PriceCategoryCode { get; set; }
        public string PriceCategoryName { get; set; }
    }

    public class Availability
    {
        public bool SoldOut { get; set; }
        public string StatusCode { get; set; }
    }
}