using FIL.Api.Repositories;
using FIL.Contracts.Commands.Account;
using FIL.Contracts.DataModels;
using MediatR;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.Account
{
    public class ChangePasswordCommandHandler : BaseCommandHandler<ChangePasswordCommand>
    {
        private readonly IUserRepository _userRepository;

        public ChangePasswordCommandHandler(IUserRepository userRepository, IMediator mediator)
            : base(mediator)
        {
            _userRepository = userRepository;
        }

        protected override async Task Handle(ChangePasswordCommand command)
        {
            User user = _userRepository.GetByAltId(command.AltId);
            if (user != null && command.ModifiedBy == user.AltId)
            {
                user.Password = command.PasswordHash;
                user.ModifiedBy = command.ModifiedBy;
                _userRepository.Save(user);
            }
        }
    }
}