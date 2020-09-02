using FIL.Api.Repositories;
using FIL.Contracts.Commands.Account;
using FIL.Contracts.DataModels;
using MediatR;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.Account
{
    public class ChangeFullNameCommandHandler : BaseCommandHandler<ChangeFullNameCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMediator _mediator;

        public ChangeFullNameCommandHandler(IUserRepository userRepository, IMediator mediator)
            : base(mediator)
        {
            _userRepository = userRepository;
            _mediator = mediator;
        }

        protected override async Task Handle(ChangeFullNameCommand command)
        {
            User user = _userRepository.GetByAltId(command.AltId);
            if (user != null && command.ModifiedBy == user.AltId && !command.isMarketingOptUpdate)
            {
                user.FirstName = command.FirstName;
                user.LastName = command.LastName;
                user.ModifiedBy = command.ModifiedBy;
                _userRepository.Save(user);
            }
            else
            {
                user.IsRASVMailOPT = command.isMarketingOptIn;
                _userRepository.Save(user);
                await _mediator.Publish(new Events.Event.HubSpot.VisitorInfoEvent(user)); // Update marketing Opt-in
            }
        }
    }
}