using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IReportingColumnRepository : IOrmRepository<ReportingColumn, ReportingColumn>
    {
        ReportingColumn Get(long id);

        IEnumerable<ReportingColumn> GetByIds(IEnumerable<long> ids);
    }

    public class ReportingColumnRepository : BaseOrmRepository<ReportingColumn>, IReportingColumnRepository
    {
        public ReportingColumnRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public ReportingColumn Get(long id)
        {
            return Get(new ReportingColumn { Id = id });
        }

        public IEnumerable<ReportingColumn> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteReportingColumn(ReportingColumn reportingColumn)
        {
            Delete(reportingColumn);
        }

        public ReportingColumn SaveReportingColumn(ReportingColumn reportingColumn)
        {
            return Save(reportingColumn);
        }

        public IEnumerable<ReportingColumn> GetByIds(IEnumerable<long> ids)
        {
            return GetAll(statement => statement
               .Where($"{nameof(ReportingColumn.Id):C} IN @Ids")
               .WithParameters(new { Ids = ids }));
        }
    }
}