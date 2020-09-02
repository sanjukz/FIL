using Microsoft.AspNetCore.Mvc;
using FIL.Foundation.Senders;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using FIL.Web.Feel.ViewModels.CustomerUpdate;
using FIL.Contracts.Queries.CustomerUpdate;
using Microsoft.Extensions.Caching.Memory;
using FIL.Contracts.Models;
using FIL.Web.Core.Providers;

namespace FIL.Web.Feel.Controllers
{
    public class CustomerUpdateController : Controller
    {
        private readonly IQuerySender _querySender;
        private readonly IMemoryCache _memoryCache;
        private readonly ISiteIdProvider _siteIdProvider;

        public CustomerUpdateController(IQuerySender querySender,
            IMemoryCache memoryCache,
            ISiteIdProvider siteIdProvider)
        {                        
            _querySender = querySender;
            _memoryCache = memoryCache;
            _siteIdProvider = siteIdProvider;
        }

        [HttpGet]
        [Route("api/customerupdate")]
        public async Task<CustomerUpdateResponseDataViewModel> Get()
        {
            var siteId = _siteIdProvider.GetSiteId();

            if (!_memoryCache.TryGetValue($"customer_updates_{(int)siteId}", out List<CustomerUpdate> customerUpdates))
            {
                var queryResult = await _querySender.Send(new CustomerUpdateQuery
                {
                    SiteId = siteId
                });

                customerUpdates = queryResult.CustomerUpdates;
                _memoryCache.Set($"customer_updates_{(int)siteId}", customerUpdates, DateTime.Now.AddMinutes(15));
            }

            return new CustomerUpdateResponseDataViewModel
            {
                CustomerUpdates = customerUpdates
            };
        }
    }
}