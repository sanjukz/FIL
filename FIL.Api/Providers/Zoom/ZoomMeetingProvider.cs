using FIL.Api.Integrations.Zoom;
using FIL.Api.Repositories;
using FIL.Api.Utilities;
using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Contracts.Enums;
using FIL.Contracts.Models.Zoom;
using FIL.Logging;
using FIL.Logging.Enums;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;

namespace FIL.Api.Providers.Zoom
{
    public interface IZoomMeetingProvider
    {
        bool CreateMeeting(FIL.Contracts.DataModels.Transaction transaction, bool? isZeroPriceEvent = false);
    }

    public class ZoomMeetingProvider : IZoomMeetingProvider
    {
        private readonly IZoomAPI _zoomAPI;
        private readonly ISettings _settings;
        private readonly ILogger _logger;
        private readonly IUserRepository _userRepository;
        private readonly IZoomMeetingRepository _zoomMeetingRepository;
        private readonly IZoomUserRepository _zoomUserRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IEventHostMappingRepository _eventHostMappingRepository;
        private readonly IEventCategoryRepository _eventCategoryRepository;
        private readonly ITransactionPaymentDetailRepository _transactionPaymentDetailRepository;
        private readonly IEventRepository _eventRepository;

        public ZoomMeetingProvider(
            IZoomAPI zoomAPI,
            ILogger logger,
            ISettings settings,
            IUserRepository userRepository,
            IZoomMeetingRepository zoomMeetingRepository,
            ITransactionRepository transactionRepository,
            IZoomUserRepository zoomUserRepository,
            IEventHostMappingRepository eventHostMappingRepository,
            IEventCategoryRepository eventCategoryRepository,
            ITransactionPaymentDetailRepository transactionPaymentDetailRepository,
             IEventRepository eventRepository)
        {
            _zoomAPI = zoomAPI;
            _logger = logger;
            _settings = settings;
            _userRepository = userRepository;
            _zoomMeetingRepository = zoomMeetingRepository;
            _transactionRepository = transactionRepository;
            _zoomUserRepository = zoomUserRepository;
            _eventHostMappingRepository = eventHostMappingRepository;
            _eventCategoryRepository = eventCategoryRepository;
            _transactionPaymentDetailRepository = transactionPaymentDetailRepository;
            _eventRepository = eventRepository;
        }

