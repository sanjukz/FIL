using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IReportingColumnsMenuMappingRepository : IOrmRepository<ReportingColumnsMenuMapping, ReportingColumnsMenuMapping>
    {
        ReportingColumnsMenuMapping Get(long id);

        IEnumerable<ReportingColumnsMenuMapping> GetByIdsAndMenuId(IEnumerable<long> ids, int menuId);

        ReportingColumnsMenuMapping GetByColumnIdAndMenuId(long columnId, int menuId);
    }

    public class ReportingColumnsMenuMappingRepository : BaseLongOrmRepository<ReportingColumnsMenuMapping>, IReportingColumnsMenuMappingRepository
    {
        public ReportingColumnsMenuMappingRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public ReportingColumnsMenuMapping Get(long id)
        {
            return Get(new ReportingColumnsMenuMapping { Id = id });
        }

        public IEnumerable<ReportingColumnsMenuMapping> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteReportingColumnMenuMapping(ReportingColumnsMenuMapping reportingColumnsMenuMapping)
        {
            Delete(reportingColumnsMenuMapping);
        }

        public ReportingColumnsMenuMapping SaveReportingColumnMenuMapping(ReportingColumnsMenuMapping reportingColumnsMenuMapping)
        {
            return Save(reportingColumnsMenuMapping);
        }

        public IEnumerable<ReportingColumnsMenuMapping> GetByIdsAndMenuId(IEnumerable<long> ids, int menuId)
        {
            return GetAll(statement => statement
              .Where($"{nameof(ReportingColumnsMenuMapping.Id):C} IN @Ids AND {nameof(ReportingColumnsMenuMapping.MenuId):C} = @MenuId")
              .WithParameters(new { Ids = ids, MenuId = menuId }));
        }

        public ReportingColumnsMenuMapping GetByColumnIdAndMenuId(long columnId, int menuId)
        {
            return GetAll(statement => statement
              .Where($"{nameof(ReportingColumnsMenuMapping.ColumnId):C} = @ColumnId AND {nameof(ReportingColumnsMenuMapping.MenuId):C} = @MenuId")
              .WithParameters(new { ColumnId = columnId, MenuId = menuId })).FirstOrDefault();
        }
    }
}