using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IDiscountPromoCodeRepository : IOrmRepository<DiscountPromoCode, DiscountPromoCode>
    {
        DiscountPromoCode Get(int id);

        DiscountPromoCode GetByDiscountId(long discountId);

        IEnumerable<DiscountPromoCode> GetDiscountPromoCodes(long discountId);

        IEnumerable<DiscountPromoCode> GetAllDiscountIds(List<long> discountIds);
    }

    public class DiscountPromoCodeRepository : BaseOrmRepository<DiscountPromoCode>, IDiscountPromoCodeRepository
    {
        public DiscountPromoCodeRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public DiscountPromoCode Get(int id)
        {
            return Get(new DiscountPromoCode { Id = id });
        }

        public IEnumerable<DiscountPromoCode> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteDiscountPromoCode(DiscountPromoCode discountPromoCode)
        {
            Delete(discountPromoCode);
        }

        public DiscountPromoCode SaveDiscountPromoCode(DiscountPromoCode discountPromoCode)
        {
            return Save(discountPromoCode);
        }

        public DiscountPromoCode GetByDiscountId(long discountId)
        {
            return GetAll(s => s.Where($"{nameof(DiscountPromoCode.DiscountId):C} = @discount AND IsEnabled=1")
                .WithParameters(new { discount = discountId })
            ).FirstOrDefault();
        }

        public IEnumerable<DiscountPromoCode> GetDiscountPromoCodes(long discountId)
        {
            return GetAll(s => s.Where($"{nameof(DiscountPromoCode.DiscountId):C} = @discount AND IsEnabled=1")
                .WithParameters(new { discount = discountId })
            );
        }

        public IEnumerable<DiscountPromoCode> GetAllDiscountIds(List<long> discountIds)
        {
            return GetAll(s => s.Where($"{nameof(DiscountPromoCode.DiscountId):C} IN @discount")
                .WithParameters(new { discount = discountIds })
            );
        }
    }
}