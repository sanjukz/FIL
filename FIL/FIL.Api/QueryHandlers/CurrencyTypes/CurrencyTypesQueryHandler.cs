using FIL.Api.Repositories;
using FIL.Contracts.Queries.CurrencyTypes;
using FIL.Contracts.QueryResults;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.Cities
{
    public class CurrencyTypesQueryHandler : IQueryHandler<CurrencyTypesQuery, CurrencyTypesQueryResult>
    {
        private readonly ICurrencyTypeRepository _currencyTypeRepository;
        private readonly FIL.Logging.ILogger _logger;

        public CurrencyTypesQueryHandler(ICurrencyTypeRepository currencyTypeRepository, FIL.Logging.ILogger logger)
        {
            _currencyTypeRepository = currencyTypeRepository;
            _logger = logger;
        }

        public CurrencyTypesQueryResult Handle(CurrencyTypesQuery query)
        {
            var currencyTypes = _currencyTypeRepository.GetAll();
            var currencyType = AutoMapper.Mapper.Map<List<Contracts.Models.CurrencyType>>(currencyTypes).ToList();

            return new CurrencyTypesQueryResult
            {
                currencyTypes = currencyType
            };
        }
    }
}