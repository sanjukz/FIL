using FIL.Api.Repositories;
using FIL.Contracts.Commands.Users;
using FIL.Contracts.DataModels;
using MediatR;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.Users
{
    public class ActivateUserCommandHandler : BaseCommandHandler<ActivateUserCommand>
    {
        private readonly IUserRepository _userRepository;

        public ActivateUserCommandHandler(IUserRepository userRepository, IMediator mediator)
            : base(mediator)
        {
            _userRepository = userRepository;
        }

        protected override Task Handle(ActivateUserCommand command)
        {
            User user = _userRepository.GetByAltId(command.AltId);
            if (user != null)
            {
                user.Id = user.Id;
                user.IsActivated = true;
                _userRepository.Save(user);
            }
            return Task.FromResult(0);
        }
    }
}