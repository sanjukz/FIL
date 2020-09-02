using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IDiscountRepository : IOrmRepository<Discount, Discount>
    {
        Discount Get(int id);

        IEnumerable<Discount> GetAllDiscountsByIds(List<long> discountIds);
    }

    public class DiscountRepository : BaseOrmRepository<Discount>, IDiscountRepository
    {
        public DiscountRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public Discount Get(int id)
        {
            return Get(new Discount { Id = id });
        }

        public IEnumerable<Discount> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteDiscount(Discount discount)
        {
            Delete(discount);
        }

        public Discount SaveDiscount(Discount discount)
        {
            return Save(discount);
        }

        public IEnumerable<Discount> GetAllDiscountsByIds(List<long> discountIds)
        {
            return GetAll(s => s.Where($"{nameof(Discount.Id):C} IN @Ids")
                .WithParameters(new { Ids = discountIds })
            );
        }
    }
}