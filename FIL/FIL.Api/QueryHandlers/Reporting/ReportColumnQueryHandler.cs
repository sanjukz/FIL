using FIL.Api.Repositories;
using FIL.Contracts.Queries.Reporting;
using FIL.Contracts.QueryResults.Reporting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.Reporting
{
    public class ReportColumnQueryHandler : IQueryHandler<ReportColumnQuery, ReportColumnQueryResult>
    {
        private readonly IReportingColumnsUserMappingRepository _reportingColumnsUserMappingRepository;
        private readonly IReportingColumnsMenuMappingRepository _reportingColumnsMenuMappingRepository;
        private readonly IReportingColumnRepository _reportingColumnRepository;
        private readonly IUserRepository _userRepository;

        public ReportColumnQueryHandler(
            IReportingColumnsUserMappingRepository reportingColumnsUserMappingRepository,
            IReportingColumnsMenuMappingRepository reportingColumnsMenuMappingRepository,
            IReportingColumnRepository reportingColumnRepository,
            IUserRepository userRepository
        )
        {
            _reportingColumnsUserMappingRepository = reportingColumnsUserMappingRepository;
            _reportingColumnsMenuMappingRepository = reportingColumnsMenuMappingRepository;
            _reportingColumnRepository = reportingColumnRepository;
            _userRepository = userRepository;
        }

        public ReportColumnQueryResult Handle(ReportColumnQuery query)
        {
            try
            {
                var loggedUserDetails = _userRepository.GetByAltId(query.UserAltId);
                var reportingColumnDetailByUser = _reportingColumnsUserMappingRepository.GetByUserId(loggedUserDetails.Id);
                var reportingColumnNameDetails = _reportingColumnsMenuMappingRepository.GetByIdsAndMenuId(reportingColumnDetailByUser.Select(s => s.ColumnsMenuMappingId).Distinct(), query.ReportId);
                reportingColumnDetailByUser = reportingColumnDetailByUser.Where(W => reportingColumnNameDetails.Select(s => s.Id).Contains(W.ColumnsMenuMappingId)).ToList().OrderBy(o => o.SortOrder);
                var reportColumnDetails = _reportingColumnRepository.GetByIds(reportingColumnNameDetails.Select(s => s.ColumnId).Distinct());
                List<FIL.Contracts.Models.ReportingColumn> reportColumns = new List<FIL.Contracts.Models.ReportingColumn>();

                foreach (var item in reportingColumnDetailByUser)
                {
                    var reportingColumnNameDetail = reportingColumnNameDetails.Where(w => w.Id == item.ColumnsMenuMappingId).FirstOrDefault();
                    var reportColumnDetail = reportColumnDetails.Where(w => w.Id == reportingColumnNameDetail.ColumnId).FirstOrDefault();
                    reportColumns.Add(AutoMapper.Mapper.Map<FIL.Contracts.Models.ReportingColumn>(reportColumnDetail));
                }

                return new ReportColumnQueryResult
                {
                    ReportColumns = reportColumns
                };
            }
            catch (System.Exception ex)
            {
                return new ReportColumnQueryResult
                {
                    ReportColumns = null
                };
            }
        }
    }
}