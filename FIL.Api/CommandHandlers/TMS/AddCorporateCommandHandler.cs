using FIL.Api.Repositories;
using FIL.Contracts.Commands.TMS;
using FIL.Contracts.DataModels;
using FIL.Contracts.Interfaces.Commands;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.TMS
{
    public class AddCorporateCommandHandler : BaseCommandHandlerWithResult<AddCorporateCommand, AddCorporateDataResult>
    {
        private readonly ISponsorRepository _sponsorRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly Logging.ILogger _logger;

        public AddCorporateCommandHandler(ISponsorRepository sponsorRepository, IMediator mediator, ICountryRepository countryRepository, Logging.ILogger logger)
            : base(mediator)
        {
            _sponsorRepository = sponsorRepository;
            _countryRepository = countryRepository;
            _logger = logger;
        }

        protected override Task<ICommandResult> Handle(AddCorporateCommand command)
        {
            AddCorporateDataResult addCorporateDataResult = new AddCorporateDataResult();
            try
            {
                if (!command.isEditCompany)
                {
                    var country = _countryRepository.Get(Convert.ToInt32(command.CountryId));
                    var sponsorDetails = _sponsorRepository.GetBySponsorName(command.SponsorName);
                    if (sponsorDetails == null)
                    {
                        var corporateData = new Sponsor
                        {
                            SponsorName = command.SponsorName,
                            FirstName = command.FirstName,
                            LastName = command.LastName,
                            Email = command.Email,
                            PhoneCode = command.PhoneCode,
                            PhoneNumber = command.PhoneNumber,
                            Address = command.Address,
                            CityId = command.CityId,
                            StateId = command.StateId,
                            CountryId = country.Name,
                            Zipcode = command.Zipcode,
                            SponsorTypeId = Contracts.Enums.SponsorType.Regular,
                            IsEnabled = true,
                            CreatedUtc = DateTime.UtcNow,
                            ModifiedBy = command.ModifiedBy,
                            IdType = command.IdType,
                            IdNumber = command.IdNumber,
                            UpdatedUtc = DateTime.UtcNow,
                        };
                        FIL.Contracts.DataModels.Sponsor result = _sponsorRepository.Save(corporateData);
                        addCorporateDataResult.Id = result.Id;
                        addCorporateDataResult.Success = true;
                    }
                    else
                    {
                        addCorporateDataResult.Id = -1;
                        addCorporateDataResult.Success = false;
                        addCorporateDataResult.message = command.SponsorName + " already exists!";
                    }
                }
                else
                {
                    var country = _countryRepository.Get(Convert.ToInt32(command.CountryId));
                    var data = _sponsorRepository.Get((long)command.Id);
                    data.SponsorName = command.SponsorName;
                    data.FirstName = command.FirstName;
                    data.LastName = command.LastName;
                    data.Email = command.Email;
                    data.PhoneCode = command.PhoneCode;
                    data.PhoneNumber = command.PhoneNumber;
                    data.Address = command.Address;
                    data.CityId = command.CityId;
                    data.StateId = command.StateId;
                    data.CountryId = country.Name;
                    data.Zipcode = command.Zipcode;
                    data.SponsorTypeId = Contracts.Enums.SponsorType.Regular;
                    data.IdType = command.IdType;
                    data.IdNumber = command.IdNumber;
                    data.ModifiedBy = command.ModifiedBy;

                    FIL.Contracts.DataModels.Sponsor result = _sponsorRepository.Save(data);
                    addCorporateDataResult.Id = result.Id;
                    addCorporateDataResult.Success = true;
                }
                return Task.FromResult<ICommandResult>(addCorporateDataResult);
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                addCorporateDataResult.Id = -1;
                addCorporateDataResult.Success = false;
                return Task.FromResult<ICommandResult>(addCorporateDataResult);
            }
        }
    }
}