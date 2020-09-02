using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.State;
using FIL.Contracts.QueryResults;
using System;
using System.Collections.Generic;

namespace FIL.Api.QueryHandlers.States
{
    public class StateQueryHandler : IQueryHandler<StateQuery, StateQueryResult>
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IStateRepository _stateRepository;
        private readonly FIL.Logging.ILogger _logger;

        public StateQueryHandler(ICountryRepository countryRepository, IStateRepository stateRepository, FIL.Logging.ILogger logger)
        {
            _countryRepository = countryRepository;
            _stateRepository = stateRepository;
            _logger = logger;
        }

        public StateQueryResult Handle(StateQuery query)
        {
            List<State> states = new List<State>();
            var country = _countryRepository.GetByAltId(query.CountryAltId);
            if (country == null)
            {
                return new StateQueryResult
                {
                    States = new List<State>(),
                };
            }
            else
            {
                var state = _stateRepository.GetAllByCountryId(country.Id);
                try
                {
                    foreach (var item in state)
                    {
                        states.Add(new State
                        {
                            Id = item.Id,
                            AltId = item.AltId,
                            Name = item.Name,
                            Abbreviation = item.Abbreviation,
                            CountryId = item.CountryId
                        });
                    }
                    return new StateQueryResult
                    {
                        States = states
                    };
                }
                catch (Exception ex)
                {
                    _logger.Log(Logging.Enums.LogCategory.Error, ex);
                    return new StateQueryResult
                    {
                        States = null
                    };
                }
            }
        }
    }
}