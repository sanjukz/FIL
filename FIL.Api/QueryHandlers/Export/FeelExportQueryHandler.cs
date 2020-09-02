using FIL.Api.Providers.Export;
using FIL.Contracts.Queries.Export;
using FIL.Contracts.QueryResults.Export;
using System.Linq;

namespace FIL.Api.QueryHandlers.Export
{
    public class FeelExportQueryHandler : IQueryHandler<FeelExportQuery, FeelExportQueryResult>
    {
        private readonly IFeelExportIAProvider _feelExportIAProvider;

        public FeelExportQueryHandler(IFeelExportIAProvider feelExportIAProvider)
        {
            _feelExportIAProvider = feelExportIAProvider;
        }

        public FeelExportQueryResult Handle(FeelExportQuery query)
        {
            return new FeelExportQueryResult
            {
                Results = _feelExportIAProvider.Get().ToList()
            };
        }
    }
}