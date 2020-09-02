using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface ILiveEventDetailRepository : IOrmRepository<LiveEventDetail, LiveEventDetail>
    {
        LiveEventDetail Get(int id);

        LiveEventDetail GetByEventId(long Id);

        IEnumerable<LiveEventDetail> GetAllByEventIds(List<long> Ids);
    }

    public class LiveEventDetailRepository : BaseOrmRepository<LiveEventDetail>, ILiveEventDetailRepository
    {
        public LiveEventDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public LiveEventDetail Get(int id)
        {
            return Get(new LiveEventDetail { Id = id });
        }

        public IEnumerable<LiveEventDetail> GetAll()
        {
            return GetAll(null);
        }

        public LiveEventDetail GetByEventId(long Id)
        {
            return GetAll(s => s.Where($"{nameof(LiveEventDetail.EventId):C} = @EventId")
          .WithParameters(new { EventId = Id })).FirstOrDefault();
        }

        public IEnumerable<LiveEventDetail> GetAllByEventIds(List<long> Ids)
        {
            return GetAll(s => s.Where($"{nameof(LiveEventDetail.EventId):C} IN @EventId")
          .WithParameters(new { EventId = Ids }));
        }
    }
}