using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.Invoice;
using FIL.Contracts.QueryResults.Invoice;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.TMS.Invoice
{
    public class CurrencyQueryHandler : IQueryHandler<CurrencyQuery, CurrencyQueryResult>
    {
        private readonly IEventRepository _eventRepository;
        private readonly ICurrencyTypeRepository _currencyTypeRepository;

        public CurrencyQueryHandler(IEventRepository eventRepository,
         ICurrencyTypeRepository currencyTypeRepository)
        {
            _eventRepository = eventRepository;
            _currencyTypeRepository = currencyTypeRepository;
        }

        public CurrencyQueryResult Handle(CurrencyQuery query)
        {
            try
            {
                var events = _eventRepository.GetByAltId(query.eventAltId);
                var currencies = AutoMapper.Mapper.Map<List<CurrencyType>>(_currencyTypeRepository.GetByEventId(events.Id)).ToList();
                return new CurrencyQueryResult
                {
                    Currencies = currencies
                };
            }
            catch (Exception ex)
            {
                return new CurrencyQueryResult
                {
                    Currencies = null
                };
            }
        }
    }
}