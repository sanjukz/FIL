using FIL.Api.Repositories;
using FIL.Api.Utilities;
using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Contracts.Commands.CitySightSeeing;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Commands;
using FIL.Contracts.Models.CitySightSeeing;
using FIL.Contracts.Utils;
using FIL.Logging;
using FIL.Logging.Enums;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.CitySightSeeing
{
    public class CitySightSeeingCommandHandler : BaseCommandHandlerWithResult<CitySightSeeingCommand, CitySightSeeingCommandResult>
    {
        private readonly ILogger _logger;
        private readonly ISettings _settings;
        private readonly IMediator _mediator;
        private readonly ICitySightSeeingLocationRepository _citySightSeeingLocationRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICityRepository _cityRepository;
        private readonly ICitySightSeeingTicketRepository _citySightSeeingTicketRepository;
        private readonly ICitySightSeeingTicketDetailRepository _citySightSeeingTicketDetailRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IEventSiteIdMappingRepository _eventSiteIdMappingRepository;
        private readonly IEventCategoryMappingRepository _eventCategoryMappingRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventDeliveryTypeDetailRepository _eventDeliveryTypeDetailRepository;
        private readonly ICitySightSeeingEventDetailMappingRepository _citySightSeeingEventDetailMappingRepository;
        private readonly ICitySightSeeingCompanyOpeningTimeRepository _citySightSeeingCompanyOpeningTimeRepository;
        private readonly ICitySightSeeingTicketTypeDetailRepository _citySightSeeingTicketTypeDetailRepository;
        private readonly ICitySightSeeingExtraOptionRepository _citySightSeeingExtraOptionRepository;
        private readonly ICitySightSeeingExtraSubOptionRepository _citySightSeeingExtraSubOptionRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly ICurrencyTypeRepository _currencyTypeRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly ITicketFeeDetailRepository _ticketFeeDetailRepository;
        private readonly ICitySightSeeingEventTicketDetailMappingRepository _citySightSeeingEventTicketDetailMappingRepository;
        private readonly ICitySightSeeingTicketDetailImageRepository _citySightSeeingTicketDetailImageRepository;
        private readonly ICitySightSeeingRouteRepository _citySightSeeingRouteRepository;
        private readonly ICitySightSeeingRouteDetailRepository _citySightSeeingRouteDetailRepository;

        public CitySightSeeingCommandHandler(ILogger logger,
            ISettings settings,
            IMediator mediator,
            ICitySightSeeingLocationRepository citySightSeeingLocationRepository,
            ICountryRepository countryRepository,
            IStateRepository stateRepository,
            ICityRepository cityRepository,
            ICitySightSeeingTicketRepository citySightSeeingTicketRepository,
            ICitySightSeeingTicketDetailRepository citySightSeeingTicketDetailRepository,
            IEventRepository eventRepository,
            IEventSiteIdMappingRepository eventSiteIdMappingRepository,
            IEventCategoryMappingRepository eventCategoryMappingRepository,
            IVenueRepository venueRepository,
            IEventDetailRepository eventDetailRepository,
            IEventDeliveryTypeDetailRepository eventDeliveryTypeDetailRepository,
            ICitySightSeeingEventDetailMappingRepository citySightSeeingEventDetailMappingRepository,
            ICitySightSeeingCompanyOpeningTimeRepository citySightSeeingCompanyOpeningTimeRepository,
            ICitySightSeeingTicketTypeDetailRepository citySightSeeingTicketTypeDetailRepository,
            ICitySightSeeingExtraOptionRepository citySightSeeingExtraOptionRepository,
            ICitySightSeeingExtraSubOptionRepository citySightSeeingExtraSubOptionRepository,
            ITicketCategoryRepository ticketCategoryRepository,
            IEventTicketDetailRepository eventTicketDetailRepository,
            IEventTicketAttributeRepository eventTicketAttributeRepository,
            ICurrencyTypeRepository currencyTypeRepository,
            ITicketFeeDetailRepository ticketFeeDetailRepository,
            ICitySightSeeingEventTicketDetailMappingRepository citySightSeeingEventTicketDetailMappingRepository, ICitySightSeeingRouteRepository citySightSeeingRouteRepository, ICitySightSeeingRouteDetailRepository citySightSeeingRouteDetailRepository,
            ICitySightSeeingTicketDetailImageRepository citySightSeeingTicketDetailImageRepository)
            : base(mediator)
        {
            _logger = logger;
            _settings = settings;
            _mediator = mediator;
            _citySightSeeingLocationRepository = citySightSeeingLocationRepository;
            _countryRepository = countryRepository;
            _stateRepository = stateRepository;
            _cityRepository = cityRepository;
            _citySightSeeingTicketRepository = citySightSeeingTicketRepository;
            _citySightSeeingTicketDetailRepository = citySightSeeingTicketDetailRepository;
            _eventRepository = eventRepository;
            _eventSiteIdMappingRepository = eventSiteIdMappingRepository;
            _eventCategoryMappingRepository = eventCategoryMappingRepository;
            _venueRepository = venueRepository;
            _eventDetailRepository = eventDetailRepository;
            _eventDeliveryTypeDetailRepository = eventDeliveryTypeDetailRepository;
            _citySightSeeingEventDetailMappingRepository = citySightSeeingEventDetailMappingRepository;
            _citySightSeeingCompanyOpeningTimeRepository = citySightSeeingCompanyOpeningTimeRepository;
            _citySightSeeingTicketTypeDetailRepository = citySightSeeingTicketTypeDetailRepository;
            _citySightSeeingExtraOptionRepository = citySightSeeingExtraOptionRepository;
            _citySightSeeingExtraSubOptionRepository = citySightSeeingExtraSubOptionRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _currencyTypeRepository = currencyTypeRepository;
            _ticketFeeDetailRepository = ticketFeeDetailRepository;
            _citySightSeeingEventTicketDetailMappingRepository = citySightSeeingEventTicketDetailMappingRepository;
            _citySightSeeingTicketDetailImageRepository = citySightSeeingTicketDetailImageRepository;
            _citySightSeeingRouteDetailRepository = citySightSeeingRouteDetailRepository;
            _citySightSeeingRouteRepository = citySightSeeingRouteRepository;
        }

        protected override async Task<ICommandResult> Handle(CitySightSeeingCommand command)
        {
            CitySightSeeingCommandResult result = new CitySightSeeingCommandResult();
            var response = await FetchAndSave(command);
            result.Success = response;
            return result;
        }

        public async Task<string> Get(object obj)
        {
            var baseAddress = new Uri(_settings.GetConfigSetting<string>(SettingKeys.Integration.CitySightSeeing.Endpoint));

            string responseData;
            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                httpClient.Timeout = new TimeSpan(1, 0, 0);
                string auth = string.Format(_settings.GetConfigSetting<string>(SettingKeys.Integration.CitySightSeeing.RequestIdentifier) + ":" + _settings.GetConfigSetting<string>(SettingKeys.Integration.CitySightSeeing.Token));

                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("x-request-authentication", _settings.GetConfigSetting<string>(SettingKeys.Integration.CitySightSeeing.RequestAuthentication));

                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("x-request-identifier", _settings.GetConfigSetting<string>(SettingKeys.Integration.CitySightSeeing.RequestIdentifier));

                var json = JsonConvert.SerializeObject(obj);

                using (var content = new StringContent(json, System.Text.Encoding.Default, "application/json"))
                {
                    using (var response = await httpClient.PostAsync("booking_service", content))
                    {
                        responseData = await response.Content.ReadAsStringAsync();
                    }
                }
            }
            return responseData;
        }

        public async Task<IEnumerable<Example>> GetResponse(string country)
        {
            HttpClient webClient = new HttpClient();
            Uri uri = new Uri("https://restcountries.eu/rest/v2/all");
            HttpResponseMessage response = await webClient.GetAsync(uri);
            var jsonString = await response.Content.ReadAsStringAsync();
            var _Data = Mapper<IList<Example>>.MapFromJson(jsonString);
            var filteredCountry = _Data.Where(s => s.name == country);
            return filteredCountry;
        }

        private async Task<bool> FetchAndSave(CitySightSeeingCommand command)
        {
            CitySightSeeingApi input = new CitySightSeeingApi();
            input.data = new RequestData();
            FIL.Contracts.Models.CitySightSeeing.Location location = new Contracts.Models.CitySightSeeing.Location();
            location.country_name = command.Location.country_name;
            location.location_name = command.Location.location_name;
            if (location != null)
            {
                var country = _countryRepository.GetByName(location.country_name);
                if (country == null)
                {
                    var getCountryCode = await GetResponse(location.country_name);
                    var CountryCodes = getCountryCode.ToList();
                    country = _countryRepository.Save(new Country
                    {
                        Name = location.country_name,
                        IsoAlphaTwoCode = (CountryCodes != null && CountryCodes.Count > 0) ? CountryCodes[0].alpha2Code : location.country_name.Substring(0, 2),
                        IsoAlphaThreeCode = (CountryCodes != null && CountryCodes.Count > 0) ? CountryCodes[0].alpha3Code : location.country_name.Substring(0, 2),
                        IsEnabled = true,
                    });
                }

                var state = _stateRepository.GetByNameAndCountryId(location.location_name, country.Id);
                if (state == null)
                {
                    state = _stateRepository.Save(new State
                    {
                        Name = location.location_name,
                        CountryId = country.Id,
                        IsEnabled = true,
                    });
                }

                var city = _cityRepository.GetByNameAndStateId(location.location_name, state.Id);
                if (city == null)
                {
                    city = _cityRepository.Save(new City
                    {
                        Name = location.location_name,
                        StateId = state.Id,
                        IsEnabled = true,
                    });
                }

                var citySightSeeingLocation = _citySightSeeingLocationRepository.GetByName(location.location_name);
                if (citySightSeeingLocation == null)
                {
                    citySightSeeingLocation = _citySightSeeingLocationRepository.Save(new CitySightSeeingLocation
                    {
                        AltId = Guid.NewGuid(),
                        Name = location.location_name,
                        CountryName = location.country_name,
                        ModifiedBy = command.ModifiedBy,
                        CreatedUtc = DateTime.UtcNow,
                        IsEnabled = true
                    });
                }
                else
                {
                    citySightSeeingLocation.IsEnabled = true;
                    _citySightSeeingLocationRepository.Save(citySightSeeingLocation);
                }

                input = new CitySightSeeingApi();
                input.data = new RequestData();
                input.request_type = "list";
                input.data.location_name = location.location_name;
                input.data.distributor_id = _settings.GetConfigSetting<string>(SettingKeys.Integration.CitySightSeeing.DistributorId);
                TicketList objTicketList = new TicketList();
                objTicketList = Mapper<TicketList>.MapFromJson((await Get(input)));

                if (objTicketList != null && objTicketList.data != null && objTicketList.data.tickets.Count > 0)
                {
                    //foreach ( Ticket ticket in objTicketList.data.tickets)
                    for (var k = 0; k < objTicketList.data.tickets.Count; k++)
                    {
                        var ticket = objTicketList.data.tickets[k];
                        try
                        {
                            var citySightSeeingTicket = _citySightSeeingTicketRepository.GetByTicketId(ticket.ticket_id);
                            if (citySightSeeingTicket == null)
                            {
                                citySightSeeingTicket = _citySightSeeingTicketRepository.Save(new CitySightSeeingTicket
                                {
                                    AltId = Guid.NewGuid(),
                                    TicketId = ticket.ticket_id,
                                    Title = ticket.ticket_title,
                                    VenueName = ticket.venue_name,
                                    Language = ticket.txt_language,
                                    StartDate = ticket.start_date,
                                    EndDate = ticket.end_date,
                                    Currency = ticket.currency,
                                    CitySightSeeingLocationId = citySightSeeingLocation.Id,
                                    ModifiedBy = command.ModifiedBy,
                                    CreatedUtc = DateTime.UtcNow,
                                    IsEnabled = true
                                });
                            }
                            else
                            {
                                citySightSeeingTicket.CitySightSeeingLocationId = citySightSeeingLocation.Id;
                                citySightSeeingTicket.TicketId = ticket.ticket_id;
                                citySightSeeingTicket.Title = ticket.ticket_title;
                                citySightSeeingTicket.VenueName = ticket.venue_name;
                                citySightSeeingTicket.StartDate = ticket.start_date;
                                citySightSeeingTicket.EndDate = ticket.end_date;
                                citySightSeeingTicket.Currency = ticket.currency;
                                citySightSeeingTicket.CitySightSeeingLocationId = citySightSeeingLocation.Id;
                                citySightSeeingTicket.IsEnabled = true;
                                _citySightSeeingTicketRepository.Save(citySightSeeingTicket);
                            }

                            input = new CitySightSeeingApi();
                            input.data = new RequestData();

                            input.request_type = "details";
                            input.data.distributor_id = _settings.GetConfigSetting<string>(SettingKeys.Integration.CitySightSeeing.DistributorId);
                            input.data.ticket_id = ticket.ticket_id;
                            input.data.txt_language = "en";
                            TicketDetails objTicketDetails = new TicketDetails();
                            objTicketDetails = Mapper<TicketDetails>.MapFromJson((await Get(input)));

                            if (objTicketDetails.data != null)
                            {
                                var citySightSeeingTicketDetail = _citySightSeeingTicketDetailRepository.GetByTicketId(ticket.ticket_id);
                                if (citySightSeeingTicketDetail == null)
                                {
                                    citySightSeeingTicketDetail = _citySightSeeingTicketDetailRepository.Save(new Contracts.DataModels.CitySightSeeingTicketDetail
                                    {
                                        AltId = Guid.NewGuid(),
                                        TicketId = objTicketDetails.data.ticket_id,
                                        Title = objTicketDetails.data.ticket_title,
                                        ShortDescription = objTicketDetails.data.short_description,
                                        Description = objTicketDetails.data.long_description,
                                        Duration = objTicketDetails.data.duration,
                                        ProductLanguage = objTicketDetails.data.product_language,
                                        TxtLanguage = objTicketDetails.data.txt_language,
                                        TicketEntryNotes = objTicketDetails.data.ticket_entry_notes,
                                        BookSizeMin = objTicketDetails.data.book_size_min,
                                        BookSizeMax = objTicketDetails.data.book_size_max,
                                        SupplierUrl = objTicketDetails.data.supplier_url,
                                        TicketClass = objTicketDetails.data.ticket_class,
                                        StartDate = objTicketDetails.data.start_date,
                                        EndDate = objTicketDetails.data.end_date,
                                        BookingStartDate = objTicketDetails.data.booking_start_date,
                                        Currency = objTicketDetails.data.currency,
                                        PickupPoints = objTicketDetails.data.pickup_points,
                                        CombiTicket = objTicketDetails.data.combi_ticket,
                                        CitySightSeeingTicketId = citySightSeeingTicket.Id,
                                        ModifiedBy = command.ModifiedBy,
                                        CreatedUtc = DateTime.UtcNow,
                                        IsEnabled = true
                                    });
                                }
                                else
                                {
                                    citySightSeeingTicketDetail.IsEnabled = true;
                                    citySightSeeingTicketDetail.TicketId = objTicketDetails.data.ticket_id;
                                    citySightSeeingTicketDetail.StartDate = objTicketDetails.data.start_date;
                                    citySightSeeingTicketDetail.EndDate = objTicketDetails.data.end_date;
                                    _citySightSeeingTicketDetailRepository.Save(citySightSeeingTicketDetail);
                                }

                                //Event, Venue & EventDetails, EventSiteIdMapping, c, EventDeliveryTypes
                                var events = _eventRepository.GetByEventName(objTicketDetails.data.ticket_title);
                                var formattedSlug = FormatSlug(objTicketDetails.data.ticket_title);
                                if (events == null)
                                {
                                    events = _eventRepository.Save(new Contracts.DataModels.Event
                                    {
                                        AltId = Guid.NewGuid(),
                                        Name = objTicketDetails.data.ticket_title,
                                        EventCategoryId = Constant.HohoConstant.EventCategoryId,
                                        EventTypeId = EventType.Perennial,
                                        Description = objTicketDetails.data.long_description,
                                        ClientPointOfContactId = 1,
                                        FbEventId = null,
                                        MetaDetails = null,
                                        IsFeel = true,
                                        EventSourceId = EventSource.CitySightSeeing,
                                        TermsAndConditions = "",
                                        IsPublishedOnSite = true,
                                        PublishedDateTime = DateTime.Now,
                                        PublishedBy = null,
                                        TestedBy = null,
                                        Slug = formattedSlug,
                                        ModifiedBy = command.ModifiedBy,
                                        IsEnabled = true
                                    });
                                }
                                else
                                {
                                    events.Slug = formattedSlug;
                                    events.IsEnabled = true;
                                    events.EventSourceId = EventSource.CitySightSeeing;
                                    _eventRepository.Save(events);
                                }
                                var last = _eventSiteIdMappingRepository.GetAll().OrderByDescending(p => p.CreatedUtc).FirstOrDefault();
                                var eventSiteIdMapping = _eventSiteIdMappingRepository.GetByEventId(events.Id);
                                if (eventSiteIdMapping == null)
                                {
                                    _eventSiteIdMappingRepository.Save(new Contracts.DataModels.EventSiteIdMapping
                                    {
                                        EventId = events.Id,
                                        SortOrder = Convert.ToInt16(last.SortOrder + 1),
                                        SiteId = Site.feelaplaceSite,
                                        ModifiedBy = command.ModifiedBy,
                                        IsEnabled = true
                                    });
                                }
                                else
                                {
                                    eventSiteIdMapping.IsEnabled = true;
                                    eventSiteIdMapping.SortOrder = Convert.ToInt16(last.SortOrder + 1);
                                    _eventSiteIdMappingRepository.Save(eventSiteIdMapping);
                                }

                                var eventCategoryMapping = _eventCategoryMappingRepository.GetByEventId(events.Id);
                                if (eventCategoryMapping.Count() == 0)
                                {
                                    _eventCategoryMappingRepository.Save(new Contracts.DataModels.EventCategoryMapping
                                    {
                                        EventId = events.Id,
                                        EventCategoryId = Constant.HohoConstant.EventCategoryId,
                                        ModifiedBy = command.ModifiedBy,
                                        IsEnabled = true
                                    });
                                }

                                var venue = _venueRepository.GetByNameAndCityId(ticket.venue_name, city.Id);
                                if (venue == null)
                                {
                                    venue = _venueRepository.Save(new Venue
                                    {
                                        AltId = Guid.NewGuid(),
                                        Name = ticket.venue_name,
                                        AddressLineOne = ticket.venue_name,
                                        CityId = city.Id,
                                        ModifiedBy = command.ModifiedBy,
                                        IsEnabled = true
                                    });
                                }

                                var eventDetail = _eventDetailRepository.GetByNameAndVenueId(objTicketDetails.data.ticket_title, venue.Id);
                                DateTime startDate, endDate;
                                if (eventDetail == null)
                                {
                                    eventDetail = _eventDetailRepository.Save(new EventDetail
                                    {
                                        Name = objTicketDetails.data.ticket_title,
                                        EventId = events.Id,
                                        VenueId = venue.Id,
                                        StartDateTime = DateTime.TryParse(citySightSeeingTicketDetail.StartDate, out startDate) ? startDate.ToUniversalTime() : DateTime.UtcNow,
                                        EndDateTime = DateTime.TryParse(citySightSeeingTicketDetail.EndDate, out endDate) ? endDate.ToUniversalTime() : DateTime.UtcNow.AddYears(1),
                                        GroupId = 1,
                                        AltId = Guid.NewGuid(),
                                        TicketLimit = 10,
                                        ModifiedBy = command.ModifiedBy,
                                        IsEnabled = true,
                                        MetaDetails = "",
                                        HideEventDateTime = false,
                                        CustomDateTimeMessage = "",
                                    });
                                }
                                else
                                {
                                    eventDetail.IsEnabled = true;
                                    eventDetail.StartDateTime = DateTime.TryParse(citySightSeeingTicketDetail.StartDate, out startDate) ? startDate.ToUniversalTime() : DateTime.UtcNow;
                                    eventDetail.EndDateTime = DateTime.TryParse(citySightSeeingTicketDetail.EndDate, out endDate) ? endDate.ToUniversalTime() : DateTime.UtcNow.AddYears(1);
                                    _eventDetailRepository.Save(eventDetail);
                                }

                                var eventDeliveryType = _eventDeliveryTypeDetailRepository.GetByEventDetailId(eventDetail.Id);
                                if (eventDeliveryType == null)
                                {
                                    _eventDeliveryTypeDetailRepository.Save(new EventDeliveryTypeDetail
                                    {
                                        EventDetailId = eventDetail.Id,
                                        DeliveryTypeId = DeliveryTypes.MTicket,
                                        Notes = "<table><tr><td valign=''top''>1.&nbsp;</td><td valign=''top''>Ticket pickup location and timing will be announced in the “Customer Update” sectionof our website closer to the event. Please check that regularly.</td></tr><tr><td valign=''top''>2.&nbsp;</td><td valign=''top''>The following documents are compulsory for ticket pickup:<br /><table><tr>  <td valign=''top''>  a.&nbsp;  </td>  <td colspan=''2'' valign=''top''>  The card / bank account owner’s original Govt. issued photo ID, along with a clean,  fully legible, photocopy of the same ID  </td></tr><tr>  <td valign=''top''>  b.&nbsp;  </td>  <td colspan=''2'' valign=''top''>  When a debit or credit card has been used for purchase, we also need the original  debit/credit card, along with a clean, fully legible, photocopy of the same card  </td></tr><tr>  <td valign=''top''>  c.&nbsp;  </td>  <td colspan=''2'' valign=''top''>  If sending someone else on behalf of the card holder / bank account owner, then  we need numbers 2.a. and 2.b above (originals and photocopies as mentioned) along  with the following below. This is required even if the representative’s name has  been entered into the system when buying:  </td></tr><tr>  <td valign=''top''>  </td>  <td valign=''top''>  i.&nbsp;  </td>  <td>  An authorization letter with the name of the representative, signed by the card  holder/bank account owner  </td></tr><tr>  <td valign=''top''>  </td>  <td valign=''top''>  ii.&nbsp;  </td>  <td>  A Govt issued photo ID of the representative, along with a clean and legible photocopy  of the same photo identification  </td></tr></table></td></tr><tr><td valign=''top''>3.&nbsp;</td><td valign=''top''>Please note, absence of any one of the documents above can result in the tickets being refused at the ticket pickup window</td></tr>  </table>",
                                        EndDate = DateTime.TryParse(citySightSeeingTicketDetail.EndDate, out endDate) ? endDate.ToUniversalTime() : DateTime.UtcNow.AddYears(1),
                                        ModifiedBy = command.ModifiedBy,
                                        IsEnabled = true
                                    });
                                }
                                if (objTicketDetails.data.routes != null && objTicketDetails.data.routes.Count > 0)                    //For Route Details
                                {
                                    foreach (var routeData in objTicketDetails.data.routes)
                                    {
                                        var citySightSeeingRoute = _citySightSeeingRouteRepository.GetByEventDetailIdAndRouteId(eventDetail.Id, routeData.route_id);
                                        var audioLanguages = "";
                                        if (routeData.route_audio_languages != null && routeData.route_audio_languages.Count > 0)
                                        {
                                            for (int i = 0; i < routeData.route_audio_languages.Count; i++)
                                            {
                                                audioLanguages += routeData.route_audio_languages[i];
                                                if (i < routeData.route_audio_languages.Count - 1)
                                                {
                                                    audioLanguages += ",";
                                                }
                                            }
                                        }
                                        var LiveLanguages = "";
                                        if (routeData.route_live_languages != null && routeData.route_live_languages.Count > 0)
                                        {
                                            for (int i = 0; i < routeData.route_live_languages.Count; i++)
                                            {
                                                LiveLanguages += routeData.route_live_languages[i];
                                                if (i < routeData.route_live_languages.Count - 1)
                                                {
                                                    LiveLanguages += ",";
                                                }
                                            }
                                        }
                                        if (citySightSeeingRoute == null)
                                        {
                                            citySightSeeingRoute = _citySightSeeingRouteRepository.Save(new Contracts.DataModels.CitySightSeeingRoute
                                            {
                                                EventDetailId = eventDetail.Id,
                                                RouteId = routeData.route_id,
                                                RouteName = routeData.route_name,
                                                RouteColor = routeData.route_color,
                                                RouteDuration = routeData.route_duration,
                                                RouteType = routeData.route_type,
                                                RouteStartTime = routeData.route_start_time,
                                                RouteEndTime = routeData.route_end_time,
                                                RouteFrequency = routeData.route_frequency,
                                                RouteLiveLanguages = LiveLanguages,
                                                RouteAudioLanguages = audioLanguages,
                                                ModifiedBy = command.ModifiedBy,
                                                IsEnabled = true
                                            });
                                        }
                                        else
                                        {
                                            citySightSeeingRoute.RouteName = routeData.route_name; citySightSeeingRoute.RouteColor = routeData.route_color; citySightSeeingRoute.RouteDuration = routeData.route_duration;
                                            citySightSeeingRoute.RouteType = routeData.route_type;
                                            citySightSeeingRoute.RouteStartTime = routeData.route_start_time;
                                            citySightSeeingRoute.RouteEndTime = routeData.route_end_time;
                                            citySightSeeingRoute.RouteFrequency = routeData.route_frequency;
                                            citySightSeeingRoute.RouteLiveLanguages = LiveLanguages;
                                            citySightSeeingRoute.RouteAudioLanguages = audioLanguages;
                                            citySightSeeingRoute.IsEnabled = true;
                                            _citySightSeeingRouteRepository.Save(citySightSeeingRoute);
                                        }
                                        foreach (var routeDetail in routeData.route_locations)
                                        {
                                            var citySightSeeingRouteDetail = _citySightSeeingRouteDetailRepository.GetByCitySightSeeingRouteIdAndLocationId(citySightSeeingRoute.Id, routeDetail.route_location_id);
                                            if (citySightSeeingRouteDetail == null)
                                            {
                                                citySightSeeingRouteDetail = _citySightSeeingRouteDetailRepository.Save(new Contracts.DataModels.CitySightSeeingRouteDetail
                                                {
                                                    CitySightSeeingRouteId = citySightSeeingRoute
                                                .Id,
                                                    RouteLocationId = routeDetail.route_location_id,
                                                    RouteLocationName = routeDetail.route_location_name,
                                                    RouteLocationDescription = routeDetail.route_location_description,
                                                    RouteLocationLatitude = routeDetail.route_location_latitude,
                                                    RouteLocationLongitude = routeDetail.route_location_longitude,
                                                    RouteLocationStopover = routeDetail.route_location_stopover,
                                                    IsEnabled = true,
                                                    ModifiedBy = command.ModifiedBy
                                                });
                                            }
                                        }
                                    }
                                }
                                var citySightSeeingEventDetailMapping = _citySightSeeingEventDetailMappingRepository.GetByEventDetailId(eventDetail.Id);
                                if (citySightSeeingEventDetailMapping == null)
                                {
                                    _citySightSeeingEventDetailMappingRepository.Save(new CitySightSeeingEventDetailMapping
                                    {
                                        CitySightSeeingTicketId = citySightSeeingTicket.Id,
                                        EventDetailId = eventDetail.Id,
                                        ModifiedBy = command.ModifiedBy,
                                        IsEnabled = true
                                    });
                                }
                                else
                                {
                                    citySightSeeingEventDetailMapping.CitySightSeeingTicketId = citySightSeeingTicket.Id;
                                    citySightSeeingEventDetailMapping.EventDetailId = eventDetail.Id;
                                    citySightSeeingEventDetailMapping.IsEnabled = true;
                                    _citySightSeeingEventDetailMappingRepository.Save(citySightSeeingEventDetailMapping);
                                }

                                if (objTicketDetails.data.ticket_type_details != null)
                                {
                                    DateTime CurrentDate = DateTime.UtcNow.Date;
                                    // to get season dates and price
                                    var tickettypeData = objTicketDetails.data.ticket_type_details.Where(w => Convert.ToDateTime(w.start_date).Date <= CurrentDate && Convert.ToDateTime(w.end_date).Date >= CurrentDate);

                                    // Initially disabling all Ticket Type
                                    var allCitySightSeeingTicketTypeDetail = _citySightSeeingTicketTypeDetailRepository.GetAllByTicketId(ticket.ticket_id);
                                    var citySightSeeingModel = AutoMapper.Mapper.Map<IEnumerable<CitySightSeeingTicketTypeDetail>>(allCitySightSeeingTicketTypeDetail);
                                    foreach (var citySightSeeingTypeDetail in citySightSeeingModel)
                                    {
                                        var tempCitySightSeeingTypeDetail = _citySightSeeingTicketTypeDetailRepository.Get(citySightSeeingTypeDetail.Id);
                                        tempCitySightSeeingTypeDetail.IsEnabled = false;
                                        _citySightSeeingTicketTypeDetailRepository.Save(tempCitySightSeeingTypeDetail);
                                    }
                                    //if ticket categories returened empty disable the event
                                    if (tickettypeData.Count() == 0)
                                    {
                                        eventDetail.IsEnabled = false;
                                        _eventDetailRepository.Save(eventDetail);
                                        events.IsEnabled = false;
                                        _eventRepository.Save(events);
                                    }

                                    foreach (TicketTypeDetail ticketTypeDetail in tickettypeData)
                                    {
                                        var fomattedTicketType = ticketTypeDetail.ticket_type + " (" + ticketTypeDetail.age_from + "-" + ticketTypeDetail.age_to + ")";
                                        var citySightSeeingTicketTypeDetail = _citySightSeeingTicketTypeDetailRepository.GetByTicketId(ticket.ticket_id, fomattedTicketType);
                                        if (citySightSeeingTicketTypeDetail == null)
                                        {
                                            citySightSeeingTicketTypeDetail = _citySightSeeingTicketTypeDetailRepository.Save(new CitySightSeeingTicketTypeDetail
                                            {
                                                AltId = Guid.NewGuid(),
                                                TicketId = ticket.ticket_id,
                                                TicketType = fomattedTicketType,
                                                StartDate = ticketTypeDetail.start_date,
                                                EndDate = ticketTypeDetail.end_date,
                                                AgeFrom = ticketTypeDetail.age_from,
                                                AgeTo = ticketTypeDetail.age_to,
                                                UnitPrice = ticketTypeDetail.unit_price,
                                                UnitListPrice = ticketTypeDetail.unit_list_price,
                                                UnitDiscount = ticketTypeDetail.unit_discount,
                                                UnitGrossPrice = ticketTypeDetail.unit_gross_price,
                                                CitySightSeeingTicketId = citySightSeeingTicket.Id,
                                                ModifiedBy = command.ModifiedBy,
                                                CreatedUtc = DateTime.UtcNow,
                                                IsEnabled = true
                                            });
                                        }
                                        else
                                        {
                                            if (citySightSeeingTicketTypeDetail.UnitPrice != ticketTypeDetail.unit_price
                                                || citySightSeeingTicketTypeDetail.UnitListPrice != ticketTypeDetail.unit_list_price
                                                || citySightSeeingTicketTypeDetail.UnitGrossPrice != ticketTypeDetail.unit_gross_price
                                                || citySightSeeingTicketTypeDetail.UnitDiscount != ticketTypeDetail.unit_discount)
                                            {
                                                citySightSeeingTicketTypeDetail.UnitPrice = ticketTypeDetail.unit_price;
                                                citySightSeeingTicketTypeDetail.UnitListPrice = ticketTypeDetail.unit_list_price;
                                                citySightSeeingTicketTypeDetail.UnitDiscount = ticketTypeDetail.unit_discount;
                                                citySightSeeingTicketTypeDetail.UnitGrossPrice = ticketTypeDetail.unit_gross_price;
                                                citySightSeeingTicketTypeDetail.StartDate = ticketTypeDetail.start_date;
                                                citySightSeeingTicketTypeDetail.EndDate = ticketTypeDetail.end_date;
                                            }
                                            else
                                            {
                                                citySightSeeingTicketTypeDetail.StartDate = ticketTypeDetail.start_date;
                                                citySightSeeingTicketTypeDetail.EndDate = ticketTypeDetail.end_date;
                                            }
                                            citySightSeeingTicketTypeDetail.IsEnabled = true;
                                            citySightSeeingTicketTypeDetail.ModifiedBy = command.ModifiedBy;
                                            _citySightSeeingTicketTypeDetailRepository.Save(citySightSeeingTicketTypeDetail);
                                        }

                                        if (ticketTypeDetail.extra_options != null)
                                        {
                                            var citySightSeeingExtraOption = _citySightSeeingExtraOptionRepository.GetByTicketTypeDetailId(citySightSeeingTicketTypeDetail.Id);
                                            if (citySightSeeingExtraOption == null)
                                            {
                                                foreach (ExtraOption extraOption in ticketTypeDetail.extra_options)
                                                {
                                                    citySightSeeingExtraOption = _citySightSeeingExtraOptionRepository.Save(new CitySightSeeingExtraOption
                                                    {
                                                        AltId = Guid.NewGuid(),
                                                        ExtraOptionId = extraOption.extra_option_id,
                                                        ExtraOptionName = extraOption.extra_option_name,
                                                        ExtraOptionType = extraOption.extra_option_type,
                                                        IsMandatory = extraOption.is_mandatory,
                                                        CitySightSeeingTicketTypeDetailId = citySightSeeingTicketTypeDetail.Id,
                                                        ModifiedBy = command.ModifiedBy,
                                                        CreatedUtc = DateTime.UtcNow,
                                                        IsEnabled = true
                                                    });

                                                    if (extraOption.options != null)
                                                    {
                                                        var citySightSeeingExtraSubOption = _citySightSeeingExtraSubOptionRepository.GetByExtraOptionId(citySightSeeingExtraOption.Id);
                                                        if (citySightSeeingExtraSubOption == null)
                                                        {
                                                            foreach (SubOption subOption in extraOption.options)
                                                            {
                                                                citySightSeeingExtraSubOption = _citySightSeeingExtraSubOptionRepository.Save(new CitySightSeeingExtraSubOption
                                                                {
                                                                    AltId = Guid.NewGuid(),
                                                                    SubOptionId = subOption.option_id,
                                                                    SubOptionName = subOption.option_name,
                                                                    SubOptionPrice = subOption.option_price,
                                                                    CitySightSeeingExtraOptionId = citySightSeeingExtraOption.Id,
                                                                    ModifiedBy = command.ModifiedBy,
                                                                    CreatedUtc = DateTime.UtcNow,
                                                                    IsEnabled = true
                                                                });
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        //Ticket Categories, Event Ticket Details & Event Ticket Attribute, Ticket Fee Details

                                        var ticketCategory = _ticketCategoryRepository.GetByName(fomattedTicketType);
                                        if (ticketCategory == null)
                                        {
                                            ticketCategory = _ticketCategoryRepository.Save(new TicketCategory
                                            {
                                                Name = fomattedTicketType,
                                                ModifiedBy = command.ModifiedBy,
                                                IsEnabled = true
                                            });
                                        }

                                        var eventTicketDetail = _eventTicketDetailRepository.GetByTicketCategoryIdAndEventDetailId(ticketCategory.Id, eventDetail.Id);
                                        if (eventTicketDetail == null)
                                        {
                                            eventTicketDetail = _eventTicketDetailRepository.Save(new EventTicketDetail
                                            {
                                                EventDetailId = eventDetail.Id,
                                                TicketCategoryId = ticketCategory.Id,
                                                ModifiedBy = command.ModifiedBy,
                                                IsEnabled = true
                                            });
                                        }

                                        var citySightSeeingEventTicketDetailMapping = _citySightSeeingEventTicketDetailMappingRepository.GetByCitySightSeeingTicketTypeDetailId(citySightSeeingTicketTypeDetail.Id);
                                        if (citySightSeeingEventTicketDetailMapping == null)
                                        {
                                            citySightSeeingEventTicketDetailMapping = _citySightSeeingEventTicketDetailMappingRepository.Save(new CitySightSeeingEventTicketDetailMapping
                                            {
                                                CitySightSeeingTicketTypeDetailId = citySightSeeingTicketTypeDetail.Id,
                                                EventTicketDetailId = eventTicketDetail.Id,
                                                ModifiedBy = command.ModifiedBy,
                                                IsEnabled = true
                                            });
                                        }

                                        var currencyType = _currencyTypeRepository.GetByCurrencyCode(ticket.currency.ToUpper());
                                        if (currencyType == null)
                                        {
                                            currencyType = _currencyTypeRepository.Save(new CurrencyType
                                            {
                                                Code = ticket.currency.ToUpper(),
                                                Name = ticket.currency.ToUpper(),
                                                CountryId = country.Id,
                                                ModifiedBy = command.ModifiedBy,
                                                IsEnabled = true
                                            });
                                        }

                                        var eventTicketAttribute = _eventTicketAttributeRepository.GetByEventTicketDetailId(eventTicketDetail.Id);
                                        if (eventTicketAttribute == null)
                                        {
                                            eventTicketAttribute = _eventTicketAttributeRepository.Save(new EventTicketAttribute
                                            {
                                                EventTicketDetailId = eventTicketDetail.Id,
                                                SalesStartDateTime = Convert.ToDateTime(citySightSeeingTicketTypeDetail.StartDate),
                                                SalesEndDatetime = Convert.ToDateTime(citySightSeeingTicketTypeDetail.EndDate),
                                                TicketTypeId = TicketType.Regular,
                                                ChannelId = Channels.Feel,
                                                CurrencyId = currencyType.Id,
                                                SharedInventoryGroupId = null,
                                                TicketCategoryDescription = "",
                                                ViewFromStand = "",
                                                IsSeatSelection = false,
                                                AvailableTicketForSale = 1000,
                                                RemainingTicketForSale = 1000,
                                                Price = Convert.ToDecimal(ticketTypeDetail.unit_gross_price),
                                                IsInternationalCardAllowed = false,
                                                IsEMIApplicable = false,
                                                ModifiedBy = command.ModifiedBy,
                                                IsEnabled = true
                                            });
                                        }
                                        else
                                        {
                                            if (eventTicketDetail.Id == citySightSeeingEventTicketDetailMapping.EventTicketDetailId)
                                            {
                                                eventTicketAttribute = _eventTicketAttributeRepository.GetByEventTicketDetailId(citySightSeeingEventTicketDetailMapping.EventTicketDetailId);
                                                if (eventTicketAttribute != null)
                                                {
                                                    if (eventTicketAttribute.Price != Convert.ToDecimal(ticketTypeDetail.unit_gross_price))
                                                    {
                                                        eventTicketAttribute.Id = eventTicketAttribute.Id;
                                                        eventTicketAttribute.Price = Convert.ToDecimal(ticketTypeDetail.unit_gross_price);
                                                        eventTicketAttribute.ModifiedBy = command.ModifiedBy;
                                                        eventTicketAttribute.SalesStartDateTime = Convert.ToDateTime(ticketTypeDetail.start_date);
                                                        eventTicketAttribute.SalesEndDatetime = Convert.ToDateTime(ticketTypeDetail.end_date);
                                                        _eventTicketAttributeRepository.Save(eventTicketAttribute);
                                                    }
                                                }
                                            }
                                        }

                                        var ticketFeeDetail = _ticketFeeDetailRepository.GetByEventTicketAttributeIdAndFeedId(eventTicketAttribute.Id, (int)FeeType.ConvenienceCharge);
                                        if (ticketFeeDetail == null)
                                        {
                                            ticketFeeDetail = _ticketFeeDetailRepository.Save(new TicketFeeDetail
                                            {
                                                EventTicketAttributeId = eventTicketAttribute.Id,
                                                FeeId = (int)FeeType.ConvenienceCharge,
                                                DisplayName = "Convienence Charge",
                                                ValueTypeId = (int)ValueTypes.Percentage,
                                                Value = 5,
                                                ModifiedBy = command.ModifiedBy,
                                                IsEnabled = true
                                            });
                                        }
                                    }

                                    // disabling the eventticketdetails
                                    var disabledCitySightSeeingTicketTypeDetails = _citySightSeeingTicketTypeDetailRepository.GetAllDisabledByTicketId(ticket.ticket_id);
                                    var disabledCitySightSeeingTicketTypeDetailsModel = AutoMapper.Mapper.Map<IEnumerable<CitySightSeeingTicketTypeDetail>>(disabledCitySightSeeingTicketTypeDetails);
                                    foreach (var disabledCitySightSeeingTypeDetail in disabledCitySightSeeingTicketTypeDetails)
                                    {
                                        var tempCitySightSeeingEventTicktDetailMapping = _citySightSeeingEventTicketDetailMappingRepository.GetByCitySightSeeingTicketTypeDetailId(disabledCitySightSeeingTypeDetail.Id);
                                        var disabledEventTicketDetail = _eventTicketDetailRepository.Get(tempCitySightSeeingEventTicktDetailMapping.EventTicketDetailId);
                                        disabledEventTicketDetail.IsEnabled = false;
                                        _eventTicketDetailRepository.Save(disabledEventTicketDetail);
                                        var disabledEventTktAttribites = _eventTicketAttributeRepository.GetByEventTicketDetailId(disabledEventTicketDetail.Id);
                                        disabledEventTktAttribites.IsEnabled = false;
                                        _eventTicketAttributeRepository.Save(disabledEventTktAttribites);
                                    }
                                }

                                if (objTicketDetails.data.company_opening_times != null && objTicketDetails.data.company_opening_times.Count > 0)
                                {
                                    var citySightSeeingCompanyOpeningTime = _citySightSeeingCompanyOpeningTimeRepository.GetByTicketId(ticket.ticket_id);
                                    if (citySightSeeingCompanyOpeningTime == null)
                                    {
                                        foreach (CompanyOpeningTime companyOpeningTime in objTicketDetails.data.company_opening_times)
                                        {
                                            citySightSeeingCompanyOpeningTime = new CitySightSeeingCompanyOpeningTime
                                            {
                                                AltId = Guid.NewGuid(),
                                                TicketId = ticket.ticket_id,
                                                Day = companyOpeningTime.day,
                                                StartFrom = companyOpeningTime.start_from,
                                                EndTo = companyOpeningTime.end_to,
                                                CitySightSeeingTicketId = citySightSeeingTicket.Id,
                                                ModifiedBy = command.ModifiedBy,
                                                CreatedUtc = DateTime.UtcNow,
                                                IsEnabled = true
                                            };

                                            citySightSeeingCompanyOpeningTime = _citySightSeeingCompanyOpeningTimeRepository.Save(citySightSeeingCompanyOpeningTime);
                                        }
                                    }
                                }

                                if (objTicketDetails.data.images != null && objTicketDetails.data.images.Count > 0)
                                {
                                    var citySightSeeingTicketDetailImage = _citySightSeeingTicketDetailImageRepository.GetByTicketId(ticket.ticket_id);
                                    if (citySightSeeingTicketDetailImage == null)
                                    {
                                        foreach (string image in objTicketDetails.data.images)
                                        {
                                            citySightSeeingTicketDetailImage = new CitySightSeeingTicketDetailImage
                                            {
                                                AltId = Guid.NewGuid(),
                                                TicketId = ticket.ticket_id,
                                                Image = image,
                                                CitySightSeeingTicketId = citySightSeeingTicket.Id,
                                                ModifiedBy = command.ModifiedBy,
                                                CreatedUtc = DateTime.UtcNow,
                                                IsEnabled = true
                                            };

                                            citySightSeeingTicketDetailImage = _citySightSeeingTicketDetailImageRepository.Save(citySightSeeingTicketDetailImage);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.Log(LogCategory.Error, new Exception("Failed to Sync HOHO Data", ex));
                            continue;
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public string FormatSlug(string tittle)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            bool flag = false;
            foreach (char c in tittle)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
                {
                    sb.Append(c);
                    flag = true;
                }
                else
                {
                    if (flag)
                    {
                        sb.Append("-");
                        flag = false;
                    }
                }
            }
            return sb.ToString();
        }
    }
}