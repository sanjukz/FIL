using FIL.Api.Providers.Algolia;
using FIL.Api.Repositories;
using FIL.Configuration;
using FIL.Contracts.Commands.Algolia;
using FIL.Contracts.DataModels;
using FIL.Contracts.Interfaces.Commands;
using FIL.Contracts.Models;
using FIL.Contracts.Models.Algolia;
using FIL.Logging;
using FIL.Logging.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.CitySightSeeing
{
    public class AlgoliaPlaceSyncCommandHandler : BaseCommandHandlerWithResult<AlgoliaPlaceSyncCommand, AlgoliaPlaceSyncCommandResult>
    {
        private readonly ILogger _logger;
        private readonly ISettings _settings;
        private readonly IEventRepository _eventRepository;
        private readonly IAlgoliaExportRepositoryRepository _algoliaExportRepositoryRepository;
        private readonly IAlgoliaClientProvider _algoliaClientProvider;

        public AlgoliaPlaceSyncCommandHandler(ILogger logger, ISettings settings, IEventRepository eventRepository, IAlgoliaExportRepositoryRepository algoliaExportRepositoryRepository, IAlgoliaClientProvider algoliaClientProvider,
            IMediator mediator) : base(mediator)
        {
            _algoliaClientProvider = algoliaClientProvider;
            _logger = logger;
            _settings = settings;
            _eventRepository = eventRepository;
            _algoliaExportRepositoryRepository = algoliaExportRepositoryRepository;
        }

        protected override async Task<ICommandResult> Handle(AlgoliaPlaceSyncCommand command)
        {
            AlgoliaPlaceSyncCommandResult result = new AlgoliaPlaceSyncCommandResult();
            try
            {
                List<AlgoliaPlacesExportModel> places = new List<AlgoliaPlacesExportModel>();
                foreach (var currentPlace in command.AllPlaces)
                {
                    try
                    {
                        bool isUpdate = false;
                        var currentAlgoliaObject = _algoliaExportRepositoryRepository.GetByObjectId(currentPlace.Id.ToString());
                        if (currentAlgoliaObject == null)
                        {
                            isUpdate = true;
                            await InsertObjects(currentPlace);
                        }
                        else
                        {
                            isUpdate = CheckForUpdate(currentAlgoliaObject, currentPlace);
                            if (isUpdate)
                            {
                                await UpdateAlgoliaExports(currentPlace, currentAlgoliaObject);
                            }
                            currentAlgoliaObject.IsIndexed = true;
                            currentAlgoliaObject.IsEnabled = true;
                            _algoliaExportRepositoryRepository.Save(currentAlgoliaObject);
                        }
                        if (isUpdate)
                        {
                            AlgoliaPlacesExportModel placeData = new AlgoliaPlacesExportModel();
                            placeData.ObjectID = currentPlace.Id.ToString();
                            placeData.Name = currentPlace.Name;
                            placeData.Description = currentPlace.EventDescription;
                            placeData.State = currentPlace.StateName;
                            placeData.Country = currentPlace.CountryName;
                            placeData.City = currentPlace.CityName;
                            placeData.Category = currentPlace.ParentCategory;
                            placeData.SubCategory = currentPlace.Category;
                            placeData.Url = currentPlace.Url;
                            placeData.PlaceImageUrl = "https://static1.feelaplace.com/images/places/tiles/" + currentPlace.AltId.ToString().ToUpper() + "-ht-c1.jpg";
                            placeData.CityId = currentPlace.CityId;
                            placeData.CountryId = currentPlace.CountryId;
                            placeData.StateId = currentPlace.StateId;
                            places.Add(placeData);
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.Log(LogCategory.Error, new Exception("Failed to Save Objects to Algolia index", e));
                        continue;
                    }
                }
                if (places.Count > 0)
                {
                    _algoliaClientProvider.SavePlaceObjects(places);
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

        private Task<bool> UpdateAlgoliaExports(PlaceDetail currentPlace, AlgoliaExport currentAlgoliaObject)
        {
            currentAlgoliaObject.Name = currentPlace.Name;
            currentAlgoliaObject.Description = currentPlace.EventDescription;
            currentAlgoliaObject.State = currentPlace.StateName;
            currentAlgoliaObject.Country = currentPlace.CountryName;
            currentAlgoliaObject.City = currentPlace.CityName;
            currentAlgoliaObject.Category = currentPlace.ParentCategory;
            currentAlgoliaObject.SubCategory = currentPlace.Category;
            currentAlgoliaObject.Url = currentPlace.Url;
            currentAlgoliaObject.PlaceImageUrl = "https://static1.feelaplace.com/images/places/tiles/" + currentPlace.AltId.ToString().ToUpper() + "-ht-c1.jpg";
            currentAlgoliaObject.IsEnabled = true;
            currentAlgoliaObject.IsIndexed = true;
            currentAlgoliaObject.CityId = currentPlace.CityId;
            currentAlgoliaObject.CountryId = currentPlace.CountryId;
            currentAlgoliaObject.StateId = currentPlace.StateId;
            _algoliaExportRepositoryRepository.Save(currentAlgoliaObject);
            return Task.FromResult(true);
        }

        private bool CheckForUpdate(AlgoliaExport currentAlgoliaObject, PlaceDetail currentPlace)
        {
            if (currentAlgoliaObject.Name != currentPlace.Name
                || currentAlgoliaObject.Description != currentPlace.EventDescription
                || currentAlgoliaObject.Category != currentPlace.ParentCategory
                || currentAlgoliaObject.SubCategory != currentPlace.Category
                || currentAlgoliaObject.City != currentPlace.CityName
                || currentAlgoliaObject.State != currentPlace.StateName
                || currentAlgoliaObject.Country != currentPlace.CountryName
                || currentAlgoliaObject.CityId != currentPlace.CityId
                || currentAlgoliaObject.StateId != currentPlace.StateId
                || currentAlgoliaObject.CountryId != currentPlace.CountryId
                || currentAlgoliaObject.Url != currentPlace.Url
                )

            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private Task<bool> InsertObjects(PlaceDetail currentPlace)
        {
            _algoliaExportRepositoryRepository.Save(new AlgoliaExport
            {
                ObjectId = currentPlace.Id.ToString(),
                Name = currentPlace.Name,
                Description = currentPlace.EventDescription,
                State = currentPlace.StateName,
                Country = currentPlace.CountryName,
                City = currentPlace.CityName,
                Category = currentPlace.ParentCategory,
                SubCategory = currentPlace.Category,
                Url = currentPlace.Url,
                PlaceImageUrl = "https://static1.feelaplace.com/images/places/tiles/" + currentPlace.AltId.ToString().ToUpper() + "-ht-c1.jpg",
                CityId = currentPlace.CityId,
                CountryId = currentPlace.CountryId,
                StateId = currentPlace.StateId,
                IsEnabled = true,
                IsIndexed = true
            });
            return Task.FromResult(true);
        }
    }
}