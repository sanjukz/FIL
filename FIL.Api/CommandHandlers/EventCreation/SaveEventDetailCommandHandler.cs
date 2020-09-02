using FIL.Api.Repositories;
using FIL.Contracts.Commands.EventCreation;
using FIL.Contracts.DataModels;
using FIL.Contracts.Interfaces.Commands;
using MediatR;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.EventCreation
{
    public class SaveEventDetailCommandHandler : BaseCommandHandlerWithResult<SaveEventDetailCommand, SaveEventDetailDataResult>
    {
        private readonly IEventDetailRepository _eventDetailRepository;

        public SaveEventDetailCommandHandler(IEventDetailRepository eventDetailRepository, IMediator mediator)
            : base(mediator)
        {
            _eventDetailRepository = eventDetailRepository;
        }

        protected override Task<ICommandResult> Handle(SaveEventDetailCommand command)
        {
            var eventDetailData = new EventDetail();
            SaveEventDetailDataResult saveEventTicketDetailResult = new SaveEventDetailDataResult();
            var checkForEventDetail = _eventDetailRepository.Get(command.Id);

            if (checkForEventDetail != null)
            {
                eventDetailData = new EventDetail
                {
                    Id = checkForEventDetail.Id,
                    Name = command.Name,
                    EventId = command.EventId,
                    VenueId = command.VenueId,
                    MetaDetails = command.MetaDetails,
                    GroupId = command.GroupId,
                    StartDateTime = command.StartDateTime,
                    EndDateTime = command.EndDateTime,
                    CreatedUtc = checkForEventDetail.CreatedUtc,
                    CreatedBy = command.ModifiedBy,
                    IsEnabled = command.IsEnabled
                };
            }
            else
            {
                eventDetailData = new EventDetail
                {
                    Id = command.Id,
                    Name = command.Name,
                    EventId = command.EventId,
                    VenueId = command.VenueId,
                    MetaDetails = command.MetaDetails,
                    GroupId = command.GroupId,
                    StartDateTime = command.StartDateTime,
                    EndDateTime = command.EndDateTime,
                    CreatedBy = command.ModifiedBy,
                    IsEnabled = command.IsEnabled
                };
            }

            FIL.Contracts.DataModels.EventDetail result = _eventDetailRepository.Save(eventDetailData);
            saveEventTicketDetailResult.Id = result.Id;
            saveEventTicketDetailResult.VenueId = result.VenueId;
            return Task.FromResult<ICommandResult>(saveEventTicketDetailResult);
        }
    }
}