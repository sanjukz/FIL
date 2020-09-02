using FIL.Api.Providers;
using FIL.Api.Repositories;
using FIL.Api.Repositories.Redemption;
using FIL.Contracts.Queries.CreateEventV1;
using FIL.Contracts.QueryResults.CreateEventV1;
using System;

namespace FIL.Api.QueryHandlers.CreateEventV1
{
    public class EventFinanceQueryHandler : IQueryHandler<FinanceQuery, FinanceQueryResult>
    {
        private readonly IMasterFinanceDetailsRepository _masterFinanceDetailsRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IEventStripeConnectMasterRepository _eventStripeConnectMasterRepository;
        private readonly IEventStripeAccountMappingRepository _eventStripeAccountMappingRepository;
        private readonly IEventCurrencyProvider _eventCurrencyProvider;
        private readonly ICountryRepository _countryRepository;

        public EventFinanceQueryHandler(
            IMasterFinanceDetailsRepository masterFinanceDetailsRepository,
            IEventStripeConnectMasterRepository eventStripeConnectMasterRepository,
            IEventStripeAccountMappingRepository eventStripeAccountMappingRepository,
            IEventRepository eventRepository,
            IEventCurrencyProvider eventCurrencyProvider,
            ICountryRepository countryRepository
            )
        {
            _masterFinanceDetailsRepository = masterFinanceDetailsRepository;
            _eventStripeConnectMasterRepository = eventStripeConnectMasterRepository;
            _eventStripeAccountMappingRepository = eventStripeAccountMappingRepository;
            _eventRepository = eventRepository;
            _eventCurrencyProvider = eventCurrencyProvider;
            _countryRepository = countryRepository;
        }

        public FinanceQueryResult Handle(FinanceQuery query)
        {
            try
            {
                var eventData = _eventRepository.Get(query.EventId);
                if (eventData == null)
                {
                    return new FinanceQueryResult
                    {
                        EventId = query.EventId,
                        IsDraft = true
                    };
                }
                var eventStripeAccount = _eventStripeAccountMappingRepository.GetByEventId(query.EventId);
                if (eventStripeAccount.StripeAccountId == Contracts.Enums.StripeAccount.StripeIndia)
                {
                    var masterFinanceDetail = _masterFinanceDetailsRepository.GetByEventId(query.EventId);
                    return new FinanceQueryResult
                    {
                        EventFinanceDetailModel = AutoMapper.Mapper.Map<FIL.Contracts.Models.CreateEventV1.EventFinanceDetailModel>(masterFinanceDetail),
                        EventId = query.EventId,
                        IsDraft = false,
                        IsValidLink = true,
                        StripeAccount = Contracts.Enums.StripeAccount.StripeIndia,
                        StripeConnectAccountId = null,
                        EventAltId = eventData.AltId,
                        Success = true
                    };
                }
                else
                {
                    var eventCurrency = _eventCurrencyProvider.GetEventCurrency(eventData);
                    var country = _countryRepository.Get(eventCurrency.CountryId);
                    var eventStripeConnectMaster = _eventStripeConnectMasterRepository.GetByEventId(query.EventId);
                    if (eventStripeConnectMaster != null)
                    {
                        return new FinanceQueryResult
                        {
                            EventId = query.EventId,
                            IsDraft = false,
                            IsValidLink = true,
                            StripeAccount = eventStripeAccount.StripeAccountId,
                            StripeConnectAccountId = eventStripeConnectMaster.StripeConnectAccountID,
                            EventAltId = eventData.AltId,
                            IsoAlphaTwoCode = country.IsoAlphaTwoCode,
                            Success = true
                        };
                    }
                    else
                    {
                        return new FinanceQueryResult
                        {
                            EventId = query.EventId,
                            IsDraft = false,
                            IsValidLink = true,
                            StripeAccount = eventStripeAccount.StripeAccountId,
                            StripeConnectAccountId = null,
                            CurrencyType = eventCurrency,
                            IsoAlphaTwoCode = country.IsoAlphaTwoCode,
                            EventAltId = eventData.AltId,
                            Success = true
                        };
                    }
                }
            }
            catch (Exception e)
            {
                return new FinanceQueryResult { };
            }
        }
    }
}