using FIL.Api.Repositories;
using FIL.Contracts.Commands.TMS;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Commands;
using MediatR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.TMS
{
    public class AddCategorySponsorCommandHandler : BaseCommandHandlerWithResult<AddCategorySponsorCommand, AddCategorySponsorCommandResult>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly IEventSponsorMappingRepository _eventSponsorMappingRepository;
        private readonly ICorporateTicketAllocationDetailRepository _corporateTicketAllocationDetailRepository;
        private readonly Logging.ILogger _logger;

        public AddCategorySponsorCommandHandler(Logging.ILogger logger,
            IEventRepository eventRepository,
            IVenueRepository venueRepository,
            IEventDetailRepository eventDetailRepository,
            IEventTicketDetailRepository eventTicketDetailRepository,
            IEventTicketAttributeRepository eventTicketAttributeRepository,
            IEventSponsorMappingRepository eventSponsorMappingRepository,
        ICorporateTicketAllocationDetailRepository corporateTicketAllocationDetailRepository,
            IMediator mediator) : base(mediator)
        {
            _eventRepository = eventRepository;
            _venueRepository = venueRepository;
            _eventDetailRepository = eventDetailRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _eventSponsorMappingRepository = eventSponsorMappingRepository;
            _corporateTicketAllocationDetailRepository = corporateTicketAllocationDetailRepository;
            _logger = logger;
        }

        protected override Task<ICommandResult> Handle(AddCategorySponsorCommand command)
        {
            AddCategorySponsorCommandResult addCategorySponsorCommandResult = new AddCategorySponsorCommandResult();
            try
            {
                if (command.AllocationType == AllocationType.Match)
                {
                    var corporateTicketAllocationDetail = _corporateTicketAllocationDetailRepository.GetByEventTicketAttributeIdandSponsorId((long)command.EventTicketAttributeId, command.SponsorId);
                    if (corporateTicketAllocationDetail == null)
                    {
                        var CorporateTicketAllocationDetail = new CorporateTicketAllocationDetail
                        {
                            AltId = new Guid(),
                            EventTicketAttributeId = (long)command.EventTicketAttributeId,
                            SponsorId = command.SponsorId,
                            Price = 0,
                            ModifiedBy = command.ModifiedBy,
                            IsEnabled = true
                        };
                        CorporateTicketAllocationDetail corporateTicketAllocationDetailResult = _corporateTicketAllocationDetailRepository.Save(CorporateTicketAllocationDetail);
                        addCategorySponsorCommandResult.Id = corporateTicketAllocationDetailResult.Id;
                        addCategorySponsorCommandResult.Success = true;
                        addCategorySponsorCommandResult.Message = "Sponsor added successfully";
                    }
                    else
                    {
                        addCategorySponsorCommandResult.Id = -1;
                        addCategorySponsorCommandResult.Success = false;
                        addCategorySponsorCommandResult.Message = "Sponsor already added for the selected category";
                    }
                }
                else if (command.AllocationType == AllocationType.Venue)
                {
                    var venueId = _venueRepository.GetByAltId((Guid)command.VenueAltId).Id;
                    var eventId = _eventRepository.GetByAltId((Guid)command.EventAltId).Id;
                    var eventDetails = _eventDetailRepository.GetEventDetailForTMS(eventId, venueId);
                    EventSponsorMapping eventSponsorMappingResult = new EventSponsorMapping();
                    if (eventDetails.Any())
                    {
                        foreach (var item in eventDetails)
                        {
                            var eventSponsorDetails = _eventSponsorMappingRepository.GetByEventDetailIdandSponsorId(item.Id, command.SponsorId);
                            if (!eventSponsorDetails.Any())
                            {
                                var eventSponsorMapping = new EventSponsorMapping
                                {
                                    SponsorId = command.SponsorId,
                                    EventDetailId = item.Id,
                                    ModifiedBy = command.ModifiedBy,
                                    IsEnabled = true
                                };
                                eventSponsorMappingResult = _eventSponsorMappingRepository.Save(eventSponsorMapping);
                            }

                            var eventTicketDetails = _eventTicketDetailRepository.GetAllByTicketCategoryIdAndEventDetailId(command.TicketCategoryId, item.Id);
                            var eventTicketAttributes = _eventTicketAttributeRepository.GetByEventTicketDetailId(eventTicketDetails.Id);
                            var corporateTicketAllocationDetail = _corporateTicketAllocationDetailRepository.GetByEventTicketAttributeIdandSponsorId(eventTicketAttributes.Id, command.SponsorId);
                            if (corporateTicketAllocationDetail == null)
                            {
                                var CorporateTicketAllocationDetails = new CorporateTicketAllocationDetail
                                {
                                    AltId = new Guid(),
                                    EventTicketAttributeId = eventTicketAttributes.Id,
                                    SponsorId = command.SponsorId,
                                    Price = 0,
                                    ModifiedBy = command.ModifiedBy,
                                    IsEnabled = true
                                };
                                CorporateTicketAllocationDetail corporateTicketAllocationDetailResult = _corporateTicketAllocationDetailRepository.Save(CorporateTicketAllocationDetails);
                                addCategorySponsorCommandResult.Id = corporateTicketAllocationDetailResult.Id;
                                addCategorySponsorCommandResult.Success = true;
                                addCategorySponsorCommandResult.Message = "Sponsor added successfully";
                            }
                        }
                    }
                    else
                    {
                        addCategorySponsorCommandResult.Id = -1;
                        addCategorySponsorCommandResult.Success = false;
                        addCategorySponsorCommandResult.Message = "Something went worng";
                    }
                }
                else if (command.AllocationType == AllocationType.Sponsor)
                {
                    /* TODO:  */
                }
                return Task.FromResult<ICommandResult>(addCategorySponsorCommandResult);
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                addCategorySponsorCommandResult.Id = -1;
                addCategorySponsorCommandResult.Success = false;
                addCategorySponsorCommandResult.Message = ex.ToString();
                return Task.FromResult<ICommandResult>(addCategorySponsorCommandResult);
            }
        }
    }
}