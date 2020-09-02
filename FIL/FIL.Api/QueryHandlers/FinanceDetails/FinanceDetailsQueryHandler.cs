using FIL.Api.Repositories;
using FIL.Contracts.Queries.FinanceDetail;

using FIL.Contracts.QueryResult;

namespace FIL.Api.QueryHandlers.FinanceDetails
{
    public class FinanceDetailsQueryHandler : IQueryHandler<FinancDetailsByIdQuery, FinancDetailsByIdQueryResults>
    {
        private readonly IFinanceDetailRepository _financeDetailRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IEventRepository _eventRepository;

        private readonly FIL.Logging.ILogger _logger;

        public FinanceDetailsQueryHandler(IFinanceDetailRepository financeDetailRepository, IEventDetailRepository eventDetailRepository,
          IVenueRepository venueRepository, ICityRepository cityRepository, IStateRepository stateRepository, ICountryRepository countryRepository,
          IEventRepository eventRepository,
          FIL.Logging.ILogger logger)
        {
            _financeDetailRepository = financeDetailRepository;
            _eventDetailRepository = eventDetailRepository;
            _venueRepository = venueRepository;
            _cityRepository = cityRepository;
            _stateRepository = stateRepository;
            _countryRepository = countryRepository;
            _logger = logger;
            _eventRepository = eventRepository;
        }

        //FinancDetailsByIdQueryResults IQueryHandler<FinancDetailsByIdQuery, FinancDetailsByIdQueryResults>.Handle(FinancDetailsByIdQuery query)
        public FinancDetailsByIdQueryResults Handle(FinancDetailsByIdQuery query)
        {
            FIL.Contracts.DataModels.FinanceDetail financeDetails = new FIL.Contracts.DataModels.FinanceDetail();
            FIL.Contracts.DataModels.EventDetail eventDetail = new FIL.Contracts.DataModels.EventDetail();
            FIL.Contracts.DataModels.Event eventRes = new FIL.Contracts.DataModels.Event();
            FIL.Contracts.DataModels.Venue venueDetail = new FIL.Contracts.DataModels.Venue();
            FIL.Contracts.DataModels.Venue venueDetailRes = new FIL.Contracts.DataModels.Venue();
            FIL.Contracts.DataModels.City cityDetailRes = new Contracts.DataModels.City();
            FIL.Contracts.DataModels.State stateDetailRes = new Contracts.DataModels.State();
            FIL.Contracts.DataModels.Country countryDetailRes = new Contracts.DataModels.Country();

            var eventResult = _eventRepository.GetByAltId(query.EventId);

            var eventDetailRes = _eventDetailRepository.GetByEventId(eventResult.Id);

            var financedetail = _financeDetailRepository.GetFinanceDetailsByEventId(eventResult.Id);

            if (eventDetailRes != null)
            {
                venueDetailRes = _venueRepository.GetByVenueId((int)eventDetailRes.VenueId);

                if (venueDetailRes != null)
                {
                    cityDetailRes = _cityRepository.GetByCityId(venueDetailRes.CityId);
                }

                if (cityDetailRes != null)
                {
                    stateDetailRes = _stateRepository.GetByStateId(cityDetailRes.StateId);
                }

                if (stateDetailRes != null)
                {
                    countryDetailRes = _countryRepository.GetByCountryId(stateDetailRes.CountryId);
                }
            }
            var resultdata = new FinancDetailsByIdQueryResults();
            if (financedetail != null)
            {
                resultdata.FirstName = financedetail.FirstName;
                resultdata.Id = financedetail.Id;

                resultdata.LastName = financedetail.LastName;
                resultdata.AccountNickName = financedetail.AccountNickName;
                resultdata.BankName = financedetail.BankName;
                resultdata.BankAccountType = financedetail.BankAccountType;
                resultdata.BankName = financedetail.BankName;
                resultdata.PANNo = financedetail.PANNo;
                resultdata.RoutingNo = financedetail.RoutingNo;
                resultdata.GSTNo = financedetail.GSTNo;
                resultdata.AccountNo = financedetail.AccountNo;
                resultdata.AccountNickName = financedetail.AccountNickName;
                resultdata.CountryId = financedetail.CountryId;
                resultdata.StateId = financedetail.StateId;
                resultdata.CurrencyId = financedetail.CurrencyId;
                resultdata.FinancialsAccountBankAccountGSTInfo = financedetail.FinancialsAccountBankAccountGSTInfo;
            }
            resultdata.EventId = eventResult.Id;
            if (venueDetailRes != null)
            {
                resultdata.location = venueDetailRes.Name;
                resultdata.address1 = venueDetailRes.AddressLineOne;
                resultdata.address2 = venueDetailRes.AddressLineOne;
            }
            if (eventDetailRes != null)
            {
                resultdata.EventDetailId = (int)(eventDetailRes.Id);
            }
            if (stateDetailRes != null)
            {
                resultdata.state = stateDetailRes.Name;
            }
            if (cityDetailRes != null)
            {
                resultdata.city = cityDetailRes.Name;
            }
            if (countryDetailRes != null)
            {
                resultdata.country = countryDetailRes.Name;
            }
            return resultdata;
        }
    }
}