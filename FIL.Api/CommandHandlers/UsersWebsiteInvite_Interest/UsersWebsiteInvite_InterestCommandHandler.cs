using FIL.Api.Repositories;
using FIL.Contracts.Commands._usersWebsiteInvite_Interest;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.UsersWebsiteInvite_Interest
{
    public class UsersWebsiteInvite_InterestCommandHandler : BaseCommandHandler<UsersWebsiteInvite_InterestCommand>
    {
        private readonly IUsersWebsiteInvite_InterestRepository _usersWebsiteInvite_InterestRepository;
        private readonly IMediator _mediator;

        public UsersWebsiteInvite_InterestCommandHandler(IUsersWebsiteInvite_InterestRepository usersWebsiteInvite_InterestRepository, IMediator mediator) : base(mediator)
        {
            _usersWebsiteInvite_InterestRepository = usersWebsiteInvite_InterestRepository;
            _mediator = mediator;
        }

        protected override async Task Handle(UsersWebsiteInvite_InterestCommand command)
        {
            var userinviteInterest = _usersWebsiteInvite_InterestRepository.GetByEmail(command.Email);
            if (userinviteInterest == null)
            {
                var userwebsiteInviteInterest = new FIL.Contracts.Models.UsersWebsiteInvite_Interest()
                {
                    Email = command.Email,
                    FirstName = command.FirstName,
                    LastName = command.LastName,
                    Nationality = command.Nationality,
                    CreatedBy = command.ModifiedBy,
                    CreatedUtc = DateTime.UtcNow,
                    ModifiedBy = command.ModifiedBy,
                    ModifiedUtc = DateTime.UtcNow
                };
                _usersWebsiteInvite_InterestRepository.Save(userwebsiteInviteInterest);
            }
            else
            {
                _usersWebsiteInvite_InterestRepository.Save(userinviteInterest);
            }
        }
    }
}