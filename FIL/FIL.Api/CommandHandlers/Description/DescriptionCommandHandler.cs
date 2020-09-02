using FIL.Api.CommandHandlers;
using FIL.Api.Repositories;
using FIL.Contracts.Commands.Description;
using FIL.Contracts.DataModels;
using FIL.Contracts.Interfaces.Commands;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.QueryHandlers.Description
{
    public class DescriptionCommandHandler : BaseCommandHandlerWithResult<DescriptionCommand, DescriptionCommandResult>
    {
        private readonly ICountryRepository _countryRepository;
        private readonly ICityRepository _cityRepository;
        private readonly ICityDescriptionRepository _cityDescriptionRepository;
        private readonly ICountryDescriptionRepository _countryDescriptionRepository;
        private readonly IStateDescriptionRepository _stateDescriptionRepository;
        private readonly IMediator _mediator;

        public DescriptionCommandHandler(ICountryRepository countryRepository,
            ICityDescriptionRepository cityDescriptionRepository,
            ICityRepository cityRepository,
            ICountryDescriptionRepository countryDescriptionRepository,
            IStateDescriptionRepository stateDescriptionRepository,
            IMediator mediator) : base(mediator)
        {
            _countryRepository = countryRepository;
            _cityDescriptionRepository = cityDescriptionRepository;
            _countryDescriptionRepository = countryDescriptionRepository;
            _cityRepository = cityRepository;
            _stateDescriptionRepository = stateDescriptionRepository;
            _mediator = mediator;
        }

        protected override async Task<ICommandResult> Handle(DescriptionCommand command)
        {
            DescriptionCommandResult DescriptionCommandResult = new DescriptionCommandResult();
            if (!command.IsStateDescription && !command.IsCityDescription)
            {
                var countryDescription = _countryDescriptionRepository.GetByCountryId(command.Country.Id);
                if (countryDescription != null)
                {
                    countryDescription.Description = command.Description;
                    _countryDescriptionRepository.Save(countryDescription);
                }
                else
                {
                    var countryDesc = new CountryDescription
                    {
                        CountryId = command.Country.Id,
                        Description = command.Description,
                        CreatedBy = command.CreatedBy,
                        ModifiedBy = command.CreatedBy,
                        CreatedUtc = DateTime.UtcNow,
                        IsEnabled = true,
                        UpdatedBy = command.CreatedBy,
                        UpdatedUtc = DateTime.UtcNow
                    };
                    _countryDescriptionRepository.Save(countryDesc);
                }
            }
            else if (!command.IsCountryDescription && !command.IsStateDescription)
            {
                var cityDescription = _cityDescriptionRepository.GetBycityId(command.City.Id);
                if (cityDescription != null)
                {
                    cityDescription.Description = command.Description;
                    cityDescription.UpdatedUtc = DateTime.UtcNow;
                    cityDescription.ModifiedBy = command.CreatedBy;
                    _cityDescriptionRepository.Save(cityDescription);
                }
                else
                {
                    var cityDesc = new CityDescription
                    {
                        CityId = command.City.Id,
                        Description = command.Description,
                        CreatedBy = command.CreatedBy,
                        ModifiedBy = command.CreatedBy,
                        CreatedUtc = DateTime.UtcNow,
                        IsEnabled = true,
                        UpdatedBy = command.CreatedBy,
                        UpdatedUtc = DateTime.UtcNow
                    };
                    _cityDescriptionRepository.Save(cityDesc);
                }
            }
            else
            {
                var stateDescription = new StateDescription
                {
                    StateId = command.StateId,
                    Description = command.Description,
                    CreatedBy = command.CreatedBy,
                    ModifiedBy = command.CreatedBy,
                    CreatedUtc = DateTime.UtcNow,
                    IsEnabled = true,
                    UpdatedBy = command.CreatedBy,
                    UpdatedUtc = DateTime.UtcNow
                };
                _stateDescriptionRepository.Save(stateDescription);
            }
            return DescriptionCommandResult;
        }
    }
}