using FIL.Api.Repositories;
using FIL.Contracts.Commands.ResetPassword;
using FIL.Contracts.DataModels;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.ResetPassword
{
    public class ResetPasswordCommandHandler : BaseCommandHandler<ResetPasswordCommand>
    {
        private readonly IUserRepository _userRepository;

        public ResetPasswordCommandHandler(IUserRepository userRepository, IMediator mediator)
            : base(mediator)
        {
            _userRepository = userRepository;
        }

        protected override async Task Handle(ResetPasswordCommand command)
        {
            User user = _userRepository.GetByAltId(command.AltId);
            if (user != null)
            {
                user.AccessFailedCount = 0;
                user.LockOutEnabled = false;
                user.LockOutEndDateUtc = DateTime.UtcNow;
                user.Password = command.PasswordHash;
                _userRepository.Save(user);
            }
        }
    }
}