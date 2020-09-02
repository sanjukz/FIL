using FIL.Api.Integrations;
using FIL.Api.Integrations.POne;
using FIL.Api.Repositories;
using FIL.Configuration;
using FIL.Contracts.Commands.POne;
using FIL.Contracts.Models.Integrations.POne;
using FIL.Logging;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.POne
{
    public class POneBookingCommandHandler : BaseCommandHandler<POneBookingCommand>
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
        private readonly IPOneBooking _pOneBooking;

        public POneBookingCommandHandler(
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
            IPOneApi pOneApi,
            IPOneBooking pOneBooking
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
            _pOneApi = pOneApi;
            _pOneBooking = pOneBooking;
        }

        protected override async Task Handle(POneBookingCommand command)
        {
            List<Product> products = new List<Product>();
            foreach (var item in command.Orders)
            {
                var pOneEventTicketAttributeMapping = _pOneEventTicketAttributeMappingRepository.GetPOneMappedEventAttribute(item.EventTicketAttributeId);

                var product = new Product
                {
                    sku = pOneEventTicketAttributeMapping.POneEventTicketAttributeId.ToString(),
                    amount = (short)item.TicketAmount
                };
                products.Add(product);
            }

            var bookingModel = new POneBookingModel
            {
                products = products
            };

            var res = await _pOneBooking.POneBookingApi(bookingModel);
        }
    }
}