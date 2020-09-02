using FIL.Api.Repositories;
using FIL.Contracts.Commands.EventCreation;
using FIL.Contracts.DataModels;
using MediatR;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.EventCreation
{
    public class DeleteSubEventCommandHandler : BaseCommandHandler<DeleteSubevent>
    {
        private readonly IEventDetailRepository _eventDetailRepository;

        public DeleteSubEventCommandHandler(IEventDetailRepository eventDetailRepository, IMediator mediator)
            : base(mediator)
        {
            _eventDetailRepository = eventDetailRepository;
        }

        protected override async Task Handle(DeleteSubevent command)
        {
            EventDetail eventDetail = _eventDetailRepository.GetById(command.Id);
            if (eventDetail != null)
            {
                _eventDetailRepository.Delete(eventDetail);
            }
        }
    }
}