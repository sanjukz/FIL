using FIL.Api.Repositories;
using FIL.Contracts.Queries.TicketCategory;
using FIL.Contracts.QueryResults.TicketCategories;
using System;
using System.Collections.Generic;

namespace FIL.Api.QueryHandlers.EventCreation
{
    public class CurrencyQueryHandler : IQueryHandler<CurrencyQuery, CurrencyQueryResult>
    {
        private readonly ICurrencyTypeRepository _currencyRepository;
        private readonly FIL.Logging.ILogger _logger;

        public CurrencyQueryHandler(ICurrencyTypeRepository currencyRepository, FIL.Logging.ILogger logger)
        {
            _currencyRepository = currencyRepository;
            _logger = logger;
        }

        public CurrencyQueryResult Handle(CurrencyQuery query)
        {
            List<FIL.Contracts.Models.CurrencyType> currencies = new List<FIL.Contracts.Models.CurrencyType>();
            try
            {
                var currency = _currencyRepository.GetAll(null);
                foreach (var item in currency)
                {
                    currencies.Add(new FIL.Contracts.Models.CurrencyType
                    {
                        Id = item.Id,
                        Name = item.Name
                    });
                }
                return new CurrencyQueryResult
                {
                    Currencies = currencies
                };
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                return new CurrencyQueryResult
                {
                    Currencies = null
                };
            }
        }
    }
}