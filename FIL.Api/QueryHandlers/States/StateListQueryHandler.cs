using FIL.Api.Repositories;
using FIL.Contracts.Queries.State;
using FIL.Contracts.QueryResults;
using System;
using System.Collections.Generic;

namespace FIL.Api.QueryHandlers.States
{
    public class StateListQueryHandler : IQueryHandler<StateListQuery, StateListQueryResult>
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IStateRepository _stateRepository;
        private readonly FIL.Logging.ILogger _logger;

        public StateListQueryHandler(ICountryRepository countryRepository, IStateRepository stateRepository, FIL.Logging.ILogger logger)
        {
            _countryRepository = countryRepository;
            _stateRepository = stateRepository;
            _logger = logger;
        }

        public StateListQueryResult Handle(StateListQuery query)
        {
            List<int> states = new List<int>();
            var country = _countryRepository.GetByAltId(query.CountryAltId);
            if (country == null)
            {
                return new StateListQueryResult
                {
                    StateIdList = new List<int>(),
                };
            }
            else
            {
                var state = _stateRepository.GetAllByCountryId(country.Id);
                try
                {
                    foreach (var item in state)
                    {
                        states.Add(item.Id);
                    }
                    return new StateListQueryResult
                    {
                        StateIdList = states
                    };
                }
                catch (Exception ex)
                {
                    _logger.Log(Logging.Enums.LogCategory.Error, ex);
                    return new StateListQueryResult
                    {
                        StateIdList = null
                    };
                }
            }
        }
    }
}