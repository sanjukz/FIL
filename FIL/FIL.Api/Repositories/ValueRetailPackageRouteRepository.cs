using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IValueRetailPackageRouteRepository : IOrmRepository<ValueRetailPackageRoute, ValueRetailPackageRoute>
    {
        ValueRetailPackageRoute Get(int id);

        IEnumerable<ValueRetailPackageRoute> GetByPackageId(int packageId);

        ValueRetailPackageRoute GetOneByPackageId(int packageId);
    }

    public class ValueRetailPackageRouteRepository : BaseOrmRepository<ValueRetailPackageRoute>, IValueRetailPackageRouteRepository
    {
        public ValueRetailPackageRouteRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public ValueRetailPackageRoute Get(int id)
        {
            return Get(new ValueRetailPackageRoute { Id = id });
        }

        public IEnumerable<ValueRetailPackageRoute> GetByPackageId(int packageId)
        {
            return GetAll(s => s.Where($"{nameof(ValueRetailPackageRoute.PackageId):C} = @PackageIdParam").WithParameters(new { PackageIdParam = packageId }));
        }

        public ValueRetailPackageRoute GetOneByPackageId(int packageId)
        {
            return GetAll(s => s.Where($"{nameof(ValueRetailPackageRoute.PackageId):C} = @PackageId")
                .WithParameters(new { PackageId = packageId })
            ).FirstOrDefault();
        }

        /*public ValueRetailPackageRoute GetByRouteIdStopIdLinkedRouteId(int routeId, int stopId, int linkedRouteId)
        {
            return GetAll(s => s.Where($"{nameof(ValueRetailRoute.RouteId):C} = @RouteIdParam AND {nameof(ValueRetailRoute.StopId):C} = @StopIdParam AND {nameof(ValueRetailRoute.LinkedRouteId):C} = @LinkedRouteIdParam").WithParameters(new { RouteIdParam = routeId, StopIdParam = stopId, LinkedRouteIdParam = linkedRouteId })).FirstOrDefault();
        } */

        /*public ValueRetailRoute GetForShoppingCart(string departureLocation, string departureTime, string returnTime)
        {
            return GetAll(s => s.Where($"{nameof(ValueRetailRoute.LocationAddress):C} = @LocationAddressParam AND {nameof(ValueRetailRoute.DepartureTime):C} = @DepartureTimeParam AND {nameof(ValueRetailRoute.ReturnTime):C} = @ReturnTimeParam").WithParameters(new { LocationAddressParam = departureLocation, DepartureTimeParam = departureTime, ReturnTimeParam = returnTime })).FirstOrDefault();
        } */

        public IEnumerable<ValueRetailPackageRoute> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteValueRetailPrimaryRoute(ValueRetailPackageRoute valueRetailPackageRoute)
        {
            Delete(valueRetailPackageRoute);
        }

        public ValueRetailPackageRoute SaveValueRetailPrimaryRoute(ValueRetailPackageRoute valueRetailPackageRoute)
        {
            return Save(valueRetailPackageRoute);
        }
    }
}