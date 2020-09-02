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
using FIL.Contracts.Models.ValueRetail.ShoppingPackage;
using FIL.Logging;
using FIL.Logging.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.ValueRetail
{
    public class ChauffeurDrivenCommandHandler : BaseCommandHandler<ChauffeurDrivenCommand>
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
        private readonly ILanguageTranslationApi _languageTranslationApi;
        private readonly ICountryAlphaCode _countryAlphaCode;
        private readonly ObjectComparersFactory objectComparersFactory = new ObjectComparersFactory();
        public List<JourneyRoute> journeyRoutes = new List<JourneyRoute>();

        public ChauffeurDrivenCommandHandler(
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
            ILanguageTranslationApi languageTranslationApi,
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
            _languageTranslationApi = languageTranslationApi;
            _countryAlphaCode = countryAlphaCode;
        }

        protected override async Task Handle(ChauffeurDrivenCommand command)
        {
            var villages = _valueRetailVillageRepository.GetAll();

            foreach (var village in villages)
            {
                var httpRequest = new ValueRetailCommanRequestModel
                {
                    from = DateTime.UtcNow,
                    cultureCode = village.CultureCode,
                    villageCode = village.VillageCode
                };

                var routeResponseString = await _valueRetailAPI.GetValueRetailAPIData(httpRequest, "Routes", "ChauffeurDriven");
                ChauffeurDrivenRouteResponse chauffeurDrivenRouteResponse = Mapper<ChauffeurDrivenRouteResponse>.MapFromJson(routeResponseString.Result);

                if (chauffeurDrivenRouteResponse.RequestStatus.Success)
                {
                    foreach (var route in chauffeurDrivenRouteResponse.Routes)
                    {
                        var @event = SaveToEvent(route, village);
                        if (objectComparersFactory.GetObjectsComparer<Event>().Compare(@event, new Event()))
                        {
                            throw new TaskCanceledException("Data Insertion Failed for Value Retail Chauffeur Drive.");
                        }
                        else
                        {
                            var cityAndCurrency = await SaveCityStateCountry(route, village);
                            var eventVenueMapping = SaveToVenue(route, @event, cityAndCurrency);
                            SaveDepartureLocations(route, eventVenueMapping);
                            var eventDetail = SaveToEventDetails(@event, eventVenueMapping);
                            SaveTicketDetail(route, eventDetail, cityAndCurrency);
                            SaveBlockedDates(route, @event, eventDetail);
                        }
                    }
                }
            }
        }

        public Event SaveToEvent(ChauffeurRoute route, ValueRetailVillage valueRetailVillage)
        {
            var eventName = CheckAndTranslateLanguage(route.ServiceType);

            var @event = new Event
            {
                AltId = Guid.NewGuid(),
                Name = $"Chauffeur {eventName} from {route.LocationHeader} to {valueRetailVillage.VillageName}",
                EventCategoryId = ValueRetailEventCategoryConstant.ShoppingPackageParentCategory,
                EventTypeId = EventType.Perennial,
                Description = $"<p>{CheckAndTranslateLanguage(route.ServiceDescription)}</p>",
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
                Slug = $"{eventName} {route.LocationHeader}".Replace(" ", "-"),
                ModifiedBy = Guid.NewGuid(),
                IsEnabled = true
            };

            try
            {
                var eventResult = _eventRepository.GetByEventName(@event.Name);
                if (eventResult == null)
                {
                    eventResult = _eventRepository.Save(@event);
                }

                var eventSiteIdMapping = _eventSiteIdMappingRepository.GetByEventId(eventResult.Id);
                if (eventSiteIdMapping == null)
                {
                    eventSiteIdMapping = _eventSiteIdMappingRepository.Save(new Contracts.DataModels.EventSiteIdMapping
                    {
                        EventId = eventResult.Id,
                        SortOrder = 999,
                        SiteId = Site.feelaplaceSite,
                        ModifiedBy = eventResult.ModifiedBy,
                        IsEnabled = true
                    });
                }

                var eventCategoryMapping = _eventCategoryMappingRepository.GetByEventId(eventResult.Id).FirstOrDefault();
                if (eventCategoryMapping == null)
                {
                    _eventCategoryMappingRepository.Save(new Contracts.DataModels.EventCategoryMapping
                    {
                        EventId = eventResult.Id,
                        EventCategoryId = ValueRetailEventCategoryConstant.ChauffeurDriveChildCategory,
                        ModifiedBy = eventResult.ModifiedBy,
                        IsEnabled = true
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
                            ModifiedBy = eventResult.ModifiedBy,
                            IsEnabled = true
                        });
                    }
                }
                return eventResult;
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, ex);
                return new Event();
            }
        }

        public async Task<SaveLocationReturnValues> SaveCityStateCountry(ChauffeurRoute routeDetail, ValueRetailVillage village)
        {
            var locationData = await _googleMapApi.GetLatLongFromAddress(routeDetail.LocationHeader);

            try
            {
                if (!locationData.Success)
                {
                    throw new ArgumentNullException(locationData.Result.CityName, "Failed to get Location Data");
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
                        ModifiedBy = Guid.NewGuid(),
                        IsEnabled = true
                    });
                }
                var values = new SaveLocationReturnValues
                {
                    cityId = city.Id,
                    currencyId = currencyType.Id,
                    lat = locationData.Result.lat,
                    lng = locationData.Result.lng
                };

                return values;
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, ex);
                return new SaveLocationReturnValues();
            }
        }

        public EventVenueMapping SaveToVenue(ChauffeurRoute routeDetail, Event @event, SaveLocationReturnValues locationValues)
        {
            try
            {
                var venue = _venueRepository.GetByVenueNameAndCityId(routeDetail.LocationHeader, locationValues.cityId);
                if (venue == null || venue.Name == null)
                {
                    venue = _venueRepository.Save(new Venue
                    {
                        AltId = Guid.NewGuid(),
                        Name = routeDetail.LocationHeader,
                        AddressLineOne = routeDetail.PickupLocation,
                        AddressLineTwo = routeDetail.PickupLocation,
                        CityId = locationValues.cityId,
                        Latitude = locationValues.lat.ToString(),
                        Longitude = locationValues.lng.ToString(),
                        ModifiedBy = @event.ModifiedBy,
                        IsEnabled = true
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
                        UpdatedUtc = null,
                        CreatedBy = @event.ModifiedBy,
                        UpdatedBy = null,
                        ModifiedBy = @event.ModifiedBy
                    });
                }
                return eventVenueMapping;
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, ex);
                return new EventVenueMapping();
            }
        }

        public void SaveDepartureLocations(ChauffeurRoute route, EventVenueMapping eventVenueMapping)
        {
            try
            {
                var eventVenueMappingTime = _eventVenueMappingTimeRepository.GetAllByEventVenueMappingId(eventVenueMapping.Id).FirstOrDefault();
                if (eventVenueMappingTime == null)
                {
                    eventVenueMappingTime = _eventVenueMappingTimeRepository.Save(new EventVenueMappingTime
                    {
                        EventVenueMappingId = eventVenueMapping.Id,
                        PickupTime = null,
                        PickupLocation = route.PickupLocation,
                        ReturnTime = null,
                        ReturnLocation = null,
                        JourneyType = 1,
                        IsEnabled = true,
                        CreatedBy = Guid.NewGuid(),
                        CreatedUtc = DateTime.UtcNow,
                        UpdatedBy = null,
                        UpdatedUtc = null,
                        ModifiedBy = Guid.NewGuid()
                    });

                    eventVenueMappingTime = _eventVenueMappingTimeRepository.Save(new EventVenueMappingTime
                    {
                        EventVenueMappingId = eventVenueMapping.Id,
                        PickupTime = null,
                        PickupLocation = route.PickupLocation,
                        ReturnTime = null,
                        ReturnLocation = null,
                        JourneyType = 2,
                        IsEnabled = true,
                        CreatedBy = Guid.NewGuid(),
                        CreatedUtc = DateTime.UtcNow,
                        UpdatedBy = null,
                        UpdatedUtc = null,
                        ModifiedBy = Guid.NewGuid()
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, ex);
            }
        }

        public EventDetail SaveToEventDetails(Event @event, EventVenueMapping eventVenueMapping)
        {
            try
            {
                var eventDetail = _eventDetailRepository.GetSubeventByEventId(eventVenueMapping.EventId).FirstOrDefault();
                if (eventDetail == null)
                {
                    eventDetail = _eventDetailRepository.Save(new EventDetail
                    {
                        Name = @event.Name,
                        EventId = eventVenueMapping.EventId,
                        VenueId = eventVenueMapping.VenueId,
                        StartDateTime = DateTime.UtcNow,
                        EndDateTime = DateTime.UtcNow.AddYears(1),
                        GroupId = 1,
                        AltId = Guid.NewGuid(),
                        TicketLimit = 4,
                        ModifiedBy = eventVenueMapping.ModifiedBy,
                        IsEnabled = true,
                        MetaDetails = "",
                        HideEventDateTime = false,
                        CustomDateTimeMessage = "",
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
                        EndDate = DateTime.UtcNow.AddYears(2),
                        RefundPolicy = 1,
                        ModifiedBy = eventVenueMapping.ModifiedBy,
                        IsEnabled = true
                    });
                }
                return eventDetail;
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, ex);
                return new EventDetail();
            }
        }

        public void SaveTicketDetail(ChauffeurRoute route, EventDetail eventDetail, SaveLocationReturnValues cityAndCurrency)
        {
            var category = "Passengers";
            try
            {
                var ticketCategory = _ticketCategoryRepository.GetByName(category);
                if (ticketCategory == null)
                {
                    ticketCategory = _ticketCategoryRepository.Save(new TicketCategory
                    {
                        Name = category,
                        ModifiedBy = eventDetail.ModifiedBy,
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
                        ModifiedBy = eventDetail.ModifiedBy,
                        IsEnabled = true
                    });
                }

                var eventTicketAttributeIn = new EventTicketAttribute
                {
                    EventTicketDetailId = eventTicketDetail.Id,
                    SalesStartDateTime = eventDetail.StartDateTime,
                    SalesEndDatetime = eventDetail.EndDateTime,
                    TicketTypeId = TicketType.Regular,
                    ChannelId = Channels.Feel,
                    CurrencyId = cityAndCurrency.currencyId,
                    SharedInventoryGroupId = null,
                    TicketCategoryDescription = $"Chauffeur Service Price from {route.LocationHeader}",
                    ViewFromStand = string.Empty,
                    IsSeatSelection = false,
                    AvailableTicketForSale = 50,
                    RemainingTicketForSale = 50,
                    Price = Convert.ToDecimal(route.ReturnPrice) < 0 ? 0 : Convert.ToDecimal(route.ReturnPrice),
                    IsInternationalCardAllowed = false,
                    IsEMIApplicable = false,
                    ModifiedBy = eventDetail.ModifiedBy,
                    IsEnabled = true
                };

                var eventTicketAttributeOut = _eventTicketAttributeRepository.GetByEventTicketDetailId(eventTicketDetail.Id);

                if (eventTicketAttributeOut == null)
                {
                    eventTicketAttributeOut = _eventTicketAttributeRepository.Save(eventTicketAttributeIn);
                }
                else
                {
                    eventTicketAttributeOut.Price = Convert.ToDecimal(route.ReturnPrice) < 0 ? 0 : Convert.ToDecimal(route.ReturnPrice);
                    eventTicketAttributeOut = _eventTicketAttributeRepository.Save(eventTicketAttributeIn);
                }

                var ticketFeeDetail = _ticketFeeDetailRepository.GetByEventTicketAttributeIdAndFeedId(eventTicketAttributeOut.Id, (int)FeeType.ConvenienceCharge);
                if (ticketFeeDetail == null)
                {
                    ticketFeeDetail = _ticketFeeDetailRepository.Save(new TicketFeeDetail
                    {
                        EventTicketAttributeId = eventTicketAttributeOut.Id,
                        FeeId = (int)FeeType.ConvenienceCharge,
                        DisplayName = "Convienence Charge",
                        ValueTypeId = (int)ValueTypes.Percentage,
                        Value = 5,
                        ModifiedBy = eventDetail.ModifiedBy,
                        IsEnabled = true
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, ex);
            }
        }

        public void SaveBlockedDates(ChauffeurRoute route, Event @event, EventDetail eventDetail)
        {
            foreach (var item in route.BlockedDate)
            {
                try
                {
                    var placeHoliDayDates = _placeHolidayDatesRepository.GetByEventandDate(@event.Id, Convert.ToDateTime(item));
                    placeHoliDayDates = _placeHolidayDatesRepository.Save(new PlaceHolidayDate
                    {
                        LeaveDateTime = Convert.ToDateTime(item),
                        EventId = @event.Id,
                        IsEnabled = true,
                        EventDetailId = eventDetail.Id,
                        ModifiedBy = @event.ModifiedBy
                    });
                }
                catch (Exception ex)
                {
                    _logger.Log(LogCategory.Error, ex);
                }
            }
        }

        public string CheckAndTranslateLanguage(string text)
        {
            string lang = _languageTranslationApi.DetectTextLanguage(text).Result;
            if (lang == "en")
            {
                return text;
            }
            else
            {
                return _languageTranslationApi.TranslateText(text).Result;
            }
        }
    }

    public class SaveLocationReturnValues
    {
        public int cityId { get; set; }
        public int currencyId { get; set; }
        public double? lat { get; set; }
        public double? lng { get; set; }
    }
}