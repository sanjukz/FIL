using System;

namespace FIL.Contracts.Models.Integrations.ValueRetail
{
    public class ValueRetailCommanRequestModel
    {
        public DateTime? from { get; set; }
        public string cultureCode { get; set; }
        public string villageCode { get; set; }
        public int? packageId { get; set; }
        public DateTime? to { get; set; }
        public int? journeyType { get; set; }
        public int? routeId { get; set; }
    }
}