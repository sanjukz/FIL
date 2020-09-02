using FIL.Api.Repositories;
using FIL.Contracts.Commands.TMS;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.TMS
{
    public class AddMatchSponsorCommandHandler : BaseCommandHandlerWithResult<AddMatchSponsorCommand, AddMatchSponsorCommandResult>
    {
        private readonly IVenueRepository _venuetRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventSponsorMappingRepository _eventSponsorMappingRepository;
        private readonly Logging.ILogger _logger;

        public AddMatchSponsorCommandHandler(Logging.ILogger logger,
            IVenueRepository venuetRepository,
            IEventRepository eventRepository,
            IEventDetailRepository eventDetailRepository,
            IEventSponsorMappingRepository eventSponsorMappingRepository,
            IMediator mediator) : base(mediator)
        {
            _venuetRepository = venuetRepository;
            _eventRepository = eventRepository;
            _eventSponsorMappingRepository = eventSponsorMappingRepository;
            _eventDetailRepository = eventDetailRepository;
            _logger = logger;
        }

        protected override Task<ICommandResult> Handle(AddMatchSponsorCommand command)
        {
            AddMatchSponsorCommandResult addMatchSponsorCommandResult = new AddMatchSponsorCommandResult();
            try
            {
                if (command.AllocationType == AllocationType.Match)
                {
                    var eventdetailId = _eventDetailRepository.GetByAltId((Guid)command.EventDetailAltId).Id;
                    var eventSponsor = _eventSponsorMappingRepository.GetByEventDetailIdandSponsorId(eventdetailId, command.SponsorId);
                    if (!eventSponsor.Any())
                    {
                        var eventSponsorMapping = new EventSponsorMapping
                        {
                            SponsorId = command.SponsorId,
                            EventDetailId = eventdetailId,
                            ModifiedBy = command.ModifiedBy,
                            IsEnabled = true
                        };

                        EventSponsorMapping eventSponsormappingResult = _eventSponsorMappingRepository.Save(eventSponsorMapping);
                        addMatchSponsorCommandResult.Id = eventSponsormappingResult.Id;
                        addMatchSponsorCommandResult.Success = true;
                        addMatchSponsorCommandResult.Message = "Sponsor added successfully!";
                    }
                    else
                    {
                        addMatchSponsorCommandResult.Id = -1;
                        addMatchSponsorCommandResult.Success = false;
                        addMatchSponsorCommandResult.Message = "Sponsor already added!";
                    }
                }
                else if (command.AllocationType == AllocationType.Venue)
                {
                    var venue = _venuetRepository.GetByAltId((Guid)command.VenueAltId);
                    var events = _eventRepository.GetByAltId((Guid)command.EventAltId);
                    var eventDetailList = AutoMapper.Mapper.Map<List<FIL.Contracts.DataModels.EventDetail>>(_eventDetailRepository.GetEventDetailForTMS(events.Id, venue.Id));
                    if (eventDetailList.Any())
                    {
                        foreach (var item in eventDetailList)
                        {
                            var eventSponsor = _eventSponsorMappingRepository.GetByEventDetailIdandSponsorId(item.Id, command.SponsorId);
                            if (!eventSponsor.Any())
                            {
                                var eventSponsorMapping = new EventSponsorMapping
                                {
                                    SponsorId = command.SponsorId,
                                    EventDetailId = item.Id,
                                    ModifiedBy = command.ModifiedBy,
                                    IsEnabled = true
                                };
                                EventSponsorMapping eventSponsormappingResult = _eventSponsorMappingRepository.Save(eventSponsorMapping);
                                addMatchSponsorCommandResult.Id = eventSponsormappingResult.Id;
                                addMatchSponsorCommandResult.Success = true;
                                addMatchSponsorCommandResult.Message = "Sponsor added successfully!";
                            }
                        }
                    }
                    else
                    {
                        addMatchSponsorCommandResult.Id = -1;
                        addMatchSponsorCommandResult.Success = false;
                        addMatchSponsorCommandResult.Message = "sub-event not exist for the current selection";
                    }
                }
                else if (command.AllocationType == AllocationType.Sponsor)
                {
                    /*TODO:*/
                }
                return Task.FromResult<ICommandResult>(addMatchSponsorCommandResult);
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                addMatchSponsorCommandResult.Id = -1;
                addMatchSponsorCommandResult.Success = false;
                addMatchSponsorCommandResult.Message = ex.ToString();
                return Task.FromResult<ICommandResult>(addMatchSponsorCommandResult);
            }
        }
    }
}