using GeoTimeZone;
using FIL.Api.Integrations;
using FIL.Api.Repositories;
using FIL.Api.Utilities;
using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Contracts.Commands.CitySightSeeing;
using FIL.Contracts.DataModels;
using FIL.Contracts.Interfaces.Commands;
using FIL.Contracts.Models.CitySightSeeing;
using FIL.Logging;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TimeZoneConverter;

namespace FIL.Api.CommandHandlers.CitySightSeeing
{
    public class ReserveTimeSlotCommandHandler : BaseCommandHandlerWithResult<ReserveTimeSlotCommand, ReserveTimeSlotCommandResult>
    {
        private readonly ILogger _logger;
        private readonly ISettings _settings;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionDetailRepository _transactionDetailRepository;
        private readonly IFeelBarcodeMappingRepository _feelBarcodeMappingRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly IEventAttributeRepository _eventAttributeRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly ICityRepository _cityRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICurrencyTypeRepository _currencyTypeRepository;
        private readonly ICitySightSeeingTicketDetailRepository _citySightSeeingTicketDetailRepository;
        private readonly ICitySightSeeingTicketRepository _citySightSeeingTicketRepository;
        private readonly ICitySightSeeingEventDetailMappingRepository _citySightSeeingEventDetailMappingRepository;
        private readonly ICitySightSeeingLocationRepository _citySightSeeingLocationRepository;
        private readonly IGoogleMapApi _googleMapApi;

        public ReserveTimeSlotCommandHandler(IEventDetailRepository eventDetailRepository,
            ITransactionRepository transactionRepository,
            ITransactionDetailRepository transactionDetailRepository,
            ITransactionSeatDetailRepository transactionSeatDetailRepository,
            IFeelBarcodeMappingRepository feelBarcodeMappingRepository,
            IEventTicketDetailRepository eventTicketDetailRepository,
            IEventTicketAttributeRepository eventTicketAttributeRepository,
            ITicketCategoryRepository ticketCategoryRepository,
            IEventAttributeRepository eventAttributeRepository,
            IEventRepository eventRepository,
            IVenueRepository venueRepository,
            ICityRepository cityRepository,
            IStateRepository stateRepository,
            ICountryRepository countryRepository,
            ICurrencyTypeRepository currencyTypeRepository,
            ICitySightSeeingTicketDetailRepository citySightSeeingTicketDetailRepository,
            ICitySightSeeingTicketRepository citySightSeeingTicketRepository,
            ICitySightSeeingEventDetailMappingRepository citySightSeeingEventDetailMappingRepository,
            Logging.ILogger logger, ICitySightSeeingLocationRepository citySightSeeingLocationRepository,
            IMatchLayoutSectionRepository matchLayoutSectionRepository, ISettings settings, IGoogleMapApi googleMapApi,
            IMediator mediator) : base(mediator)
        {
            _eventDetailRepository = eventDetailRepository;
            _transactionRepository = transactionRepository;
            _transactionDetailRepository = transactionDetailRepository;
            _feelBarcodeMappingRepository = feelBarcodeMappingRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _eventAttributeRepository = eventAttributeRepository;
            _eventRepository = eventRepository;
            _venueRepository = venueRepository;
            _cityRepository = cityRepository;
            _stateRepository = stateRepository;
            _countryRepository = countryRepository;
            _currencyTypeRepository = currencyTypeRepository;
            _citySightSeeingTicketDetailRepository = citySightSeeingTicketDetailRepository;
            _citySightSeeingTicketRepository = citySightSeeingTicketRepository;
            _citySightSeeingEventDetailMappingRepository = citySightSeeingEventDetailMappingRepository;
            _logger = logger;
            _settings = settings;
            _googleMapApi = googleMapApi;
            _citySightSeeingLocationRepository = citySightSeeingLocationRepository;
        }

