using FIL.Api.Repositories;
using FIL.Contracts.Commands.BoxOffice;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.BoxOffice
{
    public class SubmitUserDetailCommandHandler : BaseCommandHandler<SubmitUserDetailCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IBoxofficeUserAdditionalDetailRepository _boxofficeUserAdditionalDetailRepository;
        private readonly IZsuiteUserFeeDetailRepository _zsuiteUserFeeDetailRepository;

        public SubmitUserDetailCommandHandler(IUserRepository userRepository, IMediator mediator, ICountryRepository countryRepository, IBoxofficeUserAdditionalDetailRepository boxofficeUserAdditionalDetailRepository, IZsuiteUserFeeDetailRepository zsuiteUserFeeDetailRepository) : base(mediator)
        {
            _userRepository = userRepository;
            _countryRepository = countryRepository;
            _boxofficeUserAdditionalDetailRepository = boxofficeUserAdditionalDetailRepository;
            _zsuiteUserFeeDetailRepository = zsuiteUserFeeDetailRepository;
        }

        protected override async Task Handle(SubmitUserDetailCommand command)
        {
            var userData = new User
            {
                AltId = Guid.NewGuid(),
                Email = command.Email,
                Password = command.Password,
                RolesId = Convert.ToInt32(command.RoleId),
                CreatedBy = command.ModifiedBy,
                CreatedUtc = DateTime.UtcNow,
                UserName = command.Email,
                FirstName = command.FirstName,
                LastName = command.LastName,
                PhoneCode = null,
                PhoneNumber = null,
                ChannelId = Channels.ZSuite,
                SignUpMethodId = SignUpMethods.Regular,
                IsEnabled = true
            };
            var user = _userRepository.Save(userData);
            if (command.IsBOUser)
            {
                var country = _countryRepository.GetByAltId((Guid)(command.CountryId));
                var boxOfficeUserData = new BoxofficeUserAdditionalDetail
                {
                    UserId = user.Id,
                    ParentId = command.ParentId,
                    UserType = command.UserType,
                    IsETicket = 0,
                    IsChildTicket = Convert.ToBoolean(command.IsChildTicket),
                    IsSrCitizenTicket = Convert.ToBoolean(command.IsSrTicket),
                    TicketLimit = 10,
                    ChildTicketLimit = 0,
                    ChildForPerson = 0,
                    SrCitizenLimit = 0,
                    SrCitizenPerson = 0,
                    CityId = 1,
                    Address = country.Name,
                    ContactNumber = "",
                    CountryId = country.Id,
                    CreatedBy = command.ModifiedBy,
                    CreatedUtc = DateTime.UtcNow,
                    IsEnabled = true,
                };
                var boxOfficeuser = _boxofficeUserAdditionalDetailRepository.Save(boxOfficeUserData);

                foreach (var feeDetails in command.FeeDetail)
                {
                    if (feeDetails.PaymentId != 0)
                    {
                        var ZsuiteFeeDetails = new ZsuiteUserFeeDetail
                        {
                            BoxOfficeUserAdditionalDetailId = boxOfficeuser.Id,
                            ZsuitePaymentOptionId = feeDetails.PaymentId,
                            Fee = feeDetails.Value,
                            IsEnabled = true,
                            ModifiedBy = command.ModifiedBy
                        };
                        _zsuiteUserFeeDetailRepository.Save(ZsuiteFeeDetails);
                    }
                }
            }
        }
    }
}