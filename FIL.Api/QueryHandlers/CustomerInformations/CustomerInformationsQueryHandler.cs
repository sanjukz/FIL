using FIL.Api.Repositories;
using FIL.Contracts.Queries.CustomerInformations;
using FIL.Contracts.QueryResults.CustomerInformations;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.CustomerInformations
{
    public class CustomerInformationsQueryHandler : IQueryHandler<CustomerInformationsQuery, CustomerInformationsQueryResult>
    {
        private readonly ICustomerInformationRepository _customerInformationRepository;

        public CustomerInformationsQueryHandler(ICustomerInformationRepository customerInformationRepository)
        {
            _customerInformationRepository = customerInformationRepository;
        }

        public CustomerInformationsQueryResult Handle(CustomerInformationsQuery query)
        {
            var cunstomerInformation = _customerInformationRepository.GetAll();
            var cunstomerInformationModel = AutoMapper.Mapper.Map<List<Contracts.Models.CustomerInformation>>(cunstomerInformation).ToList();

            return new CustomerInformationsQueryResult
            {
                CustomerInformations = cunstomerInformationModel
            };
        }
    }
}