using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IZoomUserRepository : IOrmRepository<ZoomUser, ZoomUser>
    {
        ZoomUser Get(long id);

        ZoomUser GetByAltId(Guid altId);

        ZoomUser GetByTransactionId(long transactionId);

        IEnumerable<ZoomUser> GetByHostUserIds(IEnumerable<int> hostUserIds);

        ZoomUser GetByHostUserId(int hostUserId);

        IEnumerable<ZoomUser> GetAllByEventId(long EventId);
    }

    public class ZoomUserRepository : BaseLongOrmRepository<ZoomUser>, IZoomUserRepository
    {
        public ZoomUserRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public ZoomUser Get(long id)
        {
            return Get(new ZoomUser { Id = id });
        }

        public ZoomUser GetByAltId(Guid altId)
        {
            return GetAll(statement => statement
                    .Where($"{nameof(ZoomUser.AltId):C} = @AltId")
                    .WithParameters(new { AltId = altId })).FirstOrDefault();
        }

        public ZoomUser GetByTransactionId(long transactionId)
        {
            return GetAll(statement => statement
                    .Where($"{nameof(ZoomUser.TransactionId):C} = @TransactionId")
                    .WithParameters(new { TransactionId = transactionId })).FirstOrDefault();
        }

        public IEnumerable<ZoomUser> GetByHostUserIds(IEnumerable<int> hostUserIds)
        {
            return GetAll(s => s.Where($"{nameof(ZoomUser.EventHostUserId):C} IN @EventHostUserId")
               .WithParameters(new { EventHostUserId = hostUserIds }));
        }

        public IEnumerable<ZoomUser> GetAllByEventId(long EventId)
        {
            return GetAll(s => s.Where($"{nameof(ZoomUser.EventId):C} = @Id")
               .WithParameters(new { Id = EventId }));
        }

        public ZoomUser GetByHostUserId(int hostUserId)
        {
            return GetAll(s => s.Where($"{nameof(ZoomUser.EventHostUserId):C} = @EventHostUserId")
               .WithParameters(new { EventHostUserId = hostUserId })).FirstOrDefault();
        }
    }
}