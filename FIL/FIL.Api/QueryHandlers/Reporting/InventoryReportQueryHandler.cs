using AutoMapper;
using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.Reporting;
using FIL.Contracts.QueryResults.Reporting;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.Reporting
{
    public class InventoryReportQueryHandler : IQueryHandler<InventoryReportQuery, InventoryReportQueryResult>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionDetailRepository _transactionDetailRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly ICityRepository _cityRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly ICurrencyTypeRepository _currencyTypeRepository;
        private readonly IReportingColumnsUserMappingRepository _reportingColumnsUserMappingRepository;
        private readonly IReportingColumnsMenuMappingRepository _reportingColumnsMenuMappingRepository;
        private readonly IReportingColumnRepository _reportingColumnRepository;
        private readonly IEventAttributeRepository _eventAttributeRepository;
        private readonly IEventsUserMappingRepository _eventsUserMappingRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICorporateTicketAllocationDetailRepository _corporateTicketAllocationDetailRepository;
        private readonly ICorporateTransactionDetailRepository _corporateTransactionDetailRepository;
        private readonly ISponsorRepository _sponsorRepository;

        public InventoryReportQueryHandler(
            IEventRepository eventRepository,
            IEventDetailRepository eventDetailRepository,
            IEventTicketDetailRepository eventTicketDetailRepository,
            IEventTicketAttributeRepository eventTicketAttributeRepository,
            ITransactionRepository transactionRepository,
            ITransactionDetailRepository transactionDetailRepository,
            IVenueRepository venueRepository,
            ICityRepository cityRepository,
            ITicketCategoryRepository ticketCategoryRepository,
            ICurrencyTypeRepository currencyTypeRepository,
            IReportingColumnsUserMappingRepository reportingColumnsUserMappingRepository,
            IReportingColumnsMenuMappingRepository reportingColumnsMenuMappingRepository,
            IReportingColumnRepository reportingColumnRepository,
            IEventAttributeRepository eventAttributeRepository,
            IEventsUserMappingRepository eventsUserMappingRepository,
            IUserRepository userRepository,
            ICorporateTicketAllocationDetailRepository corporateTicketAllocationDetailRepository,
            ICorporateTransactionDetailRepository corporateTransactionDetailRepository,
            ISponsorRepository sponsorRepository
        )
        {
            _eventRepository = eventRepository;
            _eventDetailRepository = eventDetailRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _transactionRepository = transactionRepository;
            _transactionDetailRepository = transactionDetailRepository;
            _venueRepository = venueRepository;
            _cityRepository = cityRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _currencyTypeRepository = currencyTypeRepository;
            _reportingColumnsUserMappingRepository = reportingColumnsUserMappingRepository;
            _reportingColumnsMenuMappingRepository = reportingColumnsMenuMappingRepository;
            _reportingColumnRepository = reportingColumnRepository;
            _eventAttributeRepository = eventAttributeRepository;
            _eventsUserMappingRepository = eventsUserMappingRepository;
            _userRepository = userRepository;
            _corporateTicketAllocationDetailRepository = corporateTicketAllocationDetailRepository;
            _corporateTransactionDetailRepository = corporateTransactionDetailRepository;
            _sponsorRepository = sponsorRepository;
        }

        public InventoryReportQueryResult Handle(InventoryReportQuery query)
        {
            List<FIL.Contracts.Models.TicketCategory> ticketCategories = new List<FIL.Contracts.Models.TicketCategory>();
            List<EventTicketAttribute> eventTicketAttributes = new List<EventTicketAttribute>();
            List<EventTicketDetail> eventTicketDetails = new List<EventTicketDetail>();
            List<CorporateTicketAllocationDetail> corporateTicketAllocationDetails = new List<CorporateTicketAllocationDetail>();
            List<CorporateTransactionDetail> corporateTransactionDetails = new List<CorporateTransactionDetail>();
            List<Sponsor> sponsors = new List<Sponsor>();
            List<FIL.Contracts.Models.Transaction> transactions = new List<FIL.Contracts.Models.Transaction>();
            List<FIL.Contracts.Models.TransactionDetail> transactionDetails = new List<FIL.Contracts.Models.TransactionDetail>();
            try
            {
                var events = _eventRepository.GetByAltId(query.EventAltId);
                var eventDetail = _eventDetailRepository.GetByAltId(query.EventDetailAltId);
                var eventTicketDetailList = AutoMapper.Mapper.Map<List<EventTicketDetail>>(_eventTicketDetailRepository.GetByEventDetailId(eventDetail.Id));

                for (int l = 0; l < eventTicketDetailList.Count; l = l + 2000)
                {
                    var eventTicketDetailBatcher = eventTicketDetailList.Skip(l).Take(2000);
                    var ticketCategoryList = _ticketCategoryRepository.GetByTicketCategoryIds(eventTicketDetailBatcher.Select(s => s.TicketCategoryId).Distinct());
                    var eventTicketAttributeList = AutoMapper.Mapper.Map<List<EventTicketAttribute>>(_eventTicketAttributeRepository.GetByEventTicketDetailIds(eventTicketDetailBatcher.Select(s => s.Id)));
                    var transactionDetailList = AutoMapper.Mapper.Map<List<TransactionDetail>>(_transactionDetailRepository.GetByEventTicketAttributeIds(eventTicketAttributeList.Select(s => s.Id)));
                    var corporateTicketAllocationList = AutoMapper.Mapper.Map<List<CorporateTicketAllocationDetail>>(_corporateTicketAllocationDetailRepository.GetByEventTicketAttributeIds(eventTicketAttributeList.Select(s => s.Id)));
                    var corporateTransactionList = AutoMapper.Mapper.Map<List<CorporateTransactionDetail>>(_corporateTransactionDetailRepository.GetByEventTicketAttributeIds(eventTicketAttributeList.Select(s => s.Id)));
                    var sponsorList = AutoMapper.Mapper.Map<List<Sponsor>>(_sponsorRepository.GetByIds(corporateTicketAllocationList.Select(s => s.SponsorId)));

                    for (int i = 0; i < transactionDetailList.Count; i = i + 2000)
                    {
                        var transactionDetailBatcher = transactionDetailList.Skip(i).Take(2000);
                        var transactionList = _transactionRepository.GetByTransactionIds(transactionDetailBatcher.Select(s => s.TransactionId).Distinct());
                        transactionDetailBatcher = transactionDetailBatcher.Where(w => transactionList.Select(s => s.Id).Contains(w.TransactionId)).Distinct();
                        transactions = transactions.Concat(AutoMapper.Mapper.Map<IEnumerable<FIL.Contracts.Models.Transaction>>(transactionList)).ToList();
                        transactionDetails = transactionDetails.Concat(AutoMapper.Mapper.Map<List<FIL.Contracts.Models.TransactionDetail>>(transactionDetailBatcher)).ToList();
                    }

                    ticketCategories = ticketCategories.Concat(AutoMapper.Mapper.Map<List<FIL.Contracts.Models.TicketCategory>>(ticketCategoryList)).ToList();
                    eventTicketAttributes = eventTicketAttributes.Concat(AutoMapper.Mapper.Map<List<FIL.Contracts.Models.EventTicketAttribute>>(eventTicketAttributeList)).OrderByDescending(s => s.Price).ToList();
                    eventTicketDetails = eventTicketDetails.Concat(AutoMapper.Mapper.Map<List<FIL.Contracts.Models.EventTicketDetail>>(eventTicketDetailBatcher)).ToList();
                    corporateTicketAllocationDetails = corporateTicketAllocationDetails.Concat(AutoMapper.Mapper.Map<List<FIL.Contracts.Models.CorporateTicketAllocationDetail>>(corporateTicketAllocationList)).ToList();
                    corporateTransactionDetails = corporateTransactionDetails.Concat(AutoMapper.Mapper.Map<List<FIL.Contracts.Models.CorporateTransactionDetail>>(corporateTransactionList)).ToList();
                    sponsors = sponsors.Concat(AutoMapper.Mapper.Map<List<FIL.Contracts.Models.Sponsor>>(sponsorList)).ToList();
                }

                var loggedUserDetails = _userRepository.GetByAltId(query.UserAltId);
                var reportingColumnDetailByUser = _reportingColumnsUserMappingRepository.GetByUserId(loggedUserDetails.Id);
                var reportingColumnNameDetails = _reportingColumnsMenuMappingRepository.GetByIdsAndMenuId(reportingColumnDetailByUser.Select(s => s.ColumnsMenuMappingId).Distinct(), 3);
                reportingColumnDetailByUser = reportingColumnDetailByUser.Where(W => reportingColumnNameDetails.Select(s => s.Id).Contains(W.ColumnsMenuMappingId)).ToList().OrderBy(o => o.SortOrder);
                var reportColumnDetails = _reportingColumnRepository.GetByIds(reportingColumnNameDetails.Select(s => s.ColumnId).Distinct());
                List<FIL.Contracts.Models.ReportingColumn> reportColumns = new List<FIL.Contracts.Models.ReportingColumn>();

                foreach (var item in reportingColumnDetailByUser)
                {
                    var reportingColumnNameDetail = reportingColumnNameDetails.Where(w => w.Id == item.ColumnsMenuMappingId).FirstOrDefault();
                    var reportColumnDetail = reportColumnDetails.Where(w => w.Id == reportingColumnNameDetail.ColumnId).FirstOrDefault();
                    reportColumns.Add(AutoMapper.Mapper.Map<FIL.Contracts.Models.ReportingColumn>(reportColumnDetail));
                }

                return new InventoryReportQueryResult
                {
                    TicketCategory = ticketCategories,
                    EventTicketAttribute = eventTicketAttributes,
                    EventTicketDetail = eventTicketDetails,
                    CorporateTicketAllocationDetail = corporateTicketAllocationDetails,
                    CorporateTransactionDetail = corporateTransactionDetails,
                    Sponsor = sponsors,
                    Transaction = transactions,
                    TransactionDetail = transactionDetails.OrderByDescending(o => o.TransactionId).ToList(),
                    ReportColumns = reportColumns
                };
            }
            catch (System.Exception ex)
            {
                Contracts.DataModels.User loggedUserDetails = _userRepository.GetByAltId(query.UserAltId);
                IEnumerable<Contracts.DataModels.ReportingColumnsUserMapping> reportingColumnDetailByUser = _reportingColumnsUserMappingRepository.GetByUserId(loggedUserDetails.Id);
                IEnumerable<Contracts.DataModels.ReportingColumnsMenuMapping> reportingColumnNameDetails = _reportingColumnsMenuMappingRepository.GetByIdsAndMenuId(reportingColumnDetailByUser.Select(s => s.ColumnsMenuMappingId).Distinct(), 1);
                IEnumerable<Contracts.DataModels.ReportingColumn> reportColumnDetails = _reportingColumnRepository.GetByIds(reportingColumnNameDetails.Select(s => s.ColumnId).Distinct());

                return new InventoryReportQueryResult
                {
                    TicketCategory = ticketCategories,
                    EventTicketAttribute = eventTicketAttributes,
                    EventTicketDetail = eventTicketDetails,
                    CorporateTicketAllocationDetail = corporateTicketAllocationDetails,
                    CorporateTransactionDetail = corporateTransactionDetails,
                    Sponsor = sponsors,
                    Transaction = transactions,
                    TransactionDetail = transactionDetails.OrderByDescending(o => o.TransactionId).ToList(),
                    ReportColumns = AutoMapper.Mapper.Map<IEnumerable<FIL.Contracts.Models.ReportingColumn>>(reportColumnDetails)
                };
            }
        }
    }
}