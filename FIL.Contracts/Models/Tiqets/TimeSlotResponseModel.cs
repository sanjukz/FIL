using System.Collections.Generic;

namespace FIL.Contracts.Models.Tiqets
{
    public class Timeslot
    {
        public bool is_available { get; set; }
        public string timeslot { get; set; }
    }

    public class TimeSlotResponseModel
    {
        public bool success { get; set; }
        public List<Timeslot> timeslots { get; set; }
    }
}