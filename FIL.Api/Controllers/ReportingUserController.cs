using AutoMapper;
using FIL.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FIL.Api.Controllers
{
    public class ReportingUserController : Controller
    {
        private readonly IReportingUserAdditionalDetailRepository _reportingUserRepository;

        public ReportingUserController(IReportingUserAdditionalDetailRepository reportingUserRepository)
        {
            _reportingUserRepository = reportingUserRepository;
        }

        [HttpGet]
        [Route("api/reportinguser/{userId}")]
        public Contracts.Models.ReportingUserAdditionalDetail Get(long userId)
        {
            var reportingUser = _reportingUserRepository.GetByUserId(userId);
            return reportingUser == null ? null : Mapper.Map<Contracts.Models.ReportingUserAdditionalDetail>(reportingUser);
        }
    }
}