using FIL.Api.Repositories;
using FIL.Contracts.Commands.MatchLayout;
using MediatR;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.MatchLayout
{
    public class UpdateMatchLayoutCommandHandler : BaseCommandHandler<UpdateMatchLayoutCommand>
    {
        private readonly IMatchLayoutSectionRepository _matchLayoutSectionRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;

        public UpdateMatchLayoutCommandHandler(IMatchLayoutSectionRepository matchLayoutSectionRepository, IEventTicketDetailRepository eventTicketDetailRepository, IEventTicketAttributeRepository eventTicketAttributeRepository, IMediator mediator)
            : base(mediator)
        {
            _matchLayoutSectionRepository = matchLayoutSectionRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
        }

        protected override async Task Handle(UpdateMatchLayoutCommand command)
        {
            var matchLayoutSectionModel = _matchLayoutSectionRepository.Get(command.SectionId);
            matchLayoutSectionModel.SectionName = command.SectionName;
            matchLayoutSectionModel.ModifiedBy = command.ModifiedBy;
            matchLayoutSectionModel.EntryGateId = command.EntryGateId;
            matchLayoutSectionModel.Capacity = command.Capacity;
            _matchLayoutSectionRepository.Save(matchLayoutSectionModel);

            var eventTicketDetailModel = _eventTicketDetailRepository.GetByTicketCategoryIdAndEventDetailId(matchLayoutSectionModel.TicketCategoryId, command.EventDetailId);
            if (eventTicketDetailModel != null)
            {
                var eventTicketAttributeModel = _eventTicketAttributeRepository.GetByEventTicketDetailId(eventTicketDetailModel.Id);
                eventTicketAttributeModel.Price = command.Price;
                eventTicketAttributeModel.CurrencyId = command.CurrencyId;
                eventTicketAttributeModel.LocalPrice = command.LocalPrice;
                eventTicketAttributeModel.LocalCurrencyId = command.LocalCurrencyId;
                eventTicketAttributeModel.AvailableTicketForSale = command.Capacity;
                eventTicketAttributeModel.RemainingTicketForSale = command.Capacity;
                eventTicketAttributeModel.IsSeatSelection = command.IsSeatSelection;
                eventTicketAttributeModel.ModifiedBy = command.ModifiedBy;
                _eventTicketAttributeRepository.Save(eventTicketAttributeModel);
            }
        }
    }
}