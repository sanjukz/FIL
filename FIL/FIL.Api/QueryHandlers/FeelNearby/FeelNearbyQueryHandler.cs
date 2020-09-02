using FIL.Api.Repositories;
using FIL.Contracts.Queries;
using FIL.Contracts.QueryResults;
using System;
using System.Collections.Generic;

namespace FIL.Api.QueryHandlers
{
    public class FeelNearbyQueryHandler : IQueryHandler<FeelNearbyQuery, FeelNearbyQueryResult>
    {
        private readonly IFeelNearbyRepository _feelNearbyRepository;
        private readonly ICityRepository _cityRepository;
        private readonly Logging.ILogger _logger;

        public FeelNearbyQueryHandler(IFeelNearbyRepository feelNearbyRepository, Logging.ILogger logger, ICityRepository cityRepository)
        {
            _feelNearbyRepository = feelNearbyRepository;
            _cityRepository = cityRepository;
            _logger = logger;
        }

        public FeelNearbyQueryResult Handle(FeelNearbyQuery query)
        {
            try
            {
                var nearByPlaces = _feelNearbyRepository.GetNearbyPlaces(query.Latitude, query.Longitude, query.Distance);
                if (nearByPlaces == null)
                {
                    throw new ArgumentNullException($"Unable to get nearby places for lat {query.Latitude} lon {query.Longitude}");
                }
                else
                {
                    var nearbyItems = new List<VenueWithCityName>();
                    foreach (var item in nearByPlaces)
                    {
                        var city = _cityRepository.Get(item.CityId);
                        nearbyItems.Add(new VenueWithCityName
                        {
                            Id = item.Id,
                            AltId = item.AltId,
                            Name = item.Name,
                            AddressLineOne = item.AddressLineOne,
                            AddressLineTwo = item.AddressLineTwo,
                            CityId = item.CityId,
                            Latitude = item.Latitude,
                            Longitude = item.Longitude,
                            HasImages = item.HasImages,
                            Prefix = item.Prefix,
                            City = city.Name
                        });
                    }
                    return new FeelNearbyQueryResult
                    {
                        Latitude = query.Latitude,
                        Longitude = query.Longitude,
                        NearbyPlaces = nearbyItems
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                return new FeelNearbyQueryResult();
            }
        }
    }
}