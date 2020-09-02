using FIL.Api.Repositories;
using FIL.Contracts.Commands.Common;
using FIL.Contracts.DataModels;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.Cities
{
    public class SaveCityCommandHandler : BaseCommandHandler<SaveCityCommand>
    {
        private readonly IStateRepository _stateRepository;
        private readonly ICityRepository _cityRepository;

        public SaveCityCommandHandler(IStateRepository stateRepository, ICityRepository cityRepository, IMediator mediator)
            : base(mediator)
        {
            _stateRepository = stateRepository;
            _cityRepository = cityRepository;
        }

        protected override async Task Handle(SaveCityCommand command)
        {
            var state = _stateRepository.GetByAltId(command.StateAltId);
            if (state != null)
            {
                var city = new City
                {
                    AltId = Guid.NewGuid(),
                    Name = command.Name,
                    StateId = state.Id,
                    ModifiedBy = command.ModifiedBy,
                    CreatedUtc = DateTime.UtcNow,
                    IsEnabled = true
                };

                _cityRepository.Save(city);
            }
        }
    }
}