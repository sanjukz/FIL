using FIL.Api.Repositories;
using FIL.Contracts.Commands.EventCreation;
using FIL.Contracts.DataModels;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.EventCreation
{
    public class SaveEventTicketCommandHandler : BaseCommandHandler<SaveEventTicketCommand>
    {
        private readonly ITicketCategoryRepository _ticketRepository;

        public SaveEventTicketCommandHandler(ITicketCategoryRepository ticketRepository, IMediator mediator)
            : base(mediator)
        {
            _ticketRepository = ticketRepository;
        }

        protected override async Task Handle(SaveEventTicketCommand command)
        {
            var eventData = new TicketCategory
            {
                Name = command.Name,
                ModifiedBy = command.ModifiedBy,
                CreatedUtc = DateTime.UtcNow,
                IsEnabled = command.IsEnabled
            };
            _ticketRepository.Save(eventData);
            return;
        }
    }
}