using System;

namespace FIL.Contracts.Models.Integrations.POne
{
    public class POneApiRequestModel
    {
        public string token { get; set; }
        public string language { get; set; }
        public string @event { get; set; }
        public int? eventId { get; set; }
        public int? matchId { get; set; }
        public string sku { get; set; }
        public DateTime? dateFrom { get; set; }
        public DateTime? dateUntil { get; set; }
        public string category { get; set; }
        public string sub_category { get; set; }
    }
}