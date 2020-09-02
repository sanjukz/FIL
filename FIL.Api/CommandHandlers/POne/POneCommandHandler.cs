using FIL.Api.Integrations;
using FIL.Api.Integrations.POne;
using FIL.Api.Repositories;
using FIL.Configuration;
using FIL.Contracts.Commands.POne;
using FIL.Contracts.DataModels;
using FIL.Contracts.DataModels.POne;
using FIL.Contracts.Enums;
using FIL.Logging;
using MediatR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.POne
{
    public class POneCommandHandler : BaseCommandHandler<POneCommand>
    {
        private readonly ILogger _logger;
        private readonly ISettings _settings;
        private readonly IMediator _mediator;
        private readonly IEventRepository _eventRepository;
        private readonly IEventCategoryRepository _eventCategoryRepository;
        private readonly IDaysRepository _daysRepository;
        private readonly IPlaceWeekOpenDaysRepository _placeWeekOpenDaysRepository;
        private readonly IEventSiteIdMappingRepository _eventSiteIdMappingRepository;
        private readonly IEventCategoryMappingRepository _eventCategoryMappingRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IPOneEventDetailMappingRepository _pOneEventDetailMappingRepository;
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

        // POne Repositories
        private readonly IPOneEventCategoryRepository _pOneEventCategoryRepository;

        private readonly IPOneEventDetailRepository _pOneEventDetailRepository;
        private readonly IPOneEventRepository _pOneEventRepository;
        private readonly IPOneEventSubCategoryRepository _pOneEventSubCategoryRepository;
        private readonly IPOneEventTicketAttributeRepository _pOneEventTicketAttributeRepository;
        private readonly IPOneEventTicketDetailRepository _pOneEventTicketDetailRepository;
        private readonly IPOneTicketCategoryRepository _pOneTicketCategoryRepository;
        private readonly IPOneVenueRepository _pOneVenueRepository;
        private readonly IPOneEventTicketAttributeMappingRepository _pOneEventTicketAttributeMappingRepository;

        // Utilities
        private readonly IGoogleMapApi _googleMapApi;

        private readonly ICountryAlphaCode _countryAlphaCode;
        private readonly IPOneApi _pOneApi;

        public POneCommandHandler(
            ILogger logger,
            ISettings settings,
            IMediator mediator,
            IEventRepository eventRepository,
            IEventCategoryRepository eventCategoryRepository,
            IDaysRepository daysRepository,
            IPlaceWeekOpenDaysRepository placeWeekOpenDaysRepository,
            IEventSiteIdMappingRepository eventSiteIdMappingRepository,
            IEventCategoryMappingRepository eventCategoryMappingRepository,
            IVenueRepository venueRepository,
            IEventDetailRepository eventDetailRepository,
            IPOneEventDetailMappingRepository pOneEventDetailMappingRepository,
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

            // POne Repositories
            IPOneEventCategoryRepository pOneEventCategoryRepository,
            IPOneEventDetailRepository pOneEventDetailRepository,
            IPOneEventRepository pOneEventRepository,
            IPOneEventSubCategoryRepository pOneEventSubCategoryRepository,
            IPOneEventTicketAttributeRepository pOneEventTicketAttributeRepository,
            IPOneEventTicketDetailRepository pOneEventTicketDetailRepository,
            IPOneTicketCategoryRepository pOneTicketCategoryRepository,
            IPOneVenueRepository pOneVenueRepository,
            IPOneEventTicketAttributeMappingRepository pOneEventTicketAttributeMappingRepository,
            // Utilities
            IGoogleMapApi googleMapApi,
            ICountryAlphaCode countryAlphaCode,
            IPOneApi pOneApi
            )
           : base(mediator)
        {
            _logger = logger;
            _settings = settings;
            _mediator = mediator;
            _eventRepository = eventRepository;
            _eventCategoryRepository = eventCategoryRepository;
            _daysRepository = daysRepository;
            _placeWeekOpenDaysRepository = placeWeekOpenDaysRepository;
            _eventSiteIdMappingRepository = eventSiteIdMappingRepository;
            _eventCategoryMappingRepository = eventCategoryMappingRepository;
            _venueRepository = venueRepository;
            _eventDetailRepository = eventDetailRepository;
            _pOneEventDetailMappingRepository = pOneEventDetailMappingRepository;
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
            _pOneEventCategoryRepository = pOneEventCategoryRepository;
            _pOneEventDetailRepository = pOneEventDetailRepository;
            _pOneEventRepository = pOneEventRepository;
            _pOneEventSubCategoryRepository = pOneEventSubCategoryRepository;
            _pOneEventTicketAttributeRepository = pOneEventTicketAttributeRepository;
            _pOneEventTicketDetailRepository = pOneEventTicketDetailRepository;
            _pOneTicketCategoryRepository = pOneTicketCategoryRepository;
            _pOneVenueRepository = pOneVenueRepository;
            _pOneEventTicketAttributeMappingRepository = pOneEventTicketAttributeMappingRepository;
            _googleMapApi = googleMapApi;
            _countryAlphaCode = countryAlphaCode;
            _pOneApi = pOneApi;
        }

        protected override async Task Handle(POneCommand command)
        {
            var allEvents = _pOneEventRepository.GetAll().ToList();

            foreach (var item in allEvents)
            {
                //var pOneEventCategory = _pOneEventCategoryRepository.Get(item.POneEventCategoryId);
                /*var lastEventCategory = _eventCategoryRepository.GetAll().OrderByDescending(p => p.Id).FirstOrDefault();*/
                var eventCategory = _eventCategoryRepository.GetByName("Sports");
                /*if (eventCategory == null)
                {
                    eventCategory = _eventCategoryRepository.Save(new EventCategory
                    {
                        Id = lastEventCategory,
                        Category = pOneEventCategory.Name,
                        DisplayName = pOneEventCategory.Name,
                        Slug = pOneEventCategory.Name.ToLower().Replace(" ", "-"),
                        IsHomePage = false,
                        EventCategoryId = 0,
                        IsFeel = false,
                        Order = 101,
                        IsEnabled = true,
                        CreatedBy = command.ModifiedBy,
                        CreatedUtc = DateTime.UtcNow,
                        UpdatedBy = command.ModifiedBy,
                        UpdatedUtc = DateTime.UtcNow
                    });
                }*/

                var @event = _eventRepository.GetByName(item.Name).FirstOrDefault();
                if (@event == null)
                {
                    @event = _eventRepository.Save(new Event
                    {
                        AltId = Guid.NewGuid(),
                        EventCategoryId = Convert.ToInt32(eventCategory),
                        EventTypeId = EventType.Regular,
                        Name = item.Name,
                        Description = string.Empty,
                        ClientPointOfContactId = 1,
                        MetaDetails = string.Empty,
                        IsFeel = false,
                        EventSourceId = EventSource.POne,
                        TermsAndConditions = string.Empty,
                        IsPublishedOnSite = true,
                        PublishedDateTime = DateTime.Now,
                        IsEnabled = true,
                        CreatedBy = command.ModifiedBy,
                        UpdatedBy = command.ModifiedBy,
                        CreatedUtc = DateTime.UtcNow,
                        UpdatedUtc = DateTime.UtcNow
                    });
                }

                var pOneEventDetailList = _pOneEventDetailRepository.GetAllByPOneEventId(item.POneId).ToList();

                foreach (var pOneEventDetail in pOneEventDetailList)
                {
                    var pOneVenue = _pOneVenueRepository.Get(pOneEventDetail.POneVenueId);
                    var venue = _venueRepository.GetByNameAndCityId(pOneVenue.Name, pOneVenue.CityId);
                    if (venue == null)
                    {
                        venue = _venueRepository.Save(new Venue
                        {
                            AltId = Guid.NewGuid(),
                            Name = pOneVenue.Name,
                            AddressLineOne = pOneVenue.Address,
                            CityId = pOneVenue.CityId,
                            Latitude = pOneVenue.Latitude,
                            Longitude = pOneVenue.Longitude,
                            IsEnabled = true,
                            CreatedBy = command.ModifiedBy,
                            UpdatedBy = command.ModifiedBy,
                            CreatedUtc = DateTime.UtcNow,
                            UpdatedUtc = DateTime.UtcNow
                        });
                    }
                    else
                    {
                        venue.AddressLineOne = pOneVenue.Address;
                        venue.CityId = pOneVenue.CityId;
                        venue.Latitude = pOneVenue.Latitude;
                        venue.Longitude = pOneVenue.Longitude;
                        venue.UpdatedBy = command.ModifiedBy;
                        venue.UpdatedUtc = DateTime.UtcNow;
                    }

                    var eventDetailMapping = _pOneEventDetailMappingRepository.GetPOneMappedEventDetails(pOneEventDetail.POneId);

                    EventDetail eventDetail = new EventDetail();
                    if (eventDetailMapping == null)
                    {
                        eventDetail = _eventDetailRepository.Save(new EventDetail
                        {
                            AltId = Guid.NewGuid(),
                            Name = pOneEventDetail.Name,
                            EventId = @event.Id,
                            VenueId = venue.Id,
                            GroupId = 1,
                            StartDateTime = pOneEventDetail.StartDateTime,
                            EndDateTime = pOneEventDetail.StartDateTime.AddHours(3),
                            MetaDetails = pOneEventDetail.MetaDetails,
                            Description = pOneEventDetail.Description,
                            IsEnabled = true,
                            CreatedBy = command.ModifiedBy,
                            UpdatedBy = command.ModifiedBy,
                            CreatedUtc = DateTime.UtcNow,
                            UpdatedUtc = DateTime.UtcNow
                        });

                        eventDetailMapping = _pOneEventDetailMappingRepository.Save(new Contracts.DataModels.POne.POneEventDetailMapping
                        {
                            POneEventDetailId = pOneEventDetail.POneId,
                            ZoongaEventDetailId = eventDetail.Id
                        });

                        _eventDeliveryTypeDetailRepository.Save(new EventDeliveryTypeDetail
                        {
                            EventDetailId = eventDetail.Id,
                            DeliveryTypeId = pOneEventDetail.DeliveryTypeId,
                            RefundPolicy = 2,
                            EndDate = pOneEventDetail.StartDateTime,
                            Notes = pOneEventDetail.DeliveryNotes,
                            IsEnabled = true,
                            CreatedBy = command.ModifiedBy,
                            UpdatedBy = command.ModifiedBy,
                            CreatedUtc = DateTime.UtcNow,
                            UpdatedUtc = DateTime.UtcNow
                        });
                    }
                    else
                    {
                        eventDetail = _eventDetailRepository.Get(eventDetailMapping.ZoongaEventDetailId);
                        eventDetail.Name = pOneEventDetail.Name;
                        eventDetail.VenueId = venue.Id;
                        eventDetail.StartDateTime = pOneEventDetail.StartDateTime;
                        eventDetail.MetaDetails = pOneEventDetail.MetaDetails;
                        eventDetail.Description = pOneEventDetail.Description;
                        eventDetail.UpdatedUtc = DateTime.UtcNow;
                        _eventDetailRepository.Save(eventDetail);
                    }

                    var pOneEventTicketDetailList = _pOneEventTicketDetailRepository.GetByPOneEventDetail(pOneEventDetail.POneId).ToList();

                    foreach (var pOneEventTicketDetail in pOneEventTicketDetailList)
                    {
                        var pOneTicketCategory = _pOneTicketCategoryRepository.GetByPOneId(pOneEventTicketDetail.POneTicketCategoryId);
                        var ticketCategory = _ticketCategoryRepository.GetByName(pOneTicketCategory.Name);
                        if (ticketCategory == null)
                        {
                            ticketCategory = _ticketCategoryRepository.Save(new TicketCategory
                            {
                                Name = pOneTicketCategory.Name,
                                IsEnabled = true,
                                CreatedBy = command.ModifiedBy,
                                UpdatedBy = command.ModifiedBy,
                                CreatedUtc = DateTime.UtcNow,
                                UpdatedUtc = DateTime.UtcNow
                            });
                        }

                        var eventTicketDetail = _eventTicketDetailRepository.GetByTicketCategoryIdAndEventDetailId(ticketCategory.Id, eventDetail.Id);

                        if (eventTicketDetail == null)
                        {
                            eventTicketDetail = _eventTicketDetailRepository.Save(new EventTicketDetail
                            {
                                EventDetailId = eventDetail.Id,
                                TicketCategoryId = ticketCategory.Id,
                                IsBOEnabled = false,
                                IsEnabled = true,
                                CreatedBy = command.ModifiedBy,
                                UpdatedBy = command.ModifiedBy,
                                CreatedUtc = DateTime.UtcNow,
                                UpdatedUtc = DateTime.UtcNow
                            });
                        }

                        var eventTicketAttribute = _eventTicketAttributeRepository.GetByEventTicketDetailId(eventTicketDetail.Id);

                        if (eventTicketAttribute == null)
                        {
                            var pOneEventTicketAttribute = _pOneEventTicketAttributeRepository.GetByPOneEventTicketDetailId(pOneEventTicketDetail.Id);
                            var currencyType = _currencyTypeRepository.GetByCurrencyCode("EUR");

                            eventTicketAttribute = _eventTicketAttributeRepository.Save(new EventTicketAttribute
                            {
                                EventTicketDetailId = eventTicketDetail.Id,
                                SalesStartDateTime = pOneEventTicketAttribute.IsDateConfirmed ? DateTime.UtcNow.AddDays(-1) : DateTime.UtcNow.AddDays(1),
                                SalesEndDatetime = pOneEventTicketAttribute.IsDateConfirmed ? DateTime.UtcNow.AddDays(100) : DateTime.UtcNow.AddDays(-1),
                                TicketTypeId = TicketType.Regular,
                                ChannelId = Channels.Website,
                                CurrencyId = currencyType.Id,
                                AvailableTicketForSale = pOneEventTicketAttribute.AvailableTicketForSale,
                                RemainingTicketForSale = pOneEventTicketAttribute.AvailableTicketForSale,
                                TicketCategoryDescription = pOneEventTicketAttribute.TicketCategoryDescription,
                                IsSeatSelection = false,
                                Price = pOneEventTicketAttribute.Price,
                                TicketValidityType = TicketValidityTypes.None,
                                ViewFromStand = string.Empty,
                                SharedInventoryGroupId = null,
                                IsInternationalCardAllowed = true,
                                IsEMIApplicable = false,
                                IsEnabled = true,
                                CreatedBy = command.ModifiedBy,
                                UpdatedBy = command.ModifiedBy,
                                CreatedUtc = DateTime.UtcNow,
                                UpdatedUtc = DateTime.UtcNow
                            });

                            _pOneEventTicketAttributeMappingRepository.Save(new POneEventTicketAttributeMapping
                            {
                                POneEventTicketAttributeId = pOneEventTicketAttribute.POneId,
                                ZoongaEventTicketAttributeId = eventTicketAttribute.Id,
                                UpdatedUtc = DateTime.UtcNow,
                                CreatedUtc = DateTime.UtcNow
                            });

                            var ticketFeeDetail = _ticketFeeDetailRepository.GetByEventTicketAttributeId(eventTicketAttribute.Id);
                            if (ticketFeeDetail == null)
                            {
                                ticketFeeDetail = _ticketFeeDetailRepository.Save(new TicketFeeDetail
                                {
                                    EventTicketAttributeId = eventTicketAttribute.Id,
                                    FeeId = (int)FeeType.ShippingCharge,
                                    DisplayName = "Shipping Charge",
                                    ValueTypeId = (int)ValueTypes.Flat,
                                    Value = pOneEventTicketAttribute.ShippingCharge,
                                    ModifiedBy = command.ModifiedBy,
                                    IsEnabled = true
                                });
                            }
                        }
                        else
                        {
                            var pOneEventTicketAttribute = _pOneEventTicketAttributeRepository.GetByPOneEventTicketDetailId(pOneEventTicketDetail.Id);

                            eventTicketAttribute.SalesStartDateTime = pOneEventTicketAttribute.IsDateConfirmed ? DateTime.UtcNow.AddDays(-1) : DateTime.UtcNow.AddDays(1);
                            eventTicketAttribute.SalesEndDatetime = pOneEventTicketAttribute.IsDateConfirmed ? DateTime.UtcNow.AddDays(100) : DateTime.UtcNow.AddDays(-1);
                            eventTicketAttribute.AvailableTicketForSale = pOneEventTicketAttribute.AvailableTicketForSale;
                            eventTicketAttribute.RemainingTicketForSale = pOneEventTicketAttribute.AvailableTicketForSale;
                            eventTicketAttribute.TicketCategoryDescription = pOneEventTicketAttribute.TicketCategoryDescription;
                            eventTicketAttribute.Price = pOneEventTicketAttribute.Price;
                            eventTicketAttribute.UpdatedBy = command.ModifiedBy;

                            eventTicketAttribute = _eventTicketAttributeRepository.Save(eventTicketAttribute);

                            var ticketFeeDetail = _ticketFeeDetailRepository.GetByEventTicketAttributeId(eventTicketAttribute.Id);

                            ticketFeeDetail.Value = pOneEventTicketAttribute.ShippingCharge;

                            _ticketFeeDetailRepository.Save(ticketFeeDetail);
                        }
                    }
                }
            }
        }
    }
}