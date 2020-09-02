using AutoMapper;
using FIL.Api.Repositories;
using FIL.Contracts.Enums;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.Reporting;
using FIL.Contracts.QueryResults.Reporting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.Reporting
{
    public class FailedTranscationReportQueryHandler : IQueryHandler<FailedTransactionReportQuery, FailedTransactionReportQueryResult>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionDetailRepository _transactionDetailRepository;
        private readonly IUserRepository _userRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly IIPDetailRepository _iPDetailRepository;
        private readonly ITransactionDeliveryDetailRepository _transactionDeliveryDetailRepository;
        private readonly ICurrencyTypeRepository _currencyTypeRepository;
        private readonly ITransactionPaymentDetailRepository _transactionPaymentDetailRepository;
        private readonly IReportingColumnsUserMappingRepository _reportingColumnsUserMappingRepository;
        private readonly IReportingColumnsMenuMappingRepository _reportingColumnsMenuMappingRepository;
        private readonly IReportingColumnRepository _reportingColumnRepository;
        private readonly IEventAttributeRepository _eventAttributeRepository;
        private readonly IUserCardDetailRepository _userCardDetailRepository;

        public FailedTranscationReportQueryHandler(
            IEventRepository eventRepository,
            IEventDetailRepository eventDetailRepository,
            IEventTicketDetailRepository eventTicketDetailRepository,
            IEventTicketAttributeRepository eventTicketAttributeRepository,
            ITransactionRepository transactionRepository,
            ITransactionDetailRepository transactionDetailRepository,
            IUserRepository userRepository,
            IVenueRepository venueRepository,
            ICityRepository cityRepository,
            IStateRepository stateRepository,
            ICountryRepository countryRepository,
            ITicketCategoryRepository ticketCategoryRepository,
            IIPDetailRepository iPDetailRepository,
            ITransactionDeliveryDetailRepository transactionDeliveryDetailRepository,
            ICurrencyTypeRepository currencyTypeRepository,
            ITransactionPaymentDetailRepository transactionPaymentDetailRepository,
            IReportingColumnsUserMappingRepository reportingColumnsUserMappingRepository,
            IReportingColumnsMenuMappingRepository reportingColumnsMenuMappingRepository,
            IReportingColumnRepository reportingColumnRepository,
            IEventAttributeRepository eventAttributeRepository,
            IUserCardDetailRepository userCardDetailRepository
        )
        {
            _eventRepository = eventRepository;
            _eventDetailRepository = eventDetailRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _transactionRepository = transactionRepository;
            _transactionDetailRepository = transactionDetailRepository;
            _userRepository = userRepository;
            _venueRepository = venueRepository;
            _cityRepository = cityRepository;
            _stateRepository = stateRepository;
            _countryRepository = countryRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _iPDetailRepository = iPDetailRepository;
            _transactionDeliveryDetailRepository = transactionDeliveryDetailRepository;
            _currencyTypeRepository = currencyTypeRepository;
            _transactionPaymentDetailRepository = transactionPaymentDetailRepository;
            _reportingColumnsUserMappingRepository = reportingColumnsUserMappingRepository;
            _reportingColumnsMenuMappingRepository = reportingColumnsMenuMappingRepository;
            _reportingColumnRepository = reportingColumnRepository;
            _eventAttributeRepository = eventAttributeRepository;
            _userCardDetailRepository = userCardDetailRepository;
        }

        public FailedTransactionReportQueryResult Handle(FailedTransactionReportQuery query)
        {
            List<FIL.Contracts.Models.Transaction> transactions = new List<FIL.Contracts.Models.Transaction>();
            List<FIL.Contracts.Models.TransactionDetail> transactionDetails = new List<FIL.Contracts.Models.TransactionDetail>();
            List<TransactionPaymentDetail> transactionPaymentDetails = new List<TransactionPaymentDetail>();
            List<TransactionDeliveryDetail> transactionDeliveryDetails = new List<TransactionDeliveryDetail>();
            List<CurrencyType> currenyTypes = new List<CurrencyType>();
            List<User> users = new List<User>();
            List<EventTicketAttribute> eventTicketAttributes = new List<EventTicketAttribute>();
            List<EventTicketDetail> eventTicketDetails = new List<EventTicketDetail>();
            List<FIL.Contracts.Models.TicketCategory> ticketCategories = new List<FIL.Contracts.Models.TicketCategory>();
            List<FIL.Contracts.DataModels.EventDetail> eventDetails = new List<FIL.Contracts.DataModels.EventDetail>();
            FIL.Contracts.DataModels.Event events = new FIL.Contracts.DataModels.Event();
            List<FIL.Contracts.Models.EventAttribute> eventAttributes = new List<FIL.Contracts.Models.EventAttribute>();
            List<UserCardDetail> userCardDetails = new List<UserCardDetail>();
            List<FIL.Contracts.Models.IPDetail> ipDetails = new List<FIL.Contracts.Models.IPDetail>();

            try
            {
                if (query.EventAltId != null && query.EventAltId != Guid.Empty && query.EventDetailId == 0)
                {
                    events = _eventRepository.GetByAltId(query.EventAltId);
                    eventDetails = _eventDetailRepository.GetSubEventByEventId(events.Id).ToList();
                }
                else
                {
                    eventDetails = _eventDetailRepository.GetEventById(query.EventDetailId).ToList();
                    events = _eventRepository.GetById(eventDetails.Select(s => s.EventId));
                }

                for (int k = 0; k < eventDetails.Count; k = k + 2000)
                {
                    var eventDetailBatcher = eventDetails.Skip(k).Take(2000);
                    var eventTicketDetailList = AutoMapper.Mapper.Map<List<EventTicketDetail>>(_eventTicketDetailRepository.GetAllByEventDetailIds(eventDetailBatcher.Select(s => s.Id)));
                    var eventAttributeList = _eventAttributeRepository.GetByEventDetailIds(eventDetailBatcher.Select(s => s.Id).Distinct()).ToList();
                    for (int l = 0; l < eventTicketDetailList.Count; l = l + 2000)
                    {
                        var eventTicketDetailBatcher = eventTicketDetailList.Skip(l).Take(2000);
                        var ticketCategoryList = _ticketCategoryRepository.GetByTicketCategoryIds(eventTicketDetailBatcher.Select(s => s.TicketCategoryId).Distinct());
                        var eventTicketAttributeList = AutoMapper.Mapper.Map<List<EventTicketAttribute>>(_eventTicketAttributeRepository.GetByEventTicketDetailIds(eventTicketDetailBatcher.Select(s => s.Id)));
                        var transactionDetailList = AutoMapper.Mapper.Map<List<TransactionDetail>>(_transactionDetailRepository.GetByEventTicketAttributeIds(eventTicketAttributeList.Select(s => s.Id)));

                        int timeZone = eventAttributeList != null && eventAttributeList.Count() > 0 ? Convert.ToInt16(eventAttributeList[0].TimeZone) : 0;
                        for (int i = 0; i < transactionDetailList.Count; i = i + 2000)
                        {
                            var transactionDetailBatcher = transactionDetailList.Skip(i).Take(2000);
                            var transactionList = _transactionRepository.GetNonSuccessfullTransactionsByIds(transactionDetailBatcher.Select(s => s.TransactionId).Distinct(), query.FromDate.AddMinutes((-timeZone)), query.ToDate.AddMinutes(-timeZone), 1);

                            IEnumerable<FIL.Contracts.DataModels.TransactionPaymentDetail> transactionPaymentDetailList = new List<FIL.Contracts.DataModels.TransactionPaymentDetail>();

                            if ((PaymentGateway)query.PaymentGateway != 0)
                            {
                                transactionPaymentDetailList = _transactionPaymentDetailRepository.GetFailedTransactionsByIds(transactionList.Select(s => s.Id), (PaymentGateway)query.PaymentGateway);
                            }
                            else
                            {
                                transactionPaymentDetailList = _transactionPaymentDetailRepository.GetByTransactionIds(transactionList.Select(s => s.Id));
                            }

                            var transactionPaymentDetailCount = transactionPaymentDetailList.Select(x => x.TransactionId)
                                      .GroupBy(s => s).Select(g => new { TransactionId = g.Key, RequestType = g.Key, Count = g.Count() });
                            var FailedtransactionPaymentDetailCount = transactionPaymentDetailCount.Where(pr => transactionPaymentDetailCount.Any(p => pr.TransactionId == p.TransactionId && p.Count < 2));
                            var FailedtransactionPaymentDetailList = _transactionPaymentDetailRepository.GetByTransactionIds(FailedtransactionPaymentDetailCount.Select(s => s.TransactionId));
                            var FailedtransactionList = _transactionRepository.GetNonSuccessfullTransactionsByIds(FailedtransactionPaymentDetailList.Select(s => s.TransactionId).Distinct(), query.FromDate.AddMinutes((-timeZone)), query.ToDate.AddMinutes(-timeZone), 1);
                            var userList = _userRepository.GetByAltIds(FailedtransactionList.Select(s => s.CreatedBy).Distinct());
                            transactionDetailBatcher = transactionDetailBatcher.Where(w => FailedtransactionList.Select(s => s.Id).Contains(w.TransactionId)).Distinct();
                            var transactionDeliveryDetailList = _transactionDeliveryDetailRepository.GetByTransactionDetailIds(transactionDetailBatcher.Select(s => s.Id).Distinct());
                            var userCardDeatilsList = _userCardDetailRepository.GetByIds(
                                FailedtransactionPaymentDetailList.Select(s => s.UserCardDetailId));
                            var ipDetailList = _iPDetailRepository.GetByIds(FailedtransactionList.Where(w => w.IPDetailId != null).Select(s => s.IPDetailId).Distinct());

                            transactions = transactions.Concat(AutoMapper.Mapper.Map<IEnumerable<FIL.Contracts.Models.Transaction>>(FailedtransactionList)).ToList();
                            transactionDetails = transactionDetails.Concat(AutoMapper.Mapper.Map<List<FIL.Contracts.Models.TransactionDetail>>(transactionDetailBatcher)).ToList();
                            transactionDeliveryDetails = transactionDeliveryDetails.Concat(AutoMapper.Mapper.Map<List<FIL.Contracts.Models.TransactionDeliveryDetail>>(transactionDeliveryDetailList)).ToList();
                            transactionPaymentDetails = transactionPaymentDetails.Concat(AutoMapper.Mapper.Map<List<TransactionPaymentDetail>>(FailedtransactionPaymentDetailList)).ToList();
                            users = users.Concat(AutoMapper.Mapper.Map<List<FIL.Contracts.Models.User>>(userList)).ToList();
                            userCardDetails = userCardDetails.Concat(AutoMapper.Mapper.Map<List<FIL.Contracts.Models.UserCardDetail>>(userCardDeatilsList)).ToList();
                            ipDetails = ipDetails.Concat(AutoMapper.Mapper.Map<IEnumerable<FIL.Contracts.Models.IPDetail>>(ipDetailList)).ToList();
                        }
                        eventTicketDetails = eventTicketDetails.Concat(AutoMapper.Mapper.Map<List<FIL.Contracts.Models.EventTicketDetail>>(eventTicketDetailBatcher)).ToList();
                        eventTicketAttributes = eventTicketAttributes.Concat(AutoMapper.Mapper.Map<List<FIL.Contracts.Models.EventTicketAttribute>>(eventTicketAttributeList)).ToList();
                        ticketCategories = ticketCategories.Concat(AutoMapper.Mapper.Map<List<FIL.Contracts.Models.TicketCategory>>(ticketCategoryList)).ToList();
                    }
                    eventAttributes = eventAttributes.Concat(AutoMapper.Mapper.Map<List<FIL.Contracts.Models.EventAttribute>>(eventAttributeList)).ToList();
                }
                currenyTypes = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.CurrencyType>>(_currencyTypeRepository.GetByCurrencyIds(transactions.Select(s => s.CurrencyId).Distinct()));

                var venues = _venueRepository.GetByVenueIds(eventDetails.Select(s => s.VenueId).Distinct());
                var cities = _cityRepository.GetByCityIds(venues.Select(s => s.CityId).Distinct());
                var states = _stateRepository.GetByStateIds(cities.Select(s => s.StateId).Distinct());
                var countries = _countryRepository.GetByCountryIds(states.Select(s => s.CountryId).Distinct());

                var loggedUserDetails = _userRepository.GetByAltId(query.UserAltId);
                var reportingColumnDetailByUser = _reportingColumnsUserMappingRepository.GetByUserId(loggedUserDetails.Id);
                var reportingColumnNameDetails = _reportingColumnsMenuMappingRepository.GetByIdsAndMenuId(reportingColumnDetailByUser.Select(s => s.ColumnsMenuMappingId).Distinct(), 2);
                var reportColumnDetails = _reportingColumnRepository.GetByIds(reportingColumnNameDetails.Select(s => s.ColumnId).Distinct());
                transactions = transactions.Where(w => transactionDetails.Select(s => s.TransactionId).Distinct().Contains(w.Id)).ToList();

                return new FailedTransactionReportQueryResult
                {
                    Transaction = transactions,
                    TransactionDetail = transactionDetails,
                    TransactionDeliveryDetail = transactionDeliveryDetails,
                    TransactionPaymentDetail = AutoMapper.Mapper.Map<IEnumerable<TransactionPaymentDetail>>(transactionPaymentDetails.Distinct()),
                    CurrencyType = currenyTypes,
                    EventTicketAttribute = eventTicketAttributes,
                    EventTicketDetail = eventTicketDetails,
                    TicketCategory = ticketCategories,
                    EventDetail = AutoMapper.Mapper.Map<IEnumerable<EventDetail>>(eventDetails),
                    EventAttribute = AutoMapper.Mapper.Map<IEnumerable<EventAttribute>>(eventAttributes),
                    Venue = AutoMapper.Mapper.Map<IEnumerable<Venue>>(venues),
                    City = AutoMapper.Mapper.Map<IEnumerable<City>>(cities),
                    State = AutoMapper.Mapper.Map<IEnumerable<State>>(states),
                    Country = AutoMapper.Mapper.Map<IEnumerable<Country>>(countries),
                    Event = AutoMapper.Mapper.Map<IEnumerable<Event>>(events),
                    User = users,
                    UserCardDetail = userCardDetails,
                    IPDetail = ipDetails,
                    ReportColumns = AutoMapper.Mapper.Map<IEnumerable<FIL.Contracts.Models.ReportingColumn>>(reportColumnDetails)
                };
            }
            catch (System.Exception ex)
            {
                var loggedUserDetails = _userRepository.GetByAltId(query.UserAltId);
                var reportingColumnDetailByUser = _reportingColumnsUserMappingRepository.GetByUserId(loggedUserDetails.Id);
                var reportingColumnNameDetails = _reportingColumnsMenuMappingRepository.GetByIdsAndMenuId(reportingColumnDetailByUser.Select(s => s.ColumnsMenuMappingId).Distinct(), 1);
                var reportColumnDetails = _reportingColumnRepository.GetByIds(reportingColumnNameDetails.Select(s => s.ColumnId).Distinct());

                return new FailedTransactionReportQueryResult
                {
                    Transaction = transactions,
                    TransactionDetail = transactionDetails,
                    TransactionDeliveryDetail = transactionDeliveryDetails,
                    TransactionPaymentDetail = AutoMapper.Mapper.Map<IEnumerable<TransactionPaymentDetail>>(transactionPaymentDetails.Distinct()),
                    CurrencyType = currenyTypes,
                    EventTicketAttribute = eventTicketAttributes,
                    EventTicketDetail = eventTicketDetails,
                    TicketCategory = ticketCategories,
                    EventDetail = AutoMapper.Mapper.Map<IEnumerable<EventDetail>>(eventDetails),
                    EventAttribute = eventAttributes,
                    Venue = new List<Venue>(),
                    City = new List<City>(),
                    State = new List<State>(),
                    Country = new List<Country>(),
                    Event = new List<Event>(),
                    User = users,
                    UserCardDetail = userCardDetails,
                    ReportColumns = AutoMapper.Mapper.Map<IEnumerable<FIL.Contracts.Models.ReportingColumn>>(reportColumnDetails)
                };
            }
        }
    }
}