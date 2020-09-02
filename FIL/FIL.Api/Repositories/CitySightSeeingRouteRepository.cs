using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface ICitySightSeeingRouteRepository : IOrmRepository<CitySightSeeingRoute, CitySightSeeingRoute>
    {
        CitySightSeeingRoute Get(int id);

        CitySightSeeingRoute GetByEventDetailIdAndRouteId(long eventDetailId, string routeId);

        IEnumerable<CitySightSeeingRoute> GetByEventDetailId(long eventDetailId);
    }

    public class CitySightSeeingRouteRepository : BaseLongOrmRepository<CitySightSeeingRoute>, ICitySightSeeingRouteRepository
    {
        public CitySightSeeingRouteRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public CitySightSeeingRoute Get(int id)
        {
            return Get(new CitySightSeeingRoute { Id = id });
        }

        public IEnumerable<CitySightSeeingRoute> GetAll()
        {
            return GetAll(null);
        }

        public CitySightSeeingRoute GetByEventDetailIdAndRouteId(long eventDetailId, string routeId)
        {
            return GetAll(s => s.Where($"{nameof(CitySightSeeingRoute.EventDetailId):C} = @EventDetailId  AND {nameof(CitySightSeeingRoute.RouteId):C} = @RouteId AND IsEnabled = 1")
                .WithParameters(new { EventDetailId = eventDetailId, RouteId = routeId })
            ).FirstOrDefault();
        }

        public IEnumerable<CitySightSeeingRoute> GetByEventDetailId(long eventDetailId)
        {
            var CitySightSeeingRouteList = GetAll(statement => statement
                .Where($"{nameof(CitySightSeeingRoute.EventDetailId):C} = @Id")
                .WithParameters(new { Id = eventDetailId }));
            return CitySightSeeingRouteList;
        }
    }
}