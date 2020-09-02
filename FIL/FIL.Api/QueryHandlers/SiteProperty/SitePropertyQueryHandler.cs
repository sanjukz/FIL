using FIL.Api.Repositories;
using FIL.Contracts.Queries.CustomerUpdate;
using FIL.Contracts.QueryResults.CustomerUpdate;
using System;
using System.Collections.Generic;

namespace FIL.Api.QueryHandlers.SiteProperty
{
    public class SitePropertyQueryHandler : IQueryHandler<SitePropertyQuery, SitePropertyQueryResult>
    {
        private readonly ISitePropertyRepository _SitePropertyRepository;
        private readonly FIL.Logging.ILogger _logger;

        public SitePropertyQueryHandler(ISitePropertyRepository SitePropertyRepository, FIL.Logging.ILogger logger
)
        {
            _SitePropertyRepository = SitePropertyRepository;
            _logger = logger;
        }

        public SitePropertyQueryResult Handle(SitePropertyQuery query)
        {
            List<FIL.Contracts.Models.SiteProperty> SiteProperties = new List<FIL.Contracts.Models.SiteProperty>();
            var SiteProperty = _SitePropertyRepository.GetBySiteId(query.SiteId);
            try
            {
                foreach (var item in SiteProperty)
                {
                    SiteProperties.Add(new FIL.Contracts.Models.SiteProperty
                    {
                        AltId = item.AltId,
                        Name = item.Name,
                        Title = item.Title,
                        Url = item.Url,
                        Description = item.Description,
                        GoogleSiteVerification = item.GoogleSiteVerification,
                        HrefLang = item.HrefLang,
                        Keyword = item.Keyword
                    });
                }
                return new SitePropertyQueryResult
                {
                    SiteProperties = SiteProperties
                };
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                return new SitePropertyQueryResult
                {
                    SiteProperties = null
                };
            }
        }
    }
}