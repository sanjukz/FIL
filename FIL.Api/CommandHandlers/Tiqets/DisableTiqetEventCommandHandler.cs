using FIL.Api.Repositories;
using FIL.Api.Repositories.Tiqets;
using FIL.Contracts.Commands.Tiqets;
using FIL.Contracts.DataModels;
using FIL.Logging;
using FIL.Logging.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.Tiqets
{
    public class DisableTiqetEventCommandHandler : BaseCommandHandler<DisableTiqetEventCommand>
    {
        private readonly ITiqetEventDetailMappingRepository _tiqetEventDetailMappingRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventRepository _eventRepository;
        private readonly ILogger _logger;

        public DisableTiqetEventCommandHandler(
       ITiqetEventDetailMappingRepository tiqetEventDetailMappingRepository, IEventDetailRepository eventDetailRepository, IEventRepository eventRepository, ILogger logger,
        IMediator mediator) : base(mediator)
        {
            _logger = logger;
            _eventRepository = eventRepository;
            _eventDetailRepository = eventDetailRepository;
            _tiqetEventDetailMappingRepository = tiqetEventDetailMappingRepository;
        }

        protected override async Task Handle(DisableTiqetEventCommand command)
        {
            try
            {
                //for disabling eventdetails & events
                var getDisabledTiqetProductDetails = _tiqetEventDetailMappingRepository.GetAllDisabled();
                var getDisabledEventDetails = _eventDetailRepository.GetByEventDetailIds(getDisabledTiqetProductDetails.Select(s => s.EventDetailId));
                var getDisabledEventDetailsModel = AutoMapper.Mapper.Map<IEnumerable<EventDetail>>(getDisabledEventDetails);
                foreach (var eventDetailData in getDisabledEventDetailsModel)
                {
                    var disabledEventDetails = _eventDetailRepository.Get(eventDetailData.Id);
                    disabledEventDetails.IsEnabled = false;
                    _eventDetailRepository.Save(disabledEventDetails);
                    var disabledEvent = _eventRepository.Get(disabledEventDetails.EventId);
                    disabledEvent.IsEnabled = false;
                    _eventRepository.Save(disabledEvent);
                }
            }
            catch (Exception e)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to Disable Tiqets Event Data", e));
            }
        }
    }
}