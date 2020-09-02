using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IDayTimeMappingsRepository : IOrmRepository<DayTimeMappings, DayTimeMappings>
    {
        DayTimeMappings Get(long id);

        IEnumerable<DayTimeMappings> GetAllByPlaceWeekOpenDay(long id);

        IEnumerable<DayTimeMappings> GetAllByPlaceWeekOpenDays(List<long> id);

        DayTimeMappings GetIdandTime(long id, string startTime);
    }

    public class DayTimeMappingsRepository : BaseLongOrmRepository<DayTimeMappings>, IDayTimeMappingsRepository
    {
        public DayTimeMappingsRepository(IDataSettings dataSettings)
            : base(dataSettings)
        {
        }

        public DayTimeMappings Get(long id)
        {
            return Get(new DayTimeMappings { Id = id });
        }

        public IEnumerable<DayTimeMappings> GetAllByPlaceWeekOpenDay(long id)
        {
            var placeWeekOpen = GetAll(statement => statement
                .Where($"{nameof(DayTimeMappings.PlaceWeekOpenDayId):C} = @Ids")
                .WithParameters(new { Ids = id }));
            return placeWeekOpen;
        }

        public IEnumerable<DayTimeMappings> GetAllByPlaceWeekOpenDays(List<long> id)
        {
            var placeWeekOpen = GetAll(statement => statement
                .Where($"{nameof(DayTimeMappings.PlaceWeekOpenDayId):C} IN @Ids")
                .WithParameters(new { Ids = id }));
            return placeWeekOpen;
        }

        public DayTimeMappings GetIdandTime(long id, string startTime)
        {
            return GetAll(s => s.Where($"{nameof(DayTimeMappings.Id):C} = @ID And {nameof(DayTimeMappings.StartTime):c}=@StartTime")
                .WithParameters(new { ID = id, StartTime = startTime })
            ).FirstOrDefault();
        }
    }
}