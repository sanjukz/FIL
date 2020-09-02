using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IDiscountPaymentOptionRepository : IOrmRepository<DiscountPaymentOption, DiscountPaymentOption>
    {
        DiscountPaymentOption Get(int id);
    }

    public class DiscountPaymentOptionRepository : BaseOrmRepository<DiscountPaymentOption>, IDiscountPaymentOptionRepository
    {
        public DiscountPaymentOptionRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public DiscountPaymentOption Get(int id)
        {
            return Get(new DiscountPaymentOption { Id = id });
        }

        public IEnumerable<DiscountPaymentOption> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteDiscountPaymentOption(DiscountPaymentOption discountPaymentOption)
        {
            Delete(discountPaymentOption);
        }

        public DiscountPaymentOption SaveDiscountPaymentOption(DiscountPaymentOption discountPaymentOption)
        {
            return Save(discountPaymentOption);
        }
    }
}