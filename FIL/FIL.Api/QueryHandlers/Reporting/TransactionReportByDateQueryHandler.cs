using FIL.Api.Core.Utilities;
using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.Reporting;
using FIL.Contracts.QueryResults.Reporting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.TransactionReport
{
    public class TransactionReportByDateQueryHandler : IQueryHandler<TransactionReportQueryByDate, TransactionReportQueryByDateResult>
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
        private readonly IDataSettings _dataSettings;

        public TransactionReportByDateQueryHandler(
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
            ITicketFeeDetailRepository ticketFeeDetail,
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
            _dataSettings = dataSettings;
        }

        public TransactionReportQueryByDateResult Handle(TransactionReportQueryByDate query)
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

            //_dataSettings.UnitOfWork.BeginReadUncommittedTransaction();
            try
            {
                reporting = _reportingRepository.GetTransactionReportByDateData(query);
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

                //if (query.EventAltId != null && query.EventAltId != Guid.Empty && query.EventDetailId == 0)
                //{
                //    events = _eventRepository.GetByAltId(query.EventAltId);
                //    var userDetail = _userRepository.GetByAltId(query.UserAltId);
                //    eventDetails = _eventDetailRepository.GetAllByEventId(events.Id).ToList();
                //    if (userDetail.RolesId != 1)
                //    {
                //        var assignedEvents = _eventsUserMappingRepository.GetByUserIdAndEventId(userDetail.Id, events.Id);
                //        eventDetails = eventDetails.Where(w => assignedEvents.Select(s => s.EventDetailId).Contains(w.Id)).ToList();
                //    }
                //}
                //else
                //{
                //    if (query.EventAltId.ToString().ToUpper() == "E6B318DB-0945-4F96-841A-F58AED54EFCB")
                //    {
                //        eventDetails = _eventDetailRepository.GetByVenueId(Convert.ToInt32(query.EventDetailId)).ToList();
                //    }
                //    else
                //    {
                //        eventDetails = _eventDetailRepository.GetEventById(query.EventDetailId).ToList();
                //    }
                //    events = _eventRepository.GetById(eventDetails.Select(s => s.EventId).Distinct());
                //}

                //for (int k = 0; k < eventDetails.Count; k = k + 2000)
                //{
                //    var eventDetailBatcher = eventDetails.Skip(k).Take(2000);
                //    var eventTicketDetailList = AutoMapper.Mapper.Map<List<EventTicketDetail>>(_eventTicketDetailRepository.GetAllByEventDetailIds(eventDetailBatcher.Select(s => s.Id)));
                //    var eventAttributeList = _eventAttributeRepository.GetByEventDetailIds(eventDetailBatcher.Select(s => s.Id).Distinct()).ToList();
                //    for (int l = 0; l < eventTicketDetailList.Count; l = l + 2000)
                //    {
                //        var eventTicketDetailBatcher = eventTicketDetailList.Skip(l).Take(2000);
                //        var ticketCategoryList = _ticketCategoryRepository.GetByTicketCategoryIds(eventTicketDetailBatcher.Select(s => s.TicketCategoryId).Distinct());
                //        var eventTicketAttributeList = AutoMapper.Mapper.Map<List<EventTicketAttribute>>(_eventTicketAttributeRepository.GetByEventTicketDetailIds(eventTicketDetailBatcher.Select(s => s.Id)));
                //        var ticketFeeDetailList = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.TicketFeeDetail>>(_ticketFeeDetail.GetByEventTicketAttributeIds(eventTicketAttributeList.Select(s => s.Id).Distinct()));
                //        var transactionDetailList = AutoMapper.Mapper.Map<List<TransactionDetail>>(_transactionDetailRepository.GetByEventTicketAttributeIds(eventTicketAttributeList.Select(s => s.Id)));
                //        int timeZone = eventAttributeList != null && eventAttributeList.Count() > 0 ? Convert.ToInt16(eventAttributeList[0].TimeZone) : 0;
                //        for (int i = 0; i < transactionDetailList.Count; i = i + 2000)
                //        {
                //            var transactionDetailBatcher = transactionDetailList.Skip(i).Take(2000);
                //            var transactionList = _transactionRepository.GetReportTransactionsByIds(transactionDetailBatcher.Select(s => s.TransactionId).Distinct(), query.FromDate.AddMinutes((-timeZone)), query.ToDate.AddMinutes(-timeZone));
                //            var userList = _userRepository.GetByAltIds(transactionList.Select(s => s.CreatedBy).Distinct());
                //            transactionDetailBatcher = transactionDetailBatcher.Where(w => transactionList.Select(s => s.Id).Contains(w.TransactionId)).Distinct();
                //            var transactionDeliveryDetailList = _transactionDeliveryDetailRepository.GetByTransactionDetailIds(transactionDetailBatcher.Select(s => s.Id).Distinct());
                //            var transactionPaymentDetailList = _transactionPaymentDetailRepository.GetByTransactionIds(transactionList.Select(s => s.Id));
                //            var ipDetailList = _ipDetailRepository.GetByIds(transactionList.Where(w => w.IPDetailId != null).Select(s => s.IPDetailId).Distinct());
                //            var userCardDeatilsList = _userCardDetailRepository.GetByIds(transactionPaymentDetailList.Where(w => w.UserCardDetailId != null).Select(s => s.UserCardDetailId).Distinct());

                //            transactions = transactions.Concat(AutoMapper.Mapper.Map<IEnumerable<Kz.Contracts.Models.Transaction>>(transactionList)).ToList();
                //            transactionDetails = transactionDetails.Concat(AutoMapper.Mapper.Map<List<Kz.Contracts.Models.TransactionDetail>>(transactionDetailBatcher)).ToList();
                //            transactionDeliveryDetails = transactionDeliveryDetails.Concat(AutoMapper.Mapper.Map<List<Kz.Contracts.Models.TransactionDeliveryDetail>>(transactionDeliveryDetailList)).ToList();
                //            transactionPaymentDetails = transactionPaymentDetails.Concat(AutoMapper.Mapper.Map<List<Kz.Contracts.Models.TransactionPaymentDetail>>(transactionPaymentDetailList)).ToList();
                //            users = users.Concat(AutoMapper.Mapper.Map<List<Kz.Contracts.Models.User>>(userList)).ToList();
                //            ipDetails = ipDetails.Concat(AutoMapper.Mapper.Map<IEnumerable<Kz.Contracts.Models.IPDetail>>(ipDetailList)).ToList();
                //            userCardDetails = userCardDetails.Concat(AutoMapper.Mapper.Map<List<Kz.Contracts.Models.UserCardDetail>>(userCardDeatilsList)).ToList();
                //        }
                //        eventTicketDetails = eventTicketDetails.Concat(AutoMapper.Mapper.Map<List<Kz.Contracts.Models.EventTicketDetail>>(eventTicketDetailBatcher)).ToList();
                //        eventTicketAttributes = eventTicketAttributes.Concat(AutoMapper.Mapper.Map<List<Kz.Contracts.Models.EventTicketAttribute>>(eventTicketAttributeList)).ToList();
                //        ticketFeeDetails = ticketFeeDetails.Concat(AutoMapper.Mapper.Map<List<Kz.Contracts.Models.TicketFeeDetail>>(ticketFeeDetailList)).ToList();
                //        ticketCategories = ticketCategories.Concat(AutoMapper.Mapper.Map<List<Kz.Contracts.Models.TicketCategory>>(ticketCategoryList)).ToList();
                //    }
                //    eventAttributes = eventAttributes.Concat(AutoMapper.Mapper.Map<List<Kz.Contracts.Models.EventAttribute>>(eventAttributeList)).ToList();
                //}
                //currenyTypes = AutoMapper.Mapper.Map<List<Kz.Contracts.Models.CurrencyType>>(_currencyTypeRepository.GetByCurrencyIds(transactions.Select(s => s.CurrencyId).Distinct()));

                //var venues = _venueRepository.GetByVenueIds(eventDetails.Select(s => s.VenueId).Distinct());
                //var cities = _cityRepository.GetByCityIds(venues.Select(s => s.CityId).Distinct());
                //var states = _stateRepository.GetByStateIds(cities.Select(s => s.StateId).Distinct());
                //var countries = _countryRepository.GetByCountryIds(states.Select(s => s.CountryId).Distinct());

                //var loggedUserDetails = _userRepository.GetByAltId(query.UserAltId);
                //var reportingColumnDetailByUser = _reportingColumnsUserMappingRepository.GetByUserId(loggedUserDetails.Id);
                //var reportingColumnNameDetails = _reportingColumnsMenuMappingRepository.GetByIdsAndMenuId(reportingColumnDetailByUser.Select(s => s.ColumnsMenuMappingId).Distinct(), 1);
                //reportingColumnDetailByUser = reportingColumnDetailByUser.Where(W => reportingColumnNameDetails.Select(s => s.Id).Contains(W.ColumnsMenuMappingId)).ToList().OrderBy(o => o.SortOrder);
                //var reportColumnDetails = _reportingColumnRepository.GetByIds(reportingColumnNameDetails.Select(s => s.ColumnId).Distinct());
                //transactions = transactions.Where(w => transactionDetails.Select(s => s.TransactionId).Distinct().Contains(w.Id)).ToList();
                //List<Kz.Contracts.Models.ReportingColumn> reportColumns = new List<Kz.Contracts.Models.ReportingColumn>();
                //foreach (var item in reportingColumnDetailByUser)
                //{
                //    var reportingColumnNameDetail = reportingColumnNameDetails.Where(w => w.Id == item.ColumnsMenuMappingId).FirstOrDefault();
                //    var reportColumnDetail = reportColumnDetails.Where(w => w.Id == reportingColumnNameDetail.ColumnId).FirstOrDefault();
                //    reportColumns.Add(AutoMapper.Mapper.Map<Kz.Contracts.Models.ReportingColumn>(reportColumnDetail));
                //}

                //_dataSettings.UnitOfWork.Commit();

                return new TransactionReportQueryByDateResult
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
                    TicketFeeDetail = ticketFeeDetails
                };
            }
            catch (System.Exception ex)
            {
                var loggedUserDetails = _userRepository.GetByAltId(query.UserAltId);
                var reportingColumnDetailByUser = _reportingColumnsUserMappingRepository.GetByUserId(loggedUserDetails.Id);
                var reportingColumnNameDetails = _reportingColumnsMenuMappingRepository.GetByIdsAndMenuId(reportingColumnDetailByUser.Select(s => s.ColumnsMenuMappingId).Distinct(), 1);
                var reportColumnDetails = _reportingColumnRepository.GetByIds(reportingColumnNameDetails.Select(s => s.ColumnId).Distinct());

                //_dataSettings.UnitOfWork.Rollback();
                return new TransactionReportQueryByDateResult
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
                    TicketFeeDetail = ticketFeeDetails
                };
            }
        }
    }
}