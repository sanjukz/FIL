using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IDaysRepository : IOrmRepository<Days, Days>
    {
        Days Get(long id);

        Days GetByDayname(string currentDay);
    }

    public class DaysRepository : BaseLongOrmRepository<Days>, IDaysRepository
    {
        public DaysRepository(IDataSettings dataSettings)
            : base(dataSettings)
        {
        }

        public Days Get(long id)
        {
            return Get(new Days { Id = id });
        }

        public Days GetByDayname(string currentDay)
        {
            var eventList = GetAll(statement => statement
                           .Where($"{nameof(Days.Name):C} = @day")
                           .WithParameters(new { day = currentDay }));
            return eventList.FirstOrDefault();
        }
    }
}