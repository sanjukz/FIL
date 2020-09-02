using FIL.Contracts.Commands.CartTrack;
using MediatR;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.CartTrack
{
    public class CartTrackCommandHandler : BaseCommandHandler<CartTrackCommand>
    {
        private readonly IMediator _mediator;

        public CartTrackCommandHandler(IMediator mediator)
            : base(mediator)
        {
            _mediator = mediator;
        }

        protected override async Task Handle(CartTrackCommand command)
        {
            await _mediator.Publish(new Events.Event.HubSpot.CartInfoEvent(new Contracts.Models.CartTrack
            {
                HubspotUTK = command.HubspotUTK,
                UserAltId = command.UserAltId,
                EventDetailId = command.EventDetailId
            }));
        }
    }
}