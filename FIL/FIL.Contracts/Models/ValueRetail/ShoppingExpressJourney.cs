using System.Collections.Generic;

namespace FIL.Contracts.Models.ValueRetail
{
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

    public class Adult
    {
        public decimal? Price { get; set; }
        public string Description { get; set; }
    }

    public class Children
    {
        public decimal? Price { get; set; }
        public string Description { get; set; }
    }

    public class Family
    {
        public decimal? Price { get; set; }
        public string Description { get; set; }
    }

    public class Unit
    {
        public decimal? Price { get; set; }
        public string Description { get; set; }
    }

    public class Infant
    {
        public decimal? Price { get; set; }
        public string Description { get; set; }
    }

    public class Prices
    {
        public Adult Adult { get; set; }
        public Children Children { get; set; }
        public Family Family { get; set; }
        public Infant Infant { get; set; }
        public Unit Unit { get; set; }
    }

    public class Availability
    {
        public string BookFromDate { get; set; }
        public string BookToDate { get; set; }
        public string TravelFromDate { get; set; }
        public string TravelToDate { get; set; }
        public List<string> BlockedDates { get; set; }
        public List<string> AllowedDates { get; set; }
        public List<bool> DaysOfWeek { get; set; }
        public double BookingLeadTime { get; set; }
        public double CancelLeadTime { get; set; }
        public double AmendLeadTime { get; set; }
    }

    public class ShoppingExpressRoutes
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
        public List<ReturnStop> ReturnStops { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Prices Prices { get; set; }
        public Availability Availability { get; set; }
    }

    public class Journey
    {
        public int JourneyType { get; set; }
        public List<ShoppingExpressRoutes> Routes { get; set; }
    }

    public class ShoppingExpressJourney
    {
        public List<Journey> Journeys { get; set; }
    }
}