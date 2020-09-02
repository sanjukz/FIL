using FIL.Api.Repositories;
using FIL.Contracts.DataModels;
using FIL.Contracts.Models;
using FIL.Contracts.Models.Algolia;
using FIL.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.Providers.Algolia
{
    public interface IAlgoliaAddEventProvider
    {
        void AddEventToAlgolia(long eventId);
    }

    public class AlgoliaAddEventProvider : IAlgoliaAddEventProvider
    {
        private readonly ILogger _logger;
        private readonly IEventRepository _eventRepository;
        private readonly IAlgoliaClientProvider _algoliaClientProvider;
        private readonly IAlgoliaExportRepositoryRepository _algoliaExportRepositoryRepository;

        public AlgoliaAddEventProvider(
           ILogger logger, IEventRepository eventRepository,
            IAlgoliaClientProvider algoliaClientProvider,
             IAlgoliaExportRepositoryRepository algoliaExportRepositoryRepository)
        {
            _logger = logger;
            _eventRepository = eventRepository;
            _algoliaClientProvider = algoliaClientProvider;
            _algoliaExportRepositoryRepository = algoliaExportRepositoryRepository;
        }

        public void AddEventToAlgolia(long eventId)
        {
            var eventDetailModel = _eventRepository.GetPlaceForAlgolia(eventId).FirstOrDefault();

            var algoliaExportModel = _algoliaExportRepositoryRepository.GetByObjectId(eventId.ToString());

            bool isInsert = ShouldInsert(algoliaExportModel, eventDetailModel);

            if (algoliaExportModel == null || isInsert)
            {
                AlgoliaPlacesExportModel placeData = new AlgoliaPlacesExportModel();
                placeData.ObjectID = eventDetailModel.Id.ToString();
                placeData.Name = eventDetailModel.Name;
                placeData.Description = eventDetailModel.EventDescription;
                placeData.State = eventDetailModel.StateName;
                placeData.Country = eventDetailModel.CountryName;
                placeData.City = eventDetailModel.CityName;
                placeData.Category = eventDetailModel.ParentCategory;
                placeData.SubCategory = eventDetailModel.Category;
                placeData.Url = eventDetailModel.Url;
                placeData.PlaceImageUrl = "https://static5.feelaplace.com/images/places/tiles/" + eventDetailModel.AltId.ToString().ToUpper() + "-ht-c1.jpg";
                placeData.CityId = eventDetailModel.CityId;
                placeData.CountryId = eventDetailModel.CountryId;
                placeData.StateId = eventDetailModel.StateId;

                _algoliaClientProvider.SavePlaceObject(placeData);

                if (algoliaExportModel == null)
                {
                    InsertObjects(eventDetailModel);
                }
                else
                {
                    UpdateAlgoliaExports(eventDetailModel, algoliaExportModel);
                }
            }
        }

        private bool ShouldInsert(AlgoliaExport algoliaExportModel, PlaceDetail eventDetailModel)
        {
            if (algoliaExportModel != null && (algoliaExportModel.Name != eventDetailModel.Name
                || algoliaExportModel.Description != eventDetailModel.EventDescription
                || algoliaExportModel.Category != eventDetailModel.ParentCategory
                || algoliaExportModel.SubCategory != eventDetailModel.Category
                || algoliaExportModel.City != eventDetailModel.CityName
                || algoliaExportModel.State != eventDetailModel.StateName
                || algoliaExportModel.Country != eventDetailModel.CountryName
                || algoliaExportModel.CityId != eventDetailModel.CityId
                || algoliaExportModel.StateId != eventDetailModel.StateId
                || algoliaExportModel.CountryId != eventDetailModel.CountryId
                || algoliaExportModel.Url != eventDetailModel.Url
                ))

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
    }
}