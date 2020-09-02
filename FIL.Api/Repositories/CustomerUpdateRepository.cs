using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface ICustomerUpdateRepository : IOrmRepository<CustomerUpdate, CustomerUpdate>
    {
        CustomerUpdate Get(int id);

        IEnumerable<CustomerUpdate> GetBySiteId(Site siteId);
    }

    public class CustomerUpdateRepository : BaseOrmRepository<CustomerUpdate>, ICustomerUpdateRepository
    {
        public CustomerUpdateRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public CustomerUpdate Get(int id)
        {
            return Get(new CustomerUpdate { Id = id });
        }

        public IEnumerable<CustomerUpdate> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteCustomerUpdate(CustomerUpdate customerUpdate)
        {
            Delete(customerUpdate);
        }

        public CustomerUpdate SaveCustomerUpdate(CustomerUpdate customerUpdate)
        {
            return Save(customerUpdate);
        }

        public IEnumerable<CustomerUpdate> GetBySiteId(Site siteId)
        {
            var customerUpdateList = (GetAll(s => s.Where($"{nameof(CustomerUpdate.SiteId):C}=@SiteId AND IsEnabled = 1")
                                           .WithParameters(new { SiteId = siteId })
             ));
            return customerUpdateList;
        }
    }
}