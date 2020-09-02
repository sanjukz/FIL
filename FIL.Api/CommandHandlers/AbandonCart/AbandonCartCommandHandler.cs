using FIL.Api.Events.Event.HubSpot;
using FIL.Api.Repositories;
using FIL.Contracts.Commands.HubSpot;
using FIL.Contracts.DataModels;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.AbandonCart
{
    public class AbandonCartCommandHandler : BaseCommandHandler<AbandonCartCommand>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionDetailRepository _transactionDetailsRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IMediator _mediator;
        private readonly IHubspotCartTrackRepository _hubspotCartTrackRepository;

        public AbandonCartCommandHandler(ITransactionRepository transactionRepository,
        ITransactionDetailRepository transactionDetailsRepository,
        IEventTicketAttributeRepository eventTicketAttributeRepository,
        IEventTicketDetailRepository eventTicketDetailRepository,
        IEventDetailRepository eventDetailRepository,
        IEventRepository eventRepository,
        IMediator mediator,
        IHubspotCartTrackRepository hubspotCartTrackRepository)
            : base(mediator)
        {
            _eventRepository = eventRepository;
            _eventDetailRepository = eventDetailRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _transactionDetailsRepository = transactionDetailsRepository;
            _transactionRepository = transactionRepository;
            _hubspotCartTrackRepository = hubspotCartTrackRepository;
            _mediator = mediator;
        }

        protected override async Task Handle(AbandonCartCommand command)
        {
            IEnumerable<HubspotCartTrack> abandonCart = _hubspotCartTrackRepository.GetAbandonCart();
            var emailIds = abandonCart.Select(s => s.EmailId).Distinct();
            List<string> lstEmailIds = new List<string>(emailIds);

            await _mediator.Publish(new AbandonCartEvent
            {
                AbandonCart = abandonCart
            });
        }
    }
}