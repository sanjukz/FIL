using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IFloatDetailRepository : IOrmRepository<FloatDetail, FloatDetail>
    {
        FloatDetail Get(long id);

        FloatDetail GetByUserIdDate(long userId, DateTime todyDate);

        IEnumerable<FloatDetail> GetByUserId(long userId);

        IEnumerable<FloatDetail> GetByUserIds(IEnumerable<long> userIds);
    }

    public class FloatDetailRepository : BaseLongOrmRepository<FloatDetail>, IFloatDetailRepository
    {
        public FloatDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public FloatDetail Get(long id)
        {
            return Get(new FloatDetail { Id = id });
        }

        public IEnumerable<FloatDetail> GetAll()
        {
            return GetAll(null);
        }

        public FloatDetail GetByUserIdDate(long userId, DateTime todyDate)
        {
            return GetAll(s => s.Where($"{nameof(FloatDetail.UserId):C} = @UserId AND {nameof(FloatDetail.CreatedUtc):C} >= @TodayDate ")
            .WithParameters(new { UserId = userId, TodayDate = todyDate })
            ).FirstOrDefault();
        }

        public IEnumerable<FloatDetail> GetByUserId(long userId)
        {
            return GetAll(s => s.Where($"{nameof(FloatDetail.UserId):C} = @UserId")
            .WithParameters(new { UserId = userId }));
        }

        public IEnumerable<FloatDetail> GetByUserIds(IEnumerable<long> userIds)
        {
            return GetAll(s => s.Where($"{nameof(FloatDetail.UserId):C} IN @UserIds")
                .WithParameters(new
                {
                    UserIds = userIds
                })
            );
        }
    }
}