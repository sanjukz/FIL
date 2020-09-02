using FIL.Api.Repositories;
using FIL.Contracts.Commands.RegistrationEvent;
using FIL.Contracts.DataModels;
using FIL.Contracts.Interfaces.Commands;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.RegistrationEvent
{
    public class RegistrationEventCommandHandler : BaseCommandHandlerWithResult<RegistrationEventCommand, RegistrationEventCommandResult>
    {
        private readonly IRegistrationEventUserMappingRepository _registrationEventUserMappingRepository;
        private readonly ICountryRepository _countryRepository;

        public RegistrationEventCommandHandler(IRegistrationEventUserMappingRepository registrationEventUserMappingRepository, ICountryRepository countryRepository, IMediator mediator)
            : base(mediator)
        {
            _countryRepository = countryRepository;
            _registrationEventUserMappingRepository = registrationEventUserMappingRepository;
        }

        protected override Task<ICommandResult> Handle(RegistrationEventCommand command)
        {
            RegistrationEventCommandResult registrationEventCommandResult = new RegistrationEventCommandResult();
            try
            {
                var country = _countryRepository.GetByAltId((Guid)command.CountryId);
                if (country == null)
                {
                    country = _countryRepository.GetByCountryId(101);
                }
                var registrationEventUserDetails = new RegistrationEventUserMapping
                {
                    AltId = Guid.NewGuid(),
                    FirstName = command.FirstName,
                    LastName = command.LastName,
                    //ParentFirstName=command.ParentFirstName,
                    //ParentLastName=command.ParentLastName,
                    // Age=command.Age,
                    Email = command.Email,
                    PhoneNumber = command.PhoneNumber,
                    //Address=command.Address,
                    //Suburb=command.Suburb,
                    //Zipcode=command.Zipcode,
                    CountryId = country.Id,
                    IsEnabled = true,
                    ModifiedBy = command.ModifiedBy,
                    CreatedUtc = DateTime.UtcNow,
                    State = command.State,
                    TransactionId = null,
                    InstaHandle = command.InstaHandle
                };
                var result = _registrationEventUserMappingRepository.Save(registrationEventUserDetails);
                registrationEventCommandResult.IsExisting = false;
                registrationEventCommandResult.Success = true;
                registrationEventCommandResult.RegistrationEventUserMapping = result;
            }
            catch (Exception ex)
            {
                registrationEventCommandResult.IsExisting = false;
                registrationEventCommandResult.Success = false;
                registrationEventCommandResult.RegistrationEventUserMapping = null;
            }

            return Task.FromResult<ICommandResult>(registrationEventCommandResult);
        }
    }
}