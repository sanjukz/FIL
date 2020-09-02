using FluentValidation;
using FIL.Contracts.Models;
using FIL.Http;
using FIL.Http.Repositories;
using System.Threading.Tasks;

namespace FIL.Foundation.Repositories
{
    public interface IReportingUserRepository
    {
        Task<ReportingUserAdditionalDetail> GetByUserId(long userId);
    }

    public class ReportingUserRepository : BaseRepository<ReportingUserAdditionalDetail>, IReportingUserRepository
    {
        public ReportingUserRepository(IRestHelper restHelper)
            : base(restHelper)
        {
        }

        public ReportingUserRepository(IRestHelper restHelper, AbstractValidator<ReportingUserAdditionalDetail> validator)
            : base(restHelper, validator)
        {
        }

        public Task<ReportingUserAdditionalDetail> GetByUserId(long userId)
        {
            return GetResult($"api/reportinguser/{userId}");
        }
    }
}