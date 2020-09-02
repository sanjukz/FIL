using FIL.Api.Integrations;
using FIL.Api.Integrations.ASI;
using FIL.Api.Integrations.HttpHelpers;
using FIL.Api.Repositories;
using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Contracts.Commands.ASI;
using FIL.Contracts.DataModels;
using FIL.Contracts.DataModels.ASI;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Commands;
using FIL.Logging;
using FIL.Logging.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.ASI
{
    public class ASIMonumentCommandHandler : BaseCommandHandlerWithResult<ASIMonumentCommand, ASIMonumentCommandResult>
    {
        private readonly IASIApi _iASIApi;
        private readonly IASIMonumentRepository _aSIMonumentRepository;
        private readonly IASIMonumentHolidayDayRepository _aSIMonumentHolidayDayRepository;
        private readonly IASIMonumentDetailRepository _aSIMonumentDetailRepository;
        private readonly IASIMonumentTicketTypeMappingsRepository _aSIMonumentTicketTypeMappingsRepository;
        private readonly IASIMonumentTimeSlotMappingRepository _aSIMonumentTimeSlotMappingRepository;
        private readonly IASIMonumentWeekOpenDayRepository _aSIMonumentWeekOpenDayRepository;
        private readonly IASITicketTypeRepository _aSITicketTypeRepository;
        private readonly IDaysRepository _daysRepository;
        private readonly IASIMonumentEventTableMappingRepository _aSIMonumentEventTableMappingRepository;
        private readonly IEventTimeSlotMappingRepository _eventTimeSlotMappingRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IStateRepository _stateRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly IEventDeliveryTypeDetailRepository _eventDeliveryTypeDetailRepository;
        private readonly ITicketFeeDetailRepository _ticketFeeDetailRepository;
        private readonly IGoogleMapApi _googleMapApi;
        private readonly ICurrencyTypeRepository _currencyTypeRepository;
        private readonly IEventCategoryRepository _eventCategoryRepository;
        private readonly IPlaceWeekOpenDaysRepository _placeWeekOpenDaysRepository;
        private readonly IPlaceHolidayDatesRepository _placeHolidayDatesRepository;
        private readonly IMediator _mediator;
        private readonly ILogger _logger;
        private readonly ISettings _settings;

        public ASIMonumentCommandHandler(ICountryRepository countryRepository,
            IASIApi iASIApi,
            ICityRepository cityRepository,
            IStateRepository stateRepository,
            IVenueRepository venueRepository,
            IEventRepository eventRepository,
            IEventDetailRepository eventDetailRepository,
            IEventDeliveryTypeDetailRepository eventDeliveryTypeDetailRepository,
            ITicketCategoryRepository ticketCategoryRepository,
            IEventTicketDetailRepository eventTicketDetailRepository,
            IEventTicketAttributeRepository eventTicketAttributeRepository,
            ITicketFeeDetailRepository ticketFeeDetailRepository,
            IASIMonumentRepository aSIMonumentRepository,
            IASIMonumentHolidayDayRepository aSIMonumentHolidayDayRepository,
            IASIMonumentDetailRepository aSIMonumentDetailRepository,
            IASIMonumentTicketTypeMappingsRepository aSIMonumentTicketTypeMappingsRepository,
            IASIMonumentTimeSlotMappingRepository aSIMonumentTimeSlotMappingRepository,
            IASIMonumentWeekOpenDayRepository aSIMonumentWeekOpenDayRepository,
            IASITicketTypeRepository aSITicketTypeRepository,
            IDaysRepository daysRepository,
            IASIMonumentEventTableMappingRepository aSIMonumentEventTableMappingRepository,
            IEventTimeSlotMappingRepository eventTimeSlotMappingRepository,
            IGoogleMapApi googleMapApi,
            ICurrencyTypeRepository currencyTypeRepository,
            IEventCategoryRepository eventCategory,
            IPlaceWeekOpenDaysRepository placeWeekOpenDaysRepository,
            IPlaceHolidayDatesRepository placeHolidayDatesRepository,
            ILogger logger,
            ISettings settings,
            IMediator mediator) : base(mediator)
        {
            _countryRepository = countryRepository;
            _iASIApi = iASIApi;
            _cityRepository = cityRepository;
            _stateRepository = stateRepository;
            _aSIMonumentRepository = aSIMonumentRepository;
            _aSIMonumentHolidayDayRepository = aSIMonumentHolidayDayRepository;
            _aSIMonumentDetailRepository = aSIMonumentDetailRepository;
            _aSIMonumentTicketTypeMappingsRepository = aSIMonumentTicketTypeMappingsRepository;
            _aSIMonumentTimeSlotMappingRepository = aSIMonumentTimeSlotMappingRepository;
            _aSIMonumentWeekOpenDayRepository = aSIMonumentWeekOpenDayRepository;
            _aSITicketTypeRepository = aSITicketTypeRepository;
            _daysRepository = daysRepository;
            _aSIMonumentEventTableMappingRepository = aSIMonumentEventTableMappingRepository;
            _eventTimeSlotMappingRepository = eventTimeSlotMappingRepository;
            _eventRepository = eventRepository;
            _venueRepository = venueRepository;
            _eventDetailRepository = eventDetailRepository;
            _eventDeliveryTypeDetailRepository = eventDeliveryTypeDetailRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _ticketFeeDetailRepository = ticketFeeDetailRepository;
            _googleMapApi = googleMapApi;
            _currencyTypeRepository = currencyTypeRepository;
            _eventCategoryRepository = eventCategory;
            _placeWeekOpenDaysRepository = placeWeekOpenDaysRepository;
            _placeHolidayDatesRepository = placeHolidayDatesRepository;
            _mediator = mediator;
            _logger = logger;
            _settings = settings;
        }

        public void createASITicketTypeMappings(
            long ASIMonumentDetailId,
            decimal AC,
            decimal ASI,
            decimal LDA,
            decimal MSM,
            decimal Others,
            decimal Total,
            bool isEnabled,
            int ASITicketTypeId
            )
        {
            try
            {
                var asiMonumentDetailTicketTypeModel = new ASIMonumentTicketTypeMapping
                {
                    ASIMonumentDetailId = ASIMonumentDetailId,
                    IsEnabled = true,
                    AC = AC,
                    ASI = ASI,
                    ASITicketTypeId = ASITicketTypeId,
                    LDA = LDA,
                    MSM = MSM,
                    Others = Others,
                    Total = Total,

                    CreatedUtc = DateTime.UtcNow,
                    UpdatedUtc = DateTime.UtcNow
                };
                _aSIMonumentTicketTypeMappingsRepository.Save(asiMonumentDetailTicketTypeModel);
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to run ASI createASITicketTypeMappings function", ex));
            }
        }

        public void createASIDetailAndTicketTypeMappings(
            FIL.Contracts.DataModels.ASI.ASIMonument aSIMonument,
            string curentOption,
            List<Contracts.Models.ASI.Amount> amounts,
            List<Contracts.Models.ASI.Amount2> optioanlAmount,
            bool isOptional
            )
        {
            try
            {
                var asiMonumentDetailModel = new Contracts.DataModels.ASI.ASIMonumentDetail
                {
                    ASIMonumentId = aSIMonument.Id,
                    IsEnabled = true,
                    IsOptional = isOptional,
                    Name = curentOption,
                    CreatedUtc = DateTime.UtcNow,
                    UpdatedUtc = DateTime.UtcNow
                };
                var savedAsiMonumentDetail = _aSIMonumentDetailRepository.Save(asiMonumentDetailModel);
                var allAsiTicketTypes = _aSITicketTypeRepository.GetAll();
                if (isOptional)
                {
                    foreach (FIL.Contracts.Models.ASI.Amount2 currentAmount in optioanlAmount)
                    {
                        var ticketType = allAsiTicketTypes.Where(s => s.Name == currentAmount.Type).FirstOrDefault();
                        var mainTicketAmount = amounts.Where(s => s.Type == currentAmount.Type).FirstOrDefault();
                        if (mainTicketAmount != null && ticketType != null) // IT SHOULD BE TRUE ALWAYS
                        {
                            createASITicketTypeMappings(savedAsiMonumentDetail.Id,
                                0,
                                currentAmount.ASI,
                                0,
                                0,
                                0,
                                currentAmount.Total + mainTicketAmount.Total,
                                true,
                                ticketType.Id
                                );
                        }
                    }
                }
                else
                {
                    foreach (FIL.Contracts.Models.ASI.Amount currentAmount in amounts)
                    {
                        var ticketType = allAsiTicketTypes.Where(s => s.Name == currentAmount.Type).FirstOrDefault();
                        if (ticketType != null) // IT SHOULD BE TRUE ALWAYS
                        {
                            createASITicketTypeMappings(savedAsiMonumentDetail.Id,
                              currentAmount.AC,
                              currentAmount.ASI,
                              currentAmount.LDA,
                              currentAmount.MSM,
                              currentAmount.others,
                              currentAmount.Total,
                              true,
                              ticketType.Id
                              );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to run ASI createASIDetailAndTicketTypeMappings function", ex));
            }
        }

        public void updateASITicketType(
            FIL.Contracts.DataModels.ASI.ASIMonumentDetail aSIMonumentDetail,
            List<Contracts.Models.ASI.Amount> amounts,
            List<Contracts.Models.ASI.Amount2> optioanlAmount,
            bool isOptional
            )
        {
            try
            {
                var aSITicketTypeDetails = _aSIMonumentTicketTypeMappingsRepository.GetByASIDetailId(aSIMonumentDetail.Id);
                foreach (FIL.Contracts.DataModels.ASI.ASIMonumentTicketTypeMapping currentTicketType in aSITicketTypeDetails) // DISABLE ALL TICKET TYPE INITIALLY
                {
                    currentTicketType.IsEnabled = false;
                    _aSIMonumentTicketTypeMappingsRepository.Save(currentTicketType);
                }
                if (isOptional)
                {
                    foreach (FIL.Contracts.Models.ASI.Amount2 currentAmount in optioanlAmount)
                    {
                        var ticketType = _aSITicketTypeRepository.GetByName(currentAmount.Type);
                        var mainTicketAmount = amounts.Where(s => s.Type == currentAmount.Type).FirstOrDefault();
                        if (ticketType != null && mainTicketAmount != null) // IT SHOULD BE TRUE ALWAYS
                        {
                            var currentASITicketType = aSITicketTypeDetails.Where(s => s.ASITicketTypeId == ticketType.Id).FirstOrDefault();
                            if (currentASITicketType != null) // IT SHOULD BE TRUE ALWAYS
                            {
                                currentASITicketType.Total = currentAmount.Total + mainTicketAmount.Total;
                                currentASITicketType.ASI = currentAmount.ASI;
                                currentASITicketType.IsEnabled = true;
                                _aSIMonumentTicketTypeMappingsRepository.Save(currentASITicketType);
                            }
                        }
                        else // IF NEW CATEGORY FOR EXISTING ASI DETAIL..... THIS WILL NOT TRUE ALWAYS...
                        {
                            createASITicketTypeMappings(aSIMonumentDetail.Id,
                             0,
                             currentAmount.ASI,
                             0,
                             0,
                             0,
                              currentAmount.Total + mainTicketAmount.Total,
                             true,
                             ticketType.Id
                             );
                        }
                    }
                }
                else
                {
                    foreach (FIL.Contracts.Models.ASI.Amount currentAmount in amounts)
                    {
                        var ticketType = _aSITicketTypeRepository.GetByName(currentAmount.Type);
                        if (ticketType != null)
                        {
                            var currentASITicketType = aSITicketTypeDetails.Where(s => s.ASITicketTypeId == ticketType.Id).FirstOrDefault();
                            if (currentASITicketType != null) // IT SHOULD BE TRUE ALWAYS
                            {
                                currentASITicketType.Total = currentAmount.Total;
                                currentASITicketType.IsEnabled = true;
                                currentASITicketType.ASI = currentAmount.ASI;
                                currentASITicketType.LDA = currentAmount.LDA;
                                currentASITicketType.AC = currentAmount.AC;
                                currentASITicketType.Others = currentAmount.others;
                                currentASITicketType.MSM = currentAmount.MSM;
                                _aSIMonumentTicketTypeMappingsRepository.Save(currentASITicketType);
                            }
                            else // IF NEW CATEGORY FOR EXISTING ASI DETAIL THIS WILL NOT TRUE ALWAYA
                            {
                                createASITicketTypeMappings(aSIMonumentDetail.Id,
                             currentAmount.AC,
                             currentAmount.ASI,
                             currentAmount.LDA,
                             currentAmount.MSM,
                             currentAmount.others,
                             currentAmount.Total,
                             true,
                             ticketType.Id
                             );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to run ASI updateASITicketType function", ex));
            }
        }

        public ASIMonumentWeekOpenDay createASIWeekOpenDaysMappings(
            FIL.Contracts.DataModels.ASI.ASIMonument aSIMonument,
            FIL.Contracts.DataModels.Days day,
            bool isOpen
            )
        {
            var aSiMonumentWeekOpenDay = new ASIMonumentWeekOpenDay
            {
                Id = 0,
                ASIMonumentId = aSIMonument.Id,
                DayId = day.Id,
                IsEnabled = isOpen,
                CreatedUtc = DateTime.UtcNow,
                UpdatedUtc = DateTime.UtcNow
            };
            var savedASIMonumentWeekOpenDay = _aSIMonumentWeekOpenDayRepository.Save(aSiMonumentWeekOpenDay);
            return savedASIMonumentWeekOpenDay;
        }

        public ASIMonumentTimeSlotMapping createASITimingMappings(
          FIL.Contracts.DataModels.ASI.ASIMonument aSIMonument,
          string name,
          int timeSlotId,
          string startTime,
          string endTime
          )
        {
            var aSiMonumentTimeSlot = new ASIMonumentTimeSlotMapping
            {
                Id = 0,
                ASIMonumentId = aSIMonument.Id,
                Name = name,
                TimeSlotId = timeSlotId,
                EndTime = endTime,
                StartTime = startTime,
                IsEnabled = true,
                CreatedUtc = DateTime.UtcNow,
                UpdatedUtc = DateTime.UtcNow
            };
            var savedTimeSlot = _aSIMonumentTimeSlotMappingRepository.Save(aSiMonumentTimeSlot);
            return savedTimeSlot;
        }

        public void createASIHolidayMappings(
          FIL.Contracts.DataModels.ASI.ASIMonument aSIMonument,
          DateTime date
          )
        {
            var aSiMonumentHolidaySlot = new ASIMonumentHolidayDay
            {
                Id = 0,
                ASIMonumentId = aSIMonument.Id,
                HolidayDate = date,
                IsEnabled = true,
                CreatedUtc = DateTime.UtcNow,
                UpdatedUtc = DateTime.UtcNow
            };
            _aSIMonumentHolidayDayRepository.Save(aSiMonumentHolidaySlot);
        }

        public ASIMonumentEventTableMapping createAndUpdateASIEventTableMappingAndEventTable(
            FIL.Contracts.DataModels.ASI.ASIMonument aSIMonument
            )
        {
            try
            {
                var currentMappingTable = _aSIMonumentEventTableMappingRepository.GetByMonumentId(aSIMonument.Id);
                if (currentMappingTable != null && aSIMonument.IsEnabled) // CHECK IS THE MONUMENT IS ACTIVE
                {
                    currentMappingTable.IsEnabled = true;
                    _aSIMonumentEventTableMappingRepository.Save(currentMappingTable); // UPDATE MAPPING TABLE AND MAKE CURRENT ENRTY ENABLE
                }
                if (currentMappingTable != null)
                {
                    var eventTable = _eventRepository.Get(currentMappingTable.EventId);
                    if (eventTable != null && eventTable.IsEnabled && !currentMappingTable.IsEnabled) // IF MAPPING TABLE ENTRY IS DISABLED AND MAIN EVENT TABLE ENTRY IS ENABLED THEN DISABLE MAIN EVENT TABLE
                    {
                        eventTable.IsEnabled = false;
                        _eventRepository.Save(eventTable);
                    }
                }
                else if (currentMappingTable == null && aSIMonument.IsEnabled) // IS MONUMENT IS ACTIVE THEN ONLT ADD THE ENTRIES TO BOTH MAPPING AND EVENT TABLE
                {
                    var eventCategory = _eventCategoryRepository.GetByNameAndNonFeel(FIL.Contracts.Utils.Constant.ASIConstant.Category, false);
                    if (eventCategory != null)
                    {
                        var eventModel = new FIL.Contracts.DataModels.Event
                        {
                            Id = 0,
                            AltId = Guid.NewGuid(),
                            IsCreatedFromFeelAdmin = false,
                            EventSourceId = EventSource.ASI,
                            Slug = "asi-" + aSIMonument.Name.ToLower().Replace(' ', '-'),
                            IsFeel = false,
                            ClientPointOfContactId = 1,
                            EventCategoryId = eventCategory.Id,
                            TermsAndConditions = "<p>Tickets once booked cannot be refunded</p>",
                            EventTypeId = EventType.Perennial,
                            IsPublishedOnSite = true,
                            Name = aSIMonument.Name,
                            Description = aSIMonument.Comment,
                            IsEnabled = true,
                            CreatedUtc = DateTime.UtcNow,
                            UpdatedUtc = DateTime.UtcNow
                        };
                        var savedEvent = _eventRepository.Save(eventModel); // Create Event....

                        var ASIMappingTable = new ASIMonumentEventTableMapping
                        {
                            Id = 0,
                            EventId = savedEvent.Id,
                            ASIMonumentId = aSIMonument.Id,
                            IsEnabled = true,
                            CreatedUtc = DateTime.UtcNow,
                            UpdatedUtc = DateTime.UtcNow
                        };
                        currentMappingTable = _aSIMonumentEventTableMappingRepository.Save(ASIMappingTable); // Create Event....
                    }
                }
                return currentMappingTable;
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to run ASI createAndUpdateASIEventTableMappingAndEventTable function", ex));
                return null;
            }
        }

        public City CreateCity(
            string cityName,
            int stateId
            )
        {
            var city = _cityRepository.Save(new City
            {
                Name = cityName,
                StateId = stateId,
                IsEnabled = true,
                CreatedUtc = DateTime.UtcNow,
                UpdatedUtc = DateTime.UtcNow
            });
            var currentCity = _cityRepository.Save(city);
            return currentCity;
        }

        public City getCity(
            FIL.Contracts.DataModels.ASI.ASIMonument aSIMonument
            )
        {
            var country = _countryRepository.GetByName(FIL.Contracts.Utils.Constant.ASIConstant.CountryName); // COUNTRY NOT COMING FROM API BUT IT'S LIMITED TO INDIA ONLY....
            var states = _stateRepository.GetAllByCountryId(country.Id);
            var city = _cityRepository.GetByNameAndStateIds(aSIMonument.Circle.Trim(), states.Select(s => s.Id)); // CHECK FOR CITY BELONG TO THE STATES IN INDIA
            if (city != null)
            {
                return city;
            }
            else // USUALLY IT WILL NOT GO TO THIS ELSE AS THERE ARE SELECTED & POPULAR CITIES IN ASI WHICH ARE IN DB ALREADY
            {
                var googleService = _googleMapApi.GetLatLongFromAddress(aSIMonument.Circle); // CALL THE GOOGLE SERVICE TO RETUTRN THE CITY, STATE DATA
                if (!string.IsNullOrWhiteSpace(googleService.Result.Result.StateName))
                {
                    var state = _stateRepository.GetByNameAndCountryId(googleService.Result.Result.StateName, country.Id);
                    if (state != null)
                    {
                        var savedCity = CreateCity(aSIMonument.Circle, state.Id);
                        return savedCity;
                    }
                    else
                    {
                        var newState = _stateRepository.Save(new State
                        {
                            Name = googleService.Result.Result.StateName,
                            CountryId = country.Id,
                            IsEnabled = true,
                            CreatedUtc = DateTime.UtcNow,
                            UpdatedUtc = DateTime.UtcNow
                        });
                        var currentState = _stateRepository.Save(newState);
                        var savedCity = CreateCity(aSIMonument.Circle, currentState.Id);
                        return savedCity;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        public Venue getVenue(
            FIL.Contracts.DataModels.ASI.ASIMonument aSIMonument,
            City city
            )
        {
            try
            {
                var venue = _venueRepository.GetByVenueNameAndCityId(aSIMonument.Name, city.Id);
                if (venue != null)
                {
                    return venue;
                }
                else
                {
                    var newVenue = _venueRepository.Save(new Venue
                    {
                        AltId = Guid.NewGuid(),
                        Name = aSIMonument.Name,
                        AddressLineOne = aSIMonument.Name,
                        CityId = city.Id,
                        IsEnabled = true,
                        CreatedUtc = DateTime.UtcNow,
                        UpdatedUtc = DateTime.UtcNow
                    });
                    var savedVenue = _venueRepository.Save(newVenue);
                    return savedVenue;
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to run ASI getVenue function", ex));
                return null;
            }
        }

        public TicketCategory getTicketCategory(
           FIL.Contracts.DataModels.ASI.ASIMonumentTicketTypeMapping aSIMonumentTicketTypeMapping
           )
        {
            var asiTicketType = _aSITicketTypeRepository.Get(aSIMonumentTicketTypeMapping.ASITicketTypeId);
            if (asiTicketType != null)
            {
                var ticketCategory = _ticketCategoryRepository.GetByName(asiTicketType.Name);
                return ticketCategory;
            }
            else // IT WILL NOT GO ELSE AS CATEGORY CREATED ALREADY
            {
                var categotry = createTicketCategories(asiTicketType);
                return categotry;
            }
        }

        public EventTicketDetail createEventTicketDetail(
           TicketCategory ticketCategory,
           EventDetail eventDetail
          )
        {
            var eventTicketDetail = _eventTicketDetailRepository.Save(new EventTicketDetail
            {
                EventDetailId = eventDetail.Id,
                TicketCategoryId = ticketCategory.Id,
                IsEnabled = true,
                CreatedUtc = DateTime.UtcNow,
                UpdatedUtc = DateTime.UtcNow
            });
            return eventTicketDetail;
        }

        public EventTicketAttribute createEventTicketAttribute(
           EventTicketDetail eventTicketDetail,
           FIL.Contracts.DataModels.ASI.ASIMonument aSIMonument,
           FIL.Contracts.DataModels.ASI.ASIMonumentTicketTypeMapping aSIMonumentTicketTypeMapping
          )
        {
            var curency = _currencyTypeRepository.GetByCurrencyCode(FIL.Contracts.Utils.Constant.ASIConstant.CurrencyCode);
            var eventTicketAttribute = _eventTicketAttributeRepository.Save(new EventTicketAttribute
            {
                EventTicketDetailId = eventTicketDetail.Id,
                SalesStartDateTime = DateTime.UtcNow,
                SalesEndDatetime = aSIMonument.MaxDate,
                TicketTypeId = TicketType.Regular,
                ChannelId = Channels.Website,
                CurrencyId = curency.Id,
                SharedInventoryGroupId = null,
                TicketCategoryDescription = "",
                ViewFromStand = "",
                IsSeatSelection = false,
                AvailableTicketForSale = 5000,
                RemainingTicketForSale = 5000,
                Price = aSIMonumentTicketTypeMapping.Total,
                IsInternationalCardAllowed = false,
                IsEMIApplicable = false,
                IsEnabled = true,
                CreatedUtc = DateTime.UtcNow,
                UpdatedUtc = DateTime.UtcNow
            });
            var savedETA = _eventTicketAttributeRepository.Save(eventTicketAttribute);
            return savedETA;
        }

        public EventDetail CreateEventDetail(
             FIL.Contracts.DataModels.ASI.ASIMonument aSIMonument,
             FIL.Contracts.DataModels.ASI.ASIMonumentDetail aSIMonumentDetail,
             FIL.Contracts.DataModels.Event currentEvent,
            Venue venue
            )
        {
            if (venue != null)
            {
                var eventDetail = _eventDetailRepository.Save(new EventDetail
                {
                    Name = aSIMonumentDetail.Name,
                    EventId = currentEvent.Id,
                    VenueId = venue.Id,
                    StartDateTime = DateTime.UtcNow,
                    EndDateTime = aSIMonument.MaxDate,
                    GroupId = 1,
                    AltId = Guid.NewGuid(),
                    TicketLimit = 1000,
                    MetaDetails = "",
                    HideEventDateTime = false,
                    CustomDateTimeMessage = "",
                    IsEnabled = true,
                    CreatedUtc = DateTime.UtcNow,
                    UpdatedUtc = DateTime.UtcNow
                });
                _eventDetailRepository.Save(eventDetail);
                return eventDetail;
            }
            else
            {
                return null;
            }
        }

        public void CreateEventDeliveryTypeDetails(
             EventDetail eventDetail,
            FIL.Contracts.DataModels.ASI.ASIMonument aSIMonument
            )
        {
            _eventDeliveryTypeDetailRepository.Save(new EventDeliveryTypeDetail
            {
                EventDetailId = eventDetail.Id,
                DeliveryTypeId = DeliveryTypes.PrintAtHome,
                Notes = "",
                EndDate = aSIMonument.MaxDate,
                IsEnabled = true,
                RefundPolicy = 4,
                CreatedUtc = DateTime.UtcNow,
                UpdatedUtc = DateTime.UtcNow
            });
        }

        public void CreateAndUpdateEventWeekOpenDays(
           FIL.Contracts.DataModels.ASI.ASIMonumentWeekOpenDay aSIMonumentWeekOpenDay,
            FIL.Contracts.DataModels.Event currentEvent
           )
        {
            var placeWeekOpen = _placeWeekOpenDaysRepository.GetByEventIdandDayId(currentEvent.Id, aSIMonumentWeekOpenDay.DayId);
            if (placeWeekOpen != null)
            {
                placeWeekOpen.IsEnabled = placeWeekOpen.IsEnabled;
                _placeWeekOpenDaysRepository.Save(placeWeekOpen);
            }
            else
            {
                _placeWeekOpenDaysRepository.Save(new PlaceWeekOpenDays
                {
                    Id = 0,
                    AltId = Guid.NewGuid(),
                    DayId = aSIMonumentWeekOpenDay.DayId,
                    IsEnabled = aSIMonumentWeekOpenDay.IsEnabled,
                    EventId = currentEvent.Id,
                    IsSameTime = true,

                    CreatedUtc = DateTime.UtcNow,
                    UpdatedUtc = DateTime.UtcNow
                });
            }
        }

        public void CreateAndUpdateEventTimeSlotMappings(
          FIL.Contracts.DataModels.ASI.ASIMonumentTimeSlotMapping aSIMonumentTimeSlotMapping,
          FIL.Contracts.DataModels.Event currentEvent
          )
        {
            var eventTimeSlot = _eventTimeSlotMappingRepository.GetByEventIdandTimeSlotId(currentEvent.Id, aSIMonumentTimeSlotMapping.TimeSlotId);
            if (eventTimeSlot != null)
            {
                eventTimeSlot.IsEnabled = eventTimeSlot.IsEnabled;
                _eventTimeSlotMappingRepository.Save(eventTimeSlot);
            }
            else
            {
                _eventTimeSlotMappingRepository.Save(new Contracts.DataModels.ASI.EventTimeSlotMapping
                {
                    Id = 0,
                    TimeSlotId = aSIMonumentTimeSlotMapping.TimeSlotId,
                    IsEnabled = true,
                    EventId = currentEvent.Id,
                    EndTime = aSIMonumentTimeSlotMapping.EndTime,
                    Name = aSIMonumentTimeSlotMapping.Name,
                    StartTime = aSIMonumentTimeSlotMapping.StartTime,

                    CreatedUtc = DateTime.UtcNow,
                    UpdatedUtc = DateTime.UtcNow
                });
            }
        }

        public void CreatePlaceHolidayDates(
             DateTime holidayDate,
             FIL.Contracts.DataModels.Event currentEvent
             )
        {
            _placeHolidayDatesRepository.Save(new PlaceHolidayDate
            {
                Id = 0,
                EventId = currentEvent.Id,
                IsEnabled = true,
                LeaveDateTime = holidayDate,

                CreatedUtc = DateTime.UtcNow,
                UpdatedUtc = DateTime.UtcNow
            });
        }

        public void updateEDAndETDAndETA(
            FIL.Contracts.DataModels.ASI.ASIMonumentDetail aSIMonumentDetail,
            FIL.Contracts.DataModels.ASI.ASIMonument aSIMonument,
            EventDetail eventDetail
            )
        {
            try
            {
                var asiMonumentTicketDetails = _aSIMonumentTicketTypeMappingsRepository.GetByASIDetailId(aSIMonumentDetail.Id);
                var eventTicketDetailData = _eventTicketDetailRepository.GetByEventDetailId(eventDetail.Id);
                foreach (FIL.Contracts.DataModels.EventTicketDetail eventTicketDetail in eventTicketDetailData) // DISABLE ALL ETD AND ETA
                {
                    eventTicketDetail.IsEnabled = false;
                    _eventTicketDetailRepository.Save(eventTicketDetail);
                    var eventTicketAttributes = _eventTicketAttributeRepository.GetByEventTicketDetailId(eventTicketDetail.Id);
                    if (eventTicketAttributes != null)
                    {
                        eventTicketAttributes.IsEnabled = false;
                        _eventTicketAttributeRepository.Save(eventTicketAttributes);
                    }
                }
                foreach (FIL.Contracts.DataModels.ASI.ASIMonumentTicketTypeMapping currentTicketType in asiMonumentTicketDetails) // LOOK FOR ASI MONUMENT TICKET DETAILS
                {
                    var mainTIcketCateory = getTicketCategory(currentTicketType);
                    var filteredTicketCat = eventTicketDetailData.Where(s => s.TicketCategoryId == mainTIcketCateory.Id).FirstOrDefault();

                    if (filteredTicketCat != null && currentTicketType.IsEnabled) // IF ASI IS ENABLED THEN UPDATE ETD AND ETA
                    {
                        var eventTicketAttributes = _eventTicketAttributeRepository.GetByEventTicketDetailId(filteredTicketCat.Id);
                        if (eventTicketAttributes != null)
                        {
                            eventTicketAttributes.Price = currentTicketType.Total;
                            eventTicketAttributes.SalesEndDatetime = aSIMonument.MaxDate;
                            eventTicketAttributes.SalesStartDateTime = DateTime.UtcNow;
                            eventTicketAttributes.UpdatedUtc = DateTime.UtcNow;
                            eventTicketAttributes.IsEnabled = true;
                            _eventTicketAttributeRepository.Save(eventTicketAttributes);
                        }
                        filteredTicketCat.IsEnabled = true;
                        _eventTicketDetailRepository.Save(filteredTicketCat);
                    }
                    else if (filteredTicketCat == null)  // IF CATEGORY NOT EXISTS IN MAIN ETD TABLE BUT IN ASI TABLE THEN CREATE THE CATEGORY IN MAIN TABLE
                    {
                        var currentEventTicketDetail = createEventTicketDetail(mainTIcketCateory, eventDetail); // CREATE EVENTTICKET DETAIL
                        var currentEventTicketAttributes = createEventTicketAttribute(currentEventTicketDetail, aSIMonument, currentTicketType); // CREATE EVENTTICKETATTRIBUTES
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to run ASI updateEDAndETDAndETA function", ex));
            }
        }

        public void createEDAndETDAndETAAndVenueAndCityAndState(
           FIL.Contracts.DataModels.ASI.ASIMonument aSIMonument
           )
        {
            try
            {
                var currentMappingTable = _aSIMonumentEventTableMappingRepository.GetByMonumentId(aSIMonument.Id);
                if (currentMappingTable != null && currentMappingTable.IsEnabled) // THIS SHOULD BE ALWAYS TRUE
                {
                    var eventData = _eventRepository.Get(currentMappingTable.EventId);
                    if (eventData != null) // THIS SHOULD BE ALWAYS TRUE
                    {
                        if (!eventData.IsEnabled) // IF MAIN EVENT TABLE IS DISABLED ALREADY THEN ENABLE THAT
                        {
                            eventData.IsEnabled = true;
                            _eventRepository.Save(eventData);
                        }
                        var eventDetails = _eventDetailRepository.GetSubeventByEventId(eventData.Id);
                        var asiMonumentDetails = _aSIMonumentDetailRepository.GetByMonumentId(aSIMonument.Id).Where(s => s.IsEnabled); // TAKE ONLY ENABLED ASI MONUMENT DETAILS
                        foreach (FIL.Contracts.DataModels.EventDetail currentEventDetail in eventDetails) // DISABLE ALL EVENT DETAILS OF CURRENT EVENT
                        {
                            currentEventDetail.IsEnabled = false;
                            _eventDetailRepository.Save(currentEventDetail);
                        }
                        foreach (FIL.Contracts.DataModels.ASI.ASIMonumentDetail currentMonumentDetail in asiMonumentDetails)
                        {
                            var currentEventDetail = _eventDetailRepository.GetByEventIdAndName(eventData.Id, currentMonumentDetail.Name);
                            if (currentEventDetail != null)
                            {
                                currentEventDetail.IsEnabled = true;
                                _eventDetailRepository.Save(currentEventDetail);
                                updateEDAndETDAndETA(currentMonumentDetail, aSIMonument, currentEventDetail); // UPDATE MAIN ETD AND ETA...
                            }
                            else
                            {
                                var city = getCity(aSIMonument); // GET MAIN CITY
                                if (city != null)
                                {
                                    var venue = getVenue(aSIMonument, city);  // GET MAIN VENUE
                                    if (venue != null)
                                    {
                                        currentEventDetail = CreateEventDetail(aSIMonument, currentMonumentDetail, eventData, venue); // CREATE EVENT DETAILS
                                        if (currentEventDetail != null)
                                        {
                                            var asiMonumentTicketDetails = _aSIMonumentTicketTypeMappingsRepository.GetByASIDetailId(currentMonumentDetail.Id).Where(s => s.IsEnabled); // TAKE ONLY ENABLED ASI MONUMENT TICKET DETAILS
                                            foreach (FIL.Contracts.DataModels.ASI.ASIMonumentTicketTypeMapping currentTicketType in asiMonumentTicketDetails)
                                            {
                                                var currentTicketCategory = getTicketCategory(currentTicketType);  // GET MAIN TICKET CATEGORY
                                                var currentEventTicketDetail = createEventTicketDetail(currentTicketCategory, currentEventDetail); // CREATE EVENTTICKET DETAIL
                                                var currentEventTicketAttributes = createEventTicketAttribute(currentEventTicketDetail, aSIMonument, currentTicketType); // CREATE EVENTTICKETATTRIBUTES
                                            }
                                        }
                                    }
                                }
                            }
                            CreateEventDeliveryTypeDetails(currentEventDetail, aSIMonument);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to run ASI createAndUpdateASIEventTableMappingAndEventTable function", ex));
            }
        }

        public TicketCategory createTicketCategories(
            FIL.Contracts.DataModels.ASI.ASITicketType aSITicketType
            )
        {
            var ticketCat = _ticketCategoryRepository.GetByName(aSITicketType.Name);
            if (ticketCat == null)
            {
                var ticketCategory = _ticketCategoryRepository.Save(new TicketCategory
                {
                    Id = 0,
                    Name = aSITicketType.Name,
                    IsEnabled = true,

                    CreatedUtc = DateTime.UtcNow,
                    UpdatedUtc = DateTime.UtcNow
                });
                var savedTicketCategory = _ticketCategoryRepository.Save(ticketCategory);
                return savedTicketCategory;
            }
            else
            {
                return ticketCat;
            }
        }

        public void checkFinalActiveMonument()
        {
            var allASIMonuments = _eventRepository.GetBySourceId(EventSource.ASI);
            foreach (FIL.Contracts.DataModels.Event currentEvent in allASIMonuments)
            {
                var aSiMonumentMappingTable = _aSIMonumentEventTableMappingRepository.GetByEventId(currentEvent.Id);
                if (aSiMonumentMappingTable != null && !aSiMonumentMappingTable.IsEnabled && currentEvent.IsEnabled) // CHECK FOR MONUMENT-EVENT MAPPING TABLE IF IT'S DISABLED AND MAIN TABLE IS ENABLED THEN THEN DISABLE FROM EVENT TABLE
                {
                    currentEvent.IsEnabled = false;
                    _eventRepository.Save(currentEvent);
                }
                if (aSiMonumentMappingTable != null && aSiMonumentMappingTable.IsEnabled && !currentEvent.IsEnabled) // CHECK FOR MONUMENT-EVENT MAPPING TABLE IF IT'S ENABLED BUT THE EVENT TABLE IS DISABLED THEN ENABLE FROM EVENT TABLE
                {
                    currentEvent.IsEnabled = true;
                    _eventRepository.Save(currentEvent);
                }
                if (aSiMonumentMappingTable == null) // IF NO ENTRY IN MAPPING TABLE THE  BUT ENTRY IN MAIN EVENT TABLE THEN DISABLE FROM MAIN EVENT TABLE.....THIS WILL NOT HAPPEN
                {
                    currentEvent.IsEnabled = false;
                    _eventRepository.Save(currentEvent);
                }
            }
        }

        protected override async Task<ICommandResult> Handle(ASIMonumentCommand command)
        {
            ASIMonumentCommandResult aSIMonument = new ASIMonumentCommandResult();
            try
            {
                var baseAddress = new Uri(_settings.GetConfigSetting<string>(SettingKeys.Integration.ASI.MonumentEndPoint)); // GET ALL MONUMENTS API
                var monumentAPIResponse = await _iASIApi.GetAsiDetails(baseAddress.ToString());
                var monumentData = Mapper<FIL.Contracts.Models.ASI.ASIMonument>.MapJsonStringToObject(monumentAPIResponse.Result);
                //  monumentData.Data.Items = monumentData.Data.Items.Skip(2).ToList(); // SKIP 2
                monumentData.Data.Items = monumentData.Data.Items.Take(5).ToList(); // TAKE NEXT 5 ONLY JUST FOR TESTING
                if (monumentData.IsSuccess)
                {
                    var allMonuments = _aSIMonumentRepository.GetAll();
                    foreach (FIL.Contracts.DataModels.ASI.ASIMonument currentMonument in allMonuments) // INITIALLY DISABLE ALL MONUMENTS
                    {
                        currentMonument.IsEnabled = false;
                        _aSIMonumentRepository.Save(currentMonument);
                    }

                    var allMappingMonuments = _aSIMonumentEventTableMappingRepository.GetAll();
                    foreach (FIL.Contracts.DataModels.ASI.ASIMonumentEventTableMapping currentMonument in allMappingMonuments) // INITIALLY DISABLE ALL MAPPING TABLE DATA
                    {
                        currentMonument.IsEnabled = false;
                        _aSIMonumentEventTableMappingRepository.Save(currentMonument);
                    }
                    var index = 0;
                    foreach (FIL.Contracts.Models.ASI.Item currentMonument in monumentData.Data.Items)
                    {
                        index = index + 1;
                        var monumentDetailAPIResponse = await _iASIApi.GetAsiDetails(baseAddress.ToString() + "/" + currentMonument.Code); // MONUMENT DETAIL API CALL BY CODE
                        var monumentDetailData = Mapper<FIL.Contracts.Models.ASI.ASIMonumentDetail>.MapJsonStringToObject(monumentDetailAPIResponse.Result);
                        if (monumentDetailData.IsSuccess)
                        {
                            Console.WriteLine("Item Index" + index + " Monument Index " + monumentDetailData.Data.Id + " " + monumentDetailData.Data.Code);
                            // ------------------------------ ASI MONUMENT ------------------------- //
                            var asiMonument = _aSIMonumentRepository.GetByName(currentMonument.Name);
                            if (asiMonument != null)
                            {
                                asiMonument.AppConfigVersion = currentMonument.AppConfigVersion;
                                asiMonument.Circle = currentMonument.Circle;
                                asiMonument.Code = currentMonument.Code;
                                asiMonument.Comment = currentMonument.Comment;
                                asiMonument.IsEnabled = currentMonument.Status == "active" ? true : false;
                                asiMonument.IsOptional = monumentDetailData.Data.Configuration.Optionals.Any() ? true : false;
                                asiMonument.MonumentId = currentMonument.Id;
                                asiMonument.Name = currentMonument.Name;
                                asiMonument.MaxDate = monumentDetailData.Data.Availability.MaxDate;
                                asiMonument.Status = currentMonument.Status;
                                asiMonument.Version = currentMonument.Version;
                                _aSIMonumentRepository.Save(asiMonument);
                            }
                            else
                            {
                                var asiMonumentModel = new Contracts.DataModels.ASI.ASIMonument
                                {
                                    Id = 0,
                                    AppConfigVersion = currentMonument.AppConfigVersion,
                                    Circle = currentMonument.Circle,
                                    Code = currentMonument.Code,
                                    Comment = currentMonument.Comment,
                                    IsEnabled = currentMonument.Status == "active" ? true : false,
                                    IsOptional = monumentDetailData.Data.Configuration.Optionals.Any() ? true : false,
                                    MonumentId = currentMonument.Id,
                                    Name = currentMonument.Name,
                                    MaxDate = monumentDetailData.Data.Availability.MaxDate,
                                    Status = currentMonument.Status,
                                    Version = currentMonument.Version,
                                    CreatedUtc = DateTime.UtcNow,
                                    UpdatedUtc = DateTime.UtcNow
                                };
                                asiMonument = _aSIMonumentRepository.Save(asiMonumentModel);
                            }

                            // ------------------------------ CREATE AND UPDATE MAPPING TABLE AND MAIN EVENT TABLE ------------------------- //
                            var aSIMappingTable = createAndUpdateASIEventTableMappingAndEventTable(asiMonument);

                            // ------------------------------ ASI TICKET TYPE ------------------------- //
                            foreach (FIL.Contracts.Models.ASI.Amount amount in monumentDetailData.Data.Configuration.Amounts)
                            {
                                var asiMonumentTicketType = _aSITicketTypeRepository.GetByName(amount.Type);
                                if (asiMonumentTicketType == null)
                                {
                                    var asiMonumentTicketTypeModel = new ASITicketType
                                    {
                                        Id = 0,
                                        Name = amount.Type,
                                        IsEnabled = true,
                                        CreatedUtc = DateTime.UtcNow,
                                        UpdatedUtc = DateTime.UtcNow
                                    };
                                    asiMonumentTicketType = _aSITicketTypeRepository.Save(asiMonumentTicketTypeModel);
                                }
                                // ------------------------------ CREATE MAIN TICKET CATEGORY ------------------------- //
                                createTicketCategories(asiMonumentTicketType);
                            }

                            // ------------------------------ ASI MONUMENT DETAILS AND TICKET TYPE MAPPINGS ------------------------- //

                            var currentMonumentDetails = _aSIMonumentDetailRepository.GetByMonumentId(asiMonument.Id);
                            foreach (FIL.Contracts.DataModels.ASI.ASIMonumentDetail currentMonumentDetail in currentMonumentDetails)
                            {
                                currentMonumentDetail.IsEnabled = false;
                                _aSIMonumentDetailRepository.Save(currentMonumentDetail);
                            }

                            if (monumentDetailData.Data.Configuration.Optionals.Any())
                            {
                                foreach (FIL.Contracts.Models.ASI.Optional option in monumentDetailData.Data.Configuration.Optionals) // THIS WILL ALWAYS LOOP MAX. ONCE
                                {
                                    foreach (FIL.Contracts.Models.ASI.Option curentOption in option.Options) // THIS WILL ALWAYS LOOP MAX. TWICE
                                    {
                                        var currentASIMonumentDetail = _aSIMonumentDetailRepository.GetByNameAndMonumentId(curentOption.Text, asiMonument.Id); // CHECK FOR ASI DETAIL EXISTS OR NOT
                                        if (currentASIMonumentDetail != null)
                                        {
                                            currentASIMonumentDetail.IsEnabled = true;
                                            _aSIMonumentDetailRepository.Save(currentASIMonumentDetail);

                                            if (curentOption.Type == "without") // THIS IS ONLY THAT SEGRIGATE THE OPTIOANL AND NORMAL TICKET TYPE
                                            {
                                                updateASITicketType(currentASIMonumentDetail,
                                              monumentDetailData.Data.Configuration.Amounts,
                                              option.Amounts,
                                              false);
                                            }
                                            else
                                            {
                                                updateASITicketType(currentASIMonumentDetail,
                                              monumentDetailData.Data.Configuration.Amounts,
                                              option.Amounts,
                                              true);
                                            }
                                        }
                                        else
                                        {
                                            if (curentOption.Type == "without")
                                            {
                                                createASIDetailAndTicketTypeMappings(asiMonument,
                                                    curentOption.Text,
                                                    monumentDetailData.Data.Configuration.Amounts,
                                                    option.Amounts,
                                                    false);
                                            }
                                            else
                                            {
                                                createASIDetailAndTicketTypeMappings(asiMonument,
                                                   curentOption.Text,
                                                   monumentDetailData.Data.Configuration.Amounts,
                                                   option.Amounts,
                                                   true);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var currentASIMonumentDetail = _aSIMonumentDetailRepository.GetByNameAndMonumentId(asiMonument.Name, asiMonument.Id); // CHECK FOR ASI DETAIL EXISTS OR NOT
                                if (currentASIMonumentDetail != null) // IF DETAIL EXISTS
                                {
                                    currentASIMonumentDetail.IsEnabled = true;
                                    _aSIMonumentDetailRepository.Save(currentASIMonumentDetail); // ENABLE EXITSTING MONUMENT DETAIL
                                    updateASITicketType(currentASIMonumentDetail,
                                            monumentDetailData.Data.Configuration.Amounts,
                                            null,
                                            false);
                                }
                                else
                                {
                                    createASIDetailAndTicketTypeMappings(asiMonument,
                                                      asiMonument.Name,
                                                      monumentDetailData.Data.Configuration.Amounts,
                                                      null,
                                                      false);
                                }
                            }

                            // ------------------------------ CREATE AND UPDATE EVENTDETAILS, ETDETAILS, ETATTRIBUTES, EDELIVERYTYPEDETAILS ------------------------- //
                            createEDAndETDAndETAAndVenueAndCityAndState(asiMonument);

                            // ------------------------------ ASI WEEK OPEN DAYS ------------------------- //
                            var asiWeekOpenDays = _aSIMonumentWeekOpenDayRepository.GetByASIMonumentId(asiMonument.Id);
                            var allDays = _daysRepository.GetAll();
                            var eventData = _eventRepository.Get(aSIMappingTable.EventId);
                            foreach (FIL.Contracts.DataModels.Days currentDay in allDays)
                            {
                                var currentDayValue = monumentDetailData.Data.Availability.Week.GetType().GetProperty(currentDay.Name).GetValue(monumentDetailData.Data.Availability.Week, null);
                                var currentMonumentDay = _aSIMonumentWeekOpenDayRepository.GetByASIMonumentIdAndDayId(asiMonument.Id, currentDay.Id);
                                if (currentMonumentDay != null)
                                {
                                    currentMonumentDay.IsEnabled = (bool)currentDayValue;
                                    _aSIMonumentWeekOpenDayRepository.Save(currentMonumentDay);
                                }
                                else
                                {
                                    currentMonumentDay = createASIWeekOpenDaysMappings(asiMonument, currentDay, (bool)currentDayValue);
                                }
                                // ------------------------------ CREATE MAIN PLACE WEEK OPEN DAYS ------------------------- //
                                if (currentMonumentDay != null)
                                {
                                    CreateAndUpdateEventWeekOpenDays(currentMonumentDay, eventData);
                                }
                            }

                            // ------------------------------ ASI TIME SLOT  ------------------------- //
                            var aSIMonumentTimeSlotMappings = _aSIMonumentTimeSlotMappingRepository.GetByASIMonumentId(asiMonument.Id);
                            foreach (FIL.Contracts.DataModels.ASI.ASIMonumentTimeSlotMapping currentTime in aSIMonumentTimeSlotMappings) // INITIALLY DISABLE ALL TIME SLOT
                            {
                                currentTime.IsEnabled = false;
                                _aSIMonumentTimeSlotMappingRepository.Save(currentTime);
                            }

                            foreach (FIL.Contracts.Models.ASI.TimeSlot currentTime in monumentDetailData.Data.Availability.TimeSlots)
                            {
                                var currentMonumentTime = _aSIMonumentTimeSlotMappingRepository.GetByASIMonumentIdAndTimeId(asiMonument.Id, currentTime.Id);
                                if (currentMonumentTime != null)
                                {
                                    currentMonumentTime.Name = currentTime.Name;
                                    currentMonumentTime.StartTime = currentTime.StartTime;
                                    currentMonumentTime.EndTime = currentTime.EndTime;
                                    currentMonumentTime.IsEnabled = true;
                                    _aSIMonumentTimeSlotMappingRepository.Save(currentMonumentTime);
                                }
                                else
                                {
                                    currentMonumentTime = createASITimingMappings(
                                  asiMonument,
                                  currentTime.Name,
                                  currentTime.Id,
                                  currentTime.StartTime,
                                  currentTime.EndTime
                                  );
                                }
                                // ------------------------------ CREATE MAIN EVENT TIME SLOT ------------------------- //
                                if (currentMonumentTime != null)
                                {
                                    CreateAndUpdateEventTimeSlotMappings(currentMonumentTime, eventData);
                                }
                            }

                            // ------------------------------ ASI HOLIDAY ------------------------- //
                            var aSIMonumentHolidayMappings = _aSIMonumentHolidayDayRepository.GetByASIMonumentId(asiMonument.Id);
                            var plaeHoliday = _placeHolidayDatesRepository.GetAllByEventId(eventData.Id);
                            foreach (FIL.Contracts.DataModels.ASI.ASIMonumentHolidayDay currentHoliday in aSIMonumentHolidayMappings) // INITIALLY DELETE ALL HOLIDAY FROM ASI TABLE
                            {
                                _aSIMonumentHolidayDayRepository.Delete(currentHoliday);
                            }
                            foreach (FIL.Contracts.DataModels.PlaceHolidayDate currentHoliday in plaeHoliday) // INITIALLY DELETE ALL HOLIDAY FROM MAIN TABLE
                            {
                                _placeHolidayDatesRepository.Delete(currentHoliday);
                            }
                            foreach (DateTime currentDay in monumentDetailData.Data.Availability.Holidays)
                            {
                                createASIHolidayMappings(asiMonument, currentDay);
                                // ------------------------------ CREATE MAIN EVENT HOLIDAY MAPPINGS ------------------------- //
                                CreatePlaceHolidayDates(currentDay, eventData);
                            }
                        }
                    }
                    // ------------------------------ FINAL CHECK FOR ACTIVE MONUMENT  ------------------------- //
                    checkFinalActiveMonument();
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to run ASI CommandHandler", ex));
            }
            return aSIMonument;
        }
    }
}