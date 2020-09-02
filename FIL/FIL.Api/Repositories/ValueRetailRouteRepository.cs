using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IValueRetailRouteRepository : IOrmRepository<ValueRetailRoute, ValueRetailRoute>
    {
        ValueRetailRoute Get(int id);

        ValueRetailRoute GetByRouteIdStopIdLinkedRouteId(int routeId, int stopId, int linkedRouteId);

        ValueRetailRoute GetForShoppingCart(string departureLocation, string departureTime, string returnTime);

        IEnumerable<ValueRetailRoute> GetByVillageId(int villageId);
    }

    public class ValueRetailRouteRepository : BaseOrmRepository<ValueRetailRoute>, IValueRetailRouteRepository
    {
        public ValueRetailRouteRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public ValueRetailRoute Get(int id)
        {
            return Get(new ValueRetailRoute { Id = id });
        }

        public ValueRetailRoute GetByRouteIdStopIdLinkedRouteId(int routeId, int stopId, int linkedRouteId)
        {
            return GetAll(s => s.Where($"{nameof(ValueRetailRoute.RouteId):C} = @RouteIdParam AND {nameof(ValueRetailRoute.StopId):C} = @StopIdParam AND {nameof(ValueRetailRoute.LinkedRouteId):C} = @LinkedRouteIdParam").WithParameters(new { RouteIdParam = routeId, StopIdParam = stopId, LinkedRouteIdParam = linkedRouteId })).FirstOrDefault();
        }

        public ValueRetailRoute GetForShoppingCart(string departureLocation, string departureTime, string returnTime)
        {
            return GetAll(s => s.Where($"{nameof(ValueRetailRoute.LocationAddress):C} = @LocationAddressParam AND {nameof(ValueRetailRoute.DepartureTime):C} = @DepartureTimeParam AND {nameof(ValueRetailRoute.ReturnTime):C} = @ReturnTimeParam").WithParameters(new { LocationAddressParam = departureLocation, DepartureTimeParam = departureTime, ReturnTimeParam = returnTime })).FirstOrDefault();
        }

        public IEnumerable<ValueRetailRoute> GetByVillageId(int villageId)
        {
            return GetAll(s => s.Where($"{nameof(ValueRetailRoute.VillageId):C} = @VillageIdParam").WithParameters(new { VillageIdParam = villageId }));
        }

        public IEnumerable<ValueRetailRoute> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteValueRetailPrimaryRoute(ValueRetailRoute valueRetailRoute)
        {
            Delete(valueRetailRoute);
        }

        public ValueRetailRoute SaveValueRetailPrimaryRoute(ValueRetailRoute valueRetailRoute)
        {
            return Save(valueRetailRoute);
        }
    }
}