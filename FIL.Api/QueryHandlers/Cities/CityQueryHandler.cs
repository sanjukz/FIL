using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.City;
using FIL.Contracts.QueryResults;
using System;
using System.Collections.Generic;

namespace FIL.Api.QueryHandlers.Cities
{
    public class CityQueryHandler : IQueryHandler<CityQuery, CityQueryResult>
    {
        private readonly IStateRepository _stateRepository;
        private readonly ICityRepository _cityRepository;
        private readonly FIL.Logging.ILogger _logger;

        public CityQueryHandler(IStateRepository stateRepository, ICityRepository cityRepository, FIL.Logging.ILogger logger)
        {
            _stateRepository = stateRepository;
            _cityRepository = cityRepository;
            _logger = logger;
        }

        public CityQueryResult Handle(CityQuery query)
        {
            List<City> cities = new List<City>();
            var state = _stateRepository.GetByAltId(query.StateAltId);
            if (state == null)
            {
                return new CityQueryResult
                {
                    Cities = new List<City>(),
                };
            }
            else
            {
                var city = _cityRepository.GetAllByStateId(state.Id);
                try
                {
                    foreach (var item in city)
                    {
                        cities.Add(new City
                        {
                            AltId = item.AltId,
                            Name = item.Name,
                            StateId = item.StateId,
                            Id = item.Id,
                        });
                    }
                    return new CityQueryResult
                    {
                        Cities = cities
                    };
                }
                catch (Exception ex)
                {
                    _logger.Log(Logging.Enums.LogCategory.Error, ex);
                    return new CityQueryResult
                    {
                        Cities = null
                    };
                }
            }
        }
    }
}