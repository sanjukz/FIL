using FIL.Api.Repositories;
using FIL.Contracts.Commands.UserCreation;
using FIL.Contracts.Interfaces.Commands;
using MediatR;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.UserCreation
{
    public class ResetUserPasswordCommandHandler : BaseCommandHandlerWithResult<ResetUserPasswordCommand, ResetUserPasswordCommandResult>
    {
        private readonly IUserRepository _userRepository;

        public ResetUserPasswordCommandHandler(IUserRepository userRepository, IMediator mediator) : base(mediator)
        {
            _userRepository = userRepository;
        }

        protected override Task<ICommandResult> Handle(ResetUserPasswordCommand command)
        {
            ResetUserPasswordCommandResult resetUserPasswordCommandResult = new ResetUserPasswordCommandResult();
            var userDetails = _userRepository.GetByAltId(command.UserAltId);
            userDetails.Password = command.Password;
            userDetails.ModifiedBy = command.ModifiedBy;
            var result = _userRepository.Save(userDetails);

            if (result != null)
            {
                resetUserPasswordCommandResult.Id = result.Id;
                resetUserPasswordCommandResult.Success = true;
            }
            else
            {
                resetUserPasswordCommandResult.Id = -1;
                resetUserPasswordCommandResult.Success = false;
            }

            return Task.FromResult<ICommandResult>(resetUserPasswordCommandResult);
        }
    }
}