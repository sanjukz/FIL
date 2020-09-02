using System;
using System.Collections.Generic;

namespace FIL.Contracts.Models.ValueRetail.ShoppingPackage
{
    public class PricingResponse
    {
        public int RateId { get; set; }
        public decimal TotalGrossPrice { get; set; }
        public decimal TotalNetPrice { get; set; }
        public decimal PromoCodeDiscount { get; set; }
        public decimal OriginalNetPrice { get; set; }
        public decimal OriginalGrossPrice { get; set; }
        public decimal NetDifference { get; set; }
        public decimal GrossDifference { get; set; }
        public bool AccountType { get; set; }
        public bool AmendRestriction { get; set; }
        public bool CancelRestriction { get; set; }
        public object CurrencyCode { get; set; }
        public bool IsDiscounted { get; set; }
        public IList<object> CreditCards { get; set; }
    }

    public class ServiceImage
    {
        public int ServiceID { get; set; }
        public string ImageName { get; set; }
        public string URL { get; set; }
    }

    public class ChauffeurRoute
    {
        public int RouteID { get; set; }
        public string LocationHeader { get; set; }
        public int LocationID { get; set; }
        public object ExternalMessage { get; set; }
        public object TripAdvisorURL { get; set; }
        public IList<DateTime> BlockedDate { get; set; }
        public decimal SinglePrice { get; set; }
        public decimal SingleSurhcarge { get; set; }
        public string ReturnPrice { get; set; }
        public string HourlyWaiting { get; set; }
        public dynamic EstimatedJourneyTime { get; set; }
        public int WaitingTime { get; set; }
        public int BookingLeadTime { get; set; }
        public int AmendLeadTime { get; set; }
        public int CancelLeadTime { get; set; }
        public int ServiceID { get; set; }
        public string ServiceType { get; set; }
        public int NoOfpassengers { get; set; }
        public int NoOfBags { get; set; }
        public string PickupLocation { get; set; }
        public IList<object> DropOffLocations { get; set; }
        public IList<object> Images { get; set; }
        public PricingResponse PricingResponse { get; set; }
        public string LanguageCode { get; set; }
        public IList<object> CardNames { get; set; }
        public string ServiceDescription { get; set; }
        public IList<ServiceImage> ServiceImages { get; set; }
        public int IsPrivate { get; set; }
    }

    public class ChauffeurDrivenRouteResponse
    {
        public IList<ChauffeurRoute> Routes { get; set; }
        public RequestStatus RequestStatus { get; set; }
    }
}