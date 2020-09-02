using FIL.Api.Repositories;
using FIL.Contracts.DataModels;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.Algolia;
using FIL.Contracts.QueryResults.Algolia;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.Account
{
    public class GetAllPlacesQueryHandler : IQueryHandler<GetAllPlacesQuery, GetAllPlacesQueryResult>
    {
        private readonly IEventRepository _eventRepository;
        private readonly ICityRepository _cityRepository;

        public GetAllPlacesQueryHandler(IEventRepository eventRepository, ICityRepository cityRepository)
        {
            _eventRepository = eventRepository;
            _cityRepository = cityRepository;
        }

        public GetAllPlacesQueryResult Handle(GetAllPlacesQuery query)
        {
            var getAllPlaces = new List<PlaceDetail>();
            var getAllCities = new List<Itinerary>();
            if (!query.IsCities)
            {
                getAllPlaces = _eventRepository.GetAllPlacesForAlgolia(true).Skip(query.SkipIndex).Take(query.TakeIndex).ToList();
            }
            else
            {
                getAllCities = _cityRepository.GetAllFeelCityAndCountries().Skip(query.SkipIndex).Take(query.TakeIndex).ToList();
            }
            return new GetAllPlacesQueryResult
            {
                AllPlaces = getAllPlaces,
                GetAllCities = getAllCities
            };
        }
    }
}