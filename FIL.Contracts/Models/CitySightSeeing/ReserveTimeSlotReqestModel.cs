using System;
using System.Collections.Generic;

namespace FIL.Contracts.Models.CitySightSeeing
{
    public class BookingTimeSlotDetail
    {
        public string ticket_type { get; set; }
        public int count { get; set; }
        public IList<object> extra_options { get; set; }
    }

    public class TimeSlotData
    {
        public string distributor_id { get; set; }
        public string ticket_id { get; set; }
        public string pickup_point_id { get; set; }
        public DateTime from_date_time { get; set; }
        public DateTime to_date_time { get; set; }
        public IList<BookingTimeSlotDetail> booking_details { get; set; }
        public string distributor_reference { get; set; }
    }

    public class ReserveTimeSlotReqestModel
    {
        public string request_type { get; set; }
        public TimeSlotData data { get; set; }
    }
}