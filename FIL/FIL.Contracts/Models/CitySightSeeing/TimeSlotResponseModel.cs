using System;

namespace FIL.Contracts.Models.CitySightSeeing
{
    public class TimeSlotResponseData
    {
        public string reservation_reference { get; set; }
        public string distributor_reference { get; set; }
        public DateTime reservation_valid_until { get; set; }
        public string booking_status { get; set; }
    }

    public class TimeSlotResponseModel
    {
        public string response_type { get; set; }
        public TimeSlotResponseData data { get; set; }
    }
}