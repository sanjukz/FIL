using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IValueRetailPackageReturnRepository : IOrmRepository<ValueRetailPackageReturn, ValueRetailPackageReturn>
    {
        ValueRetailPackageReturn Get(int id);

        IEnumerable<ValueRetailPackageReturn> GetByValueRetailPackageRouteIdAndRouteId(int packageId, int routeId);
    }

    public class ValueRetailPackageReturnRepository : BaseOrmRepository<ValueRetailPackageReturn>, IValueRetailPackageReturnRepository
    {
        public ValueRetailPackageReturnRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public ValueRetailPackageReturn Get(int id)
        {
            return Get(new ValueRetailPackageReturn { Id = id });
        }

        public IEnumerable<ValueRetailPackageReturn> GetByValueRetailPackageRouteIdAndRouteId(int packageId, int routeId)
        {
            return GetAll(s => s.Where($"{nameof(ValueRetailPackageReturn.ValueRetailPackageRouteId):C} = @ValueRetailPackageRouteIdParam AND {nameof(ValueRetailPackageReturn.RouteId):C} = @RouteIdParam")
            .WithParameters(new { ValueRetailPackageRouteIdParam = packageId, RouteIdParam = routeId }));
        }

        /*
        public ValueRetailPackageReturn GetByStopId(int stopId)
        {
            return GetAll(s => s.Where($"{nameof(ValueRetailPackageReturn.StopId):C} = @stopIdParam").WithParameters(new { stopIdParam = stopId })).FirstOrDefault();
        }

        public IEnumerable<ValueRetailPackageReturn> GetByValueRetailRouteId(int valueRetailRouteId)
        {
            return GetAll(s => s.Where($"{nameof(ValueRetailPackageReturn.ValueRetailRouteId):C} = @ValueRetailRouteIdParam").WithParameters(new { ValueRetailRouteIdParam = valueRetailRouteId }));
        } */

        public IEnumerable<ValueRetailPackageReturn> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteValueRetailReturnStop(ValueRetailPackageReturn valueRetailPackageReturn)
        {
            Delete(valueRetailPackageReturn);
        }

        public ValueRetailPackageReturn SaveValueRetailPrimaryRoute(ValueRetailPackageReturn valueRetailPackageReturn)
        {
            return Save(valueRetailPackageReturn);
        }
    }
}