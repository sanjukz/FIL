using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FIL.Foundation.Senders;
using FIL.Web.Core.Providers;
using FIL.Web.Feel.Providers;
using Microsoft.Extensions.Caching.Memory;
using FIL.Contracts.Queries.CustomerDocumentType;
using FIL.Web.Feel.ViewModels.DocumentTypes;

namespace FIL.Web.Feel.Controllers
{
    public class GuestDetailController : Controller
    {
        private readonly IQuerySender _querySender;
        private readonly ISearchProvider _searchProvider;
        private readonly IMemoryCache _memoryCache;
        private readonly ISiteIdProvider _siteIdProvider;

        public GuestDetailController(IQuerySender querySender, IMemoryCache memoryCache, ISearchProvider searchProvider, ISiteIdProvider siteIdProvider)
        {
            _querySender = querySender;
            _memoryCache = memoryCache;
            _searchProvider = searchProvider;
            _siteIdProvider = siteIdProvider;
        }

        [HttpGet]
        [Route("api/get/DocumentTypes")]
        public async Task<DocumentTypeResponseViewModel> GetDocumentTypes()
        {
            try
            {
                var queryResult = await _querySender.Send(new CustomerDocumentTypeQuery { });
                return new DocumentTypeResponseViewModel
                {
                    DocumentTypes = queryResult.CustomerDocumentTypes
                };
            }
            catch (Exception e)
            {
                return new DocumentTypeResponseViewModel { };
            }
        }
    }
}
