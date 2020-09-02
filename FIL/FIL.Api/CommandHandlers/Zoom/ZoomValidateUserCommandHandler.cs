using FIL.Api.Providers;
using FIL.Api.Repositories;
using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Contracts.Commands.Zoom;
using FIL.Contracts.DataModels;
using FIL.Contracts.Interfaces.Commands;
using FIL.Contracts.Utils;
using FIL.Logging;
using FIL.Logging.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.Zoom
{
    public class ZoomValidateUserCommandHandler : BaseCommandHandlerWithResult<ZoomValidateUserCommand, ZoomValidateUserCommandResult>
    {
        private readonly ILogger _logger;
        private readonly ISettings _settings;
        private readonly IZoomUserRepository _zoomUserRepository;
        private readonly IZoomMeetingRepository _zoomMeetingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEventHostMappingRepository _eventHostMappingRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventAttributeRepository _eventAttributeRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly ILiveEventDetailRepository _liveEventDetailRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IEventRepository _eventRepository;
        private readonly ITransactionDetailRepository _transactionDetailRepository;
        private readonly ILocalTimeZoneConvertProvider _localTimeZoneConvertProvider;
        private readonly ITransactionScheduleDetail _transactionScheduleDetail;
        private readonly IScheduleDetailRepository _scheduleDetailRepository;

        public ZoomValidateUserCommandHandler(
       ILogger logger,
       ISettings settings,
       IMediator mediator,
       IZoomUserRepository zoomUserRepository,
       IZoomMeetingRepository zoomMeetingRepository,
       IUserRepository userRepository,
       IEventHostMappingRepository eventHostMappingRepository,
       ITransactionRepository transactionRepository,
       IEventTicketAttributeRepository eventTicketAttributeRepository,
       IEventTicketDetailRepository eventTicketDetailRepository,
       IEventDetailRepository eventDetailRepository,
       IEventAttributeRepository eventAttributeRepository,
       IEventRepository eventRepository,
       ITransactionDetailRepository transactionDetailRepository,
       ILiveEventDetailRepository liveEventDetailRepository,
       ITransactionScheduleDetail transactionScheduleDetail,
       IScheduleDetailRepository scheduleDetailRepository,
       ILocalTimeZoneConvertProvider localTimeZoneConvertProvider) : base(mediator)
        {
            _logger = logger;
            _settings = settings;
            _zoomUserRepository = zoomUserRepository;
            _zoomMeetingRepository = zoomMeetingRepository;
            _userRepository = userRepository;
            _eventHostMappingRepository = eventHostMappingRepository;
            _transactionRepository = transactionRepository;
            _eventDetailRepository = eventDetailRepository;
            _eventAttributeRepository = eventAttributeRepository;
            _liveEventDetailRepository = liveEventDetailRepository;
            _eventRepository = eventRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _transactionDetailRepository = transactionDetailRepository;
            _localTimeZoneConvertProvider = localTimeZoneConvertProvider;
            _transactionScheduleDetail = transactionScheduleDetail;
            _scheduleDetailRepository = scheduleDetailRepository;
        }

        protected override async Task<ICommandResult> Handle(ZoomValidateUserCommand command)
        {
            ZoomValidateUserCommandResult zoomValidateUserCommandResult = new ZoomValidateUserCommandResult();
            try
            {
                var zoomUserModel = _zoomUserRepository.GetByAltId(command.AltId);
                var getTimeStamp = GetTimeStamp(zoomUserModel);
                zoomValidateUserCommandResult.timeStamp = getTimeStamp;
                //var checkTimeStamp = CheckValidTimeStamp(zoomUserModel);
                //if (!checkTimeStamp.IsValid)
                //{
                //    zoomValidateUserCommandResult.message = checkTimeStamp.Message;
                //    zoomValidateUserCommandResult.success = false;
                //    return zoomValidateUserCommandResult;
                //}
                if (zoomUserModel == null)
                {
                    if (zoomUserModel == null)
                    {
                        zoomValidateUserCommandResult.message = Constant.Zoom.Message.Invalid;
                    }
                    else
                    {
                        zoomValidateUserCommandResult.message = Constant.Zoom.Message.Active;
                    }
                    zoomValidateUserCommandResult.success = false;
                    return zoomValidateUserCommandResult;
                }
                else
                {
                    string apikey = _settings.GetConfigSetting<string>(SettingKeys.Integration.Zoom.API_Key);
                    string apiSecret = _settings.GetConfigSetting<string>(SettingKeys.Integration.Zoom.Secret_key);

                    var zoomMeetingModel = _zoomMeetingRepository.GetByEventId(zoomUserModel.EventId);

                    //Interactivity levels available
                    zoomValidateUserCommandResult.isAudioAvailable = true;
                    zoomValidateUserCommandResult.isVideoAvailable = true;
                    zoomValidateUserCommandResult.isChatAvailable = true;

                    bool isHost = false;
                    var access = Access.None;
                    var eventHostModel = new EventHostMapping();
                    if (zoomUserModel.RoleId != 21 && zoomUserModel.RoleId != 26) // If the user is attendee
                    {
                        var userModel = _userRepository.Get(zoomUserModel.UserId);
                        zoomValidateUserCommandResult.email = userModel.Email;
                        zoomValidateUserCommandResult.userName = userModel.FirstName + " " + userModel.LastName;
                        zoomValidateUserCommandResult.UserAltId = userModel.AltId;
                        var transactionDetailsModel = _transactionRepository.GetFeelOnlineDetails(zoomUserModel.TransactionId).Where(s => s.TransactionType == Contracts.Enums.TransactionType.LiveOnline).FirstOrDefault();
                        if (transactionDetailsModel == null)
                        {
                            return zoomValidateUserCommandResult;
                        }
                        //interactivity levels available based on ticket cat.
                        if (transactionDetailsModel.TicketCategoryId == 1360)
                        {
                            access = Access.GA;
                            zoomValidateUserCommandResult.isAudioAvailable = false;
                            zoomValidateUserCommandResult.isVideoAvailable = false;
                            zoomValidateUserCommandResult.isChatAvailable = false;
                        }
                        if (transactionDetailsModel.TicketCategoryId == 606)
                        {
                            access = Access.VIP;
                            zoomValidateUserCommandResult.isAudioAvailable = false;
                            zoomValidateUserCommandResult.isVideoAvailable = false;
                        }
                        if (transactionDetailsModel.TicketCategoryId == 12080 || transactionDetailsModel.TicketCategoryId == 19352)
                        {
                            zoomValidateUserCommandResult.isVideoAvailable = false;
                        }
                    }
                    else // Host user
                    {
                        eventHostModel = _eventHostMappingRepository.Get(zoomUserModel.EventHostUserId);
                        zoomValidateUserCommandResult.email = eventHostModel.Email;
                        zoomValidateUserCommandResult.userName = eventHostModel.FirstName + " " + eventHostModel.LastName;
                        isHost = true;
                    }

                    zoomValidateUserCommandResult.meetingNumber = zoomMeetingModel.MeetingNumber.ToString();
                    zoomValidateUserCommandResult.apikey = apikey;
                    zoomValidateUserCommandResult.secretkey = apiSecret;
                    zoomValidateUserCommandResult.success = true;
                    zoomValidateUserCommandResult.roleId = (zoomUserModel.RoleId == 21 || zoomUserModel.RoleId == 26) ? "1" : "0";

                    if (!zoomUserModel.IsActive && command.IsZoomLandingPage)
                    {
                        zoomUserModel.UniqueId = command.UniqueId;
                        zoomUserModel.IsActive = true;
                        _zoomUserRepository.Save(zoomUserModel);
                    }
                    var eventDetail = _eventDetailRepository.GetAllByEventId(zoomUserModel.EventId).FirstOrDefault();
                    zoomValidateUserCommandResult.eventName = eventDetail.Name;
                    if (!command.IsZoomLandingPage)
                    {
                        zoomValidateUserCommandResult.EventAttribute = _eventAttributeRepository.GetByEventDetailId(eventDetail.Id);
                        zoomValidateUserCommandResult.EventHostMappings = _eventHostMappingRepository.GetAllByEventId(eventDetail.EventId).OrderBy(s => s.StartDateTime).ToList();
                        foreach (var currentEHM in zoomValidateUserCommandResult.EventHostMappings)
                        {
                            currentEHM.StartDateTime = currentEHM.StartDateTime != null ? _localTimeZoneConvertProvider.ConvertToLocal((DateTime)currentEHM.StartDateTime, zoomValidateUserCommandResult.EventAttribute.TimeZone) : currentEHM.StartDateTime;
                            currentEHM.EndDateTime = currentEHM.EndDateTime != null ? _localTimeZoneConvertProvider.ConvertToLocal((DateTime)currentEHM.EndDateTime, zoomValidateUserCommandResult.EventAttribute.TimeZone) : currentEHM.EndDateTime;
                        }
                        zoomValidateUserCommandResult.EventDetail = _eventDetailRepository.Get(eventDetail.Id);
                        zoomValidateUserCommandResult.LiveEventDetail = _liveEventDetailRepository.GetByEventId(eventDetail.EventId);
                        if (isHost && eventHostModel.StartDateTime != null && zoomValidateUserCommandResult.LiveEventDetail != null)
                        {
                            zoomValidateUserCommandResult.LiveEventDetail.EventStartDateTime = eventHostModel.StartDateTime;
                        }
                        zoomValidateUserCommandResult.Event = _eventRepository.Get(eventDetail.EventId);
                        var eventTicketDetails = _eventTicketDetailRepository.GetByEventDetailId(eventDetail.Id).Where(s => s.IsEnabled).ToList();
                        var eventTicketAttributes = _eventTicketAttributeRepository.GetByEventTicketDetailIds(eventTicketDetails.Select(s => s.Id)).Where(s => s.IsEnabled).ToList();
                        if (eventTicketDetails.Select(s => s.TicketCategoryId).Contains(19452) || eventTicketDetails.Select(s => s.TicketCategoryId).Contains(12259)) // if donate
                        {
                            zoomValidateUserCommandResult.IsDonate = true;
                        }
                        if ((access == Access.GA || access == Access.VIP) && eventTicketDetails.Select(s => s.TicketCategoryId).Contains(19350) || eventTicketDetails.Select(s => s.TicketCategoryId).Contains(12079)) // if BSP Exists
                        {
                            var price = getBSPPrice(eventTicketDetails, eventTicketAttributes) - (access == Access.GA ? getGAPrice(eventTicketDetails, eventTicketAttributes) : getVIP(eventTicketDetails, eventTicketAttributes));
                            var transacions = _transactionRepository.GetAllSuccessfulTransactionByReferralId(zoomUserModel.TransactionId.ToString());
                            if (price >= 0 && !transacions.Any())
                            {
                                zoomValidateUserCommandResult.IsUpgradeToBSP = true;
                                zoomValidateUserCommandResult.Price = price;
                            }
                        }
                        if (zoomUserModel.RoleId != 21 && zoomUserModel.RoleId != 26) // If the user is attendee
                        {
                            var transactionDetail = _transactionDetailRepository.GetByTransactionId(zoomUserModel.TransactionId);
                            var eta = _eventTicketAttributeRepository.Get(transactionDetail.Count() > 1 ? transactionDetail.Where(s => s.TransactionType == Contracts.Enums.TransactionType.LiveOnline).FirstOrDefault().EventTicketAttributeId : transactionDetail.FirstOrDefault().EventTicketAttributeId);
                            zoomValidateUserCommandResult.ImportantInformation = eta.TicketCategoryNotes;
                            /* If Recurring then update Event Start & Interactivity start time */
                            if (eventDetail.EventFrequencyType == Contracts.Enums.EventFrequencyType.Recurring)
                            {
                                UpdateEventSchedule(zoomValidateUserCommandResult, transactionDetail.ToList());
                            }
                        }
                        else
                        {
                            zoomValidateUserCommandResult.ImportantInformation = zoomValidateUserCommandResult.EventHostMappings.Where(s => s.Id == zoomUserModel.EventHostUserId).FirstOrDefault().ImportantInformation;
                        }
                    }
                }
                zoomValidateUserCommandResult.success = true;
                zoomValidateUserCommandResult.TransactionId = zoomUserModel.TransactionId;
                return zoomValidateUserCommandResult;
            }
            catch (Exception e)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to validate zoom details " + e.Message, e));
                return zoomValidateUserCommandResult;
            }
        }

        private decimal getGAPrice(List<EventTicketDetail> eventTicketDetails, List<EventTicketAttribute> eventTicketAttributes)
        {
            var etd = eventTicketDetails.Where(s => s.TicketCategoryId == 1360).FirstOrDefault();
            var eta = eventTicketAttributes.Where(s => s.EventTicketDetailId == etd.Id).FirstOrDefault();
            return eta.Price;
        }

        private decimal getBSPPrice(List<EventTicketDetail> eventTicketDetails, List<EventTicketAttribute> eventTicketAttributes)
        {
            var etd = eventTicketDetails.Where(s => s.TicketCategoryId == 19350 || s.TicketCategoryId == 12079).FirstOrDefault();
            var eta = eventTicketAttributes.Where(s => s.EventTicketDetailId == etd.Id).FirstOrDefault();
            return eta.Price;
        }

        private decimal getVIP(List<EventTicketDetail> eventTicketDetails, List<EventTicketAttribute> eventTicketAttributes)
        {
            var etd = eventTicketDetails.Where(s => s.TicketCategoryId == 606).FirstOrDefault();
            var eta = eventTicketAttributes.Where(s => s.EventTicketDetailId == etd.Id).FirstOrDefault();
            return eta.Price;
        }

        private long GetTimeStamp(ZoomUser zoomUserModel)
        {
            long timeStamp = 0;
            var eventDetail = _eventDetailRepository.GetAllByEventId(zoomUserModel.EventId).FirstOrDefault();
            if (eventDetail.StartDateTime > DateTime.UtcNow)
            {
                var differenceTime = eventDetail.StartDateTime - DateTime.UtcNow;
                timeStamp = (differenceTime.Ticks - 621355968000000000) / 10000;
            }

            return timeStamp;
        }

        void UpdateEventSchedule(ZoomValidateUserCommandResult zoomValidateUserCommandHandler,
            List<FIL.Contracts.DataModels.TransactionDetail> transactionDetails)
        {
            var transactionDetail = transactionDetails.Where(s => s.TransactionType == Contracts.Enums.TransactionType.LiveOnline);
            var transactionScheduleDetail = _transactionScheduleDetail.GetAllByTransactionDetails(transactionDetail.Select(s => s.Id).ToList());
            var scheduleDetail = _scheduleDetailRepository.GetAllByIds(transactionScheduleDetail.Select(s => s.ScheduleDetailId).ToList()).FirstOrDefault();
            zoomValidateUserCommandHandler.EventDetail.StartDateTime = scheduleDetail.StartDateTime;
            zoomValidateUserCommandHandler.EventDetail.EndDateTime = scheduleDetail.EndDateTime;
            var timespan = zoomValidateUserCommandHandler.EventDetail.StartDateTime.Subtract((DateTime)zoomValidateUserCommandHandler.LiveEventDetail.EventStartDateTime);
            zoomValidateUserCommandHandler.LiveEventDetail.EventStartDateTime = ((DateTime)zoomValidateUserCommandHandler.LiveEventDetail.EventStartDateTime).Add(timespan);
            zoomValidateUserCommandHandler.ScheduleDetailId = scheduleDetail.Id;
        }

        public TimeStampResponse CheckValidTimeStamp(ZoomUser zoomUser)
        {
            //This is the utility to check if meeting is requested in the given interval
            TimeStampResponse timeStampResponse = new TimeStampResponse();
            var eventDetail = _eventDetailRepository.GetAllByEventId(zoomUser.EventId).FirstOrDefault();

            var currentUtcDateTime = DateTime.UtcNow.AddMinutes(60); //early 10 min access
            if (eventDetail.StartDateTime <= currentUtcDateTime && eventDetail.EndDateTime >= DateTime.UtcNow)
            {
                timeStampResponse.IsValid = true;
            }
            else
            {
                if (currentUtcDateTime < eventDetail.EndDateTime)
                {
                    timeStampResponse.Message = Constant.Zoom.Message.MeetingNotStarted;
                }
                else
                {
                    timeStampResponse.Message = Constant.Zoom.Message.MeetingEnd;
                }

                timeStampResponse.IsValid = false;
            }
            return timeStampResponse;
        }
    }

    public class TimeStampResponse
    {
        public bool IsValid { get; set; }
        public string Message { get; set; }
    }

    enum Access
    {
        None = 1,
        GA,
        BSP,
        VIP
    };
}