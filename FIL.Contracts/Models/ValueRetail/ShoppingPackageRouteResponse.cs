using System;
using System.Collections.Generic;

namespace FIL.Contracts.Models.ValueRetail.ShoppingPackage
{
    public class RoutePricePerPerson
    {
        public int PackagePriceType { get; set; }
        public decimal Price { get; set; }
        public decimal MaxPrice { get; set; }
    }

    public class RoutePricePerUnitOption
    {
        public decimal MaxPrice { get; set; }
        public dynamic MaxType { get; set; }
        public int MaxUnitPerBooking { get; set; }
        public int MaxUnits { get; set; }
        public int MinUnitPerBooking { get; set; }
        public decimal Price { get; set; }
        public int PriceFor { get; set; }
        public string UnitType { get; set; }
    }

    public class RoutePackage
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
        public IList<DateTime> BlockedDate { get; set; }
        public IList<object> AllowedDate { get; set; }
        public string Category { get; set; }
        public decimal BookingLeadTime { get; set; }
        public decimal AmendLeadTime { get; set; }
        public decimal CancelLeadTime { get; set; }
        public int PriceOptionType { get; set; }
        public bool RedemptionDateNotRequired { get; set; }
        public IList<RoutePricePerPerson> PricePerPersons { get; set; }
        public RoutePricePerUnitOption PricePerUnitOption { get; set; }
        public string DaysofWeek { get; set; }
        public decimal TotalGrossPrice { get; set; }
        public decimal TotalNetPrice { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsMice { get; set; }
        public IList<object> ShoppingExpressItems { get; set; }
    }

    public class Image
    {
        public string StandardDesc { get; set; }
        public string StandardURL { get; set; }
        public string ThumbnailDesc { get; set; }
        public string ThumbnailURL { get; set; }
        public int ImageType { get; set; }
        public bool Lead { get; set; }
    }

    public class ReturnStop
    {
        public int RouteId { get; set; }
        public int StopId { get; set; }
        public int StopOrder { get; set; }
        public string Name { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public string LocationAddress { get; set; }
        public string ReturnTime { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class RouteAdult
    {
        public decimal Price { get; set; }
        public string Description { get; set; }
    }

    public class RouteChildren
    {
        public decimal Price { get; set; }
        public string Description { get; set; }
    }

    public class RouteFamily
    {
        public decimal Price { get; set; }
        public string Description { get; set; }
    }

    public class RouteInfant
    {
        public decimal Price { get; set; }
        public string Description { get; set; }
    }

    public class Prices
    {
        public RouteAdult Adult { get; set; }
        public RouteChildren Children { get; set; }
        public RouteFamily Family { get; set; }
        public RouteInfant Infant { get; set; }
    }

    public class RouteAvailability
    {
        public DateTime BookFromDate { get; set; }
        public DateTime BookToDate { get; set; }
        public DateTime TravelFromDate { get; set; }
        public DateTime TravelToDate { get; set; }
        public IList<DateTime> BlockedDates { get; set; }
        public IList<object> AllowedDates { get; set; }
        public IList<bool> DaysOfWeek { get; set; }
        public decimal BookingLeadTime { get; set; }
        public decimal CancelLeadTime { get; set; }
        public decimal AmendLeadTime { get; set; }
    }

    public class Route
    {
        public int RouteId { get; set; }
        public string DepartureTime { get; set; }
        public int LinkedRouteId { get; set; }
        public string ReturnTime { get; set; }
        public string Name { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public string LocationAddress { get; set; }
        public int StopId { get; set; }
        public int StopOrder { get; set; }
        public IList<ReturnStop> ReturnStops { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Prices Prices { get; set; }
        public RouteAvailability Availability { get; set; }
    }

    public class Journey
    {
        public int JourneyType { get; set; }
        public IList<Route> Routes { get; set; }
    }

    public class RoutePackages
    {
        public RoutePackage Package { get; set; }
        public IList<Image> Images { get; set; }
        public IList<Journey> Journeys { get; set; }
    }

    public class ShoppingPackageRouteResponse
    {
        public IList<RoutePackages> Packages { get; set; }
        public RequestStatus RequestStatus { get; set; }
    }
}