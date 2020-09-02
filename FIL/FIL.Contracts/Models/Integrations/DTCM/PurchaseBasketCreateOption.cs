using System.Collections.Generic;

namespace FIL.Contracts.Models.Integrations.DTCM.PurchaseBasketCreateOptions
{
    public class PurchaseBasketCreateOption
    {
        public string BasketID { get; set; }
        public string Seller { get; set; }
        public Customer customer { get; set; }
        public List<Payment> Payments { get; set; }
    }

    public class Customer
    {
        public int ID { get; set; }
        public int Account { get; set; }
        public string AFile { get; set; }
    }

    public class Payment
    {
        public int Amount { get; set; }
        public string MeansOfPayment { get; set; }
    }
}