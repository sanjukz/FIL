using System;

namespace FIL.Contracts.Models
{
    public class PlaceHolidayDate
    {
        public long Id { get; set; }
        public long EventId { get; set; }
        public DateTime LeaveDateTime { get; set; }
    }
}