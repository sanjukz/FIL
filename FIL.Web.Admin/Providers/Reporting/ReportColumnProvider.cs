using FIL.Contracts.Queries.Reporting;
using FIL.Contracts.QueryResults.Reporting;
using FIL.Foundation.Senders;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace FIL.Web.Providers.Reporting
{
    public interface IReportColumnProvider
    {
        Task<ReportColumnQueryResult> Get(ReportColumnQuery query);
    }

    public class ReportColumnProvider : IReportColumnProvider
    {
        private readonly IQuerySender _querySender;
        private readonly IMemoryCache _memoryCache;
        public ReportColumnProvider(IQuerySender querySender,
            IMemoryCache memoryCache)
        {
            _querySender = querySender;
            _memoryCache = memoryCache;
        }

        public async Task<ReportColumnQueryResult> Get(ReportColumnQuery query)
        {
            string reportName = query.ReportId  == 1 ? "transaction" : query.ReportId  == 2 ? "failedtransaction" : query.ReportId  == 3 ? "inventory" : query.ReportId  == 3 ? "scanning" : "";
            if (!_memoryCache.TryGetValue($"reportcolumn_{reportName}", out ReportColumnQueryResult queryResult))
            {
                queryResult = await _querySender.Send(query);                
                _memoryCache.Set($"reportcolumn_{reportName}", queryResult, DateTime.Now.AddMinutes(10));
            }
            return queryResult.ReportColumns != null ? queryResult : null;
        }
    }
}
