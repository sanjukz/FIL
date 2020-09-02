using FIL.Contracts.Enums;

namespace FIL.Contracts.Models
{
    public class PlaceWeekOff
    {
        public long Id { get; set; }
        public long EventId { get; set; }
        public WeekOffDays WeekOffDay { get; set; }
    }
}