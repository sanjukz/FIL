using FIL.Api.Repositories;
using FIL.Contracts.Queries.City;
using FIL.Contracts.QueryResults;
using System;
using System.Collections.Generic;

namespace FIL.Api.QueryHandlers.Cities
{
    public class CitiesQueryHandler : IQueryHandler<CitiesQuery, CitiesQueryResult>
    {
        private readonly IStateRepository _stateRepository;
        private readonly ICityRepository _cityRepository;
        private readonly FIL.Logging.ILogger _logger;

        public CitiesQueryHandler(IStateRepository stateRepository, ICityRepository cityRepository, FIL.Logging.ILogger logger)
        {
            _stateRepository = stateRepository;
            _cityRepository = cityRepository;
            _logger = logger;
        }

        public CitiesQueryResult Handle(CitiesQuery query)
        {
            try
            {
                var state = _stateRepository.GetByAltId(query.StateAltId);
                var cities = _cityRepository.GetAllByStateId(state.Id);
                return new CitiesQueryResult
                {
                    Cities = AutoMapper.Mapper.Map<List<Contracts.Models.City>>(cities),
                    Success = true
                };
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                return new CitiesQueryResult { };
            }
        }
    }
}