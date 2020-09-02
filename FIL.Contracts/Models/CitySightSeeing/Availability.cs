using System;
using System.Collections.Generic;

namespace FIL.Contracts.Models.CitySightSeeing
{
    public class Availability
    {
        public DateTime from_date_time { get; set; }
        public DateTime to_date_time { get; set; }
        public string vacancies { get; set; }
    }

    public class AvailabilityResponseData
    {
        public List<Availability> availabilities { get; set; }
    }

    public class AvailabilityResponse
    {
        public string response_type { get; set; }
        public AvailabilityResponseData data { get; set; }
    }
}