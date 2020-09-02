using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Contracts.Commands.Zoom;
using FIL.Foundation.Senders;
using FIL.Logging;
using FIL.Logging.Enums;
using FIL.Web.Feel.ViewModels.Zoom;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace FIL.Web.Feel.Controllers
{
    public class ZoomController : Controller
    {
        static readonly char[] padding = { '=' };
        private readonly ICommandSender _commandSender;
        private readonly IQuerySender _querySender;
        private readonly ISettings _settings;
        private readonly ILogger _logger;
        private readonly IMemoryCache _memoryCache;

        public ZoomController(ICommandSender commandSender, IQuerySender querySender, ILogger logger, IMemoryCache memoryCache, ISettings settings)
        {
            _commandSender = commandSender;
            _querySender = querySender;
            _logger = logger;
            _memoryCache = memoryCache;
            _settings = settings;
        }


        [HttpGet]
        [Route("api/validate-user/{altId}/{uniqueId}/{isZoomLandingPage?}")]
        public async Task<GetZoomDetailsViewModel> ValidateAndGetDetails(Guid altId, string uniqueId, bool? isZoomLandingPage = false)
        {
            try
            {
                ZoomValidateUserCommandResult zoomValidateCommandResult = await _commandSender.Send<ZoomValidateUserCommand, ZoomValidateUserCommandResult>(new ZoomValidateUserCommand
                {
                    AltId = altId,
                    UniqueId = uniqueId,
                    IsZoomLandingPage = isZoomLandingPage != null ? (bool)isZoomLandingPage : false
                }, new TimeSpan(2, 0, 0));

                string signature = String.Empty;

                if (zoomValidateCommandResult.success && !string.IsNullOrEmpty(zoomValidateCommandResult.apikey) && isZoomLandingPage != null && (bool)isZoomLandingPage)
                {
                    string ts = ToTimestamp(DateTime.UtcNow.ToUniversalTime()).ToString();
                    signature = GenerateSignature(zoomValidateCommandResult.apikey, zoomValidateCommandResult.secretkey, zoomValidateCommandResult.meetingNumber, ts, zoomValidateCommandResult.roleId);
                }
                else if (!zoomValidateCommandResult.success)
                {
                    return new GetZoomDetailsViewModel
                    {
                        success = false,
                        message = zoomValidateCommandResult.message
                    };
                }
                var VideoEndTimeVimeoString = "";
                var videoEndDateTime = DateTime.UtcNow;
                //Early access for host 10 minutes 
                if (zoomValidateCommandResult != null && zoomValidateCommandResult.success && zoomValidateCommandResult.EventDetail != null)
                {
                    if (zoomValidateCommandResult.LiveEventDetail != null && zoomValidateCommandResult.LiveEventDetail.EventStartDateTime != null)
                    {
                        var eventStartDateTime = zoomValidateCommandResult.EventDetail.StartDateTime;
                        var interactivity = (DateTime)zoomValidateCommandResult.LiveEventDetail.EventStartDateTime;
                        var diff = interactivity.Subtract(eventStartDateTime);
                        VideoEndTimeVimeoString = ((diff.Hours * 60) + diff.Minutes) + "m" + diff.Seconds + "s";
                    }
                    if (zoomValidateCommandResult.roleId == "1")
                    {
                        if (zoomValidateCommandResult.LiveEventDetail != null && zoomValidateCommandResult.LiveEventDetail.EventStartDateTime != null)
                        {
                            videoEndDateTime = Convert.ToDateTime(zoomValidateCommandResult.LiveEventDetail.EventStartDateTime);
                            zoomValidateCommandResult.LiveEventDetail.EventStartDateTime = Convert.ToDateTime(zoomValidateCommandResult.LiveEventDetail.EventStartDateTime).AddMinutes(-15);
                        }
                        else if (zoomValidateCommandResult.LiveEventDetail != null && zoomValidateCommandResult.LiveEventDetail.EventStartDateTime == null)
                        {
                            zoomValidateCommandResult.LiveEventDetail.EventStartDateTime = zoomValidateCommandResult.EventDetail.StartDateTime;
                        }
                    }
                    else
                    {
                        if (zoomValidateCommandResult.LiveEventDetail != null && zoomValidateCommandResult.LiveEventDetail.EventStartDateTime != null)
                        {
                            videoEndDateTime = Convert.ToDateTime(zoomValidateCommandResult.LiveEventDetail.EventStartDateTime);
                            zoomValidateCommandResult.LiveEventDetail.EventStartDateTime = Convert.ToDateTime(zoomValidateCommandResult.LiveEventDetail.EventStartDateTime).AddMinutes(-1);
                        }
                        else if (zoomValidateCommandResult.LiveEventDetail != null && zoomValidateCommandResult.LiveEventDetail.EventStartDateTime == null)
                        {
                            zoomValidateCommandResult.LiveEventDetail.EventStartDateTime = zoomValidateCommandResult.EventDetail.StartDateTime;
                        }
                    }
                }

                return new GetZoomDetailsViewModel
                {
                    success = zoomValidateCommandResult.success,
                    message = zoomValidateCommandResult.message,
                    signature = signature,
                    roleId = zoomValidateCommandResult.roleId,
                    email = zoomValidateCommandResult.email,
                    meetingNumber = zoomValidateCommandResult.meetingNumber,
                    apikey = zoomValidateCommandResult.apikey,
                    userName = zoomValidateCommandResult.userName,
                    isAudioAvailable = zoomValidateCommandResult.isAudioAvailable,
                    isChatAvailable = zoomValidateCommandResult.isChatAvailable,
                    isVideoAvailable = zoomValidateCommandResult.isVideoAvailable,
                    eventName = zoomValidateCommandResult.eventName,
                    timeStamp = zoomValidateCommandResult.timeStamp,
                    EventAttribute = zoomValidateCommandResult.EventAttribute,
                    Event = zoomValidateCommandResult.Event,
                    EventDetail = zoomValidateCommandResult.EventDetail,
                    EventHostMappings = zoomValidateCommandResult.EventHostMappings,
                    LiveEventDetail = zoomValidateCommandResult.LiveEventDetail,
                    hostStartTime = (zoomValidateCommandResult.LiveEventDetail != null && zoomValidateCommandResult.LiveEventDetail.EventStartDateTime != null) ? Convert.ToDateTime(zoomValidateCommandResult.EventDetail.StartDateTime).AddMinutes(-10) : DateTime.UtcNow,
                    utcTimeNow = DateTime.UtcNow.ToUniversalTime(),
                    ImportantInformation = zoomValidateCommandResult.ImportantInformation,
                    IsDonate = zoomValidateCommandResult.IsDonate,
                    UserAltId = zoomValidateCommandResult.UserAltId,
                    VideoEndTimeVimeoString = VideoEndTimeVimeoString,
                    VideoEndDateTime = videoEndDateTime,
                    IsUpgradeToBSP = zoomValidateCommandResult.IsUpgradeToBSP,
                    Price = zoomValidateCommandResult.Price,
                    TransactionId = zoomValidateCommandResult.TransactionId,
                    ScheduleDetailId = zoomValidateCommandResult.ScheduleDetailId
                };
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to get zoom details " + ex.Message, ex));
                return new GetZoomDetailsViewModel
                {
                    success = false,
                    message = "Internal Server Error, Please try again after"
                };
            }
        }

        [HttpGet]
        [Route("api/deactivate-user/{altId}/{uniqueId}")]
        public async Task<GetZoomDetailsViewModel> DeActivateUser(Guid altId, string uniqueId)
        {
            try
            {
                ZoomDeActivateUserCommandResult zoomDeActivateUserCommandResult = await _commandSender.Send<ZoomDeActivateUserCommand, ZoomDeActivateUserCommandResult>(new ZoomDeActivateUserCommand { AltId = altId, UniqueId = uniqueId }, new TimeSpan(2, 0, 0));
                return new GetZoomDetailsViewModel
                {
                    success = zoomDeActivateUserCommandResult.success,
                };
            }

            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to get zoom details " + ex.Message, ex));
                return new GetZoomDetailsViewModel
                {
                    success = false,
                    message = "Internal Server Error, Please try again after"
                };
            }
        }

        public static long ToTimestamp(DateTime value)
        {
            long epoch = (value.Ticks - 621355968000000000) / 10000;
            return epoch;
        }
        public static string GenerateSignature(string apiKey, string apiSecret, string meetingNumber, string ts, string role)
        {
            string message = String.Format("{0}{1}{2}{3}", apiKey, meetingNumber, ts, role);
            apiSecret = apiSecret ?? "";
            var encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(apiSecret);
            byte[] messageBytesTest = encoding.GetBytes(message);
            string msgHashPreHmac = System.Convert.ToBase64String(messageBytesTest);
            byte[] messageBytes = encoding.GetBytes(msgHashPreHmac);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                string msgHash = System.Convert.ToBase64String(hashmessage);
                string token = String.Format("{0}.{1}.{2}.{3}.{4}", apiKey, meetingNumber, ts, role, msgHash);
                var tokenBytes = System.Text.Encoding.UTF8.GetBytes(token);
                return System.Convert.ToBase64String(tokenBytes).TrimEnd(padding);
            }
        }
    }
}