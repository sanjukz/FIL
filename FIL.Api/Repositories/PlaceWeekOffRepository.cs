using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IPlaceWeekOffRepository : IOrmRepository<PlaceWeekOff, PlaceWeekOff>
    {
        PlaceWeekOff Get(long id);

        IEnumerable<PlaceWeekOff> GetAllByEventId(long ids);

        IEnumerable<PlaceWeekOff> GetAllByEventIds(List<long> ids);
    }

    public class PlaceWeekOffRepository : BaseLongOrmRepository<PlaceWeekOff>, IPlaceWeekOffRepository
    {
        public PlaceWeekOffRepository(IDataSettings dataSettings)
            : base(dataSettings)
        {
        }

        public PlaceWeekOff Get(long id)
        {
            return Get(new PlaceWeekOff { Id = id });
        }

        public IEnumerable<PlaceWeekOff> GetAllByEventId(long ids)
        {
            var placeWeekOffList = GetAll(statement => statement
                .Where($"{nameof(PlaceWeekOff.EventId):C} = @Ids")
                .WithParameters(new { Ids = ids }));
            return placeWeekOffList;
        }

        public IEnumerable<PlaceWeekOff> GetAllByEventIds(List<long> ids)
        {
            var placeWeekOffList = GetAll(statement => statement
                .Where($"{nameof(PlaceWeekOff.EventId):C} IN @Ids")
                .WithParameters(new { Ids = ids }));
            return placeWeekOffList;
        }
    }
}