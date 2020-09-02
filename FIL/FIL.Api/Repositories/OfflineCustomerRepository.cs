using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IOfflineCustomerRepository : IOrmRepository<OfflineCustomer, OfflineCustomer>
    {
        OfflineCustomer Get(long id);
    }

    public class OfflineCustomerRepository : BaseLongOrmRepository<OfflineCustomer>, IOfflineCustomerRepository
    {
        public OfflineCustomerRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public OfflineCustomer Get(long id)
        {
            return Get(new OfflineCustomer { Id = id });
        }

        public IEnumerable<OfflineCustomer> GetAll()
        {
            return GetAll(null);
        }
    }
}