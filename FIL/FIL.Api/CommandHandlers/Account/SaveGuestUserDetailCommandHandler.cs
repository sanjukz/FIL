using FIL.Api.Repositories;
using FIL.Contracts.Commands.Account;
using FIL.Contracts.DataModels;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.Account
{
    public class SaveGuestUserDetailCommandHandler : BaseCommandHandler<SaveGuestUserDetailCommand>
    {
        private readonly IGuestUserAdditionalDetailRepository _guestUserAdditionalDetailRepository;

        public SaveGuestUserDetailCommandHandler(IGuestUserAdditionalDetailRepository guestUserAdditionalDetailRepository, IMediator mediator)
            : base(mediator)
        {
            _guestUserAdditionalDetailRepository = guestUserAdditionalDetailRepository;
        }

        protected override Task Handle(SaveGuestUserDetailCommand command)
        {
            _guestUserAdditionalDetailRepository.Save(new GuestUserAdditionalDetail
            {
                UserId = command.UserId,
                FirstName = command.FirstName,
                AltId = Guid.NewGuid(),
                LastName = command.LastName,
                DocumentNumber = command.DocumentNumber,
                DocumentType = command.DocumentType,
                Nationality = command.Nationality,
                IsEnabled = true,
                ModifiedBy = command.ModifiedBy,
            });
            return Task.FromResult(0);
        }
    }
}