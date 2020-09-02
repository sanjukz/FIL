using FIL.Api.Repositories;
using FIL.Contracts.Commands.RegistrationEvent;
using MediatR;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.RegistrationEvent
{
    public class RegistrationEventSaveTransactionCommandHandler : BaseCommandHandler<RegistrationEventSaveTransactionCommand>
    {
        private readonly IRegistrationEventUserMappingRepository _registrationEventUserMappingRepository;

        public RegistrationEventSaveTransactionCommandHandler(IRegistrationEventUserMappingRepository registrationEventUserMappingRepository, IMediator mediator)
            : base(mediator)
        {
            _registrationEventUserMappingRepository = registrationEventUserMappingRepository;
        }

        protected override async Task Handle(RegistrationEventSaveTransactionCommand command)
        {
            var id = System.Convert.ToInt32(command.RegistrationEventUserId);
            var registrationEventUserDetails = _registrationEventUserMappingRepository.Get(id);
            registrationEventUserDetails.TransactionId = command.TransactionId;
            _registrationEventUserMappingRepository.Save(registrationEventUserDetails);
        }
    }
}