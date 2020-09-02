using FIL.Logging;
using FIL.Configuration;
using System;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Web.Feel.Providers
{
    public interface INearestPlaceProvider
    {
        SearchVenue GetNearestPlace(List<SearchVenue> allPlaces, System.Device.Location.GeoCoordinate coord);
    }

    public class NearestPlaceProvider : INearestPlaceProvider
    {
        private readonly FIL.Logging.ILogger _logger;
        public NearestPlaceProvider(ILogger logger, ISettings settings
            )
        {
            _logger = logger;
        }

        public SearchVenue GetNearestPlace(List<SearchVenue> allPlaces, System.Device.Location.GeoCoordinate coord)
        {
            try
            {
                var nearest = allPlaces.Select(x => new System.Device.Location.GeoCoordinate(Double.Parse(x.Latitude), Double.Parse(x.Longitude)))
                                                                                  .OrderBy(x => x.GetDistanceTo(coord))
                                                                                  .First();
                var foundPlace = allPlaces.ToList().Find(x => Double.Parse(x.Latitude) == nearest.Latitude);
                return foundPlace;
            }
            catch (Exception e)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, e);
                return null;
            }
        }
    }
}
