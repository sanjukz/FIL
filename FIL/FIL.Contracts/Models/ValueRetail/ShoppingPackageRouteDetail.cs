using System;
using System.Collections.Generic;

namespace FIL.Contracts.Models.ValueRetail.ShoppingPackage
{
    public class PricePerPerson
    {
        public int PackagePriceType { get; set; }
        public decimal Price { get; set; }
        public decimal MaxPrice { get; set; }
    }

    public class StopLocation
    {
        public int StopId { get; set; }
        public object StopTitle { get; set; }
        public int StopOrder { get; set; }
        public string LocationName { get; set; }
        public string LocationAddress { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string ArrivalTime { get; set; }
        public object EstimatedTime { get; set; }
        public bool OtaDefaultStop { get; set; }
    }

    public class OutboundJourney
    {
        public int JourneyType { get; set; }
        public object JourneyTitle { get; set; }
        public int RouteId { get; set; }
        public IList<int> LinkedRouteId { get; set; }
        public bool IsOneWayEnabled { get; set; }
        public IList<StopLocation> StopLocations { get; set; }
    }

    public class PriceOption
    {
        public int PriceGroupType { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
    }

    public class Availability
    {
        public IList<object> AllowedDate { get; set; }
        public dynamic AmendLeadTime { get; set; }
        public IList<DateTime> BlockedDate { get; set; }
        public DateTime BookFromDate { get; set; }
        public DateTime BookToDate { get; set; }
        public decimal BookingLeadTime { get; set; }
        public decimal CancelLeadTime { get; set; }
        public string DaysOfWeek { get; set; }
        public string ExternalMessage { get; set; }
        public IList<object> Images { get; set; }
        public string LocationHeader { get; set; }
        public int LocationID { get; set; }
        public bool OneWayEnabled { get; set; }
        public int RouteID { get; set; }
        public string RouteType { get; set; }
        public object RouteTilte { get; set; }
        public int Seats { get; set; }
        public int SeatsAvailable { get; set; }
        public int SeatsBooked { get; set; }
        public DateTime TravelFromDate { get; set; }
        public DateTime TravelToDate { get; set; }
    }

    public class ShoppingExpressItem
    {
        public OutboundJourney OutboundJourney { get; set; }
        public IList<PriceOption> PriceOptions { get; set; }
        public Availability Availability { get; set; }
    }

    public class Package
    {
        public int PackageID { get; set; }
        public int PackageOrder { get; set; }
        public string PackageName { get; set; }
        public string Description { get; set; }
        public string LongDescription { get; set; }
        public string AlternateName { get; set; }
        public string Redemption { get; set; }
        public string Restriction { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public IList<object> BlockedDate { get; set; }
        public IList<object> AllowedDate { get; set; }
        public string Category { get; set; }
        public decimal BookingLeadTime { get; set; }
        public decimal AmendLeadTime { get; set; }
        public decimal CancelLeadTime { get; set; }
        public int PriceOptionType { get; set; }
        public bool RedemptionDateNotRequired { get; set; }
        public IList<PricePerPerson> PricePerPersons { get; set; }
        public object PricePerUnitOption { get; set; }
        public string DaysofWeek { get; set; }
        public decimal TotalGrossPrice { get; set; }
        public decimal TotalNetPrice { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsMice { get; set; }
        public IList<ShoppingExpressItem> ShoppingExpressItems { get; set; }
    }

    public class DetailCapacity
    {
        public DateTime Date { get; set; }
        public int Capacity { get; set; }
        public int Booked { get; set; }
        public int Availability { get; set; }
        public decimal AdultPrice { get; set; }
        public decimal ChildPrice { get; set; }
        public decimal FamilyPrice { get; set; }
        public decimal InfantPrice { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class RouteDetails
    {
        public Package Package { get; set; }
        public IList<DetailCapacity> Capacities { get; set; }
    }

    public class ShoppingPackageRouteDetail
    {
        public RouteDetails RouteDetails { get; set; }
        public RequestStatus RequestStatus { get; set; }
    }
}