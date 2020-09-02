using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface ICitySightSeeingRouteDetailRepository : IOrmRepository<CitySightSeeingRouteDetail, CitySightSeeingRouteDetail>
    {
        CitySightSeeingRouteDetail Get(int id);

        CitySightSeeingRouteDetail GetByCitySightSeeingRouteId(long Id);

        CitySightSeeingRouteDetail GetByCitySightSeeingRouteIdAndLocationId(long routeId, string locationId);

        IEnumerable<CitySightSeeingRouteDetail> GetByCitySightSeeingRouteIds(IEnumerable<long> ids);
    }

    public class CitySightSeeingRouteDetailRepository : BaseLongOrmRepository<CitySightSeeingRouteDetail>, ICitySightSeeingRouteDetailRepository
    {
        public CitySightSeeingRouteDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public CitySightSeeingRouteDetail Get(int id)
        {
            return Get(new CitySightSeeingRouteDetail { Id = id });
        }

        public IEnumerable<CitySightSeeingRouteDetail> GetAll()
        {
            return GetAll(null);
        }

        public CitySightSeeingRouteDetail GetByCitySightSeeingRouteId(long Id)
        {
            return GetAll(s => s.Where($"{nameof(CitySightSeeingRouteDetail.CitySightSeeingRouteId):C} = @CitySightSeeingRouteId")
                .WithParameters(new { CitySightSeeingRouteId = Id })
            ).FirstOrDefault();
        }

        public CitySightSeeingRouteDetail GetByCitySightSeeingRouteIdAndLocationId(long routeId, string locationId)
        {
            return GetAll(s => s.Where($"{nameof(CitySightSeeingRouteDetail.CitySightSeeingRouteId):C} = @RouteId  AND {nameof(CitySightSeeingRouteDetail.RouteLocationId):C} = @LocationId AND IsEnabled = 1")
                .WithParameters(new { RouteId = routeId, LocationId = locationId })
            ).FirstOrDefault();
        }

        public IEnumerable<CitySightSeeingRouteDetail> GetByCitySightSeeingRouteIds(IEnumerable<long> ids)
        {
            var CitySightSeeingRouteDetailList = GetAll(statement => statement
                                 .Where($"{nameof(CitySightSeeingRouteDetail.CitySightSeeingRouteId):C} IN @CitySightSeeingRouteIds AND IsEnabled = 1")
                                 .WithParameters(new { CitySightSeeingRouteIds = ids }));
            return CitySightSeeingRouteDetailList;
        }
    }
}