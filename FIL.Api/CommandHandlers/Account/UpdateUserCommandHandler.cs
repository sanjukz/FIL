using FIL.Api.Repositories;
using FIL.Contracts.Commands.Account;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Commands;
using FIL.Contracts.Models;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.Account
{
    public class UpdateUserCommandHandler : BaseCommandHandlerWithResult<UpdateUserCommand, UpdateUserCommandResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IFeelUserAdditionalDetailRepository _feelUserAdditionalDetailRepository;
        private readonly ICountryRepository _countryRepository;

        public UpdateUserCommandHandler(IUserRepository userRepository,
            IFeelUserAdditionalDetailRepository feelUserAdditionalDetailRepository, IMediator mediator, ICountryRepository countryRepository)
            : base(mediator)
        {
            _userRepository = userRepository;
            _feelUserAdditionalDetailRepository = feelUserAdditionalDetailRepository;
            _countryRepository = countryRepository;
        }

        protected override async Task<ICommandResult> Handle(UpdateUserCommand command)
        {
            UpdateUserCommandResult commandResult = new UpdateUserCommandResult();
            var userModel = _userRepository.GetByAltId(command.UserProfile.AltId);
            var userAdditionalModel = _feelUserAdditionalDetailRepository.GetByUserId(Convert.ToInt32(userModel.Id));

            if (!string.IsNullOrEmpty(command.UserProfile.FirstName) && !string.IsNullOrEmpty(command.UserProfile.LastName))
            {
                var countryAltId = Guid.Empty;
                if (!string.IsNullOrEmpty(command.UserProfile.PhoneCode))
                {
                    countryAltId = new Guid(command.UserProfile.PhoneCode.Split("~")[1]);
                    command.UserProfile.PhoneCode = command.UserProfile.PhoneCode.Split("~")[0];
                }
                else
                {
                    command.UserProfile.PhoneCode = "";
                }
                if (ShouldUpdate(userModel, null, command.UserProfile))
                {
                    var country = new Contracts.DataModels.Country();
                    if (countryAltId != Guid.Empty)
                    {
                        country = _countryRepository.GetByAltId(countryAltId);
                    }
                    userModel.FirstName = command.UserProfile.FirstName;
                    userModel.LastName = command.UserProfile.LastName;
                    userModel.PhoneCode = command.UserProfile.PhoneCode;
                    userModel.PhoneNumber = command.UserProfile.PhoneNumber;
                    userModel.CountryId = country != null ? country.Id : null as int?;
                    _userRepository.Save(userModel);
                }

                if (userAdditionalModel == null)
                {
                    SaveUserAdditionalDetail(userModel, command.UserProfile);
                }
                else
                {
                    if (ShouldUpdate(null, userAdditionalModel, command.UserProfile))
                    {
                        var formattedDate = "";
                        if (!string.IsNullOrEmpty(command.UserProfile.DOB))
                        {
                            var splittedDate = command.UserProfile.DOB.Split("/");
                            formattedDate = splittedDate[2] + "-" + splittedDate[1] + "-" + splittedDate[0];
                        }
                        if (!string.IsNullOrEmpty(command.UserProfile.Gender))
                        {
                            userAdditionalModel.Gender = (Gender)Enum.Parse(typeof(Gender), command.UserProfile.Gender);
                        }

                        userAdditionalModel.BirthDate = formattedDate;
                        userAdditionalModel = _feelUserAdditionalDetailRepository.Save(userAdditionalModel);
                    }
                }
            }
            var userProfileModel = GetUserProfile(userModel, userAdditionalModel);
            commandResult.UserProfile = userProfileModel;
            return commandResult;
        }

        private bool ShouldUpdate(Contracts.DataModels.User userModel, FeelUserAdditionalDetail userAdditionalDetail, UserProfile userProfile)
        {
            bool shouldUpdate = false;
            if (userModel != null)
            {
                if (userModel.FirstName != userProfile.FirstName || userModel.LastName != userProfile.LastName
                    || (userModel.PhoneCode == null ? "" : userModel.PhoneCode) != (userProfile.PhoneCode == null ? "" : userProfile.PhoneCode) || (userModel.PhoneNumber == null ? "" : userModel.PhoneNumber) != (userProfile.PhoneNumber == null ? "" : userProfile.PhoneNumber))
                {
                    shouldUpdate = true;
                }
            }
            if (userAdditionalDetail != null)
            {
                if ((userAdditionalDetail.BirthDate == null ? "" : userAdditionalDetail.BirthDate) != (userProfile.DOB == null ? "" : userProfile.DOB)
                    || (!string.IsNullOrEmpty(userProfile.Gender) ? (userAdditionalDetail.Gender != (Gender)Enum.Parse(typeof(Gender), userProfile.Gender)) : userAdditionalDetail.Gender != null))
                {
                    shouldUpdate = true;
                }
            }
            return shouldUpdate;
        }

        private void SaveUserAdditionalDetail(Contracts.DataModels.User userModel, UserProfile userProfile)
        {
            _feelUserAdditionalDetailRepository.Save(new FeelUserAdditionalDetail
            {
                UserId = Convert.ToInt32(userModel.Id),
                BirthDate = userProfile.DOB == null ? "" : userProfile.DOB,
                Gender = (Gender)Enum.Parse(typeof(Gender), userProfile.Gender),
                IsEnabled = true,
                ModifiedBy = userProfile.AltId
            });
        }

        private Contracts.Models.UserProfile GetUserProfile(Contracts.DataModels.User userModel, FeelUserAdditionalDetail userAdditionalModel)
        {
            Contracts.Models.UserProfile userProfile = new Contracts.Models.UserProfile();

            var countryAltId = Guid.NewGuid();

            if (userModel.CountryId != null && userModel.CountryId != 0)
            {
                var country = _countryRepository.Get(Convert.ToInt32(userModel.CountryId));
                countryAltId = country.AltId;
            }
            else
            {
                if (!string.IsNullOrEmpty(userModel.PhoneCode))
                {
                    var country = _countryRepository.GetByPhoneCode(userModel.PhoneCode);
                    countryAltId = country.AltId;
                }
            }

            var formattedDate = "";
            if (userAdditionalModel != null && !string.IsNullOrEmpty(userAdditionalModel.BirthDate))
            {
                var splittedDate = userAdditionalModel.BirthDate.Split("-");
                formattedDate = splittedDate[1] + "/" + splittedDate[2] + "/" + splittedDate[0];
            }

            userProfile.FirstName = userModel.FirstName;
            userProfile.LastName = userModel.LastName;
            userProfile.PhoneCode = (string.IsNullOrEmpty(userModel.PhoneCode) || countryAltId == Guid.NewGuid()) ? "" : userModel.PhoneCode + "~" + countryAltId.ToString().ToLower();
            userProfile.PhoneNumber = userModel.PhoneNumber;
            userProfile.Id = userProfile.Id;
            userProfile.DOB = formattedDate;
            userProfile.Gender = ((userAdditionalModel == null) || (userAdditionalModel != null && userAdditionalModel.Gender == null)) ? "" : Enum.GetName(typeof(Gender), userAdditionalModel.Gender);
            userProfile.AltId = userModel.AltId;
            userProfile.Email = userModel.Email;

            return userProfile;
        }
    }
}