using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IScheduleDetailRepository : IOrmRepository<ScheduleDetail, ScheduleDetail>
    {
        ScheduleDetail Get(long id);

        IEnumerable<ScheduleDetail> GetAllByEventScheduleIds(List<long> ScheduleIds);

        IEnumerable<ScheduleDetail> GetAllByEventScheduleId(long ScheduleId);
        IEnumerable<ScheduleDetail> GetAllByIds(List<long> Ids);
    }

    public class ScheduleDetailRepository : BaseLongOrmRepository<ScheduleDetail>, IScheduleDetailRepository
    {
        public ScheduleDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public ScheduleDetail Get(long id)
        {
            return Get(new ScheduleDetail { Id = id });
        }

        public IEnumerable<ScheduleDetail> GetAllByEventScheduleIds(List<long> ScheduleIds)
        {
            return GetAll(s => s.Where($"{nameof(ScheduleDetail.EventScheduleId):C} In @Id")
               .WithParameters(new { Id = ScheduleIds }));
        }

        public IEnumerable<ScheduleDetail> GetAllByEventScheduleId(long ScheduleId)
        {
            return GetAll(s => s.Where($"{nameof(ScheduleDetail.EventScheduleId):C} = @Id")
               .WithParameters(new { Id = ScheduleId }));
        }

        public IEnumerable<ScheduleDetail> GetAllByIds(List<long> Ids)
        {
            return GetAll(s => s.Where($"{nameof(ScheduleDetail.Id):C} IN @Id")
               .WithParameters(new { Id = Ids }));
        }
    }
}