        protected override async Task<ICommandResult> Handle(ReserveTimeSlotCommand command)
        {
            ReserveTimeSlotCommandResult results = new ReserveTimeSlotCommandResult();
            ReserveTimeSlotReqestModel reserveTimelotRequestModel = new ReserveTimeSlotReqestModel();

            IEnumerable<long> eventTicketAttributeIds = command.EventTicketAttributeList.Select(s => s.Id).Distinct();
            IEnumerable<Contracts.Models.EventTicketAttribute> eventTicketAttributes = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.EventTicketAttribute>>(_eventTicketAttributeRepository.GetByIds(eventTicketAttributeIds));

            var allETD = _eventTicketDetailRepository.GetByEventTicketDetailsIds(eventTicketAttributes.Select(s => s.EventTicketDetailId).Distinct()).Distinct();
            var allTC = _ticketCategoryRepository.GetByTicketCategoryIds(allETD.Select(s => s.TicketCategoryId).Distinct()).Distinct();
            var allED = _eventDetailRepository.GetByIds(allETD.Select(s => s.EventDetailId).Distinct()).Distinct().FirstOrDefault();
            var citySightEventDetails = _citySightSeeingEventDetailMappingRepository.GetByEventDetailId(allED.Id);
            var citySightSeeingTickets = _citySightSeeingTicketRepository.Get(citySightEventDetails.CitySightSeeingTicketId);
            var citySightSeeingTicketDetails = _citySightSeeingTicketDetailRepository.GetByTicketId(citySightSeeingTickets.TicketId);
            var citySightSeeingLocations = _citySightSeeingLocationRepository.Get(citySightSeeingTickets.CitySightSeeingLocationId);

            var locationDetail = await _googleMapApi.GetLatLongFromAddress(citySightSeeingLocations.Name);
            string tz = ""; DateTime fromDate = DateTime.Now; ; var offset = "";
            if (locationDetail.Success)
            {
                double lat = Convert.ToDouble(locationDetail.Result.lat);
                double lng = Convert.ToDouble(locationDetail.Result.lng);
                tz = TimeZoneLookup.GetTimeZone(lat, lng).Result;
                TimeZoneInfo tzi = TZConvert.GetTimeZoneInfo(tz);
                var visitDate = command.EventTicketAttributeList[0].VisitDate;
                var timeSlot = command.EventTicketAttributeList[0].TimeSlot;
                var utcOffset = tzi.BaseUtcOffset.ToString().Split(":")[0];
                var minutes = Convert.ToInt64(tzi.BaseUtcOffset.ToString().Split(":")[1]);
                if (utcOffset.Contains("-"))
                {
                    var hours = Convert.ToInt64((tzi.BaseUtcOffset.ToString().Split(":")[0]).Split("-")[1]);
                    visitDate = visitDate.AddHours(hours).AddMinutes(minutes);
                    offset = utcOffset;
                }
                else
                {
                    var hours = Convert.ToInt64((tzi.BaseUtcOffset.ToString().Split(":")[0]));
                    visitDate = visitDate.AddHours(-hours).AddMinutes(-minutes);
                    offset = "+" + utcOffset;
                }
                var formattedDateTime = visitDate.Year + "-" + visitDate.Month + "-" + visitDate.Day + "T" + timeSlot;
                fromDate = Convert.ToDateTime(formattedDateTime);
            }

            var duration = citySightSeeingTicketDetails.Duration;

            reserveTimelotRequestModel.request_type = "reserve";
            TimeSlotData timeSlotData = new TimeSlotData();
            timeSlotData.distributor_id = _settings.GetConfigSetting<string>(SettingKeys.Integration.CitySightSeeing.DistributorId);
            timeSlotData.ticket_id = citySightSeeingTickets.TicketId;
            timeSlotData.pickup_point_id = (citySightSeeingTicketDetails.PickupPoints != null && citySightSeeingTicketDetails.PickupPoints != "") ? citySightSeeingTicketDetails.PickupPoints : "";

            DateTime endDate = DateTime.Now;
            List<BookingTimeSlotDetail> bookingTimeSlotDetailList = new List<BookingTimeSlotDetail>();
            foreach (Contracts.Commands.Transaction.EventTicketAttribute ticketAttributes in command.EventTicketAttributeList)
            {
                BookingTimeSlotDetail bookingTimeSlotDetail = new BookingTimeSlotDetail();
                var currentTA = ticketAttributes;
                Contracts.Models.EventTicketAttribute checkoutCommandEventTicketAttribute = eventTicketAttributes.Where(w => w.Id == ticketAttributes.Id).FirstOrDefault();
                EventTicketDetail eventTicketDetail = allETD.Where(s => s.Id == checkoutCommandEventTicketAttribute.EventTicketDetailId).FirstOrDefault();
                var ticketCategoryId = allTC.Where(s => s.Id == eventTicketDetail.TicketCategoryId).FirstOrDefault();
                bookingTimeSlotDetail.count = ticketAttributes.TotalTickets;
                var formattedTicketCat = ticketCategoryId.Name.Split("(");
                bookingTimeSlotDetail.ticket_type = formattedTicketCat[0].ToUpper();
                bookingTimeSlotDetail.extra_options = null;
                bookingTimeSlotDetailList.Add(bookingTimeSlotDetail);
            }
            if (duration.Contains("day"))
            {
                string[] day = duration.Split("day");
                endDate = fromDate.AddDays(Convert.ToDouble(day[0]));
            }
            if (duration.Contains("hour"))
            {
                string[] hour = duration.Split("hour");
                endDate = fromDate.AddHours(Convert.ToDouble(hour[0]));
            }
            var fromDate_with_time_slot = Convert.ToDateTime(fromDate.ToString() + offset);
            var endDate_with_time_slot = Convert.ToDateTime(endDate.ToString() + offset);

            timeSlotData.from_date_time = fromDate_with_time_slot;
            timeSlotData.to_date_time = endDate_with_time_slot;
            timeSlotData.booking_details = bookingTimeSlotDetailList;
            reserveTimelotRequestModel.data = timeSlotData;
            reserveTimelotRequestModel.data.distributor_reference = RandomDigits(10);
            var response = Mapper<TimeSlotResponseModel>.MapFromJson(await ReserveBooking(reserveTimelotRequestModel));
            if (response != null && response.data.booking_status == "Reserved")
            {
                results.Success = true;
                results.FromTime = fromDate.ToString() + offset;
                results.EndTime = endDate.ToString() + offset;
                results.Reservation_reference = response.data.reservation_reference;
                results.Reservation_valid_until = response.data.reservation_valid_until;
                results.Distributor_reference = response.data.distributor_reference;
                results.TicketId = citySightSeeingTicketDetails.TicketId;
                results.TimeSlot = command.EventTicketAttributeList[0].TimeSlot;
            }
            else
            {
                results.Success = false;
            }
            return results;
        }

