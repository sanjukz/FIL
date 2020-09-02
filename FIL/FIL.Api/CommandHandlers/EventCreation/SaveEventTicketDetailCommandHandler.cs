using FIL.Api.Repositories;
using FIL.Contracts.Commands.EventCreation;
using FIL.Contracts.DataModels;
using FIL.Contracts.Interfaces.Commands;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.EventCreation
{
    public class SaveEventTicketDetailCommandHandler : BaseCommandHandlerWithResult<SaveEventTicketDetailCommand, SaveEventTicketDetailResult>
    {
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;

        public SaveEventTicketDetailCommandHandler(IEventTicketDetailRepository eventTicketDetailRepository, IMediator mediator) : base(mediator)
        {
            _eventTicketDetailRepository = eventTicketDetailRepository;
        }

        protected override Task<ICommandResult> Handle(SaveEventTicketDetailCommand command)
        {
            var EventTicketDetail = new EventTicketDetail();
            SaveEventTicketDetailResult saveEventTicketDetailResult = new SaveEventTicketDetailResult();
            var checkForEventTicketDetail = _eventTicketDetailRepository.Get(command.Id);
            if (checkForEventTicketDetail != null)
            {
                EventTicketDetail = new EventTicketDetail
                {
                    Id = checkForEventTicketDetail.Id,
                    EventDetailId = command.EventDetailId,
                    TicketCategoryId = command.TicketCategoryId,
                    IsEnabled = command.IsEnabled,
                    CreatedUtc = DateTime.UtcNow,
                    CreatedBy = command.ModifiedBy
                };
            }
            else
            {
                EventTicketDetail = new EventTicketDetail
                {
                    Id = command.Id,
                    EventDetailId = command.EventDetailId,
                    TicketCategoryId = command.TicketCategoryId,
                    IsEnabled = command.IsEnabled,
                    CreatedUtc = DateTime.UtcNow,
                    CreatedBy = command.ModifiedBy
                };
            }

            FIL.Contracts.DataModels.EventTicketDetail result = _eventTicketDetailRepository.Save(EventTicketDetail);
            saveEventTicketDetailResult.Id = result.Id;
            return Task.FromResult<ICommandResult>(saveEventTicketDetailResult);
        }
    }
}