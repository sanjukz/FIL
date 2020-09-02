using FIL.Api.Repositories;
using FIL.Api.Repositories.Redemption;
using FIL.Contracts.Commands.Redemption;
using FIL.Contracts.DataModels;
using FIL.Contracts.DataModels.Redemption;
using FIL.Contracts.Interfaces.Commands;
using MediatR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.Redemption
{
    public class GuideDetailsCommandHandler : BaseCommandHandlerWithResult<GuideDetailsCommand, GuideDetailsCommandResult>
    {
        private readonly IGuideDetailsRepository _GuideDetailsRepository;
        private readonly IUserRepository _UserRepository;
        private readonly IIPDetailRepository _IpDetailRepository;
        private readonly FIL.Logging.ILogger _logger;
        private readonly IGuidePlaceMappingsRepository _GuidePlaceMappings;
        private readonly ICountryRepository _countryRepository;
        private readonly IGuideServicesRepository _GuideServicesRepository;
        private readonly IGuideFinanceMappingsRepository _GuideFinanceMappingsRepository;
        private readonly IMasterFinanceDetailsRepository _MasterFinanceDetailsRepository;
        private readonly IGuideDocumentMappingsRepository _guideDocumentMappingsRepository;
        private readonly IUserAddressDetailRepository _userAddressDetailRepository;
        private readonly IServicesRepository _ServicesRepository;
        private readonly IZipcodeRepository _zipcodeRepository;
        private readonly IMediator _mediator;

        public GuideDetailsCommandHandler(IGuideDetailsRepository GuideDetailsRepository,
            IGuidePlaceMappingsRepository GuidePlaceMappings,
            IGuideServicesRepository GuideServicesRepository,
            ICountryRepository countryRepository,
            IMasterFinanceDetailsRepository MasterFinanceDetailsRepository,
            IGuideFinanceMappingsRepository GuideFinanceMappingsRepository,
            IUserRepository IserRepository,
            IIPDetailRepository IpDetailRepository,
            IServicesRepository ServicesRepository,
            IGuideDocumentMappingsRepository guideDocumentMappingsRepository,
            IUserAddressDetailRepository userAddressDetailRepository,
            IZipcodeRepository zipcodeRepository,
            IUserRepository UserRepository,
            FIL.Logging.ILogger logger,
            IMediator mediator)
           : base(mediator)
        {
            _GuideDetailsRepository = GuideDetailsRepository;
            _UserRepository = UserRepository;
            _IpDetailRepository = IpDetailRepository;
            _countryRepository = countryRepository;
            _GuidePlaceMappings = GuidePlaceMappings;
            _GuideServicesRepository = GuideServicesRepository;
            _GuideFinanceMappingsRepository = GuideFinanceMappingsRepository;
            _MasterFinanceDetailsRepository = MasterFinanceDetailsRepository;
            _guideDocumentMappingsRepository = guideDocumentMappingsRepository;
            _ServicesRepository = ServicesRepository;
            _userAddressDetailRepository = userAddressDetailRepository;
            _zipcodeRepository = zipcodeRepository;
            _logger = logger;
            _mediator = mediator;
        }

        protected override async Task<ICommandResult> Handle(GuideDetailsCommand command)
        {
            GuideDetailsCommandResult guideDetailsCommandResult = new GuideDetailsCommandResult();
            try
            {
                var user = _UserRepository.GetByEmailAndChannel(command.EmailId, Contracts.Enums.Channels.Feel, Contracts.Enums.SignUpMethods.None);
                var country = _countryRepository.GetByAltId(new Guid(command.TaxCountry));
                if (user == null)
                {
                    user = new FIL.Contracts.DataModels.User
                    {
                        AltId = Guid.NewGuid(),
                        Email = command.EmailId,
                        Password = Guid.NewGuid().ToString(),
                        RolesId = 20,// new guideID in Guid
                        CreatedBy = command.ModifiedBy,
                        CreatedUtc = DateTime.UtcNow,
                        UserName = command.EmailId,
                        FirstName = command.FirstName,
                        LastName = command.LastName,
                        PhoneCode = command.CountryCode,
                        PhoneNumber = command.MobileNo,
                        ChannelId = Contracts.Enums.Channels.Feel,
                        SignUpMethodId = Contracts.Enums.SignUpMethods.None,
                        IsEnabled = true,
                        LockOutEnabled = false,
                        AccessFailedCount = 0,
                        LoginCount = 0,
                        PhoneConfirmed = true,
                        IsActivated = true
                    };
                    user = _UserRepository.Save(user);
                }

                var zipCode = _zipcodeRepository.GetByZipcode(command.ZipCode);
                if (zipCode == null)
                {
                    zipCode = new Zipcode
                    {
                        AltId = Guid.NewGuid(),
                        CityId = command.CityId,
                        CreatedBy = command.ModifiedBy,
                        CreatedUtc = DateTime.UtcNow,
                        IsEnabled = true,
                        Postalcode = command.ZipCode,
                        ModifiedBy = command.ModifiedBy
                    };
                    zipCode = _zipcodeRepository.Save(zipCode);
                }

                var userAddress = _userAddressDetailRepository.GetByUserId(user.Id).FirstOrDefault();
                UserAddressDetail userAddressDetail = new UserAddressDetail
                {
                    Id = userAddress != null ? userAddress.Id : 0,
                    AddressLine1 = command.AddressLineOne,
                    AddressLine2 = command.AddressLineTwo,
                    AddressTypeId = Contracts.Enums.AddressTypes.Billing,
                    AltId = Guid.NewGuid(),
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    IsEnabled = true,
                    CityId = command.CityId,
                    PhoneCode = user.PhoneCode,
                    UserId = user.Id,
                    IsDefault = true,
                    PhoneNumber = user.PhoneNumber,
                    Zipcode = zipCode.Id,
                    CreatedBy = command.ModifiedBy,
                    CreatedUtc = DateTime.UtcNow
                };
                if (userAddress != null)
                {
                    userAddress.UpdatedUtc = DateTime.UtcNow;
                    userAddress.ModifiedBy = command.ModifiedBy;
                    userAddress.UpdatedBy = command.ModifiedBy;
                }
                userAddressDetail = _userAddressDetailRepository.Save(userAddressDetail);

                var guideDetails = _GuideDetailsRepository.GetByUserId(user.Id);
                Redemption_GuideDetails GuideDetails = new Redemption_GuideDetails
                {
                    ApproveStatusId = Contracts.Enums.ApproveStatus.Pending,
                    Id = guideDetails != null ? guideDetails.Id : 0,
                    IsEnabled = false,
                    UserAddressDetailId = userAddressDetail.Id,
                    LanguageId = String.Join(",", command.LanguageIds.ToArray()),
                    UserId = user.Id,
                    CreatedBy = command.ModifiedBy,
                    ModifiedBy = command.ModifiedBy,
                    CreatedUtc = DateTime.UtcNow
                };
                if (guideDetails != null)
                {
                    guideDetails.UpdatedUtc = DateTime.UtcNow;
                    guideDetails.ModifiedBy = command.ModifiedBy;
                    guideDetails.UpdatedBy = command.ModifiedBy;
                }
                GuideDetails = _GuideDetailsRepository.Save(GuideDetails);
                foreach (int eventId in command.EventIDs)
                {
                    Redemption_GuidePlaceMappings GuidePlaceMappings = new Redemption_GuidePlaceMappings
                    {
                        ApproveStatusId = 1,
                        EventId = Convert.ToInt64(eventId),
                        GuideId = GuideDetails.Id,
                        IsEnabled = true,
                        CreatedBy = command.ModifiedBy,
                        ModifiedBy = command.ModifiedBy,
                        CreatedUtc = DateTime.UtcNow
                    };
                    GuidePlaceMappings = _GuidePlaceMappings.Save(GuidePlaceMappings);
                }

                var guideServices = _GuideServicesRepository.GetAllByGuideId(GuideDetails.Id);
                if (guideServices != null)
                {
                    foreach (Contracts.DataModels.Redemption.Redemption_GuideServices service in guideServices)
                    {
                        _GuideServicesRepository.Delete(service);
                    }
                }

                foreach (Contracts.DataModels.Redemption.Services service in command.Services)
                {
                    var currentService = service;
                    currentService.Id = 0;
                    currentService.IsEnabled = true;
                    currentService.CreatedBy = command.ModifiedBy;
                    currentService.CreatedUtc = DateTime.UtcNow;
                    currentService = _ServicesRepository.Save(service);
                    Redemption_GuideServices GuideServices = new Redemption_GuideServices
                    {
                        GuideId = GuideDetails.Id,
                        IsEnabled = true,
                        Notes = command.Notes,
                        ServiceId = currentService.Id,
                        CreatedBy = command.ModifiedBy,
                        ModifiedBy = command.ModifiedBy,
                        CreatedUtc = DateTime.UtcNow
                    };
                    GuideServices = _GuideServicesRepository.Save(GuideServices);
                }

                MasterFinanceDetails masterFinanceDetails = new MasterFinanceDetails
                {
                    AccountNumber = command.AccountNumber,
                    AccountTypeId = command.AccountType,
                    BankAccountTypeId = command.BankAccountType,
                    BankName = command.BankName,
                    BranchCode = command.SwiftCode,
                    CountryId = country.Id,
                    CurrenyId = command.CurrencyId,
                    IsEnabled = true,
                    StateId = command.State,
                    TaxId = command.TaxNumber,
                    RoutingNumber = command.RoutingNumber,
                    CreatedBy = command.ModifiedBy,
                    ModifiedBy = command.ModifiedBy,
                    CreatedUtc = DateTime.UtcNow
                };
                masterFinanceDetails = _MasterFinanceDetailsRepository.Save(masterFinanceDetails);

                var AllGuideFinanceMappings = _GuideFinanceMappingsRepository.GetAllByGuideId(GuideDetails.Id);
                foreach (Contracts.DataModels.Redemption.Redemption_GuideFinanceMappings guideFinanceMappings in AllGuideFinanceMappings)
                {
                    _GuideFinanceMappingsRepository.Delete(guideFinanceMappings);
                }
                Redemption_GuideFinanceMappings GuideFinanceMappings = new Redemption_GuideFinanceMappings
                {
                    GuideId = GuideDetails.Id,
                    IsEnabled = true,
                    CreatedBy = command.ModifiedBy,
                    CreatedUtc = DateTime.UtcNow,
                    MasterFinanceDetailId = masterFinanceDetails.Id
                };
                GuideFinanceMappings = _GuideFinanceMappingsRepository.Save(GuideFinanceMappings);

                var guideDocuments = _guideDocumentMappingsRepository.GetAllByGuideId(GuideDetails.Id);
                foreach (Contracts.DataModels.Redemption.Redemption_GuideDocumentMappings guideDocumentMappings in guideDocuments)
                {
                    _guideDocumentMappingsRepository.Delete(guideDocumentMappings);
                }
                var guideDocument = new Redemption_GuideDocumentMappings
                {
                    GuideId = GuideDetails.Id,
                    IsEnabled = true,
                    CreatedBy = command.ModifiedBy,
                    ModifiedBy = command.ModifiedBy,
                    CreatedUtc = DateTime.UtcNow,
                    DocumentID = command.Document,
                    DocumentSouceURL = "",
                };
                _guideDocumentMappingsRepository.Save(guideDocument);

                return new GuideDetailsCommandResult
                {
                    UserId = user.Id,
                    Success = true
                };
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                return new GuideDetailsCommandResult
                {
                    Success = false
                };
            }
        }
    }
}