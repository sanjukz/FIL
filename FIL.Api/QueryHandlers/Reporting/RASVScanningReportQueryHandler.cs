using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.Reporting;
using FIL.Contracts.QueryResults.Reporting;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.Reporting
{
    public class RASVScanningReportQueryHandler : IQueryHandler<RASVScanningReportQuery, RASVScanningReportQueryResult>
    {
        private readonly IReportingRepository _reportingRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionDetailRepository _transactionDetailRepository;
        private readonly IReportingColumnsUserMappingRepository _reportingColumnsUserMappingRepository;
        private readonly IReportingColumnsMenuMappingRepository _reportingColumnsMenuMappingRepository;
        private readonly IReportingColumnRepository _reportingColumnRepository;
        private readonly IEventAttributeRepository _eventAttributeRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;

        public RASVScanningReportQueryHandler(
            IReportingRepository reportingRepository,
            IEventRepository eventRepository,
            IEventDetailRepository eventDetailRepository,
            IEventTicketDetailRepository eventTicketDetailRepository,
            IEventTicketAttributeRepository eventTicketAttributeRepository,
            ITransactionRepository transactionRepository,
            ITransactionDetailRepository transactionDetailRepository,
            IReportingColumnsUserMappingRepository reportingColumnsUserMappingRepository,
            IReportingColumnsMenuMappingRepository reportingColumnsMenuMappingRepository,
            IReportingColumnRepository reportingColumnRepository,
            IEventAttributeRepository eventAttributeRepository,
            IUserRepository userRepository,
            ITicketCategoryRepository ticketCategoryRepository
        )
        {
            _reportingRepository = reportingRepository;
            _eventRepository = eventRepository;
            _eventDetailRepository = eventDetailRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _transactionRepository = transactionRepository;
            _transactionDetailRepository = transactionDetailRepository;
            _reportingColumnsUserMappingRepository = reportingColumnsUserMappingRepository;
            _reportingColumnsMenuMappingRepository = reportingColumnsMenuMappingRepository;
            _reportingColumnRepository = reportingColumnRepository;
            _eventAttributeRepository = eventAttributeRepository;
            _userRepository = userRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
        }

        public RASVScanningReportQueryResult Handle(RASVScanningReportQuery query)
        {
            FIL.Contracts.DataModels.Reporting reporting = new FIL.Contracts.DataModels.Reporting();
            List<FIL.Contracts.Models.Transaction> transactions = new List<FIL.Contracts.Models.Transaction>();
            List<FIL.Contracts.Models.TransactionDetail> transactionDetails = new List<FIL.Contracts.Models.TransactionDetail>();
            List<EventTicketAttribute> eventTicketAttributes = new List<EventTicketAttribute>();
            List<EventTicketDetail> eventTicketDetails = new List<EventTicketDetail>();
            List<FIL.Contracts.Models.EventDetail> eventDetails = new List<FIL.Contracts.Models.EventDetail>();
            List<FIL.Contracts.Models.Event> events = new List<FIL.Contracts.Models.Event>();
            List<FIL.Contracts.Models.EventAttribute> eventAttributes = new List<FIL.Contracts.Models.EventAttribute>();
            List<FIL.Contracts.Models.TicketCategory> ticketCategories = new List<FIL.Contracts.Models.TicketCategory>();
            List<FIL.Contracts.Models.ScanningDetailModel> matchSeatTicketDetails = new List<FIL.Contracts.Models.ScanningDetailModel>();
            List<FIL.Contracts.Models.ReportingColumn> reportColumns = new List<FIL.Contracts.Models.ReportingColumn>();
            try
            {
                reporting = _reportingRepository.GetRASVScanningReportData(query);
                transactions = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.Transaction>>(reporting.Transaction);
                transactionDetails = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.TransactionDetail>>(reporting.TransactionDetail);
                matchSeatTicketDetails = AutoMapper.Mapper.Map<List<ScanningDetailModel>>(reporting.MatchSeatTicketDetail);
                //eventTicketAttributes = AutoMapper.Mapper.Map<List<EventTicketAttribute>>(reporting.EventTicketAttribute);
                eventTicketDetails = AutoMapper.Mapper.Map<List<EventTicketDetail>>(reporting.EventTicketDetail);
                eventDetails = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.EventDetail>>(reporting.EventDetail);
                events = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.Event>>(reporting.Event);
                eventAttributes = AutoMapper.Mapper.Map<List<EventAttribute>>(reporting.EventAttribute);
                ticketCategories = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.TicketCategory>>(reporting.TicketCategory);
                var reportColumnDetails = AutoMapper.Mapper.Map<List<ReportingColumn>>(reporting.ReportColumns);

                var reportingColumnDetailByUser = AutoMapper.Mapper.Map<List<FIL.Contracts.DataModels.ReportingColumnsUserMapping>>(reporting.ReportingColumnsUserMapping);
                var reportingColumnNameDetails = AutoMapper.Mapper.Map<List<FIL.Contracts.DataModels.ReportingColumnsMenuMapping>>(reporting.ReportingColumnsMenuMapping);
                reportingColumnDetailByUser = reportingColumnDetailByUser.Where(W => reportingColumnNameDetails.Select(s => s.Id).Contains(W.ColumnsMenuMappingId)).ToList().OrderBy(o => o.SortOrder).ToList();

                foreach (var item in reportingColumnDetailByUser)
                {
                    var reportingColumnNameDetail = reportingColumnNameDetails.Where(w => w.Id == item.ColumnsMenuMappingId).FirstOrDefault();
                    var reportColumnDetail = reportColumnDetails.Where(w => w.Id == reportingColumnNameDetail.ColumnId).FirstOrDefault();
                    reportColumns.Add(AutoMapper.Mapper.Map<FIL.Contracts.Models.ReportingColumn>(reportColumnDetail));
                }

                return new RASVScanningReportQueryResult
                {
                    Transaction = transactions,
                    TransactionDetail = transactionDetails.OrderByDescending(o => o.TransactionId).ToList(),
                    MatchSeatTicketDetail = matchSeatTicketDetails,
                    EventTicketDetail = eventTicketDetails,
                    EventDetail = AutoMapper.Mapper.Map<IEnumerable<EventDetail>>(eventDetails),
                    EventAttribute = AutoMapper.Mapper.Map<IEnumerable<EventAttribute>>(eventAttributes),
                    Event = AutoMapper.Mapper.Map<List<Event>>(events),
                    TicketCategory = AutoMapper.Mapper.Map<IEnumerable<FIL.Contracts.Models.TicketCategory>>(ticketCategories),
                    ReportColumns = AutoMapper.Mapper.Map<IEnumerable<FIL.Contracts.Models.ReportingColumn>>(reportColumns)
                };
            }
            catch (System.Exception ex)
            {
                var loggedUserDetails = _userRepository.GetByAltId(query.UserAltId);
                var reportingColumnDetailByUser = _reportingColumnsUserMappingRepository.GetByUserId(loggedUserDetails.Id);
                var reportingColumnNameDetails = _reportingColumnsMenuMappingRepository.GetByIdsAndMenuId(reportingColumnDetailByUser.Select(s => s.ColumnsMenuMappingId).Distinct(), 1);
                var reportColumnDetails = _reportingColumnRepository.GetByIds(reportingColumnNameDetails.Select(s => s.ColumnId).Distinct());

                return new RASVScanningReportQueryResult
                {
                    Transaction = transactions,
                    TransactionDetail = transactionDetails,
                    MatchSeatTicketDetail = matchSeatTicketDetails,
                    EventTicketDetail = eventTicketDetails,
                    EventDetail = AutoMapper.Mapper.Map<IEnumerable<EventDetail>>(eventDetails),
                    EventAttribute = eventAttributes,
                    Event = AutoMapper.Mapper.Map<List<Event>>(events),
                    TicketCategory = AutoMapper.Mapper.Map<IEnumerable<FIL.Contracts.Models.TicketCategory>>(ticketCategories),
                    ReportColumns = AutoMapper.Mapper.Map<IEnumerable<FIL.Contracts.Models.ReportingColumn>>(reportColumns)
                };
            }
        }
    }
}