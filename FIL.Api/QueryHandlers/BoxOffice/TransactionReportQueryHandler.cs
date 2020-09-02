using FIL.Api.Core.Utilities;
using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.BoxOffice;
using FIL.Contracts.QueryResults.Reporting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.BoxOffice
{
    public class TransactionReportQueryHandler : IQueryHandler<FIL.Contracts.Queries.BoxOffice.TransactionReportQuery, TransactionReportQueryResult>
    {
        private readonly IReportingRepository _reportingRepository;
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
        private readonly IIPDetailRepository _ipDetailRepository;
        private readonly ITransactionDeliveryDetailRepository _transactionDeliveryDetailRepository;
        private readonly ICurrencyTypeRepository _currencyTypeRepository;
        private readonly ITransactionPaymentDetailRepository _transactionPaymentDetailRepository;
        private readonly IReportingColumnsUserMappingRepository _reportingColumnsUserMappingRepository;
        private readonly IReportingColumnsMenuMappingRepository _reportingColumnsMenuMappingRepository;
        private readonly IReportingColumnRepository _reportingColumnRepository;
        private readonly IEventAttributeRepository _eventAttributeRepository;
        private readonly IEventsUserMappingRepository _eventsUserMappingRepository;
        private readonly IUserCardDetailRepository _userCardDetailRepository;
        private readonly ITicketFeeDetailRepository _ticketFeeDetail;
        private readonly IBoxofficeUserAdditionalDetailRepository _boxofficeUserAdditionalDetailRepository;
        private readonly IDataSettings _dataSettings;

        public TransactionReportQueryHandler(
            IReportingRepository reportingRepository,
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
            IIPDetailRepository ipDetailRepository,
            ITransactionDeliveryDetailRepository transactionDeliveryDetailRepository,
            ICurrencyTypeRepository currencyTypeRepository,
            ITransactionPaymentDetailRepository transactionPaymentDetailRepository,
            IReportingColumnsUserMappingRepository reportingColumnsUserMappingRepository,
            IReportingColumnsMenuMappingRepository reportingColumnsMenuMappingRepository,
            IReportingColumnRepository reportingColumnRepository,
            IEventAttributeRepository eventAttributeRepository,
            IEventsUserMappingRepository eventsUserMappingRepository,
            IUserCardDetailRepository userCardDetailRepository,
            ITicketFeeDetailRepository ticketFeeDetail, IBoxofficeUserAdditionalDetailRepository boxofficeUserAdditionalDetailRepository,
            IDataSettings dataSettings
        )
        {
            _reportingRepository = reportingRepository;
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
            _ipDetailRepository = ipDetailRepository;
            _transactionDeliveryDetailRepository = transactionDeliveryDetailRepository;
            _currencyTypeRepository = currencyTypeRepository;
            _transactionPaymentDetailRepository = transactionPaymentDetailRepository;
            _reportingColumnsUserMappingRepository = reportingColumnsUserMappingRepository;
            _reportingColumnsMenuMappingRepository = reportingColumnsMenuMappingRepository;
            _reportingColumnRepository = reportingColumnRepository;
            _eventAttributeRepository = eventAttributeRepository;
            _eventsUserMappingRepository = eventsUserMappingRepository;
            _userCardDetailRepository = userCardDetailRepository;
            _ticketFeeDetail = ticketFeeDetail;
            _boxofficeUserAdditionalDetailRepository = boxofficeUserAdditionalDetailRepository;
            _dataSettings = dataSettings;
        }

        public TransactionReportQueryResult Handle(TransactionReportQuery query)
        {
            FIL.Contracts.DataModels.Reporting reporting = new FIL.Contracts.DataModels.Reporting();
            List<FIL.Contracts.Models.Transaction> transactions = new List<FIL.Contracts.Models.Transaction>();
            List<FIL.Contracts.Models.TransactionDetail> transactionDetails = new List<FIL.Contracts.Models.TransactionDetail>();
            List<TransactionPaymentDetail> transactionPaymentDetails = new List<TransactionPaymentDetail>();
            List<TransactionDeliveryDetail> transactionDeliveryDetails = new List<TransactionDeliveryDetail>();
            List<CurrencyType> currenyTypes = new List<CurrencyType>();
            List<User> users = new List<User>();
            List<EventTicketAttribute> eventTicketAttributes = new List<EventTicketAttribute>();
            List<EventTicketDetail> eventTicketDetails = new List<EventTicketDetail>();
            List<FIL.Contracts.Models.TicketCategory> ticketCategories = new List<FIL.Contracts.Models.TicketCategory>();
            List<FIL.Contracts.Models.EventDetail> eventDetails = new List<FIL.Contracts.Models.EventDetail>();
            List<FIL.Contracts.Models.Event> events = new List<FIL.Contracts.Models.Event>();
            List<FIL.Contracts.Models.EventAttribute> eventAttributes = new List<FIL.Contracts.Models.EventAttribute>();
            List<FIL.Contracts.Models.IPDetail> ipDetails = new List<FIL.Contracts.Models.IPDetail>();
            List<UserCardDetail> userCardDetails = new List<UserCardDetail>();
            List<TicketFeeDetail> ticketFeeDetails = new List<TicketFeeDetail>();

            try
            {
                reporting = _reportingRepository.GetBOTransactionReportData(query);
                transactions = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.Transaction>>(reporting.Transaction);
                transactionDetails = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.TransactionDetail>>(reporting.TransactionDetail);
                transactionPaymentDetails = AutoMapper.Mapper.Map<List<TransactionPaymentDetail>>(reporting.TransactionPaymentDetail);
                transactionDeliveryDetails = AutoMapper.Mapper.Map<List<TransactionDeliveryDetail>>(reporting.TransactionDeliveryDetail);
                currenyTypes = AutoMapper.Mapper.Map<List<CurrencyType>>(reporting.CurrencyType);
                users = AutoMapper.Mapper.Map<List<User>>(reporting.User);
                eventTicketAttributes = AutoMapper.Mapper.Map<List<EventTicketAttribute>>(reporting.EventTicketAttribute);
                eventTicketDetails = AutoMapper.Mapper.Map<List<EventTicketDetail>>(reporting.EventTicketDetail);
                ticketCategories = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.TicketCategory>>(reporting.TicketCategory);
                eventDetails = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.EventDetail>>(reporting.EventDetail);
                events = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.Event>>(reporting.Event);
                eventAttributes = AutoMapper.Mapper.Map<List<EventAttribute>>(reporting.EventAttribute);
                ipDetails = AutoMapper.Mapper.Map<List<IPDetail>>(reporting.IPDetail);
                userCardDetails = AutoMapper.Mapper.Map<List<UserCardDetail>>(reporting.UserCardDetail);
                ticketFeeDetails = AutoMapper.Mapper.Map<List<TicketFeeDetail>>(reporting.TicketFeeDetail);
                var venues = AutoMapper.Mapper.Map<List<Venue>>(reporting.Venue);
                var cities = AutoMapper.Mapper.Map<List<City>>(reporting.City);
                var states = AutoMapper.Mapper.Map<List<State>>(reporting.State);
                var countries = AutoMapper.Mapper.Map<List<Country>>(reporting.Country);
                var reportColumnDetails = AutoMapper.Mapper.Map<List<ReportingColumn>>(reporting.ReportColumns);
                var bOCustomerDetail = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.BOCustomerDetail>>(reporting.BOCustomerDetail);

                var reportingColumnDetailByUser = AutoMapper.Mapper.Map<List<FIL.Contracts.DataModels.ReportingColumnsUserMapping>>(reporting.ReportingColumnsUserMapping);
                var reportingColumnNameDetails = AutoMapper.Mapper.Map<List<FIL.Contracts.DataModels.ReportingColumnsMenuMapping>>(reporting.ReportingColumnsMenuMapping);
                reportingColumnDetailByUser = reportingColumnDetailByUser.Where(W => reportingColumnNameDetails.Select(s => s.Id).Contains(W.ColumnsMenuMappingId)).ToList().OrderBy(o => o.SortOrder).ToList();
                List<FIL.Contracts.Models.ReportingColumn> reportColumns = new List<FIL.Contracts.Models.ReportingColumn>();
                foreach (var item in reportingColumnDetailByUser)
                {
                    var reportingColumnNameDetail = reportingColumnNameDetails.Where(w => w.Id == item.ColumnsMenuMappingId).FirstOrDefault();
                    var reportColumnDetail = reportColumnDetails.Where(w => w.Id == reportingColumnNameDetail.ColumnId).FirstOrDefault();
                    reportColumns.Add(AutoMapper.Mapper.Map<FIL.Contracts.Models.ReportingColumn>(reportColumnDetail));
                }
                var userDetail = _userRepository.GetByAltId(query.UserAltId);
                var boxOfficeUser = _boxofficeUserAdditionalDetailRepository.GetByUserId(userDetail.Id);
                return new TransactionReportQueryResult
                {
                    Transaction = transactions,
                    TransactionDetail = transactionDetails.OrderByDescending(o => o.TransactionId).ToList(),
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
                    ReportColumns = reportColumns,
                    IPDetail = ipDetails,
                    UserCardDetail = userCardDetails,
                    TicketFeeDetail = ticketFeeDetails,
                    BoxofficeUserAdditionalDetail = boxOfficeUser,
                    BOCustomerDetail = bOCustomerDetail,
                };
            }
            catch (System.Exception ex)
            {
                var loggedUserDetails = _userRepository.GetByAltId(query.UserAltId);
                var reportingColumnDetailByUser = _reportingColumnsUserMappingRepository.GetByUserId(loggedUserDetails.Id);
                var reportingColumnNameDetails = _reportingColumnsMenuMappingRepository.GetByIdsAndMenuId(reportingColumnDetailByUser.Select(s => s.ColumnsMenuMappingId).Distinct(), 1);
                var reportColumnDetails = _reportingColumnRepository.GetByIds(reportingColumnNameDetails.Select(s => s.ColumnId).Distinct());

                return new TransactionReportQueryResult
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
                    ReportColumns = AutoMapper.Mapper.Map<IEnumerable<FIL.Contracts.Models.ReportingColumn>>(reportColumnDetails),
                    IPDetail = ipDetails,
                    UserCardDetail = userCardDetails,
                    TicketFeeDetail = ticketFeeDetails,
                    BoxofficeUserAdditionalDetail = null,
                    BOCustomerDetail = new List<BOCustomerDetail>(),
                };
            }
        }
    }
}