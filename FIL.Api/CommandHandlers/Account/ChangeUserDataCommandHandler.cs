using FIL.Api.Repositories;
using FIL.Contracts.Commands.Account;
using FIL.Contracts.DataModels;
using MediatR;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.Account
{
    public class ChangeUserDataCommandHandler : BaseCommandHandler<ChangeUserDataCommand>
    {
        private readonly IUserRepository _userRepository;

        public ChangeUserDataCommandHandler(IUserRepository userRepository, IMediator mediator)
            : base(mediator)
        {
            _userRepository = userRepository;
        }

        protected override async Task Handle(ChangeUserDataCommand command)
        {
            User user = _userRepository.GetByAltId(command.AltId);
            if (user != null && command.ModifiedBy == user.AltId)
            {
                user.Email = command.Email;
                user.Gender = command.Gender;
                user.DOB = command.DOB;
                user.ModifiedBy = command.ModifiedBy;
                _userRepository.Save(user);
            }
        }
    }
}