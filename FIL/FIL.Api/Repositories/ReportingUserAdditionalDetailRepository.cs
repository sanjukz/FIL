using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IReportingUserAdditionalDetailRepository : IOrmRepository<ReportingUserAdditionalDetail, ReportingUserAdditionalDetail>
    {
        ReportingUserAdditionalDetail Get(int id);

        ReportingUserAdditionalDetail GetByUserId(long userId);
    }

    public class ReportingUserAdditionalDetailRepository : BaseOrmRepository<ReportingUserAdditionalDetail>, IReportingUserAdditionalDetailRepository
    {
        public ReportingUserAdditionalDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public ReportingUserAdditionalDetail Get(int id)
        {
            return Get(new ReportingUserAdditionalDetail { Id = id });
        }

        public IEnumerable<ReportingUserAdditionalDetail> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteReportingUserAdditionalDetail(ReportingUserAdditionalDetail reportingUserAdditionalDetail)
        {
            Delete(reportingUserAdditionalDetail);
        }

        public ReportingUserAdditionalDetail SaveReportingUserAdditionalDetail(ReportingUserAdditionalDetail reportingUserAdditionalDetail)
        {
            return Save(reportingUserAdditionalDetail);
        }

        public ReportingUserAdditionalDetail GetByUserId(long userId)
        {
            return GetAll(s => s.Where($"{nameof(ReportingUserAdditionalDetail.UserId):C} = @UserId")
                .WithParameters(new { UserId = userId })
            ).FirstOrDefault();
        }
    }
}