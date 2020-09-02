using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface ITicketStockDetailRepository : IOrmRepository<TicketStockDetail, TicketStockDetail>
    {
        TicketStockDetail Get(long id);

        TicketStockDetail GetByUserIdDate(long userId, DateTime todyDate);

        IEnumerable<TicketStockDetail> GetByUserId(long userId);

        IEnumerable<TicketStockDetail> GetByUserIds(IEnumerable<long> userIds);
    }

    public class TicketStockDetailRepository : BaseLongOrmRepository<TicketStockDetail>, ITicketStockDetailRepository
    {
        public TicketStockDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public TicketStockDetail Get(long id)
        {
            return Get(new TicketStockDetail { Id = id });
        }

        public IEnumerable<TicketStockDetail> GetAll()
        {
            return GetAll(null);
        }

        public TicketStockDetail GetByUserIdDate(long userId, DateTime todyDate)
        {
            return GetAll(s => s.Where($"{nameof(TicketStockDetail.UserId):C} = @UserId AND {nameof(TicketStockDetail.CreatedUtc):C} >= @TodayDate ")
            .WithParameters(new { UserId = userId, TodayDate = todyDate })
            ).FirstOrDefault();
        }

        public IEnumerable<TicketStockDetail> GetByUserId(long userId)
        {
            return GetAll(s => s.Where($"{nameof(TicketStockDetail.UserId):C} = @UserId")
            .WithParameters(new { UserId = userId }));
        }

        public IEnumerable<TicketStockDetail> GetByUserIds(IEnumerable<long> userIds)
        {
            return GetAll(s => s.Where($"{nameof(TicketStockDetail.UserId):C} IN @UserIds")
                .WithParameters(new
                {
                    UserIds = userIds
                })
            );
        }
    }
}