using System;

namespace FIL.Contracts.Models.CitySightSeeing
{
    public class ReserveTimeslotData
    {
        public string reservation_reference { get; set; }
        public string distributor_reference { get; set; }
        public DateTime reservation_valid_until { get; set; }
        public string booking_status { get; set; }
    }

    public class ReserveTimeslot
    {
        public string request_type { get; set; }
        public ReserveTimeslotData data { get; set; }
    }
}