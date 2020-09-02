using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.PublishEvent
{
    public class PublishSubEventQueryResult
    {
        public List<FIL.Contracts.Models.EventDetail> SubEvents { get; set; }
        public List<string> Sites { get; set; }
        public Contracts.Models.FeaturedEvent featuredEvents { get; set; }
    }
}