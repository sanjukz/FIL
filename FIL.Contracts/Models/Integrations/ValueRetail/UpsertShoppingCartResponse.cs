using System.Collections.Generic;

namespace FIL.Contracts.Models.Integrations.ValueRetail
{
    public class QuotedPrice
    {
        public int RateId { get; set; }
        public double TotalGrossPrice { get; set; }
        public double TotalNetPrice { get; set; }
        public double PromoCodeDiscount { get; set; }
        public double OriginalNetPrice { get; set; }
        public double OriginalGrossPrice { get; set; }
        public double NetDifference { get; set; }
        public double GrossDifference { get; set; }
        public bool AccountType { get; set; }
        public bool AmendRestriction { get; set; }
        public bool CancelRestriction { get; set; }
        public string CurrencyCode { get; set; }
        public bool IsDiscounted { get; set; }
        public IList<string> CreditCards { get; set; }
    }

    public class UpsertShoppingCartResponse
    {
        public Basket Basket { get; set; }
        public RequestStatus RequestStatus { get; set; }
    }
}