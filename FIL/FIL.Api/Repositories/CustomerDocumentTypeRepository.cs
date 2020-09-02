using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;

namespace FIL.Api.Repositories
{
    public interface ICustomerDocumentTypeRepository : IOrmRepository<CustomerDocumentType, CustomerDocumentType>
    {
        CustomerDocumentType Get(long id);
    }

    public class CustomerDocumentTypeRepository : BaseLongOrmRepository<CustomerDocumentType>, ICustomerDocumentTypeRepository
    {
        public CustomerDocumentTypeRepository(IDataSettings dataSettings)
            : base(dataSettings)
        {
        }

        public CustomerDocumentType Get(long id)
        {
            return Get(new CustomerDocumentType { Id = id });
        }
    }
}