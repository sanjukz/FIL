using FIL.Api.Repositories;
using FIL.Contracts.Queries.CustomerDocumentType;
using FIL.Contracts.QueryResults;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.CustomerDocumentType
{
    public class CustomerDocumentTypeQueryHandler : IQueryHandler<CustomerDocumentTypeQuery, CustomerDocumentTypeQueryResult>
    {
        private readonly ICustomerDocumentTypeRepository _customerDocumentTypeRepository;
        private readonly FIL.Logging.ILogger _logger;

        public CustomerDocumentTypeQueryHandler(ICustomerDocumentTypeRepository customerDocumentTypeRepository, FIL.Logging.ILogger logger)
        {
            _customerDocumentTypeRepository = customerDocumentTypeRepository;
            _logger = logger;
        }

        public CustomerDocumentTypeQueryResult Handle(CustomerDocumentTypeQuery query)
        {
            var customerDocumentTypesDataModel = _customerDocumentTypeRepository.GetAll();
            var customerDocumentTypesModel = AutoMapper.Mapper.Map<List<Contracts.Models.CustomerDocumentType>>(customerDocumentTypesDataModel).ToList();

            return new CustomerDocumentTypeQueryResult
            {
                CustomerDocumentTypes = customerDocumentTypesModel
            };
        }
    }
}