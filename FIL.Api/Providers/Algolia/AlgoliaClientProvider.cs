using Algolia.Search.Clients;
using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Contracts.Models.Algolia;
using System.Collections.Generic;

namespace FIL.Api.Providers.Algolia
{
    public interface IAlgoliaClientProvider
    {
        void SavePlaceObject(AlgoliaPlacesExportModel places);

        void SavePlaceObjects(List<AlgoliaPlacesExportModel> places);

        void SaveCitiesObjects(List<AlgoliaCitiesExportModel> cities);

        void DeleteObjects(List<string> objectIds);

        void DeleteObject(string objectId);
    }

    public class AlgoliaClientProvider : IAlgoliaClientProvider
    {
        private ISettings _settings;
        private SearchClient _client;
        private SearchIndex _index;

        public AlgoliaClientProvider(
            ISettings settings)
        {
            _settings = settings;
            _client = new SearchClient(_settings.GetConfigSetting<string>(SettingKeys.Integration.Algolia.APP_ID), _settings.GetConfigSetting<string>(SettingKeys.Integration.Algolia.API_Key));
        }

        public void SavePlaceObject(AlgoliaPlacesExportModel places)
        {
            _index = _client.InitIndex((_settings.GetConfigSetting<string>(SettingKeys.Integration.Algolia.Index)));
            _index.SaveObject(places);
        }

        public void SavePlaceObjects(List<AlgoliaPlacesExportModel> places)
        {
            _index = _client.InitIndex((_settings.GetConfigSetting<string>(SettingKeys.Integration.Algolia.Index)));
            _index.SaveObjects(places);
        }

        public void SaveCitiesObjects(List<AlgoliaCitiesExportModel> cities)
        {
            _index = _client.InitIndex((_settings.GetConfigSetting<string>(SettingKeys.Integration.Algolia.CityIndex)));
            _index.SaveObjects(cities);
        }

        public void DeleteObjects(List<string> objectIds)
        {
            _index = _client.InitIndex((_settings.GetConfigSetting<string>(SettingKeys.Integration.Algolia.Index)));
            _index.DeleteObjects(objectIds);
        }

        public void DeleteObject(string objectId)
        {
            _index = _client.InitIndex((_settings.GetConfigSetting<string>(SettingKeys.Integration.Algolia.Index)));
            _index.DeleteObject(objectId);
        }
    }
}