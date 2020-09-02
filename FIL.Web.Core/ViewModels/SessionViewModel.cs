using System;

namespace FIL.Web.Core.ViewModels
{
    public class SessionViewModel
    {
        public Guid AltId { get; set; }
        public bool IsAuthenticated { get; set; }
        public bool Success { get; set; }
        // Sessions expire after 15 minutes, rolling basis. So let the frontend know
        public int ExpirationSecondsRemaining { get; set; } = 60 * 15; 
        public UserViewModel User { get; set; }
        public string HubspotUserToken { get; set; }
        public string IntercomHash { get; set; }
        public ReportingUserViewModel ReportingUser { get; set; }
        public bool IsFeelExists { get; set; }
    }
}
