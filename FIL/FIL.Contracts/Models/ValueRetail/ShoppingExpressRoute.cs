using System.Collections.Generic;

namespace FIL.Contracts.Models.ValueRetail
{
    public class Stop
    {
        public int StopId { get; set; }
        public string Time { get; set; }
        public string LocationName { get; set; }
        public string LocationAddress { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
    }

    public class Timetable
    {
        public string Date { get; set; }
        public int JourneyType { get; set; }
        public List<Stop> Stops { get; set; }
    }

    public class RouteCapacity
    {
        public string Date { get; set; }
        public int Capacity { get; set; }
        public int Booked { get; set; }
        public int Availability { get; set; }
        public decimal AdultPrice { get; set; }
        public decimal ChildPrice { get; set; }
        public decimal FamilyPrice { get; set; }
        public decimal InfantPrice { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class AvailabilityList
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public List<string> BlockedDates { get; set; }
        public bool IsSunday { get; set; }
        public bool IsMonday { get; set; }
        public bool IsTuesday { get; set; }
        public bool IsWednesday { get; set; }
        public bool IsThursday { get; set; }
        public bool IsFriday { get; set; }
        public bool IsSaturday { get; set; }
        public List<RouteCapacity> Capacity { get; set; }
    }

    public class PriceList
    {
        public int PriceGroupType { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
    }

    public class RequestStatus
    {
        public bool Success { get; set; }
        public List<object> Errors { get; set; }
    }

    public class ShoppingExpressRoute
    {
        public Timetable Timetable { get; set; }
        public AvailabilityList Availability { get; set; }
        public List<PriceList> Prices { get; set; }
        public RequestStatus RequestStatus { get; set; }
    }
}