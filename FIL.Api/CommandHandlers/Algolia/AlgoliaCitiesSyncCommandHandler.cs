using FIL.Api.Providers.Algolia;
using FIL.Api.Repositories;
using FIL.Configuration;
using FIL.Contracts.Commands.Algolia;
using FIL.Contracts.DataModels;
using FIL.Contracts.Interfaces.Commands;
using FIL.Contracts.Models.Algolia;
using FIL.Logging;
using FIL.Logging.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.CitySightSeeing
{
    public class AlgoliaCitiesSyncCommandHandler : BaseCommandHandlerWithResult<AlgoliaCitiesSyncCommand, AlgoliaCitiesSyncCommandResult>
    {
        private readonly ILogger _logger;
        private readonly ISettings _settings;
        private readonly IAlgoliaCitiesExportRepository _algoliaCitiesExportRepository;
        private readonly IAlgoliaClientProvider _algoliaClientProvider;

        public AlgoliaCitiesSyncCommandHandler(ILogger logger, ISettings settings, IAlgoliaClientProvider algoliaClientProvider, IAlgoliaCitiesExportRepository algoliaCitiesExportRepository,
            IMediator mediator) : base(mediator)
        {
            _algoliaClientProvider = algoliaClientProvider;
            _logger = logger;
            _settings = settings;
            _algoliaCitiesExportRepository = algoliaCitiesExportRepository;
        }

        protected override async Task<ICommandResult> Handle(AlgoliaCitiesSyncCommand command)
        {
            AlgoliaCitiesSyncCommandResult result = new AlgoliaCitiesSyncCommandResult();
            try
            {
                List<AlgoliaCitiesExportModel> cities = new List<AlgoliaCitiesExportModel>();
                foreach (var currentCity in command.AllCities)
                {
                    bool isUpdate = false;
                    var currentAlgoliaObject = _algoliaCitiesExportRepository.GetByObjectId(currentCity.CityId.ToString());
                    if (currentAlgoliaObject == null)
                    {
                        isUpdate = true;
                        await InsertObjects(currentCity);
                    }
                    else
                    {
                        isUpdate = CheckForUpdate(currentAlgoliaObject, currentCity);
                        if (isUpdate)
                        {
                            await UpdateAlgoliaExports(currentCity, currentAlgoliaObject);
                        }
                        currentAlgoliaObject.IsIndexed = true;
                        currentAlgoliaObject.IsEnabled = true;
                        _algoliaCitiesExportRepository.Save(currentAlgoliaObject);
                    }
                    if (isUpdate)
                    {
                        AlgoliaCitiesExportModel cityData = new AlgoliaCitiesExportModel();
                        cityData.ObjectID = currentCity.CityId.ToString();
                        cityData.State = currentCity.StateName;
                        cityData.Country = currentCity.CountryName;
                        cityData.CityName = currentCity.CityName;
                        cityData.CityId = currentCity.CityId;
                        cityData.CountryId = currentCity.CountryId;
                        cityData.StateId = currentCity.StateId;
                        cities.Add(cityData);
                    }
                }
                if (cities.Count > 0)
                {
                    _algoliaClientProvider.SaveCitiesObjects(cities);
                    result.IsSuccess = true;
                }
                return result;
            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                _logger.Log(LogCategory.Error, new Exception("Failed to Save Objects to Algolia index", e));
                return result;
            }
        }

        private Task<bool> UpdateAlgoliaExports(Itinerary currentCity, AlgoliaCitiesExport currentAlgoliaObject)
        {
            currentAlgoliaObject.CityName = currentCity.CityName;
            currentAlgoliaObject.State = currentCity.StateName;
            currentAlgoliaObject.Country = currentCity.CountryName;
            currentAlgoliaObject.IsEnabled = true;
            currentAlgoliaObject.IsIndexed = true;
            currentAlgoliaObject.CityId = currentCity.CityId;
            currentAlgoliaObject.CountryId = currentCity.CountryId;
            currentAlgoliaObject.StateId = currentCity.StateId;
            _algoliaCitiesExportRepository.Save(currentAlgoliaObject);
            return Task.FromResult(true);
        }

        private bool CheckForUpdate(AlgoliaCitiesExport currentAlgoliaObject, Itinerary currentCity)
        {
            if (currentAlgoliaObject.CityName != currentCity.CityName
                || currentAlgoliaObject.State != currentCity.StateName
                || currentAlgoliaObject.Country != currentCity.CountryName
                || currentAlgoliaObject.CityId != currentCity.CityId
                || currentAlgoliaObject.StateId != currentCity.StateId
                || currentAlgoliaObject.CountryId != currentCity.CountryId
                )

            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private Task<bool> InsertObjects(Itinerary currentCity)
        {
            _algoliaCitiesExportRepository.Save(new AlgoliaCitiesExport
            {
                ObjectId = currentCity.CityId.ToString(),
                CityName = currentCity.CityName,
                State = currentCity.StateName,
                Country = currentCity.CountryName,
                CityId = currentCity.CityId,
                CountryId = currentCity.CountryId,
                StateId = currentCity.StateId,
                IsEnabled = true,
                IsIndexed = true
            });
            return Task.FromResult(true);
        }
    }
}