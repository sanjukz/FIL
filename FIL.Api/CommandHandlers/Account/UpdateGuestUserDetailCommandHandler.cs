using FIL.Api.Repositories;
using FIL.Contracts.Commands.Account;
using MediatR;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.Account
{
    public class UpdateGuestUserDetailCommandHandler : BaseCommandHandler<UpdateGuestUserDetailCommand>
    {
        private readonly IGuestUserAdditionalDetailRepository _guestUserAdditionalDetailRepository;

        public UpdateGuestUserDetailCommandHandler(IGuestUserAdditionalDetailRepository guestUserAdditionalDetailRepository, IMediator mediator)
            : base(mediator)
        {
            _guestUserAdditionalDetailRepository = guestUserAdditionalDetailRepository;
        }

        protected override async Task Handle(UpdateGuestUserDetailCommand command)
        {
            var guestUserDetails = _guestUserAdditionalDetailRepository.GetByAltId(command.UserId);
            guestUserDetails.FirstName = command.FirstName;
            guestUserDetails.LastName = command.LastName;
            guestUserDetails.Nationality = command.Nationality;
            guestUserDetails.DocumentType = command.DocumentType;
            guestUserDetails.DocumentNumber = command.DocumentNumber;
            _guestUserAdditionalDetailRepository.Save(guestUserDetails);
        }
    }
}