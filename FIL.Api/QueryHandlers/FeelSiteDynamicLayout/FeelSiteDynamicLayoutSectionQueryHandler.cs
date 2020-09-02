using FIL.Api.Repositories;
using FIL.Contracts.Queries;
using FIL.Contracts.QueryResults;
using System;

namespace FIL.Api.QueryHandlers
{
    public class FeelSiteDynamicLayoutSectionQueryHandler : IQueryHandler<FeelSiteDynamicLayoutSectionQuery, FeelSiteDynamicLayoutSectionQueryResult>
    {
        private readonly IFeelDynamicLayoutSectionRepository _feelDynamicLayoutSectionRepository;
        private readonly IFeelDynamicLayoutPageRepository _feelDynamicLayoutPageRepository;
        private readonly Logging.ILogger _logger;

        public FeelSiteDynamicLayoutSectionQueryHandler(IFeelDynamicLayoutSectionRepository feelDynamicLayoutSectionRepository, IFeelDynamicLayoutPageRepository feelDynamicLayoutPageRepository, Logging.ILogger logger)
        {
            _feelDynamicLayoutSectionRepository = feelDynamicLayoutSectionRepository;
            _feelDynamicLayoutPageRepository = feelDynamicLayoutPageRepository;
            _logger = logger;
        }

        public FeelSiteDynamicLayoutSectionQueryResult Handle(FeelSiteDynamicLayoutSectionQuery query)
        {
            try
            {
                var pageData = _feelDynamicLayoutPageRepository.Get(query.PageId);
                var sectionData = _feelDynamicLayoutSectionRepository.GetAllSectionsByPageId(query.PageId);

                if (sectionData == null)
                {
                    throw new ArgumentNullException($"Unable to get sections for page {pageData.PageName}");
                }
                else
                {
                    return new FeelSiteDynamicLayoutSectionQueryResult
                    {
                        PageName = pageData.PageName,
                        Sections = sectionData
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                return new FeelSiteDynamicLayoutSectionQueryResult();
            }
        }
    }
}