using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IBoOpeningDetailRepository : IOrmRepository<BoOpeningDetail, BoOpeningDetail>
    {
        BoOpeningDetail Get(long id);

        BoOpeningDetail GetByUserIdDate(long userId, DateTime todyDate);
    }

    public class BoOpeningDetailRepository : BaseLongOrmRepository<BoOpeningDetail>, IBoOpeningDetailRepository
    {
        public BoOpeningDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public BoOpeningDetail Get(long id)
        {
            return Get(new BoOpeningDetail { Id = id });
        }

        public IEnumerable<BoOpeningDetail> GetAll()
        {
            return GetAll(null);
        }

        public BoOpeningDetail GetByUserIdDate(long userId, DateTime todyDate)
        {
            return GetAll(s => s.Where($"{nameof(BoOpeningDetail.UserId):C} = @UserId AND {nameof(BoOpeningDetail.CreatedUtc):C} >= @TodayDate ")
            .WithParameters(new { UserId = userId, TodayDate = todyDate })
            ).FirstOrDefault();
        }
    }
}