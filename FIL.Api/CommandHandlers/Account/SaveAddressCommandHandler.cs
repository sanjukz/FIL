using FIL.Api.Repositories;
using FIL.Contracts.Commands.Account;
using FIL.Contracts.DataModels;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.Account
{
    public class SaveAddressCommandHandler : BaseCommandHandler<SaveAddressCommand>
    {
        private readonly IUserAddressDetailRepository _userAddressDetailRepository;
        private readonly IUserRepository _userRepository;
        private readonly IZipcodeRepository _zipcodeRepository;

        public SaveAddressCommandHandler(IUserAddressDetailRepository userAddressDetailRepository, IUserRepository userRepository, IZipcodeRepository zipcodeRepository, IMediator mediator)
            : base(mediator)
        {
            _userAddressDetailRepository = userAddressDetailRepository;
            _userRepository = userRepository;
            _zipcodeRepository = zipcodeRepository;
        }

        protected override async Task Handle(SaveAddressCommand command)
        {
            var user = _userRepository.GetByAltId(command.UserAltId);
            var zipcode = _zipcodeRepository.GetByZipcode(command.Zipcode.ToString());
            if (zipcode == null)
            {
                var zipCode = new Zipcode
                {
                    AltId = Guid.NewGuid(),
                    Postalcode = command.Zipcode.ToString(),
                    IsEnabled = true
                };

                _zipcodeRepository.Save(zipCode);

                zipcode = _zipcodeRepository.GetByZipcode(command.Zipcode.ToString());
            }
            if (user != null && zipcode != null)
            {
                var addressDetail = new UserAddressDetail
                {
                    UserId = user.Id,
                    AltId = Guid.NewGuid(),
                    FirstName = command.FirstName,
                    LastName = command.LastName,
                    PhoneCode = command.PhoneCode,
                    PhoneNumber = command.PhoneNumber,
                    AddressLine1 = command.AddressLine1,
                    Zipcode = zipcode.Id,
                    AddressTypeId = command.AddressTypeId,
                    ModifiedBy = command.ModifiedBy,
                    IsEnabled = true,
                    CityId = null
                };

                _userAddressDetailRepository.Save(addressDetail);
            }
        }
    }
}