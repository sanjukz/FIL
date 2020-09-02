using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IReplayDetailRepository : IOrmRepository<ReplayDetail, ReplayDetail>
    {
        ReplayDetail Get(long id);

        IEnumerable<ReplayDetail> GetAllByEventId(long EventId);
    }

    public class ReplayDetailRepository : BaseLongOrmRepository<ReplayDetail>, IReplayDetailRepository
    {
        public ReplayDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public ReplayDetail Get(long id)
        {
            return Get(new ReplayDetail { Id = id });
        }

        public IEnumerable<ReplayDetail> GetAllByEventId(long EventId)
        {
            return GetAll(s => s.Where($"{nameof(ReplayDetail.EventId):C} = @Id")
               .WithParameters(new { Id = EventId }));
        }
    }
}