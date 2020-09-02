using FIL.Api.Repositories;
using FIL.Contracts.Queries.State;
using FIL.Contracts.QueryResults;

namespace FIL.Api.QueryHandlers.States
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
            var stateResult = _stateRepository.GetByName(query.Name);
            return new StateSearchQueryResult
            {
                IsExisting = stateResult != null,
            };
        }
    }
}