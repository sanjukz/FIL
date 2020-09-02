using FIL.Api.Repositories;
using FIL.Contracts.Queries.MasterBudgetRange;
using FIL.Contracts.QueryResults.MasterBudgetRange;
using System.Linq;

namespace FIL.Api.QueryHandlers.MasterBudgetRange
{
    public class MasterBudgetRangeQueryHandler : IQueryHandler<MasterBudgetRangeQuery, MasterBudgetRangeQueryResult>
    {
        private readonly IMasterBudgetRangeRepository _masterBudgetRangeRepository;
        private readonly ICurrencyTypeRepository _currencyTypeRepository;

        public MasterBudgetRangeQueryHandler(IMasterBudgetRangeRepository masterBudgetRangeRepository, ICurrencyTypeRepository currencyTypeRepository)
        {
            _masterBudgetRangeRepository = masterBudgetRangeRepository;
            _currencyTypeRepository = currencyTypeRepository;
        }

        public MasterBudgetRangeQueryResult Handle(MasterBudgetRangeQuery query)
        {
            var budgetRanges = _masterBudgetRangeRepository.GetAll().OrderBy(s => s.SortOrder);
            var currencyTypes = _currencyTypeRepository.GetAll();

            return new MasterBudgetRangeQueryResult
            {
                MasterBudgetRanges = budgetRanges.ToList(),
                CurrencyTypes = currencyTypes.ToList()
            };
        }
    }
}