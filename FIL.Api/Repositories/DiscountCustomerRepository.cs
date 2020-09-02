using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IDiscountCustomerRepository : IOrmRepository<DiscountCustomer, DiscountCustomer>
    {
        DiscountCustomer Get(int id);
    }

    public class DiscountCustomerRepository : BaseOrmRepository<DiscountCustomer>, IDiscountCustomerRepository
    {
        public DiscountCustomerRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public DiscountCustomer Get(int id)
        {
            return Get(new DiscountCustomer { Id = id });
        }

        public IEnumerable<DiscountCustomer> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteDiscountCustomer(DiscountCustomer discountCustomer)
        {
            Delete(discountCustomer);
        }

        public DiscountCustomer SaveDiscountCustomer(DiscountCustomer discountCustomer)
        {
            return Save(discountCustomer);
        }
    }
}