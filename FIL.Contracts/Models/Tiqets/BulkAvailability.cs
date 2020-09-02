namespace FIL.Contracts.Models.Tiqets
{
    public class variant
    {
        public int max_per_order { get; set; }
    }

    public class variant_availabilities
    {
        public variant variant { get; set; }
    }

    public class Time
    {
        public variant_availabilities variant_availabilities { get; set; }
        public int max_per_order { get; set; }
    }

    public class TimeslotAvailabilities
    {
        public Time time { get; set; }
    }

    public class Day
    {
        public TimeslotAvailabilities timeslot_availabilities { get; set; }
        public int max_per_order { get; set; }
    }

    public class DayAvailabilities
    {
        public Day day { get; set; }
    }

    public class BulkAvailabilit
    {
        public bool success { get; set; }
        public DayAvailabilities day_availabilities { get; set; }
    }
}