using FIL.Api.Repositories;
using FIL.Contracts.Queries.CustomerUpdate;
using FIL.Contracts.QueryResults.CustomerUpdate;
using System;
using System.Collections.Generic;

namespace FIL.Api.QueryHandlers.CustomerUpdate
{
    public class CustomerUpdateQueryHandler : IQueryHandler<CustomerUpdateQuery, CustomerUpdateQueryResult>
    {
        private readonly ICustomerUpdateRepository _customerUpdateRepository;
        private readonly FIL.Logging.ILogger _logger;

        public CustomerUpdateQueryHandler(ICustomerUpdateRepository customerUpdateRepository, FIL.Logging.ILogger logger
)
        {
            _customerUpdateRepository = customerUpdateRepository;
            _logger = logger;
        }

        public CustomerUpdateQueryResult Handle(CustomerUpdateQuery query)
        {
            List<FIL.Contracts.Models.CustomerUpdate> customerUpdates = new List<FIL.Contracts.Models.CustomerUpdate>();
            var customerUpdate = _customerUpdateRepository.GetBySiteId(query.SiteId);
            try
            {
                foreach (var item in customerUpdate)
                {
                    customerUpdates.Add(new FIL.Contracts.Models.CustomerUpdate
                    {
                        Description = item.Description
                    });
                }
                return new CustomerUpdateQueryResult
                {
                    CustomerUpdates = customerUpdates
                };
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                return new CustomerUpdateQueryResult
                {
                    CustomerUpdates = null
                };
            }
        }
    }
}