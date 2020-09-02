using FIL.Api.Repositories;
using FIL.Contracts.Queries.StateSearch;
using FIL.Contracts.QueryResults.StateSearch;
using System.Collections.Generic;

namespace FIL.Api.QueryHandlers.StateSearch
{
    public class StateSearchQueryHandler : IQueryHandler<StateSearchQuery, StateSearchQueryResult>
    {
        private readonly IStateRepository _stateRepository;

        public StateSearchQueryHandler(IStateRepository stateRepository)
        {
            _stateRepository = stateRepository;
        }

        public StateSearchQueryResult Handle(StateSearchQuery query)
        {
            var searchResult = _stateRepository.SearchByStateName(query.Name);
            var stateResult = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.State>>(searchResult);
            return new StateSearchQueryResult
            {
                States = stateResult,
            };
        }
    }
}