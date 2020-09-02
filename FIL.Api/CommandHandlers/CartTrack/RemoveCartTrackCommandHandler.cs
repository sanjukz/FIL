using FIL.Contracts.Commands.CartTrack;
using MediatR;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.CartTrack
{
    public class RemoveCartTrackCommandHandler : BaseCommandHandler<RemoveCartTrackCommand>
    {
        private readonly IMediator _mediator;

        public RemoveCartTrackCommandHandler(IMediator mediator)
            : base(mediator)
        {
            _mediator = mediator;
        }

        protected override async Task Handle(RemoveCartTrackCommand command)
        {
            await _mediator.Publish(new Events.Event.HubSpot.RemoveCartEvent(new Contracts.Models.CartTrack
            {
                HubspotUTK = command.HubspotUTK
            }));
        }
    }
}