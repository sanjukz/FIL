using GeoTimeZone;
using FIL.Api.Integrations;
using FIL.Api.Repositories;
using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Contracts.Commands.CitySightSeeing;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Commands;
using FIL.Contracts.Models.FeelBarcode;
using FIL.Contracts.Models.FeelBarcodeResponse;
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
    public class CreateBookingCommandHandler : BaseCommandHandlerWithResult<CreateBookingCommand, CreateBookingCommandResult>
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
        private readonly ICitySightSeeingTransactionDetailRepository _citySightSeeingTransactionDetailRepository;
        private readonly IGoogleMapApi _googleMapApi;
        private readonly ICitySightSeeingLocationRepository _citySightSeeingLocationRepository;

        public CreateBookingCommandHandler(IEventDetailRepository eventDetailRepository,
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
            Logging.ILogger logger,
            IMatchLayoutSectionRepository matchLayoutSectionRepository, ISettings settings, ICitySightSeeingTransactionDetailRepository citySightSeeingTransactionDetailRepository, IGoogleMapApi googleMapApi, ICitySightSeeingLocationRepository citySightSeeingLocationRepository,
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
            _citySightSeeingTransactionDetailRepository = citySightSeeingTransactionDetailRepository;
            _googleMapApi = googleMapApi;
            _citySightSeeingLocationRepository = citySightSeeingLocationRepository;
        }

        protected override async Task<ICommandResult> Handle(CreateBookingCommand command)
        {
            CreateBookingCommandResult results = new CreateBookingCommandResult();
            var transaction = _transactionRepository.Get(command.TransactionId);
            var transactionDetail = _transactionDetailRepository.GetByTransactionId(command.TransactionId);
            RootObject rootObject = new RootObject();

            if (transaction != null)
            {
                IDictionary<string, long> ticketCategoryValues = new Dictionary<string, long>();
                var feelBarcodemapping = _feelBarcodeMappingRepository.GetByTransactionDetailIds(transactionDetail.Select(s => s.Id)).ToList();
                if (feelBarcodemapping.Count() == 0)
                {
                    var transactionDetailModel = AutoMapper.Mapper.Map<IEnumerable<TransactionDetail>>(transactionDetail);
                    GetBarcodeResponseViewModel getBarcodeResponseViewModel = new GetBarcodeResponseViewModel();
                    FIL.Contracts.Models.FeelBarcode.Data data = new FIL.Contracts.Models.FeelBarcode.Data();
                    Address address = new Address();
                    Contact contact = new Contact();
                    BookingType bookingType = new BookingType();
                    List<FIL.Contracts.Models.FeelBarcode.BookingDetail> bookingDetails = new List<FIL.Contracts.Models.FeelBarcode.BookingDetail>();
                    foreach (var transactiondetail in transactionDetailModel)
                    {
                        var eventTicketAttributes = _eventTicketAttributeRepository.Get(transactiondetail.EventTicketAttributeId);
                        var eventTicketDetails = _eventTicketDetailRepository.Get(eventTicketAttributes.EventTicketDetailId);
                        var eventDetails = _eventDetailRepository.Get(eventTicketDetails.EventDetailId);
                        var currentTransaction = _transactionRepository.Get(transaction.Id);
                        var currentTransactionDetail = _transactionDetailRepository.Get(transactiondetail.Id);
                        var currentEventTicketAttributes = _eventTicketAttributeRepository.Get((int)currentTransactionDetail.EventTicketAttributeId);
                        var currentEventTicketDetail = _eventTicketDetailRepository.Get(currentEventTicketAttributes.EventTicketDetailId);
                        var currentTicketCategory = _ticketCategoryRepository.Get((int)currentEventTicketDetail.TicketCategoryId);
                        var currentEventDetails = _eventDetailRepository.Get(currentEventTicketDetail.EventDetailId);
                        var curretVenue = _venueRepository.Get(currentEventDetails.VenueId);
                        var currentCity = _cityRepository.Get(curretVenue.CityId);
                        var currentstate = _stateRepository.Get(currentCity.StateId);
                        var currentcountry = _countryRepository.Get(currentstate.CountryId);
                        var currentEvent = _eventRepository.Get(currentEventDetails.EventId);
                        var currentCurrencyType = _currencyTypeRepository.Get(currentEventTicketAttributes.CurrencyId);

                        if (currentEvent.EventSourceId == EventSource.CitySightSeeing)
                        {
                            ticketCategoryValues.Add(currentTicketCategory.Name.ToLower(), transactiondetail.Id);
                            var fromDate = Convert.ToDateTime(transactiondetail.VisitDate).ToUniversalTime();
                            DateTime localTime1 = fromDate;
                            DateTime endDate = DateTime.Now;
                            localTime1 = DateTime.SpecifyKind(localTime1, DateTimeKind.Local);
                            DateTimeOffset localTime2 = localTime1;
                            var mystring = localTime2.ToString();
                            var offset = mystring.Substring(mystring.Length - 6);
                            var citySightSeeingEventDetailMapping = _citySightSeeingEventDetailMappingRepository.GetByEventDetailId(currentEventDetails.Id);
                            var citySightSeeingtickets = _citySightSeeingTicketRepository.Get(citySightSeeingEventDetailMapping.CitySightSeeingTicketId);
                            FIL.Contracts.Models.FeelBarcode.BookingDetail bookingDetail = new FIL.Contracts.Models.FeelBarcode.BookingDetail();
                            var citySightSeeingTicketDetails = _citySightSeeingTicketDetailRepository.GetByTicketId(citySightSeeingtickets.TicketId);
                            var bookingDistributorReference = RandomDigits(10);
                            var citySightSeeingTransactionDetail = new CitySightSeeingTransactionDetail();
                            if (citySightSeeingTicketDetails.TicketClass != 1)
                            {
                                citySightSeeingTransactionDetail = _citySightSeeingTransactionDetailRepository.GetByTransactionId(transactiondetail.TransactionId);
                                bookingType.from_date_time = citySightSeeingTransactionDetail.FromDateTime;
                                bookingType.to_date_time = citySightSeeingTransactionDetail.EndDateTime;
                                data.reservation_reference = citySightSeeingTransactionDetail.ReservationReference;
                            }

                            var citySightSeeingLocations = _citySightSeeingLocationRepository.Get(citySightSeeingtickets.CitySightSeeingLocationId);

                            var locationDetail = await _googleMapApi.GetLatLongFromAddress(citySightSeeingLocations.Name);
                            string tz = ""; DateTime fromDateTime = DateTime.Now; ; var offsetTime = "";
                            var duration = citySightSeeingTicketDetails.Duration;
                            double lat = Convert.ToDouble(locationDetail.Result.lat);
                            double lng = Convert.ToDouble(locationDetail.Result.lng);
                            tz = TimeZoneLookup.GetTimeZone(lat, lng).Result;
                            TimeZoneInfo tzi = TZConvert.GetTimeZoneInfo(tz);
                            var visitDateTime = Convert.ToDateTime(transactiondetail.VisitDate);
                            var timeSlot = citySightSeeingTicketDetails.TicketClass == 1 ? "00:00" : citySightSeeingTransactionDetail.TimeSlot;
                            var utcOffset = tzi.BaseUtcOffset.ToString().Split(":")[0];
                            if (utcOffset.Contains("-"))
                            {
                                offset = utcOffset;
                            }
                            else
                            {
                                offset = "+" + utcOffset;
                            }
                            var formattedDateTime = visitDateTime.Year + "-" + visitDateTime.Month + "-" + visitDateTime.Day + "T" + timeSlot;
                            fromDate = Convert.ToDateTime(formattedDateTime);
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
                            var formattedEndDate = endDate.Year + "-" + endDate.Month + "-" + endDate.Day + "T" + endDate.TimeOfDay.ToString();
                            bookingType.from_date_time = formattedDateTime + offset;
                            bookingType.to_date_time = formattedEndDate + offset;
                            bookingDetail.ticket_type = (currentTicketCategory.Name).ToUpper();
                            bookingDetail.count = currentTransactionDetail.TotalTickets;
                            bookingDetail.extra_options = null;
                            bookingDetails.Add(bookingDetail);

                            bookingType.ticket_id = citySightSeeingTicketDetails.TicketId;
                            address.street = curretVenue.Name;
                            address.postal_code = "432243";
                            address.city = currentCity.Name;

                            contact.address = address;
                            contact.phonenumber = currentTransaction.PhoneNumber;
                            data.currency = currentCurrencyType.Code;
                            bookingType.booking_details = bookingDetails;
                            data.distributor_id = _settings.GetConfigSetting<string>(SettingKeys.Integration.CitySightSeeing.DistributorId);
                            data.booking_type = bookingType;
                            data.booking_name = transaction.FirstName;
                            data.booking_email = transaction.EmailId;
                            data.contact = contact;
                            data.notes = null;
                            data.product_language = "en";
                            data.distributor_reference = bookingDistributorReference;
                            rootObject.request_type = "booking";
                            rootObject.data = data;
                            var responeBooking = await GetBarcodeAsync(rootObject, transaction.Id, ticketCategoryValues);
                            if (responeBooking.data != null)
                            {
                                if (citySightSeeingTicketDetails.TicketClass != 1)
                                {
                                    citySightSeeingTransactionDetail.BookingReference = responeBooking.data.booking_reference;
                                    citySightSeeingTransactionDetail.BookingDistributorReference = responeBooking.data.distributor_reference;
                                    citySightSeeingTransactionDetail.IsOrderConfirmed = true;
                                    _citySightSeeingTransactionDetailRepository.Save(citySightSeeingTransactionDetail);
                                }
                                else
                                {
                                    _citySightSeeingTransactionDetailRepository.Save(new CitySightSeeingTransactionDetail
                                    {
                                        AltId = Guid.NewGuid(),
                                        FromDateTime = fromDate.ToString() + offset,
                                        EndDateTime = endDate.ToString() + offset,
                                        HasTimeSlot = false,
                                        BookingReference = responeBooking.data.booking_reference,
                                        BookingDistributorReference = bookingDistributorReference,
                                        TicketId = citySightSeeingtickets.TicketId,
                                        TransactionId = command.TransactionId,
                                        IsOrderConfirmed = true,
                                        ModifiedBy = command.ModifiedBy
                                    });
                                }
                                results.Success = true;
                                return results;
                            }
                            else
                            {
                                results.Success = false;
                                return results;
                            }
                        }
                    }
                }
                else
                {
                    results.IsExists = true;
                    return results;
                }
            }
            else
            {
                results.Success = false;
                return results;
            }
            return results;
        }

        public async Task<BookingResponse> GetBarcodeAsync(RootObject rootObject, long transactionid, IDictionary<string, long> ticketCategoryValues)
        {
            BookingResponse data = new BookingResponse();
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
                    string json = JsonConvert.SerializeObject(rootObject, Formatting.Indented);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(new Uri(endpoint), content);
                    responseData = await response.Content.ReadAsStringAsync();
                    data = JsonConvert.DeserializeObject<BookingResponse>(responseData);
                    var groupCode = data.data.booking_details[0].group_code;
                    var transactiondetails = _transactionDetailRepository.GetByTransactionId(transactionid);
                    if (data.data.booking_details[0].group_code != null)
                    {
                        foreach (var transactindetaildata in transactiondetails)
                        {
                            for (int i = 0; i < transactindetaildata.TotalTickets; i++)
                            {
                                _feelBarcodeMappingRepository.Save(new FeelBarcodeMapping
                                {
                                    Barcode = groupCode,
                                    TransactionDetailId = transactindetaildata.Id,
                                    IsEnabled = true,
                                    CreatedUtc = DateTime.UtcNow,
                                    CreatedBy = Guid.NewGuid(),
                                    UpdatedUtc = null,
                                    UpdatedBy = null,
                                    GroupCodeExist = true
                                });
                            }
                        }
                    }
                    else if ((data.data.booking_details[0].ticket_details.Count > 0))
                    {
                        for (int i = 0; i < data.data.booking_details[0].ticket_details.Count; i++)
                        {
                            var transDetailId = ticketCategoryValues[data.data.booking_details[0].ticket_details[i].ticket_type.ToLower()];
                            _feelBarcodeMappingRepository.Save(new FeelBarcodeMapping
                            {
                                Barcode = data.data.booking_details[0].ticket_details[i].ticket_code,
                                TransactionDetailId = transDetailId,
                                IsEnabled = true,
                                CreatedUtc = DateTime.UtcNow,
                                CreatedBy = Guid.NewGuid(),
                                UpdatedUtc = null,
                                UpdatedBy = null,
                                GroupCodeExist = false
                            });
                        }
                    }
                    return data;
                }
            }
            catch (Exception e)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, new Exception(e.Message));
                return data;
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