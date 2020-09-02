using FIL.Api.Repositories;
using FIL.Contracts.Commands.Account;
using FIL.Contracts.DataModels;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.Account
{
    public class SaveCardCommandHandler : BaseCommandHandler<SaveCardCommand>
    {
        private readonly IUserCardDetailRepository _userCardDetailRepository;
        private readonly IUserRepository _userRepository;

        public SaveCardCommandHandler(IUserCardDetailRepository userCardDetailRepository, IUserRepository userRepository, IMediator mediator)
            : base(mediator)
        {
            _userCardDetailRepository = userCardDetailRepository;
            _userRepository = userRepository;
        }

        protected override async Task Handle(SaveCardCommand command)
        {
            var user = _userRepository.GetByAltId(command.UserAltId);
            if (user != null)
            {
                var _cardDetail = new UserCardDetail
                {
                    UserId = user.Id,
                    AltId = Guid.NewGuid(),
                    NameOnCard = command.NameOnCard,
                    CardNumber = command.CardNumber,
                    ExpiryMonth = command.ExpiryMonth,
                    ExpiryYear = command.ExpiryYear,
                    CardTypeId = command.CardTypeId,
                    ModifiedBy = command.ModifiedBy,
                    IsEnabled = true
                };

                _userCardDetailRepository.Save(_cardDetail);
            }
        }
    }
}