using System.Collections.Generic;

namespace FIL.Contracts.Models.Integrations.ExOz
{
    public class ExOzSessionResponse
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public string SessionName { get; set; }
        public List<ExOzProductOptionResponse> ProductOptions { get; set; }
        public string HasPickups { get; set; }
        public string Levy { get; set; }
        public bool IsExtra { get; set; }

        public string TourHour { get; set; }
        public string TourMinute { get; set; }
        public string TourDuration { get; set; }
    }

    public class SessionList
    {
        public List<ExOzSessionResponse> ProductSessions { get; set; }
    }
}