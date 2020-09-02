using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IPlaceVisitDurationRepository : IOrmRepository<PlaceVisitDuration, PlaceVisitDuration>
    {
        PlaceVisitDuration Get(long id);

        PlaceVisitDuration GetBySingleEventId(long id);

        IEnumerable<PlaceVisitDuration> GetByEventId(long ids);

        IEnumerable<PlaceVisitDuration> GetByEventIds(List<long> ids);
    }

    public class PlaceVisitDurationRepository : BaseLongOrmRepository<PlaceVisitDuration>, IPlaceVisitDurationRepository
    {
        public PlaceVisitDurationRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public PlaceVisitDuration Get(long id)
        {
            return Get(new PlaceVisitDuration { Id = id });
        }

        public PlaceVisitDuration GetBySingleEventId(long ids)
        {
            var placeVisitDurationList = GetAll(statement => statement
               .Where($"{nameof(PlaceVisitDuration.EventId):C} = @Ids")
               .WithParameters(new { Ids = ids }));
            return placeVisitDurationList.FirstOrDefault();
        }

        public IEnumerable<PlaceVisitDuration> GetByEventId(long ids)
        {
            var placeVisitDurationList = GetAll(statement => statement
               .Where($"{nameof(PlaceVisitDuration.EventId):C} = @Ids")
               .WithParameters(new { Ids = ids }));
            return placeVisitDurationList;
        }

        public IEnumerable<PlaceVisitDuration> GetByEventIds(List<long> ids)
        {
            var placeVisitDurationList = GetAll(statement => statement
               .Where($"{nameof(PlaceVisitDuration.EventId):C} IN @Ids")
               .WithParameters(new { Ids = ids }));
            return placeVisitDurationList;
        }

        public IEnumerable<PlaceVisitDuration> GetAll()
        {
            return GetAll(null);
        }
    }
}