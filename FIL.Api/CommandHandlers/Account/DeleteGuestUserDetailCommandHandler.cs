using FIL.Api.Repositories;
using FIL.Contracts.Commands.Account;
using MediatR;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.Account
{
    public class DeleteGuestUserDetailCommandHandler : BaseCommandHandler<DeleteGuestUserDetailCommand>
    {
        private readonly IGuestUserAdditionalDetailRepository _guestUserAdditionalDetailRepository;

        public DeleteGuestUserDetailCommandHandler(IGuestUserAdditionalDetailRepository guestUserAdditionalDetailRepository, IMediator mediator)
            : base(mediator)
        {
            _guestUserAdditionalDetailRepository = guestUserAdditionalDetailRepository;
        }

        protected override async Task Handle(DeleteGuestUserDetailCommand command)
        {
            var guestUserDetails = _guestUserAdditionalDetailRepository.GetByAltId(command.UserId);
            _guestUserAdditionalDetailRepository.Delete(guestUserDetails);
        }
    }
}