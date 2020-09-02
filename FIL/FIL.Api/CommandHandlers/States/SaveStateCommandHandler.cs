using FIL.Api.Repositories;
using FIL.Contracts.Commands.Common;
using FIL.Contracts.DataModels;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.States
{
    public class SaveStateCommandHandler : BaseCommandHandler<SaveStateCommand>
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IStateRepository _stateRepository;

        public SaveStateCommandHandler(ICountryRepository countryRepository, IStateRepository stateRepository, IMediator mediator)
           : base(mediator)
        {
            _countryRepository = countryRepository;
            _stateRepository = stateRepository;
        }

        protected override async Task Handle(SaveStateCommand command)
        {
            var country = _countryRepository.GetByAltId(command.CountryAltId);
            if (country != null)
            {
                var state = new State
                {
                    AltId = Guid.NewGuid(),
                    Name = command.Name,
                    Abbreviation = command.Abbreviation,
                    CountryId = country.Id,
                    ModifiedBy = command.ModifiedBy,
                    CreatedUtc = DateTime.UtcNow,
                    IsEnabled = true
                };

                _stateRepository.Save(state);
            }
        }
    }
}