        public async Task<string> ReserveBooking(ReserveTimeSlotReqestModel reserveTimelotRequestModel)
        {
            try
            {
                var builder = new UriBuilder(_settings.GetConfigSetting<string>(SettingKeys.Integration.CitySightSeeing.Endpoint));
                builder.Port = -1;
                string endpoint = builder.ToString();
                string responseData;
                using (var httpClient = new HttpClient())
                {
                    string auth = string.Format(_settings.GetConfigSetting<string>(SettingKeys.Integration.CitySightSeeing.RequestIdentifier) + ":" + _settings.GetConfigSetting<string>(SettingKeys.Integration.CitySightSeeing.Token));
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("x-request-authentication", _settings.GetConfigSetting<string>(SettingKeys.Integration.CitySightSeeing.RequestAuthentication));
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("x-request-identifier", _settings.GetConfigSetting<string>(SettingKeys.Integration.CitySightSeeing.RequestIdentifier));
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                    string json = JsonConvert.SerializeObject(reserveTimelotRequestModel, Formatting.Indented);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(new Uri(endpoint), content);
                    responseData = await response.Content.ReadAsStringAsync();
                    return responseData;
                }
            }
            catch (Exception e)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, new Exception(e.Message));
                return null;
            }
        }

        public string RandomDigits(int length)
        {
            var random = new Random();
            string s = string.Empty;
            for (int i = 0; i < length; i++)
                s = String.Concat(s, random.Next(10).ToString());
            return s;
        }
    }
}