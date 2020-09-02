using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface ICustomerInformationRepository : IOrmRepository<CustomerInformation, CustomerInformation>
    {
        CustomerInformation Get(long id);
    }

    public class CustomerInformationRepository : BaseLongOrmRepository<CustomerInformation>, ICustomerInformationRepository
    {
        public CustomerInformationRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public CustomerInformation Get(long id)
        {
            return Get(new CustomerInformation { Id = id });
        }

        public IEnumerable<CustomerInformation> GetAll()
        {
            return GetAll(null);
        }
    }
}