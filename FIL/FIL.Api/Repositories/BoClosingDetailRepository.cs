using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IBoClosingDetailRepository : IOrmRepository<BoClosingDetail, BoClosingDetail>
    {
        BoClosingDetail Get(long id);

        BoClosingDetail GetByUserIdDate(long userId, DateTime todyDate);

        IEnumerable<BoClosingDetail> GetByUserIds(IEnumerable<long> userIds);
    }

    public class BoClosingDetailRepository : BaseLongOrmRepository<BoClosingDetail>, IBoClosingDetailRepository
    {
        public BoClosingDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public BoClosingDetail Get(long id)
        {
            return Get(new BoClosingDetail { Id = id });
        }

        public IEnumerable<BoClosingDetail> GetAll()
        {
            return GetAll(null);
        }

        public BoClosingDetail GetByUserIdDate(long userId, DateTime todyDate)
        {
            return GetAll(s => s.Where($"{nameof(BoClosingDetail.UserId):C} = @UserId AND {nameof(BoClosingDetail.CreatedUtc):C} >= @TodayDate ")
            .WithParameters(new { UserId = userId, TodayDate = todyDate })
            ).FirstOrDefault();
        }

        public IEnumerable<BoClosingDetail> GetByUserIds(IEnumerable<long> userIds)
        {
            return GetAll(s => s.Where($"{nameof(BoClosingDetail.UserId):C} IN @UserIds")
                .WithParameters(new
                {
                    UserIds = userIds
                })
            );
        }
    }
}