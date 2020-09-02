using FIL.Api.Repositories;
using FIL.Contracts.Queries.TMS;
using FIL.Contracts.QueryResults.TMS;
using System;
using System.Collections.Generic;

namespace FIL.Api.QueryHandlers.TMS
{
    public class CompanyNameQueryHandler : IQueryHandler<CompanyNameQuery, CompanyNameQueryResult>
    {
        private readonly ISponsorRepository _sponsorRepository;
        private readonly FIL.Logging.ILogger _logger;

        public CompanyNameQueryHandler(ISponsorRepository sponsorRepository, FIL.Logging.ILogger logger)
        {
            _sponsorRepository = sponsorRepository;
            _logger = logger;
        }

        public CompanyNameQueryResult Handle(CompanyNameQuery query)
        {
            try
            {
                var sponsers = _sponsorRepository.GetAllByDateSponsors(DateTime.UtcNow.AddMonths(-6));

                return new CompanyNameQueryResult
                {
                    Sponsors = AutoMapper.Mapper.Map<List<Contracts.Models.Sponsor>>(sponsers)
                };
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                return new CompanyNameQueryResult
                {
                    Sponsors = null
                };
            }
        }
    }
}