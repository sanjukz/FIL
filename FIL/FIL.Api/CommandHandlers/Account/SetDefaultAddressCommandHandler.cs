using FIL.Api.Repositories;
using FIL.Contracts.Commands.Account;
using FIL.Contracts.DataModels;
using MediatR;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.Account
{
    public class SetDefaultAddressCommandHandler : BaseCommandHandler<SetDefaultAddressCommand>
    {
        private readonly IUserAddressDetailRepository _userAddressDetailRepository;

        public SetDefaultAddressCommandHandler(IUserAddressDetailRepository userAddressDetailRepository, IMediator mediator)
            : base(mediator)
        {
            _userAddressDetailRepository = userAddressDetailRepository;
        }

        protected override async Task Handle(SetDefaultAddressCommand command)
        {
            UserAddressDetail addressDetail = _userAddressDetailRepository.GetByAltId(command.AltId);
            if (addressDetail != null)
            {
                if (command.MakeDefault)
                {
                    var addresses = _userAddressDetailRepository.GetByUserId(addressDetail.UserId);
                    foreach (var item in addresses)
                    {
                        if (addressDetail.AddressTypeId == item.AddressTypeId)
                        {
                            item.IsDefault = item.AltId == command.AltId ? true : false;
                            item.ModifiedBy = command.ModifiedBy;
                            _userAddressDetailRepository.Save(item);
                        }
                    }
                }
                else
                {
                    addressDetail.IsDefault = false;
                    addressDetail.ModifiedBy = command.ModifiedBy;
                    _userAddressDetailRepository.Save(addressDetail);
                }
            }
        }
    }
}