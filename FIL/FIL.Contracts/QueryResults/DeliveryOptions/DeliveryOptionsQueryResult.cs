using FIL.Contracts.Models;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.DeliveryOptions
{
    public class DeliveryOptionsQueryResult
    {
        public List<EventDeliveryTypeDetail> EventDeliveryTypeDetails { get; set; }
        public FIL.Contracts.Models.User UserDetails { get; set; }
    }
}