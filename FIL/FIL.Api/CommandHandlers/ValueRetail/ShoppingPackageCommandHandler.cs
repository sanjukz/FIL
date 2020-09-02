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
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.ValueRetail
{
    public class ShoppingPackageCommandHandler : BaseCommandHandler<ShoppingPackageCommand>
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
        private readonly IValueRetailPackageRouteRepository _valueRetailPackageRouteRepository;
        private readonly IValueRetailPackageReturnRepository _valueRetailPackageReturnRepository;
        private readonly IToEnglishTranslator _toEnglishTranslator;
        private readonly ICountryAlphaCode _countryAlphaCode;
        public List<JourneyRoute> journeyRoutes = new List<JourneyRoute>();
        private readonly ObjectComparersFactory objectComparersFactory = new ObjectComparersFactory();

        public ShoppingPackageCommandHandler(
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
            IValueRetailPackageRouteRepository valueRetailPackageRouteRepository,
            IValueRetailPackageReturnRepository valueRetailPackageReturnRepository,
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
            _valueRetailPackageRouteRepository = valueRetailPackageRouteRepository;
            _valueRetailPackageReturnRepository = valueRetailPackageReturnRepository;
            _toEnglishTranslator = toEnglishTranslator;
            _countryAlphaCode = countryAlphaCode;
        }

        protected override async Task Handle(ShoppingPackageCommand command)
        {
            var villages = _valueRetailVillageRepository.GetAll();

            foreach (var village in villages)
            {
                ValueRetailCommanRequestModel ValueRetailCommanRequestModel = new ValueRetailCommanRequestModel
                {
                    cultureCode = village.CultureCode,
                    villageCode = village.VillageCode,
                    from = DateTime.UtcNow
                };

                var routeResonseString = await _valueRetailAPI.GetValueRetailAPIData(ValueRetailCommanRequestModel, "Routes", "ShoppingPackage");
                if (routeResonseString.Success == true)
                {
                    ShoppingPackageRouteResponse shoppingPackageRouteResponse = Mapper<ShoppingPackageRouteResponse>.MapFromJson(routeResonseString.Result);
                    foreach (var package in shoppingPackageRouteResponse.Packages)
                    {
                        if (package.Package.PackageID == 367 || package.Package.PackageID == 366 || package.Package.PackageID == 368) { continue; }
                        ValueRetailCommanRequestModel httpRequest = new ValueRetailCommanRequestModel
                        {
                            from = DateTime.UtcNow,
                            to = DateTime.UtcNow.AddYears(1),
                            packageId = package.Package.PackageID,
                            cultureCode = village.CultureCode,
                            villageCode = village.VillageCode
                        };

                        var routeDetailResponseString = await _valueRetailAPI.GetValueRetailAPIData(httpRequest, "RouteDetails", "ShoppingPackage");
                        ShoppingPackageRouteDetail shoppingPackageRouteDetail = Mapper<ShoppingPackageRouteDetail>.MapFromJson(routeDetailResponseString.Result);
                        if (shoppingPackageRouteDetail.RequestStatus.Success)
                        {
                            var @event = SaveToEvent(shoppingPackageRouteDetail, village);
                            if (!objectComparersFactory.GetObjectsComparer<Event>().Compare(@event, new Event()))
                            {
                                var cityAndCurrency = await SaveCityStateCountry(shoppingPackageRouteDetail, village);
                                var eventVenueMapping = SaveToVenue(shoppingPackageRouteDetail, @event, cityAndCurrency, village);
                                SaveDepartureLocations(shoppingPackageRouteDetail, eventVenueMapping);
                                var eventDetail = SaveToEventDetails(shoppingPackageRouteDetail, eventVenueMapping, @event);
                                SaveBlockedDates(shoppingPackageRouteDetail, @event, eventDetail);
                                SaveTicketDetail(shoppingPackageRouteDetail, eventDetail, cityAndCurrency);
                            }
                            else
                            {
                                throw new TaskCanceledException("Data insertion for Value Retail Package failed!");
                            }
                        }
                    }
                }
            }
        }

        public Event SaveToEvent(ShoppingPackageRouteDetail routeDetail, ValueRetailVillage village)
        {
            string longDescription = StripText(routeDetail.RouteDetails.Package.LongDescription),
                redemption = StripText(routeDetail.RouteDetails.Package.Redemption),
                restriction = StripText(routeDetail.RouteDetails.Package.Restriction);

            string originalEventName = _toEnglishTranslator.TranslateToEnglish(routeDetail.RouteDetails.Package.PackageName);

            string customEventName = originalEventName.ToLower().Contains("village") ? originalEventName : $"{originalEventName} at {village.VillageName}";

            var @event = new Event
            {
                AltId = Guid.NewGuid(),
                Name = customEventName,
                EventCategoryId = ValueRetailEventCategoryConstant.ShoppingPackageParentCategory,
                EventTypeId = EventType.Perennial,
                Description = $"<p>{ _toEnglishTranslator.TranslateToEnglish(longDescription)}</p><p>{_toEnglishTranslator.TranslateToEnglish(redemption)}</p><p>{_toEnglishTranslator.TranslateToEnglish(restriction) }</p>",
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
                Slug = $"shopping-package-{routeDetail.RouteDetails.Package.PackageID}".ToLower(),
                ModifiedBy = Guid.NewGuid(),
                IsEnabled = true,
                UpdatedUtc = DateTime.Now,
                UpdatedBy = Guid.NewGuid()
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
                    _eventSiteIdMappingRepository.Save(new Contracts.DataModels.EventSiteIdMapping
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
                        EventCategoryId = ValueRetailEventCategoryConstant.ShoppingPackageChildCategory,
                        ModifiedBy = eventResult.ModifiedBy,
                        IsEnabled = true
                    });
                }

                var days = _daysRepository.GetAll();
                //Save Open days and Timing
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
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                return new Event();
            }
        }

        public async Task<SaveLocationReturnValues> SaveCityStateCountry(ShoppingPackageRouteDetail routeDetail, ValueRetailVillage village)
        {
            var locationData = await _googleMapApi.GetLatLongFromAddress(village.VillageName);

            try
            {
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
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                return new SaveLocationReturnValues();
            }
        }

        public EventVenueMapping SaveToVenue(ShoppingPackageRouteDetail routeDetail, Event @event, SaveLocationReturnValues locationValues, ValueRetailVillage village)
        {
            try
            {
                var venue = _venueRepository.GetByVenueNameAndCityId(village.VillageName, locationValues.cityId);
                if (venue == null)
                {
                    venue = _venueRepository.Save(new Venue
                    {
                        AltId = Guid.NewGuid(),
                        Name = village.VillageName,
                        AddressLineOne = "",
                        AddressLineTwo = "",
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
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                return new EventVenueMapping();
            }
        }

        public void SaveDepartureLocations(ShoppingPackageRouteDetail shoppingPackageRouteDetail, EventVenueMapping eventVenueMapping)
        {
            var departureTimelist = ValueRetailRouteFilter(shoppingPackageRouteDetail);

            try
            {
                var eventVenueMappingTime = _eventVenueMappingTimeRepository.GetAllByEventVenueMappingId(eventVenueMapping.Id).FirstOrDefault();
                if (eventVenueMappingTime == null)
                {
                    foreach (var timelist in departureTimelist)
                    {
                        eventVenueMappingTime = _eventVenueMappingTimeRepository.Save(new EventVenueMappingTime
                        {
                            EventVenueMappingId = eventVenueMapping.Id,
                            PickupTime = timelist.DepartureTime,
                            PickupLocation = timelist.DepartureAddress,
                            ReturnTime = timelist.ReturnTime,
                            ReturnLocation = timelist.DepartureAddress,
                            JourneyType = timelist.JourneyType,
                            IsEnabled = true,
                            CreatedBy = Guid.NewGuid(),
                            CreatedUtc = DateTime.UtcNow,
                            UpdatedBy = null,
                            UpdatedUtc = null,
                            ModifiedBy = Guid.NewGuid()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
            }
        }

        public EventDetail SaveToEventDetails(ShoppingPackageRouteDetail shoppingPackageRouteDetail, EventVenueMapping eventVenueMapping, Event @event)
        {
            var eventDetail = _eventDetailRepository.GetSubEventByEventId(eventVenueMapping.EventId).FirstOrDefault();
            if (eventDetail == null)
            {
                eventDetail = _eventDetailRepository.Save(new EventDetail
                {
                    Name = @event.Name,
                    EventId = eventVenueMapping.EventId,
                    VenueId = eventVenueMapping.VenueId,
                    StartDateTime = shoppingPackageRouteDetail.RouteDetails.Package.StartDate,
                    EndDateTime = shoppingPackageRouteDetail.RouteDetails.Package.EndDate,
                    GroupId = 1,
                    AltId = Guid.NewGuid(),
                    TicketLimit = 10,
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
                    RefundPolicy = 1,
                    EndDate = shoppingPackageRouteDetail.RouteDetails.Package.EndDate,
                    ModifiedBy = eventVenueMapping.ModifiedBy,
                    IsEnabled = true
                });
            }
            return eventDetail;
        }

        public void SaveBlockedDates(ShoppingPackageRouteDetail shoppingPackageRouteDetail, Event @event, EventDetail eventDetail)
        {
            if (shoppingPackageRouteDetail.RouteDetails.Package.BlockedDate.Count > 0)
            {
                foreach (var date in shoppingPackageRouteDetail.RouteDetails.Package.BlockedDate)
                {
                    var placeHoliDayDates = _placeHolidayDatesRepository.GetByEventandDate(@event.Id, Convert.ToDateTime(date));
                    placeHoliDayDates = _placeHolidayDatesRepository.Save(new PlaceHolidayDate
                    {
                        LeaveDateTime = Convert.ToDateTime(date),
                        EventId = @event.Id,
                        IsEnabled = true,
                        EventDetailId = eventDetail.Id,
                        ModifiedBy = @event.ModifiedBy
                    });
                }
            }
        }

        public void SaveTicketDetail(ShoppingPackageRouteDetail shoppingPackageRouteDetail, EventDetail eventDetail, SaveLocationReturnValues cityAndCurrency)
        {
            foreach (var price in shoppingPackageRouteDetail.RouteDetails.Package.PricePerPersons)
            {
                if (price.PackagePriceType == 0)
                {
                    var category = "Adult";
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

                    var eventTicketAttribute = _eventTicketAttributeRepository.GetByEventTicketDetailId(eventTicketDetail.Id);
                    if (eventTicketAttribute == null)
                    {
                        eventTicketAttribute = _eventTicketAttributeRepository.Save(new EventTicketAttribute
                        {
                            EventTicketDetailId = eventTicketDetail.Id,
                            SalesStartDateTime = eventDetail.StartDateTime,
                            SalesEndDatetime = eventDetail.EndDateTime,
                            TicketTypeId = TicketType.Regular,
                            ChannelId = Channels.Feel,
                            CurrencyId = cityAndCurrency.currencyId,
                            SharedInventoryGroupId = null,
                            TicketCategoryDescription = "Adult Return Ticket Price",
                            ViewFromStand = string.Empty,
                            IsSeatSelection = false,
                            AvailableTicketForSale = 100,
                            RemainingTicketForSale = 100,
                            Price = Convert.ToDecimal(price.Price < 0 ? 0 : price.Price),
                            IsInternationalCardAllowed = false,
                            IsEMIApplicable = false,
                            ModifiedBy = eventDetail.ModifiedBy,
                            IsEnabled = true
                        });
                    }
                    else
                    {
                        eventTicketAttribute.Price = Convert.ToDecimal(price.Price < 0 ? 0 : price.Price);
                        eventTicketAttribute = _eventTicketAttributeRepository.Save(eventTicketAttribute);
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
                            ModifiedBy = eventDetail.ModifiedBy,
                            IsEnabled = true
                        });
                    }
                }

                if (price.PackagePriceType == 1)
                {
                    var category = "Child";
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

                    var eventTicketAttribute = _eventTicketAttributeRepository.GetByEventTicketDetailId(eventTicketDetail.Id);
                    if (eventTicketAttribute == null)
                    {
                        eventTicketAttribute = _eventTicketAttributeRepository.Save(new EventTicketAttribute
                        {
                            EventTicketDetailId = eventTicketDetail.Id,
                            SalesStartDateTime = eventDetail.StartDateTime,
                            SalesEndDatetime = eventDetail.EndDateTime,
                            TicketTypeId = TicketType.Regular,
                            ChannelId = Channels.Feel,
                            CurrencyId = cityAndCurrency.currencyId,
                            SharedInventoryGroupId = null,
                            TicketCategoryDescription = "Child Return Ticket Price",
                            ViewFromStand = "",
                            IsSeatSelection = false,
                            AvailableTicketForSale = 100,
                            RemainingTicketForSale = 100,
                            Price = Convert.ToDecimal(price.Price < 0 ? 0 : price.Price),
                            IsInternationalCardAllowed = false,
                            IsEMIApplicable = false,
                            ModifiedBy = eventDetail.ModifiedBy,
                            IsEnabled = true
                        });
                    }
                    else
                    {
                        eventTicketAttribute.Price = Convert.ToDecimal(price.Price < 0 ? 0 : price.Price);
                        eventTicketAttribute = _eventTicketAttributeRepository.Save(eventTicketAttribute);
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
                            ModifiedBy = eventDetail.ModifiedBy,
                            IsEnabled = true
                        });
                    }
                }

                if (price.PackagePriceType == 2)
                {
                    var category = "Infant";
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

                    var eventTicketAttribute = _eventTicketAttributeRepository.GetByEventTicketDetailId(eventTicketDetail.Id);
                    if (eventTicketAttribute == null)
                    {
                        eventTicketAttribute = _eventTicketAttributeRepository.Save(new EventTicketAttribute
                        {
                            EventTicketDetailId = eventTicketDetail.Id,
                            SalesStartDateTime = eventDetail.StartDateTime,
                            SalesEndDatetime = eventDetail.EndDateTime,
                            TicketTypeId = TicketType.Regular,
                            ChannelId = Channels.Feel,
                            CurrencyId = cityAndCurrency.currencyId,
                            SharedInventoryGroupId = null,
                            TicketCategoryDescription = "Infant Return Ticket Price",
                            ViewFromStand = "",
                            IsSeatSelection = false,
                            AvailableTicketForSale = 100,
                            RemainingTicketForSale = 100,
                            Price = Convert.ToDecimal(price.Price < 0 ? 0 : price.Price),
                            IsInternationalCardAllowed = false,
                            IsEMIApplicable = false,
                            ModifiedBy = eventDetail.ModifiedBy,
                            IsEnabled = true
                        });
                    }
                    else
                    {
                        eventTicketAttribute.Price = Convert.ToDecimal(price.Price < 0 ? 0 : price.Price);
                        eventTicketAttribute = _eventTicketAttributeRepository.Save(eventTicketAttribute);
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
                            ModifiedBy = eventDetail.ModifiedBy,
                            IsEnabled = true
                        });
                    }
                }

                if (price.PackagePriceType == 3)
                {
                    var category = "Family";
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

                    var eventTicketAttribute = _eventTicketAttributeRepository.GetByEventTicketDetailId(eventTicketDetail.Id);
                    if (eventTicketAttribute == null)
                    {
                        eventTicketAttribute = _eventTicketAttributeRepository.Save(new EventTicketAttribute
                        {
                            EventTicketDetailId = eventTicketDetail.Id,
                            SalesStartDateTime = eventDetail.StartDateTime,
                            SalesEndDatetime = eventDetail.EndDateTime,
                            TicketTypeId = TicketType.Regular,
                            ChannelId = Channels.Feel,
                            CurrencyId = cityAndCurrency.currencyId,
                            SharedInventoryGroupId = null,
                            TicketCategoryDescription = "",
                            ViewFromStand = "",
                            IsSeatSelection = false,
                            AvailableTicketForSale = 100,
                            RemainingTicketForSale = 100,
                            Price = Convert.ToDecimal(price.Price < 0 ? 0 : price.Price),
                            IsInternationalCardAllowed = false,
                            IsEMIApplicable = false,
                            ModifiedBy = eventDetail.ModifiedBy,
                            IsEnabled = true
                        });
                    }
                    else
                    {
                        eventTicketAttribute.Price = Convert.ToDecimal(price.Price < 0 ? 0 : price.Price);
                        eventTicketAttribute = _eventTicketAttributeRepository.Save(eventTicketAttribute);
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
                            ModifiedBy = eventDetail.ModifiedBy,
                            IsEnabled = true
                        });
                    }
                }
            }
        }

        public List<DepartureLocationTiming> ValueRetailRouteFilter(ShoppingPackageRouteDetail shoppingPackage)
        {
            var packagePoutes = _valueRetailPackageRouteRepository.GetByPackageId(shoppingPackage.RouteDetails.Package.PackageID).ToList();

            var packageDistinctRoute = packagePoutes
                .GroupBy(p => new { p.LocationName });

            var distinctRouteResult = new List<DepartureLocationTiming>();

            foreach (var distinctRoute in packageDistinctRoute)
            {
                var distinctDepartureTime = distinctRoute.GroupBy(g => new { g.DepartureTime });

                foreach (var timeGrp in distinctDepartureTime)
                {
                    foreach (var departureTime in timeGrp)
                    {
                        var returnTimeModel = _valueRetailPackageReturnRepository.GetByValueRetailPackageRouteIdAndRouteId(departureTime.Id, departureTime.LinkedRouteId);

                        foreach (var returnTime in returnTimeModel)
                        {
                            distinctRouteResult.Add(new DepartureLocationTiming
                            {
                                DepartureLocation = departureTime.LocationName,
                                DepartureAddress = departureTime.LocationAddress,
                                DepartureTime = departureTime.DepartureTime,
                                ReturnTime = returnTime.ReturnTime,
                                JourneyType = departureTime.JourneyType
                            });
                        }
                    }
                }
            }
            return distinctRouteResult;
        }

        private string StripText(string text)
        {
            //Regex to strip all the texts wrapped in hashtags
            string regexOne = "(\\#[a-zA-Z]+\\#)";
            //string regexTwo = "(#BOLDSTART#)";
            //string regexThree = "(#BOLDEND#)";
            //string regexFour = "(#LINE#)";
            //string outputOne = Regex.Replace(text, regexTwo, "<strong>");
            //string outputTwo = Regex.Replace(outputOne, regexThree, "</strong>");
            string outputThree = Regex.Replace(text, regexOne, "");
            return outputThree;
        }

        public class SaveLocationReturnValues
        {
            public int cityId { get; set; }
            public int currencyId { get; set; }
            public double? lat { get; set; }
            public double? lng { get; set; }
        }

        public class DepartureLocationTiming
        {
            public string DepartureLocation { get; set; }
            public string DepartureAddress { get; set; }
            public string DepartureTime { get; set; }
            public string ReturnTime { get; set; }
            public int JourneyType { get; set; }
        }
    }
}