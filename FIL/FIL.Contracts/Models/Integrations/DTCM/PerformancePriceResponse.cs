using System.Collections.Generic;

namespace FIL.Contracts.Models.Integrations.DTCM.PerformancePrices
{
    public class PerformancePriceResponse
    {
        public string ErrorMessage { get; set; }
        public List<PriceCategory> PriceCategories { get; set; }
        public List<PriceType> PriceTypes { get; set; }
        public List<Price> BasketPrices { get; set; }
        public TicketPrice TicketPrices { get; set; }
        public OfferPrice OfferPrices { get; set; }
    }

    public class PriceCategory
    {
        public int PriceCategoryId { get; set; }
        public string PriceCategoryCode { get; set; }
        public string PriceCategoryName { get; set; }
    }

    public class PriceType
    {
        public int PriceTypeId { get; set; }
        public string PriceTypeCode { get; set; }
        public string PriceTypeName { get; set; }
        public string PriceTypeDescription { get; set; }
        public int PriceSheetId { get; set; }
        public int AdmitCount { get; set; }
        public int ConcessionCount { get; set; }
        public string OfferCode { get; set; }
        public string QualifierCode { get; set; }
        public string Entitlement { get; set; }
    }

    public class Price
    {
        public int PriceId { get; set; }
        public int PriceCategoryId { get; set; }
        public string PriceCategoryCode { get; set; }
        public int PriceTypeId { get; set; }
        public string PriceTypeCode { get; set; }
        public int PriceSheetId { get; set; }
        public int PriceNet { get; set; }
        public List<FeeTypes> FeeTypes { get; set; }
    }

    public class TicketPrice
    {
        public List<Price> Prices { get; set; }
        public List<FeeTypes> FeeTypes { get; set; }
    }

    public class OfferPrice
    {
        public List<Price> Prices { get; set; }
        public List<FeeTypes> FeeTypes { get; set; }
    }

    public class FeeTypes
    {
        public string FeeType { get; set; }
        public string FeeTypeName { get; set; }
        public bool Inside { get; set; }
        public string FeeBucket { get; set; }
        public List<FeeDetail> FeesDetailed { get; set; }
    }

    public class FeeDetail
    {
        public string FeeId { get; set; }
        public int FeeSheetId { get; set; }
        public string FeeCode { get; set; }
        public string FeeName { get; set; }
        public string FeeDescription { get; set; }
        public double FeeTotal { get; set; }
        public string FinanceCode { get; set; }
    }
}