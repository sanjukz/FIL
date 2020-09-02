using FIL.Api.Repositories;
using FIL.Contracts.Commands.ExOz;
using FIL.Contracts.DataModels;
using FIL.Contracts.Interfaces.Commands;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.ExOz
{
    public class SaveExOzStateCommandHandler : BaseCommandHandlerWithResult<SaveExOzStateCommand, SaveExOzStateCommandResult>
    {
        private readonly IExOzCountryRepository _exOzCountryRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IExOzStateRepository _exOzStateRepository;
        private readonly IStateRepository _stateRepository;

        public SaveExOzStateCommandHandler(IExOzStateRepository exOzStateRepository, IStateRepository stateRepository,
        IExOzCountryRepository exOzCountryRepository, ICountryRepository countryRepository, IMediator mediator)
            : base(mediator)
        {
            _exOzCountryRepository = exOzCountryRepository;
            _countryRepository = countryRepository;
            _exOzStateRepository = exOzStateRepository;
            _stateRepository = stateRepository;
        }

        protected override Task<ICommandResult> Handle(SaveExOzStateCommand command)
        {
            _exOzStateRepository.DisableAllExOzStates();
            UpdateStates(command);
            SaveExOzStateCommandResult updatedStates = new SaveExOzStateCommandResult();
            updatedStates.StateList = _exOzStateRepository.GetAll().ToList();
            return Task.FromResult<ICommandResult>(updatedStates);
        }

        protected void UpdateStates(SaveExOzStateCommand command)
        {
            List<string> apiStateNames = command.StateList.Select(w => w.Name).Distinct().ToList();
            var kzStates = _stateRepository.GetByNames(apiStateNames);
            var exOzStates = _exOzStateRepository.GetByNames(apiStateNames);

            foreach (var item in command.StateList)
            {
                State existingKzState = kzStates.Where(w => w.Name == item.Name).FirstOrDefault();
                ExOzState existingExOzState = exOzStates.Where(w => w.StateId == item.Id).FirstOrDefault();

                Country kzCountry = _countryRepository.GetByName(item.Country);
                ExOzCountry exOzCountry = _exOzCountryRepository.GetByName(item.Country);

                State kzStateInserted = new State();
                if (existingKzState == null)
                {
                    var newKzState = new State
                    {
                        Name = item.Name,
                        CountryId = kzCountry.Id,
                        IsEnabled = true,
                    };
                    kzStateInserted = _stateRepository.Save(newKzState);
                }
                else
                {
                    kzStateInserted = existingKzState;
                }
                if (existingExOzState == null)
                {
                    var newExOzState = new ExOzState
                    {
                        StateId = item.Id,
                        Name = item.Name,
                        UrlSegment = item.UrlSegment,
                        CountryId = exOzCountry.Id,
                        StateMapId = kzStateInserted.Id,
                        IsEnabled = true,
                    };
                    ExOzState exOzStateInserted = _exOzStateRepository.Save(newExOzState);
                }
            }
        }
    }
}