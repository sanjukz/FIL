using System.Collections.Generic;

namespace FIL.Contracts.Models.Integrations.DTCM.OrderCreateOption
{
    public class OrderCreateOption
    {
        public string OrderId { get; set; }
        public string Seller { get; set; }
        public List<Refund> Refunds { get; set; }
    }

    public class Refund
    {
        public int Amount { get; set; }
        public string MeansOfPayment { get; set; }
    }
}