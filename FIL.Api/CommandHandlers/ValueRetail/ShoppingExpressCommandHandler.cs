using FIL.Api.Integrations;
using FIL.Api.Integrations.ValueRetail;
using FIL.Api.Repositories;
using FIL.Api.Utilities;
using FIL.Configuration;
using FIL.Contracts.Commands.ValueRetail;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using FIL.Contracts.Models.Integrations.ValueRetail;
using FIL.Contracts.Models.ValueRetail;
using FIL.Logging;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.ValueRetail
{
    public class ShoppingExpressCommandHandler : BaseCommandHandler<ShoppingExpressCommand>
    {
        private readonly ILogger _logger;
        private readonly ISettings _settings;
        private readonly IMediator _mediator;
        private readonly IValueRetailAuth _valueRetailAuth;
        private readonly IValueRetailVillageRepository _valueRetailVillageRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IDaysRepository _daysRepository;
        private readonly IPlaceWeekOpenDaysRepository _placeWeekOpenDaysRepository;
        private readonly IEventSiteIdMappingRepository _eventSiteIdMappingRepository;
        private readonly IEventCategoryMappingRepository _eventCategoryMappingRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventDeliveryTypeDetailRepository _eventDeliveryTypeDetailRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly ICurrencyTypeRepository _currencyTypeRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly ITicketFeeDetailRepository _ticketFeeDetailRepository;
        private readonly IPlaceHolidayDatesRepository _placeHolidayDatesRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IValueRetailAPI _valueRetailAPI;
        private readonly IGoogleMapApi _googleMapApi;
        private readonly IEventVenueMappingTimeRepository _eventVenueMappingTimeRepository;
        private readonly IEventVenueMappingRepository _eventVenueMappingRepository;
        private readonly IValueRetailRouteRepository _valueRetailRouteRepository;
        private readonly IValueRetailReturnStopRepository _valueRetailReturnStopRepository;
        private readonly ICountryAlphaCode _countryAlphaCode;
        private readonly IToEnglishTranslator _toEnglishTranslator;
        private readonly Guid tempAltId = Guid.NewGuid();

        public ShoppingExpressCommandHandler(
            ILogger logger,
            ISettings settings,
            IMediator mediator,
            IValueRetailAuth valueRetailAuth,
            IValueRetailVillageRepository valueRetailVillageRepository,
            IEventRepository eventRepository,
            IDaysRepository daysRepository,
            IPlaceWeekOpenDaysRepository placeWeekOpenDaysRepository,
            IEventSiteIdMappingRepository eventSiteIdMappingRepository,
            IEventCategoryMappingRepository eventCategoryMappingRepository,
            IVenueRepository venueRepository,
            IEventDetailRepository eventDetailRepository,
            IEventDeliveryTypeDetailRepository eventDeliveryTypeDetailRepository,
            ITicketCategoryRepository ticketCategoryRepository,
            IEventTicketDetailRepository eventTicketDetailRepository,
            ICurrencyTypeRepository currencyTypeRepository,
            IEventTicketAttributeRepository eventTicketAttributeRepository,
            ITicketFeeDetailRepository ticketFeeDetailRepository,
            IPlaceHolidayDatesRepository placeHolidayDatesRepository,
            ICountryRepository countryRepository,
            IStateRepository stateRepository,
            ICityRepository cityRepository,
            IValueRetailAPI valueRetailAPI,
            IGoogleMapApi googleMapApi,
            IEventVenueMappingTimeRepository eventVenueMappingTimeRepository,
            IEventVenueMappingRepository eventVenueMappingRepository,
            IValueRetailRouteRepository valueRetailRouteRepository,
            IValueRetailReturnStopRepository valueRetailReturnStopRepository,
            IToEnglishTranslator toEnglishTranslator,
            ICountryAlphaCode countryAlphaCode
           )
           : base(mediator)
        {
            _logger = logger;
            _settings = settings;
            _mediator = mediator;
            _valueRetailAuth = valueRetailAuth;
            _valueRetailVillageRepository = valueRetailVillageRepository;
            _eventRepository = eventRepository;
            _daysRepository = daysRepository;
            _placeWeekOpenDaysRepository = placeWeekOpenDaysRepository;
            _eventSiteIdMappingRepository = eventSiteIdMappingRepository;
            _eventCategoryMappingRepository = eventCategoryMappingRepository;
            _venueRepository = venueRepository;
            _eventDetailRepository = eventDetailRepository;
            _eventDeliveryTypeDetailRepository = eventDeliveryTypeDetailRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _currencyTypeRepository = currencyTypeRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _ticketFeeDetailRepository = ticketFeeDetailRepository;
            _placeHolidayDatesRepository = placeHolidayDatesRepository;
            _countryRepository = countryRepository;
            _stateRepository = stateRepository;
            _cityRepository = cityRepository;
            _valueRetailAPI = valueRetailAPI;
            _googleMapApi = googleMapApi;
            _eventVenueMappingTimeRepository = eventVenueMappingTimeRepository;
            _eventVenueMappingRepository = eventVenueMappingRepository;
            _valueRetailRouteRepository = valueRetailRouteRepository;
            _valueRetailReturnStopRepository = valueRetailReturnStopRepository;
            _toEnglishTranslator = toEnglishTranslator;
            _countryAlphaCode = countryAlphaCode;
        }

        protected override async Task Handle(ShoppingExpressCommand command)
        {
            VillageDescription villageDescription = Mapper<VillageDescription>.MapFromJson(await LoadJson());

            var villages = _valueRetailVillageRepository.GetAll();

            foreach (var village in villages)
            {
                var routes = FilterExpressRoutes(village.Id);

                var firstRoute = routes.FirstOrDefault();

                if (firstRoute != null)
                {
                    try
                    {
                        var @event = SaveToEvent(firstRoute, villageDescription, village);
                        var cityAndCurrency = await SaveCityStateCountry(firstRoute, village);
                        var eventVenueMapping = SaveToVenue(firstRoute, @event, cityAndCurrency);
                        var eventDetail = SaveToEventDetails(firstRoute, eventVenueMapping, @event);
                        SaveTicketDetails(firstRoute, eventDetail, cityAndCurrency);
                        await SaveBlockedDates(firstRoute, @event, eventDetail, village);

                        SaveDepartureLocations(routes, eventVenueMapping);
                    }
                    catch (Exception ex)
                    {
                        _logger.Log(Logging.Enums.LogCategory.Error, ex);
                    }
                }
            }
        }

        public Event SaveToEvent(ValueRetailExpressData valueRetailExpressRoute, VillageDescription villageDescription, ValueRetailVillage village)
        {
            string eventName = $"Luxury Shopping Trip from {_toEnglishTranslator.TranslateToEnglish(valueRetailExpressRoute.Name)}";

            var eventResult = new Event();

            eventResult = _eventRepository.GetByEventName(eventName);
            if (eventResult == null)
            {
                eventResult = _eventRepository.Save(new Event
                {
                    AltId = Guid.NewGuid(),
                    Name = eventName,
                    EventCategoryId = ValueRetailEventCategoryConstant.ShoppingPackageParentCategory,
                    EventTypeId = EventType.Perennial,
                    Description = villageDescription.desc.FirstOrDefault(x => x.name == village.VillageCode).description,
                    ClientPointOfContactId = 2,
                    FbEventId = null,
                    MetaDetails = null,
                    IsFeel = true,
                    EventSourceId = EventSource.ValueRetail,
                    TermsAndConditions = "",
                    IsPublishedOnSite = true,
                    PublishedDateTime = DateTime.Now,
                    PublishedBy = null,
                    TestedBy = null,
                    Slug = valueRetailExpressRoute.Name.Replace(" ", "-"),
                    IsEnabled = true,
                    ModifiedBy = tempAltId,
                    UpdatedBy = tempAltId,
                    UpdatedUtc = DateTime.UtcNow,
                    CreatedBy = tempAltId,
                    CreatedUtc = DateTime.UtcNow
                });

                var eventSiteIdMapping = _eventSiteIdMappingRepository.GetByEventId(eventResult.Id);
                if (eventSiteIdMapping == null)
                {
                    _eventSiteIdMappingRepository.Save(new Contracts.DataModels.EventSiteIdMapping
                    {
                        EventId = eventResult.Id,
                        SortOrder = 999,
                        SiteId = Site.feelaplaceSite,
                        ModifiedBy = tempAltId,
                        IsEnabled = true,
                        CreatedUtc = DateTime.UtcNow,
                        CreatedBy = tempAltId,
                        UpdatedUtc = DateTime.UtcNow,
                        UpdatedBy = tempAltId
                    });
                }

                var eventCategoryMapping = _eventCategoryMappingRepository.GetByEventId(eventResult.Id).FirstOrDefault();
                if (eventCategoryMapping == null)
                {
                    _eventCategoryMappingRepository.Save(new Contracts.DataModels.EventCategoryMapping
                    {
                        EventId = eventResult.Id,
                        EventCategoryId = ValueRetailEventCategoryConstant.ShoppingPackageChildCategory,
                        ModifiedBy = tempAltId,
                        IsEnabled = true,
                        CreatedUtc = DateTime.UtcNow,
                        CreatedBy = tempAltId,
                        UpdatedUtc = DateTime.UtcNow,
                        UpdatedBy = tempAltId
                    });
                }

                var days = _daysRepository.GetAll();
                foreach (var day in days)
                {
                    var placeweekOpenDays = _placeWeekOpenDaysRepository.GetByEventIdandDayId(eventResult.Id, day.Id);
                    if (placeweekOpenDays == null)
                    {
                        placeweekOpenDays = _placeWeekOpenDaysRepository.Save(new PlaceWeekOpenDays
                        {
                            AltId = Guid.NewGuid(),
                            EventId = eventResult.Id,
                            DayId = day.Id,
                            IsSameTime = false,
                            ModifiedBy = tempAltId,
                            IsEnabled = true,
                            CreatedUtc = DateTime.UtcNow,
                            CreatedBy = tempAltId,
                            UpdatedUtc = DateTime.UtcNow,
                            UpdatedBy = tempAltId
                        });
                    }
                }
            }
            return eventResult;
        }

        public async Task<SaveLocationReturnValues> SaveCityStateCountry(ValueRetailExpressData route, ValueRetailVillage village)
        {
            var locationData = await _googleMapApi.GetLocationFromLatLong(route.Latitude.ToString(), route.Longitude.ToString());

            if (!locationData.Success)
            {
                throw new ArgumentNullException($"Failed to get Location Data in {this}");
            }

            var countryCodeAndCurrency = await _countryAlphaCode.GetCountryCodeByName(locationData.Result.CountryName);

            var country = _countryRepository.GetByName(locationData.Result.CountryName);
            if (country == null)
            {
                country = _countryRepository.Save(new Country
                {
                    Name = locationData.Result.CountryName,
                    IsoAlphaTwoCode = countryCodeAndCurrency.Result.IsoAlphaTwoCode.ToUpper(),
                    IsoAlphaThreeCode = countryCodeAndCurrency.Result.IsoAlphaThreeCode.ToUpper(),
                    IsEnabled = true,
                });
            }

            var state = _stateRepository.GetByNameAndCountryId(locationData.Result.StateName, country.Id);
            if (state == null)
            {
                state = _stateRepository.Save(new State
                {
                    Name = locationData.Result.StateName,
                    CountryId = country.Id,
                    IsEnabled = true,
                });
            }

            var city = _cityRepository.GetByNameAndStateId(locationData.Result.CityName, state.Id);
            if (city == null)
            {
                city = _cityRepository.Save(new City
                {
                    Name = locationData.Result.CityName,
                    StateId = state.Id,
                    IsEnabled = true,
                });
            }

            var currencyType = _currencyTypeRepository.GetByCurrencyCode(village.CurrencyCode.ToUpper());
            if (currencyType == null)
            {
                currencyType = _currencyTypeRepository.Save(new CurrencyType
                {
                    Code = village.CurrencyCode.ToUpper(),
                    Name = village.CurrencyCode.ToUpper(),
                    CountryId = country.Id,
                    ModifiedBy = tempAltId,
                    IsEnabled = true
                });
            }

            var values = new SaveLocationReturnValues
            {
                cityName = city.Name,
                stateName = state.Name,
                countryName = country.Name,
                cityId = city.Id,
                currencyId = currencyType.Id,
                lat = locationData.Result.lat,
                lng = locationData.Result.lng
            };

            return values;
        }

        public EventVenueMapping SaveToVenue(ValueRetailExpressData route, Event @event, SaveLocationReturnValues locationValues)
        {
            var venue = _venueRepository.GetByVenueNameAndCityId(route.DepartureName, locationValues.cityId);
            if (venue == null)
            {
                venue = _venueRepository.Save(new Venue
                {
                    AltId = Guid.NewGuid(),
                    Name = route.DepartureName,
                    AddressLineOne = route.DepartureAddress,
                    AddressLineTwo = "",
                    CityId = locationValues.cityId,
                    Latitude = locationValues.lat.ToString(),
                    Longitude = locationValues.lng.ToString(),
                    ModifiedBy = tempAltId,
                    IsEnabled = true,
                    CreatedUtc = DateTime.UtcNow,
                    CreatedBy = tempAltId,
                    UpdatedUtc = DateTime.UtcNow,
                    UpdatedBy = tempAltId
                });
            }

            var eventVenueMapping = _eventVenueMappingRepository.GetByEventIdAndVenueId(@event.Id, venue.Id);
            if (eventVenueMapping == null)
            {
                eventVenueMapping = _eventVenueMappingRepository.Save(new EventVenueMapping
                {
                    EventId = @event.Id,
                    VenueId = venue.Id,
                    IsEnabled = true,
                    CreatedUtc = DateTime.UtcNow,
                    UpdatedUtc = DateTime.UtcNow,
                    CreatedBy = tempAltId,
                    UpdatedBy = tempAltId,
                    ModifiedBy = tempAltId
                });
            }
            return eventVenueMapping;
        }

        public void SaveDepartureLocations(List<ValueRetailExpressData> routes, EventVenueMapping eventVenueMapping)
        {
            var eventVenueMappingTime = _eventVenueMappingTimeRepository.GetAllByEventVenueMappingId(eventVenueMapping.Id).FirstOrDefault();
            if (eventVenueMappingTime == null)
            {
                foreach (var item in routes)
                {
                    eventVenueMappingTime = _eventVenueMappingTimeRepository.Save(new EventVenueMappingTime
                    {
                        EventVenueMappingId = eventVenueMapping.Id,
                        PickupTime = item.DepartureTime,
                        PickupLocation = item.DepartureAddress,
                        ReturnTime = item.ReturnTime,
                        ReturnLocation = item.ReturnAddress,
                        JourneyType = item.JourneyType,
                        IsEnabled = true,
                        CreatedBy = tempAltId,
                        CreatedUtc = DateTime.UtcNow,
                        UpdatedBy = tempAltId,
                        UpdatedUtc = DateTime.UtcNow,
                        ModifiedBy = tempAltId
                    });
                }
            }
        }

        public EventDetail SaveToEventDetails(ValueRetailExpressData route, EventVenueMapping eventVenueMapping, Event @event)
        {
            var eventDetail = _eventDetailRepository.GetByEventIdAndVenueId(eventVenueMapping.EventId, eventVenueMapping.VenueId).FirstOrDefault();
            if (eventDetail == null)
            {
                eventDetail = _eventDetailRepository.Save(new EventDetail
                {
                    Name = @event.Name,
                    EventId = eventVenueMapping.EventId,
                    VenueId = eventVenueMapping.VenueId,
                    StartDateTime = DateTime.UtcNow,
                    EndDateTime = DateTime.UtcNow.AddYears(2),
                    GroupId = 1,
                    AltId = Guid.NewGuid(),
                    TicketLimit = 10,
                    ModifiedBy = tempAltId,
                    IsEnabled = true,
                    MetaDetails = "",
                    HideEventDateTime = false,
                    CustomDateTimeMessage = "",
                    CreatedBy = tempAltId,
                    CreatedUtc = DateTime.UtcNow,
                    UpdatedBy = tempAltId,
                    UpdatedUtc = DateTime.UtcNow
                });
            }

            var eventDeliveryType = _eventDeliveryTypeDetailRepository.GetByEventDetailId(eventDetail.Id).FirstOrDefault();
            if (eventDeliveryType == null)
            {
                _eventDeliveryTypeDetailRepository.Save(new EventDeliveryTypeDetail
                {
                    EventDetailId = eventDetail.Id,
                    DeliveryTypeId = DeliveryTypes.MTicket,
                    Notes = "<table><tr><td valign=''top''>1.&nbsp;</td><td valign=''top''>Ticket pickup location and timing will be announced in the “Customer Update” sectionof our website closer to the event. Please check that regularly.</td></tr><tr><td valign=''top''>2.&nbsp;</td><td valign=''top''>The following documents are compulsory for ticket pickup:<br /><table><tr>  <td valign=''top''>  a.&nbsp;  </td>  <td colspan=''2'' valign=''top''>  The card / bank account owner’s original Govt. issued photo ID, along with a clean,  fully legible, photocopy of the same ID  </td></tr><tr>  <td valign=''top''>  b.&nbsp;  </td>  <td colspan=''2'' valign=''top''>  When a debit or credit card has been used for purchase, we also need the original  debit/credit card, along with a clean, fully legible, photocopy of the same card  </td></tr><tr>  <td valign=''top''>  c.&nbsp;  </td>  <td colspan=''2'' valign=''top''>  If sending someone else on behalf of the card holder / bank account owner, then  we need numbers 2.a. and 2.b above (originals and photocopies as mentioned) along  with the following below. This is required even if the representative’s name has  been entered into the system when buying:  </td></tr><tr>  <td valign=''top''>  </td>  <td valign=''top''>  i.&nbsp;  </td>  <td>  An authorization letter with the name of the representative, signed by the card  holder/bank account owner  </td></tr><tr>  <td valign=''top''>  </td>  <td valign=''top''>  ii.&nbsp;  </td>  <td>  A Govt issued photo ID of the representative, along with a clean and legible photocopy  of the same photo identification  </td></tr></table></td></tr><tr><td valign=''top''>3.&nbsp;</td><td valign=''top''>Please note, absence of any one of the documents above can result in the ticketsbeing refused at the ticket pickup window</td></tr>  </table>",
                    RefundPolicy = 1,
                    EndDate = DateTime.UtcNow.AddYears(1),
                    ModifiedBy = tempAltId,
                    IsEnabled = true,
                    CreatedBy = tempAltId,
                    CreatedUtc = DateTime.UtcNow,
                    UpdatedUtc = DateTime.UtcNow,
                    UpdatedBy = tempAltId
                });
            }
            return eventDetail;
        }

        public void SaveTicketDetails(ValueRetailExpressData valueRetailExpressRoute, EventDetail eventDetail, SaveLocationReturnValues locationValues)
        {
            Dictionary<string, decimal> prices = new Dictionary<string, decimal>();
            prices.Add("Adult", (decimal)valueRetailExpressRoute.AdultPrice);
            prices.Add("Child", (decimal)valueRetailExpressRoute.ChildrenPrice);
            prices.Add("Family", (decimal)valueRetailExpressRoute.FamilyPrice);
            prices.Add("Infant", (decimal)valueRetailExpressRoute.InfantPrice);
            prices.Add("Unit Price", (decimal)valueRetailExpressRoute.UnitPrice);

            foreach (var item in prices)
            {
                var ticketCategory = _ticketCategoryRepository.GetByName(item.Key);
                if (ticketCategory == null)
                {
                    ticketCategory = _ticketCategoryRepository.Save(new TicketCategory
                    {
                        Name = item.Key,
                        ModifiedBy = tempAltId,
                        IsEnabled = true,
                        CreatedBy = tempAltId,
                        CreatedUtc = DateTime.UtcNow,
                        UpdatedUtc = DateTime.UtcNow,
                        UpdatedBy = tempAltId
                    });
                }

                var eventTicketDetail = _eventTicketDetailRepository.GetByTicketCategoryIdAndeventDetailId(ticketCategory.Id, eventDetail.Id).FirstOrDefault();
                if (eventTicketDetail == null)
                {
                    eventTicketDetail = _eventTicketDetailRepository.Save(new EventTicketDetail
                    {
                        EventDetailId = eventDetail.Id,
                        TicketCategoryId = ticketCategory.Id,
                        ModifiedBy = tempAltId,
                        IsEnabled = true,
                        CreatedBy = tempAltId,
                        CreatedUtc = DateTime.UtcNow,
                        UpdatedUtc = DateTime.UtcNow,
                        UpdatedBy = tempAltId
                    });
                }

                var eventTicketAttribute = _eventTicketAttributeRepository.GetByEventTicketDetailId(eventTicketDetail.Id);
                if (eventTicketAttribute == null)
                {
                    eventTicketAttribute = _eventTicketAttributeRepository.Save(new EventTicketAttribute
                    {
                        EventTicketDetailId = eventTicketDetail.Id,
                        SalesStartDateTime = DateTime.UtcNow,
                        SalesEndDatetime = DateTime.UtcNow.AddYears(1),
                        TicketTypeId = TicketType.Regular,
                        ChannelId = Channels.Feel,
                        CurrencyId = locationValues.currencyId,
                        SharedInventoryGroupId = null,
                        TicketCategoryDescription = "",
                        ViewFromStand = "",
                        IsSeatSelection = false,
                        AvailableTicketForSale = 100,
                        RemainingTicketForSale = 100,
                        Price = Convert.ToDecimal(item.Value < 0 ? 0 : item.Value),
                        IsInternationalCardAllowed = false,
                        IsEMIApplicable = false,
                        ModifiedBy = tempAltId,
                        IsEnabled = true,
                        CreatedBy = tempAltId,
                        CreatedUtc = DateTime.UtcNow,
                        UpdatedUtc = DateTime.UtcNow,
                        UpdatedBy = tempAltId
                    });
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
                        ModifiedBy = tempAltId,
                        IsEnabled = true,
                        CreatedBy = tempAltId,
                        CreatedUtc = DateTime.UtcNow,
                        UpdatedUtc = DateTime.UtcNow,
                        UpdatedBy = tempAltId
                    });
                }
            }
            prices.Clear();
        }

        public async Task SaveBlockedDates(ValueRetailExpressData valueRetailExpressRoute, Event @event, EventDetail eventDetail, ValueRetailVillage village)
        {
            var responseString = await _valueRetailAPI.GetValueRetailAPIData(new ValueRetailCommanRequestModel
            {
                villageCode = village.VillageCode,
                cultureCode = village.CultureCode,
                journeyType = valueRetailExpressRoute.JourneyType,
                routeId = valueRetailExpressRoute.RouteId,
                from = DateTime.UtcNow
            }, "Route", "ShoppingExpress");

            var responseData = Mapper<ShoppingExpressRoute>.MapFromJson(responseString.Result);
            foreach (var date in responseData.Availability.BlockedDates)
            {
                var placeHoliDayDates = _placeHolidayDatesRepository.GetByEventandDate(@event.Id, Convert.ToDateTime(date));
                placeHoliDayDates = _placeHolidayDatesRepository.Save(new PlaceHolidayDate
                {
                    LeaveDateTime = Convert.ToDateTime(date),
                    EventId = @event.Id,
                    IsEnabled = true,
                    EventDetailId = eventDetail.Id,
                    ModifiedBy = tempAltId,
                    CreatedBy = tempAltId,
                    CreatedUtc = DateTime.UtcNow,
                    UpdatedUtc = DateTime.UtcNow,
                    UpdatedBy = tempAltId
                });
            }
        }

        public async Task<string> LoadJson()
        {
            string jsonData = string.Empty;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var data = await client.GetAsync("https://feel-cdn.s3-us-west-2.amazonaws.com/document/village-description.json");
                    jsonData = await data.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
            }
            return jsonData;
        }

        public List<ValueRetailExpressData> FilterExpressRoutes(int villageId)
        {
            var routes = _valueRetailRouteRepository.GetByVillageId(villageId).ToList();
            var routeGroupByName = routes.GroupBy(r => new { r.Name });

            var routeResultList = new List<ValueRetailExpressData>();

            foreach (var routeName in routeGroupByName)
            {
                var grpOnLocationName = routeName.GroupBy(g => new { g.LocationName });

                foreach (var routeGrp in grpOnLocationName)
                {
                    var distinctDepartureTime = routeGrp.GroupBy(g => new { g.DepartureTime });

                    foreach (var timeGrp in distinctDepartureTime)
                    {
                        foreach (var departureTime in timeGrp)
                        {
                            var returnTimeModel = _valueRetailReturnStopRepository.GetByValueRetailRouteIdAndRouteId(departureTime.Id, departureTime.LinkedRouteId);

                            foreach (var returnTime in returnTimeModel)
                            {
                                routeResultList.Add(new ValueRetailExpressData
                                {
                                    VillageId = departureTime.VillageId,
                                    JourneyType = departureTime.JourneyType,
                                    RouteId = departureTime.RouteId,
                                    DepartureTime = departureTime.DepartureTime,
                                    ReturnTime = returnTime.ReturnTime,
                                    Name = departureTime.Name,
                                    LocationId = departureTime.LocationId,
                                    DepartureName = departureTime.LocationName,
                                    DepartureAddress = departureTime.LocationAddress,
                                    ReturnName = returnTime.LocationName,
                                    ReturnAddress = returnTime.LocationAddress,
                                    Latitude = departureTime.Latitude,
                                    Longitude = departureTime.Longitude,
                                    AdultPrice = departureTime.AdultPrice,
                                    ChildrenPrice = departureTime.ChildrenPrice,
                                    FamilyPrice = departureTime.FamilyPrice,
                                    InfantPrice = departureTime.InfantPrice,
                                    UnitPrice = departureTime.UnitPrice
                                });
                            }
                        }
                    }
                }
            }

            return routeResultList;
        }

        public class ValueRetailExpressData
        {
            public int VillageId { get; set; }
            public int JourneyType { get; set; }
            public int RouteId { get; set; }
            public string DepartureTime { get; set; }
            public string ReturnTime { get; set; }
            public string Name { get; set; }
            public int LocationId { get; set; }
            public string DepartureName { get; set; }
            public string DepartureAddress { get; set; }
            public string ReturnName { get; set; }
            public string ReturnAddress { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public decimal AdultPrice { get; set; }
            public decimal ChildrenPrice { get; set; }
            public decimal FamilyPrice { get; set; }
            public decimal InfantPrice { get; set; }
            public decimal UnitPrice { get; set; }
        }

        public class SaveLocationReturnValues
        {
            public string cityName { get; set; }
            public string stateName { get; set; }
            public string countryName { get; set; }
            public int cityId { get; set; }
            public int currencyId { get; set; }
            public double? lat { get; set; }
            public double? lng { get; set; }
        }
    }
}