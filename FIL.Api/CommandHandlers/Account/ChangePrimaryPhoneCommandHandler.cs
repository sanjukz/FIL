using FIL.Api.Repositories;
using FIL.Contracts.Commands.Account;
using FIL.Contracts.DataModels;
using MediatR;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.Account
{
    public class ChangePrimaryPhoneCommandHandler : BaseCommandHandler<ChangePrimaryPhoneCommand>
    {
        private readonly IUserRepository _userRepository;

        public ChangePrimaryPhoneCommandHandler(IUserRepository userRepository, IMediator mediator)
            : base(mediator)
        {
            _userRepository = userRepository;
        }

        protected override async Task Handle(ChangePrimaryPhoneCommand command)
        {
            User user = _userRepository.GetByAltId(command.AltId);
            if (user != null && command.ModifiedBy == user.AltId)
            {
                user.PhoneCode = command.PhoneCode;
                user.PhoneNumber = command.PhoneNumber;
                user.ModifiedBy = command.ModifiedBy;
                _userRepository.Save(user);
            }
        }
    }
}