        public bool CreateMeeting(FIL.Contracts.DataModels.Transaction transaction, bool? isZeroPriceEvent = false)
        {
            try
            {
                var liveOnlineTransactionDetailModel = transaction.ChannelId == Channels.Feel ? _transactionRepository.GetFeelOnlineDetails(transaction.Id).Where(s => s.TransactionType == Contracts.Enums.TransactionType.LiveOnline).FirstOrDefault() : _transactionRepository.GetFeelOnlineDetails(transaction.Id).FirstOrDefault();

                if (liveOnlineTransactionDetailModel != null)
                {
                    //check if subcategory is LiveOnline
                    bool isLiveOnline = false;
                    if (liveOnlineTransactionDetailModel.Channel == Channels.Feel && liveOnlineTransactionDetailModel.EventcategoryId != 0)
                    {
                        var @event = _eventRepository.Get(liveOnlineTransactionDetailModel.EventId);
                        if (@event != null && @event.MasterEventTypeId == MasterEventType.Online)
                        {
                            isLiveOnline = true;
                        }
                    }
                    else if (liveOnlineTransactionDetailModel.Channel == Channels.Website)
                    {
                        var eventModel = _eventRepository.Get(liveOnlineTransactionDetailModel.EventId);
                        if (eventModel.EventCategoryId == 119)
                        {
                            isLiveOnline = true;
                        }
                    }

                    if (isLiveOnline)
                    {
                        //check if meeting is created for requested event
                        var zoomMeeting = _zoomMeetingRepository.GetByEventId(liveOnlineTransactionDetailModel.EventId);

                        // Update Zero Price Event Transaction Status to success
                        if (isZeroPriceEvent != null && (bool)isZeroPriceEvent)
                        {
                            UpdateTransactionStatus(transaction);
                        }

                        if (zoomMeeting == null)
                        {
                            var userModel = _userRepository.GetByAltId(liveOnlineTransactionDetailModel.UserTransactionAltId);
                            var createMeeting = Create(liveOnlineTransactionDetailModel);

                            var insertUsers = InsertUsers(liveOnlineTransactionDetailModel);

                            var saveUser = SaveTransactedUser(userModel.Id, liveOnlineTransactionDetailModel, transaction.Id);
                        }
                        else
                        {
                            var userModel = _userRepository.GetByAltId(liveOnlineTransactionDetailModel.UserTransactionAltId);
                            var saveUser = SaveTransactedUser(userModel.Id, liveOnlineTransactionDetailModel, transaction.Id);
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to create zoom meeting" + e.Message, e));
                return false;
            }
            return true;
        }

        private void UpdateTransactionStatus(FIL.Contracts.DataModels.Transaction transaction)
        {
            transaction.TransactionStatusId = Contracts.Enums.TransactionStatus.Success;
            transaction.UpdatedUtc = DateTime.UtcNow;
            _transactionRepository.Save(transaction);

            _transactionPaymentDetailRepository.Save(new Contracts.DataModels.TransactionPaymentDetail
            {
                Amount = transaction.NetTicketAmount.ToString(),
                CreatedBy = Guid.NewGuid(),
                CreatedUtc = DateTime.UtcNow,
                PayConfNumber = "",
                PaymentDetail = "FAP Live 0 price event",
                PaymentOptionId = Contracts.Enums.PaymentOptions.CashCard,
                PaymentGatewayId = Contracts.Enums.PaymentGateway.Stripe,
                TransactionId = transaction.Id,
                UserCardDetailId = 6,
                RequestType = "Charge Received"
            });
        }

        private bool InsertUsers(LiveOnlineTransactionDetailResponseModel liveOnlineTransactionDetailModel)
        {
            var eventHostUsers = _eventHostMappingRepository.GetAllByEventId(liveOnlineTransactionDetailModel.EventId);

            foreach (var currentHost in eventHostUsers)
            {
                _zoomUserRepository.Save(new Contracts.DataModels.ZoomUser
                {
                    AltId = System.Guid.NewGuid(),
                    RoleId = 21, //check for prod host role Id
                    IsActive = false,
                    IsEnabled = true,
                    EventHostUserId = currentHost.Id,
                    EventId = liveOnlineTransactionDetailModel.EventId,
                    ModifiedBy = liveOnlineTransactionDetailModel.CreatorAltId
                });
            }
            return true;
        }

        private bool Create(LiveOnlineTransactionDetailResponseModel liveOnlineTransactionDetailModel)
        {
            ZoomCreateMeetingModel zoomCreateMeetingModel = new ZoomCreateMeetingModel();
            Contracts.Models.Zoom.Settings settings = new Contracts.Models.Zoom.Settings();
            var bearerToken = GenerateToken();
            var hostUserId = GetHostId(liveOnlineTransactionDetailModel, bearerToken);

            zoomCreateMeetingModel.topic = liveOnlineTransactionDetailModel.Name;
            zoomCreateMeetingModel.start_time = liveOnlineTransactionDetailModel.StartDateTime.ToString();

            if (liveOnlineTransactionDetailModel.Channel == Channels.Feel)
            {
                zoomCreateMeetingModel.duration = GetDuration(liveOnlineTransactionDetailModel.StartTime, liveOnlineTransactionDetailModel.EndTime); // In minutes
            }
            else if (liveOnlineTransactionDetailModel.Channel == Channels.Website)
            {
                TimeSpan span = (liveOnlineTransactionDetailModel.EndDateTime - liveOnlineTransactionDetailModel.StartDateTime);

                zoomCreateMeetingModel.duration = Convert.ToInt32(span.Hours);
            }
            zoomCreateMeetingModel.timezone = "UTC";
            zoomCreateMeetingModel.agenda = liveOnlineTransactionDetailModel.Description;
            settings.mute_upon_entry = true;
            settings.registrants_confirmation_email = false;
            settings.registrants_email_notification = false;
            settings.approval_type = 2;
            zoomCreateMeetingModel.settings = settings;

            string meetingEndpoint = "/users/" + hostUserId + "/meetings";
            var createMeetingResponse = Mapper<ZoomCreateMeetingResponseModel>.MapFromJson(_zoomAPI.PostAPIDetails(meetingEndpoint, zoomCreateMeetingModel, bearerToken).Result);

            if (createMeetingResponse != null)
            {
                _zoomMeetingRepository.Save(new Contracts.DataModels.ZoomMeeting
                {
                    MeetingNumber = createMeetingResponse.id,
                    EventId = liveOnlineTransactionDetailModel.EventId,
                    HostUserId = hostUserId,
                    MeetingUuid = createMeetingResponse.uuid,
                    DurationMinutes = createMeetingResponse.duration,
                    IsEnabled = true,
                    ModifiedBy = liveOnlineTransactionDetailModel.CreatorAltId
                });
            }
            else
            {
                return false;
            }
            return true;
        }

        private int GetDuration(string startTime, string endTime)
        {
            int hr = Convert.ToInt16(endTime.Split(":")[0]) - Convert.ToInt16(startTime.Split(":")[0]);
            int min = Convert.ToInt16(endTime.Split(":")[1]) - Convert.ToInt16(startTime.Split(":")[1]);
            int totalminutes = (hr * 60) + min;
            return totalminutes;
        }

        public string GetHostId(LiveOnlineTransactionDetailResponseModel liveOnlineTransactionDetailResponseModel, string bearerToken)
        {
            try
            {
                //var hostUsers = _eventHostMappingRepository.GetAllByEventId(liveOnlineTransactionDetailResponseModel.EventId).FirstOrDefault();
                //ZoomCreateUserModel zoomCreateUserModel = new ZoomCreateUserModel();
                //zoomCreateUserModel.action = "custCreate";
                //UserInfo userInfo = new UserInfo();
                //userInfo.email = hostUsers.Email;
                //userInfo.first_name = hostUsers.FirstName;
                //userInfo.last_name = hostUsers.LastName;
                //userInfo.type = 1;
                //zoomCreateUserModel.user_info = userInfo;

                //var createUser = Mapper<ZoomCreateUserResponseModel>.MapFromJson(_zoomAPI.PostAPIDetails("/users", zoomCreateUserModel, bearerToken).Result);
                //if (createUser != null && !string.IsNullOrEmpty(createUser.id))
                //{
                //    return createUser.id;
                //}
                //else
                //{
                //    return "eh8X6_qoQlCfo4V1MhXbmA";
                //}
                return "eh8X6_qoQlCfo4V1MhXbmA";
            }
            catch (Exception e)
            {
                return "eh8X6_qoQlCfo4V1MhXbmA";
            }
        }

        public string GenerateToken()
        {
            try
            {
                string apikey = _settings.GetConfigSetting<string>(SettingKeys.Integration.Zoom.API_Key);
                string apiSecret = _settings.GetConfigSetting<string>(SettingKeys.Integration.Zoom.Secret_key);
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(apiSecret));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var header = new JwtHeader(credentials);
                var epoch = (DateTime.UtcNow.AddMinutes(10) - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
                var payload = new JwtPayload { { "iss", apikey }, { "exp", epoch },
                };
                var secToken = new JwtSecurityToken(header, payload);
                var handler = new JwtSecurityTokenHandler();
                return handler.WriteToken(secToken);
            }
            catch (Exception e)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to generate token " + e.Message, e));
                return null;
            }
        }

        public bool SaveTransactedUser(long userId, LiveOnlineTransactionDetailResponseModel liveOnlineTransactionDetailModel, long transactionId)
        {
            _zoomUserRepository.Save(new Contracts.DataModels.ZoomUser
            {
                AltId = System.Guid.NewGuid(),
                RoleId = 22, //check for prod attendee role Id
                IsActive = false,
                IsEnabled = true,
                UserId = userId,
                TransactionId = transactionId,
                EventId = liveOnlineTransactionDetailModel.EventId,
                ModifiedBy = liveOnlineTransactionDetailModel.CreatorAltId
            });
            return true;
        }
    }
}