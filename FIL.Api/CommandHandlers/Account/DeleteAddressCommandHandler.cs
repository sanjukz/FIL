using FIL.Api.Repositories;
using FIL.Contracts.Commands.Account;
using FIL.Contracts.DataModels;
using MediatR;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.Account
{
    public class DeleteAddressCommandHandler : BaseCommandHandler<DeleteAddressCommand>
    {
        private readonly IUserAddressDetailRepository _userAddressDetailRepository;

        public DeleteAddressCommandHandler(IUserAddressDetailRepository userAddressDetailRepository, IMediator mediator)
            : base(mediator)
        {
            _userAddressDetailRepository = userAddressDetailRepository;
        }

        protected override async Task Handle(DeleteAddressCommand command)
        {
            UserAddressDetail addressDetail = _userAddressDetailRepository.GetByAltId(command.AltId);
            if (addressDetail != null)
            {
                _userAddressDetailRepository.Delete(addressDetail);
            }
        }
    }
}