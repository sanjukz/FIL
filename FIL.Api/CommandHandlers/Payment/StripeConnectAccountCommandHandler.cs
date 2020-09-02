using FIL.Api.Providers;
using FIL.Api.Repositories;
using FIL.Contracts.Commands.Payment;
using FIL.Contracts.DataModels;
using FIL.Contracts.Interfaces.Commands;
using MediatR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.Payment
{
    public class StripeConnectAccountCommandHandler : BaseCommandHandlerWithResult<StripeConnectAccountCommand, StripeConnectAccountCommandResult>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventStripeConnectMasterRepository _eventStripeConnectMasterRepository;
        private readonly IGenerateStripeConnectProvider _generateStripeConnectProvider;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly IPlaceWeekOpenDaysRepository _placeWeekOpenDaysRepository;
        private readonly IDayTimeMappingsRepository _dayTimeMappingsRepository;
        private readonly IEventCategoryRepository _eventCategoryRepository;
        private readonly IEventCategoryMappingRepository _eventCategoryMappingRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICurrencyTypeRepository _currencyTypeRepository;
        private readonly IEventAttributeRepository _eventAttributeRepository;
        private readonly ILocalTimeZoneConvertProvider _localTimeZoneConvertProvider;
        private readonly IStepProvider _stepProvider;
        private readonly FIL.Logging.ILogger _logger;

        public StripeConnectAccountCommandHandler(
            FIL.Logging.ILogger logger,
            IEventStripeConnectMasterRepository eventStripeConnectMasterRepository,
            IEventRepository eventRepository,
            IGenerateStripeConnectProvider generateStripeConnectProvider,
            IEventDetailRepository eventDetailRepository,
            IEventTicketDetailRepository eventTicketDetailRepository,
            IEventTicketAttributeRepository eventTicketAttributeRepository,
            IPlaceWeekOpenDaysRepository placeWeekOpenDaysRepository,
            IDayTimeMappingsRepository dayTimeMappingsRepository,
             IEventCategoryRepository eventCategoryRepository,
             ITicketCategoryRepository ticketCategoryRepository,
             IUserRepository userRepository,
             IEventCategoryMappingRepository eventCategoryMappingRepository,
             ICurrencyTypeRepository currencyTypeRepository,
              IEventAttributeRepository eventAttributeRepository,
              ILocalTimeZoneConvertProvider localTimeZoneConvertProvider,
              IStepProvider stepProvider,
            IMediator mediator)
           : base(mediator)
        {
            _eventStripeConnectMasterRepository = eventStripeConnectMasterRepository;
            _eventRepository = eventRepository;
            _generateStripeConnectProvider = generateStripeConnectProvider;
            _logger = logger;
            _eventDetailRepository = eventDetailRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _placeWeekOpenDaysRepository = placeWeekOpenDaysRepository;
            _dayTimeMappingsRepository = dayTimeMappingsRepository;
            _eventCategoryRepository = eventCategoryRepository;
            _eventCategoryMappingRepository = eventCategoryMappingRepository;
            _currencyTypeRepository = currencyTypeRepository;
            _userRepository = userRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _eventAttributeRepository = eventAttributeRepository;
            _localTimeZoneConvertProvider = localTimeZoneConvertProvider;
            _stepProvider = stepProvider;
        }

        protected override async Task<ICommandResult> Handle(StripeConnectAccountCommand command)
        {
            try
            {
                var eventData = _eventRepository.GetByAltId(command.EventId);
                if (eventData != null)
                {
                    var eventDetails = _eventDetailRepository.GetByEventId(eventData.Id);
                    var eventTicketDetails = _eventTicketDetailRepository.GetByEventDetailId(eventDetails.Id);
                    var ticketCategories = _ticketCategoryRepository.GetByTicketCategoryIds(eventTicketDetails.Select(s => s.TicketCategoryId));
                    var eventTicketAttributes = _eventTicketAttributeRepository.GetByEventTicketDetailId(eventTicketDetails.Select(s => s.Id));
                    var placeWeekOpenDay = _placeWeekOpenDaysRepository.GetByEventId(eventData.Id).FirstOrDefault();
                    var dayTimeMappings = _dayTimeMappingsRepository.GetAllByPlaceWeekOpenDay(placeWeekOpenDay.Id).FirstOrDefault();
                    var eventCategoryMappings = _eventCategoryMappingRepository.GetByEventId(eventData.Id).FirstOrDefault();
                    var subCategory = _eventCategoryRepository.Get(eventCategoryMappings.EventCategoryId);
                    var user = _userRepository.GetByAltId(eventData.CreatedBy);
                    var currencyType = _currencyTypeRepository.Get(eventTicketAttributes.FirstOrDefault().CurrencyId);
                    var eventAttribute = _eventAttributeRepository.GetByEventDetailId(eventDetails.Id);
                    eventDetails.StartDateTime = _localTimeZoneConvertProvider.ConvertToLocal(eventDetails.StartDateTime, eventAttribute.TimeZone);
                    var stripeConnectAccountId = command.IsStripeConnect ? _generateStripeConnectProvider.GenerateStripeAccessToken(command.AuthorizationCode, command.channels, eventData.Id) : null;
                    if (stripeConnectAccountId != null && stripeConnectAccountId != "")
                    {
                        var eventStripeConnect = _eventStripeConnectMasterRepository.GetByEventId(eventData.Id);
                        var eventStepDetails = _stepProvider.SaveEventStepDetails(eventData.Id, 9, true, command.ModifiedBy);
                        var stripeConnnect = new EventStripeConnectMaster
                        {
                            Id = eventStripeConnect != null ? eventStripeConnect.Id : 0,
                            StripeConnectAccountID = stripeConnectAccountId,
                            CreatedBy = eventStripeConnect != null ? eventStripeConnect.CreatedBy : command.ModifiedBy,
                            CreatedUtc = eventStripeConnect != null ? eventStripeConnect.CreatedUtc : DateTime.UtcNow,
                            ExtraCommisionFlat = command.ExtraCommisionFlat,
                            ExtraCommisionPercentage = command.ExtraCommisionPercentage,
                            EventId = eventData.Id,
                            IsEnabled = true,
                            ModifiedBy = command.ModifiedBy,
                            UpdatedBy = command.ModifiedBy,
                            UpdatedUtc = DateTime.UtcNow,
                        };
                        _eventStripeConnectMasterRepository.Save(stripeConnnect);
                    }
                    return new StripeConnectAccountCommandResult
                    {
                        Success = true,
                        EventTicketAttribute = eventTicketAttributes.ToList(),
                        DayTimeMappings = dayTimeMappings,
                        Event = eventData,
                        EventDetail = eventDetails,
                        EventTicketDetail = eventTicketDetails.ToList(),
                        TicketCategories = ticketCategories.ToList(),
                        ParentCategoryId = subCategory.EventCategoryId,
                        CurrencyType = currencyType,
                        Email = user.Email
                    };
                }
                else
                {
                    return new StripeConnectAccountCommandResult { };
                }
            }
            catch (Exception e)
            {
                _logger.Log(FIL.Logging.Enums.LogCategory.Error, e);
                return new StripeConnectAccountCommandResult { };
            }
        }
    }
}