using FIL.Api.Repositories;
using FIL.Contracts.Commands.CitySightSeeing;
using FIL.Contracts.DataModels;
using FIL.Logging;
using FIL.Logging.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.CitySightSeeing
{
    public class DisabledCitySightSeeingEventCommandHandler : BaseCommandHandler<DisabledCitySightSeeingEventCommand>
    {
        private readonly ICitySightSeeingEventDetailMappingRepository _citySightSeeingEventDetailMappingRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventRepository _eventRepository;
        private readonly ILogger _logger;

        public DisabledCitySightSeeingEventCommandHandler(
        IEventDetailRepository eventDetailRepository, IEventRepository eventRepository, ICitySightSeeingEventDetailMappingRepository citySightSeeingEventDetailMappingRepository, ILogger logger,
        IMediator mediator) : base(mediator)
        {
            _logger = logger;
            _eventRepository = eventRepository;
            _eventDetailRepository = eventDetailRepository;
            _citySightSeeingEventDetailMappingRepository = citySightSeeingEventDetailMappingRepository;
        }

        protected override async Task Handle(DisabledCitySightSeeingEventCommand command)
        {
            try
            {
                //for disabling eventdetails & events
                var getDisabledCitySightEventDetails = _citySightSeeingEventDetailMappingRepository.GetAllDisabledDetails();
                var getDisabledEventDetails = _eventDetailRepository.GetByEventDetailIds(getDisabledCitySightEventDetails.Select(s => s.EventDetailId));
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
                _logger.Log(LogCategory.Error, new Exception("Failed to Disable Event HOHO Data", e));
            }
        }
    }
}