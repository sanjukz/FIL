using FIL.Api.Repositories;
using FIL.Contracts.Queries.EntryGate;
using FIL.Contracts.QueryResults.EntryGate;
using System.Collections.Generic;

namespace FIL.Api.QueryHandlers.EntryGate
{
    public class EntryGateQueryHandler : IQueryHandler<EntryGateQuery, EntryGateQueryResult>
    {
        private readonly IEntryGateRepository _entryGateRepository;

        public EntryGateQueryHandler(IEntryGateRepository entryGateRepository)
        {
            _entryGateRepository = entryGateRepository;
        }

        public EntryGateQueryResult Handle(EntryGateQuery query)
        {
            var entryGateDataModel = _entryGateRepository.GetAll();
            var entryGateModel = AutoMapper.Mapper.Map<List<Contracts.Models.EntryGate>>(entryGateDataModel);
            return new EntryGateQueryResult
            {
                EntryGates = entryGateModel
            };
        }
    }
}