using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IReportingColumnsUserMappingRepository : IOrmRepository<ReportingColumnsUserMapping, ReportingColumnsUserMapping>
    {
        ReportingColumnsUserMapping Get(long id);

        IEnumerable<ReportingColumnsUserMapping> GetByUserId(long userId);

        ReportingColumnsUserMapping GetByUserIdandColumnsMenuMappingId(long userId, long columnsMenuMappingId);
    }

    public class ReportingColumnsUserMappingRepository : BaseLongOrmRepository<ReportingColumnsUserMapping>, IReportingColumnsUserMappingRepository
    {
        public ReportingColumnsUserMappingRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public ReportingColumnsUserMapping Get(long id)
        {
            return Get(new ReportingColumnsUserMapping { Id = id });
        }

        public IEnumerable<ReportingColumnsUserMapping> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteReportingColumnUserMapping(ReportingColumnsUserMapping reportingColumnsUserMapping)
        {
            Delete(reportingColumnsUserMapping);
        }

        public ReportingColumnsUserMapping SaveReportingColumnUserMapping(ReportingColumnsUserMapping reportingColumnsUserMapping)
        {
            return Save(reportingColumnsUserMapping);
        }

        public IEnumerable<ReportingColumnsUserMapping> GetByUserId(long userId)
        {
            return GetAll(statement => statement
               .Where($"{nameof(ReportingColumnsUserMapping.UserId):C} = @UserId And IsEnabled=1")
               .WithParameters(new { UserId = userId }));
        }

        public ReportingColumnsUserMapping GetByUserIdandColumnsMenuMappingId(long userId, long columnsMenuMappingId)
        {
            return GetAll(statement => statement
               .Where($"{nameof(ReportingColumnsUserMapping.UserId):C} = @UserId AND {nameof(ReportingColumnsUserMapping.ColumnsMenuMappingId):C} = @ColumnsMenuMappingId")
              .WithParameters(new { UserId = userId, ColumnsMenuMappingId = columnsMenuMappingId })).FirstOrDefault();
        }
    }
}