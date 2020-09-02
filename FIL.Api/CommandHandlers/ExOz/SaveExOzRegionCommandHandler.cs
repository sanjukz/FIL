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
    public class SaveExOzRegionCommandHandler : BaseCommandHandlerWithResult<SaveExOzRegionCommand, SaveExOzRegionCommandResult>
    {
        private readonly IExOzStateRepository _exOzStateRepository;
        private readonly IStateRepository _stateRepository;
        private readonly IExOzRegionRepository _exOzRegionRepository;
        private readonly ICityRepository _cityRepository;

        public SaveExOzRegionCommandHandler(IExOzRegionRepository exOzRegionRepository, ICityRepository cityRepository,
        IExOzStateRepository exOzStateRepository, IStateRepository stateRepository, IMediator mediator)
            : base(mediator)
        {
            _exOzStateRepository = exOzStateRepository;
            _stateRepository = stateRepository;
            _exOzRegionRepository = exOzRegionRepository;
            _cityRepository = cityRepository;
        }

        protected override Task<ICommandResult> Handle(SaveExOzRegionCommand command)
        {
            _exOzRegionRepository.DisableAllExOzRegions();
            UpdateRegions(command);
            SaveExOzRegionCommandResult updatedRegions = new SaveExOzRegionCommandResult();
            updatedRegions.RegionList = _exOzRegionRepository.GetAll().ToList();
            return Task.FromResult<ICommandResult>(updatedRegions);
        }

        protected void UpdateRegions(SaveExOzRegionCommand command)
        {
            List<string> apiRegionNames = command.RegionList.Select(w => w.Name).Distinct().ToList();
            var kzCities = _cityRepository.GetByNames(apiRegionNames);
            var exOzRegions = _exOzRegionRepository.GetByNames(apiRegionNames);

            foreach (var item in command.RegionList)
            {
                ExOzRegion existingExOzRegion = exOzRegions.Where(w => w.RegionId == item.Id).FirstOrDefault();
                City existingKzCity = kzCities.Where(w => w.Name == item.Name).FirstOrDefault();

                ExOzState exOzState = _exOzStateRepository.GetByUrlSegment(item.StateUrlSegment);
                State kzState = _stateRepository.GetByName(exOzState.Name);

                City kzCityInserted = new City();
                if (existingKzCity == null)
                {
                    var newKzCity = new City
                    {
                        Name = item.Name,
                        StateId = kzState.Id,
                        IsEnabled = true,
                    };
                    kzCityInserted = _cityRepository.Save(newKzCity);
                }
                else
                {
                    kzCityInserted = existingKzCity;
                }
                if (existingExOzRegion == null)
                {
                    var newExOzRegion = new ExOzRegion
                    {
                        RegionId = item.Id,
                        Name = item.Name,
                        UrlSegment = item.UrlSegment,
                        StateId = exOzState.Id,
                        CityId = kzCityInserted.Id,
                        IsEnabled = true,
                        Offset = item.Offset,
                        TimeStamp = item.Timestamp,
                        OperatorCount = item.Operators.Count,
                        CategoryCount = item.Categories.Count,
                    };
                    ExOzRegion exOzRegionInserted = _exOzRegionRepository.Save(newExOzRegion);
                }
                else
                {
                    existingExOzRegion.IsEnabled = true;
                    ExOzRegion exOzRegionInserted = _exOzRegionRepository.Save(existingExOzRegion);
                }
            }
        }
    }
}