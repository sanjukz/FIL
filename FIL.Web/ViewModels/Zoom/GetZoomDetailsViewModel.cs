using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Feel.ViewModels.Zoom
{
    public class GetZoomDetailsViewModel
    {
        public bool success { get; set; }
        public string message { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
        public string meetingNumber { get; set; }
        public string apikey { get; set; }
        public string roleId { get; set; }
        public string signature { get; set; }
        public bool isAudioAvailable { get; set; }
        public bool isVideoAvailable { get; set; }
        public bool isChatAvailable { get; set; }
        public string eventName { get; set; }
        public long timeStamp { get; set; }
        public List<FIL.Contracts.DataModels.EventHostMapping> EventHostMappings { get; set; }
        public FIL.Contracts.DataModels.Event Event { get; set; }
        public FIL.Contracts.DataModels.LiveEventDetail LiveEventDetail { get; set; }
        public FIL.Contracts.DataModels.EventDetail EventDetail { get; set; }
        public FIL.Contracts.DataModels.EventAttribute EventAttribute { get; set; }
        public DateTime hostStartTime { get; set; }
        public DateTime utcTimeNow { get; set; }
        public DateTime VideoEndDateTime { get; set; }
        public string VideoEndTimeVimeoString { get; set; }
        public string ImportantInformation { get; set; }
        public bool IsDonate { get; set; }
        public bool IsUpgradeToBSP { get; set; }
        public Guid UserAltId { get; set; }
        public decimal Price { get; set; }
        public long TransactionId { get; set; }
        public long ScheduleDetailId { get; set; }
    }
}
