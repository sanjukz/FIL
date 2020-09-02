using FIL.Contracts.Interfaces.Commands;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.Zoom
{
    public class ZoomValidateUserCommand : ICommandWithResult<ZoomValidateUserCommandResult>
    {
        public Guid AltId { get; set; }
        public string UniqueId { get; set; }
        public bool IsZoomLandingPage { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class ZoomValidateUserCommandResult : ICommandResult
    {
        public long Id { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
        public string meetingNumber { get; set; }
        public string secretkey { get; set; }
        public string apikey { get; set; }
        public string roleId { get; set; }
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
        public string ImportantInformation { get; set; }
        public bool IsDonate { get; set; }
        public bool IsUpgradeToBSP { get; set; }
        public decimal Price { get; set; }
        public long TransactionId { get; set; }
        public Guid UserAltId { get; set; }
        public long ScheduleDetailId { get; set; }
    }
}